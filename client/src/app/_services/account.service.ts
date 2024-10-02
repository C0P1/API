import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  baseurl = "http://localhost:5000/api/"

  login(model : any): Observable<Object>{
    return this.http.post(this.baseurl + "account/login",model);
  }
}
