import {Component, inject} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {Account} from '../_services/account';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav {
//TODO: Check why drop down doesn't work. Should i implement some libraries?
  accountService = inject(Account);
  model: any = {};

  login(){
    this.accountService.login(this.model).subscribe({
      next: (result) => {
        console.log(result);
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('Logged in');
      }
    })
  }

  logout(){
    this.accountService.logout();
  }
}
