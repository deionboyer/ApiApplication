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
            bool isEmailvalid = IsEmailUnique(patient.EmailAddress); // Check if the email is unique
            if (isEmailvalid == false)
            {
                throw new Exception("Email Address is already in use."); // Throw an exception if the email is not unique
            }

            var insertSql = @"INSERT INTO PATIENTS (FIRSTNAME, LASTNAME, PHONENUMBER,BIRTHDATE,EMAILADDRESS,INSURANCEID)
                                  OUTPUT INSERTED.PATIENTID
                                  VALUES (@FIRSTNAME, @LASTNAME,@PHONENUMBER,@BIRTHDATE,@EMAILADDRESS,@INSURANCEID)";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(insertSql, sqlConnection))
                {
                    // Add parameters to the SQL command
                    sqlCommand.Parameters.Add(new SqlParameter("@FIRSTNAME", patient.FirstName));
                    sqlCommand.Parameters.Add(new SqlParameter("@LASTNAME", patient.LastName));
                    sqlCommand.Parameters.Add(new SqlParameter("@PHONENUMBER", patient.PhoneNumber));
                    sqlCommand.Parameters.Add(new SqlParameter("@BIRTHDATE", patient.BirthDate));
                    sqlCommand.Parameters.Add(new SqlParameter("@EMAILADDRESS", patient.EmailAddress));
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCEID",patient.InsuranceId));
                    // Open the connection, execute the command, and get the inserted patient ID
                    sqlCommand.Connection.Open();
                    patientId = (int)sqlCommand.ExecuteScalar();//Executes INSERT query
                    sqlCommand.Connection.Close();
                }
            }
            return patientId; // Return the ID of the created patient
        }
        // Get all patients    
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
                            sqlDataAdtapter.Fill(dataTable); // Fill the DataTable with the result of the query
                            foreach (DataRow row in dataTable.Rows)
                            {
                                // Create a new patient object and populate its properties
                                Patients patient = new Patients();
                                patient.PatientId = Convert.ToInt32(row["PATIENTID"]);
                                patient.FirstName = row["FIRSTNAME"].ToString();
                                patient.LastName = row["LASTNAME"].ToString();
                                patient.PhoneNumber = row["PHONENUMBER"].ToString();
                                patient.BirthDate = row["BIRTHDATE"].ToString();
                                patient.EmailAddress = row["EMAILADDRESS"].ToString();
                                patient.InsuranceId = Convert.ToInt32(row["INSURANCEID"]);

                                patientsList.Add(patient); // Add the patient to the list
                            }
                        }
                    }
                }
            }
            return patientsList; // Return the list of patients
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
                    sqlCommand.Parameters.Add(new SqlParameter("@PATIENTID", patientId)); // Add the patient ID parameter
                    sqlCommand.Connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            // Populate the patient object with the data from the reader
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
                            throw new Exception("No rows found."); // Throw an exception if no rows are found
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return patients; // Return the patient object
        }
        //Update a Patient
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
                    // Add parameters to the SQL command
                    sqlCommand.Parameters.Add(new SqlParameter("@FIRSTNAME", patients.FirstName));
                    sqlCommand.Parameters.Add(new SqlParameter("@LASTNAME", patients.LastName));
                    sqlCommand.Parameters.Add(new SqlParameter("@PHONENUMBER",patients.PhoneNumber));
                    sqlCommand.Parameters.Add(new SqlParameter("BIRTHDATE",patients.BirthDate));
                    sqlCommand.Parameters.Add(new SqlParameter("@EMAILADDRESS", patients.EmailAddress));
                    sqlCommand.Parameters.Add(new SqlParameter("@PATIENTID", patients.PatientId));
                    sqlCommand.Parameters.Add(new SqlParameter("@INSURANCEID", patients.InsuranceId));
                    // Open the connection and execute the command
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            return patients.PatientId; // Return the ID of the updated patient
        }
        // Delete a patient by ID
        public bool DeletePatients(int patientId)
        {
            var deleteSql = @"DELETE FROM PATIENTS WHERE PATIENTID = @PATIENTID";
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(deleteSql, sqlConnection))
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@PATIENTID", patientId)); // Add the patient ID parameter
                    // Open the connection and execute the command
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            return true; // Return true to indicate successful deletion
        }
        // Check if an email address is unique
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

