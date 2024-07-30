import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { MainPageComponent } from './pages/main-page/main-page.component';
import { UserListGetComponent } from './pages/user-list-get/user-list-get.component';
import { UserPostComponent } from './pages/user-post/user-post.component';

const routes: Routes = [
    { path: '', component: AppComponent },
    { path: 'Users', component: UserListGetComponent },
    { path: 'NewUser', component: UserPostComponent },
    { path: 'Birthdays', component: MainPageComponent },
];

@NgModule({
  imports: [
    AppRoutingModule
  ],
  declarations: [
      AppComponent,
      MainPageComponent,
      UserListGetComponent,
      UserPostComponent
  ],
  providers: [],
  bootstrap: [AppComponent],
  exports: []
})
export class AppRoutingModule { }