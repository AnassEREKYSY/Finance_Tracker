import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { LoginService } from '../../core/services/login.service';
import { SnackBarService } from '../../core/services/snack-bar.service';
import { Login } from '../../core/models/Login';

@Component({
    selector: 'app-login',
    standalone:true,
    imports: [
        ReactiveFormsModule,
        MatCardModule,
        MatFormFieldModule,
        MatButtonModule,
        MatInputModule,
    ],
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  loginService = inject(LoginService);
  snackBarService =  inject(SnackBarService);
  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const loginModel: Login = this.loginForm.value;

      this.loginService.login(loginModel).subscribe({
        next: () => {
          this.snackBarService.success('Login successful');
        },
        error: () => {
          this.snackBarService.error('Login failed');
        },
      });
    } else {
      this.snackBarService.error('Please fill all required fields');
    }
  }
}
