import { Routes } from '@angular/router';
import { LayoutsComponent } from './components/layouts/layouts.component';
import { inject } from '@angular/core';
import { AuthService } from './services/auth.service';
import DetailComponent from './components/detail/detail.component';

export const routes: Routes = [
    {
       path: "login",
       loadComponent : ()=> import("./components/login/login.component")
    },
    {
       path: "",
       component: LayoutsComponent,
       canActivateChild: [() => inject(AuthService).checkAuthentication()],
       children: [
    {
        path: "",
        loadComponent: () => import("./components/home/home.component"),
    },

      {
        path: "ticket-detail/:value",
         component: DetailComponent
      }
    ]



}];
