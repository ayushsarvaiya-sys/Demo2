import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EmployeeService } from '../../../services/employee.service';

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add.html',
  styleUrl: './add.css'
})
export class AddEmployeeComponent implements OnInit {
  form!: FormGroup;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private employeeService: EmployeeService,
    private router: Router,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.form = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      departmentId: [0, [Validators.required, Validators.min(1)]],
      designationId: [0, [Validators.required, Validators.min(1)]]
    });
  }

  submit() {
    if (!this.form.valid) {
      this.errorMessage = 'Please fill in all required fields correctly';
      this.markFormGroupTouched(this.form);
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    this.employeeService.createEmployee(this.form.value).subscribe({
      next: (response) => {
        if (response.statusCode === 201) {
          this.successMessage = 'Employee created successfully!';
          setTimeout(() => {
            this.router.navigate(['/employees']);
          }, 1500);
        }
        this.isSubmitting = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to create employee';
        this.isSubmitting = false;
        this.cdr.detectChanges();
      }
    });
  }

  getFieldError(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (!control || !control.errors || !control.touched) {
      return '';
    }

    if (control.errors['required']) {
      return `${this.formatFieldName(fieldName)} is required`;
    }
    if (control.errors['minlength']) {
      return `${this.formatFieldName(fieldName)} must be at least ${control.errors['minlength'].requiredLength} characters`;
    }
    if (control.errors['email']) {
      return 'Please enter a valid email address';
    }
    if (control.errors['min']) {
      return `${this.formatFieldName(fieldName)} must be greater than 0`;
    }
    return '';
  }

  private formatFieldName(name: string): string {
    return name.replace(/([A-Z])/g, ' $1').trim().charAt(0).toUpperCase() + name.slice(1);
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
  }

  onCancel() {
    this.router.navigate(['/employees']);
  }
}
