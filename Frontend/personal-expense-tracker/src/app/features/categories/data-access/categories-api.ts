
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
    Category,
    CreateCategoryRequest,
    UpdateCategoryRequest
} from '../models/category.model';

@Injectable({
    providedIn: 'root'
})
export class CategoriesApiService {

    private http = inject(HttpClient);

    private readonly baseUrl =
        'https://localhost:7062/api/categories';

    // GET: api/categories
    getAll(): Observable<Category[]> {
        return this.http.get<Category[]>(this.baseUrl);
    }

    // POST: api/categories
    create(request: CreateCategoryRequest): Observable<Category> {
        return this.http.post<Category>(this.baseUrl, request);
    }

    // PUT: api/categories/{id}
    update(
        categoryId: number,
        request: UpdateCategoryRequest
    ): Observable<Category> {
        return this.http.put<Category>(
            `${this.baseUrl}/${categoryId}`,
            request
        );
    }

    // DELETE: api/categories/{id}
    delete(categoryId: number): Observable<void> {
        return this.http.delete<void>(
            `${this.baseUrl}/${categoryId}`
        );
    }
}

