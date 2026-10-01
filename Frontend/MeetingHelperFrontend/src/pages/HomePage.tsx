import "../index.css"
import FeatureCard from "../components/FeatureCard";
import SummorizeTextPage from "../components/SummarizeTextCardContent";
import AiResponseDisplay from "../components/aiResponseDisplay";
import SummorizeAudioCardContent from "../components/SummarizeAudioCardContent";
import InvitationMessageCardContent from "../components/InvitationMessageCardContent";
import AgendaMessageCardContent from "../components/AgendaMessageCardContent";
export default function HomePage(){
    const features = [
        { title: "Summarize meeting notes", content: <SummorizeTextPage /> },
        { title: "Summarize audio recording", content: <SummorizeAudioCardContent /> },
        { title: "Generate meeting invitation", content: <InvitationMessageCardContent /> },
        { title: "Generate meeting agenda", content: <AgendaMessageCardContent /> },
    ];

    return (
    <>
        <section id="center">

            <div>
                <h1 style={{ paddingTop: 20}}>Meeting helper</h1>
                <p>
                    The ulitmate AI assistant to help manage your meetings
                </p>
            </div>

                <ul className="feature-list">
                    {features.map(({ title, content }) => (
                        <li key={title}>
                            <FeatureCard title={title}>{content}</FeatureCard>
                        </li>
                    ))}
                </ul>

                <AiResponseDisplay />


        </section>
    </>
    )
}