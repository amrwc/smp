import { Component } from '@angular/core';
import { CurrentUser } from '../models/current-user';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  currentUser: CurrentUser;

  constructor() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }
}
