import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../../../core/services/auth.service';
import { LoginRequest } from '../../../../core/models/login-request.model';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [ReactiveFormsModule, RouterLink],
    templateUrl: './login.html',
    styleUrl: './login.css'
})
export class Login {

    private fb = inject(FormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);

    errorMessage = '';
    isSubmitting = false;

    loginForm = this.fb.nonNullable.group({
        email: ['', [
            Validators.required,
            Validators.email
        ]],

        password: ['', Validators.required]
    });

    onSubmit(): void {

        if (this.loginForm.invalid) {
            this.loginForm.markAllAsTouched();
            return;
        }

        this.errorMessage = '';
        this.isSubmitting = true;

        const request: LoginRequest =
            this.loginForm.getRawValue();

        this.authService.login(request).subscribe({

            next: () => {
                this.isSubmitting = false;
                this.router.navigate(['/dashboard']);
            },

            error: (error) => {
                this.isSubmitting = false;

                this.errorMessage =
                    error.error?.message ||
                    error.error?.title ||
                    'Invalid email or password.';
            }
        });
    }
}