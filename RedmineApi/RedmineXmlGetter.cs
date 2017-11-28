using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using FTTW.Persistency;
using HtmlAgilityPack;

namespace FTTW.RedmineApi
{
    class RedmineXmlGetter
    {
        private XmlDocument _xmlDoc;
        private ProjectList _projects;
        private string _link;
        private DataManager _manager;

        public ProjectList Projects
        {
            get
            {
                return _projects;
            }
        }

        public DataManager Manager
        {
            get
            {
                return _manager;
            }
        }

        public RedmineXmlGetter(DataManager manager)
        {
            _manager = manager;
            _xmlDoc = new XmlDocument();
            this._link = manager.UserData.Link;

            //Colocando a autenticação
            XmlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = new NetworkCredential(manager.UserData.RedmineLogin, manager.UserData.RedminePassword);
            _xmlDoc.XmlResolver = resolver;
        }

        /// <summary>
        /// Get the time worked on a single day.
        /// </summary>
        /// <param name="date">Date to get the time worked</param>
        /// <returns>A TimeSpan with the amount of time worked on date.</returns>
        public TimeSpan getWorkedHoursInDate(DateTime date)
        {
            int offset = 0;
            int totalCount = 0;
            TimeSpan time = new TimeSpan();
            do
            {
                _xmlDoc.Load(_link + "/time_entries.xml?sort=created_on&limit=100&spent_on=" + date.ToString("yyyy-MM-dd") + 
                    "&offset=" + offset.ToString() + "&user_id=" + _manager.UserData.RedmineId);
                if (offset == 0)
                    totalCount = Int32.Parse(_xmlDoc.ChildNodes[1].Attributes["total_count"].Value);
                totalCount = Math.Max(0, totalCount - 100);
                offset += 100;

                foreach (XmlNode timeEntry in _xmlDoc.ChildNodes[1].ChildNodes)
                {
                    foreach (XmlNode node in timeEntry.ChildNodes)
                    {
                        if (node.Name == "hours")
                        {
                            time += TimeSpan.FromSeconds(3600 * double.Parse(node.InnerText, System.Globalization.CultureInfo.InvariantCulture));
                            break;
                        }
                    }
                }

            } while (totalCount > 0);
            return time;
        }

        /// <summary>
        /// Update all issues spent hours based on the data in the server.
        /// </summary>
        private void updateSpentHours()
        {
            foreach (Project project in Projects.List)
            {
                foreach (Issue issue in project.Issues.List)
                {
                    _xmlDoc.Load(_link + "/issues/" + issue.Id + ".xml");
                    foreach (XmlNode node in _xmlDoc.ChildNodes[1].ChildNodes)
                    {
                        if (node.Name == "spent_hours")
                        {
                            issue.TimeSpent = 3600 * double.Parse(node.InnerText, System.Globalization.CultureInfo.InvariantCulture);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads all projects and issues.
        /// </summary>
        public void Load()
        {
            Project project = null;
            Issue issue;

            _projects = new ProjectList();

            Project all = new Project();
            all.Name = "Todos";
            all.Id = 0;
            _projects.addProject(all);

            try
            {
                //Getting the projects
                int offset = 0;
                int totalCount = 0;
                do
                {
                    _xmlDoc.Load(_link + "/projects.xml?offset=" + offset.ToString() + "&limit=100");                    
                    if (offset == 0)
                        totalCount = Int32.Parse(_xmlDoc.ChildNodes[1].Attributes["total_count"].Value);
                    totalCount = Math.Max(0, totalCount - 100);
                    offset += 100;

                    foreach (XmlNode node in _xmlDoc.ChildNodes[1].ChildNodes)
                    {
                        //In case it's not a project
                        if (node.Name != "project")
                            continue;

                        project = new Project();

                        project.Type = 0;
                        project.Id = Int32.Parse(node.SelectSingleNode("id").InnerText);
                        project.Name = node.SelectSingleNode("name").InnerText;
                        project.Identifier = node.SelectSingleNode("identifier").InnerText;
                        project.Description = node.SelectSingleNode("description").InnerText;
                        project.Status = Int32.Parse(node.SelectSingleNode("status").InnerText);
                        project.IsPublic = Boolean.Parse(node.SelectSingleNode("is_public").InnerText);
                        if (node.SelectSingleNode("parent") == null)
                            project.ParentId = -1;
                        else
                            project.ParentId = Int32.Parse(node.SelectSingleNode("parent").Attributes["id"].Value); 
                        
                        Projects.addProject(project);                        
                    }
                } while (totalCount > 0);

                //Getting the issues
                offset = 0;
                totalCount = 0;
                do
                {
                    _xmlDoc.Load(_link + "/issues.xml?assigned_to_id=me&sort=project&offset=" + offset.ToString() + "&limit=100");

                    if (offset == 0)
                        totalCount = Int32.Parse(_xmlDoc.ChildNodes[1].Attributes["total_count"].Value);
                    totalCount = Math.Max(0, totalCount - 100); 
                    offset += 100;

                    int lastProjectId = -1;

                    foreach (XmlNode node in _xmlDoc.ChildNodes[1].ChildNodes)
                    {
                        //In case it's not an issue
                        if (node.Name != "issue")
                            continue;

                        int projectId = Int32.Parse(node.SelectSingleNode("project").Attributes["id"].Value);
                        if (lastProjectId != projectId)
                        {
                            project = Projects.getProjectById(projectId);
                            lastProjectId = projectId;
                        }

                        if (project == null)
                            return;

                        issue = new Issue();
                        issue.Id = Int32.Parse(node.SelectSingleNode("id").InnerText);
                        issue.Project = project;
                        issue.TrackerId = Int32.Parse(node.SelectSingleNode("tracker").Attributes["id"].Value);
                        issue.TrackerName = node.SelectSingleNode("tracker").Attributes["name"].Value;
                        issue.StatusId = Int32.Parse(node.SelectSingleNode("status").Attributes["id"].Value);
                        issue.StatusName = node.SelectSingleNode("status").Attributes["name"].Value;
                        issue.PriorityId = Int32.Parse(node.SelectSingleNode("priority").Attributes["id"].Value);
                        issue.PriorityName = node.SelectSingleNode("priority").Attributes["name"].Value;
                        issue.AuthorId = Int32.Parse(node.SelectSingleNode("author").Attributes["id"].Value);
                        issue.AuthorName = node.SelectSingleNode("author").Attributes["name"].Value;
                        issue.AssignedToId = Int32.Parse(node.SelectSingleNode("assigned_to").Attributes["id"].Value);
                        issue.AssignedToName = node.SelectSingleNode("assigned_to").Attributes["name"].Value;
                        issue.Subject = node.SelectSingleNode("subject").InnerText;
                        issue.Description = node.SelectSingleNode("description").InnerText;
                        issue.StartDate = DateTime.Parse(node.SelectSingleNode("start_date").InnerText);

                        project.addIssue(issue);
                        _projects.List[0].addIssue(issue);
                    }
                } while (totalCount > 0);
                updateSpentHours();
                _projects.concatChildIssues();
                /*
                //Getting custom queries:
                offset = 0;
                totalCount = 0;

                List<Project> customQueries = new List<Project>();

                do
                {
                    _xmlDoc.Load(_link + "/queries.xml?offset=" + offset.ToString() + "&limit=100");
                    if (offset == 0)
                        totalCount = Int32.Parse(_xmlDoc.ChildNodes[1].Attributes["total_count"].Value);
                    totalCount = Math.Max(0, totalCount - 100);
                    offset += 100;

                    foreach (XmlNode node in _xmlDoc.ChildNodes[1].ChildNodes)
                    {
                        //In case it's not a project
                        if (node.Name != "query")
                            continue;

                        project = new Project();

                        project.Type = 1;
                        project.Id = Int32.Parse(node.SelectSingleNode("id").InnerText);
                        project.Name = node.SelectSingleNode("name").InnerText;                        
                        project.IsPublic = Boolean.Parse(node.SelectSingleNode("is_public").InnerText);                    
                        
                        customQueries.Add(project);
                    }

                } while (totalCount > 0);

                //Getting custom query's issues
                //Getting the issues                
                foreach (Project query in customQueries)
                {
                    offset = 0;
                    totalCount = 0;
                    do
                    {
                        _xmlDoc.Load(_link + "/issues.xml?query_id=" + query.Id + "&assigned_to_id=me&sort=project&offset=" + offset.ToString() + "&limit=100");

                        if (offset == 0)
                            totalCount = Int32.Parse(_xmlDoc.ChildNodes[1].Attributes["total_count"].Value);
                        totalCount = Math.Max(0, totalCount - 100);
                        offset += 100;                       

                        foreach (XmlNode node in _xmlDoc.ChildNodes[1].ChildNodes)
                        {
                            //In case it's not an issue
                            if (node.Name != "issue")
                                continue;

                            int projectId = Int32.Parse(node.SelectSingleNode("project").Attributes["id"].Value);
                            int issueId = Int32.Parse(node.SelectSingleNode("id").InnerText);

                            Issue queryIssue = _projects.findIssue(projectId, issueId);
                            if (queryIssue != null)
                            {
                                query.addIssue(queryIssue);
                            }
                                    
                        }
                    } while (totalCount > 0);
                }
                _projects.List.AddRange(customQueries);*/


            }
            catch (WebException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get the datetime of the last time entry
        /// </summary>
        /// <returns>Last entry's datetime</returns>
        public DateTime getLastTimeEntry()
        {
            DateTime lastEntry = DateTime.MinValue;
            _xmlDoc.Load(_link + "/time_entries.xml?sort=updated_on&limit=1");

            foreach (XmlNode node in _xmlDoc.ChildNodes[1].ChildNodes)
            {
                if (node.Name == "time_entry")
                {
                    lastEntry = DateTime.Parse(node.SelectSingleNode("updated_on").InnerText);
                    break;
                }
            }

            return lastEntry;
        }

        /// <summary>
        /// Gets an userdata object based on the credentials input.
        /// </summary>
        /// <param name="link">Link to the redmine server</param>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <returns>Userdata object.</returns>
        public static Userdata getSelfUserdata(string link, string login, string password)
        {
            ServicePointManager
    .ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;
            XmlDocument xmlDoc = new XmlDocument();
            Userdata ud = new Userdata();
            ud.RedmineLogin = login;
            ud.RedminePassword = password;

            //Colocando a autenticação
            XmlResolver resolver = new XmlUrlResolver();

            resolver.Credentials = new NetworkCredential(login, password);
            xmlDoc.XmlResolver = resolver;

            if (!checkRedmineAvailability(link))
                throw new Exceptions.RedmineServerInaccessibleException();

            try
            {
                xmlDoc.Load(link + "/users/current.xml");
                ud.RedmineId = Int32.Parse(xmlDoc.ChildNodes[1].SelectSingleNode("id").InnerText);
                ud.Name = xmlDoc.ChildNodes[1].SelectSingleNode("firstname").InnerText + xmlDoc.ChildNodes[1].SelectSingleNode("lastname").InnerText;
                ud.RedmineLogin = xmlDoc.ChildNodes[1].SelectSingleNode("api_key").InnerText;
                ud.RedminePassword = "abcdefg;";
                ud.Link = link;
                return ud;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure)
                    throw new Exceptions.RedmineServerInaccessibleException();
                if (e.Status == WebExceptionStatus.ProtocolError)
                    throw new Exceptions.InvalidLoginPasswordCombinationException();
                throw e;
            }
        }

        public static bool checkRedmineAvailability(string link)
        {
            var htmlWeb = new HtmlWeb();
            HtmlDocument htmlDoc;
            try {
                htmlDoc = htmlWeb.Load(link);
            } catch (WebException ex)
            {
                return false;
            } catch (UriFormatException ex)
            {
                return false;
            }
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//meta/@content");
            if (nodes == null)
                return false;
            foreach (HtmlNode node in nodes)
            {
                if (node.Attributes["name"] == null || node.Attributes["content"] == null)
                    continue;
                if (node.Attributes["name"].Value == "description")
                {
                    if (node.Attributes["content"].Value == "Redmine")
                        return true;
                    return false;
                }
            }
            return false;
        }
    }
}
