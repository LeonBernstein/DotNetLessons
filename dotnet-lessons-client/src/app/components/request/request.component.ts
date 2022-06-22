import { HttpErrorResponse } from '@angular/common/http'
import { Component, Input, OnChanges, OnDestroy, SimpleChange, SimpleChanges } from '@angular/core'
import { catchError, of, Subscription, timeInterval } from 'rxjs'
import { msToTime } from 'src/app/helpers/msToTime'
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

  public executeAction(path: string, btnIndex: number): void {
    const requestState = this.requestStates[btnIndex]
    requestState.isRequestActive = true
    const sub = this.httpService.initiateGetRequest$<any>(path)
      .pipe(
        catchError((e: HttpErrorResponse) => of(e)),
        timeInterval(),
      ).subscribe(responseWithTimeInterval => {
        const response = responseWithTimeInterval.value
        this.requestSubs.remove(sub)
        const body = response instanceof HttpErrorResponse ? response.error : response.body
        requestState.status = response.status
        requestState.statusText = response.statusText
        requestState.body = typeof (body) === `object` ? JSON.stringify(body, null, `\t`) : body
        requestState.isRequestActive = false
        requestState.requestTime = msToTime(responseWithTimeInterval.interval)
      })
    this.requestSubs.add(sub)
  }


}
