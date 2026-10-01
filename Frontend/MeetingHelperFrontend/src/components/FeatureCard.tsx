import { useState, type ReactNode} from "react";

interface FeatureCardProps{
    title: string;
    children: ReactNode; //Lewts us put other components inside of this component :)
}

export default function FeatureCard({title, children}: FeatureCardProps) {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <div>
            <button 
            className="function-button"
            type="button"
            onClick={() => setIsOpen(!isOpen)}
            style={{
            backgroundColor: isOpen ? "#c084fc" : "#c084fc59",
            color: isOpen ? "black" : "white",
            fontWeight: isOpen ? "bold" : "normal",}}>
            <span>{title}</span>
            <span>{isOpen ? '▲' : '▼' }</span>
            </button>

            {isOpen && (
                <div className = "child-container"> 
                    
                    {children}
                </div>
            )}
        </div>
    )
}