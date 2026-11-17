
import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [RouterLink],
    templateUrl: './dashboard.html',
    styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

    private authService = inject(AuthService);
    private router = inject(Router);

    get currentUser() {
        return this.authService.currentUser;
    }

    ngOnInit(): void {
        this.authService.loadCurrentUser().subscribe({
            error: () => {
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
