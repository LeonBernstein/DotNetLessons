import { HttpErrorResponse, HttpResponse } from '@angular/common/http'
import { Component, Input, OnChanges, OnDestroy, SimpleChange, SimpleChanges } from '@angular/core'
import { catchError, Observable, of, Subscription, timeInterval } from 'rxjs'
import { HttpMethods } from 'src/app/enums/http-methods'
import { msToTime } from 'src/app/helpers/ms-to-time'
import { RequestConfig } from 'src/app/models/request-config.interface'
import { RequestState } from 'src/app/models/request-state.interface'
import { HttpService } from 'src/app/services/http.service'

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.scss']
})
export class RequestComponent implements OnChanges, OnDestroy {
  @Input() public requestConfigs: RequestConfig[] = []
  public requestStates: RequestState[] = []
  public requestSubs: Subscription = new Subscription()

  constructor(
    private readonly httpService: HttpService
  ) { }
  ngOnDestroy(): void {
    this.requestSubs.unsubscribe()
  }

  ngOnChanges(changes: SimpleChanges): void {
    const requestsChange: SimpleChange = changes[`requestConfigs`]
    if (requestsChange && requestsChange.currentValue !== requestsChange.previousValue) {
      this.requestStates = this.requestConfigs.map(() => ({
        isRequestActive: false,
      }))
    }
  }

  public executeAction(requestConfig: RequestConfig, btnIndex: number): void {
    const requestState = this.requestStates[btnIndex]
    requestState.isRequestActive = true
    const sub = this.resolveRequestObservable$(requestConfig)
      .pipe(
        catchError((e: HttpErrorResponse) => of(e)),
        timeInterval(),
      ).subscribe(responseWithTimeInterval => {
        const response = responseWithTimeInterval.value
        this.requestSubs.remove(sub)
        requestState.status = response.status
        requestState.statusText = response.statusText
        requestState.isRequestActive = false
        requestState.requestTime = msToTime(responseWithTimeInterval.interval)
        if (requestConfig.shouldParseResponseBody) {
          const body = response instanceof HttpErrorResponse ? response.error : response.body
          requestState.body = typeof (body) === `object` ? JSON.stringify(body, null, '  ') : body
        } else {
          requestState.body = null
        }
      })
    this.requestSubs.add(sub)
  }

  public resolveRequestObservable$<TResponse>(requestConfig: RequestConfig): Observable<HttpResponse<TResponse>> {
    switch (requestConfig.httpMethod) {
      case (HttpMethods.get): return this.httpService.initiateGetRequest$<any>(requestConfig.actionPath, requestConfig.paramsResolver?.())
      case (HttpMethods.post): return this.httpService.initiatePostRequest$<any>(requestConfig.actionPath, requestConfig.requestBodyResolver?.())
      case (HttpMethods.delete): return this.httpService.initiateDeleteRequest$<any>(requestConfig.actionPath, requestConfig.paramsResolver?.())
    }
  }
}
