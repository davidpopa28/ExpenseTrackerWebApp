import { Record } from "./record";
export interface Account {
    id: number;
    name: string;
    balance: number;
    records: Record[];
}