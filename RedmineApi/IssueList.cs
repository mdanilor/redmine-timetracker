using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.RedmineApi
{
    class IssueList
    {
        private List<Issue> _list;

        internal List<Issue> List
        {
            get
            {
                return _list;
            }
        }

        public IssueList ()
        {
            _list = new List<Issue>();
        }

        private IssueList(IssueList i1, IssueList i2)
        {
            _list = new List<Issue>();
            _list.AddRange(i1._list);
            _list.AddRange(i2._list);
        }

        /// <summary>
        /// Overloads the sum operator to concat the two lists.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static IssueList operator +(IssueList i1, IssueList i2)
        {
            return new IssueList(i1, i2);
        }



        public Issue getIssueById(int id)
        {
            foreach (Issue issue in _list)
            {
                if (issue.Id == id)
                    return issue;
            }
            return null;
        }

        /// <summary>
        /// Adds an issue
        /// </summary>
        /// <param name="issue">Issue to be added</param>
        public void addIssue(Issue issue)
        {
            if (!_list.Contains(issue))
                _list.Add(issue);
        }
       
    }
}
