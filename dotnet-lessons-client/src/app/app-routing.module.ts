import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { HomePageComponent } from './pages/home-page/home-page.component'
import { AsyncLessonPageComponent } from './pages/lessons-page/async-lesson-page/async-lesson-page.component'
import { LessonsPageComponent } from './pages/lessons-page/lessons-page.component'

const routes: Routes = [
  {
    path: `home`,
    component: HomePageComponent,
  },
  {
    path: `lessons`,
    component: LessonsPageComponent,
    children: [
      {
        path: `async`,
        component: AsyncLessonPageComponent,
      }
    ]
  },
  {
    path: `**`,
    redirectTo: `home`,
  },
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
