import { Component,  OnInit } from '@angular/core';
import { MenubarModule } from 'primeng/menubar';
import { MenuItem } from 'primeng/api';
import { Router, RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../services/auth.service';


@Component({
  selector: 'app-layouts',
  standalone: true,
  imports: [MenubarModule , RouterOutlet, ButtonModule],
  templateUrl: './layouts.component.html',
  styleUrl: './layouts.component.css'
})
export class LayoutsComponent implements OnInit {

  constructor(private router : Router,
              public auth: AuthService) {
    

  }



  items: MenuItem[] | undefined;

    ngOnInit() {
        this.items = [
            {
                label: 'Ana Sayfa',
                icon: 'pi pi-fw pi-home',
                routerLink: "/"
            }
        ];
    }



    logOut(){
      
      localStorage.removeItem('response')
      this.router.navigateByUrl('login')
      location.reload()
 




    }

}
