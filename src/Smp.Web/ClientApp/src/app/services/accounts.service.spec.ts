import { TestBed } from '@angular/core/testing';

import { AccountsService } from './accounts.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('AccountsService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientTestingModule],
    providers: [{ provide: 'BASE_URL', useValue: "https://www.smp.org/" }]
  }));

  it('should be created', () => {
    const service: AccountsService = TestBed.get(AccountsService);
    expect(service).toBeTruthy();
  });
});
