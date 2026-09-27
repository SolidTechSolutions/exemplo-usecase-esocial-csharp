using System.Text.Json;

namespace SolidSign.Examples.UsecaseEsocial;

/// <summary>
/// [EN]    Signs an eSocial event XML (any S-xxxx event — S-1000, S-2200, etc.) with the
///         employer's e-CPF/e-CNPJ certificate, using a SolidSign KMS-custodied certificate
///         (POST /solidsign/dsig/xml/sign-kms). Always signs the ENTIRE document with the same
///         fixed parameters, regardless of event type.
/// [PT-BR] Assina o XML de um evento eSocial (qualquer evento S-xxxx) com o certificado
///         e-CPF/e-CNPJ do empregador, via KMS SolidSign. Sempre assina o DOCUMENTO INTEIRO
///         com os mesmos parâmetros fixos, independente do tipo de evento.
/// </summary>
public class EsocialSigningService(IConfiguration cfg, HttpClient http)
{
    private string BaseUrl => cfg["SolidSign:Api:BaseUrl"]!.TrimEnd('/');
    private string Auth => cfg["SolidSign:Api:Authorization"]!;
    private string Profile => cfg["SolidSign:Esocial:Profile"] ?? "XML_INTERNATIONAL";
    private string HashAlg => cfg["SolidSign:Esocial:HashAlgorithm"] ?? "SHA256";
    private string SigPackaging => cfg["SolidSign:Esocial:SignaturePackaging"] ?? "ENVELOPED";
    private string CanonMethod => cfg["SolidSign:Esocial:CanonicalizationMethod"] ?? "INCLUSIVE";
    private bool RemoveXPathFilter => bool.Parse(cfg["SolidSign:Esocial:IsRemoveXPathExclusionFilter"] ?? "true");
    private bool RemoveNsPrefix => bool.Parse(cfg["SolidSign:Esocial:IsRemoveNamespacePrefixFromNodeNames"] ?? "true");

    public async Task<byte[]?> SignAsync(Stream document, string fileName, string kmsCode)
    {
        using var form = new MultipartFormDataContent();
        form.Add(new StreamContent(document), "document[0]", fileName);
        form.Add(new StringContent(kmsCode), "kmsCode");
        form.Add(new StringContent(Profile), "profile");
        form.Add(new StringContent(HashAlg), "hashAlgorithm");
        form.Add(new StringContent(SigPackaging), "signaturePackaging");
        form.Add(new StringContent(CanonMethod), "canonicalizationMethod");
        form.Add(new StringContent(RemoveXPathFilter.ToString().ToLowerInvariant()), "isRemoveXPathExclusionFilter");
        form.Add(new StringContent(RemoveNsPrefix.ToString().ToLowerInvariant()), "isRemoveNamespacePrefixFromNodeNames");

        var req = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/solidsign/dsig/xml/sign-kms") { Content = form };
        req.Headers.TryAddWithoutValidation("Authorization", Auth);
        var resp = await http.SendAsync(req);
        if (!resp.IsSuccessStatusCode)
        {
            Console.Error.WriteLine($"SolidSign API error {(int)resp.StatusCode}: {await resp.Content.ReadAsStringAsync()}");
            return null;
        }

        var signResp = JsonSerializer.Deserialize<SignResponse>(await resp.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var href = signResp?.Documents.FirstOrDefault()?.HalLinks?.Self?.Href
                ?? signResp?.Documents.FirstOrDefault()?.Links?.FirstOrDefault(l => l.Rel == "self")?.Href;
        if (href is null)
        {
            Console.Error.WriteLine("SolidSign response had no download link.");
            return null;
        }

        var dlReq = new HttpRequestMessage(HttpMethod.Get, href);
        dlReq.Headers.TryAddWithoutValidation("Authorization", Auth);
        var dlResp = await http.SendAsync(dlReq);
        return dlResp.IsSuccessStatusCode ? await dlResp.Content.ReadAsByteArrayAsync() : null;
    }
}
