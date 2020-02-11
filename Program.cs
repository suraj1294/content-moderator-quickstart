using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace content_moderator_quickstart
{
    class Program
    {
        // TEXT MODERATION
        // Name of the file that contains text
        private static readonly string TextFile = "TextFile.txt";
        // The name of the file to contain the output from the evaluation.
        private static string TextOutputFile = "TextModerationOutput.txt";
        // Your Content Moderator subscription key is found in your Azure portal resource on the 'Keys' page. Add to your environment variables.
        private static readonly string SubscriptionKey = Environment.GetEnvironmentVariable("CONTENT_MODERATOR_SUBSCRIPTION_KEY");
        // Base endpoint URL. Add this to your environment variables. Found on 'Overview' page in Azure resource. For example: https://westus.api.cognitive.microsoft.com
        private static readonly string Endpoint = Environment.GetEnvironmentVariable("CONTENT_MODERATOR_ENDPOINT");
        static void Main(string[] args)
        {

            ContentModeratorClient clientText = Authenticate("8b211180b3ce440f8836fdcee2fedf1c", "https://test-moderation.cognitiveservices.azure.com/");
            ModerateText(clientText, TextFile, TextOutputFile);



            Console.WriteLine(Environment.GetEnvironmentVariable("CONTENT_MODERATOR_SUBSCRIPTION_KEY"));
            Console.WriteLine("End of the quickstart.");
        }

        public static ContentModeratorClient Authenticate(string key, string endpoint)
        {
            ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(key));
            client.Endpoint = endpoint;

            return client;
        }

        /*
            * TEXT MODERATION
            * This example moderates text from file.
        */
        public static void ModerateText(ContentModeratorClient client, string inputFile, string outputFile)
        {
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("TEXT MODERATION");
            Console.WriteLine();
            // Load the input text.
            string text = File.ReadAllText(inputFile);

            // Remove carriage returns
            text = text.Replace(Environment.NewLine, " ");
            // Convert string to a byte[], then into a stream (for parameter in ScreenText()).
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream(textBytes);

            Console.WriteLine("Screening {0}...", inputFile);
            // Format text

            // Save the moderation results to a file.
            using (StreamWriter outputWriter = new StreamWriter(outputFile, false))
            {
                using (client)
                {
                    // Screen the input text: check for profanity, classify the text into three categories,
                    // do autocorrect text, and check for personally identifying information (PII)
                    outputWriter.WriteLine("Autocorrect typos, check for matching terms, PII, and classify.");

                    // Moderate the text
                    var screenResult = client.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);
                    outputWriter.WriteLine(JsonConvert.SerializeObject(screenResult, Formatting.Indented));
                }

                outputWriter.Flush();
                outputWriter.Close();
            }

            Console.WriteLine("Results written to {0}", outputFile);
            Console.WriteLine();
        }
    }
}
