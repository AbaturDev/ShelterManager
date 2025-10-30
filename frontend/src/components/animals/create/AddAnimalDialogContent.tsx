import { useState } from "react";
import {
  AddAnimalProvider,
  AnimalDetailsStep,
  AnimalSpeciesStep,
  AnimalSummaryStep,
} from ".";

interface Props {
  onClose: () => void;
}

export const AddAnimalDialogContent = ({ onClose }: Props) => {
  const [step, setStep] = useState(0);

  const steps = [
    <AnimalDetailsStep key={0} onNext={() => setStep(1)} />,
    <AnimalSpeciesStep
      key={1}
      onBack={() => setStep(0)}
      onNext={() => setStep(2)}
    />,
    <AnimalSummaryStep
      key={2}
      onBack={() => setStep(1)}
      onClose={() => onClose()}
    />,
  ];

  return <AddAnimalProvider>{steps[step]}</AddAnimalProvider>;
};
