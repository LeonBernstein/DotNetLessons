import { HttpParams } from '@angular/common/http'
import { HttpMethods } from '../enums/http-methods'

export interface RequestConfig {
  actionPath: string,
  actionDesc?: string,
  httpMethod: HttpMethods,
  shouldParseResponseBody: boolean,
  paramsResolver?: () => HttpParams
  requestBodyResolver?: () => any
  isStressTestAllowed: boolean,
  defaultStressTestCounter?: number,
}
