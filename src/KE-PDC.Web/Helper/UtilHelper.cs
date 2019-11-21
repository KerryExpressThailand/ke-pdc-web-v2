using KE_PDC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.Helper
{
    public class UtilHelper
    {
        public decimal ToMoney(string moneyText, string moneySign = null, bool hasDecimal = true)
        {
            try
            {
                if (moneyText.Contains(",") || moneyText.Contains("."))
                {
                    return Convert.ToDecimal(moneyText);
                }

                if (moneyText.Length > 2 && hasDecimal)
                {
                    return Convert.ToDecimal(moneyText);
                }
                else
                {
                    return Convert.ToDecimal(string.Format("{0}{1}.00", moneySign, moneyText));
                }
            }
            catch
            {
                return 0;
            }
        }

        public int ToInt32(string numberText)
        {
            try
            {
                return Convert.ToInt32(numberText);
            }
            catch
            {
                return 0;
            }
        }

        public string Scientific(string numberText)
        {
            return decimal.Parse(numberText, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint).ToString();
        }

       
        public DateTime? ToDate(string dateText, string dateFormat = "dd/MM/yyyy", string _culture = "en-US")
        {
            try
            {

                if (dateText.Contains("-"))
                {
                    DateTime dateTime = DateTime.ParseExact(dateText, "dd-MMM-yyyy", CultureInfo.GetCultureInfo(_culture));
                    return dateTime;
                }
                else 
                {
                    DateTime dateTime = DateTime.ParseExact(dateText, dateFormat, CultureInfo.GetCultureInfo(_culture));
                    return dateTime;
                }
            
            }
            catch
            {
                if (dateText.Contains("AM") || dateText.Contains("PM"))
                {
                    return Convert.ToDateTime(dateText);
                }

                if (!DateTime.TryParseExact(dateText, dateFormat, CultureInfo.GetCultureInfo(_culture), DateTimeStyles.None, out DateTime dateTime) && !DateTime.TryParseExact(dateText, "ddMMyy", CultureInfo.GetCultureInfo(_culture), DateTimeStyles.None, out dateTime))
                {
                    return null;
                }
                return dateTime;
            }
        }

        public TimeSpan? ToTime(string timeText, string timeFormat = "HH:mm:ss")
        {
            try
            {
                return TimeSpan.Parse(timeText);
            }
            catch
            {
                if (timeText.Contains("AM") || timeText.Contains("PM"))
                {
                    return Convert.ToDateTime(timeText).TimeOfDay;
                }
                if (!TimeSpan.TryParseExact(timeText, timeFormat, CultureInfo.InvariantCulture, out TimeSpan timeSpan) && !TimeSpan.TryParseExact(timeText, "HH:mm:ss", CultureInfo.InvariantCulture, out timeSpan))
                {
                    return null;
                }
                return timeSpan;
            }
        }

        public string Unleading(string paddingText)
        {
            return paddingText.TrimStart(new char[] { '0' });
        }

        public string IsEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            value = value.Trim();
            if (value.All(x => x == '0'))
            {
                return string.Empty;
            }
            return value.Trim();
        }

       

    }
}
