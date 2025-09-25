import {Component, inject, OnInit, signal} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {environment} from '../Environments/environments';
import {API_ENDPOINTS} from './EndPoint/endpoints';
import {Nav} from './nav/nav';
import {Account} from './_services/account';
import {Home} from './home/home';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, Nav, Home],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  http = inject(HttpClient);
  protected readonly title = 'SWFrontendNg_02';
  private accountService = inject(Account);
  users: any;

  baseUrl = environment.baseUrl;

  ngOnInit() {
    this.getUsers();
    this.setCurrentUser();
  }

  getUsers(){
    this.http.get(`${this.baseUrl}${API_ENDPOINTS.getAll}` ).subscribe({
      next: data => {this.users = data},
      error: error => {console.log(error)},
      complete: () => {console.log("Request has completed successfully.")}
    })
  }
  setCurrentUser(){
    const userString = localStorage.getItem('currentUser');
    if(!userString){
      return;
    }
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }
}
