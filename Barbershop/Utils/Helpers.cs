using System;

namespace Barbershop {
    internal sealed class Helpers {
        public static TimeSpan ExtractTimeFromString(string time) {
            string[] splittedTimeString = time.Split(':');

            if (splittedTimeString.Length > 2) throw new FormatException("Invalid time format");
            int hours = int.Parse(splittedTimeString[0]);
            int minutes = int.Parse(splittedTimeString[1]);

            return new TimeSpan(hours, minutes, 0);
        }
    }
}
