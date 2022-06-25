import { HttpMethods } from '../enums/http-methods'
import { LazyParamsResolvers } from '../enums/lazy-params-resolvers'
import { splitPascalToWords } from '../helpers/split-pascal-to-words'
import { RequestConfig } from '../models/request-config.interface'
import { createRandomPerson } from './create-random-person'

export function getAsyncLessonRequestsData(): RequestConfig[] {
  const result: RequestConfig[] = [
    {
      actionPath: `GetTheTimeOfLongRunningTask`,
      isStressTestAllowed: false,
      httpMethod: HttpMethods.get,
      shouldParseResponseBody: true,
    },
    {
      actionPath: `GetTheTimeOfLongRunningTaskInParallel`,
      isStressTestAllowed: false,
      httpMethod: HttpMethods.get,
      shouldParseResponseBody: true,
    },
    {
      actionPath: `AddPersonSync`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.post,
      shouldParseResponseBody: false,
      defaultSimultaneousRequestsNumber: 10,
      requestBodyResolver: createRandomPerson
    },
    {
      actionPath: `GetAllSync`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.get,
      shouldParseResponseBody: true,
    },
    {
      actionPath: `GetAllSyncAndLazy`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.get,
      shouldParseResponseBody: true,
    },
    {
      actionPath: `DeletePersonSync`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.delete,
      shouldParseResponseBody: false,
      defaultSimultaneousRequestsNumber: 10,
      lazyParamsResolver: LazyParamsResolvers.personIds,
    },
    {
      actionPath: `AddPerson`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.post,
      shouldParseResponseBody: false,
      defaultSimultaneousRequestsNumber: 1000,
      requestBodyResolver: createRandomPerson
    },
    {
      actionPath: `GetAll`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.get,
      shouldParseResponseBody: true,
    },
    {
      actionPath: `DeletePerson`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.delete,
      shouldParseResponseBody: false,
      defaultSimultaneousRequestsNumber: 1000,
      lazyParamsResolver: LazyParamsResolvers.personIds,
    },
  ]
  return result.map(x => {
    x.actionDesc = splitPascalToWords(x.actionPath)
    x.actionPath = `api/AsyncLesson/` + x.actionPath
    return x
  })
}
