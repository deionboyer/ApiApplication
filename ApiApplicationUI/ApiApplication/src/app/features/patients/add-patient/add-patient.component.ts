import { Component, NgModule, model } from '@angular/core';
import { AddPatientRequest } from '../models/add-patient-request.model';
import { FormsModule} from '@angular/forms';
import { PatientsService } from '../services/patients.service';
import { subscribe } from 'diagnostics_channel';
import { response } from 'express';
import { error } from 'console';
import { rmSync } from 'fs';
import { InsuranceProviderService } from '../services/insurance-providers.service';
import { InsuranceProviders } from '../models/insurance-providers-request.model';
import { CommonModule, NgForOf } from '@angular/common';


@Component({
  selector: 'app-add-patient',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-patient.component.html',
  styleUrl: './add-patient.component.css'
})


export class AddPatientComponent {
 

  model: AddPatientRequest
  
  constructor(
    private patientService: PatientsService,
    private insuranceProviderService: InsuranceProviderService) {
    this.model = {
      firstName: '',
      lastName: '',
      emailAddress: '',
      insuranceProvider: {
        insuranceId: 1,
        state: '',
        insuranceName: '',
        coPay: 1
      }
    }
  }
  @NgModule({
    imports: [CommonModule]
  })
  onFormSubmit() {
    this.patientService.addPatient(this.model)
      .subscribe({
        // Handle the response here
        next: (response) => {
          console.log("The was successful! ")
        }
      });
  }
  
  ngOnInit() {
    this.insuranceProviderService.getProviders()
        .subscribe({
            next: (providers) => {
                // Handle the response here (e.g., populate your model or display the providers)
                console.log("Insurance providers retrieved:", providers);
            },
            error: (err) => {
                console.error("An error occurred:", err);
                // Handle error cases
            }
        });
}
}
