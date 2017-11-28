using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FTTW.Front
{
    class FProject
    {
        private int _id;
        private string _name;
        private List<FIssue> _issues;
        private string _description;
        private TimeSpan _time;
        private Persistency.DataManager _manager;
        private int _type;
        public int Id
        {
            get
            {
                return _id;
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

        internal List<FIssue> Issues
        {
            get
            {
                return _issues;
            }
        }

        public string Time
        {
            get
            {
                return _time.ToString(@"hh\:mm\:ss");
            }
        }

        public TimeSpan CurrentSessionTime
        {
            get
            {
                TimeSpan t = new TimeSpan();
                foreach (FIssue issue in _issues)
                {
                    t += issue.CurrentSessionTime;
                }
                return t;
            }
        }

        public Brush ForegroundColor
        {
            get
            {
                if (_type == 0)
                    return Brushes.Black;
                else
                    return Brushes.DarkSalmon;
            }
        }

        private FProject() {
        }

        public FIssue getIssueById(int id)
        {
            foreach (FIssue issue in _issues)
            {
                if (issue.Id == id)
                    return issue;
            }
            return null;
        }

        private static FProject getFromRAProject(RedmineApi.Project pProject, Persistency.DataManager manager)
        {
            FProject project = new FProject();
            project._type = pProject.Type;
            project._description = pProject.Description;
            project._id = pProject.Id;
            project._issues = FIssue.getFrontIssueList(pProject.Issues, manager);
            project._manager = manager;
            project._name = pProject.Name;
            project._time = new TimeSpan();

            foreach (FIssue issue in project._issues)
                project._time += issue.Time;

            return project;
        }

        private static List<FProject> getFrontProjectList(RedmineApi.ProjectList pProjects, Persistency.DataManager manager)
        {
            List<RedmineApi.Project> rapl = pProjects.List;
            List<FProject> projects = new List<FProject>();

            foreach (RedmineApi.Project p in rapl)
            {
                projects.Add(getFromRAProject(p, manager));
            }

            return projects;
        }

        /// <summary>
        /// Get all projects and issues assigned to the user.
        /// </summary>
        /// <param name="getter">RedmineXmlGetter object</param>
        /// <returns>List of all projects the user is allowed to see.</returns>
        public static List<FProject> getAll(RedmineApi.RedmineXmlGetter getter)
        {            
            getter.Load();
            return getFrontProjectList(getter.Projects, getter.Manager);
        }

        public List<FIssue> getWithFilter(string filter)
        {
            if (filter == null || filter == string.Empty) return _issues;
            return _issues.Where(issue => issue.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        }
        
    }
}
