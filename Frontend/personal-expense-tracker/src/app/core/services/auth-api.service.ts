import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { RegisterRequest } from '../models/register-request.model';
import { LoginRequest } from '../models/login-request.model';
import { AuthResponse } from '../models/auth-response.model';
import { CurrentUser } from '../models/current-user.model';

@Injectable({
    providedIn: 'root'
})
export class AuthApiService {

    private http = inject(HttpClient);

    // Replace PORT with your backend HTTPS port.
   private readonly baseUrl = 'https://localhost:7062/api/auth';

    register(request: RegisterRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(
            `${this.baseUrl}/register`,
            request
        );
    }

    login(request: LoginRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(
            `${this.baseUrl}/login`,
            request
        );
    }

    getCurrentUser(): Observable<CurrentUser> {
        return this.http.get<CurrentUser>(
            `${this.baseUrl}/me`
        );
    }
}