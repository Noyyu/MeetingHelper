import { useState } from 'react';
import { getTextSummary } from '../services/AiService';

export default function SummorizeTextPage(){
    const [notes, setNotes] = useState("");
    const [summery, setSummery] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState("");

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!notes.trim()) return; // Do not send an empty string to be processed.

        setIsLoading(true);
        setError("");

        try{
            const result = await getTextSummary(notes);
            setSummery(result);
            
        } catch (error) {
            setError("Could not reach the server.")
            console.error(error);
        } finally{
            setIsLoading(false);
        }
    }


    return (
    <>
        <h2> Enter text notes</h2>
        <form onSubmit={handleSubmit}>
            <textarea 
            placeholder="Meeting notes..."
            value= {notes}
            onChange={(e) => setNotes(e.target.value)}/>
            <button type="submit" disabled={isLoading || !notes.trim()}> {isLoading ? "Summarising..." : "Summarise" } </button>
        </form>

        {error && <p style={{ color : "red"}}> {error} </p>}

        {summery && (
            <div>
                <h3> Summary </h3>
                <p>{summery}</p>
            </div>
        )}
    </>
    )
}