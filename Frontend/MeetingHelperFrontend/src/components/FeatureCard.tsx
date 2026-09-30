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
            type="button"
            onClick={() => setIsOpen(!isOpen)}>
            <span>{title}</span>
            <span>{isOpen ? '▲' : '▼' }</span>
            </button>

            {isOpen && (
                <div> 
                    {children}
                </div>
            )}
        </div>
    )
}