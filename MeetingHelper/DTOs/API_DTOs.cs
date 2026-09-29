namespace MeetingHelper.DTOs
{
    //The data from the meeting participants notes or voice recordnings. Frontend > Backend
    public record TextPromptRequestDTO(string Content);

    //This is used to help the user generate an agenda for the meeting. Frontend > Backend
    public record AgendaRequestDTO(string Topic, string Duration, string Attendees);

    //The package with the pretty invetation message to the meeting. Frontend > Backend
    public record InvitationRequestDTO(string MeetingTitle, string DateAndTime, string Location, string Purpose);

    //This is used so that we do not send raw data in between things. Its not good practice. Backend > Frontend
    public record AiResponseDTO(string Result); 
}
