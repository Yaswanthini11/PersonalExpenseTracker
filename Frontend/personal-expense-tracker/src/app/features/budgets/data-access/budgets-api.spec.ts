import { TestBed } from '@angular/core/testing';
import { BudgetsApi } from './budgets-api';

describe('BudgetsApi', () => {
  let service: BudgetsApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BudgetsApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
