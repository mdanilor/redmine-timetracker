using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace FTTW.Persistency
{
    class DataManager
    {
        private List<WorkedHours> _workedHours;
        private List<RedmineApi.Issue> _sessionHistory;
        private Userdata _userData;  

        public List<WorkedHours> WorkedHours
        {
            get
            {
                return _workedHours;
            }
        }

        internal Userdata UserData
        {
            get
            {
                return _userData;
            }

            set
            {
                _userData = value;
            }
        }

        public DataManager(Userdata ud)
        {
            _sessionHistory = new List<RedmineApi.Issue>();
            _workedHours = this.localLoad();
            _userData = ud;
        }

        /// <summary>
        /// Called when an Issue starts to be tracked. 
        /// </summary>
        /// <param name="issue">Issue on which the user will be working.</param>
        /// <returns>Returns a started WorkedHours object.</returns>
        public WorkedHours startWork(RedmineApi.Issue issue)
        {
            WorkedHours current = new WorkedHours(issue);
            current.StartTime = DateTime.Now;
            return current;
        }

        /// <summary>
        /// Stops current WorkedHours object and saves it, remotely if possible, localy if necessary.
        /// </summary>
        /// <param name="current">Current active WorkedHours object.</param>
        /// <param name="getter">RedmineXmlGetter object</param>
        /// <returns>True in case it was possible to save remotely. False in case it was a local save.</returns>
        public bool endWork(WorkedHours current, RedmineApi.RedmineXmlGetter getter)
        {
            current.EndTime = DateTime.Now;
            _workedHours.Add(current);

            //History:
            current.Issue.WorkedThisSession += (current.EndTime - current.StartTime);
            if (_sessionHistory.Contains(current.Issue)) {
                _sessionHistory.Remove(current.Issue);
            }
            _sessionHistory.Add(current.Issue);


            if (!remoteSave(getter)) //Saves localy in case it cannot save remotelly.
            {
                this.localSave();
                return false;
            }
            else //In case it can save remotely, cleans things up
            {
                _workedHours = new List<WorkedHours>();
                clearLocalFile();
                return true;
            }
        }

        /// <summary>
        /// Saves remotely
        /// </summary>
        /// <param name="getter">RedmineXmlGetter object</param>
        /// <returns>True in case it was sucessful. False otherwise.</returns>
        private bool remoteSave(RedmineApi.RedmineXmlGetter getter)
        {
            return RedmineApi.RedmineUploadData.uploadWorkedHours(getter);
        }

        /// <summary>
        /// Clears local storing file.
        /// </summary>
        private void clearLocalFile()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = Path.Combine(folder, "TimeTracker");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, "data.xml");

            File.WriteAllText(@path, String.Empty);
        }

        /// <summary>
        /// Perfoms a local save.
        /// </summary>
        private void localSave()
        {
            try
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                folder = Path.Combine(folder, "TimeTracker");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                using (Stream stream = File.Open(Path.Combine(folder, "data.xml"), FileMode.OpenOrCreate))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<WorkedHours>));
                    xs.Serialize(stream, this._workedHours);
                }
            } catch (IOException)
            {
                Console.WriteLine("Error: could not open file");
            }
        }

        /// <summary>
        /// Load unsent worked hours.
        /// </summary>
        /// <returns>WorkedHours in local storage.</returns>
        private List<WorkedHours> localLoad()
        {
            try
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                folder = Path.Combine(folder, "TimeTracker");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                using (Stream stream = File.Open(Path.Combine(folder, "data.xml"), FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<WorkedHours>));
                    List < WorkedHours > list = (List<WorkedHours>)xs.Deserialize(stream);
                    stream.Close();
                    return list;
                }
            } catch (Exception e)
            {
                Console.WriteLine("Error: could not open file");
                return new List<WorkedHours>();
            }
        }

        /// <summary>
        /// Gets the issues on which the user worked in this session.
        /// </summary>
        /// <returns>Issues worked on this session</returns>
        public List<RedmineApi.Issue> getSessionHistory()
        {
            return _sessionHistory;
        }
    }
}
