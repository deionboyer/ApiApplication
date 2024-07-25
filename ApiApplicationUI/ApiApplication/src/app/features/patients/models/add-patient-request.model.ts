import { InsuranceProviders } from "./insurance-providers-request.model";

export interface AddPatientRequest{
    firstName: string;
    lastName: string;
    emailAddress: string;
    insuranceProvider: InsuranceProviders; //This will be changed to int(number) insuranceproviderID
}


// array of insurance providers display the name but when you select it you are passing the ID and not the name. 
// When you populate the list it will dislplay the namespace,e of all isnurance, when you click it will pass the ID to the format, 
// when you submit the form you ar passing the specific ID dont have to pass the whole ID(Thats why we are using a foreign key )