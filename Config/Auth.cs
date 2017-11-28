using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.Config
{
    class Auth
    {
        private static List<Auth> _instances = null;

        private string _link;
        private string _token;
        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
                save();
            }
        }

        public string Link
        {
            get
            {
                return _link;
            }
            set
            {
                _link = value;
                save();
            }
        }

        private Auth()
        {
            
        }
        
        public static List<Auth> getList()
        {
            List<Auth> list = _instances;
            if (list == null)
            {
                list = new List<Auth>();

                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                folder = Path.Combine(folder, "TimeTracker");
                string path = Path.Combine(folder, "auths");
                try
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] splitted = line.Split(";".ToCharArray(), 2);
                            if (splitted[0] == null || splitted[0] == string.Empty || splitted[1] == null || splitted[1] == string.Empty)
                                continue;
                            Auth auth = new Auth();
                            auth._link = splitted[0];
                            auth._token = splitted[1];
                            list.Add(auth);
                        }
                    }
                    _instances = list;     
                }
                catch (FileNotFoundException ex)
                {
                    _instances = list;
                    return list;
                }
                catch (DirectoryNotFoundException ex)
                {
                    _instances = list;
                    return list;
                } 
            }
            return list;
        }

        public static void save()
        {
            if (_instances == null) return; //Nothing to save.
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folder = Path.Combine(folder, "TimeTracker");
            string path = Path.Combine(folder, "auths");

            if (File.Exists(path))
                File.Delete(path);

            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (Auth auth in _instances)
                {
                    string line = auth.Link + ";" + auth._token;
                    writer.WriteLine(line);
                }
            }
        }

        public static Auth addServer(string link, string token)
        {
            Auth server = new Auth();

            if (_instances == null)
                Auth.getList();

            foreach (Auth auth in _instances)
            {
                if (auth._link == link)
                    _instances.Remove(auth);
            }

            server._link = link;
            server._token = token;
            _instances.Add(server);
            save();
            return server;
        }

        public static void removeServer(Auth deleteThis)
        {
            if (_instances == null) return;
            _instances.Remove(deleteThis);
            save();
        }
    }
}
