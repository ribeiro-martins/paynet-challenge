import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { LoadingService } from '../../services/loading.service';
import { finalize } from 'rxjs';
import { MenuComponent } from '../../menu/menu.component';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  imports: [CommonModule, MenuComponent],
  standalone: true,
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  users: any[] = [];

  constructor(private authService: AuthService, private loadingService: LoadingService) { }

  ngOnInit() {
    this.loadingService.show();

    this.authService.getUsers().pipe(
      finalize(() => {
        this.loadingService.hide();
      })
    ).subscribe(
      response => {
        this.users = response.users;
      },
      error => {
        console.error('Failed to fetch users', error);
      }
    );
  }
}
