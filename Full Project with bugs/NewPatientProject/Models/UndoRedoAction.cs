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
        public string PatientName;
        public string Action;
        public UndoRedoAction(string name, string action)
        {
            PatientName = name;
            Action = action;
        }
    }

   

}
