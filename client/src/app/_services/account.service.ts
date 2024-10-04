import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  baseurl = "https://localhost:5001/api/"
  currentUser = signal<User | null>(null);
  
  login(model : any):  Observable<User | void>{
    return this.http.post<User>(this.baseurl + "account/login",model).pipe(
      map((user) =>{
        if (user){
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUser.set(user);
        }
      })
    );
  }

  logout(): void{
    localStorage.removeItem("user");
    this.currentUser.set(null);
  }
}
