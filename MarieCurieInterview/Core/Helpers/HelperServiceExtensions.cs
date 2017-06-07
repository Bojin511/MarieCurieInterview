using MarieCurie.Interview.Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarieCurieInterview.Core.Helpers
{
    public static class HelperServiceExtensions
    {
        public static bool IsOpenNow(this List<int> openingHours) 
        {
            var currentHour = DateTime.Now.Hour;

            if (openingHours.Count >= 2)
            {
                int openingHour = openingHours.First();
                int closingHour = openingHours.Last();

                return openingHour <= currentHour && currentHour < closingHour;
            } else return false;
        }

        public static int? WillClose(this List<int> openingHours)
        {
            var currentHour = DateTime.Now.Hour;

            int openingHour = openingHours.First();
            int closingHour = openingHours.Last();

            if (openingHour <= currentHour && currentHour < closingHour) return closingHour;
            else return null;
        }

        public static int? ServiceWillClose(this HelperService service)
        {
            var openingHours = service.OpeningHoursForDayOfWeek(DateTime.Today.DayOfWeek);
            int? willClose = openingHours.WillClose();

            return willClose;
        }

        public static bool ServiceIsOpenNow(this HelperService service)
        {
            var openingHours = service.OpeningHoursForDayOfWeek(DateTime.Today.DayOfWeek);
            bool isOpenNow = openingHours.IsOpenNow();

            return isOpenNow;
        }

        public static string ExtraInfo(this HelperService service)
        {
            string info = "";

            if (service.ServiceIsOpenNow())
            {
                if (service.ServiceWillClose().HasValue)
                {
                    info = string.Format("Open until {0}", service.ServiceWillClose().Value);
                }

                else throw new Exception("An error occured when calculating when the service will close");
            }
            else 
            {
                info = string.Format("Reopens {0}", service.NextOpen());
            }



            return info;
        }

        public static string NextOpen(this HelperService service)
        {
            var currentHour = DateTime.Now.Hour;

            var openingHours = service.OpeningHoursForDayOfWeek(DateTime.Today.DayOfWeek);

            int openingHour = openingHours.First();
            int closingHour = openingHours.Last();

            DateTime openTime = DateTime.Today.AddHours(openingHour);

            if (currentHour < openingHour) 
                return string.Format("today at {0}", openTime.ToString("hh:mm tt"));
            else
            {
                DayOfWeek tomorrowDayOfWeek = DateTime.Today.DayOfWeek+1;

                string reopensText = service.ReOpensText(tomorrowDayOfWeek);
                if (reopensText == tomorrowDayOfWeek.ToString())
                    return "tomorrow";
                else
                    return reopensText;
            }
        }

        private static string ReOpensText(this HelperService service, DayOfWeek dayOfWeek)
        {
            var openingHours = service.OpeningHoursForDayOfWeek(dayOfWeek);

            if (openingHours.First() == 0) 
                return service.ReOpensText(dayOfWeek + 1);
            else 
                return dayOfWeek.ToString();
        }

        public static List<int> OpeningHoursForDayOfWeek(this HelperService service, DayOfWeek dayOfWeek)
        {

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return service.MondayOpeningHours;

                case DayOfWeek.Tuesday:
                    return service.TuesdayOpeningHours;
                    
                case DayOfWeek.Wednesday:
                    return service.WednesdayOpeningHours;
                    
                case DayOfWeek.Thursday:
                    return service.ThursdayOpeningHours;
                    
                case DayOfWeek.Friday:
                    return service.FridayOpeningHours;
                   
                case DayOfWeek.Saturday:
                    return service.SaturdayOpeningHours;
                
                case DayOfWeek.Sunday:
                    return service.SundayOpeningHours;

                   
            }

            return null;

        }
    }
}