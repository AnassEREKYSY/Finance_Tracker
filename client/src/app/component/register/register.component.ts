import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RegisterService } from '../../core/services/register.service';
import { SnackBarService } from '../../core/services/snack-bar.service';
import { Register } from '../../core/models/Register';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup = new FormGroup({});
  private registerService = inject(RegisterService);
  private snackBarService = inject(SnackBarService);

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const registerModel: Register = this.registerForm.value;

      this.registerService.register(registerModel).subscribe({
        next: () => {
          this.snackBarService.success('Registration successful');
        },
        error: () => {
          this.snackBarService.error('Registration failed');
        },
      });
    } else {
      this.snackBarService.error('Please fill all required fields');
    }
  }
}
