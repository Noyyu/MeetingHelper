import "../index.css"
import FeatureCard from "../components/FeatureCard";
import SummorizeTextPage from "../components/SummorizeTextCardContent";
export default function HomePage(){

    return (
    <>
        <section id="center">

            <div>
                <h1>Meeting helper </h1>
                <p>
                    The ulitmate AI assistant to help manage your meetings
                </p>
            </div>

                <div className="page-button-container">
                    <FeatureCard title="Summarize meeting notes">
                        <SummorizeTextPage />
                    </FeatureCard>
                </div>


        </section>
    </>
    )
}