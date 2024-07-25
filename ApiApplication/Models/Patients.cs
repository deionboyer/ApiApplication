namespace ApiApplication.Models
{
    public class Patients
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber {  get; set; }
        public string BirthDate { get; set; }
        public string EmailAddress { get; set; }
        public int InsuranceId { get; set; } // this will be  fereign key of id public int InsuranceId { get; set; } 
        // Add other relevant properties (DOB, address, etc.)
    }
}
