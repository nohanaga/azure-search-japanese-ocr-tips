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

このカスタムスキルは Azure Cognitive Search 日本語OCRからの出力からスペースを除去します。

## Requirements

This skill has no additional requirements than the ones described in [the root `README.md` file](../../README.md).

## Settings

This function doesn't require any application settings.

## Deployment

[![Deploy to Azure](https://azuredeploy.net/deploybutton.svg)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fnohanaga%2Fazure-search-japanese-ocr-tips%2Fmaster%2Fazuredeploy.json)

## hello-world

### Sample Input:

```json
{
    "values": [
        {
            "recordId": "r1",
            "data":
            {
            	"name": "い れ た て の お 茶"
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
                "ocrtext": "いれたてのお茶"
            },
            "errors": [],
            "warnings": []
        }
    ]
}
```

## Sample Skillset Integration

In order to use this skill in a cognitive search pipeline, you'll need to add a skill definition to your skillset.
Here's a sample skill definition for this example (inputs and outputs should be updated to reflect your particular scenario and skillset environment):

```json
{
    "@odata.type": "#Microsoft.Skills.Custom.WebApiSkill",
    "description": "Hello world",
    "uri": "[AzureFunctionEndpointUrl]/api/hello-world?code=[AzureFunctionDefaultHostKey]",
    "batchSize": 1,
    "context": "/document/merged_content/people/*",
    "inputs": [
        {
            "name": "name",
            "source": "/document/merged_content/people/*"
        }
    ],
    "outputs": [
        {
            "name": "greeting",
            "targetName": "greeting"
        }
    ]
}
```
