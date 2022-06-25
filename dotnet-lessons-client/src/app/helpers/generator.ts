
export function* generator<T>(arr: T[]): Generator<T> {
  for (const item of arr) yield item
}
