import { Component, inject } from '@angular/core';
import {
    FormBuilder,
    ReactiveFormsModule,
    Validators,
    AbstractControl,
    ValidationErrors
} from '@angular/forms';

import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { RegisterRequest } from '../../../../core/models/register-request.model';

function passwordMatchValidator(
    control: AbstractControl
): ValidationErrors | null {

    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    return password === confirmPassword
        ? null
        : { passwordMismatch: true };
}

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [ReactiveFormsModule, RouterLink],
    templateUrl: './register.html',
    styleUrl: './register.css'
})
export class Register {

    private fb = inject(FormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);

    errorMessage = '';
    isSubmitting = false;

    registerForm = this.fb.nonNullable.group(
        {
            fullName: ['', Validators.required],

            email: ['', [
                Validators.required,
                Validators.email
            ]],

            password: ['', [
                Validators.required,
                Validators.minLength(8)
            ]],

            confirmPassword: ['', Validators.required]
        },
        {
            validators: passwordMatchValidator
        }
    );

    onSubmit(): void {

        if (this.registerForm.invalid) {
            this.registerForm.markAllAsTouched();
            return;
        }

        this.errorMessage = '';
        this.isSubmitting = true;

        const request: RegisterRequest =
            this.registerForm.getRawValue();

        this.authService.register(request).subscribe({
            next: () => {
                this.isSubmitting = false;
                this.router.navigate(['/dashboard']);
            },

            error: (error) => {
                this.isSubmitting = false;

                this.errorMessage =
                    error.error?.message ||
                    error.error?.title ||
                    'Registration failed. Please check your details.';
            }
        });
    }
}