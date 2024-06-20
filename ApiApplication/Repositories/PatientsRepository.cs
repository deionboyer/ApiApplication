using ApiApplication.Interfaces;
using ApiApplication.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Mail;

namespace ApiApplication.Repositories
{
    public class PatientsRepository : IPatientsRepository
    {
        private readonly string _connectionString;
        public PatientsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //Create NEW PATIENT
        public int CreatePatient(Patients patient)
        {
            int contactId = 0;
            bool isEmailvalid = IsEmailUnique(patient.EmailAddress);
            if (isEmailvalid == false)
            {
                throw new Exception("Emailaddress is already in use.");
            }

            var insertSql = @"INSERT INTO PATIENTS (FIRSTNAME, LASTNAME, EMAILADDRESS)
                                  OUPU  T INSERTED.PATIENTID
                                  VALUES (@FIRSTNAME, @LASTNAME, @EMAILADDRESS)";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(insertSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@FIRSTNAME", patient.FirstName));
                    sqlCommand.Parameters.Add(new SqlParameter("@LASTNAME", patient.LastName));
                    sqlCommand.Parameters.Add(new SqlParameter("@EMAILADDRESS", patient.EmailAddress));

                    sqlCommand.Connection.Open();
                    contactId = (int)sqlCommand.ExecuteScalar();//Executes INSERT query
                    sqlCommand.Connection.Close();
                }//Return ID
            }
            return contactId;
        }

                
        public List<Patients>? GetAllPatients()
        {
            List<Patients> patientsList = new List<Patients>();
            var getAllSql = "SELECT * FROM PATIENTS ORDER BY PATIENTNAME DESC";
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(getAllSql, sqlConnection))
                {
                    using (SqlDataAdapter sqlDataAdtapter = new SqlDataAdapter(sqlCommand))
                    {
                        using (DataTable dataTable = new DataTable())
                        {
                            sqlDataAdtapter.Fill(dataTable);
                            foreach (DataRow row in dataTable.Rows)
                            {
                                Patients patient = new Patients();
                                patient.PatientId = Convert.ToInt32(row["PATIENTID"]);
                                patient.LastName = row["LASTNAME"].ToString();
                                patient.EmailAddress = row["EMAILADDRESS"].ToString();

                                patientsList.Add(patient);
                            }
                        }
                    }

                }
            }
            return patientsList;

        }

        //GET PATIENT BY ID
        public Patients? GetPatientById(int patientId)
        {
            Patients patients = new Patients();

            var singleRecordSql = "SELECT * FROM PATIENTS WHERE PATIENTID = @PATIENTID";
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(singleRecordSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@PATIENTID", patientId));
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();


                            patients.PatientId = Convert.ToInt32(reader["PATIENTID"]);
                            patients.FirstName = reader["FIRSTNAME"].ToString();
                            patients.LastName = reader["LASTNAME"].ToString();
                            patients.EmailAddress = reader["EMAILADDRESS"].ToString();


                        }
                        else
                        {
                            throw new Exception("No rows found.");
                        }
                    }
                    sqlConnection.Close();
                }
            }

            return patients;
        }

        public int UpdatePatient(Patients patients)
        {
            var updateSql = @"UPDATE PATIENTS
                                         SET FIRSTNAME = @FIRSTNAME,
                                             LASTNAME = @LASTNAME,
                                             EMAILADDRESS = @EMAILADDRESS
                                         WHERE PATIENTID = @PATIENTID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(updateSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@FIRSTNAME", patients.FirstName));
                    sqlCommand.Parameters.Add(new SqlParameter("@LASTNAME", patients.LastName));
                    sqlCommand.Parameters.Add(new SqlParameter("@EMAILADDRESS", patients.EmailAddress));
                    sqlCommand.Parameters.Add(new SqlParameter("@PATIENTID", patients.PatientId));

                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();

                }
            }
            return patients.PatientId;
        }

        public bool DeletePatients(int patientId)
        {
            var deleteSql = @"DELETE FROM PATIENTS WHERE PATIENTID = @PATIENTID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(deleteSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@PATIENTID", patientId));

                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            return true;
        }
        public bool IsEmailUnique(string emailAddress)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                // Create a parameterized query to avoid SQL injection
                string query = "SELECT COUNT(*) FROM Patients WHERE EmailAddress = @EmailAddress";
                using (var command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@EmailAddress", emailAddress);

                    int count = (int)command.ExecuteScalar();
                    return count == 0; // If count is 0, email is unique
                }
            }
        }
    }
}

