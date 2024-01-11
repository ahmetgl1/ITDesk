import { Component } from '@angular/core';
import { CardModule } from 'primeng/card';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { HttpService } from '../../services/http.service';
import { TicketDetail } from '../../models/ticketdetail.model';
import { SendMessage } from '../../models/message.model';
import { FormsModule } from '@angular/forms';
import { TicketModel } from '../../models/ticket.model';
import { AvatarModule } from 'primeng/avatar';
import { AvatarGroupModule } from 'primeng/avatargroup';
import { ButtonModule } from 'primeng/button';



@Component({
  selector: 'app-detail',
  standalone: true,
  imports: [CardModule, CommonModule, FormsModule, AvatarModule, AvatarGroupModule, ButtonModule],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.css'
})
export default class DetailComponent {

  ticketId: string =""
  content: string = ""
  details: TicketDetail[] = []
  ticket: TicketModel = new TicketModel()
   message: SendMessage=   new SendMessage()



  constructor(public auth: AuthService ,
              private activated: ActivatedRoute,
              private http: HttpService) {

     this.activated.params.subscribe(  (res) => {

      this.ticketId = res["value"]

  })

  this.getDetail()
  this.getTicket()

}

getDetail() {

  this.http.get(`/Ticket/GetDetail/${this.ticketId}`, (res) => { 

    this.details = res
    console.log(this.details);


    
  })
  
}

sendMessage(){

  this.message.appUserId = this.auth.token.userId
  this.message.ticketId = this.ticketId
 this.content =  this.message.content
  this.http.post("Ticket/SendMessage", this.message , () => {

    console.log(this.content);
    
    this.getDetail()
  })

}

getTicket(){

  this.http.get(`/Ticket/GetByDetail/${this.ticketId}`, (res) => {

    this.ticket = res
  console.log(this.ticket);
  
  })
}

getAvatarLetter(firstName: string): string {
  if (firstName && firstName.trim() !== '') {
    return firstName.charAt(0).toUpperCase(); 
  } else {
    return '?'; 
  }
}

ChangeTicketStatus(ticketId: string){

  this.http.get(`/Ticket/ChangeTicketStatus/${ticketId}` , () => { 

this.getTicket()




  })
}









     }
