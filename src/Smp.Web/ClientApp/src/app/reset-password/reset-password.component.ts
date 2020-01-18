import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsService } from '../services/accounts.service';
import { ResetPasswordRequest } from '../models/requests/reset-password-request';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {

  public resetPasswordRequest: ResetPasswordRequest = new ResetPasswordRequest();
  public loading: boolean;
  public validationErrors: Error[] = [];
  public resetPasswordSuccessful: boolean = false;

  constructor(private accountsService: AccountsService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.resetPasswordRequest.actionId = this.route.snapshot.paramMap.get('id');
  }

  public resetPassword(): void {
    this.loading = true;
    this.validationErrors = [];
    this.resetPasswordSuccessful = false;

    this.accountsService.resetPassword(this.resetPasswordRequest).subscribe({
      next: () => {
        this.loading = false;
        this.resetPasswordSuccessful = true;
      },
      error: (error: any) => {
        this.loading = false;
        this.validationErrors = error.error;
      }
    });
  }
}
