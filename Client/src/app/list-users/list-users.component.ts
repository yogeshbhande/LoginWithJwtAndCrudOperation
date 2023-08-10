import { Component } from '@angular/core';
import { LoginService } from '../services/login.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-list-users',
  templateUrl: './list-users.component.html',
  styleUrls: ['./list-users.component.css']
})
export class ListUsersComponent {
  displayedColumns: string[] = ['No', 'username', 'contact', 'email','actions'];
  dataSource = new MatTableDataSource();
  constructor(private userService : LoginService, private router: Router,private matSnackBar: MatSnackBar){}

  ngOnInit(){
    this.getAllUsers();
  }

  //Get All user list
  async getAllUsers() {
    await this.userService.getAllUser()
      .subscribe({
        next: (res) => {
          this.dataSource = new MatTableDataSource(res.Value);
        },
        error: (err) => {
          alert(err.error.Message)
        }
      })
  }

  //To open Add user Component
  openAddUser(){
    let data = {
      'kind' : 'Add'
    }
    this.router.navigate(['/add-user'],{ queryParams: data });
  }

  //To update user details
  editUser(UserId: number) {
    // Implement the logic to handle the edit action here
    let data = {
      'kind' : 'Update',
      'id' : UserId
    }
    this.router.navigate(['/update-user'],{ queryParams: data });
  }

  async deleteUser(id: any) {
    // Implement the logic to handle the delete action here
    const res = await this.userService.DeleteUser(id)
    .subscribe({
      next: (res) => {
        this.getAllUsers()
        this.matSnackBar.open("User Deleted Successfully", 'Close', {
          duration: 5000
        });
      },
      error: (err) => {
        alert(err.error.Message)
      }
    })
  }
}
