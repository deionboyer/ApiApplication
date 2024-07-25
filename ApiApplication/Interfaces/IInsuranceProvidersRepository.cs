using ApiApplication.Models;

namespace ApiApplication.Interfaces
{
    public interface IInsuranceProvidersRepository
    {
        int CreateInsuranceProvider(InsuranceProviders provider);
        List<InsuranceProviders> GetAllProviders();
        InsuranceProviders? GetProviderById(int insuranceId);
        int UpdateProviders(InsuranceProviders providers);
        bool DeleteProvider(int insuranceId);
    }
}
