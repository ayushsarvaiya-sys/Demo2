import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface EmployeeGetDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  departmentId: number;
  departmentName: string;
  designationId: number;
  designationTitle: string;
  createdAt: string; // ISO datetime string
}

export interface DepartmentDto {
  id: number;
  name: string;
  code: string;
  description?: string;
}

export interface EmployeeCreateDto {
  firstName: string;
  lastName: string;
  email: string;
  departmentId: number;
  designationId: number;
}

export interface EmployeeUpdateDto {
  firstName: string;
  lastName: string;
  email: string;
  departmentId: number;
  designationId: number;
}

export interface ApiResponse {
  statusCode: number;
  message: string;
  data: any;
}

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'https://localhost:7238/api/Employees';
  private departmentApiUrl = 'https://localhost:7238/api/Departments';

  constructor(private http: HttpClient) {}

  getAllEmployees(skip: number = 0, take: number = 10): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}?skip=${skip}&take=${take}`);
  }

  getEmployeeById(id: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}/${id}`);
  }

  createEmployee(dto: EmployeeCreateDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(this.apiUrl, dto);
  }

  updateEmployee(id: number, dto: EmployeeUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}/${id}`, dto);
  }

  deleteEmployee(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getCount(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}/count`);
  }

  search(searchTerm?: string, startDate?: Date | string, endDate?: Date | string, departmentId?: number | null, skip: number = 0, take: number = 10): Observable<ApiResponse> {
    let params = `skip=${skip}&take=${take}`;
    
    if (searchTerm && searchTerm.trim()) {
      params += `&searchTerm=${encodeURIComponent(searchTerm.trim())}`;
    }
    
    if (departmentId && departmentId > 0) {
      params += `&departmentId=${departmentId}`;
    }
    
    if (startDate) {
      const startDateIso = typeof startDate === 'string' ? new Date(startDate).toISOString() : startDate.toISOString();
      params += `&startDate=${startDateIso}`;
    }
    
    if (endDate) {
      const endDateIso = typeof endDate === 'string' ? new Date(endDate).toISOString() : endDate.toISOString();
      params += `&endDate=${endDateIso}`;
    }

    return this.http.get<ApiResponse>(`${this.apiUrl}/search?${params}`);
  }

  getDepartments(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.departmentApiUrl}`);
  }
}
