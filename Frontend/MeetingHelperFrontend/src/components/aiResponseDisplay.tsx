
import { type RootState } from "../store/store"
import { useSelector } from 'react-redux';
import ReactMarkdown from "react-markdown";
import remarkBreaks from "remark-breaks";
import remarkGfm from "remark-gfm";

export default function AiResponseDisplay(){
    const aiResponse = useSelector((state: RootState) => state.aiResponse.response)

    return(
        <section className="ai-response" style={{ paddingBottom: 20}}>
            {aiResponse && (
                <>
                    <h2>Text generated</h2>
                    <ReactMarkdown remarkPlugins={[remarkGfm, remarkBreaks]}>
                        {aiResponse}
                    </ReactMarkdown>
                </>
            )}
        </section>
    )
}