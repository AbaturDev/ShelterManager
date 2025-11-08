import { zodResolver } from "@hookform/resolvers/zod";
import {
  useMutation,
  useQueryClient,
  type UseMutationResult,
} from "@tanstack/react-query";
import { createContext, useContext, useState, type ReactNode } from "react";
import { FormProvider, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import z from "zod";
import { AdoptionService } from "../../../api/services/adoption-service";
import type { CreateAdoption } from "../../../models/adoptions";
import { toaster } from "../../ui/toaster";
import { setFormErrorMessage } from "../../../utils";
import { AxiosError } from "axios";

interface AddAdoptionContextType {
  addAdoptionMutation: UseMutationResult<void, Error, CreateAdoption, unknown>;
  formMethods: ReturnType<typeof useForm<AddAdoptionSchema>>;
  currentStep: number;
  nextStep: () => void;
  prevStep: () => void;
}

const schema = z.object({
  animalId: z.string(setFormErrorMessage("adoptions.fields.errors.animal")),
  animalName: z.string(setFormErrorMessage("adoptions.fields.errors.animal")),
  note: z
    .string()
    .max(350, setFormErrorMessage("adoptions.fields.errors.note"))
    .nullable(),
  personName: z
    .string()
    .min(1, setFormErrorMessage("adoptions.fields.errors.person.name.required"))
    .max(30, setFormErrorMessage("adoptions.fields.errors.person.name.max")),
  personSurname: z
    .string()
    .min(
      1,
      setFormErrorMessage("adoptions.fields.errors.person.surname.required")
    )
    .max(30, setFormErrorMessage("adoptions.fields.errors.person.surname.max")),
  personPhoneNumber: z
    .string()
    .min(
      1,
      setFormErrorMessage("adoptions.fields.errors.person.phone.required")
    )
    .max(15, setFormErrorMessage("adoptions.fields.errors.person.phone.max")),
  personEmail: z
    .string()
    .email(setFormErrorMessage("adoptions.fields.errors.person.email.invalid"))
    .max(50, setFormErrorMessage("adoptions.fields.errors.person.email.max")),
  personPesel: z
    .string()
    .regex(
      /^\d{11}$/,
      setFormErrorMessage("adoptions.fields.errors.person.pesel.invalid")
    ),
  personDocumentId: z
    .string()
    .min(
      1,
      setFormErrorMessage("adoptions.fields.errors.person.document.required")
    )
    .max(
      20,
      setFormErrorMessage("adoptions.fields.errors.person.document.max")
    ),
  personStreet: z
    .string()
    .min(
      1,
      setFormErrorMessage("adoptions.fields.errors.person.street.required")
    )
    .max(100, setFormErrorMessage("adoptions.fields.errors.person.street.max")),
  personCity: z
    .string()
    .min(1, setFormErrorMessage("adoptions.fields.errors.person.city.required"))
    .max(50, setFormErrorMessage("adoptions.fields.errors.person.city.max")),
  personPostalCode: z
    .string()
    .min(
      1,
      setFormErrorMessage("adoptions.fields.errors.person.postalCode.required")
    )
    .max(
      20,
      setFormErrorMessage("adoptions.fields.errors.person.postalCode.max")
    ),
});

type AddAdoptionSchema = z.infer<typeof schema>;

const AddAdoptionContext = createContext<AddAdoptionContextType | null>(null);

const AddAdoptionProvider = ({ children }: { children: ReactNode }) => {
  const queryClient = useQueryClient();
  const { t } = useTranslation();

  const [currentStep, setCurrentStep] = useState(0);

  const nextStep = () => setCurrentStep((s) => s + 1);
  const prevStep = () => setCurrentStep((s) => s - 1);

  const addAdoptionFormMethods = useForm<AddAdoptionSchema>({
    resolver: zodResolver(schema),
    mode: "onChange",
  });

  const addAdoptionMutation = useMutation({
    mutationFn: (data: CreateAdoption) => AdoptionService.createAdoption(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["adoptions"] });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("adoptions.create.toast.success"),
        closable: true,
      });
    },
    onError: (err) => {
      let message: string;
      if (err instanceof AxiosError && err.status === 400) {
        message = t("adoptions.create.toast.exist");
      } else {
        message = t("adoptions.create.toast.error");
      }

      toaster.create({
        type: "error",
        title: t("error"),
        description: message,
        closable: true,
      });
    },
  });

  return (
    <AddAdoptionContext.Provider
      value={{
        addAdoptionMutation,
        formMethods: addAdoptionFormMethods,
        currentStep,
        nextStep,
        prevStep,
      }}
    >
      <FormProvider {...addAdoptionFormMethods}>{children}</FormProvider>
    </AddAdoptionContext.Provider>
  );
};

const useAddAdoptionContext = () => {
  const context = useContext(AddAdoptionContext);
  if (!context) {
    throw new Error(
      "Add adoption context must be used with a settings provider"
    );
  }
  return context;
};
export { AddAdoptionProvider, useAddAdoptionContext };
export type { AddAdoptionSchema };
