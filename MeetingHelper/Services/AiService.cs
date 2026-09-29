using System.Text;
using System.Text.Json;

namespace MeetingHelper.Services
{
    public class AiService : IAiService
    {
        // This is the bpy that makes the web request to the Gemini API. It will send the prompt and get a response back. He's a good boy. 
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Gemini:ApiKey"] ?? throw new ArgumentNullException("Gemini:ApiKey can not be read."); // Will try to read the key from the user secrets. 
        }

        public async Task<string> GenerateTextAsync(string prompt)
        {
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            var requestBody = new //The package that will be sent to the API.
            {
                contents = new[] // An array is needed here as the API "can" accept several messages. 
                {
                    new // A single message in the list. ^
                    { 
                        parts = new[] // One of the parts if the message. Add more if there are more than one file format. 
                        { 
                            new { text = prompt } // The actual content of the fucking prompt. 
                        } 
                    }
                } 
            };

           // Note: repsone and doc uses a variable type that does not free up memory automatically. Somehow im supposed to remember this shit. 

            using var response = await _httpClient.PostAsJsonAsync(endpoint, requestBody); // Sends the request to the API and gets the response.
            response.EnsureSuccessStatusCode(); // Safity check to make sure the request was successful.

            var json = await response.Content.ReadAsStringAsync(); // Reads the content from the response as string and saves it into the variable (var json)

            using var doc = JsonDocument.Parse(json); // Parse the JSON response and extract the generated text.

            return doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? string.Empty;
        }


        public async Task<string> SummarizeAudioAsync(IFormFile audioFile)
        {
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            using var memoryStream = new MemoryStream();
            await audioFile.CopyToAsync(memoryStream); // Copy the audio file to a memory stream.
            var base64Data = Convert.ToBase64String(memoryStream.ToArray()); // Converts the ausdio file to a format that can be written in JSON so it can be sent to the API! :D !!!!! >:(

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[] // Uses object so that it does not get angry with me for using two different data formats.(text and audio)
                        {
                            new { text = "Summarize the following audio file." },
                            new { audio = base64Data }
                        }
                    }
                }
            };

            using var response = await _httpClient.PostAsJsonAsync(endpoint, requestBody);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? string.Empty;
        }
    }
}
