**ApiApplication**

This project is a comprehensive web application for a doctor’s office, featuring both a backend API built with ASP.NET Core and a frontend developed with Angular. The backend handles various operations related to insurance providers and patients, while the frontend provides a user-friendly interface for interacting with the API.

**Table of Contents**

*Prerequisites

*Installation

*API Endpoints

*Frontend

*Contributing

**Prerequisites**

.NET 8 SDK

Node.js (which includes npm)

Angular CLI

**Clone the repository:**

git clone https://github.com/deionboyer/ApiApplication.git


**API Endpoints**

**Insurance Providers-EndPoints**

**Create Provider**

*POST: /api/InsuranceProviders/CreateProvider

*Request Body: InsuranceProviders object

*Response: int (Insurance Provider ID)

**Get All Providers**

*Get: /api/InsuranceProviders/GetAllProviders

*Response: List<InsuranceProviders>

**Get Provider By ID**

*GET: /api/InsuranceProviders/GetProviderById

*Query Parameter: int insuranceId

*Response: InsuranceProviders

**Update Provider**

*PUT: /api/InsuranceProviders/UpdateProvider

*Request Body: InsuranceProviders object

*Response: int (Updated Provider ID)

**Delete Provider**

*DELETE: /api/InsuranceProviders/DeleteProvider

*Query Parameter: int insuranceId

*Response: bool (Is Deleted)

**Patients-EndPoints**

**Create Patient**

*POST: /api/Patients/CreatePatient

*Request Body: Patients object

*Response:int (Patient ID)

**Get All Patients**

*GET: /api/Patients/GetAllPatients

*Response: List<Patients>

**Get Patient By ID**

*GET: /api/Patients/GetPatientById/{patientId}

*Response: Patients

**Update Patient**

*PUT: /api/Patients/UpdatePatient

*Request Body: Patients object

*Response: int (Updated Patient ID)

**Delete Patient**

*DELETE: /api/Patients/DeletePatient

*Query Parameter: int patientId

*Response: bool (Is Deleted)

**Angular-Frontend**

The frontend is built with Angular and provides a user-friendly interface for interacting with the API. It includes views for managing insurance providers and patients.

**Contributing**

Contributions are welcome! Please fork the repository and create a pull request with your changes.

