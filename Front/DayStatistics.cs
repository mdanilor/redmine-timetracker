using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.Front
{
    class DayStatistics
    {
        public enum Days : int { Domingo, Segunda, Terça, Quarta, Quinta, Sexta, Sábado};

        private int _day;
        private TimeSpan _time;
        private Stopwatch _watch;
        RedmineApi.RedmineXmlGetter _getter;

        public string Day
        {
            get
            {
                switch (_day)
                {
                    case (int)Days.Domingo:
                        return "Domingo";                        
                    case (int)Days.Segunda:
                        return "Segunda-feira";                        
                    case (int)Days.Terça:
                        return "Terça-feira";                        
                    case (int)Days.Quarta:
                        return "Quarta-feira";                        
                    case (int)Days.Quinta:
                        return "Quinta-feira";                        
                    case (int)Days.Sexta:
                        return "Sexta-feira";                        
                    case (int)Days.Sábado:
                        return "Sábado";                        
                    default:
                        return "Hoje";
                }
            }
        }

        public string Time
        {
            get
            {
                TimeSpan elapsed = _time + _watch.Elapsed;
                return elapsed.ToString(@"hh\:mm\:ss");
            }
        }
        private DayStatistics()
        {
            _watch = new Stopwatch();
        }
        private DayStatistics (int daysAgo, RedmineApi.RedmineXmlGetter getter)
        {
            _getter = getter;
            _watch = new Stopwatch();
            double daysToSubtract = -daysAgo;
            _day = (int)DateTime.Today.AddDays(-daysAgo).DayOfWeek;
            _time = _getter.getWorkedHoursInDate(DateTime.Today.AddDays(-daysAgo));
        }
        
        /// <summary>
        /// Get the amount of hours worked on the last 7 days.
        /// </summary>
        /// <param name="getter">RedmineXmlGetter instance</param>
        /// <returns>List of last 7 days (including today).</returns>
        public static List<DayStatistics> getPastWeek(RedmineApi.RedmineXmlGetter getter)
        {
            List<DayStatistics> days = new List<DayStatistics>();
            DayStatistics today = new DayStatistics();
            today._day = (int)DateTime.Today.DayOfWeek;
            today._time = getter.getWorkedHoursInDate(DateTime.Today);
            days.Add(today);
            for (int i = 1; i < 7; i++) 
                days.Add(new DayStatistics(i, getter));
            return days;
        }

        /// <summary>
        /// Starts counting time. Can be used to track today's time.
        /// </summary>
        public void startWatch()
        {
            _watch.Start();
        }

        /// <summary>
        /// Stops tracking time.
        /// </summary>
        public void stopWatch()
        {
            _watch.Stop();
        }

        
    }
}
