import { CanActivateFn } from '@angular/router';
import {inject} from '@angular/core';
import {Account} from '../_services/account';
import {toast} from 'ngx-sonner';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(Account);
  if(accountService.currentUser()) {
    return true
  }else{
    toast.error('You are not logged in!',{
      position: 'top-right',
      duration: 4000
    });
    return false;
  }
};
