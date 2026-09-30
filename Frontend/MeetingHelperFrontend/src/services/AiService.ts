
export const getTextSummary = async (textToSummarize: string) => {
    const payload = { content: textToSummarize };

    const response = await fetch("https://localhost:7210/ai/summarizeText", { 
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
    });

    if (!response.ok) {
        throw new Error("Could not create a summurization.");
    }

    const data = await response.json();
    return data.result; 
};