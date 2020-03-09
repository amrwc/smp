import { TestBed } from '@angular/core/testing';

import { GlobalHelper } from './global';
import { CurrentUser } from '../models/current-user';
import { Error } from '../models/error';
import { HttpHeaders } from '@angular/common/http';

describe('GlobalHelper', () => {
  const globalHelper: GlobalHelper = new GlobalHelper();

  const user: CurrentUser = {
    id: 'cuId',
    fullName: 'cuFullName',
    email: 'cu@email.com',
    token: 'cuToken'
  } as CurrentUser;

  const error: Error = {
    key: 'error-key',
    value: 'error-value'
  } as Error;

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

  describe('localStorageItem', () => {
    it('should parse a local storage item and cast to specified type', () => {
      const err = globalHelper.localStorageItem<Error>('error');
      const usr = globalHelper.localStorageItem<CurrentUser>('currentUser');

      expect(err).toEqual(error);
      expect(usr).toEqual(user);
    });
  });

  describe('getAuthHeader', () => {
    const expectedHeaders = new HttpHeaders().set('Authorization', 'Bearer cuToken');

    it('should return the correct value', () => {
      const headers = globalHelper.getAuthHeader();

      expect(headers).toEqual(expectedHeaders);
    });
  });
});
