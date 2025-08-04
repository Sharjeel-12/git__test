using NewPatientProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace NewPatientProject.Services
{
    public class myJSON
    {
        public int Consultation_Fee { get; set; }
        public int FollowUp_Fee { get; set; }
        public int Emergency_Fee { get; set; }
    }
    public class FeeCalculator
    {

        private string _FilePath = "../../../PatientFeeRecord.txt";
        private string jsonString = File.ReadAllText("../../../Services/fees.json");
        
        public List<string> CalculateAndWriteFee(List<Patient> AllPAtients)
        {
            myJSON fees_json=JsonSerializer.Deserialize<myJSON>(jsonString);
            List<string> PatientFeeRecord = new List<string>();
            foreach(Patient p in AllPAtients)
            {
                int patient_fee=1;

                string visit_type = p.VisitType.ToLower();
                if (visit_type == "consultation")
                {
                    patient_fee = 500;
                }

                if (visit_type == "follow-up")
                {
                    patient_fee = 300;
                }
                if(visit_type == "emergency")
                {
                    patient_fee = 1000;
                }
                        
                
                PatientFeeRecord.Add($"Patient Name: {p.Name}    Visit Type: {p.VisitType}    Duration: {p.Duration}    Visit Fee: {patient_fee*p.Duration }");
            }
            File.WriteAllLines(_FilePath, PatientFeeRecord);
            return PatientFeeRecord;
        }




    }
}
