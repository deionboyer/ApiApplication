using ApiApplication.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace ApiApplication.Repositories
{
    public class InsuranceProvidersRepository
    {
        private readonly string _connectionString;
        public InsuranceProvidersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int CreateInsuranceProvider(InsuranceProviders provider)
        {
            int insuranceId = 0;
            string insertSql = @"INSERT INTO INSURANCEPROVIDERS (STATE,INSURANCENAME,COVERED,COPAY)
                                     OUTPUT INSERTED.INSURANCEID
                                     VALUES (@STATE, @COVERED,@COPAY)";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(insertSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@STATE", provider.State));
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCENAME",provider.InsuranceName));
                    sqlCommand.Parameters.Add(new SqlParameter("@COVERED", provider.Covered));
                    sqlCommand.Parameters.Add(new SqlParameter("@COPAY",provider.CoPay));

                    sqlCommand.Connection.Open();
                    insuranceId = (int)sqlCommand.ExecuteScalar();
                    sqlCommand.Connection.Close();
                }
            }
            return insuranceId;
        }

        public List<InsuranceProviders> GetAllProviders()
        {
            List<InsuranceProviders> providersList = new List<InsuranceProviders>();
            var getAllSql = @"SELECT * FROM INSURANCEPROVIDERS ORDER BY INSURANCENAME DESC";
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
                                providers.Covered = Convert.ToBoolean(row["COVERED"]);
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
            var singleRecordSql = @"SELECT * FROM INSURANCEPROVIDERS WHERE INSURANCEID = @INSURANCEID";
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
                            providers.Covered = Convert.ToBoolean(reader["COVERED"]);
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
            var updateSql = @"UPDATE INSURANCEPROVIDERS
                              SET STATE = @STATE,
                                  INSURANCENAME = @INSURANCENAME,
                                  COVERED = @COVERED,
                                  COPAY = @COPAY
                              WHERE INSURANCEID = @INSURANCEID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(updateSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@STATE", providers.State));
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCENAME",providers.InsuranceName));
                    sqlCommand.Parameters.Add(new SqlParameter("@COVERED", providers.Covered));
                    sqlCommand.Parameters.Add(new SqlParameter("@COPAY", providers.CoPay));

                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            return providers.InsuranceId;
        }

        public bool DeleteProvider(int insiranceId)
        {
            var deleteSql = @"DELETE FROM INSURANCEPROVIDERS WHERE INSURANCEID = @INSURANCEID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(deleteSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCEID", insiranceId));

                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            return true;
        }
    }
}
