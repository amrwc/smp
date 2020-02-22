import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GlobalHelper {

  public localStorageItem<T>(key: string): T {
    return JSON.parse(localStorage.getItem(key)) as T;
  }

  public getAuthHeader(): HttpHeaders {
    return new HttpHeaders().set('Authorization', 'Bearer ' + JSON.parse(localStorage.getItem('currentUser')).token);
  }
}
