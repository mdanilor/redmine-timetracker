using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FTTW.Config
{
    public class Config
    {
        //Static
        private static Config instance = null;
        private static string CONFIG_FILE = "config";

        //Dynamic
        private uint _activityId;
        private uint _idleSecondsBeforeStopTracking;        
        private int _closeAction; //0: Ask; 1: Minimize to tray; 2: Close

        string _path;

        public uint ActivityId
        {
            get
            {
                return _activityId;
            }

            set
            {
                _activityId = value;
            }
        }       

        public uint IdleSecondsBeforeStopTracking
        {
            get
            {
                return _idleSecondsBeforeStopTracking;
            }

            set
            {
                _idleSecondsBeforeStopTracking = value;
            }
        }        

        public int CloseAction
        {
            get
            {
                return _closeAction;
            }

            set
            {
                _closeAction = value;
            }
        }



        /// <summary>
        /// Only available constructor. Gets the only existing instance of config. If none exists, creates a new one.
        /// </summary>
        /// <returns></returns>
        public static Config getConfig()
        {
            if (instance == null)
                instance = new Config();
            return instance;
        }        

        public void save()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = Path.Combine(folder, "TimeTracker");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (File.Exists(_path))
                File.Delete(_path);

            //File.Create(_path);

            using (StreamWriter writer = new StreamWriter(_path))
            {
                writer.WriteLine("ActivityId=" + _activityId.ToString());
                writer.WriteLine("IdleSecondsBeforeStopTracking=" + _idleSecondsBeforeStopTracking);
                writer.WriteLine("CloseAction=" + _closeAction.ToString());
            }
        }

        private Config()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = Path.Combine(folder, "TimeTracker");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            _path = Path.Combine(folder, CONFIG_FILE);           

            //Default values:
            _activityId = 9;
            _idleSecondsBeforeStopTracking = 600;
            _closeAction = 0;

            if (!File.Exists(_path))
            {
                //File.Create(_path);
                using (StreamWriter writer = new StreamWriter(_path))
                {
                    writer.WriteLine("ActivityId=" + _activityId);
                    writer.WriteLine("IdleSecondsBeforeStopTracking=" + _idleSecondsBeforeStopTracking);
                    writer.WriteLine("CloseAction=" + _closeAction.ToString());
                }
            }
            else {

                //Reading the file:
                try
                {
                    using (StreamReader file = new StreamReader(_path))
                    {
                        string line;
                        string[] lineParameters;

                        while ((line = file.ReadLine()) != null)
                        {
                            lineParameters = line.Split("=".ToCharArray(), 2);
                            if (lineParameters[0] == "ActivityId")
                            {
                                UInt32.TryParse(lineParameters[1], out _activityId);
                            }
                            else if (lineParameters[0] == "IdleSecondsBeforeStopTracking")
                            {
                                UInt32.TryParse(lineParameters[1], out _idleSecondsBeforeStopTracking);
                            }
                            else if (lineParameters[0] == "CloseAction")
                            {
                                Int32.TryParse(lineParameters[1], out _closeAction);
                            }
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    throw new Exceptions.MissingConfigFileException();
                }
            }
        }        

    }
}
