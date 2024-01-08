import { Component } from '@angular/core';
import { GlobalService } from '../services/global.service';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { UserService } from '../services/user.service';
import { User } from '../models/user';
import { Account } from '../models/account';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category';
import { Subcategory } from '../models/subcategory';
import { RecordService } from '../services/record.service';
import { Record } from '../models/record';
import { RecordDTO } from '../models/recordDTO';
import { RecordsPageComponent } from '../records-page/records-page.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  currentUser: User = {} as User;
  accounts: Account[] = [];
  categories: Category[] = [];
  subcategories: Subcategory[] = [];
  records: Record[] = [];

  form: FormGroup = new FormGroup({
    selectedAccount: new FormControl(),
    type: new FormControl<string>('', [Validators.required]),
    recordValue: new FormControl<number>(0, [Validators.required, Validators.minLength(0)]),
    notes: new FormControl(),
    date: new FormControl<string>('', [Validators.required]),
    selectedCategory: new FormControl(''),
    selectedSubcategory: new FormControl(''),
  });

  constructor(private service: GlobalService, private router: Router, private accountService: AccountService,
    private userService: UserService, private categoryService: CategoryService,
    private recordService: RecordService) {
    this.userService.getCurrentUser().subscribe(res => {
      this.currentUser = res;
    })
    if (localStorage.getItem('token')) {
      this.service.setLogIn(true);
    }
  }

  addRecord() {
    this.userService.getCurrentUser().subscribe(res => {
      this.currentUser = res;
      let accountId = this.form.controls['selectedAccount'].value;
      let subcategoryId = this.form.get('selectedSubcategory')!.value;
      let record: RecordDTO = {
        type: this.form.controls['type'].value,
        value: this.form.controls['recordValue'].value,
        note: this.form.controls['notes'].value,
        date: this.form.controls['date'].value,
      } as RecordDTO;
      this.recordService.addRecord(this.currentUser.id, accountId, subcategoryId, record).subscribe(() => {
        this.recordService.getRecordsByUser(this.currentUser.id).subscribe(() => {
          this.router.navigate(['/home']);
        });
      })
    });
  }

  modalRecord() {
    this.userService.getCurrentUser().subscribe(res => {
      this.currentUser = res;
      console.log(this.currentUser);
      this.accountService.getAccountsByUser(this.currentUser.id).subscribe(res => {
        this.accounts = res;
        console.log(this.accounts);
        this.categoryService.getCategories().subscribe(res => {
          this.categories = res;
        });
      });
    });
  }

  onCategoryChange() {
    const selectedCategory = this.form.get('selectedCategory')!.value;
    this.getSubcategoriesByCategory(selectedCategory);
  }

  getSubcategoriesByCategory(categoryId: number) {
    this.categoryService.getSubcategoriesByCategory(categoryId).subscribe(res => {
      this.subcategories = res;
      this.form.get('selectedSubcategory')!.setValue('');
      this.form.get('selectedSubcategory')!.enable();
    });
  }

  logout() {
    localStorage.removeItem('token');
    this.service.setLogIn(false);
    this.router.navigate(['/login']);
  }
}
