import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { EmailConfirmationComponent } from './emailconfirmation/emailconfirmation.component';


const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'emailconfirmation', component: EmailConfirmationComponent},
  {path: 'register', component: RegisterComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
