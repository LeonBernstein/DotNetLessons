import { faker } from '@faker-js/faker/locale/en'
import { Address } from '../models/address.interface'
import { Person } from '../models/person.interface'

export function createRandomPerson(): Person {
  return {
    firstName: faker.name.firstName(),
    lastName: faker.name.lastName(),
    isFromEarth: faker.datatype.boolean(),
    Addresses: createRandomAddresses(5)
  }
}

function createRandomAddresses(numOfAddressesToCreate: number): Address[] {
  return [...Array(numOfAddressesToCreate)].map(() => ({
    galaxyName: faker.random.word(),
    planetName: faker.random.word(),
    hasCosmicRadiation: faker.datatype.boolean(),
  }))
}
