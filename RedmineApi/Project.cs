using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.RedmineApi
{
    class Project
    {
        private int _id;
        private string _name;
        private IssueList _issues;
        private string _identifier;
        private string _description;
        private int _status;
        private bool _isPublic;
        private int _parentId;

        private int _type; //0: Project; 1: Issue


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

        internal IssueList Issues
        {
            get
            {
                return _issues;
            }
        }

        public string Identifier
        {
            get
            {
                return _identifier;
            }

            set
            {
                _identifier = value;
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

        public int Status
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

        public bool IsPublic
        {
            get
            {
                return _isPublic;
            }

            set
            {
                _isPublic = value;
            }
        }

        public int ParentId
        {
            get
            {
                return _parentId;
            }

            set
            {
                _parentId = value;
            }
        }

        public int Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        public Project ()
        {
            _issues = new IssueList();
        }

        /// <summary>
        /// Adds an issue to project.
        /// </summary>
        /// <param name="issue">Issue to be added</param>
        public void addIssue(Issue issue)
        {
            _issues.addIssue(issue);
        }
    }
}
