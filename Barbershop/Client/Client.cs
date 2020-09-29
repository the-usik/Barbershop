using System;

namespace Barbershop {
    internal sealed class Client {
        public DateTime receptionDate { get; set; }
        public int pastimesInMinutes { get; set; }
        public string phoneNumber { get; set; }

        public Client(DateTime receptionDate, int pastimesInMinutes, string phoneNumber) {
            this.phoneNumber = phoneNumber;
            this.receptionDate = receptionDate;
            this.pastimesInMinutes = pastimesInMinutes;
        }
    }
}
