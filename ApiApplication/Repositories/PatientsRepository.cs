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
            int patientId = 0;
            bool isEmailvalid = IsEmailUnique(patient.EmailAddress);
            if (isEmailvalid == false)
            {
                throw new Exception("Emailaddress is already in use.");
            }

            var insertSql = @"INSERT INTO PATIENTS (FIRSTNAME, LASTNAME, PHONENUMBER,BIRTHDATE,EMAILADDRESS,INSURANCEID)
                                  OUTPUT INSERTED.PATIENTID
                                  VALUES (@FIRSTNAME, @LASTNAME,@PHONENUMBER,@BIRTHDATE,@EMAILADDRESS,@INSURANCEID)";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(insertSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@FIRSTNAME", patient.FirstName));
                    sqlCommand.Parameters.Add(new SqlParameter("@LASTNAME", patient.LastName));
                    sqlCommand.Parameters.Add(new SqlParameter("@PHONENUMBER", patient.PhoneNumber));
                    sqlCommand.Parameters.Add(new SqlParameter("@BIRTHDATE", patient.BirthDate));
                    sqlCommand.Parameters.Add(new SqlParameter("@EMAILADDRESS", patient.EmailAddress));
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCEID",patient.InsuranceId));
                    ///create
                    sqlCommand.Connection.Open();
                    patientId = (int)sqlCommand.ExecuteScalar();//Executes INSERT query
                    sqlCommand.Connection.Close();
                }//Return ID
            }
            return patientId;
        }

                
        public List<Patients>? GetAllPatients()
        {
            List<Patients> patientsList = new List<Patients>();
            var getAllSql = "SELECT * FROM PATIENTS ORDER BY LASTNAME ASC";
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(getAllSql, sqlConnection))
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
                                patient.FirstName = row["FIRSTNAME"].ToString();
                                patient.LastName = row["LASTNAME"].ToString();
                                patient.PhoneNumber = row["PHONENUMBER"].ToString();
                                patient.BirthDate = row["BIRTHDATE"].ToString();
                                patient.EmailAddress = row["EMAILADDRESS"].ToString();
                                patient.InsuranceId = Convert.ToInt32(row["INSURANCEID"]);

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
                    sqlCommand.Connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();


                            patients.PatientId = Convert.ToInt32(reader["PATIENTID"]);
                            patients.FirstName = reader["FIRSTNAME"].ToString();
                            patients.LastName = reader["LASTNAME"].ToString();
                            patients.PhoneNumber = reader["PhoneNumber"].ToString();
                            patients.BirthDate = reader["BIRTHDATE"].ToString();
                            patients.EmailAddress = reader["EMAILADDRESS"].ToString();
                            patients.InsuranceId = Convert.ToInt32(reader["INSURANCEID"]);


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
                                             PHONENUMBER = @PHONENUMBER,
                                             BIRTHDATE = @BIRTHDATE,
                                             EMAILADDRESS = @EMAILADDRESS,
                                             INSURANCEID = @INSURANCEID
                                         WHERE PATIENTID = @PATIENTID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(updateSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@FIRSTNAME", patients.FirstName));
                    sqlCommand.Parameters.Add(new SqlParameter("@LASTNAME", patients.LastName));
                    sqlCommand.Parameters.Add(new SqlParameter("@PHONENUMBER",patients.PhoneNumber));
                    sqlCommand.Parameters.Add(new SqlParameter("BIRTHDATE",patients.BirthDate));
                    sqlCommand.Parameters.Add(new SqlParameter("@EMAILADDRESS", patients.EmailAddress));
                    sqlCommand.Parameters.Add(new SqlParameter("@PATIENTID", patients.PatientId));
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCEID", patients.InsuranceId));
                   

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

