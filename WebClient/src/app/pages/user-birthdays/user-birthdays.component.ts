import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RouterOutlet } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

import { User } from '../../model/user.model'; // Импортируйте класс User из внешнего файла

@Component({
  selector: 'app-user-birthdays',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule, CommonModule],
  templateUrl: './user-birthdays.component.html',
  styleUrl: './user-birthdays.component.css',
  schemas: [NO_ERRORS_SCHEMA]
})
export class UserBirthdaysComponent implements OnInit {
  users: User[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.fetchUsers();
    // update
  }

  fetchUsers() {
    this.http.get<string[]>('https://localhost:7267/api/BirthdaysAPI').subscribe(
      (data) => {
        this.users = data.map(userJson => new User(JSON.parse(userJson)));
        console.log(data);
        console.log(new Date());
        this.users.forEach(element => {
          console.log(element.Birthday);
        });
        this.displayUsers();
      },
      (error) => {
        console.error('Error fetching users:', error);
        this.users = [];
      }
    );
  }
  
  displayUsers() {
    // Здесь вы можете добавить код для вывода пользователей в HTML
    this.users.forEach(user => {
      console.log(`Name: ${user.UserName}, Email: ${user.Email}, Country: ${user.Country}`);
      // Добавьте здесь код для вывода пользователя в HTML
    });
  }

  isTodaysBirthday(user: User): boolean {
    const today = new Date();
    const userBirthday = user.Birthday;
    if (userBirthday)
    return (
      userBirthday.getMonth() === today.getMonth() &&
      userBirthday.getDate() === today.getDate()
    );
    return false
  }

  isFirstRecord(user: User): boolean {
    return this.users[0] === user;
  }
}