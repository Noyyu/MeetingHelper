namespace MeetingHelper.DTOs
{
    //The data from the meeting participants notes or voice recordnings. React will send this data to the backend for processing.
    public record TextPromptRequestDTO(string Content);

    //This is used to help the user generate an agenda for the meeting. 
    public record AgendaRequestDTO(string Topic, string Duration, string Attendees);

    //The package with the pretty invetation message to the meeting. 
    public record InvitationRequest(string MeetingTitle, string DateAndTime, string Location, string Purpose);

    //This is used so that we do not send war data in between things. Its not good practice. 
    public record AiResponse(string Result);
}
