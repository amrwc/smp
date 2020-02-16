import { Component, OnInit } from '@angular/core';
import { CurrentUser } from '../models/current-user';

@Component({
  selector: 'app-nav-footer',
  templateUrl: './nav-footer.component.html',
  styleUrls: ['./nav-footer.component.scss']
})
export class NavFooterComponent implements OnInit {

  public currentUser: CurrentUser;

  constructor() { }

  ngOnInit() {
    let currentUser = localStorage.getItem('currentUser');

    if (currentUser) this.currentUser = JSON.parse(currentUser);
  }

}
