using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using System.Net;
using System.IO;

namespace FTTW.RedmineApi
{
    class RedmineUploadData
    {

        private static string postXMLData(string destinationUrl, string requestXml, string login, string password)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            request.Credentials = new NetworkCredential(login, password);
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }

        /// <summary>
        /// Uploads WorkedHours stored in the manager inside the RedmineXmlGetter object.
        /// </summary>
        /// <param name="getter">RedmineXmlGetter object</param>
        /// <returns>True in case of success. False otherwise.</returns>
        public static bool uploadWorkedHours(RedmineXmlGetter getter)
        {
            try
            {
                
                DateTime lastEntry = getter.getLastTimeEntry();                              

                foreach (Persistency.WorkedHours hour in getter.Manager.WorkedHours)
                {
                    if (hour.StartTime.CompareTo(lastEntry) <= 0)
                        continue;

                    XmlDocument xml = new XmlDocument();
                    xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));

                    XmlNode node = xml.CreateElement("time_entry");
                    

                    //IssueId
                    XmlNode issueId = xml.CreateElement("issue_id");
                    XmlNode issueIdText = xml.CreateTextNode(hour.IssueId.ToString());
                    issueId.AppendChild(issueIdText);
                    node.AppendChild(issueId);

                    //Hours
                    XmlNode hours = xml.CreateElement("hours");
                    XmlNode hoursText = xml.CreateTextNode(hour.Hours.ToString());
                    hours.AppendChild(hoursText);
                    node.AppendChild(hours);

                    //Spent on
                    XmlNode spentOn = xml.CreateElement("spent_on");
                    XmlNode spentOnText = xml.CreateTextNode(hour.StartTime.ToString("yyyy-MM-dd"));
                    spentOn.AppendChild(spentOnText);
                    node.AppendChild(spentOn);

                    //ActivityId
                    Config.Config config = Config.Config.getConfig();
                    XmlNode activityId = xml.CreateElement("activity_id");
                    XmlNode activityIdText = xml.CreateTextNode(config.ActivityId.ToString());
                    activityId.AppendChild(activityIdText);
                    node.AppendChild(activityId);

                    //Comment (Idle Time)
                    XmlNode comment = xml.CreateElement("comments");
                    XmlNode commentText = xml.CreateTextNode("Tempo de atividade: " + (100*(1 - hour.IdleTime/(hour.Hours*3600))).ToString() + "%");
                    comment.AppendChild(commentText);
                    node.AppendChild(comment);

                    xml.AppendChild(node);
                    
                    string response = postXMLData(getter.Manager.UserData.Link + "/time_entries.xml", xml.InnerXml, getter.Manager.UserData.RedmineLogin, getter.Manager.UserData.RedminePassword);                
                }
                return true;
            } catch (WebException ex)
            {                
                return false;
            }                
            
        }
    }
}
