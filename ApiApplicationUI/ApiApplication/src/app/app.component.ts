import { Component, NgModule } from '@angular/core';
import { Route, RouterOutlet } from '@angular/router';
import { NavbarComponent } from "./core/componets/navbar/navbar.component";
import { PatientListComponent } from './features/patients/patient-list/patient-list.component';
import { AddPatientComponent } from './features/patients/add-patient/add-patient.component';
import { FormsModule } from '@angular/forms';
import { provideRouter,RouterLink,Router } from '@angular/router';
import { bootstrapApplication } from '@angular/platform-browser';
import { InsuranceProviderService } from './features/patients/services/insurance-providers.service';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [RouterOutlet, NavbarComponent,PatientListComponent,AddPatientComponent,FormsModule,RouterLink,CommonModule]
})

export class AppComponent {
  title = 'ApiApplication';
}

