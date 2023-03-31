import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  usernameControl: FormControl;
  passwordControl: FormControl;
  confirmPasswordControl: FormControl;
  genderControl: FormControl;
  knowsAsControl: FormControl;
  dateOfBirthControl: FormControl;
  cityControl: FormControl;
  countryControl: FormControl;
  maxDate: Date;
  validationErrors: string[] = [];

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.usernameControl = new FormControl('', Validators.required);
    this.passwordControl = new FormControl('', [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(8),
    ]);
    this.confirmPasswordControl = new FormControl('', [
      Validators.required,
      this.matchValues('password'),
    ]);
    this.genderControl = new FormControl('male');
    this.knowsAsControl = new FormControl('', Validators.required);
    this.dateOfBirthControl = new FormControl('', Validators.required);
    this.cityControl = new FormControl('', Validators.required);
    this.countryControl = new FormControl('', Validators.required);

    this.registerForm = new FormGroup({
      username: this.usernameControl,
      password: this.passwordControl,
      confirmPassword: this.confirmPasswordControl,
      gender: this.genderControl,
      knownAs: this.knowsAsControl,
      dateOfBirth: this.dateOfBirthControl,
      city: this.cityControl,
      country: this.countryControl,
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null
        : { isMatching: true };
    };
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/members');
        this.cancel();
      },
      error: (er) => {
        this.validationErrors = er;
      },
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
