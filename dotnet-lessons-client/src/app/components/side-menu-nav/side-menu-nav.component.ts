import { Component } from '@angular/core'

@Component({
  selector: 'app-side-menu-nav',
  templateUrl: './side-menu-nav.component.html',
  styleUrls: ['./side-menu-nav.component.scss']
})
export class SideMenuNavComponent {

  public lessonsLink = [
    {
      name: `Async`,
      path: `lessons/async`
    }
  ]

  constructor() { }
}
