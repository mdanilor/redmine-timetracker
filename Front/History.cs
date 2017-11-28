using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.Front
{
    class History : IStartEnd
    {
        private string _project;
        private string _task;
        //private TimeSpan _time;
        private Front.FIssue _fIssue;

        public string Project
        {
            get
            {
                return _project;
            }
        }

        public string Task
        {
            get
            {
                return _task;
            }
        }

        public string Time
        {
            get
            {
                return _fIssue.TimeWorkedThisSession.ToString(@"hh\:mm\:ss");
            }
        }

        public bool IsActive
        {
            get
            {
                return _fIssue.IsActive;
            }
        }

        public static void addHistory(FIssue fIssue, List<History> historyList)
        {
            if (historyList == null) historyList = new List<History>();
            History instance = null;
            foreach (History history in historyList)
            {
                if (history._fIssue == fIssue)
                {
                    instance = history;
                    historyList.Remove(instance);                    
                    break;
                }
            }

            if (instance == null)
                instance = new History(fIssue);
            historyList.Insert(0, instance);
        }

        private History(Front.FIssue fIssue)
        {                        
            _project = fIssue.ProjectName;
            _task = fIssue.Id.ToString();
            //_time = fIssue.TimeWorkedThisSession;
            _fIssue = fIssue;
        }        

        public FIssue getIssue()
        {
            return _fIssue;
        }       
    }
}
