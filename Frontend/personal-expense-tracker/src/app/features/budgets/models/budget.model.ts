
export interface Budget {
    budgetId: number;
    year: number;
    month: number;
    budgetAmount: number;
    spentAmount: number;
    remainingAmount: number;
    percentageUsed: number;
}

export interface CreateBudgetRequest {
    year: number;
    month: number;
    amount: number;
}

export interface UpdateBudgetRequest {
    year: number;
    month: number;
    amount: number;
}
