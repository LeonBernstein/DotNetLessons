import { RequestConfig } from '../models/request-config.interface'

export function getAsyncLessonRequestsData(): RequestConfig[] {
  return [
    {
      actionPath: `GetTheTimeOfLongRunningTask`,
      isStressTestAllowed: false,
    },
    {
      actionPath: `GetTheTimeOfLongRunningTaskInParallel`,
      isStressTestAllowed: false,
    },
  ].map((x: RequestConfig) => {
    x.actionDesc = x.actionPath.replace(/([a-z])([A-Z])/g, '$1 $2')
    x.actionPath = `api/AsyncLesson/` + x.actionPath
    return x
  })
}
