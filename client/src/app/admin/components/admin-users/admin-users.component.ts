import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { IRole, IUser } from 'src/app/shared/models/user';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.scss']
})
export class AdminUsersComponent implements OnInit {
  users: IUser[];
  users$: Observable<IUser>;
  roles: IRole[];

  constructor(private adminService: AdminService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.users$ = this.adminService.getUserList() as Observable<IUser>;
    this.adminService.getUserList().subscribe({
      next: (users: IUser[]) => {
        console.log(users);
        this.users = users;
      },
      error: (e) => console.error(e)
    });
  }
}
