import { RouterModule, Routes} from '@angular/router';
import { PatientListComponent } from './features/patients/patient-list/patient-list.component';
import { NgModule } from '@angular/core';
import { AddPatientComponent } from './features/patients/add-patient/add-patient.component';
import { FormsModule } from '@angular/forms';


export const routes: Routes = [
    {
        path: 'admin/patients',
        component: PatientListComponent
    },
    {
        path: 'admin/patients/add',
        component: AddPatientComponent
    }
];


@NgModule({
    imports: [RouterModule.forRoot(routes),FormsModule],
    exports: [RouterModule],
    
})
export class AppRoutingModule {}