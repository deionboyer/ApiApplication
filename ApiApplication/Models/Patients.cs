namespace ApiApplication.Models
{
    public class Patients
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public ICollection<InsuranceProviders> InsuranceProviders { get; set; }
        // Add other relevant properties (DOB, address, etc.)
    }
}
