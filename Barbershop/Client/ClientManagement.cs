using System;
using System.Collections.Generic;

namespace Barbershop {
    internal sealed class ClientManagement {
        public List<Client> clientList { get; }
        private WorkFlow parentContext;

        public ClientManagement(WorkFlow parentContext) {
            clientList = new List<Client>();
            this.parentContext = parentContext;
        }

        public void AddClient(Client client) {
            if (!CheckClientReceptionTime(client)) throw new FormatException("Unavailable time for reception");
            if (IsIntersectionBy(client)) throw new FormatException("The specified time is occupied by others");

            clientList.Add(client);
        }

        private bool CheckClientReceptionTime(Client client) {
            WorkTime availableWorkTime = parentContext.workTimeList
                .Find(workTime => workTime.IsAvailableClientToReception(client));
            return (availableWorkTime != null);
        }

        private bool IsIntersectionBy(Client client) {
            bool result = false;

            foreach (var workTime in parentContext.workTimeList)
                foreach (var otherClient in clientList) {
                    var clientBeginTime = client.receptionDate.TimeOfDay.TotalMinutes;
                    var clientEndTime = clientBeginTime + client.pastimesInMinutes;
                    var otherClientBeginTime = otherClient.receptionDate.TimeOfDay.TotalMinutes;
                    var otherClientEndTime = otherClientBeginTime + otherClient.pastimesInMinutes;

                    if (clientBeginTime >= otherClientBeginTime && clientBeginTime < otherClientEndTime) result = true;
                    if (clientEndTime >= otherClientBeginTime && clientEndTime < otherClientEndTime) result = true;
                }

            return result;
        }
    }
}
