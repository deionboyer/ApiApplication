using System.Collections.Generic;
using System.Data.SqlClient;
using ApiApplication.Interfaces;
using ApiApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApiApplication.Controllers
{
    [Route("api/[controller]"),AllowAnonymous]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientsRepository _patientsRepo;
        public PatientsController(IPatientsRepository patientsRepo)
        {
            _patientsRepo = patientsRepo;
        }

        [HttpPost("CreatePatient")]
        public async Task<int> CreatePatientAsync([FromBody] Patients patient) 
        {
            try
            {
                int patientId = await Task.Run(() => _patientsRepo.CreatePatient(patient));
                return patientId;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error Creating patient: {ex.Message}");
            }
        }
        [HttpGet("GetAllPatients")]
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
        [HttpGet("GetPatientById/{patientId}")]
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
        [HttpPut("UpdatePatient")]
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
        [HttpDelete("DeletePatient")]
        public async Task<bool> DeletePatientAsync(int patientId)
        {
            try
            {
                bool isDeleted = await Task.Run(() => _patientsRepo.DeletePatients(patientId));
                return isDeleted;
            }
            catch(Exception ex)
            {
                throw new Exception($"Error deleting patient: {ex.Message}");
            }
        }
    }
}

