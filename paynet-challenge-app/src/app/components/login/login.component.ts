import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { LoadingService } from '../../services/loading.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    RouterModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessages: string[] = [];

  constructor(private authService: AuthService, private router: Router, private loadingService: LoadingService) { }

  onSubmit() {
    this.loadingService.show();

    this.authService.login({ email: this.email, password: this.password }).pipe(
      finalize(() => {
        this.loadingService.hide();
      })
    ).subscribe(
      response => {
        localStorage.setItem('token', response.accessToken);
        this.router.navigate(['/users']);
      },
      error => {
        this.errorMessages = error.error.errors.map((t: any) => t.message);
      }
    );
  }
}
