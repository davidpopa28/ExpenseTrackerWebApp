import { Account } from '../models/account'
import { Subcategory } from './subcategory';
import { User } from './user';
export interface Record {
    id: number;
    type: string;
    value: number;
    note: string;
    date: Date;
    account: Account;
    user: User;
    subcategory: Subcategory;
}