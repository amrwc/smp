import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignInRequest } from '../models/sign-in-request';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
  private readonly baseUrl: string;
  private readonly httpClient: HttpClient;
  public signInRequest = new SignInRequest();
  public loading: boolean = false;
  public returnUrl: string;

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.baseUrl = baseUrl;
    this.httpClient = http;
  }

  ngOnInit(): void {
    // NOTE: Sign out functionality
    //       When user presses 'Sign out', the router will send them
    //       to the Sign in page, which voids the session.
    //       -- They wouldn't go to Sign in page if they wanted to stay
    //       signed in.
    //       __Uncomment 'implements OnInit__
    // localStorage.removeItem('currentUser');
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  public signIn() {
    this.loading = true;

    this.httpClient
      .post(this.baseUrl + 'api/Auth/SignIn', this.signInRequest)
      .subscribe(result => {
        localStorage.setItem('currentUser', JSON.stringify(result));
        this.loading = false;
        this.router.navigate([this.returnUrl]);
      }, error => {
        console.error(error);
        setTimeout(() => this.loading = false, 1500);
      });
  }

  // NOTE: Wire this up with a Sign out button in settings.
  // public signOut() {
  //   localStorage.removeItem('currentUser');
  // }
}
