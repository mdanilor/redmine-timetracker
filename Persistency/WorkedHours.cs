using FTTW.RedmineApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.Persistency
{
    [Serializable]
    public class WorkedHours
    {
        private int _projectId;
        private DateTime _startTime;
        private DateTime _endTime;
        private uint _idleTime;

        [NonSerialized]
        private Issue _issue;


        public int IssueId
        {
            get
            {
                return _issue.Id;
            }
        }

        public int ProjectId
        {
            get
            {
                return _projectId;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }

            set
            {
                _startTime = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }

            set
            {
                _endTime = value;
            }
        }
        
        public Issue Issue
        {
            get
            {
                return _issue;
            }
            set
            {
                _issue = value;
            }
        }

        public uint IdleTime
        {
            get
            {
                return _idleTime;
            }
            set
            {
                _idleTime = value;
            }
        }
        public double Hours
        {
            get
            {
                TimeSpan worked = _endTime - _startTime;
                return worked.TotalHours;
            }
        }

        public WorkedHours(Issue issue)
        {
            _issue = issue;
            _projectId = issue.Project.Id;
            _idleTime = 0;
        }
        public WorkedHours()
        {
            
        }

        /// <summary>
        /// Increases the idle time by 1 [second]
        /// </summary>
        public void increaseIdleTime()
        {
            _idleTime++;
        }
    }
}
