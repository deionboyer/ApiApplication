namespace ApiApplication.Models
{
    public class InsuranceProviders
    {
        public int InsuranceId { get; set; }
        public string State {  get; set; }
        public string InsuranceName {  get; set; }
        public bool Covered {  get; set; }
        public int CoPay {  get; set; }
    }
}