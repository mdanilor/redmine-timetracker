using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTTW.Front;
using FTTW.Persistency;
using FTTW.RedmineApi;

namespace FTTW.Config
{
    class Server
    {
        private string _address;
        private Auth _auth;
        private DataManager _manager;
        private List<Front.FProject> _projects;
        private int _currentProjetI;
        private RedmineXmlGetter _getter;
        private List<Front.History> _history;
        private List<DayStatistics> _ds;
        private bool _status;

        public string Address
        {
            get
            {
                return _address;
            }
        }

        public string Name
        {
            get
            {
                return _address.Replace("http://", "").Replace("https://", "");
            }
        }

        public Auth Auth
        {
            get
            {
                return _auth;
            }
        }

        internal DataManager Manager
        {
            get
            {
                return _manager;
            }
        }

        internal List<FProject> Projects
        {
            get
            {
                return _projects;
            }

            set
            {
                _projects = value;
            }
        }

        public int CurrentProjetI
        {
            get
            {
                return _currentProjetI;
            }
            set
            {
                _currentProjetI = value;
            }
        }

        internal RedmineXmlGetter Getter
        {
            get
            {
                return _getter;
            }
        }

        internal List<History> History
        {
            get
            {
                return _history;
            }

            set
            {
                _history = value;
            }
        }

        public List<DayStatistics> Ds
        {
            get
            {
                return _ds;
            }

            set
            {
                _ds = value;
            }
        }

        public bool Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
            }
        }

        public Server(Auth auth)
        {
            _auth = auth;
            _address = auth.Link;
            _manager = new DataManager(RedmineApi.RedmineXmlGetter.getSelfUserdata(_auth.Link, _auth.Token, "abcde"));
            _getter = new RedmineXmlGetter(_manager);
            _projects = FProject.getAll(_getter);
            _currentProjetI = 0;
            _history = new List<History>();
            _ds = Front.DayStatistics.getPastWeek(_getter);
        }

        public Server (string address, string login, string password, bool persist)
        {            
            _manager = new DataManager(RedmineXmlGetter.getSelfUserdata(address, login, password));
            if (_manager != null && persist)
            {
                _auth = Auth.addServer(address, _manager.UserData.RedmineLogin);
            }
            _address = address;
            _getter = new RedmineXmlGetter(_manager);
            _projects = FProject.getAll(_getter);
            _currentProjetI = 0;
            _history = new List<History>();
            _ds = Front.DayStatistics.getPastWeek(_getter);
        }

        public void forgetThisServer()
        {
            List<Auth> authList = Auth.getList();
            authList.Remove(_auth);
            Auth.save();               
        }
    }
}
