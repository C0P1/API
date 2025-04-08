<<<<<<< HEAD
import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
=======
import { Component, inject, OnInit, output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators, ValidatorFn, AbstractControl, FormBuilder } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { JsonPipe,NgIf } from '@angular/common';
import { TextInputComponent } from '../_forms/text-input/text-input.component';
import { DatePickerComponent } from '../_forms/date-picker/date-picker.component';
import { Router } from '@angular/router';
>>>>>>> Parcial04

@Component({
  selector: 'app-register',
  standalone: true,
<<<<<<< HEAD
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  //usersFromHomeComponent = input.required<any>();
  private toastr = inject(ToastrService);
  cancelRegister = output<boolean>();
  model: any= {};

  register(): void {
    this.accountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.cancel();
      },
      error: (error) => {
        console.log(error);
        this.toastr.error(error.error);
      }
=======
  imports: [ReactiveFormsModule, TextInputComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  private accountService = inject(AccountService);
  //usersFromHomeComponent = input.required<any>();
  private fb = inject(FormBuilder)
  private router = inject(Router);
  cancelRegister = output<boolean>();
  registerForm: FormGroup = new FormGroup({});
  maxDate = new Date();
  validationErrors: string[] | undefined;

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ["female"],
      username: ["", Validators.required],
      knownAs: ["", Validators.required],
      birthDay: ["", Validators.required],
      city: ["", Validators.required],
      country: ["", Validators.required],
      password: ["", [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ["", [Validators.required, this.matchValues("password")]]
    });
    this.registerForm.controls["password"].valueChanges.subscribe({
      next: () => this.registerForm.controls["confirmPassword"].updateValueAndValidity()
      })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : { isMatching: true }
    };
  }

  register(): void {
    const bd = this.getDateOnly(this.registerForm.get("birthDay")?.value);
    this.registerForm.patchValue({ birthDay: bd });
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => this.router.navigateByUrl("/members"),
      error: (error) => this.validationErrors = error
>>>>>>> Parcial04
    });
  }

  cancel(): void {
    this.cancelRegister.emit(false);
  }
<<<<<<< HEAD
=======

  private getDateOnly(birthDay: string | undefined) {
    if (!birthDay) return;
    return new Date(birthDay).toISOString().slice(0, 10);
  }
>>>>>>> Parcial04
}
