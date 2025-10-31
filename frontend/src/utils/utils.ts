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

export const formatDate = (date: string | undefined) => {
  if (!date) return "";
  const d = new Date(date);
  const month = String(d.getMonth() + 1).padStart(2, "0");
  const day = String(d.getDate()).padStart(2, "0");
  return `${d.getFullYear()}-${month}-${day}`;
};
