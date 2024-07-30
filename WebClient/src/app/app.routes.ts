import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { UserListGetComponent } from './pages/user-list-get/user-list-get.component';
import { UserPostComponent } from './pages/user-post/user-post.component';
import { MainPageComponent } from './pages/main-page/main-page.component';
import { UserBirthdaysComponent } from './pages/user-birthdays/user-birthdays.component'; 
//import { TestComponent } from './pages/test/test.component';

export const routes: Routes = [
    { path: '', component: UserBirthdaysComponent, pathMatch: 'full' },
    { path: 'Users', component: UserListGetComponent, pathMatch: 'full' },
    { path: 'NewUser', component: UserPostComponent },
    { path: 'Birthdays', component: UserBirthdaysComponent },
    { path: 'MainPage', component: MainPageComponent },
    { path: '**', component: UserBirthdaysComponent }
];
