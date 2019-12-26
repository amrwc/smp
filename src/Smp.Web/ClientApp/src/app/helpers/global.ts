import { Injectable, Inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GlobalHelper {

  public localStorageItem<T>(key: string): T {
    return <T> JSON.parse(localStorage.getItem(key));
  }
}
