using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PatientRecords
{
    public class Patient
    {
        public string SSN { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime DOB { get; set; }
        public string DepartmentLastVisited { get; set; }
        public List<Prescription> Rxs { get; set; }

        public static Patient Load(string ssn)
        {
            string serPat = File.ReadAllText("./" + ssn + ".txt");
            return JsonConvert.DeserializeObject<Patient>(serPat);
        }

        public static void Save(Patient patient)
        {
            string serPat =  JsonConvert.SerializeObject(patient);
            TextWriter tw = new StreamWriter("./" + patient.SSN + ".txt", false);
            tw.Write(serPat);
            tw.Close();
        }
    }
}
