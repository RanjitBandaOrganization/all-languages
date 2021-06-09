using System;
//using System.Data;
//using System.Data.SqlClient;
using PatientRecords;

namespace PatientConsole
{
    public class PatientService
    {
        public string DepartmentLastVisited { get; set; }

        public PatientService(int departmentLastVisited)
        {
            //SqlCommand sqlCommand = new SqlCommand();
            //sqlCommand.CommandText = "SELECT DepartmentCode FROM Departments WHERE DepartmentId = @DepartmentId";
            //sqlCommand.Parameters.AddWithValue("@DepartmentId", departmentLastVisited);
            //sqlCommand.Connection = new SqlConnection("connectionstring");
            //sqlCommand.Connection.Open();

            //DepartmentLastVisited = (string)sqlCommand.ExecuteScalar();
            //sqlCommand.Connection.Close();
        }

        public void SaveAndDisplayPatient(Patient patient)
        {
            patient.DepartmentLastVisited = DepartmentLastVisited;

            Patient.Save(patient);

            Patient loadedPatient = Patient.Load(patient.SSN);

            OutputPatient(loadedPatient);
        }

        private void OutputPatient(Patient loadedPatient)
        {
            Console.WriteLine(loadedPatient.FName);
            Console.WriteLine(loadedPatient.LName);
            Console.WriteLine(loadedPatient.DOB.ToShortDateString());
            Console.WriteLine(loadedPatient.SSN);
            Console.WriteLine("{0}, {1} {2}", loadedPatient.Rxs[0].DrugId, loadedPatient.Rxs[0].Dose, loadedPatient.Rxs[0].DoseUnit);
        }
    }
}