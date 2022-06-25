
export interface RequestState {
  isRequestActive: boolean,
  body?: any,
  status?: number,
  statusText?: string
  requestTime?: string
  numberSimultaneousRequests?: number
  successCounterResult?: string
}
