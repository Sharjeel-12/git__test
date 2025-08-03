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
            if(_UndoFileStrings.Count > 0)
            {
                foreach (string undostr in _UndoFileStrings)
                {
                    string[] undo_action_words = undostr.Split(new[] { " the record of the patient: " }, StringSplitOptions.None);
                    string patient_name = undo_action_words[1];
                    string action = undo_action_words[0];
                    AddUndoAction(patient_name, action);
                }
            }



            if (_RedoFileStrings.Count > 0)
            {
                foreach (string redostr in _RedoFileStrings)
                {
                    string[] redo_action_words = redostr.Split(new[] { " the record of the patient: " }, StringSplitOptions.None);
                    string patient_name = redo_action_words[1];
                    string action = redo_action_words[0];
                    AddRedoAction(patient_name, action);
                }
            }

               
        }

        // Adding an action" -

        public void AddUndoAction(string patient_name, string action)
        {
            UndoRedoAction UndoAction=new UndoRedoAction(patient_name,action);
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
        public void AddRedoAction(string patient_name, string action)
        {
            UndoRedoAction RedoAction = new UndoRedoAction(patient_name, action);
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
                UndoRedoAction UndoGarbage = Actions_Undo.Dequeue();
                Actions_Undo.Enqueue(redone_action);
            }
            else
            {
                Actions_Undo.Enqueue(redone_action);
            }
        }
        // Add to Redo List: After we do Undo, this will add the undone thing to the Redo list
        public void AddtoRedoList(UndoRedoAction undone_action)
        {
            if (Actions_Undo.Count > 10)
            {
                UndoRedoAction UndoGarbage = Actions_Undo.Dequeue();
                Actions_Undo.Enqueue(undone_action);
            }
            else
            {
                Actions_Undo.Enqueue(undone_action);
            }
        }

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
            if (index < 0 || index >= Actions_Undo.Count)
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
        // get strings from actions as well
        public List<string> GetStringsFromActions(Queue<UndoRedoAction> Actions)
        {
            
            List<string> Action_Strings=new List<string>();
            foreach(UndoRedoAction action in Actions)
            {
                
                Action_Strings.Add($"{action.Action} the record of the patient: {action.PatientName}");
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
            foreach (string str in ActionStrings) {
                Console.WriteLine(str);
            }
        }

        public void DisplayRedoActions()
        {
            List<string> ActionStrings = GetStringsFromActions(Actions_Redo);
            foreach (string str in ActionStrings)
            {
                Console.WriteLine(str);
            }
        }


        /*********************************************************************************************************/
    }
}
