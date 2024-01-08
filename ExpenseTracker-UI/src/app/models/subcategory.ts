import { Category } from "./category";
import { Record } from "./record";
export interface Subcategory {
    id: number;
    name: string;
    category: Category;
    records: Record[];
    value: number;
}