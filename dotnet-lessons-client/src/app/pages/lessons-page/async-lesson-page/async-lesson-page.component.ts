import { Component } from '@angular/core'
import { getAsyncLessonRequestsData } from 'src/app/data/async-lesson-requests'
import { RequestConfig } from 'src/app/models/request-config.interface'

@Component({
  templateUrl: './async-lesson-page.component.html',
  styleUrls: ['./async-lesson-page.component.scss']
})
export class AsyncLessonPageComponent {

  public requestConfigs: RequestConfig[]

  constructor() {
    this.requestConfigs = getAsyncLessonRequestsData()
  }
}
