using System;
using System.Globalization;

namespace MDemCalendarTools {
    public static class Kalendarz {

        public static byte NrAktTyg => (byte)Tydzien_w_roku(DateTime.Now);
        /// <summary> Sprawdza czy podany string da się sparsować do int, i czy mieści się w zakresie 1 - 52 (53) </summary>
        public static bool PoprawnyNrTyg(string tydzStr) {
            return int.TryParse(tydzStr, out int tydz) && (tydz <= GetWeeksInYear(DateTime.Now.Year) && tydz > 0);
        }
        /// <summary> Odległość między dwoma tygodniami. Odległość ujemna, gdy tydz1 jest później niż tydz2. (UWAGA! Może liczyć z błędem przy odległości większej niż 2 lata!) </summary>
        public static int OdlMiedzyTyg((byte tydz, int rok) tydz1, (byte tydz, int rok) tydz2) {
            if (tydz1.rok == tydz2.rok) {
                return tydz2.tydz - tydz1.tydz;
            }
            var zakresLat = Math.Abs(tydz1.rok - tydz2.rok);            
            if (tydz2.rok > tydz1.rok) {
                return (GetWeeksInYear(tydz1.rok) - tydz1.tydz) + tydz2.tydz + ((zakresLat - 1) * GetWeeksInYear(tydz2.rok - 1));
            }
            return - ((GetWeeksInYear(tydz2.rok) - tydz2.tydz) + tydz1.tydz + ((zakresLat - 1) * GetWeeksInYear(tydz1.rok - 1)));
        }
        /// <summary> Odległość między dwoma tygodniami. Odległość ujemna, gdy tydz1 jest później niż tydz2. (UWAGA! Może liczyć z błędem przy odległości większej niż 2 lata!) </summary>
        public static int OdlMiedzyTyg(byte tydz1, int rok1, byte tydz2, int rok2) {
            if (rok1 == rok2) {
                return tydz2 - tydz1;
            }
            var zakresLat = Math.Abs(rok1 - rok2);            
            if (rok2 > rok1) {
                return (GetWeeksInYear(rok1) - tydz1) + tydz2 + ((zakresLat - 1) * GetWeeksInYear(rok2 - 1));
            }
            return - ((GetWeeksInYear(rok2) - tydz2) + tydz1 + ((zakresLat - 1) * GetWeeksInYear(rok1 - 1)));
        }
        /// <summary> Ilość tygodni w danym roku </summary>
        public static int GetWeeksInYear(int year) {
            DateTime date1 = new DateTime(year, 12, 31);
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date1);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
                date1 = date1.AddDays(-3);
            }
            int ret = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return ret;
        }
        /// <summary> Zwraca numer tygodnia daty 'time' </summary>
        public static int Tydzien_w_roku(DateTime time) {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        /// <summary> Zwraca datę danego dnia tygodnia, dla podanego numery tyg. i roku </summary>
        public static DateTime GetDateFromWeek(int weak, int year, DayOfWeek day) {
            DateTime jan1          = new DateTime(year, 1, 1);
            int      daysOffset    = DayOfWeek.Thursday - jan1.DayOfWeek;
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var      cal           = CultureInfo.CurrentCulture.Calendar;
            int      firstWeek     = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var weekNum = weak;
            if (firstWeek == 1) { weekNum -= 1; }
            var result = firstThursday.AddDays(weekNum * 7);
            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            DateTime pierwszy_dzien = result.AddDays(-3);
            switch (day) {
                case DayOfWeek.Monday:
                    return pierwszy_dzien.AddDays(0).Date;
                case DayOfWeek.Tuesday:
                    return pierwszy_dzien.AddDays(1).Date;
                case DayOfWeek.Wednesday:
                    return pierwszy_dzien.AddDays(2).Date;
                case DayOfWeek.Thursday:
                    return pierwszy_dzien.AddDays(3).Date;
                case DayOfWeek.Friday:
                    return pierwszy_dzien.AddDays(4).Date;
                case DayOfWeek.Saturday:
                    return pierwszy_dzien.AddDays(5).Date;
                case DayOfWeek.Sunday:
                    return pierwszy_dzien.AddDays(6).Date;
                default:
                    throw new Exception("TAKI DZIEŃ NIE ISTNIEJE");
            }
        }

    }
}
