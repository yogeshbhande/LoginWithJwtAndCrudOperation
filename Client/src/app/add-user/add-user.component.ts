import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginService } from '../services/login.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent {
  userForm !: FormGroup;
  editData: any;
  actiontitle: string = 'Add User' ;
  actionBtn: string = 'Save';
  SubmitFlag: boolean = false;
  constructor(private userSerive : LoginService,private fb: FormBuilder,private router : Router,private activateRoute: ActivatedRoute,private matSnackBar: MatSnackBar) { }

  ngOnInit() {
    this.editData = this.activateRoute.snapshot.queryParams;

      if (this.editData['kind'] == 'Add') {
        this.actiontitle = 'Add User Information';
        this.actionBtn = 'Save';
        this.SubmitFlag = true
      } else {
        this.actiontitle = 'Update User Information';
        this.actionBtn = 'Update';
        this.getUserById(this.editData.id)
        // this.getUserById(this.editData.id)
      }

    this.initForm();
  }

  initForm() {
    this.userForm = this.fb.group({
      Username: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      Contact: [],
      Password: ['',Validators.required]
    });
  }

  async getUserById(id: number) {
    const res = await this.userSerive.getUserbyid(id)
    .subscribe({
      next: (res) => {
        if(res){
          this.userForm.controls['Username'].setValue(res.Value['Username']);
          this.userForm.controls['Email'].setValue(res.Value['Email']);
          this.userForm.controls['Contact'].setValue(res.Value['Contact']);
          this.userForm.controls['Password'].setValue(res.Value['Password']);
        }
      },
      error: (err) => {
        alert(err.error.Message)
      }
    })
  }

  //To Update user details
  UpdateUser(){
          this.userSerive.UpdateUser(this.editData.id,this.userForm.getRawValue()).subscribe({
            next:(res)=>{
              this.matSnackBar.open("User Update Successfully", 'Close', {
                duration: 5000
              });
              this.router.navigate(['/user-list']);
            },error: (err) => {
              alert(err.error.Message)
            }             
        })
  }

  //To Add new User
  async onSubmit() {
    if (this.editData['kind'] == "Add"){
      if(this.userForm.valid){
        const formData = this.userForm.value;
        this.userSerive.AddUser(this.userForm.getRawValue()).subscribe({
          next:(res)=>{
            this.matSnackBar.open("User Add Successfully", 'Close', {
              duration: 5000
            });
            this.router.navigate(['/user-list']);
          },error: (err) => {
            alert(err.error.Message)
          }
            
          })
      }
    
   }
  }
}