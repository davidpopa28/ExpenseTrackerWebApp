import { Component } from '@angular/core';
import { Account } from '../models/account';
import { Subscription, forkJoin} from 'rxjs';
import { FormGroup,FormControl, Validators } from '@angular/forms';
import { User } from '../models/user';
import { Record } from '../models/record';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category';
import { RecordService } from '../services/record.service';
import { Subcategory } from '../models/subcategory';
import { UserService } from '../services/user.service';
import { switchMap, map } from 'rxjs/operators';


@Component({
  selector: 'app-analytics-page',
  templateUrl: './analytics-page.component.html',
  styleUrls: ['./analytics-page.component.scss']
})
export class AnalyticsPageComponent {
  form: FormGroup = new FormGroup({
    name: new FormControl<string>('', [Validators.required]),
    amount: new FormControl<number>(0, [Validators.required, Validators.minLength(0)]),
  }); 

  categories: Category [] = [];
  subcategories: Subcategory[] =[];
  records : Record[] =[];
  currentUser: User = {} as User;
  selectedAccountId : number | null = null;
  allaccounts: Account[] =[];
  selectedCategoryId: number | null = null;


  constructor(private categoryService: CategoryService, private recordService: RecordService, private userService:UserService) {
    this.getCurrentUser();
    this.getCategories();
  }

  getCategories() {
    this.categoryService.getCategories().subscribe(res=>{
      this.categories=res;
    })
  }
  getCurrentUser() {
    this.userService.getCurrentUser().subscribe(res =>{
      this.currentUser=res;
      this.recordService.getRecordsByUser(this.currentUser.id).subscribe(res =>{
        this.records=res;
      })
    })
  }
  selectCategory(categoryId: number) {
    if (this.selectedCategoryId === categoryId) {
      this.subcategories = [];
      this.selectedCategoryId = null;
    } else {
      this.selectedCategoryId = categoryId;
      this.categoryService.getSubcategoriesByCategory(categoryId).subscribe(subcategories => {
        this.subcategories = subcategories;
      });
    }
  }

  selectSubcategory(subcategoryId: number) {
    this.userService.getCurrentUser().subscribe(res =>{
      this.currentUser=res;
      this.recordService.getRecordsBySubcategoriesAndUser(subcategoryId, this.currentUser.id) 
      .subscribe(records => {
        this.records = records;
      });
    })
  }


}
