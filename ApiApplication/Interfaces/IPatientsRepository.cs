using ApiApplication.Models;

namespace ApiApplication.Interfaces
{
    public interface IPatientsRepository
    {
        Patients? GetPatientById(int patientId);
        List<Patients>? GetAllPatients();
        int CreatePatient(Patients patient);
        int UpdatePatient(Patients patients);
        bool DeletePatients(int patientId);
        // Other methods for update and delete

        //Create Interface to me, instead of calling methidds in class call methds in interface. The only relationship, with controller and repository, will be only with interface. 
    }
}
