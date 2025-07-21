import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Post } from '../models/post.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient, private router: Router) { }

  login() {
    // Backend will handle redirect to Azure
    window.location.href = 'https://localhost:5000/auth/login-azure?returnUrl=http://localhost:4200/profile';
  }

  getProfile() {
    return this.http.get<any>('https://localhost:5000/auth/me', {
      withCredentials: true
    });
  }

  logout() {
    window.location.href = 'https://localhost:5000/auth/logout';
  }

  getPosts(): Observable<Post[]> {
    return this.http.get<Post[]>('https://localhost:5000/auth/posts');
  }
}
