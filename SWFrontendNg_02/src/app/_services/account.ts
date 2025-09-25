import {inject, Injectable, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../Environments/environments';
import {API_ENDPOINTS} from '../EndPoint/endpoints';
import {User} from '../_model/user';
import {map} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Account {
  private http = inject(HttpClient);
  baseUrl = environment.baseUrl;
  currentUser = signal<User | null>(null);

  login(model: any){
    return this.http.post<User>(`${this.baseUrl}${API_ENDPOINTS.login}`, model).pipe(
      map(user => {
        if(user){
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUser.set(user)
        }
      })
    );
  }

  logout(){
    localStorage.removeItem('currentUser');
    this.currentUser.set(null);
  }
}
