export interface InvitationMessage {
    meetingTitle: string;
    dateAndTime: string;
    location: string;
    purpose: string;
}

export interface AgendaMessage{
    topic : string;
    duration: string;
    attendees: string;
}

export const getTextSummary = async (textToSummarize: string) => {
    const payload = { content: textToSummarize };

    const response = await fetch("https://localhost:7210/ai/summarizeText", { 
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
    });

    if (!response.ok) {
         const error = await response.text();
        throw new Error( error || "Could not create the summurization.");
    }

    const data = await response.json();
    return data.result; 
};

export const getAudioSummary = async (audioToSummarize: File) : Promise <string> => {
    const formData = new FormData();
    

    formData.append("file", audioToSummarize);

    const response = await fetch("https://localhost:7210/ai/summarizeAudio", {
        method: "POST",
        body: formData
    });

    if(!response.ok) {
        const error = await response.text();
        throw new Error(error || "Could not create the summurization.");
    }

    const data = await response.json();
    return data.result;
}

export const getAgenda = async (agendaMessage: AgendaMessage) : Promise <string> => {

    const response = await fetch("https://localhost:7210/ai/agenda", { 
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(agendaMessage)
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || "Could not create the agenda.");
    }

    const data = await response.json();
    return data.result; 
};

export const getInvitationMessage = async (invitationMessage: InvitationMessage) : Promise <string> => {

    const response = await fetch("https://localhost:7210/ai/invitation", { 
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(invitationMessage)
    });

    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || "Could not create the invitation message.");
    }

    const data = await response.json();
    return data.result; 
};