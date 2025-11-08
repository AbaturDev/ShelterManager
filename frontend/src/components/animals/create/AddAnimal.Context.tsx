import { zodResolver } from "@hookform/resolvers/zod";
import {
  useMutation,
  useQueryClient,
  type UseMutationResult,
} from "@tanstack/react-query";
import { createContext, useContext, useState, type ReactNode } from "react";
import { FormProvider, useForm } from "react-hook-form";
import z from "zod";
import type { CreateAnimal } from "../../../models/animal";
import { AnimalService } from "../../../api/services/animals-service";
import { toaster } from "../../ui/toaster";
import { useTranslation } from "react-i18next";
import { setFormErrorMessage } from "../../../utils/form-error-handlers";

interface AddAnimalContextType {
  addAnimalMutation: UseMutationResult<void, Error, CreateAnimal, unknown>;
  formMethods: ReturnType<typeof useForm<AddAnimalSchema>>;
  currentStep: number;
  nextStep: () => void;
  prevStep: () => void;
}

const schema = z.object({
  name: z
    .string()
    .min(3, setFormErrorMessage("animals.create.errors.nameMin"))
    .max(30, setFormErrorMessage("animals.create.errors.nameMax")),
  admissionDate: z
    .date(setFormErrorMessage("animals.create.errors.date"))
    .max(new Date(), setFormErrorMessage("animals.create.errors.dateFuture")),
  sex: z.number(),
  age: z
    .number()
    .min(0, setFormErrorMessage("animals.create.errors.ageMin"))
    .max(100, setFormErrorMessage("animals.create.errors.ageMax"))
    .nullable(),
  speciesId: z.string(setFormErrorMessage("animals.create.errors.species")),
  species: z.string(setFormErrorMessage("animals.create.errors.species")),
  breed: z.string(setFormErrorMessage("animals.create.errors.breed")),
  breedId: z.string(setFormErrorMessage("animals.create.errors.breed")),
  description: z
    .string()
    .max(350, setFormErrorMessage("animals.create.errors.description"))
    .nullable(),
});

type AddAnimalSchema = z.infer<typeof schema>;

const AddAnimalContext = createContext<AddAnimalContextType | null>(null);

const AddAnimalProvider = ({ children }: { children: ReactNode }) => {
  const queryClient = useQueryClient();
  const { t } = useTranslation();

  const [currentStep, setCurrentStep] = useState(0);

  const nextStep = () => setCurrentStep((s) => s + 1);
  const prevStep = () => setCurrentStep((s) => s - 1);

  const addAnimalFormMethods = useForm<AddAnimalSchema>({
    resolver: zodResolver(schema),
    mode: "onChange",
  });

  const addAnimalMutation = useMutation({
    mutationFn: (data: CreateAnimal) => AnimalService.createAnimal(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["animals"] });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("animals.create.toast.success"),
        closable: true,
      });
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("animals.create.toast.error"),
        closable: true,
      });
    },
  });

  return (
    <AddAnimalContext.Provider
      value={{
        addAnimalMutation,
        formMethods: addAnimalFormMethods,
        currentStep,
        nextStep,
        prevStep,
      }}
    >
      <FormProvider {...addAnimalFormMethods}>{children}</FormProvider>
    </AddAnimalContext.Provider>
  );
};

const useAddAnimalContext = () => {
  const context = useContext(AddAnimalContext);
  if (!context) {
    throw new Error("Add animal context must be used with a settings provider");
  }

  return context;
};

export { AddAnimalProvider, useAddAnimalContext };
export type { AddAnimalSchema };
