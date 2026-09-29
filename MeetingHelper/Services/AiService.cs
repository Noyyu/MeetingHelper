using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Text.Json;

namespace MeetingHelper.Services
{
    public class AiService : IAiService
    {
        // This is the bpy that makes the web request to the Gemini API. It will send the prompt and get a response back. He's a good boy. 
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _geminiModel;

        public AiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Gemini:ApiKey"] ?? throw new ArgumentNullException("Gemini:ApiKey can not be read."); // Will try to read the key from the user secrets. 
            _geminiModel = config["Gemini:Model"] ?? throw new ArgumentNullException("Gemini:Model can not find AI model.");
        }

        public async Task<string> GetSummeryFromText(string prompt)
        {
            var cleanText = prompt.Replace("\r\n", " ").Replace("\n", " "); //Removes ENTER and replaces them with BLANKSPACE (The ai dont like enters for some reason)
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{_geminiModel}:generateContent?key={_apiKey}";

            var requestBody = CreateRequestBody(
                new { text = $"Summerize the following text from the meeting: {prompt}" }
            );

            using var response = await _httpClient.PostAsJsonAsync(endpoint, requestBody); // Sends the request to the API and gets the response.

            return await GetResponse(response);
        }


        public async Task<string> GetSummeryFromAudio(IFormFile audioFile)
        {
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{_geminiModel}:generateContent?key={_apiKey}";

            using var memoryStream = new MemoryStream();
            await audioFile.CopyToAsync(memoryStream); // Copy the audio file to a memory stream.
            var base64Data = Convert.ToBase64String(memoryStream.ToArray()); // Converts the ausdio file to a format that can be written in JSON so it can be sent to the API! :D !!!!! >:(

            var requestBody = CreateRequestBody(
            new { text = "Summarize the following audio file from a meeting." },
            new { audio = base64Data }
            );

            using var response = await _httpClient.PostAsJsonAsync(endpoint, requestBody);
            return await GetResponse(response);
        }

        public async Task<string> CreateMeetingAgenda(string topic, string duration, string attendees)
        {
            var cleanTopic = topic.Replace("\r\n", " ").Replace("\n", " ");
            var cleanDuration = topic.Replace("\r\n", " ").Replace("\n", " ");
            var cleanAttendees = topic.Replace("\r\n", " ").Replace("\n", " ");

            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{_geminiModel}:generateContent?key={_apiKey}";

            var requestBody = CreateRequestBody(
                new { text = $"Create a meeting agenda and only answer based on the following format: title, the name of the attendees, duration, agenda. Title, based on topic: {cleanTopic}, Duration: {cleanDuration}, Attendees: {cleanAttendees}, and the agenda based on the topic" }
            );


            using var response = await _httpClient.PostAsJsonAsync(endpoint, requestBody); // Sends the request to the API and gets the response.
            return await GetResponse(response);
        }

        private async Task<string> GetResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var json = await response.Content.ReadAsStringAsync(); // Reads the content from the response as string and saves it into the variable (var json)

            using var doc = JsonDocument.Parse(json); // Parse the JSON response and extract the generated text.

            return doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? string.Empty;
        }
        private object CreateRequestBody(params object[] promptParts)
        {
            return new
            {
                contents = new[]
                {
                    new
                    {
                        parts = promptParts
                    }
                }
            };
        }
    }
}
