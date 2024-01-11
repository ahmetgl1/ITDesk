import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MessageService } from 'primeng/api';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogRef } from 'primeng/dynamicdialog';


interface UploadEvent {
  originalEvent: Event;
  files: File[];
}


@Component({
  selector: 'app-creates',
  standalone: true,
  imports: [InputTextModule, FormsModule, FileUploadModule, ToastModule , InputTextareaModule , CommonModule ,ButtonModule],
  templateUrl: './creates.component.html',
  styleUrl: './creates.component.css'
})


export class CreatesComponent {


  uploadedFiles: any[] = [];

  constructor(private messageService: MessageService , private dialog: DynamicDialogRef) {}


  subject: string = ""
  summary: string = ""

  onUpload(event:any) {
    for(let file of event.files) {
        this.uploadedFiles.push(file);
    }

    this.messageService.add({severity: 'info', summary: 'File Uploaded', detail: ''});
}


create(){

if(this.subject == ""){
  this.messageService.add({severity:'error', summary: 'Konu Alanı Boş Olamaz'});

  return;
}
if(this.summary == ""){
  this.messageService.add({severity:'warn', summary: 'Detay Alanı Boş Olamaz'});
return
}




  const formData = new FormData()

  formData.append("subject" , this.subject)
  formData.append("summary" , this.summary)

  for(let file of this.uploadedFiles){
    formData.append("files" , file, file.name)

  }

  this.dialog.close(formData)
}




}
