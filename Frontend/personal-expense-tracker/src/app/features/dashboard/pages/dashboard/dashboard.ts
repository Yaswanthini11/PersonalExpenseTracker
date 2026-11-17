
import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../../../core/services/auth.service';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [],
    templateUrl: './dashboard.html',
    styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

    // Dependency injection
    authService = inject(AuthService);
    private router = inject(Router);

    // UI state
    isLoading = signal(true);
    errorMessage = signal('');

    ngOnInit(): void {

        this.authService.loadCurrentUser().subscribe({

            next: (user) => {

                console.log('User details received:', user);

                console.log(
                    'Current user signal:',
                    this.authService.currentUser()
                );

                this.isLoading.set(false);
                this.errorMessage.set('');

            },

            error: (error) => {

                console.error('Failed to load user:', error);

                this.isLoading.set(false);
                this.errorMessage.set(
                    'Unable to load your profile. Please login again.'
                );

                this.authService.logout();
                this.router.navigate(['/login']);

            }

        });

    }

    logout(): void {

        this.authService.logout();

        this.router.navigate(['/login']);

    }

}
