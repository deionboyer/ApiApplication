using System.Collections.Generic;
using System.Data.SqlClient;
using ApiApplication.Interfaces;
using ApiApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApiApplication.Controllers
{
    [ApiController] //ANNOTTION
    [Route("[controller]")] //ANNOTATION
    public class PatientsController : ControllerBase
    {
        private readonly IPatientsRepository _patientsRepo;
        public PatientsController(IPatientsRepository patientsRepo)
        {
            _patientsRepo = patientsRepo;
        }

        [HttpPost("Create")]
        public async Task<int> CreatePatientAsync([FromBody] Patients patient)
        {
            try
            {
                int patientId = await Task.Run(() => _patientsRepo.CreatePatient(patient));
                return patientId;
            }
            catch (Exception ex) 
            {
                throw new Exception("Error Creating patient: {ex.Message");
            }
        }

        public async Task<List<Patients>> GetAllPatientsAsync()
        {
            try
            {
                List<Patients> patients = await Task.Run(() => _patientsRepo.GetAllPatients());
                return patients;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving patients: {ex.Message}");
            }
        }

        public async Task<Patients> GetPatientByIdAsync(int patientId)
        {
            try
            {
                Patients patient = await Task.Run(() => _patientsRepo.GetPatientById(patientId));
                return patient;
            } 
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving patient: {ex.Message}");
            }
        }

        public async Task<int> UpdatePatientAsync(Patients patient)
        {
            try
            {
                int updatedPatientId = await Task.Run(() => _patientsRepo.UpdatePatient(patient));
                return updatedPatientId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating patient: {ex.Message}");
            }
        }
        public async Task<bool> DeletePatientAsync(int patientId)
        {
            try
            {
                bool disDeleted = await Task.Run(() => _patientsRepo.DeletePatients(patientId));
                return disDeleted;
            }
            catch(Exception ex)
            {
                throw new Exception($"Error deleting patient: {ex.Message}");
            }
        }
       
        
    }
}

