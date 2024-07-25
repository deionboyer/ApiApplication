import { Component } from '@angular/core';
import { RouterLink,Route,RouterOutlet,RouterLinkActive } from '@angular/router';
import { AddPatientComponent } from '../add-patient/add-patient.component';
import { RouterModule } from '@angular/router';



@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [RouterLink,RouterOutlet,RouterLinkActive,RouterModule],
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.css'
})
export class PatientListComponent {
  
}
