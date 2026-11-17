
import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

import { BudgetsApiService } from '../../data-access/budgets-api';

import type {
    Budget,
    CreateBudgetRequest,
    UpdateBudgetRequest
} from '../../models/budget.model';

@Component({
    selector: 'app-budgets',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './budgets.html',
    styleUrl: './budgets.css'
})
export class Budgets implements OnInit {

    private budgetsApi = inject(BudgetsApiService);
    private fb = inject(FormBuilder);

    budget = signal<Budget | null>(null);

    loading = signal(false);
    saving = signal(false);

    error = signal('');
    success = signal('');

    isEditing = signal(false);

    selectedMonth = signal(new Date().getMonth() + 1);
    selectedYear = signal(new Date().getFullYear());

    months = [
        { value: 1, name: 'January' },
        { value: 2, name: 'February' },
        { value: 3, name: 'March' },
        { value: 4, name: 'April' },
        { value: 5, name: 'May' },
        { value: 6, name: 'June' },
        { value: 7, name: 'July' },
        { value: 8, name: 'August' },
        { value: 9, name: 'September' },
        { value: 10, name: 'October' },
        { value: 11, name: 'November' },
        { value: 12, name: 'December' }
    ];

    percentageWidth = computed(() => {
        const percentage = this.budget()?.percentageUsed ?? 0;

        return Math.min(Math.max(percentage, 0), 100) + '%';
    });

    budgetForm = this.fb.nonNullable.group({
        amount: [0, [
            Validators.required,
            Validators.min(0.01)
        ]]
    });

    ngOnInit(): void {
        this.loadBudget();
    }

    loadBudget(): void {

        this.loading.set(true);
        this.error.set('');
        this.success.set('');
        this.isEditing.set(false);

        this.budgetsApi.getBudget(
            this.selectedYear(),
            this.selectedMonth()
        ).subscribe({

            next: (response) => {

                this.budget.set(response);

                this.budgetForm.patchValue({
                    amount: response.budgetAmount
                });

                this.loading.set(false);
            },

            error: (error: HttpErrorResponse) => {

                this.budget.set(null);

                this.budgetForm.reset({
                    amount: 0
                });

                if (error.status === 404) {
                    this.error.set(
                        'No budget found for this month. Create one below.'
                    );
                } else if (error.status === 401) {
                    this.error.set('Session expired. Please log in again.');
                } else {
                    this.error.set('Unable to load budget. Please try again.');
                }

                this.loading.set(false);
            }
        });
    }

    onMonthChange(value: string): void {
        this.selectedMonth.set(Number(value));
        this.loadBudget();
    }

    onYearChange(value: string): void {
        const year = Number(value);

        if (!Number.isInteger(year) || year < 1 || year > 9999) {
            this.error.set('Please enter a valid year.');
            return;
        }

        this.selectedYear.set(year);
        this.loadBudget();
    }

    startEditing(): void {
        const currentBudget = this.budget();

        if (!currentBudget) {
            return;
        }

        this.budgetForm.patchValue({
            amount: currentBudget.budgetAmount
        });

        this.isEditing.set(true);
        this.error.set('');
        this.success.set('');
    }

    cancelEditing(): void {
        this.isEditing.set(false);

        const currentBudget = this.budget();

        this.budgetForm.patchValue({
            amount: currentBudget?.budgetAmount ?? 0
        });

        this.error.set('');
    }

    onSubmit(): void {

        if (this.budgetForm.invalid) {
            this.budgetForm.markAllAsTouched();
            return;
        }

        this.saving.set(true);
        this.error.set('');
        this.success.set('');

        const request: CreateBudgetRequest = {
            year: this.selectedYear(),
            month: this.selectedMonth(),
            amount: this.budgetForm.getRawValue().amount
        };

        if (this.budget()) {

            const updateRequest: UpdateBudgetRequest = request;

            this.budgetsApi.updateBudget(updateRequest).subscribe({

                next: (response) => {

                    this.budget.set(response);
                    this.isEditing.set(false);
                    this.saving.set(false);
                    this.success.set('Budget updated successfully!');
                },

                error: (error: HttpErrorResponse) => {
                    this.handleSaveError(error);
                }
            });

        } else {

            this.budgetsApi.createBudget(request).subscribe({

                next: (response) => {

                    this.budget.set(response);
                    this.isEditing.set(false);
                    this.saving.set(false);
                    this.success.set('Budget created successfully!');
                },

                error: (error: HttpErrorResponse) => {
                    this.handleSaveError(error);
                }
            });
        }
    }

    private handleSaveError(error: HttpErrorResponse): void {

        this.saving.set(false);

        if (error.status === 400) {
            this.error.set('Invalid budget amount. Please check your input.');
        } else if (error.status === 401) {
            this.error.set('Session expired. Please log in again.');
        } else {
            this.error.set('Unable to save budget. Please try again.');
        }
    }

    get selectedMonthName(): string {
        return this.months.find(
            month => month.value === this.selectedMonth()
        )?.name ?? '';
    }
}
