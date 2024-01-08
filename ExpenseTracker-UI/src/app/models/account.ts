import { Record } from "./record";
import { UserRole } from "./userRole";
export interface Account {
    id: number;
    name: string;
    balance: number;
    records: Record[];
    userRole: UserRole;
}