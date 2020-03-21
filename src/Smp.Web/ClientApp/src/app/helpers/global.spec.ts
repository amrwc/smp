import { HttpHeaders } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { CurrentUser } from '../models/current-user';
import { Error } from '../models/error';
import { GlobalHelper } from './global';

describe('GlobalHelper', () => {
  const error: Error = {
    key: 'error-key',
    value: 'error-value',
  } as Error;
  const globalHelper: GlobalHelper = new GlobalHelper();
  const user: CurrentUser = {
    id: 'cuId',
    fullName: 'cuFullName',
    email: 'cu@email.com',
    token: 'cuToken',
  } as CurrentUser;

  beforeAll(() => {
    localStorage.setItem('currentUser', JSON.stringify(user));
    localStorage.setItem('error', JSON.stringify(error));
  });

  afterAll(() => {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('error');
  });

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  describe('localStorageItem()', () => {
    it('should have parsed a local storage item and casted to specified type', () => {
      const err: Error = globalHelper.localStorageItem<Error>('error');
      const usr: CurrentUser = globalHelper.localStorageItem<CurrentUser>('currentUser');
      expect(err).toEqual(error);
      expect(usr).toEqual(user);
    });
  });

  describe('getAuthHeader()', () => {
    const expectedHeaders: HttpHeaders = new HttpHeaders().set('Authorization', 'Bearer cuToken');

    it('should have returned the correct value', () => {
      const headers = globalHelper.getAuthHeader();
      expect(headers).toEqual(expectedHeaders);
    });
  });
});
