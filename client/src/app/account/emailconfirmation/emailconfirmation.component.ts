import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-emailconfirmation',
  templateUrl: './emailconfirmation.component.html',
  styleUrls: ['./emailconfirmation.component.scss']
})
export class EmailConfirmationComponent implements OnInit {
  email: string;
  confirmForm: FormGroup;
  rememberMe: boolean;
  returnUrl: string;

  constructor(private accountService: AccountService, 
              private router: Router, 
              private activatedRoute: ActivatedRoute,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.email = this.activatedRoute.snapshot.queryParams.email;
    this.accountService.emailConfirmation(this.email);
    this.rememberMe = (this.activatedRoute.snapshot.queryParams.rememberMe == "true");
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl;
    this.createConfirmForm();
  }

  createConfirmForm() {
    this.confirmForm = new FormGroup({
      email: new FormControl(this.email, [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]),
      confirmationCode: new FormControl('', [Validators.required]),
      rememberMe: new FormControl(this.rememberMe)
    })
  }

  onSubmit() {
    this.accountService.emailConfirm(this.confirmForm.value).subscribe({
      next: () => {
        this.toastr.success("Your email is confirmed successfully");
        this.router.navigateByUrl(this.returnUrl);
      }
    })
  }

  onResendConfirmationCode() {
    this.accountService.emailConfirmation(this.email);
    this.toastr.success("New email confirmation code is sent");
  }
}
