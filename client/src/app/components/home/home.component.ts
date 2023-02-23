import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  registerMode = false;
  users: any;

  constructor(private http: HttpClient) {
    this.users = this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  getUsers() {
    this.http.get('https://localhost:7211/api/users')
      .subscribe({
        next: (result) => {
          this.users = result;
        },
        error: (error) => {
          console.log(error);
        }
      })
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
