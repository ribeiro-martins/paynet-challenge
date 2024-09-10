import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { RouterModule } from '@angular/router';
import { LoadingService } from '../services/loading.service';
import { finalize } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  imports: [
    FormsModule,
    RouterModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  standalone: true,
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  email: string = '';
  newPassword: string = '';
  verifyingPassword: boolean = false;
  secretCode: string = '';
  errorMessages: string[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private loadingService: LoadingService,
    private toastr: ToastrService
  ) { }

  forgotPassword() {
    this.loadingService.show();

    this.authService.forgotPassword({ email: this.email, newPassword: this.newPassword }).pipe(
      finalize(() => {
        this.loadingService.hide();
      })
    ).subscribe(
      response => {
        this.verifyingPassword = true;
      },
      error => {
        this.errorMessages = error.error.errors.map((t: any) => t.message);
      }
    );
  }

  verifyPassword() {
    this.loadingService.show();

    this.authService.verifyPassword({ email: this.email, secretCode: this.secretCode }).pipe(
      finalize(() => {
        this.loadingService.hide();
      })
    ).subscribe(
      _ => {
        this.toastr.success('Senha alterada com sucesso! Redirecionando para a página de login')
        this.router.navigate(['/login']);
        localStorage.removeItem('token');
      },
      error => {
        this.errorMessages = error.error.errors.map((t: any) => t.message);
      }
    );
  }
}
