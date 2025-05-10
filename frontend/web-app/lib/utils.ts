export function filterEmptyValues(values: object) {
  return Object.fromEntries(
    Object.entries(values).filter(
      ([, value]) =>
        value !== '' &&
        value !== null &&
        value !== undefined &&
        value.length !== 0
    )
  );
}
