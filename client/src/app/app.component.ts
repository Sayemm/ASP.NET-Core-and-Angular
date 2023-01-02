import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'The Dating App';
  users: any;

  constructor(
    private httpService: HttpClient
  ) {
  }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.httpService.get('https://localhost:7211/api/users')
    .subscribe({
      next: (result) => {
        this.users = result;
      },
      error: (error) => {
        console.log(error);
      }
    })
  }
}
