import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';
import { IUser } from 'src/app/shared/models/user';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  returnUrl: string;
  emailConfirmationUrl: string

  constructor(private accountService: AccountService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/shop';
    this.emailConfirmationUrl = 'account/emailconfirmation';
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]),
      password: new FormControl('', [Validators.required]),
      rememberMe: new FormControl(true)
    });
  }

  onSubmit() {
    this.accountService.login(this.loginForm.value).subscribe(
      {
        next: (user) => {
          
          if (user.emailConfirmationRequired) {
            this.router.navigateByUrl(
              this.router.createUrlTree(
                [this.emailConfirmationUrl], { queryParams: { email: user.email, rememberMe: this.loginForm.get("rememberMe").value, returnUrl: this.returnUrl  } }));
          }
          else {
            this.router.navigateByUrl(this.returnUrl);
          }
        },
        error: error => {
          console.log(error);
        },
      }
    );
  }
}
