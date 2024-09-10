import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { jwtDecode } from "jwt-decode";
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  login(credentials: { email: string, password: string }): Observable<any> {
    return this.http.post(`${environment.apiUrl}/auth/login`, credentials);
  }

  forgotPassword(forgotPasswordForm: { email: string, newPassword: string }): Observable<any> {
    return this.http.post(`${environment.apiUrl}/auth/forgot-password`, forgotPasswordForm);
  }

  verifyPassword(verifyPasswordForm: { email: string, secretCode: string }): Observable<any> {
    return this.http.post(`${environment.apiUrl}/auth/verify-forgot-password`, verifyPasswordForm);
  }

  register(user:any): Observable<any> {
    return this.http.post(`${environment.apiUrl}/users`, user);
  }

  getUsers(): Observable<any> {
    return this.http.get(`${environment.apiUrl}/users`);
  }

  getUserInfoFromToken(): any {
    const token = localStorage.getItem('token');
    const decodedToken = this.getDecodedToken(token);
    if (decodedToken) {
      return {
        name: decodedToken.name,
        email: decodedToken.email,
      };
    }
    return null;
  }

  getDecodedToken(token: any): any {
    try {
      return jwtDecode(token);
    } catch (Error) {
      console.error('Invalid token:', Error);
      return null;
    }
  }
}
