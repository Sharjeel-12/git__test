using NewPatientProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewPatientProject.Services
{
    public class FileManager
    {
        /*
         Lets list up some functions of this class: -
        1- Read the file and prepare the patient objects from the csv file: PatientRecords.csv
        2- Write data to the .csv file using the loaded 
        3- we write data as: WriteAllText(_FirstLine);AppendAllLines(_ObjStrings)

         */
        private static readonly string _FilePath = @"../../../PatientRecords.csv";
        public static List<Patient> PatientObjects_Loaded= new List<Patient>();
        private static string _FirstLine = "Name,VisitDate,VisitTime,VisitType,Description,DoctorName,Duration(minutes)";
        private static List<string> _ObjStrings= new List<string>();



        // Consturctor
        public void ReadFile()
        {
            if (File.Exists(_FilePath))
            {
                string[] allLines = File.ReadAllLines(_FilePath);

                if (allLines.Length == 0)
                {
                    _ObjStrings.Clear(); // No data
                }
                else
                {
                    string firstLine = allLines[0].Trim();

                    if (string.Equals(firstLine, _FirstLine.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        _ObjStrings = allLines.Skip(1).ToList();  // Skip header
                    }
                    else
                    {
                        _ObjStrings = allLines.ToList(); // No header
                    }
                }
            }
            else
            {
                // Create file with header
                File.WriteAllText(_FilePath, _FirstLine + Environment.NewLine);
                _ObjStrings.Clear();
            }

        }
        public List<Patient> PreparePatientObjectsFromFile()
        {
            PatientObjects_Loaded.Clear();

            if (_ObjStrings.Count > 0)
            {
                foreach (string str in _ObjStrings) {
                    string[] Patient_params=str.Split(',');
                    Patient patient=new Patient(Patient_params[0], DateOnly.ParseExact(Patient_params[1], new string[] { "dd-MM-yyyy","M/d/yyyy" }), TimeOnly.ParseExact(Patient_params[2], new string[] { "HH:mm:ss" ,"H:mm", "h:mm tt" }), Patient_params[3], Patient_params[4], Patient_params[5], Convert.ToInt32(Patient_params[6]));
                    PatientObjects_Loaded.Add(patient);
                }
            }
            
            return PatientObjects_Loaded;
            // this returned list of objects will be going to the Record Manager for further processing
        }
        public List<string> ConvertObjsToStrings(List<Patient> PatientObjects)
        {
            List<string> ObjectStrings=new List<string>();
            foreach (Patient obj in PatientObjects)
            {
                ObjectStrings.Add($"{obj.Name},{obj.VisitDate.ToString()},{obj.VisitTime.ToString()},{obj.VisitType},{obj.Description},{obj.DoctorName},{obj.Duration}");

            }
            return ObjectStrings;
        }
        // function to write all the patient records in a .csv file
        public void WritePatientRecord(List<Patient> PatientObjects)
        {
            List<string> data_strings=ConvertObjsToStrings(PatientObjects);

            List<string> allLinesToWrite = new List<string> { _FirstLine };
            allLinesToWrite.AddRange(data_strings);
            File.WriteAllLines(_FilePath, allLinesToWrite);

        }

        // function  display the records of the list of Patients given to it.
        public  void DisplayPatientRecord(List<Patient> List_of_Patients)
        {
            
            if(List_of_Patients.Count > 0)
            {
                foreach(Patient patient in List_of_Patients)
                {

                    Console.WriteLine("\n**************************************************\n");
                    Console.WriteLine($"Name of Patient: {patient.Name}");
                    Console.WriteLine($"Visit Date: {patient.VisitDate.ToString()}");
                    Console.WriteLine($"Visit Time: {patient.VisitTime.ToString()}");
                    Console.WriteLine($"Type of Visit: {patient.VisitType}");
                    Console.WriteLine($"Desciption: {patient.Description}");
                    Console.WriteLine($"DoctorName: {patient.DoctorName}");
                    Console.WriteLine($"Duration (minutes): {patient.Duration}");
                    Console.WriteLine("\nn**************************************************\n\n");

                }
            }
            else
            {
                Console.Write("Nothing found\n\n");
                Console.Write("There is no record to display");
            }
        }
        // function to filter and display the sorted results

        public void FilterByName(List<Patient> AllPatients,string name)
        {
            
            var filtered=AllPatients.Where(patient=>patient.Name == name).ToList();
            filtered.Sort((p1, p2) => p1.VisitType.CompareTo(p2.VisitType));
            DisplayPatientRecord(filtered);
        }
        public void FilterByDate(List<Patient> AllPatients,DateOnly date)
        {
            
            var filtered = AllPatients.Where(patient => patient.VisitDate == date).ToList();
            filtered.Sort((p1, p2) => p1.VisitDate.CompareTo(p2.VisitDate));
            DisplayPatientRecord(filtered);
        }

        public void FilterByVisitType(List<Patient> AllPatients,string visit_type) 
        {
            
            var filtered = AllPatients.Where(patient => patient.VisitType == visit_type).ToList();
            filtered.Sort((p1, p2) => p1.Name.CompareTo(p2.Name));
            DisplayPatientRecord(filtered);

        }


        public Patient GetPatientFromName(List<Patient> AllPatients,string name)
        {   
            Patient patient=null;
            
            foreach(Patient pat in AllPatients)
            {
                if (pat.Name == name)
                    patient = pat;
                }
            
            return patient;
        }






        /******************************************************************/

    }
}
