import { Record } from "./record";
export interface User {
    id: number;
    name: string;
    email: string;
    password: string;
    records: Record[];
}