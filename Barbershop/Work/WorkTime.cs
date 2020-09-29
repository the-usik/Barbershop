using System;

namespace Barbershop {
    class WorkTime {
        public TimeSpan startTime { get; }
        public TimeSpan endTime { get; }

        public WorkTime(TimeSpan startTime, TimeSpan endTime) {
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public bool IsAvailableClientToReception(Client client) {
            TimeSpan timeClient = client.receptionDate.TimeOfDay;
            int interval = CalculateInterval();
            int clientInterval = (int)timeClient.TotalMinutes + client.pastimesInMinutes;
            bool availableByHours = timeClient.Hours >= startTime.Hours && timeClient.Hours <= endTime.Hours;

            return (availableByHours && clientInterval >= startTime.TotalMinutes && clientInterval >= interval && clientInterval <= endTime.TotalMinutes);
        }

        public int CalculateInterval() {
            return (int)Math.Round(endTime.Subtract(startTime).TotalMinutes, 0);
        }
    }
}
