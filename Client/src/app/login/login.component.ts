import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  formGroup!: FormGroup;
  successData : any;
  userList : any;
  userById : any;
  JwtToken: any;
  constructor(private _loginService : LoginService, private fb: FormBuilder,private router: Router){ }
  ngOnInit() {
    this.initForm();
  }

  initForm(){
    this.formGroup = this.fb.group({
      Username : new FormControl('',[Validators.required]),
      Password : new FormControl('',[Validators.required])
    })
  }

  //Login 
  loginProcess(){
    if(this.formGroup.valid){
      this._loginService.login(this.formGroup.getRawValue()).subscribe({
        next:(res)=>{
          this.successData=res;
          // this.matSnackBar.open("User Deleted Successfully", 'Close', {
          //   duration: 5000
          // });
          sessionStorage.setItem('jwtToken', this.successData.token);
          this.GoToUserList();
        },error: (err) => {
          alert(err.error.Message)
        }         
        })
    }
  }

  // async getUserById(id: number) {
  //   const res = await this._loginService.getUserbyid(id)
  //   .subscribe({
  //     next: (res) => {
  //       this.userById = res;
  //     },
  //     error: (err) => {
  //       alert(err.error.Message)
  //     }
  //   })
  // }

  GoToUserList(){
    this.router.navigate(['/user-list']);
  }
  
}
