export interface AuthResponse {
    token: string;
    expiresAt: string;
    userId: string;
    fullName: string;
    email: string;
}