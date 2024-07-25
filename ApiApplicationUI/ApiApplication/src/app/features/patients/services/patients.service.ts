import { Injectable } from '@angular/core';
import { AddPatientRequest } from '../models/add-patient-request.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class PatientsService {

  constructor(private http: HttpClient) { }

  addPatient(model: AddPatientRequest): Observable<void> {
    // Assuming this.http.post<void> returns an Observable<void>
    return this.http.post<void>('https://localhost:7104/api/patients/createpatient', model);
  }
}

