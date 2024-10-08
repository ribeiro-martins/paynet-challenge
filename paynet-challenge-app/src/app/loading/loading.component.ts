import { Component, OnInit } from '@angular/core';
import { LoadingService } from '../services/loading.service';

@Component({
  selector: 'app-loading',
  standalone: true,
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css']
})
export class LoadingComponent implements OnInit {
  isLoading: boolean = false;

  constructor(private loadingService: LoadingService) {}

  ngOnInit(): void {
    this.loadingService.loading$.subscribe(loading => {
      this.isLoading = loading;
    });
  }
}
