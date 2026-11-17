
import {
    Component,
    OnInit,
    inject,
    signal,
    computed
} from '@angular/core';

import {
    FormBuilder,
    ReactiveFormsModule,
    Validators
} from '@angular/forms';

import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs';

import { CategoriesApiService } from '../../data-access/categories-api';

import type {
    Category,
    CreateCategoryRequest,
    UpdateCategoryRequest
} from '../../models/category.model';

@Component({
    selector: 'app-categories',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        RouterLink
    ],
    templateUrl: './categories.html',
    styleUrl: './categories.css'
})
export class Categories implements OnInit {

    // Inject dependencies
    private categoriesApi = inject(CategoriesApiService);
    private fb = inject(FormBuilder);

    // Category data
    categories = signal<Category[]>([]);

    // Loading and saving states
    isLoading = signal(false);
    isSaving = signal(false);

    // Messages
    errorMessage = signal('');
    successMessage = signal('');

    // Currently edited category
    editingCategory = signal<Category | null>(null);

    // Separate default and custom categories
    defaultCategories = computed(() =>
        this.categories().filter(category => category.isDefault)
    );

    customCategories = computed(() =>
        this.categories().filter(category => !category.isDefault)
    );

    // Reactive form
    categoryForm = this.fb.nonNullable.group({
        name: [
            '',
            [
                Validators.required,
                Validators.minLength(2),
                Validators.maxLength(50),
                Validators.pattern(/\S/)
            ]
        ],
        description: [
            '',
            [
                Validators.maxLength(200)
            ]
        ]
    });

    // Load categories when component initializes
    ngOnInit(): void {
        this.loadCategories();
    }

    // GET: Retrieve all categories
    loadCategories(): void {

        this.isLoading.set(true);
        this.errorMessage.set('');

        this.categoriesApi.getAll()
            .pipe(
                finalize(() => this.isLoading.set(false))
            )
            .subscribe({
                next: (response: Category[]) => {
                    this.categories.set(response);
                },
                error: (error: HttpErrorResponse) => {
                    this.errorMessage.set(
                        this.getErrorMessage(
                            error,
                            'Unable to load categories.'
                        )
                    );
                }
            });
    }

    // Create or update category
    saveCategory(): void {

        if (this.categoryForm.invalid) {
            this.categoryForm.markAllAsTouched();
            return;
        }

        const formValue = this.categoryForm.getRawValue();

        const request: CreateCategoryRequest = {
            name: formValue.name.trim(),
            description: formValue.description.trim() || null
        };

        this.isSaving.set(true);
        this.errorMessage.set('');
        this.successMessage.set('');

        const editing = this.editingCategory();

        if (editing) {

            // PUT: Update an existing category
            const updateRequest: UpdateCategoryRequest = request;

            this.categoriesApi
                .update(editing.categoryId, updateRequest)
                .pipe(
                    finalize(() => this.isSaving.set(false))
                )
                .subscribe({
                    next: () => {
                        this.successMessage.set(
                            'Category updated successfully.'
                        );

                        this.resetForm();
                        this.loadCategories();
                    },
                    error: (error: HttpErrorResponse) => {
                        this.errorMessage.set(
                            this.getErrorMessage(
                                error,
                                'Unable to update category.'
                            )
                        );
                    }
                });

        } else {

            // POST: Create a new category
            this.categoriesApi
                .create(request)
                .pipe(
                    finalize(() => this.isSaving.set(false))
                )
                .subscribe({
                    next: () => {
                        this.successMessage.set(
                            'Category created successfully.'
                        );

                        this.resetForm();
                        this.loadCategories();
                    },
                    error: (error: HttpErrorResponse) => {
                        this.errorMessage.set(
                            this.getErrorMessage(
                                error,
                                'Unable to create category.'
                            )
                        );
                    }
                });
        }
    }

    // Edit an existing custom category
    editCategory(category: Category): void {

        // Default categories cannot be edited
        if (category.isDefault) {
            return;
        }

        this.editingCategory.set(category);

        this.categoryForm.patchValue({
            name: category.name,
            description: category.description ?? ''
        });

        this.errorMessage.set('');
        this.successMessage.set('');
    }

    // Cancel editing
    cancelEdit(): void {
        this.resetForm();
    }

    // DELETE: Remove a custom category
    deleteCategory(category: Category): void {

        // Default categories cannot be deleted
        if (category.isDefault) {
            return;
        }

        const confirmed = window.confirm(
            `Are you sure you want to delete "${category.name}"?`
        );

        if (!confirmed) {
            return;
        }

        this.errorMessage.set('');
        this.successMessage.set('');

        this.categoriesApi
            .delete(category.categoryId)
            .subscribe({
                next: () => {
                    this.successMessage.set(
                        'Category deleted successfully.'
                    );

                    this.loadCategories();
                },
                error: (error: HttpErrorResponse) => {
                    this.errorMessage.set(
                        this.getErrorMessage(
                            error,
                            'Unable to delete category.'
                        )
                    );
                }
            });
    }

    // Reset the form
    private resetForm(): void {

        this.categoryForm.reset({
            name: '',
            description: ''
        });

        this.editingCategory.set(null);
    }

    // Handle API errors
    private getErrorMessage(
        error: HttpErrorResponse,
        fallback: string
    ): string {

        return error.error?.message
            ?? error.error?.title
            ?? fallback;
    }
}
