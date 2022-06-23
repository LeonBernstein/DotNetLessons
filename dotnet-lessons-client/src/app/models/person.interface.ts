import { Address } from './address.interface'

export interface Person {
  firstName: string,
  lastName: string,
  isFromEarth: boolean,

  Addresses: Address[]
}
