import { Component, OnInit } from '@angular/core';
import { CurrentUser } from '../models/current-user';

@Component({
  selector: 'app-nav-footer',
  templateUrl: './nav-footer.component.html',
  styleUrls: ['./nav-footer.component.scss']
})
export class NavFooterComponent implements OnInit {

  private currentUser: CurrentUser;

  constructor() { }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

}
