namespace MeetingHelper.Services
{
    public interface IAiService
    {
        //Responds with a pretty text based on the prompt. 
        Task<string> GenerateTextAsync(string prompt);

        //Responds with a summerized version of the audio file.
        Task<string> SummarizeAudioAsync(IFormFile audioFile);
    }
}
