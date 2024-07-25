import { Component } from '@angular/core';
import { RouterLink,provideRouter,Route,RouterLinkActive,RouterOutlet } from '@angular/router';
import { AddPatientComponent } from '../../../features/patients/add-patient/add-patient.component';
import { PatientListComponent } from '../../../features/patients/patient-list/patient-list.component';

export const routes: Route[] = [
  {path: 'admin/patients', component: PatientListComponent},
  
]

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink,RouterLinkActive,RouterOutlet],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

}
