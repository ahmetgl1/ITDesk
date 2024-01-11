import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { InputGroupModule } from 'primeng/inputgroup';
import { DividerModule } from 'primeng/divider';
import { PasswordModule } from 'primeng/password';
import { FormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { CheckboxModule } from 'primeng/checkbox';
import { Router } from '@angular/router';
import { LoginRequest } from '../../models/request.model';
import { GoogleSigninButtonModule, SocialAuthService } from '@abacritt/angularx-social-login';
import { ErrorService } from '../../services/error.service';
import { HttpService } from '../../services/http.service';



@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CardModule , ButtonModule,
            InputTextModule, CommonModule,
            InputGroupModule, DividerModule,
            PasswordModule, FormsModule , 
            ToastModule, CheckboxModule,
             FormsModule, GoogleSigninButtonModule ],
  templateUrl: './login.component.html',
  providers: [],
  styleUrl: './login.component.css'
})
export default class LoginComponent implements OnInit{

  constructor(private messageService: MessageService ,
              private http : HttpService , 
              private router: Router,
              private auth: SocialAuthService,
             
                ) {}



  ngOnInit(): void {
   
this.auth.authState.subscribe(res => {



  this.http.post(`Auth/GoogleLogin` , res , (data) => {

    localStorage.setItem('response' , JSON.stringify(data))
    this.router.navigateByUrl('/')
  })

       })


  }

request: LoginRequest =  new LoginRequest()

signIn(){

  // if(this.request.userNameOrEmail.length < 3){

  //   this.messageService.add({ severity: 'error', summary: 'Validasyon Hatası', detail: 'Kullanıcı Adınız veye Emailiniz 3 haneden az uzunlukta olamaz' });
  //  return;

  // }
  // if(this.request.password.length <5){
  //   this.messageService.add({ severity: 'error', summary: 'Validasyon Hatası', detail: 'Şifreniz 5 haneden az uzunlukta olamaz' });

  // }

this.http.post(`Auth/Login`, this.request , (res) =>{

  localStorage.setItem('response' , JSON.stringify(res))
  this.router.navigateByUrl('/')
})
    }






}
