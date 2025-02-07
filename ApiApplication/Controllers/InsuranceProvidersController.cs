﻿using ApiApplication.Interfaces;
using ApiApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceProvidersController : ControllerBase
    {
        private readonly IInsuranceProvidersRepository _insuranceProvidersRepo;
        public InsuranceProvidersController(IInsuranceProvidersRepository insuranceProvidersRepo)
        {
            _insuranceProvidersRepo = insuranceProvidersRepo;
        }
        [HttpPost("CreateProvider")]
        public async Task<int> CreateProviderAsync([FromBody] InsuranceProviders providers)
        {
            try
            {
                // Create a new insurance provider asynchronously and return the insurance ID
                int insuranceId = await Task.Run(() => _insuranceProvidersRepo.CreateInsuranceProvider(providers));
                return insuranceId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating insurance provider: {ex.Message}");
            }
        }
        [HttpGet("GetAllProviders")]
        public async Task<List<InsuranceProviders>> GetAllProvidersAsync()
        {
            try
            {
                // Retrieve all insurance providers asynchronously and return the list of providers
                List<InsuranceProviders> providersList = await Task.Run(() => _insuranceProvidersRepo.GetAllProviders());
                return providersList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving insurance providers: {ex.Message}");
            }
        }
        [HttpGet("GetProviderById")]
        public async Task<InsuranceProviders> GetProviderByIdAsync(int insuranceId)
        {
            try
            {
                // Retrieve an insurance provider by ID asynchronously and return the provider
                InsuranceProviders provider = await Task.Run(() => _insuranceProvidersRepo.GetProviderById(insuranceId));
                return provider;
            }
            catch(Exception ex)
            {
                throw new Exception($"Error retrieving insurance provider: {ex.Message}");
            }
        }
        [HttpPut("UpdateProvider")]
        public async Task<int> UpdateProviderAsync(InsuranceProviders provider)
        {
            try
            {
                // Update an insurance provider asynchronously and return the updated provider ID
                int updatedProvider = await Task.Run(() => _insuranceProvidersRepo.UpdateProviders(provider));
                return updatedProvider;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating insurance provider: {ex.Message}");
            }
        }
        [HttpDelete("DeleteProvider")]
        public async Task<bool> DeleteProviderAsync(int insuranceId)
        {
            try
            {
                // Delete an insurance provider asynchronously and return a boolean indicating success
                bool isDeleted = await Task.Run(() => _insuranceProvidersRepo.DeleteProvider(insuranceId));
                return isDeleted;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting insurance provider: {ex.Message}");
            }
        }
    }
}
