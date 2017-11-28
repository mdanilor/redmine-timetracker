using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.Front
{
    class FIssue : IStartEnd
    {
        private Persistency.DataManager _manager;
        private int _id;
        private string _name;
        private string _description;
        private string _tracker;
        private string _status;
        private string _author;
        private string _priority;
        private DateTime _dueDate;
        private TimeSpan _time;
        private int _projectId;
        private Persistency.WorkedHours _current;
        private Stopwatch _currentSessionTime;
        private RedmineApi.Issue _pIssue;        
        private bool _isSelected;

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public string Tracker
        {
            get
            {
                return _tracker;
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }
        }

        public string Author
        {
            get
            {
                return _author;
            }
        }

        public string Priority
        {
            get
            {
                return _priority;
            }
        }

        public DateTime DueDate
        {
            get
            {
                return _dueDate;
            }
        }

        public string TimeString
        {
            get
            {
                TimeSpan result = new TimeSpan();
                result += _currentSessionTime.Elapsed;
                result += _time;
                return result.ToString(@"hh\:mm\:ss");
            }
        }

        public TimeSpan Time
        {
            get
            {
                return _time;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public TimeSpan CurrentSessionTime
        {
            get
            {
                return _currentSessionTime.Elapsed;
            }
        }

        public bool IsActive
        {
            get
            {
                return _pIssue.IsActive;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
            }
        }

        public TimeSpan TimeWorkedThisSession
        {
            get
            {
                return _pIssue.WorkedThisSession;
            }
        }

        public string ProjectName
        {
            get
            {
                return _pIssue.Project.Name;
            }
        }

        private FIssue() {
            _currentSessionTime = new Stopwatch();
            _time = new TimeSpan();
            _isSelected = false;
        }

        private static FTTW.Front.FIssue getFromRAIssue(FTTW.RedmineApi.Issue rAIssue, Persistency.DataManager dataManager)
        {
            FIssue issue = new FIssue();
            issue._pIssue = rAIssue;
            issue._author = rAIssue.AuthorName;
            issue._dueDate = rAIssue.DueDate;
            issue._id = rAIssue.Id;
            issue._name = rAIssue.Subject;
            issue._description = rAIssue.Description;
            issue._priority = rAIssue.PriorityName;
            issue._status = rAIssue.StatusName;
            issue._tracker = rAIssue.TrackerName;
            issue._manager = dataManager;
            issue._projectId = rAIssue.Project.Id;
            //issue._time = TimeSpan.FromSeconds(WSCom.ExchangeData.getWorkedIssueSeconds(rAIssue, dataManager));
            issue._time = TimeSpan.FromSeconds(rAIssue.TimeSpent);
            return issue;
        }

        /// <summary>
        /// Get all issues from a backend IssueList object.
        /// </summary>
        /// <param name="rAIssues">Backend IssueList object</param>
        /// <param name="manager">Datamanager</param>
        /// <returns>List of issues.</returns>
        public static List<FIssue> getFrontIssueList(RedmineApi.IssueList rAIssues, Persistency.DataManager manager)
        {
            List<RedmineApi.Issue> rail = rAIssues.List;
            List<FIssue> list = new List<FIssue>();
            foreach (RedmineApi.Issue issue in rail)
            {
                list.Add(getFromRAIssue(issue, manager));
            }
            return list;
        }

        /// <summary>
        /// Starts tracking time  for this issue.
        /// </summary>
        public void startWork()
        {
            _current = _manager.startWork(_pIssue);
            _currentSessionTime.Start();
            _pIssue.IsActive = true;
        }

        /// <summary>
        /// Stops tracking time for this issue.
        /// </summary>
        /// <param name="getter">RedmineXmlGetter object.</param>
        /// <returns>True in case it could be remotely stored, false in case it could only be localy stored.</returns>
        public bool endWork(RedmineApi.RedmineXmlGetter getter)
        {            
            _currentSessionTime.Stop();
            _pIssue.IsActive = false;
            return _manager.endWork(_current, getter);            
        }

        /// <summary>
        /// In case it stays idle for one entire second, this method should be called to increase idle time by one.
        /// </summary>
        public void increaseIdleTime()
        {
            _current.increaseIdleTime();
        }

        public uint getIdleTime()
        {
            return _current.IdleTime;
        }

        public FIssue getIssue()
        {
            return this;
        }

    }
}
