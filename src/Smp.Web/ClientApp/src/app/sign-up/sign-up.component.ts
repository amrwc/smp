import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CreateUserRequest } from '../models/requests/create-user-request';
import { Error } from '../models/error';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent {
  public createUserRequest: CreateUserRequest = new CreateUserRequest();
  public loading: boolean = false;
  public validationErrors: Array<Error> = [];

  constructor(
    private usersService: UsersService,
    private router: Router
  ) {  }

  public signUp(): void {
    this.validationErrors = [];

    if (this.createUserRequest.password !== this.createUserRequest.confirmPassword) {
      this.validationErrors.push(new Error("invalid_password", "Passwords must match."));
      return;
    }

    this.loading = true;
    this.createUserRequest.email = this.createUserRequest.email.toLowerCase();

    this.usersService.createUser(this.createUserRequest).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/sign-in'], { queryParams: { signUpSuccessful: 'true' } });
      },
      error: (error: any) => {
        this.validationErrors.push(error.error);
        this.loading = false;
      }
    });
  }
}
