import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { LoadingService } from '../services/loading.service';
import { MenuComponent } from '../menu/menu.component';
import { FormsModule } from '@angular/forms';
import { LoadingComponent } from '../loading/loading.component';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  standalone:true,
  imports: [MenuComponent, FormsModule, LoadingComponent],
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent {
  fullName: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  address = {
    street: '',
    number: '',
    neighborhood: '',
    city: '',
    state: '',
    country: 'Brasil',
    zipCode: '',
    complement: ''
  };
  errorMessages: string[] = [];
  ufs: string[] = ['AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA', 'MT', 'MS', 'MG', 'PA', 'PB', 'PR', 'PE', 'PI', 'RJ', 'RN', 'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO'];

  constructor(
    private authService: AuthService,
    private router: Router,
    private http: HttpClient,
    private loadingService: LoadingService
  ) {}

  onSubmit() {
    const user = {
      fullName: this.fullName,
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword,
      address: this.address
    };

    this.loadingService.show();

    this.authService.register(user).pipe(
      finalize(() => this.loadingService.hide())
    ).subscribe(
      _ => this.router.navigate(['/users']),
      error => this.errorMessages = error.error.errors.map((t: any) => t.message)
    );
  }

  fetchAddress() {
    const cep = this.address.zipCode.replace(/\D/g, '');
    if (cep.length === 8) {
      this.loadingService.show();
      this.http.get<any>(`https://viacep.com.br/ws/${cep}/json/`).pipe(
        finalize(() => this.loadingService.hide())
      ).subscribe(
        data => {
          if (!data.erro) {
            this.address.street = data.logradouro;
            this.address.neighborhood = data.bairro;
            this.address.city = data.localidade;
            this.address.state = data.uf;
          } else {
            this.errorMessages.push('CEP não encontrado. Digite o seu endereço.');
          }
        },
        error => {
          this.errorMessages.push('CEP não encontrado. Digite o seu endereço.');
        }
      );
    }
  }
}
