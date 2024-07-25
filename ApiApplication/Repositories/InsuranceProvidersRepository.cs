using ApiApplication.Interfaces;
using ApiApplication.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiApplication.Repositories
{
    public class InsuranceProvidersRepository : IInsuranceProvidersRepository
    {
        private readonly string _connectionString;
        public InsuranceProvidersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // Method to create a new insurance provider
        public int CreateInsuranceProvider(InsuranceProviders provider)
        {
            int insuranceId = 0;
            string insertSql = @"INSERT INTO INSURANCEPROVIDER (STATE,INSURANCENAME,COPAY)
                                     OUTPUT INSERTED.INSURANCEID
                                     VALUES (@STATE, @INSURANCENAME,@COPAY)";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(insertSql, sqlConnection))
                {
                    // Add parameters to the SQL command
                    sqlCommand.Parameters.Add(new SqlParameter("@STATE", provider.State));
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCENAME",provider.InsuranceName));
                    sqlCommand.Parameters.Add(new SqlParameter("@COPAY",provider.CoPay));
                    // Open the connection, execute the command, and get the inserted insurance ID
                    sqlConnection.Open();
                    insuranceId = (int)sqlCommand.ExecuteScalar();
                    sqlCommand.Connection.Close();
                }
            }
            return insuranceId; // Return the ID of the created insurance provider
        }
        // Method to get all insurance providers
        public List<InsuranceProviders> GetAllProviders()
        {
            List<InsuranceProviders> providersList = new List<InsuranceProviders>();
            var getAllSql = @"SELECT * FROM INSURANCEPROVIDER ORDER BY INSURANCEID DESC";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(@getAllSql, sqlConnection))
                {
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        using (DataTable dataTable = new DataTable())
                        {
                            sqlDataAdapter.Fill(dataTable); // Fill the DataTable with the result of the query
                            foreach (DataRow row in dataTable.Rows)
                            {
                                // Create a new insurance provider object and populate its properties
                                InsuranceProviders providers = new InsuranceProviders();
                                providers.InsuranceId = Convert.ToInt32(row["INSURANCEID"]);
                                providers.State = row["STATE"].ToString();
                                providers.InsuranceName = row["INSURANCENAME"].ToString();
                                providers.CoPay = Convert.ToInt32(row["COPAY"]);

                                providersList.Add(providers); // Add the provider to the list
                            }
                        }
                    }
                }
            }
            return providersList; // Return the list of insurance providers
        }
        // Method to get an insurance provider by ID
        public InsuranceProviders? GetProviderById(int insuranceId)
        {
            InsuranceProviders providers = new InsuranceProviders();
            var singleRecordSql = @"SELECT * FROM INSURANCEPROVIDER WHERE INSURANCEID = @INSURANCEID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(singleRecordSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("INSURANCEID", insuranceId)); // Add the insurance ID parameter
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            // Populate the insurance provider object with the data from the reader
                            providers.InsuranceId = Convert.ToInt32(reader["INSURANCEID"]);
                            providers.State = reader["STATE"].ToString();
                            providers.InsuranceName = reader["INSURANCENAME"].ToString();
                            providers.CoPay = Convert.ToInt32(reader["COPAY"]);
                        }
                        else
                        {
                            throw new Exception("No rows found");
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return providers; // Return the insurance provider object
        }
        // Method to update an insurance provider
        public int UpdateProviders(InsuranceProviders providers)
        {
            var updateSql = @"UPDATE INSURANCEPROVIDER
                              SET STATE = @STATE,
                                  INSURANCENAME = @INSURANCENAME,
                                  COPAY = @COPAY
                              WHERE INSURANCEID = @INSURANCEID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(updateSql, sqlConnection))
                {
                    // Add parameters to the SQL command
                    sqlCommand.Parameters.Add(new SqlParameter("@STATE", providers.State));
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCENAME",providers.InsuranceName));
                    sqlCommand.Parameters.Add(new SqlParameter("@COPAY", providers.CoPay));
                    // Open the connection and execute the command
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            return providers.InsuranceId; // Return the ID of the updated insurance provider
        }
        // Method to delete an insurance provider by ID
        public bool DeleteProvider(int insuranceId)
        {
            var deleteSql = @"DELETE FROM INSURANCEPROVIDER WHERE INSURANCEID = @INSURANCEID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(deleteSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCEID", insuranceId)); // Add the insurance ID parameter
                    // Open the connection and execute the command
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            return true; // Return true to indicate successful deletion
        }
    }
}
