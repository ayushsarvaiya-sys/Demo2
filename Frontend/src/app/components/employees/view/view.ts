import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { EmployeeService, EmployeeGetDto } from '../../../services/employee.service';

@Component({
  selector: 'app-view-employee',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './view.html',
  styleUrl: './view.css'
})
export class ViewEmployeeComponent implements OnInit {
  employee: EmployeeGetDto | null = null;
  isLoading = false;
  errorMessage = '';

  constructor(
    private employeeService: EmployeeService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['id'];
      this.loadEmployee(id);
    });
  }

  loadEmployee(id: number) {
    this.isLoading = true;
    this.employeeService.getEmployeeById(id).subscribe({
      next: (response) => {
        if (response.statusCode === 200) {
          this.employee = response.data;
        }
        console.log(this.employee);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.errorMessage = 'Failed to load employee';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  goBack() {
    this.router.navigate(['/employees']);
  }

  edit() {
    if (this.employee) {
      this.router.navigate(['/employees/update', this.employee.id]);
    }
  }
}
