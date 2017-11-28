using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.Config
{
    class ServerHistory
    {
        private static ServerHistory _instance = null;

        private List<string> _history;
        private string _path;

        public List<string> History
        {
            get
            {
                return _history;
            }
        }

        public static ServerHistory get()
        {
            if (_instance == null)
                _instance = new ServerHistory();
            return _instance;
        }

        private ServerHistory()
        {
            _history = new List<string>();
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = Path.Combine(folder, "TimeTracker");
            _path = Path.Combine(folder, "serverhistory");
            if (File.Exists(_path))
            {
                using (StreamReader reader = new StreamReader(_path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        _history.Add(line);                         
                    }
                }
            }
        }

        public void addWord(string word)
        {
            if (!_history.Contains(word))
            {
                _history.Add(word);

                using (StreamWriter writer = new StreamWriter(_path, append: true))
                {
                    writer.WriteLine(word);
                }
            }
        }
        
    }
}
