import { useState } from 'react';
import { useDispatch } from 'react-redux';
import { getTextSummary } from '../services/AiService';
import { setResponse } from '../store/aiResponseSlice';

export default function SummorizeTextPage(){
    const [notes, setNotes] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState("");
    const dispatch = useDispatch();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!notes.trim()) return; // Do not send an empty string to be processed.

        setIsLoading(true);
        setError("");

        try{
            const result = await getTextSummary(notes);
            dispatch(setResponse(result))
            
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

        
    </>
    )
}