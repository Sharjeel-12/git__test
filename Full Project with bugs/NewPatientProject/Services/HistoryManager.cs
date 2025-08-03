using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPatientProject.Services
{
    public class HistoryManager
    {
        public DateTime TimeStamp;
        public string Msg;
        public string Status;
        private static readonly string _FilePath_AdminHist = @"..\..\AdminHistory.txt";
        private static readonly string _FilePath_RecepHist = @"..\..\ReceptionHistory.txt";
        public HistoryManager(DateTime ts,string msg, string status) 
        { 
            TimeStamp = ts;
            Msg= msg; 
            Status = status;
        }
        public static void WriteAdminHistory(HistoryManager obj)
        {
            File.AppendAllText(_FilePath_AdminHist, $"{Convert.ToString(obj.TimeStamp)}: Admin {obj.Msg} | status: {obj.Status}\n");
        }
        public static void WriteReceptionHistory(HistoryManager obj)
        {
            File.AppendAllText(_FilePath_RecepHist, $"{Convert.ToString(obj.TimeStamp)}: Admin {obj.Msg} | status: {obj.Status}\n");
        }


    }
}
