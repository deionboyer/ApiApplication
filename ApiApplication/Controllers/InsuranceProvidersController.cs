using ApiApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiApplication.Controllers
{
    [ApiController] //ANNOTTION
    [Route("[controller]")] //ANNOTATION
    public class InsuranceProvidersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        public InsuranceProvidersController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        private readonly List<InsuranceProviders> _providers = new List<InsuranceProviders>();
        //private readonly ILogger<InsuranceProvidersController> _logger;
        //public InsuranceProvidersController(ILogger<InsuranceProvidersController> logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet("GetAllProviders")]
        public IActionResult GetAllProviders(int id) //List of providers
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var querySql = "SELECT * FROM InsuranceProviders";
                var command = new SqlCommand(querySql, connection);
                var providers = new List<InsuranceProviders>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var provider = new InsuranceProviders
                        {
                            InsuranceId = Convert.ToInt32(reader["InsuranceId"]),
                            State = reader["State"].ToString(),
                            InsuranceName = reader["InsuranceName"].ToString(),
                            Covered = Convert.ToBoolean(reader["Covered"]),
                            CoPay = (int)Convert.ToDecimal(reader["CoPay"])
                        };
                        providers.Add(provider);
                    }
                }

                return Ok(providers);
                // Return Ok(patient) or NotFound() if not found
            }
        }
      

        [HttpGet("GetProviderById")]
        public IActionResult GetProviderByID(int insuranceId) ///return providers
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var querySql = "SELECT * FROM InsuranceProviders WHERE @InsuranceID = InsuranceID";
                var command = new SqlCommand(querySql,connection);
                command.Parameters.AddWithValue(@"InsuranceID", insuranceId);

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var providers = new InsuranceProviders()
                        {
                            InsuranceId = Convert.ToInt32(reader["InsuranceId"]),
                            State = reader["State"].ToString(),
                            InsuranceName = reader["InsuranceName"].ToString(),
                            Covered = Convert.ToBoolean(reader["Covered"]),
                            CoPay = (int)Convert.ToDecimal(reader["CoPay"])
                        };
                        return Ok(providers);
                    }
                    else
                    {
                        return NotFound("Patient not found");
                    }
                }
            }
        }
    }
}
