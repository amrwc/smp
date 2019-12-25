import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../models/user';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  private userId: string;
  private user: User;

  constructor(
    private httpClient: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private route: ActivatedRoute,
    private router: Router
  ) {  }



  ngOnInit() {
    this.userId = this.route.snapshot.paramMap.get('id');

    this.httpClient.get(this.baseUrl + 'api/Users/GetUser/' + this.userId)
    .subscribe(
      result => {
        this.user = JSON.parse(result.toString());
      },
      error => {
        console.error(error);
      }
    )
    //TODO: Get all relevant information 
  }

}
