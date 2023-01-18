using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//using Azure.ResourceManager.Communication;
//using Azure.ResourceManager.Communication.Models;
//using Azure.Identity;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using System.Collections.Generic;
using System.Threading;

namespace emailHttpFunction
{
    public static class emailHttpFunction
    {
        [FunctionName("emailHttpFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            /* References
                Quickstart at https://learn.microsoft.com/en-us/azure/communication-services/quickstarts/create-communication-resource?tabs=windows&pivots=platform-net
                Docs at https://learn.microsoft.com/en-us/azure/communication-services/quickstarts/email/send-email?pivots=programming-language-csharp
            */
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                string toAddress = data?.toAddress;
                string toDisplayName = data?.toDisplayName;
                string fromAddress = Environment.GetEnvironmentVariable("DONOTREPLY_ADDRESS"); //"donotreply@yourdomain.com";
                string subject = data?.subject;
                string body = data?.body;

                string connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
                EmailClient emailClient = new EmailClient(connectionString);
                EmailContent emailContent = new EmailContent(subject);
                emailContent.PlainText = body;
                List<EmailAddress> emailAddresses = new List<EmailAddress>() { new EmailAddress(toAddress) };
                EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
                EmailMessage emailMessage = new EmailMessage(fromAddress, emailContent, emailRecipients);
                SendEmailResult emailResult = emailClient.Send(emailMessage, CancellationToken.None);

                return new OkObjectResult("Ok");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
