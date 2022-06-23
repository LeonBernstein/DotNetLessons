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

  public initiatePostRequest$<TResponse>(path: string, body?: any): Observable<HttpResponse<TResponse>> {
    return this.httpClient.post<TResponse>(environment.apiUrlPrefix + path, body, { observe: `response` })
  }

  public initiateDeleteRequest$<TResponse>(path: string, params?: HttpParams): Observable<HttpResponse<TResponse>> {
    return this.httpClient.delete<TResponse>(environment.apiUrlPrefix + path, { params: params, observe: `response` })
  }
}
