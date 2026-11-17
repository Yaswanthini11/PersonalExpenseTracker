export interface Category {
    categoryId: number;
    name: string;
    description: string | null;
    isDefault: boolean;
}

export interface CreateCategoryRequest {
    name: string;
    description: string | null;
}

export interface UpdateCategoryRequest {
    name: string;
    description: string | null;
}