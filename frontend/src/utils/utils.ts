export const praseBool = (value: unknown): boolean => {
  if (value === undefined || value === null) return false;

  if (typeof value === "boolean") {
    return value;
  }
  if (typeof value === "number") {
    return value === 1;
  }
  if (typeof value === "string") {
    const v = value.toLowerCase().trim();
    return v === "true" || v === "1";
  }

  return false;
};
