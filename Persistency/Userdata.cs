using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.Persistency
{
    class Userdata
    {
        private int _redmineId;
        private string _redmineLogin;
        private string _redminePassword;
        private string _name;
        private string _link;

        public int RedmineId
        {
            get
            {
                return _redmineId;
            }

            set
            {
                _redmineId = value;
            }
        }

        public string RedmineLogin
        {
            get
            {
                return _redmineLogin;
            }

            set
            {
                _redmineLogin = value;
            }
        }

        public string RedminePassword
        {
            get
            {
                return _redminePassword;
            }

            set
            {
                _redminePassword = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
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
            }
        }
    }
}
