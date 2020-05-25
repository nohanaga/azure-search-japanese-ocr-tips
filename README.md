---
page_type: sample
languages:
- csharp
products:
- azure
- azure-search
azureDeploy: https://raw.githubusercontent.com/nohanaga/azure-search-japanese-ocr-tips/master/azuredeploy.json
name: "Azure Cognitive Search 日本語OCR用スペース除去"
description: "このカスタムスキルは日本語OCRからの出力からスペースを除去します。"
---

# Azure Cognitive Search 日本語OCR用スペース除去カスタムスキル

このカスタムスキルは Azure Cognitive Search 日本語OCRからの出力から半角スペースを除去します。この時、正規表現を使って全角文字＋半角スペースとなっている場合のみ、半角スペースを削除するようにして、日本語外国語混じりの文章にも対応するようになっています。

## 必要条件

このスキルには、[README.mdファイル](README.md) で説明されているもの以外の要件はありません。

## 設定

この機能は、アプリケーション設定を必要としません。

## デプロイ

[![Deploy to Azure](https://azuredeploy.net/deploybutton.svg)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fnohanaga%2Fazure-search-japanese-ocr-tips%2Fmaster%2Fazuredeploy.json)

## remove-spaces

### Sample Input:

```json
{
    "values": [
        {
            "recordId": "r1",
            "data":
            {
            	"name": "Green tea is synonymous with Japanese tea. It is the most... い れ た て の お 茶"
            }
        }
    ]
}
```

### Sample Output:

```json
{
    "values": [
        {
            "recordId": "r1",
            "data": {
                "ocrtext": "Green tea is synonymous with Japanese tea. It is the most... いれたてのお茶"
            },
            "errors": [],
            "warnings": []
        }
    ]
}
```

## スキルセット統合の例

このスキルを Azure Cognitive Search パイプラインで使用するには、スキル定義をスキルセットに追加する必要があります。
この例のスキル定義の例を次に示します（特定のシナリオとスキルセット環境を反映するように入力と出力を更新する必要があります）。

```json
{
    "@odata.type": "#Microsoft.Skills.Custom.WebApiSkill",
    "description": "remove-spaces",
    "uri": "[AzureFunctionEndpointUrl]/api/remove-spaces?code=[AzureFunctionDefaultHostKey]",
    "batchSize": 1,
    "context": "/document",
    "inputs": [
        {
            "name": "name",
            "source": "/document/normalized_images/*/text"
        }
    ],
    "outputs": [
        {
            "name": "ocrtext",
            "targetName": "ocrtext"
        }
    ]
}
```
