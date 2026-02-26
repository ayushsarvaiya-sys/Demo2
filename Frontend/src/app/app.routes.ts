import { Routes } from '@angular/router';
import { EmployeesComponent } from './components/employees/employees';
import { AddEmployeeComponent } from './components/employees/add/add';
import { UpdateEmployeeComponent } from './components/employees/update/update';
import { ViewEmployeeComponent } from './components/employees/view/view';

export const routes: Routes = [
  { path: '', redirectTo: '/employees', pathMatch: 'full' },
  { path: 'employees', component: EmployeesComponent },
  { path: 'employees/add', component: AddEmployeeComponent },
  { path: 'employees/update/:id', component: UpdateEmployeeComponent },
  { path: 'employees/view/:id', component: ViewEmployeeComponent }
];
