import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  model: any = {};
  currentUser$: Observable<User>;

  constructor(
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService) {}

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }

  login() {
    this.accountService.login(this.model)
      .subscribe({
        next: response => {
          console.log(response);
          this.router.navigateByUrl('/members');
        },
        error: er => {
          console.log(er);
          this.toastr.error(er.error);
        }
      })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}