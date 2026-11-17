
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import type {
    Budget,
    CreateBudgetRequest,
    UpdateBudgetRequest
} from '../models/budget.model';

@Injectable({
    providedIn: 'root'
})
export class BudgetsApiService {

    private http = inject(HttpClient);

    private readonly baseUrl =
        'https://localhost:7062/api/budgets';

    getBudget(year: number, month: number): Observable<Budget> {

        const params = new HttpParams()
            .set('year', year)
            .set('month', month);

        return this.http.get<Budget>(this.baseUrl, { params });
    }

    createBudget(request: CreateBudgetRequest): Observable<Budget> {
        return this.http.post<Budget>(this.baseUrl, request);
    }

    updateBudget(request: UpdateBudgetRequest): Observable<Budget> {
        return this.http.put<Budget>(this.baseUrl, request);
    }
}
