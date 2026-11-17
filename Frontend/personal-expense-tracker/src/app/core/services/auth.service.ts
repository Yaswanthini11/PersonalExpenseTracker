import { Injectable, inject, signal, computed } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { AuthApiService } from './auth-api.service';
import { RegisterRequest } from '../models/register-request.model';
import { LoginRequest } from '../models/login-request.model';
import { AuthResponse } from '../models/auth-response.model';
import { CurrentUser } from '../models/current-user.model';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private authApi = inject(AuthApiService);

    private token = signal<string | null>(
        sessionStorage.getItem('authToken')
    );

    currentUser = signal<CurrentUser | null>(null);

    isAuthenticated = computed(() => this.token() !== null);

    register(request: RegisterRequest): Observable<AuthResponse> {
        return this.authApi.register(request).pipe(
            tap(response => this.saveToken(response))
        );
    }

    login(request: LoginRequest): Observable<AuthResponse> {
        return this.authApi.login(request).pipe(
            tap(response => this.saveToken(response))
        );
    }

    loadCurrentUser(): Observable<CurrentUser> {
        return this.authApi.getCurrentUser().pipe(
            tap(user => this.currentUser.set(user))
        );
    }

    getToken(): string | null {
        return this.token();
    }

    logout(): void {
        sessionStorage.removeItem('authToken');

        this.token.set(null);
        this.currentUser.set(null);
    }

    private saveToken(response: AuthResponse): void {
        sessionStorage.setItem('authToken', response.token);
        this.token.set(response.token);

        this.currentUser.set({
            userId: response.userId,
            fullName: response.fullName,
            email: response.email,
            phoneNumber: null
        });
    }
}