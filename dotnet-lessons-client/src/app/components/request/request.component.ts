import { HttpErrorResponse, HttpResponse } from '@angular/common/http'
import { Component, Input, OnChanges, OnDestroy, SimpleChange, SimpleChanges } from '@angular/core'
import { catchError, forkJoin, Observable, of, Subscription, timeInterval } from 'rxjs'
import { HttpMethods } from 'src/app/enums/http-methods'
import { LazyParamsResolvers as LazyParamsResolver } from 'src/app/enums/lazy-params-resolvers'
import { generator } from 'src/app/helpers/generator'
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
    if (requestState.isRequestActive) return
    requestState.isRequestActive = true

    setTimeout(() => {
      this.handleRequestsAsync(requestConfig, requestState)
    }, 300)
  }

  private async handleRequestsAsync(requestConfig: RequestConfig, requestState: RequestState): Promise<void> {
    try {
      await this.handlePreFetchData(requestConfig, requestState)
    } catch {
      requestState.isRequestActive = false
      return alert(`Can't resolve params.`)
    }


    const requestObservable = Array(requestState.numberSimultaneousRequests || 1)
      .fill(null)
      .map(() => this.resolveRequestObservable$(requestConfig)
        .pipe(catchError((e: HttpErrorResponse) => of(e))))

    const sub = forkJoin(requestObservable)
      .pipe(timeInterval()).subscribe(responseWithTimeInterval => {
        this.requestSubs.remove(sub)

        const responses: (HttpResponse<unknown> | HttpErrorResponse)[] = responseWithTimeInterval.value
        this.handleResponses(responses, requestConfig, requestState)

        requestState.requestTime = msToTime(responseWithTimeInterval.interval)
        requestState.isRequestActive = false
      })

    this.requestSubs.add(sub)
  }

  private async handlePreFetchData(requestConfig: RequestConfig, requestState: RequestState): Promise<void> {
    return new Promise(async (resolve, reject) => {
      if (requestConfig.lazyParamsResolver === LazyParamsResolver.personIds) {
        const personIdsParams = await this.httpService.getPersonsIdsAsync(requestState.numberSimultaneousRequests || 1)
        if (!personIdsParams || !personIdsParams.length) return reject()
        if (requestState.numberSimultaneousRequests
          && personIdsParams.length < requestState.numberSimultaneousRequests
        ) {
          requestState.numberSimultaneousRequests = personIdsParams.length
        }
        const paramsGenerator = generator(personIdsParams)
        requestConfig.paramsResolver = () => paramsGenerator.next().value
      }
      resolve()
    })
  }

  private resolveRequestObservable$<TResponse>(requestConfig: RequestConfig): Observable<HttpResponse<TResponse>> {
    switch (requestConfig.httpMethod) {
      case (HttpMethods.get): return this.httpService.initiateGetRequest$<any>(requestConfig.actionPath, requestConfig.paramsResolver?.())
      case (HttpMethods.post): return this.httpService.initiatePostRequest$<any>(requestConfig.actionPath, requestConfig.requestBodyResolver?.())
      case (HttpMethods.delete): return this.httpService.initiateDeleteRequest$<any>(requestConfig.actionPath, requestConfig.paramsResolver?.())
    }
  }

  private handleResponses(
    responses: (HttpResponse<unknown> | HttpErrorResponse)[],
    requestConfig: RequestConfig,
    requestState: RequestState
  ) {
    const lastResponse = responses[responses.length - 1]

    requestState.status = lastResponse.status
    requestState.statusText = lastResponse.statusText

    if (requestConfig.shouldParseResponseBody) {
      const body = lastResponse instanceof HttpErrorResponse ? lastResponse.error : lastResponse.body
      requestState.body = typeof (body) === `object` ? JSON.stringify(body, null, '  ') : body
    } else {
      requestState.body = null
    }

    const successCounter = responses.filter(x => x.status && x.status < 400 && x.status > 199).length
    requestState.successCounterResult = `${successCounter}/${responses.length}`
  }
}
