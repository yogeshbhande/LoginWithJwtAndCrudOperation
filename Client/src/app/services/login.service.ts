import { HttpClient, HttpHeaders, HttpRequest  } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private apiUrl = 'http://localhost:7145/api/JWTToken';
  private authToken !: string | null;
  constructor(private http : HttpClient) {
   }

   private getRequestHeaders(): HttpHeaders {
    let headers = new HttpHeaders();
    this.authToken = sessionStorage['jwtToken'];
    if (this.authToken) {
      headers = headers.set('Authorization', `Bearer ${this.authToken}`);
    }
    return headers;
  }

  login(data :any)
  {
    return this.http.post(this.apiUrl+'/Login'+"?Username="+data.Username+"&Password="+data.Password, {responseType:"json"});
  }

  getAllUser() {
    const headers = this.getRequestHeaders();
    return this.http.get<any>(this.apiUrl +'/GetAllUsers', { headers });
  }

  getUserbyid(id: any) {
    const headers = this.getRequestHeaders();
    return this.http.get<any>(this.apiUrl +'/GetUserById'+"?id="+id, { headers });
  }

  AddUser(data :any){
    const headers = this.getRequestHeaders();
    return this.http.post<any>(this.apiUrl+'/AddUser',data, { headers });
  }

  UpdateUser(id: any,data :any){
    const headers = this.getRequestHeaders();
    return this.http.put<any>(this.apiUrl+'/UpdateUser'+"?id="+id , data, { headers });
  }

  DeleteUser(id : any){
    const headers = this.getRequestHeaders();
    return this.http.delete<any>(this.apiUrl +'/DeleteUser'+"?id="+id, { headers });
  }


}
