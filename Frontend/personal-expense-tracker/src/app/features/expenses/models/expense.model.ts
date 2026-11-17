export interface Expense {
    expenseId: number;
    title: string;
    amount: number;
    expenseDate: string;
    description: string | null;
    categoryId: number;
    categoryName: string;
}

export interface CreateExpenseRequest {
    title: string;
    amount: number;
    expenseDate: string;
    description: string | null;
    categoryId: number;
}

export interface UpdateExpenseRequest {
    title: string;
    amount: number;
    expenseDate: string;
    description: string | null;
    categoryId: number;
}

export interface ExpenseFilters {
    month?: number;
    year?: number;
    categoryId?: number;
}