using NewPatientProject.Models;
using NewPatientProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewPatientProject.Interfaces
{
    public interface IAdminInteraction
    {
        void AddPatientRecord();
        void DeletePatientRecord();
        void SearchPatientRecord();
        void UpdatePatient();
        void ViewAllPatientRecords();
        void UndoAction();
        void RedoAction();
        void FilterbyName(string name);
        void FilterbyDate(string date);
        void FilterbyVisitType(string visit_type);
        void GenerateSummaryAndStats();



    }
    public interface IReceptionInteraction
    {
        void SearchPatientRecord();
        void ViewAllPatientRecords();
        void FilterbyName(string name);
        void FilterbyDate(string date);
        void FilterbyVisitType(string visit_type);
        void GenerateSummaryAndStats();

    }
    public class Admin:IAdminInteraction
    {
        
        // Preparing the managers of the patient data as follows: -

        public FileManager file_manager = new FileManager();
        public RecordManager record_manager = new RecordManager();
        public UndoRedoManager undo_redo_manager = new UndoRedoManager();
        
        private static string[] VisitTypes = new string[] { "consultation", "follow-up", "emergency" };
        public Patient PreparePatientFromPrompt()
        {
            // Creating User Prompt for entering patient data: -

            
            Console.Write("Enter the Patient Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter the Patient Visit Date (format: yyyy-mm-dd) :- ");
            string date = Console.ReadLine();
            //Making sure the correct date is entered by the user.....
            while (!(DateOnly.TryParseExact(date, "yyyy-MM-dd", out DateOnly result)))
            {
                Console.WriteLine("You entered the date in an invalid format");
                Console.WriteLine("Please re-enter the date in the corect format (yyyy-MM-dd)");
                date = Console.ReadLine();
            }
            Console.Write("Enter the Patient Visit Time (format: HH:mm:ss) :- ");
            string time = Console.ReadLine();
            //Making sure the correct meeting time is entered by the user.....
            while (!(TimeOnly.TryParseExact(time, "HH:mm:ss", out TimeOnly result)))
            {
                Console.WriteLine("You entered the meeting time in an invalid format");
                Console.WriteLine("Please re-enter the visit in the corect format (HH:mm:ss)");
                time = Console.ReadLine();
            }


            Console.Write("Enter the Patient Visit Type (Consultation, Follow-Up, Emergency) :- ");
            string visit_type = Console.ReadLine();
            //Making sure the correct visit type is entered by the user.....
            while (!VisitTypes.Contains(visit_type.ToLower()))
            {
                Console.WriteLine("You entered invalid visit type");
                Console.WriteLine("Please re-enter the visit type of the patient as one of the following: Consultation, Follow-Up, Emergency");
                visit_type = Console.ReadLine();

            }


            Console.Write("Enter the doctor name for consultation :- ");
            string dr = Console.ReadLine();
            Console.Write("Enter the patient description :- ");
            string desc = Console.ReadLine();
            Console.Write("Enter the visit duration :- ");
            int duration = Convert.ToInt32(Console.ReadLine());

            // Preparing the Patient Object
            Patient patient = new Patient(name, DateOnly.ParseExact(date, "yyyy-MM-dd"), TimeOnly.ParseExact(time, "HH:mm:ss"), visit_type, desc, dr, duration);

            return patient;
        }
        public void AddPatientRecord()
        {
            string action = "Added";
            string status = "failed";
            string permission="";
            // Calling functions from their respective classes
            file_manager.ReadFile();
            undo_redo_manager.ReadFiles();

            Console.WriteLine("Please Enter the details of the new Patient");
            Patient patient=PreparePatientFromPrompt();
            List<Patient>AllPatients=file_manager.PreparePatientObjectsFromFile();
            //////////////////////////////////////////////////////////////////////
            foreach(Patient pat in AllPatients)
            {
                if (pat.Name == patient.Name)
                {
                    double time_gap = (patient.VisitTime - pat.VisitTime).TotalMinutes;
                    if (time_gap < 30)
                    {
                        Console.WriteLine($"The patient {patient.Name} already has a visit scheduled and is conflicting with the other visit by more than 30 minutes (i.e. by {time_gap} minutes)");
                        Console.Write("Do you wish to continue? (Yes or No):   ");
                        permission=Console.ReadLine();
                        while( permission.ToLower() != "yes"||permission.ToLower()!="no" )
                        {
                            Console.WriteLine("Please write either yes or no");
                        }
                    }
                }
            }
            //////////////////////////////////////////////////////////////////////
            record_manager.setAllPatientRecord(AllPatients);
            if (permission.ToLower() == "yes")
            {
                List<Patient> updated_record_list = record_manager.AddNewPatient(patient);

                file_manager.WritePatientRecord(updated_record_list);

                // Add the action to Undo List.txt
                undo_redo_manager.AddUndoAction(patient.Name, action);
                undo_redo_manager.WriteUndoRecord();
                // writing the history to a file
                HistoryManager history_1 = new HistoryManager(DateTime.Now, $"Added the record of patient{patient.Name}", "success");
                HistoryManager.WriteAdminHistory(history_1);
            }
            else
            {
                HistoryManager history_2 = new HistoryManager(DateTime.Now, $"Added the record of patient{patient.Name}", "failed");
                HistoryManager.WriteAdminHistory(history_2);
            }

        }
        public void DeletePatientRecord()
        {
            file_manager.ReadFile();
            undo_redo_manager.ReadFiles();
            int counter = 0;
            // Defining its action
            string action = "Deleted";
            Console.Write("Enter the Name of the patient whose record has to be Deleted: ");
            string patient_name=Console.ReadLine();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            record_manager.setAllPatientRecord(AllPatients);
            ///////////////////////////////////////////
            var ToBeDeleted = AllPatients.Where(pat => pat.Name == patient_name);
            counter = ToBeDeleted.Count(); 
            ////////////////////////////////////////////
            if (counter != 0)
            {
                List<Patient> updated_record_list=new List<Patient>();

                for (int i = 0; i < counter; i++) 
                {
                updated_record_list = record_manager.DeletePatient(patient_name);
                }
                file_manager.WritePatientRecord(updated_record_list);

                undo_redo_manager.AddUndoAction(patient_name, action);
                undo_redo_manager.WriteUndoRecord();
                // writing the history to a file
                HistoryManager history_1 = new HistoryManager(DateTime.Now, $"Deleted the record of patient{patient_name}", "success");
                HistoryManager.WriteAdminHistory(history_1);

            }
            else
            {
                Console.WriteLine("This patient record doesnot exist already!");
                HistoryManager history_2 = new HistoryManager(DateTime.Now, $"Deleted the record of patient{patient_name}", "failed");
                HistoryManager.WriteAdminHistory(history_2);

            }
            
            

            
        }
        public void UpdatePatient()
        {
            int counter = 0;
            file_manager.ReadFile();
            undo_redo_manager.ReadFiles();

            // Defining its action
            string action = "Updated";
            Console.Write("Enter the Name of the patient whose record has to be Updated: ");
            string patient_name = Console.ReadLine();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            record_manager.setAllPatientRecord(AllPatients);
            List<Patient> updated_record_list=new List<Patient>();
            ///////////////////////////////////////
            var ToBeUpdated = AllPatients.Where(p => p.Name == patient_name);
            counter=ToBeUpdated.Count();
            if (counter > 0)
            {
                updated_record_list = record_manager.UpdatePatientRecord(patient_name);
                file_manager.WritePatientRecord(updated_record_list);

                undo_redo_manager.AddUndoAction(patient_name, action);
                undo_redo_manager.WriteUndoRecord();
                // writing the history to a file
                HistoryManager history_1 = new HistoryManager(DateTime.Now, $"Updated the record of patient{patient_name}", "success");
                HistoryManager.WriteAdminHistory(history_1);

            }

            else
            {
                Console.WriteLine("The patient record of this patient is not present");
                HistoryManager history_2 = new HistoryManager(DateTime.Now, $"Updated the record of patient{patient_name}", "failed");
                HistoryManager.WriteAdminHistory(history_2);
            }




        }

        public void SearchPatientRecord()
        {
            file_manager.ReadFile();
            undo_redo_manager.ReadFiles();

            // Defining its action
            string action = "Searched";
            Console.Write("Enter the Name of the patient whose record has to be searched: ");
            string patient_name = Console.ReadLine();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            Patient patient=file_manager.GetPatientFromName(AllPatients, patient_name);
            if(patient != null)
            {
                Console.WriteLine($"Name of Patient: {patient.Name}");
                Console.WriteLine($"Visit Date: {patient.VisitDate.ToString()}");
                Console.WriteLine($"Visit Time: {patient.VisitTime.ToString()}");
                Console.WriteLine($"Type of Visit: {patient.VisitType}");
                Console.WriteLine($"Desciption: {patient.Description}");
                Console.WriteLine($"DoctorName: {patient.DoctorName}");
                Console.WriteLine($"Duration (minutes): {patient.Duration}");
                HistoryManager history_1 = new HistoryManager(DateTime.Now, $"Searched the record of patient{patient_name}", "success");
                HistoryManager.WriteAdminHistory(history_1);

            }
            else
            {
                Console.WriteLine("No such Patient Found");
                HistoryManager history_2 = new HistoryManager(DateTime.Now, $"Searched the record of patient{patient_name}", "failed");
                HistoryManager.WriteAdminHistory(history_2);
            }
            

        }
        public void ViewAllPatientRecords()
        {
            file_manager.ReadFile();
            undo_redo_manager.ReadFiles();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            file_manager.DisplayPatientRecord(AllPatients);
            HistoryManager history = new HistoryManager(DateTime.Now, $"Viewed All Patient Record!", "success");
            HistoryManager.WriteAdminHistory(history);
        }
        public void UndoAction()
        {
            undo_redo_manager.ReadFiles();
            Console.WriteLine("Following are the actions in the Undo List: -");

            undo_redo_manager.DisplayUndoActions();

            Console.WriteLine("To Undo an action on the Patient, Please enter the action Number ");
            int index=Convert.ToInt32(Console.ReadLine())-1;
            string status=undo_redo_manager.UndoAction(index);
            
            undo_redo_manager.WriteUndoRecord();
            undo_redo_manager.WriteRedoRecord();
            HistoryManager history = new HistoryManager(DateTime.Now, $"Tried to Undo Patient Record!", status);
            HistoryManager.WriteAdminHistory(history);

        }
        public void RedoAction()
        {
            undo_redo_manager.ReadFiles();
            Console.WriteLine("Following are the actions in the Redo List: -");

            undo_redo_manager.DisplayRedoActions();

            Console.WriteLine("To Undo an action on the Patient, Please enter the action Number ");
            int index = Convert.ToInt32(Console.ReadLine()) - 1;
            string status = undo_redo_manager.RedoAction(index);

            undo_redo_manager.WriteUndoRecord();
            undo_redo_manager.WriteRedoRecord();
            HistoryManager history = new HistoryManager(DateTime.Now, $"Tried to Undo Patient Record!", status);
            HistoryManager.WriteAdminHistory(history);
        }
        public void FilterbyVisitType(string visit_type)
        {
            
            file_manager.ReadFile();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            file_manager.FilterByVisitType(AllPatients,visit_type);
            HistoryManager history= new HistoryManager(DateTime.Now, $"Viewed the results filtered by visit type", "success");
            HistoryManager.WriteAdminHistory(history);
        }
        public void FilterbyDate(string date)
        {
            while (!(DateOnly.TryParseExact(date, "yyyy-MM-dd", out DateOnly result)))
            {
                Console.WriteLine("You entered the date in an invalid format");
                Console.WriteLine("Please re-enter the date in the corect format (yyyy-MM-dd)");
                date = Console.ReadLine();
            }
            file_manager.ReadFile();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            file_manager.FilterByDate(AllPatients, DateOnly.ParseExact(date,"yyyy-MM-dd"));
            HistoryManager history = new HistoryManager(DateTime.Now, $"Viewed the results filtered by visit date", "success");
            HistoryManager.WriteAdminHistory(history);

        }
        public void FilterbyName(string name)
        {
            file_manager.ReadFile();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            file_manager.FilterByName(AllPatients, name);
            HistoryManager history = new HistoryManager(DateTime.Now, $"Viewed the results filtered by name", "success");
            HistoryManager.WriteAdminHistory(history);
        }

        public void GenerateSummaryAndStats()
        {
            file_manager.ReadFile();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            //(Consultation, Follow-Up, Emergency)
            var filtered_consultation = AllPatients.Where(patient => patient.VisitType.ToLower() == "consultation");
            int consultation_count= filtered_consultation.Count();
            
            var filtered_followup = AllPatients.Where(patient => patient.VisitType.ToLower() == "follow-up");
            int followup_count = filtered_followup.Count();

            var filtered_emergency = AllPatients.Where(patient => patient.VisitType.ToLower() == "emergency");
            int emergency_count = filtered_emergency.Count();
            int total_count = AllPatients.Count;
            AllPatients.Sort((p1,p2)=>p1.VisitDate.CompareTo(p2.VisitDate));
            int total_weeks = (AllPatients[AllPatients.Count-1].VisitDate.DayNumber - AllPatients[0].VisitDate.DayNumber) / 7;
            int visits_per_week = total_count / total_weeks;
            
            
            Console.WriteLine($"Total Visits: {total_count}\n");
            Console.WriteLine($"The visits per week as given as: {visits_per_week}");
            Console.WriteLine($"Number of Patients for Simple Consultation visit: {consultation_count}\n");
            Console.WriteLine($"Number of Patients for a Follow-Up visit: {followup_count}\n");
            Console.WriteLine($"Number of Patients for an Emergency visit: {emergency_count} \n");

            HistoryManager history = new HistoryManager(DateTime.Now, $"Viewed the Summary and Statistics of Patient Visits", "success");
            HistoryManager.WriteAdminHistory(history);

        }
        
    //*****************************************************************
    }
    public class Reception: IReceptionInteraction
    {
        public FileManager file_manager=new FileManager();
        public void SearchPatientRecord()
        {
            file_manager.ReadFile();
            // Defining its action
            string action = "Searched";
            Console.Write("Enter the Name of the patient whose record has to be searched: ");
            string patient_name = Console.ReadLine();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            Patient patient = file_manager.GetPatientFromName(AllPatients, patient_name);
            if (patient != null)
            {
                Console.WriteLine($"Name of Patient: {patient.Name}");
                Console.WriteLine($"Visit Date: {patient.VisitDate.ToString()}");
                Console.WriteLine($"Visit Time: {patient.VisitTime.ToString()}");
                Console.WriteLine($"Type of Visit: {patient.VisitType}");
                Console.WriteLine($"Desciption: {patient.Description}");
                Console.WriteLine($"DoctorName: {patient.DoctorName}");
                Console.WriteLine($"Duration (minutes): {patient.Duration}");
                HistoryManager history_1 = new HistoryManager(DateTime.Now, $"Searched the record of patient{patient_name}", "success");
                HistoryManager.WriteReceptionHistory(history_1);

            }
            else
            {
                Console.WriteLine("No such Patient Found");
                HistoryManager history_2 = new HistoryManager(DateTime.Now, $"Searched the record of patient{patient_name}", "failed");
                HistoryManager.WriteReceptionHistory(history_2);
            }


        }
        public void ViewAllPatientRecords()
        {
            file_manager.ReadFile();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            file_manager.DisplayPatientRecord(AllPatients);
            HistoryManager history = new HistoryManager(DateTime.Now, $"Viewed All Patient Record!", "success");
            HistoryManager.WriteReceptionHistory(history);
        }
        public void FilterbyVisitType(string visit_type)
        {

            file_manager.ReadFile();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            file_manager.FilterByVisitType(AllPatients, visit_type);
            HistoryManager history = new HistoryManager(DateTime.Now, $"Viewed the results filtered by visit type", "success");
            HistoryManager.WriteReceptionHistory(history);
        }
        public void FilterbyDate(string date)
        {
            while (!(DateOnly.TryParseExact(date, "yyyy-MM-dd", out DateOnly result)))
            {
                Console.WriteLine("You entered the date in an invalid format");
                Console.WriteLine("Please re-enter the date in the corect format (yyyy-MM-dd)");
                date = Console.ReadLine();
            }
            file_manager.ReadFile();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            file_manager.FilterByDate(AllPatients, DateOnly.ParseExact(date, "yyyy-MM-dd"));
            HistoryManager history = new HistoryManager(DateTime.Now, $"Viewed the results filtered by visit date", "success");
            HistoryManager.WriteReceptionHistory(history);

        }
        public void FilterbyName(string name)
        {
            file_manager.ReadFile();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            file_manager.FilterByName(AllPatients, name);
            HistoryManager history = new HistoryManager(DateTime.Now, $"Viewed the results filtered by name", "success");
            HistoryManager.WriteReceptionHistory(history);
        }

        public void GenerateSummaryAndStats()
        {
            file_manager.ReadFile();
            List<Patient> AllPatients = file_manager.PreparePatientObjectsFromFile();
            //(Consultation, Follow-Up, Emergency)
            var filtered_consultation = AllPatients.Where(patient => patient.VisitType.ToLower() == "consultation");
            int consultation_count = filtered_consultation.Count();

            var filtered_followup = AllPatients.Where(patient => patient.VisitType.ToLower() == "follow-up");
            int followup_count = filtered_followup.Count();

            var filtered_emergency = AllPatients.Where(patient => patient.VisitType.ToLower() == "emergency");
            int emergency_count = filtered_emergency.Count();
            int total_count = AllPatients.Count;
            AllPatients.Sort((p1, p2) => p1.VisitDate.CompareTo(p2.VisitDate));
            int total_weeks = (AllPatients[^-1].VisitDate.DayNumber - AllPatients[0].VisitDate.DayNumber) / 7;
            int visits_per_week = total_count / total_weeks;


            Console.WriteLine($"Total Visits: {total_count}\n");
            Console.WriteLine($"The visits per week as given as: {visits_per_week}");
            Console.WriteLine($"Number of Patients for Simple Consultation visit: {consultation_count}\n");
            Console.WriteLine($"Number of Patients for a Follow-Up visit: {followup_count}\n");
            Console.WriteLine($"Number of Patients for an Emergency visit: {emergency_count} \n");

            HistoryManager history = new HistoryManager(DateTime.Now, $"Viewed the Summary and Statistics of Patient Visits", "success");
            HistoryManager.WriteReceptionHistory(history);

        }



        //***********************************************************
    }









    //**************************************************
}
