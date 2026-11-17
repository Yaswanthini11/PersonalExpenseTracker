import {
    Component,
    OnInit,
    inject,
    signal,
    computed
} from '@angular/core';

import { CommonModule } from '@angular/common';
import {
    FormBuilder,
    ReactiveFormsModule,
    Validators
} from '@angular/forms';

import { HttpErrorResponse } from '@angular/common/http';

import { ExpensesApiService } from '../../data-access/expenses-api';

import {
    Expense,
    CreateExpenseRequest,
    UpdateExpenseRequest
} from '../../models/expense.model';

import { CategoriesApiService } from '../../../categories/data-access/categories-api';

import { Category } from '../../../categories/models/category.model';

@Component({
    selector: 'app-expenses',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule
    ],
    templateUrl: './expenses.html',
    styleUrl: './expenses.css'
})
export class Expenses implements OnInit {

    private expensesApi = inject(ExpensesApiService);
    private categoriesApi = inject(CategoriesApiService);
    private fb = inject(FormBuilder);

    // UI state
    expenses = signal<Expense[]>([]);
    categories = signal<Category[]>([]);

    loading = signal(false);
    saving = signal(false);

    error = signal('');
    success = signal('');

    editingExpenseId = signal<number | null>(null);

    // Filter state
    selectedMonth = signal<number | null>(null);
    selectedYear = signal<number | null>(null);
    selectedCategoryId = signal<number | null>(null);

    // Derived values
    totalExpenses = computed(() => this.expenses().length);

    totalAmount = computed(() =>
        this.expenses().reduce(
            (total, expense) => total + expense.amount,
            0
        )
    );

    // Expense form
    expenseForm = this.fb.nonNullable.group({
        title: ['', [
            Validators.required,
            Validators.minLength(2),
            Validators.maxLength(100)
        ]],

        amount: [0, [
            Validators.required,
            Validators.min(0.01)
        ]],

        expenseDate: ['', Validators.required],

        description: ['', Validators.maxLength(250)],

        categoryId: [0, [
            Validators.required,
            Validators.min(1)
        ]]
    });

    ngOnInit(): void {
        this.loadCategories();
        this.loadExpenses();
    }

    // Load categories for the dropdown
    loadCategories(): void {
        this.categoriesApi.getAll().subscribe({
            next: (response) => {
                this.categories.set(response);
            },
            error: (error: HttpErrorResponse) => {
                this.error.set(
                    this.getErrorMessage(error)
                );
            }
        });
    }

    // Load expenses
    loadExpenses(): void {
        this.loading.set(true);
        this.error.set('');

        const filters = {
            month: this.selectedMonth() ?? undefined,
            year: this.selectedYear() ?? undefined,
            categoryId: this.selectedCategoryId() ?? undefined
        };

        this.expensesApi.getAll(filters).subscribe({
            next: (response) => {
                this.expenses.set(response);
                this.loading.set(false);
            },
            error: (error: HttpErrorResponse) => {
                this.error.set(
                    this.getErrorMessage(error)
                );
                this.loading.set(false);
            }
        });
    }

    // Create or update expense
    onSubmit(): void {
        if (this.expenseForm.invalid) {
            this.expenseForm.markAllAsTouched();
            return;
        }

        this.saving.set(true);
        this.error.set('');
        this.success.set('');

        const formValue = this.expenseForm.getRawValue();

        if (this.editingExpenseId() === null) {

            const request: CreateExpenseRequest = {
                title: formValue.title.trim(),
                amount: formValue.amount,
                expenseDate: formValue.expenseDate,
                description: formValue.description.trim() || null,
                categoryId: formValue.categoryId
            };

            this.expensesApi.create(request).subscribe({
                next: () => {
                    this.success.set('Expense created successfully.');
                    this.resetForm();
                    this.loadExpenses();
                    this.saving.set(false);
                },
                error: (error: HttpErrorResponse) => {
                    this.error.set(
                        this.getErrorMessage(error)
                    );
                    this.saving.set(false);
                }
            });

        } else {

            const request: UpdateExpenseRequest = {
                title: formValue.title.trim(),
                amount: formValue.amount,
                expenseDate: formValue.expenseDate,
                description: formValue.description.trim() || null,
                categoryId: formValue.categoryId
            };

            this.expensesApi.update(
                this.editingExpenseId()!,
                request
            ).subscribe({
                next: () => {
                    this.success.set('Expense updated successfully.');
                    this.resetForm();
                    this.loadExpenses();
                    this.saving.set(false);
                },
                error: (error: HttpErrorResponse) => {
                    this.error.set(
                        this.getErrorMessage(error)
                    );
                    this.saving.set(false);
                }
            });
        }
    }

    // Load selected expense into the form
    editExpense(expense: Expense): void {
        this.editingExpenseId.set(expense.expenseId);

        this.expenseForm.patchValue({
            title: expense.title,
            amount: expense.amount,
            expenseDate: expense.expenseDate.substring(0, 10),
            description: expense.description ?? '',
            categoryId: expense.categoryId
        });

        this.success.set('');
        this.error.set('');
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }

    // Delete expense
    deleteExpense(expenseId: number): void {
        const confirmed = window.confirm(
            'Are you sure you want to delete this expense?'
        );

        if (!confirmed) {
            return;
        }

        this.error.set('');
        this.success.set('');

        this.expensesApi.delete(expenseId).subscribe({
            next: () => {
                this.success.set('Expense deleted successfully.');

                if (this.editingExpenseId() === expenseId) {
                    this.resetForm();
                }

                this.loadExpenses();
            },
            error: (error: HttpErrorResponse) => {
                this.error.set(
                    this.getErrorMessage(error)
                );
            }
        });
    }

    // Apply filters
    applyFilters(): void {
        this.loadExpenses();
    }

    // Clear filters
    clearFilters(): void {
        this.selectedMonth.set(null);
        this.selectedYear.set(null);
        this.selectedCategoryId.set(null);

        this.loadExpenses();
    }

    // Reset form
    resetForm(): void {
        this.expenseForm.reset({
            title: '',
            amount: 0,
            expenseDate: '',
            description: '',
            categoryId: 0
        });

        this.editingExpenseId.set(null);
    }

    // Error message helper
    private getErrorMessage(error: HttpErrorResponse): string {
        if (typeof error.error === 'string') {
            return error.error;
        }

        if (error.error?.message) {
            return error.error.message;
        }

        if (error.status === 400) {
            return 'Please check the information you entered.';
        }

        if (error.status === 401) {
            return 'Your session has expired. Please log in again.';
        }

        if (error.status === 404) {
            return 'The requested expense was not found.';
        }

        return 'Something went wrong. Please try again.';
    }
}