import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { SocialLoginModule, SocialAuthServiceConfig, GoogleLoginProvider, FacebookLoginProvider } from '@abacritt/angularx-social-login';
import { MessageService } from 'primeng/api';

export const appConfig: ApplicationConfig = {
  providers: [

    MessageService,
    provideHttpClient(),
    provideRouter(routes),
    DynamicDialogModule,
    importProvidersFrom(
      [
      
        BrowserAnimationsModule,
        SocialLoginModule
      ]),
      {
        provide: 'SocialAuthServiceConfig',
        useValue: {
          autoLogin: false,
          providers: [
            {
              id: GoogleLoginProvider.PROVIDER_ID,
              provider: new GoogleLoginProvider('614018984564-bo6thfiefv9vl679c99oan6bek14o55m.apps.googleusercontent.com')
            },
            {
              id: FacebookLoginProvider.PROVIDER_ID,
              provider: new FacebookLoginProvider('clientId')
            }
          ],
          onError: (err) => {
            console.error(err);
          }
        } as SocialAuthServiceConfig,
      }
  ]
};
