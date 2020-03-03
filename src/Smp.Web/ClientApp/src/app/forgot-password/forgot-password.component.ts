import { Component, OnInit } from '@angular/core';
import { AccountsService } from '../services/accounts.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {

  public accountEmail: string;
  public loading: boolean;

  public startPasswordResetUnsuccessful: boolean = false;
  public startPasswordResetSuccessful: boolean = false;

  public validationError: Error;
  public response: string;

  constructor(private accountsService: AccountsService) { }

  ngOnInit() {
  }

  public startPasswordReset(): void {
    this.validationError = null;
    this.startPasswordResetUnsuccessful = false;
    this.startPasswordResetSuccessful = false;
    this.accountEmail = this.accountEmail.toLowerCase();

    this.accountsService.forgottenPassword(this.accountEmail).subscribe({
      next: () => {
        this.loading = false;
        this.startPasswordResetSuccessful = true;
        this.response = "An email has been sent to you. Click it to reset your password!";
      },
      error: (error: any) => {
        this.loading = false;
        this.startPasswordResetUnsuccessful = true;
        this.validationError = error.error;
      }
    });
  }
}
