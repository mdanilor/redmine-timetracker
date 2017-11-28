using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using FTTW.Persistency;
using FTTW.RedmineApi;
using System.IO;

namespace FTTW.WSCom
{
    class ExchangeData
    {      

        private static string getAddress ()
        {
            return File.ReadAllText("address");
        }
        public static DateTime getLastEntry(DataManager manager)
        {
            Userdata user = manager.UserData;          
            string link = getAddress() + "/index.php?request=1&id=" + user.RedmineId;
            string dateString = new WebClient().DownloadString(link);

            DateTime result;
            if (DateTime.TryParse(dateString, out result))
                return result;
            return DateTime.MinValue;
        }

        public static bool uploadWorkedHours(DataManager manager)
        {            
            try {
                DateTime lastEntry = getLastEntry(manager);
                foreach (WorkedHours hour in manager.WorkedHours)
                {
                    if (hour.StartTime.CompareTo(lastEntry) >= 0)
                    {
                        string link = getAddress() + "/index.php?request=0";
                        link += "&id=" + manager.UserData.RedmineId;
                        link += "&projectId=" + hour.ProjectId;
                        link += "&issueId=" + hour.IssueId;
                        link += "&startTime=" + hour.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                        link += "&endTime=" + hour.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                        link += "&idleTime=" + hour.IdleTime.ToString();
                        string result = new WebClient().DownloadString(link);
                        if (result != "\r\n1")
                            return false;
                    }
                }
            } catch (WebException ex)
            {
                return false;
            }
            return true;
        }

        public static int getWorkedIssueSeconds(Issue issue, DataManager manager)
        {
            string link = getAddress() + "/index.php?request=2&id=" + manager.UserData.RedmineId + "&issueId=" + issue.Id;
            string result = new WebClient().DownloadString(link);
            int numResult;
            if (Int32.TryParse(result, out numResult))
                return numResult;
            return 0;
        }

        public static int getWorkedProjectSeconds(Project project, DataManager manager)
        {
            string link = getAddress() + "/index.php?request=3&id=" + manager.UserData.RedmineId + "&projectId=" + project.Id;
            string result = new WebClient().DownloadString(link);
            int numResult;
            if (Int32.TryParse(result, out numResult))
                return numResult;
            return 0;
        }

        public static TimeSpan getWorkedHoursBetweenDates(DateTime start, DateTime end, DataManager manager)
        {
            string link = getAddress() + "/index.php?request=4&id=" + manager.UserData.RedmineId.ToString() + "&startTime=" + start.ToString("yyyy-MM-dd HH:mm:ss") + "&endTime=" + end.ToString("yyyy-MM-dd HH:mm:ss");
            string result = new WebClient().DownloadString(link);
            int numResult;
            if (!Int32.TryParse(result, out numResult))
                return TimeSpan.Zero;
            return TimeSpan.FromSeconds(numResult);
        }
    }
}
