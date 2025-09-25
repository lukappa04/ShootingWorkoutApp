import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  model: any = {};

  register(){
    console.log(this.model);
  }

  cancel(){
    console.log("cancelled");
  }

  protected readonly screen = screen;
}
