using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.RedmineApi
{
    public class Issue
    {
        private int _id;
        private Project _project;
        private int _trackerId;
        private string _trackerName;
        private int _statusId;
        private string _statusName;
        private int _priorityId;
        private string _priorityName;
        private int _authorId;
        private string _authorName;
        private int _assignedToId;
        private string _assignedToName;
        private string _subject;
        private string _description;
        private DateTime _startDate;
        private DateTime _dueDate;
        private int _doneRatio;
        private bool _isPrivate;
        private double _estimatedHours;
        private double _timeSpent;
        private DateTime _createdOn;
        private DateTime _updatedOn;
        private DateTime _closedOn;
        private bool _isActive;
        //For the history:
        private TimeSpan _workedThisSession;

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        internal Project Project
        {
            get
            {
                return _project;
            }

            set
            {
                _project = value;
            }
        }

        public int TrackerId
        {
            get
            {
                return _trackerId;
            }

            set
            {
                _trackerId = value;
            }
        }

        public string TrackerName
        {
            get
            {
                return _trackerName;
            }

            set
            {
                _trackerName = value;
            }
        }

        public int StatusId
        {
            get
            {
                return _statusId;
            }

            set
            {
                _statusId = value;
            }
        }

        public string StatusName
        {
            get
            {
                return _statusName;
            }

            set
            {
                _statusName = value;
            }
        }

        public int PriorityId
        {
            get
            {
                return _priorityId;
            }

            set
            {
                _priorityId = value;
            }
        }

        public string PriorityName
        {
            get
            {
                return _priorityName;
            }

            set
            {
                _priorityName = value;
            }
        }

        public int AuthorId
        {
            get
            {
                return _authorId;
            }

            set
            {
                _authorId = value;
            }
        }

        public string AuthorName
        {
            get
            {
                return _authorName;
            }

            set
            {
                _authorName = value;
            }
        }

        public int AssignedToId
        {
            get
            {
                return _assignedToId;
            }

            set
            {
                _assignedToId = value;
            }
        }

        public string AssignedToName
        {
            get
            {
                return _assignedToName;
            }

            set
            {
                _assignedToName = value;
            }
        }

        public string Subject
        {
            get
            {
                return _subject;
            }

            set
            {
                _subject = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }

            set
            {
                _startDate = value;
            }
        }

        public DateTime DueDate
        {
            get
            {
                return _dueDate;
            }

            set
            {
                _dueDate = value;
            }
        }

        public int DoneRatio
        {
            get
            {
                return _doneRatio;
            }

            set
            {
                _doneRatio = value;
            }
        }

        public bool IsPrivate
        {
            get
            {
                return _isPrivate;
            }

            set
            {
                _isPrivate = value;
            }
        }

        public double EstimatedHours
        {
            get
            {
                return _estimatedHours;
            }

            set
            {
                _estimatedHours = value;
            }
        }

        public DateTime CreatedOn
        {
            get
            {
                return _createdOn;
            }

            set
            {
                _createdOn = value;
            }
        }

        public DateTime UpdatedOn
        {
            get
            {
                return _updatedOn;
            }

            set
            {
                _updatedOn = value;
            }
        }

        public DateTime ClosedOn
        {
            get
            {
                return _closedOn;
            }

            set
            {
                _closedOn = value;
            }
        }

        public TimeSpan WorkedThisSession
        {
            get
            {
                return _workedThisSession;
            }

            set
            {
                _workedThisSession = value;
            }
        }

        public double TimeSpent
        {
            get
            {
                return _timeSpent;
            }

            set
            {
                _timeSpent = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                _isActive = value;
            }
        }

        public Issue()
        {
            _isActive = false;
            _workedThisSession = new TimeSpan();
        }

    }
}
