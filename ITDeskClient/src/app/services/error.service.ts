import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';


@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  constructor(private messageService: MessageService) { }


errorHandler(err: HttpErrorResponse){


  switch (err.status) {
    case 400:
  
    this.messageService.add({severity: 'warn' , summary: "Error", detail: err.error})
      break;

      case 401:
        this.messageService.add({severity: 'warn' , summary: "Authoriation Hatası", detail: err.error})
break;
      case 0:
      this.messageService.add({severity: 'error' , summary: "SERVER ERROR ", detail: 'Daha Sonra Giriş Yapın '})
      break;
  
  
    case 422 :
  
      for(let e of err.error){
      
       this.messageService.add({severity: 'error' , summary: "Validasyon Hatası", detail: e})
      }
      break;
  }




}








}
