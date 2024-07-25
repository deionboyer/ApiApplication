import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InsuranceProviders } from '../models/insurance-providers-request.model';

@Injectable({
    providedIn: 'root' // Makes the service available at the root level
})
export class InsuranceProviderService {
    private apiUrl =  'https://localhost:7104/api/InsuranceProviders/GetAllProviders'; // Replace with your API URL

    constructor(private http: HttpClient) {}

    getProviders(): Observable<InsuranceProviders[]> {
        return this.http.get<InsuranceProviders[]>(this.apiUrl);
    }
}