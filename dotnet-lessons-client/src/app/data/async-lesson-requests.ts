import { HttpMethods } from '../enums/http-methods'
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
      defaultStressTestCounter: 10,
      requestBodyResolver: createRandomPerson
    },
    {
      actionPath: `GetAllSync`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.get,
      shouldParseResponseBody: true,
      defaultStressTestCounter: 1,
    },
    {
      actionPath: `GetAllSyncAndLazy`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.get,
      shouldParseResponseBody: true,
      defaultStressTestCounter: 1,
    },
    {
      actionPath: `DeletePersonSync`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.delete,
      shouldParseResponseBody: false,
      defaultStressTestCounter: 10,
    },
    {
      actionPath: `AddPersonAsync`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.post,
      shouldParseResponseBody: false,
      defaultStressTestCounter: 1000,
      requestBodyResolver: createRandomPerson
    },
    {
      actionPath: `GetAllAsync`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.get,
      shouldParseResponseBody: true,
      defaultStressTestCounter: 1,
    },
    {
      actionPath: `DeletePersonAsync`,
      isStressTestAllowed: true,
      httpMethod: HttpMethods.delete,
      shouldParseResponseBody: false,
      defaultStressTestCounter: 1000,
    },
  ]
  return result.map(x => {
    x.actionDesc = x.actionPath.replace(/([a-z])([A-Z])/g, '$1 $2')
    x.actionPath = `api/AsyncLesson/` + x.actionPath
    return x
  })
}
