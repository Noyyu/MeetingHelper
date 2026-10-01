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
            string? fileName = null;
            _httpClient.Timeout = TimeSpan.FromMinutes(10);

            try
            {
                var initUrl = $"https://generativelanguage.googleapis.com/upload/v1beta/files?key={_apiKey}";
                using var initRequest = new HttpRequestMessage(HttpMethod.Post, initUrl);
                initRequest.Headers.Add("X-Goog-Upload-Protocol", "resumable");
                initRequest.Headers.Add("X-Goog-Upload-Command", "start");
                initRequest.Headers.Add("X-Goog-Upload-Header-Content-Length", audioFile.Length.ToString());
                initRequest.Headers.Add("X-Goog-Upload-Header-Content-Type", audioFile.ContentType);
                initRequest.Content = JsonContent.Create(new { file = new { display_name = audioFile.FileName } });

                using var initResponse = await _httpClient.SendAsync(initRequest);
                initResponse.EnsureSuccessStatusCode();

                var uploadUrl = initResponse.Headers.GetValues("x-goog-upload-url").First();

                using var fileStream = audioFile.OpenReadStream();
                using var uploadRequest = new HttpRequestMessage(HttpMethod.Post, uploadUrl);
                uploadRequest.Headers.Add("X-Goog-Upload-Offset", "0");
                uploadRequest.Headers.Add("X-Goog-Upload-Command", "upload, finalize");
                uploadRequest.Content = new StreamContent(fileStream);

                using var uploadResponse = await _httpClient.SendAsync(uploadRequest);
                uploadResponse.EnsureSuccessStatusCode();

                using var uploadResult = await JsonDocument.ParseAsync(await uploadResponse.Content.ReadAsStreamAsync());
                var fileElement = uploadResult.RootElement.GetProperty("file");
                var fileUri = fileElement.GetProperty("uri").GetString();
                fileName = fileElement.GetProperty("name").GetString();

                var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{_geminiModel}:generateContent?key={_apiKey}";

                var requestBody = CreateRequestBody(
                new { text = "This meeting might be in english or swedish. Please only answer with a summarization of the meeting and a list of important bulletpoints." },
                new { file_data = new { mime_type = audioFile.ContentType, file_uri = fileUri } }
                );


                //using var response = await _httpClient.PostAsJsonAsync(endpoint, requestBody);

                HttpResponseMessage? httpPesponse = null;
                for(int attempt = 1; attempt <= 3; attempt++)
                {
                    httpPesponse = await _httpClient.PostAsJsonAsync(endpoint, requestBody);

                    if (httpPesponse.StatusCode != System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        break;
                    }
                    if(attempt < 3 )
                    {
                        await Task.Delay(attempt * 2500);
                    }
                }
                return await GetResponse(httpPesponse);
            }
            finally
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    var deleteUrl = $"https://generativelanguage.googleapis.com/v1beta/{fileName}?key={_apiKey}";
                    await _httpClient.DeleteAsync(deleteUrl);
                }
            }
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

        public async Task<string> CreateInvitationMessage(string MeetingTitle, string DateAndTime, string Location, string Purpose)
        {
            var cleanMeetingTitle = MeetingTitle.Replace("\r\n", " ").Replace("\n", " ");
            var cleanDateAndTime = DateAndTime.Replace("\r\n", " ").Replace("\n", " ");
            var cleanLocation = Location.Replace("\r\n", " ").Replace("\n", " ");
            var cleanPurpose = Purpose.Replace("\r\n", " ").Replace("\n", " ");

            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{_geminiModel}:generateContent?key={_apiKey}";
            var requestBody = CreateRequestBody(
                new { text = $"Create and only respond with a meeting invitation message based on the following information: Meeting Title: {cleanMeetingTitle}, Date and Time: {cleanDateAndTime}, Location: {cleanLocation}, Purpose: {cleanPurpose}" }
            );

            using var response = await _httpClient.PostAsJsonAsync(endpoint, requestBody);
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
