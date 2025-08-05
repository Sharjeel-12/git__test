using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_03.Singletons
{
    public interface IConfigurationManager
    {
        string GetSetting(string key);
        void SetSetting(string key, string value);

    }
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
        void LogWarning(string message);

    }


    public class ConfigManager:IConfigurationManager
    {
        private static readonly string _key = "Agdsaigi@134i1";
        private static string Setting="Unconfigured";
        private static ConfigManager _instance;
        private static readonly object _instanceLock = new object();
        public static ConfigManager Instance 
        { 
            get 
            {
                lock (_instanceLock)
                {
                    return _instance??=new ConfigManager();
                }
            
            } 
        }

        private ConfigManager() { }

        public string GetSetting(string key)
        {
            string setting = "Incorrect Key";
            if (_key == key)
            {
                setting= Setting;
            }
            return setting;
        }
        public void SetSetting(string key, string value)
        {
            if (_key == key)
            {
                Setting= value;
            }
        }
    }


    public class LogManager : ILogger
    {
       
        private static LogManager _instance;
        private static readonly object _instanceLock = new object();
        public static LogManager Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    return _instance ??= new LogManager();
                }

            }
        }

        private LogManager() { }


        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }
        public void LogError(string message)
        {
            Console.WriteLine(message);
        }
        public void LogWarning(string message)
        {
            Console.WriteLine(message);
        }


    }

}
