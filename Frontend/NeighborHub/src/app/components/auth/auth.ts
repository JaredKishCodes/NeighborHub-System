import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

type AuthMode = 'login' | 'register';

@Component({
  selector: 'app-auth',
  imports: [CommonModule, FormsModule],
  templateUrl: './auth.html',
  styleUrl: './auth.css',
})
export class AuthComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  mode: AuthMode = 'login';
  loading = false;
  error: string | null = null;
  darkMode = false;

  loginModel = {
    email: '',
    password: '',
  };

  registerModel = {
    firstName: '',
    lastName: '',
    streetAddress: '',
    city: '',
    baranggay: '',
    email: '',
    password: '',
    confirmPassword: '',
  };

  switchMode(mode: AuthMode): void {
    this.mode = mode;
    this.error = null;
  }

  submitLogin(): void {
    this.error = null;
    if (!this.loginModel.email || !this.loginModel.password) {
      this.error = 'Please enter your email and password.';
      return;
    }

    this.loading = true;
    this.authService.login(this.loginModel).subscribe({
      next: (res) => {
        this.loading = false;
        if (!res.success) {
          this.error = res.message || 'Login failed.';
          return;
        }
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.loading = false;
        // Extracts backend response error message object cleanly if available
        this.error = err?.error?.message || err?.message || 'Login failed.';
        alert(`Login failed: ${this.error}`);
        this.cdr.detectChanges();
      },
    });
  }

  submitRegister(): void {
    this.error = null;
    if (this.registerModel.password !== this.registerModel.confirmPassword) {
      this.error = 'Passwords do not match.';
      return;
    }

    this.authService.register(this.registerModel).subscribe({
      next: (res) => {
        this.loading = false;
        if (!res.success) {
          this.error = res.message || 'Registration failed.';
          return;
          this.cdr.detectChanges();
          
        }
        alert('Registration successful! Please log in with your new account.');

        const registeredEmail = this.registerModel.email;

        // 2. Clear out the registration fields completely
        this.registerModel = {
          firstName: '',
          lastName: '',
          streetAddress: '',
          city: '',
          baranggay: '',
          email: '',
          password: '',
          confirmPassword: '',
        };

        this.loginModel.email = registeredEmail;

        this.mode = 'login';
        this.cdr.detectChanges();


      },
      error: (err) => {
        this.loading = false;
        
        // Fix: Extracts 'User creation failed: Passwords must be...' directly out from the 400 response body object
        this.error = err?.error?.message || err?.message || 'Registration failed.';
        
        console.log("Captured registration error text:", this.error);
        alert(`Registration failed: ${this.error}`);
        this.cdr.detectChanges();
      },
    });
  }
}