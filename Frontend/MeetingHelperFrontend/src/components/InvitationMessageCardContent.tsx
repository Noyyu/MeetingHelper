import { useState } from "react";
import { getInvitationMessage, type InvitationMessage } from "../services/AiService";
import { useDispatch } from 'react-redux';
import { setResponse } from "../store/aiResponseSlice";

export default function InvitationMessageCardContent(){

    const [meetingTitle, setMeetingTitle] = useState("");
    const [dateAndTime, setDateAndTime] = useState("");
    const [location, setLocation] = useState("");
    const [purpose, setPurpose] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const dispatch = useDispatch();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!meetingTitle.trim() || !dateAndTime.trim() || !location.trim() || !purpose.trim()) {
            setError("Please fill in all fields.");
            return;
        }

        setLoading(true);
        setError("");

        const invitationMessage: InvitationMessage = {
            meetingTitle,
            dateAndTime,
            location,
            purpose
        };

        try {
            const response = await getInvitationMessage(invitationMessage);
            dispatch(setResponse(response));
        } catch (error) {
            setError( "Could not reach the server.");
            console.error(error);
        } finally {
            setLoading(false);
            setError("");
        }
    }

    return(
        <div>
            <h2>Enter meeting details</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    placeholder="Meeting Title"
                    value={meetingTitle}
                    onChange={(e) => setMeetingTitle(e.target.value)}
                />
                <input
                    type="text"
                    placeholder="Date and Time"
                    value={dateAndTime}
                    onChange={(e) => setDateAndTime(e.target.value)}
                />
                <input
                    type="text"
                    placeholder="Location"
                    value={location}
                    onChange={(e) => setLocation(e.target.value)}
                />
                <input
                    type="text"
                    placeholder="Purpose"
                    value={purpose}
                    onChange={(e) => setPurpose(e.target.value)}
                />
                <button type="submit" disabled={loading}>
                    {loading ? "Generating..." : "Generate Invitation Message"}
                </button>
            </form>
            {error && <p role="alert" style={{ color: "red" }}>{error}</p>}
        </div>
    )

}