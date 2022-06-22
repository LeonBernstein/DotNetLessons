import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { environment } from 'src/environments/environment'

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  constructor(
    private readonly httpClient: HttpClient
  ) { }

  public initiateGetRequest$<TResponse>(path: string, params?: HttpParams): Observable<HttpResponse<TResponse>> {
    return this.httpClient.get<TResponse>(environment.apiUrlPrefix + path, { params: params, observe: `response` })
  }
}
