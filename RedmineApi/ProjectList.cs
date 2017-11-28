using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.RedmineApi
{
    class ProjectList
    {
        private List<Project> _list;

        public List<Project> List
        {
            get
            {
                return _list;
            }       
        }

        public ProjectList ()
        {
            _list = new List<Project>();
        }

        public Project getProjectById(int id)
        {
            foreach (Project project in _list)
            {
                if (project.Id == id)
                    return project;
            }
            return null;
        }

        public void concatChildIssues()
        {
            foreach (Project project in _list)
            {
                if (project == _list[0] || project.Type == 1) continue;
                if (project.ParentId == -1)
                    continue;
                Project parent = getProjectById(project.ParentId);
                if (parent == null)
                    continue;
                parent.Issues.List.AddRange(project.Issues.List);
            }
        }

        public void addProject(Project project)
        {
            if (!_list.Contains(project))
                _list.Add(project);
        }

        public Issue findIssue(int projectId, int issueId)
        {
            foreach (Project project in _list)
            {
                if (project.Id == projectId)
                {
                    foreach (Issue issue in project.Issues.List)
                    {
                        if (issue.Id == issueId)
                            return issue;
                    }
                    break;
                }
            }
            return null;
        }
    }
}
