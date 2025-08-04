using System;
using NewPatientProject.Models;
using NewPatientProject.Services;
using NewPatientProject.Interfaces;
using System.Collections.Generic;
using System.Globalization;

/*
 
 IMPORTANT INSTRUCTIONS BEFORE USE: -
Admin Username: admin
Admin Password: admin123

Reception Username: reception
Reception Password: reception123


All Fee Records are saved in .txt file
All Patient Records are saved in .csv file
All the Undo Redo History is saved in Txt file
Logger History is saved in .txt file in path .bin/
 
 
 */


namespace NewPatientProject

{
    class Program
    {
        static void Main(string[] args)
        {
            /*FileManager fileManager = new FileManager();
            fileManager.ReadFile();

            RecordManager recordManager = new RecordManager();
            recordManager.setAllPatientRecord(FileManager.PatientObjects_Loaded);

            UndoRedoManager undoRedoManager = new UndoRedoManager();
            undoRedoManager.ReadFiles();*/

            Console.WriteLine("======= Patient Visit Management System =======");

            string username, password;
            Console.Write("Username: ");
            username = Console.ReadLine();
            Console.Write("Password: ");
            password = Console.ReadLine();

            bool isAdmin = username == "admin" && password == "admin123";
            bool isReceptionist = username == "reception" && password == "reception123";

            if (!isAdmin && !isReceptionist)
            {
                Console.WriteLine("Invalid credentials. Exiting...");
                return;
            }

            if (isAdmin)
            {
                Admin admin = new Admin();
                RunAdminMenu(admin);
            }
            else
            {
                Receptionist receptionist = new Receptionist();
                RunReceptionMenu(receptionist);
            }
        }

        static void RunAdminMenu(Admin admin)
        {
            while (true)
            {
                Console.WriteLine("\n===== Admin Menu =====");
                Console.WriteLine("1. Add Patient Record");
                Console.WriteLine("2. Delete Patient Record");
                Console.WriteLine("3. Search Patient Record");
                Console.WriteLine("4. Update Patient Record");
                Console.WriteLine("5. View All Records");
                Console.WriteLine("6. Undo Last Action");
                Console.WriteLine("7. Redo Action");
                Console.WriteLine("8. Filter Records");
                Console.WriteLine("9. Generate Report");
                Console.WriteLine("10. View the Fee Record of the Patient");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string option = Console.ReadLine();
                switch (option)
                {
                    case "1": admin.AddPatientRecord(); break;
                    case "2": admin.DeletePatientRecord(); break;
                    case "3": admin.SearchPatientRecord(); break;
                    case "4": admin.UpdatePatient(); break;
                    case "5": admin.ViewAllPatientRecords(); break;
                    case "6": admin.UndoAction(); break;
                    case "7": admin.RedoAction(); break;
                    case "8": RunFilterMenu(admin); break;
                    case "9": admin.GenerateSummaryAndStats(); break;
                    case "10": admin.CalculateAndViewPatientFeeRecord(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option"); break;
                }
            }
        }

        static void RunReceptionMenu(Receptionist receptionist)
        {
            while (true)
            {
                Console.WriteLine("\n===== Receptionist Menu =====");
                Console.WriteLine("1. Search Patient Record");
                Console.WriteLine("2. View All Records");
                Console.WriteLine("3. Filter Records");
                Console.WriteLine("4. Generate Report");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string option = Console.ReadLine();
                switch (option)
                {
                    case "1": receptionist.SearchPatientRecord(); break;
                    case "2": receptionist.ViewAllPatientRecords(); break;
                    case "3": RunFilterMenu(receptionist); break;
                    case "4": receptionist.GenerateSummaryAndStats(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option"); break;
                }
            }
        }

        static void RunFilterMenu(object user)
        {
            Console.WriteLine("\n--- Filter Options ---");
            Console.WriteLine("1. By Patient Name");
            Console.WriteLine("2. By Visit Date");
            Console.WriteLine("3. By Visit Type");
            Console.Write("Choose filter option: ");
            string filterOption = Console.ReadLine();

            Console.Write("Enter filter value: ");
            string filterValue = Console.ReadLine();

            switch (filterOption)
            {
                case "1":
                    if (user is IAdminInteraction admin) admin.FilterbyName(filterValue);
                    else if (user is IReceptionInteraction recep) recep.FilterbyName(filterValue);
                    break;
                case "2":
                    if (user is IAdminInteraction admin2) admin2.FilterbyDate(filterValue);
                    else if (user is IReceptionInteraction recep2) recep2.FilterbyDate(filterValue);
                    break;
                case "3":
                    if (user is IAdminInteraction admin3) admin3.FilterbyVisitType(filterValue);
                    else if (user is IReceptionInteraction recep3) recep3.FilterbyVisitType(filterValue);
                    break;
                default:
                    Console.WriteLine("Invalid filter option");
                    break;
            }
        }
    }

    public class Receptionist : IReceptionInteraction
    {
        private Admin baseAdmin = new Admin();
        public void SearchPatientRecord() => baseAdmin.SearchPatientRecord();
        public void ViewAllPatientRecords() => baseAdmin.ViewAllPatientRecords();
        public void FilterbyName(string name) => baseAdmin.FilterbyName(name);
        public void FilterbyDate(string date) => baseAdmin.FilterbyDate(date);
        public void FilterbyVisitType(string type) => baseAdmin.FilterbyVisitType(type);
        public void GenerateSummaryAndStats() => baseAdmin.GenerateSummaryAndStats();
    }
}
