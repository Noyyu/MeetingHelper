import { useState } from "react"
import { getAudioSummary } from "../services/AiService"
import { useDispatch } from 'react-redux';
import { setResponse } from "../store/aiResponseSlice";

export default function SummarizeAudioCardContent(){

    const [audioFile, setAudioFile] = useState<File | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const dispatch = useDispatch();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!audioFile) {
            setError("Please select an audio file to summarize.");
            return;
        }

        setLoading(true);
        setError("");

        try {
            const response = await getAudioSummary(audioFile);
            dispatch(setResponse(response));
        } catch (error) {
            setError("Could not reach the server.");
            console.error(error);
        } finally {
            setLoading(false);
        }
    }

    return (
        <>
            <h2>Upload an audio recording</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="file"
                    accept="audio/*"
                    onChange={(event) => setAudioFile(event.currentTarget.files?.[0] ?? null)}
                    aria-label="Audio file"
                />
                <button type="submit" disabled={loading}>
                    {loading ? "Summarising..." : "Summarise audio"}
                </button>
            </form>

            {error && <p role="alert" style={{ color: "red" }}>{error}</p>}
        </>
    );
}