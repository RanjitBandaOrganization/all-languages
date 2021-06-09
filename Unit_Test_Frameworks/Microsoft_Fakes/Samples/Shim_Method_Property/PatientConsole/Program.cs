using System;
using System.Collections.Generic;
using PatientRecords;

namespace PatientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Patient patient = new Patient();

            patient.FName = "John";
            patient.LName = "Doe";
            patient.DOB = new DateTime(1990, 01, 02);
            patient.SSN = "123456789";
            patient.Rxs = new List<Prescription>();
            patient.Rxs.Add(new Prescription { DrugId = 123, Dose = 25, DoseUnit = "mg" });
            patient.Rxs.Add(new Prescription { DrugId = 456, Dose = 50, DoseUnit = "mg" });

            new PatientService(1).SaveAndDisplayPatient(patient);

            Console.ReadKey(false);
        }
    }
}
