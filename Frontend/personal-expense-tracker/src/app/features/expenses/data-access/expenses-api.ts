import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
    Expense,
    CreateExpenseRequest,
    UpdateExpenseRequest,
    ExpenseFilters
} from '../models/expense.model';

@Injectable({
    providedIn: 'root'
})
export class ExpensesApiService {

    private http = inject(HttpClient);

    private readonly baseUrl =
        'https://localhost:7062/api/expenses';

    // Get all expenses with optional filters
    getAll(filters?: ExpenseFilters): Observable<Expense[]> {

        let params = new HttpParams();

        if (filters?.month !== undefined) {
            params = params.set('month', filters.month);
        }

        if (filters?.year !== undefined) {
            params = params.set('year', filters.year);
        }

        if (filters?.categoryId !== undefined) {
            params = params.set('categoryId', filters.categoryId);
        }

        return this.http.get<Expense[]>(this.baseUrl, { params });
    }

    // Get expense by ID
    getById(expenseId: number): Observable<Expense> {
        return this.http.get<Expense>(
            `${this.baseUrl}/${expenseId}`
        );
    }

    // Create expense
    create(request: CreateExpenseRequest): Observable<Expense> {
        return this.http.post<Expense>(
            this.baseUrl,
            request
        );
    }

    // Update expense
    update(
        expenseId: number,
        request: UpdateExpenseRequest
    ): Observable<Expense> {

        return this.http.put<Expense>(
            `${this.baseUrl}/${expenseId}`,
            request
        );
    }

    // Delete expense
    delete(expenseId: number): Observable<void> {
        return this.http.delete<void>(
            `${this.baseUrl}/${expenseId}`
        );
    }
}