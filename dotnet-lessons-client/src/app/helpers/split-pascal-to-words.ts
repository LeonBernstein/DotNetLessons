
export function splitPascalToWords(pascalStr: string): string {
  return pascalStr.replace(/([a-z])([A-Z])/g, '$1 $2')
}
