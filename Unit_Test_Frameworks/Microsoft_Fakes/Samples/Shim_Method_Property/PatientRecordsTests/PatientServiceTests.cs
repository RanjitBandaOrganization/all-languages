using System;
using System.Collections.Generic;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientConsole;
using PatientConsole.Fakes;
using PatientRecords;
using PatientRecords.Fakes;

namespace PatientRecordsTests
{
    [TestClass]
    public class PatientServiceTests
    {
        [TestMethod]
        public void SaveAndDisplayPatient_SavesThePatient_SHIMMING_STATIC_METHODS_Basic()
        {
            Patient patient = new Patient();
            patient.FName = "John";
            patient.LName = "Doe";
            patient.DOB = new DateTime(1990, 01, 02);
            patient.SSN = "123456789";
            patient.Rxs = new List<Prescription>();
            patient.Rxs.Add(new Prescription { DrugId = 123, Dose = 25, DoseUnit = "mg" });
            patient.Rxs.Add(new Prescription { DrugId = 456, Dose = 50, DoseUnit = "mg" });

            Patient savedPatient = null;

            using(ShimsContext.Create())
            {
                //SHIMMING THE STATIC METHODS
                ShimPatient.SavePatient = (patientParam) => { savedPatient = patientParam; };
                ShimPatient.LoadString = (ssn) => { return new Patient() { Rxs = new List<Prescription>() { new Prescription() } } ; };

                new PatientService(1).SaveAndDisplayPatient(patient);
            }

            Assert.AreSame(patient, savedPatient);
        }

        [TestMethod]
        public void SaveAndDisplayPatient_SavesThePatient__SHIMMING_STATIC_AND_PRIVATE_METHODS_Advanced()
        {
            Patient patient = new Patient();
            patient.FName = "John";
            patient.LName = "Doe";
            patient.DOB = new DateTime(1990, 01, 02);
            patient.SSN = "123456789";
            patient.Rxs = new List<Prescription>();
            patient.Rxs.Add(new Prescription { DrugId = 123, Dose = 25, DoseUnit = "mg" });
            patient.Rxs.Add(new Prescription { DrugId = 456, Dose = 50, DoseUnit = "mg" });

            Patient savedPatient = null;

            using (ShimsContext.Create())
            {
                ShimPatient.SavePatient = (patientParam) => { savedPatient = patientParam; };
                ShimPatient.LoadString = (ssn) => { return new Patient(); };

                ShimPatientService.AllInstances.OutputPatientPatient = (service, patientParam) => { };

                ShimPatientService.ConstructorInt32 = (service, i) => { service.DepartmentLastVisited = "RAD"; };

                new PatientService(1).SaveAndDisplayPatient(patient);
            }

            Assert.AreSame(patient, savedPatient);
            Assert.AreEqual("RAD", savedPatient.DepartmentLastVisited);
        }
    }
}
