import { Subcategory } from "./subcategory";

export interface Category {
    id: number;
    name: string;
    records: Subcategory[];
}