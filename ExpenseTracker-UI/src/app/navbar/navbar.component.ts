import { Component } from '@angular/core';
import { GlobalService } from '../services/global.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  constructor(public service: GlobalService, private router: Router) {
    if(localStorage.getItem('token')) {
			this.service.setLogIn(true);
		}    
  }

  logout() {
    localStorage.removeItem('token');
		this.service.setLogIn(false);
    this.router.navigate(['/login']);
  }
}
