import { HttpParams } from '@angular/common/http'
import { HttpMethods } from '../enums/http-methods'
import { LazyParamsResolvers } from '../enums/lazy-params-resolvers'

export interface RequestConfig {
  actionPath: string,
  actionDesc?: string,
  httpMethod: HttpMethods,
  shouldParseResponseBody: boolean,
  paramsResolver?: () => HttpParams
  requestBodyResolver?: () => any
  isStressTestAllowed: boolean,
  defaultSimultaneousRequestsNumber?: number,
  lazyParamsResolver?: LazyParamsResolvers
}
