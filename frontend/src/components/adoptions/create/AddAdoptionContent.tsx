import { useState } from "react";
import { AddAdoptionProvider } from "./AddAdoptionContext";
import { GeneralStep, PersonStep, SummaryStep } from "./steps";

interface Props {
  onClose: () => void;
}

export const AddAdoptionContent = ({ onClose }: Props) => {
  const [step, setStep] = useState(0);

  const steps = [
    <GeneralStep key={0} onNext={() => setStep(1)} />,
    <PersonStep key={1} onBack={() => setStep(0)} onNext={() => setStep(2)} />,
    <SummaryStep key={2} onBack={() => setStep(1)} onClose={() => onClose()} />,
  ];

  return <AddAdoptionProvider>{steps[step]}</AddAdoptionProvider>;
};
