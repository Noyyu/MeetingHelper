namespace MeetingHelper.Services
{
    public interface IAiService
    {
        //Responds with a pretty text based on the prompt. 
        Task<string> GetSummeryFromText(string prompt);

        //Responds with a summerized version of the audio file.
        Task<string> GetSummeryFromAudio(IFormFile audioFile);
        Task<string> CreateMeetingAgenda(string topic, string duration, string attendees);
        Task<string> CreateInvitationMessage(string MeetingTitle, string DateAndTime, string Location, string Purpose);
    }
}
