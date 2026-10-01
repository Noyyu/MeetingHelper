import { useState } from "react";
import { useDispatch } from "react-redux";
import { setResponse } from "../store/aiResponseSlice";
import { getAgenda } from "../services/AiService";
import { type AgendaMessage } from "../services/AiService";

export default function AgendaMessageCardContent(){

    const [attendees, setAttendees] = useState("");
    const [topic, setTopic] = useState("");
    const [duration, setDuration] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const dispatch = useDispatch();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        if (!attendees.trim() || !topic.trim() || !duration.trim()) {
            setError("Please fill in all fields.");
            return;
        }

        setLoading(true);
        setError("");

        const agendaMessage: AgendaMessage = {
            attendees,
            topic,
            duration
        };

        try {
            const response = await getAgenda(agendaMessage);
            dispatch(setResponse(response));
        } catch (error) {
            setError("Could not reach the server.");
            console.error(error);
        } finally {
            setLoading(false);
            setError("");
        }
    };
    return(
            <div>
                <h2>Enter meeting details</h2>
                <form onSubmit={handleSubmit}>
                    <input
                        type="text"
                        placeholder="Attendees"
                        value={attendees}
                        onChange={(e) => setAttendees(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="Topic"
                        value={topic}
                        onChange={(e) => setTopic(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="Duration"
                        value={duration}
                        onChange={(e) => setDuration(e.target.value)}
                    />
                    <button type="submit" disabled={loading}>
                        {loading ? "Generating agenda..." : "Generate agenda"}
                    </button>
                </form>
                {error && <p role="alert" style={{ color: "red" }}>{error}</p>}
            </div>
         )
}