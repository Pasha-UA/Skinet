import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormControl, FormGroup, NG_ASYNC_VALIDATORS, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { map, of, switchMap, timer } from 'rxjs';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup
  errors: string[];
  returnUrl: string;
  emailConfirmationUrl: string;
  defaultPhonePrefix: string;

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.createRegisterForm();
    this.emailConfirmationUrl = 'account/emailconfirmation';
    this.returnUrl = '/shop';
    this.defaultPhonePrefix = '+380'
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      displayName: [null, [Validators.required]],
      email: [null,
        [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
        [this.validateEmailNotTaken()]
      ],
      phoneNumber: [null, 
        [Validators.required
          , Validators.pattern('^[0]+[0-9]{9}$')
        ]
      ],
      password: [null, [Validators.required]],
      rememberMe: new FormControl(true)      
    });
  }

  validateEmailNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value) {
            return of(null);
          }
          return this.accountService.checkEmailExists(control.value).pipe(
            map(res => {
              return res ? { emailExists: true } : null;
            })
          );
        })
      );
    }
  }

  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe(
      {
        next: (user) => {
//          this.router.navigateByUrl('/shop');
          if (user.emailConfirmationRequired) {
            this.router.navigateByUrl(
              this.router.createUrlTree(
                [this.emailConfirmationUrl], { queryParams: { email: user.email, rememberMe: this.registerForm.get("rememberMe").value, returnUrl: this.returnUrl  } }));
          }
          else {
            this.router.navigateByUrl(this.returnUrl);
          }
 
        },
        error: error => {
          console.log(error);
          this.errors = error.errors;
        }
      })
  }
}
