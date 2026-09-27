# 🇧🇷 SolidSign API - Caso de Uso: eSocial — C#

Este projeto demonstra a integração com a **SolidSign API** para assinar eventos do **eSocial** (S-1000, S-2200 e qualquer outro evento S-xxxx): uma assinatura XML única sobre o envelope inteiro, com o certificado e-CPF/e-CNPJ do empregador, via KMS SolidSign.

## Endpoint

`POST /api/esocial/sign-evento` — recebe o XML do evento (`document`) e o `kmsCode` do empregador, retorna o XML assinado.

## Parâmetros de assinatura (fixos, iguais pra qualquer evento)

| Parâmetro | Valor |
| :--- | :--- |
| `profile` | `XML_INTERNATIONAL` |
| `signaturePackaging` | `ENVELOPED` |
| `hashAlgorithm` | `SHA256` |
| `canonicalizationMethod` | `INCLUSIVE` |
| `isRemoveXPathExclusionFilter` | `true` |
| `isRemoveNamespacePrefixFromNodeNames` | `true` |

## Stack
1. .NET 8 (Minimal API)

## Como Executar

```bash
dotnet run
curl -X POST http://localhost:5097/api/esocial/sign-evento \
  -F "document=@evento-s1000.xml" -F "kmsCode=$KMS_EMPREGADOR" \
  -o evento-s1000-assinado.xml
```

## Outros métodos de certificação

Para HSM em nuvem ou navegador (PKCS#1), aplique os mesmos parâmetros aos exemplos genéricos [`exemplo-csharp-integracao-xml-cloud`](https://github.com/SolidTechSolutions/exemplo-csharp-integracao-xml-cloud) e [`exemplo-csharp-integracao-xml-pkcs1`](https://github.com/SolidTechSolutions/exemplo-csharp-integracao-xml-pkcs1).

## Tratamento de Erros
O sistema loga o JSON detalhado de erro da SolidSign.

---

# 🇬🇧 SolidSign API - Use Case: eSocial — C#

Covers signing **eSocial** events (S-1000, S-2200, any S-xxxx event): a single XML signature over the entire envelope, with the employer's e-CPF/e-CNPJ certificate, via SolidSign's KMS.

## Endpoint

`POST /api/esocial/sign-evento` — takes the event XML (`document`) and the employer's `kmsCode`, returns the signed XML.

## Signature parameters (fixed, same for any event)

| Parameter | Value |
| :--- | :--- |
| `profile` | `XML_INTERNATIONAL` |
| `signaturePackaging` | `ENVELOPED` |
| `hashAlgorithm` | `SHA256` |
| `canonicalizationMethod` | `INCLUSIVE` |
| `isRemoveXPathExclusionFilter` | `true` |
| `isRemoveNamespacePrefixFromNodeNames` | `true` |

## Stack
1. .NET 8 (Minimal API)

## How to Run

```bash
dotnet run
```

## Other certification methods

For cloud HSM or browser (PKCS#1) signing, apply the same parameters to [`exemplo-csharp-integracao-xml-cloud`](https://github.com/SolidTechSolutions/exemplo-csharp-integracao-xml-cloud) and [`exemplo-csharp-integracao-xml-pkcs1`](https://github.com/SolidTechSolutions/exemplo-csharp-integracao-xml-pkcs1).

## Error Handling
The system logs SolidSign's detailed error JSON.

---

# 🇪🇸 SolidSign API - Caso de Uso: eSocial — C#

Cubre la firma de eventos del **eSocial** (S-1000, S-2200, cualquier evento S-xxxx): una única firma XML sobre el sobre entero, con el certificado e-CPF/e-CNPJ del empleador, vía KMS.

## Endpoint

`POST /api/esocial/sign-evento` — recibe el XML del evento (`document`) y el `kmsCode` del empleador, devuelve el XML firmado.

## Stack
1. .NET 8 (Minimal API)

## Cómo Ejecutar

```bash
dotnet run
```

## Otros métodos de certificación

Para HSM en la nube o navegador (PKCS#1), aplique los mismos parámetros a [`exemplo-csharp-integracao-xml-cloud`](https://github.com/SolidTechSolutions/exemplo-csharp-integracao-xml-cloud) y [`exemplo-csharp-integracao-xml-pkcs1`](https://github.com/SolidTechSolutions/exemplo-csharp-integracao-xml-pkcs1).

## Gestión de Errores
El sistema registra el JSON detallado de errores de SolidSign.
