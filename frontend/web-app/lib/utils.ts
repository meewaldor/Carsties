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
export function numberWithCommas(amount: number) {
  return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}
