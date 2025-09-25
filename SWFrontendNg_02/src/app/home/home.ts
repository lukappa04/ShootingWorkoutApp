import { Component } from '@angular/core';
import {Register} from '../register/register';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    Register
  ],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  registerMode = false;

  registerToggle(){
    this.registerMode = !this.registerMode;
  }
}
