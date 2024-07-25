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
                    sqlCommand.Parameters.Add(new SqlParameter("@STATE", provider.State));
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCENAME",provider.InsuranceName));
                    sqlCommand.Parameters.Add(new SqlParameter("@COPAY",provider.CoPay));

                    sqlConnection.Open();
                    insuranceId = (int)sqlCommand.ExecuteScalar();
                    sqlCommand.Connection.Close();
                }
            }
            return insuranceId;
        }

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
                            sqlDataAdapter.Fill(dataTable);
                            foreach (DataRow row in dataTable.Rows)
                            {
                                InsuranceProviders providers = new InsuranceProviders();
                                providers.InsuranceId = Convert.ToInt32(row["INSURANCEID"]);
                                providers.State = row["STATE"].ToString();
                                providers.InsuranceName = row["INSURANCENAME"].ToString();
                                providers.CoPay = Convert.ToInt32(row["COPAY"]);

                                providersList.Add(providers);
                            }
                        }
                    }
                }
            }
            return providersList;
        }

        public InsuranceProviders? GetProviderById(int insuranceId)
        {
            InsuranceProviders providers = new InsuranceProviders();
            var singleRecordSql = @"SELECT * FROM INSURANCEPROVIDER WHERE INSURANCEID = @INSURANCEID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(singleRecordSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("INSURANCEID", insuranceId));
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

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
            return providers;
        }

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
                    sqlCommand.Parameters.Add(new SqlParameter("@STATE", providers.State));
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCENAME",providers.InsuranceName));
                    sqlCommand.Parameters.Add(new SqlParameter("@COPAY", providers.CoPay));

                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            return providers.InsuranceId;
        }

        public bool DeleteProvider(int insuranceId)
        {
            var deleteSql = @"DELETE FROM INSURANCEPROVIDER WHERE INSURANCEID = @INSURANCEID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(deleteSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCEID", insuranceId));

                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            return true;
        }
    }
}
