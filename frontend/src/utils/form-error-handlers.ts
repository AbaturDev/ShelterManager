import type {
  FieldError,
  FieldErrorsImpl,
  FieldValue,
  FieldValues,
  Merge,
} from "react-hook-form";

import i18next from "i18next";

type FormError = {
  translationKey: string;
  params?: Record<string, string | number>;
};

export const setFormErrorMessage = (
  translationKey: string,
  params?: Record<string, string | number>
) => {
  return JSON.stringify({ translationKey, params });
};

export const getFormErrorMessage = (formErrorAsString: string | undefined) => {
  if (!formErrorAsString) return;

  try {
    const parsedFormError: FormError = JSON.parse(formErrorAsString);
    if (parsedFormError?.translationKey) {
      return i18next.t(parsedFormError.translationKey, parsedFormError.params);
    }
  } catch {
    return formErrorAsString;
  }
};

export const getFormErrorMessageFromArray = <T extends FieldValues>(
  array?:
    | Merge<
        FieldError,
        (Merge<FieldError, FieldErrorsImpl<FieldValue<T>>> | undefined)[]
      >
    | undefined
): string | undefined => {
  if (!Array.isArray(array)) return;
  const errors: string[] = [];

  array.forEach((item) => {
    if (Array.isArray(item)) return getFormErrorMessageFromArray(item);
    if (item instanceof Object) {
      Object.values(item).forEach((value) => {
        if (value instanceof Object && "message" in value) {
          return errors.push(value.message as string);
        }
      });
    }
  });

  const uniqueErrors = [...new Set(errors)];

  const finalErrors = uniqueErrors.map((error) => {
    const parsedError = JSON.parse(error);
    if (parsedError && "translationKey" in parsedError) {
      return getFormErrorMessage(error);
    }
    return error;
  });

  return finalErrors.join(". ");
};
