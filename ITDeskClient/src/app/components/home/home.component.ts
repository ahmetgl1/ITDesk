import { Component, OnInit } from '@angular/core';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import {DialogService, DynamicDialogModule, DynamicDialogRef} from 'primeng/dynamicdialog';
import { MessageService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import { CreatesComponent } from '../creates/creates.component';
import { TicketModel } from '../../models/ticket.model';
import { HttpService } from '../../services/http.service';
import { AgGridModule } from 'ag-grid-angular';
import { BadgeModule } from 'primeng/badge';
import { AuthService } from '../../services/auth.service';



@Component({
  selector: 'app-home',
  standalone: true,
  imports: [TableModule , TagModule ,
            InputTextModule , ButtonModule ,
            DynamicDialogModule , DialogModule,
             AgGridModule, BadgeModule ],

   providers: [ MessageService , DialogModule, DialogService],
   templateUrl: './home.component.html',
   styleUrl: './home.component.css'
}) 
export default class HomeComponent implements OnInit  {

    frameworkComponents: any
    selectedSubject!: any;
    
    visible: boolean = false;
    
    ref: DynamicDialogRef | undefined;

   tickets: TicketModel[] = []


 defaultColDef: any =  { 
    filter: true,
    resizable: true
} 

autoSizeStrategy :  any = { 
    type: 'fitGridWidth',
    defaultWidth: 100
}


   colDefs: any[] = [
    { field: "#" , valueGetter : (params: any) => params.node.rowIndex +1 , width: 30 },

    { field: "subject"  , cellRenderer: (params: any) => {
        return `<a href = "/ticket-detail/${params.data.id}">${params.value}</a>`
    }
  },
  
    { field: "isOpen" , cellRenderer: (params: any) => {

             if(params.value){

                //OBJECT OBJECT KISMI DÜZELTİLEECEK
                return params.value ? '<span ng-reflect-ng-class="[object Object]" severity="success" class="p-badge p-component p-badge-lg p-badge-success ng-star-inserted">Açık</span>' : '<span ng-reflect-ng-class="[object Object]" severity="danger" class="p-badge p-component p-badge-lg p-badge-danger ng-star-inserted">Kapalı</span>';
            }
             else{
                return params.value ? '<span ng-reflect-ng-class="[object Object]" severity="error" class="p-badge p-component p-badge-lg p-badge-success ng-star-inserted">Kapakı</span>' : '<span ng-reflect-ng-class="[object Object]" severity="danger" class="p-badge p-component p-badge-lg p-badge-danger ng-star-inserted">Kapalı</span>';
            }


    }},
    { field: "createdDate" , valueFormatter : (params: any) => {
        return new Date(params.value).toLocaleDateString('tr-TR',  {day: '2-digit', 
                                                                   month: '2-digit',
                                                                   year: 'numeric',
                                                                   hour: 'numeric',
                                                                   minute: 'numeric' })
    } }
  ];




        constructor(private messageService: MessageService ,
                public dialogService: DialogService,
                private http: HttpService,
                private auth: AuthService
                ) {    }


    ngOnInit(): void {
       
        this.GetAllListById()
    }


    GetAllListById() {


    const data = {
            roles: this.auth.token.roles
    }

        this.http.post('Ticket/GetAllByUserId', data ,(res) => {  
            this.tickets = []

           for(var r  of res){
 
            const ticket =  new TicketModel()
            
            ticket.id = r.id
            ticket.subject = r.subject
            ticket.isOpen = r.isOpen
            ticket.createdDate =  r.createdDate


            this.tickets.push(ticket)
           }



        });
    }
    
      
  //method ismi değiştirilecek !

  hello() {
        this.ref = this.dialogService.open(CreatesComponent, {
            header: 'Yeni Destek Oluştur',
            width: '40%',
            baseZIndex: 10000,
            maximizable: false,
            data: {

            }
        } );

        this.ref.onClose.subscribe((data: any) => {
            
            if (data) {
                console.log(data);

                this.http.post("Ticket/CreateTicket" , data , (res) => {
                    
                    this.GetAllListById()
                    this.messageService.add({severity:'info', summary: 'Destek Talebi Alındı'});
  
                })
            }
        });



        this.ref.onMaximize.subscribe((value: any) => {
            this.messageService.add({severity: 'info', summary: 'Maximized', detail:  `maximized: ${value.maximized}`});
    });
    }

    ngOnDestroy() {
        if (this.ref) {
            this.ref.close();
        }
    }


    
    goToDetail(event: any){

          console.log(event.rowData);



    }

    







}






