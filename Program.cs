/**
 * [EN]    eSocial use case — single sign-evento endpoint, KMS custody.
 *         Run: dotnet run
 *         POST http://localhost:5097/api/esocial/sign-evento
 * [PT-BR] Caso de uso eSocial — endpoint único de assinatura, custódia KMS.
 *         Executar: dotnet run
 */
using SolidSign.Examples.UsecaseEsocial;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<EsocialSigningService>();
var app = builder.Build();

app.MapPost("/api/esocial/sign-evento", async (HttpRequest req, EsocialSigningService svc) =>
{
    var form = await req.ReadFormAsync();
    var document = form.Files.GetFile("document");
    var kmsCode = form["kmsCode"].ToString();
    if (document is null || string.IsNullOrWhiteSpace(kmsCode))
        return Results.BadRequest(new { error = "document and kmsCode are required" });

    await using var stream = document.OpenReadStream();
    var signed = await svc.SignAsync(stream, document.FileName, kmsCode);
    return signed is not null
        ? Results.File(signed, "application/xml", "evento_esocial_signed.xml")
        : Results.Problem("Signing failed. Check logs.");
});

app.Run("http://0.0.0.0:5097");
