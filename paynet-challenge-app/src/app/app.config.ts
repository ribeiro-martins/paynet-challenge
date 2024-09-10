import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { TokenInterceptor } from './interceptors/token.service';
import { BrowserModule } from '@angular/platform-browser';
import { LoadingComponent } from './loading/loading.component';
import { provideToastr } from 'ngx-toastr';
import { provideAnimations } from "@angular/platform-browser/animations";

export const appConfig: ApplicationConfig = {
  providers: [
    BrowserModule,
    LoadingComponent,
    provideRouter(routes),
    provideHttpClient(),
    provideToastr(),
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }]
};
