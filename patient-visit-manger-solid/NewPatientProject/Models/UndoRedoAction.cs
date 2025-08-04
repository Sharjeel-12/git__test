using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPatientProject.Models
{
    public class UndoRedoAction
    {
        public Patient Patient;
        public string Action;
        public UndoRedoAction(Patient patient, string action)
        {
            Patient = patient;
            Action = action;
        }
    }

   

}
