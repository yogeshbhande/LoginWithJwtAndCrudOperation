import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ListUsersComponent } from './list-users/list-users.component';
import { AddUserComponent } from './add-user/add-user.component';

const routes: Routes = [
  {
    path : '',
    component : LoginComponent
  },
  {
    path: 'user-list',
    component : ListUsersComponent
  },
  {
    path: 'add-user',
    component : AddUserComponent
  },
  {
    path: 'update-user',
    component : AddUserComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
