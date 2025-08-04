using NewPatientProject.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPatientProject.Services
{
    public class UndoRedoManager
    {  
        private static RecordManager recordManager=new RecordManager();
        private static FileManager fileManager=new FileManager();
        private static List<Patient> Patient_Records=new List<Patient>();


        private readonly string _UndoFilePath = @"../../../Items_to_Undo.txt";
        private readonly string _RedoFilePath = @"../../../Items_to_Redo.txt";
        private static Queue<UndoRedoAction> Actions_Undo = new Queue<UndoRedoAction>();
        private static Queue<UndoRedoAction> Actions_Redo = new Queue<UndoRedoAction>();
        private static List<string> _UndoFileStrings= new List<string>();
        private static List<string> _RedoFileStrings= new List<string>();
        
        public void ReadFiles()
        {
            // Check the existence of data in the Undo File.

            if (File.Exists(_UndoFilePath))
            {
                _UndoFileStrings=File.ReadAllLines(_UndoFilePath).ToList();
            }
            else
            {
                File.WriteAllText(_UndoFilePath, "");
            }

            // Check the existence of data in the Undo File.

            if (File.Exists(_RedoFilePath))
            {
                _RedoFileStrings = File.ReadAllLines(_RedoFilePath).ToList();
            }
            else
            {
                File.WriteAllText(_RedoFilePath, "");
            }
        }

        // the structure of the string is: "The record of the patient: {patient_name} has been {status}"
        // The {status} mean Added, Updated, or deleted
        public void GetActionsFromStrings()
        {
            Actions_Undo.Clear();
            Actions_Redo.Clear();
            if (_UndoFileStrings.Count > 0)
            {
                foreach (string undostr in _UndoFileStrings)
                {
                    string[] undo_action_words = undostr.Split(new[] { " the record of the patient: ", " Description: ", " VisitDate: ", " VisitTime: ", " DoctorName: ", " Duration: ", " VisitType: " }, StringSplitOptions.None);
                    
                    string action = undo_action_words[0];
                    string patient_name = undo_action_words[1];
                    string desc= undo_action_words[2];
                    DateOnly date = DateOnly.ParseExact(undo_action_words[3], new string[] { "yyyy-MM-dd", "M/d/yyyy" });
                    TimeOnly time = TimeOnly.ParseExact(undo_action_words[4], new string[] { "HH:mm:ss", "H:mm", "h:mm tt" });
                    string dr= undo_action_words[5];
                    int dur = Convert.ToInt32(undo_action_words[6]);
                    string type= undo_action_words[7];
                    Patient pat = new Patient(patient_name, date, time, type, desc, dr, dur);
                    AddUndoAction(pat, action);
                }
            }



            if (_RedoFileStrings.Count > 0)
            {
                foreach (string redostr in _RedoFileStrings)
                {
                    string[] redo_action_words = redostr.Split(new[] { " the record of the patient: ", " Description: ", " VisitDate: ", " VisitTime: ", " DoctorName: ", " Duration: ", " VisitType: " }, StringSplitOptions.None);
                    string action = redo_action_words[0];
                    string patient_name = redo_action_words[1];
                    string desc = redo_action_words[2];
                    DateOnly date = DateOnly.ParseExact(redo_action_words[3], new string[] { "yyyy-MM-dd", "M/d/yyyy" });
                    TimeOnly time = TimeOnly.ParseExact(redo_action_words[4], new string[] { "HH:mm:ss", "H:mm", "h:mm tt" });
                    string dr = redo_action_words[5];
                    int dur = Convert.ToInt32(redo_action_words[6]);
                    string type = redo_action_words[7];
                    Patient pat = new Patient(patient_name, date, time, type, desc, dr, dur);
                    AddRedoAction(pat, action);
                }
            }

               
        }

        // Adding an action" -

        public void AddUndoAction(Patient patient, string action)
        {
            UndoRedoAction UndoAction=new UndoRedoAction(patient,action);
            if (Actions_Undo.Count > 10)
            {
                UndoRedoAction UndoGarbage=Actions_Undo.Dequeue();
                Actions_Undo.Enqueue(UndoAction);
            }
            else
            {
                Actions_Undo.Enqueue(UndoAction);
            }
        }
        public void AddRedoAction(Patient patient, string action)
        {
            UndoRedoAction RedoAction = new UndoRedoAction(patient, action);
            if (Actions_Redo.Count > 10)
            {
                UndoRedoAction RedoGarbage = Actions_Redo.Dequeue();
                Actions_Redo.Enqueue(RedoAction);
            }
            else
            {
                Actions_Redo.Enqueue(RedoAction);
            }
        }
        // Add to Undo List: After we do Redo, this will add the redone thing to the Undo list
        public void AddtoUndoList(UndoRedoAction redone_action)
        {
            if (Actions_Undo.Count > 10)
            {
                Actions_Undo.Dequeue();
            }
            Actions_Undo.Enqueue(redone_action);
        }
        // Add to Redo List: After we do Undo, this will add the undone thing to the Redo list
        public void AddtoRedoList(UndoRedoAction undone_action)
        {
            if (Actions_Redo.Count > 10)
            {
                Actions_Redo.Dequeue();
            }
            Actions_Redo.Enqueue(undone_action);
        }


        /*
         Let me test new functions
        public string UndoAction(int index)
        {
            string status = "failed";
            if(index<0 || index >= Actions_Undo.Count)
            {
                Console.WriteLine("Out of index, no such action exists");
            }
            else
            {
                List<UndoRedoAction> List_UndoActions = Actions_Undo.ToList();
                UndoRedoAction undoaction = List_UndoActions[index];
                List_UndoActions.Remove(undoaction);
                AddtoRedoList(undoaction);
                Queue<UndoRedoAction> Undo_Actions = new Queue<UndoRedoAction>(List_UndoActions);
                Actions_Undo = Undo_Actions;
                status = "success";
            }
                return status;
            // procedure
        }
        public  string RedoAction(int index)
        {
            string status = "failed";
            if (index < 0 || index >= Actions_Redo.Count)
            {
                Console.WriteLine("Out of index, no such action exists");
            }
            else
            {
                List<UndoRedoAction> List_RedoActions = Actions_Redo.ToList();
                UndoRedoAction redoaction = List_RedoActions[index];
                List_RedoActions.Remove(redoaction);
                AddtoUndoList(redoaction);
                Queue<UndoRedoAction> Redo_Actions = new Queue<UndoRedoAction>(List_RedoActions);
                Actions_Redo = Redo_Actions;
                status = "success";
            }
            return status;   
        }

         
         */

        // New Functions for UndoAction and RedoAction: -

        public string UndoAction(int index)
        {
            string status = "failed";
            if (index < 0 || index >= Actions_Undo.Count)
            {
                Console.WriteLine("Out of index, no such action exists");
            }
            else
            {
                
                Patient_Records = fileManager.PreparePatientObjectsFromFile();

                List<UndoRedoAction> List_UndoActions = Actions_Undo.ToList();
                UndoRedoAction undoAction = List_UndoActions[index];

                Patient p = undoAction.Patient;
                string action = undoAction.Action;

                switch (action)
                {
                    case "Added":
                        
                        Patient_Records = Patient_Records.Where(pt => !(pt.Name == p.Name && pt.VisitDate == p.VisitDate && pt.VisitTime == p.VisitTime)).ToList();
                        break;

                    case "Deleted":
                        
                        Patient_Records.Add(p);
                        break;

                    case "Updated":
                        
                        Patient_Records = Patient_Records.Where(pt => !(pt.Name == p.Name && pt.VisitDate == p.VisitDate && pt.VisitTime == p.VisitTime)).ToList();
                        
                        Patient_Records.Add(p);
                        break;

                    default:
                        Console.WriteLine($"Unknown action type: {action}");
                        return status;
                }

                fileManager.WritePatientRecord(Patient_Records);

                List_UndoActions.Remove(undoAction);
                AddtoRedoList(undoAction);
                Actions_Undo = new Queue<UndoRedoAction>(List_UndoActions);

                WriteUndoRecord();
                WriteRedoRecord();

                status = "success";
            }
            return status;
        }
        public string RedoAction(int index)
        {
            string status = "failed";
            if (index < 0 || index >= Actions_Redo.Count)
            {
                Console.WriteLine("Out of index, no such action exists");
            }
            else
            {
                Patient_Records = fileManager.PreparePatientObjectsFromFile();

                List<UndoRedoAction> List_RedoActions = Actions_Redo.ToList();
                UndoRedoAction redoAction = List_RedoActions[index];

                Patient p = redoAction.Patient;
                string action = redoAction.Action;

                switch (action)
                {
                    case "Added":
                        Patient_Records.Add(p);
                        break;

                    case "Deleted":
                        Patient_Records = Patient_Records.Where(pt => !(pt.Name == p.Name && pt.VisitDate == p.VisitDate && pt.VisitTime == p.VisitTime)).ToList();
                        break;

                    case "Updated":
                        Patient_Records = Patient_Records.Where(pt => !(pt.Name == p.Name && pt.VisitDate == p.VisitDate && pt.VisitTime == p.VisitTime)).ToList();
                        Patient_Records.Add(p);
                        break;

                    default:
                        Console.WriteLine($"Unknown action type: {action}");
                        return status;
                }

                fileManager.WritePatientRecord(Patient_Records);

                List_RedoActions.Remove(redoAction);
                AddtoUndoList(redoAction);
                Actions_Redo = new Queue<UndoRedoAction>(List_RedoActions);

                WriteUndoRecord();
                WriteRedoRecord();

                status = "success";
            }
            return status;
        }


        // get strings from actions as well
        public List<string> GetStringsFromActions(Queue<UndoRedoAction> Actions)
        {
            
            List<string> Action_Strings=new List<string>();
            foreach(UndoRedoAction action in Actions)
            {
                
                Action_Strings.Add($"{action.Action} the record of the patient: {action.Patient.Name} Description: {action.Patient.Description} VisitDate: {action.Patient.VisitDate} VisitTime: {action.Patient.VisitTime} DoctorName: {action.Patient.DoctorName} Duration: {action.Patient.Duration} VisitType: {action.Patient.VisitType}");
            }
            return Action_Strings;
        }

        // Writing in the undo record file: -
        public void WriteUndoRecord()
        {
            List<string> UndoActionStrings=GetStringsFromActions(Actions_Undo);
            
            File.WriteAllLines(_UndoFilePath, UndoActionStrings);

        }
        // Writing in the redo record file: -

        public void WriteRedoRecord()
        {
            List<string> RedoActionStrings = GetStringsFromActions(Actions_Redo);

            File.WriteAllLines(_RedoFilePath, RedoActionStrings);

        }

        public void DisplayUndoActions()
        {
            List<string> ActionStrings = GetStringsFromActions(Actions_Undo);
            int i = 1;
            foreach (string str in ActionStrings) {
                Console.Write($"{i}-  ");
                Console.WriteLine(str);
                i++;
            }
        }

        public void DisplayRedoActions()
        {
            int i = 1;
            List<string> ActionStrings = GetStringsFromActions(Actions_Redo);
            foreach (string str in ActionStrings)
            {
                Console.Write($"{i}-    ");
                Console.WriteLine(str);
                i++;
            }
        }


        /*********************************************************************************************************/
    }
}
