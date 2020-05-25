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

                    var name = inRecord.Data["name"] as string;

                    // ���K�\���ŕ����񒆂̋󔒂�S�폜
                    var newContent = Regex.Replace(name, @"\s", "");

                    outRecord.Data["ocrtext"] = newContent;
                    return outRecord;
                });

            return new OkObjectResult(response);
        }
    }
}