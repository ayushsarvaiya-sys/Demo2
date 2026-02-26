import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { EmployeeService, EmployeeGetDto, DepartmentDto } from '../../services/employee.service';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './employees.html',
  styleUrl: './employees.css',
})
export class EmployeesComponent implements OnInit {
  employees: EmployeeGetDto[] = [];
  departments: DepartmentDto[] = [];
  currentPage = 1;
  pageSize = 5;
  totalEmployees = 0;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  // Filter properties
  searchTerm = '';
  selectedDepartmentId: number | null = null;
  startDate: string | null = null;
  endDate: string | null = null;
  isSearching = false;

  constructor(
    private employeeService: EmployeeService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.loadDepartments();
    this.loadEmployees();
    this.loadCount();
  }

  loadDepartments() {
    this.employeeService.getDepartments().subscribe({
      next: (response) => {
        if (response.statusCode === 200) {
          this.departments = response.data;
        }
      },
      error: () => {
        console.error('Failed to load departments');
      },
    });
  }

  loadEmployees() {
    this.isLoading = true;
    this.errorMessage = '';
    const skip = (this.currentPage - 1) * this.pageSize;

    this.employeeService.getAllEmployees(skip, this.pageSize).subscribe({
      next: (response) => {
        if (response.statusCode === 200) {
          this.employees = response.data;
        }
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.errorMessage = 'Failed to load employees';
        this.isLoading = false;
      },
    });
  }

  searchEmployees() {
    // Reset to first page when user clicks search button (not during pagination)
    if (!this.isSearching) {
      this.currentPage = 1;
    }
    this.isSearching = true;
    this.isLoading = true;
    this.errorMessage = '';
    const skip = (this.currentPage - 1) * this.pageSize;

    // Call single search API with all filters
    this.employeeService
      .search(
        this.searchTerm || undefined,
        this.startDate || undefined,
        this.endDate || undefined,
        this.selectedDepartmentId || undefined,
        skip,
        this.pageSize,
      )
      .subscribe({
        next: (response) => {
          if (response.statusCode === 200) {
            this.employees = response.data;
            this.totalEmployees = response.data.length;
          }
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          this.errorMessage = 'Failed to search employees';
          this.isLoading = false;
        },
      });
  }

  clearFilters() {
    this.searchTerm = '';
    this.selectedDepartmentId = null;
    this.startDate = null;
    this.endDate = null;
    this.isSearching = false;
    this.currentPage = 1;
    this.loadEmployees();
    this.loadCount();
  }

  loadCount() {
    this.employeeService.getCount().subscribe({
      next: (response) => {
        if (response.statusCode === 200) {
          this.totalEmployees = response.data.count;
        }
      },
      error: () => {},
    });
  }

  getTotalPages(): number {
    return Math.ceil(this.totalEmployees / this.pageSize);
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      if (this.isSearching) {
        this.searchEmployees();
      } else {
        this.loadEmployees();
      }
    }
  }

  nextPage() {
    if (this.currentPage < this.getTotalPages()) {
      this.currentPage++;
      if (this.isSearching) {
        this.searchEmployees();
      } else {
        this.loadEmployees();
      }
    }
  }

  deleteEmployee(id: number) {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.deleteEmployee(id).subscribe({
        next: () => {
          this.successMessage = 'Employee deleted successfully';
          if (this.isSearching) {
            this.searchEmployees();
          } else {
            this.loadEmployees();
          }
          this.loadCount();
          setTimeout(() => {
            this.successMessage = '';
          }, 3000);
        },
        error: () => {
          this.errorMessage = 'Failed to delete employee';
        },
      });
    }
  }

  getFullName(employee: EmployeeGetDto): string {
    return `${employee.firstName} ${employee.lastName}`;
  }

  formatDate(dateString: string): string {
    if (!dateString) return '';

    return new Date(dateString).toLocaleDateString('en-IN', {
      timeZone: 'Asia/Kolkata',
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  }
}
