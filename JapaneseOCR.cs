// Copyright (c) Microsoft. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AzureCognitiveSearch.PowerSkills.Common;
using System.Text.RegularExpressions;

namespace AzureCognitiveSearch.PowerSkills.Template.JapaneseOCR
{
    public static class JapaneseOCR
    {
        [FunctionName("remove-spaces")]
        public static async Task<IActionResult> RunJapaneseOCR(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext executionContext)
        {

            log.LogInformation("JapaneseOCR remove spaces Custom Skill: C# HTTP trigger function processed a request.");

            string skillName = executionContext.FunctionName;
            IEnumerable<WebApiRequestRecord> requestRecords = WebApiSkillHelpers.GetRequestRecords(req);
            if (requestRecords == null)
            {
                return new BadRequestObjectResult($"{skillName} - Invalid request record array.");
            }

            WebApiSkillResponse response = WebApiSkillHelpers.ProcessRequestRecords(skillName, requestRecords,
                (inRecord, outRecord) => {

                    var inputText = inRecord.Data["text"] as string;
                    outRecord.Data["ocrtext"] = "";

                    if (string.IsNullOrEmpty(inputText))
                    {
                        return outRecord;
                    }

                    // 正規表現で全角文字+半白スペースの場合、半角スペースのみを除去
                    var newContent = Regex.Replace(inputText, @"([^\x01-\x7E])\x20", "$1");
                    // 全角文字+ハイフン+半角スペースの場合、ハイフンを全角長音に変換し半角スペースを除去
                    newContent = Regex.Replace(newContent, @"([^\x01-\x7E])-\x20?", "$1ー");

                    log.LogInformation($"newContent:  {newContent}");

                    outRecord.Data["ocrtext"] = newContent;

                    return outRecord;
                });

            return new OkObjectResult(response);
        }
    }
}
