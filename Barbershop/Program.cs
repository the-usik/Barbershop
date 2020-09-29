using System;
using System.Text;
using System.IO;
using DNLang;
using DNLang.SyntaxAnalyzer.Model;
using System.Collections.Generic;
using Barbershop.IO;

namespace Barbershop {
    internal sealed class WorkFlow {
        public const string FILE_PATH = @"B:\programming\c_sharp\barbershop\data.dn";
        public List<WorkTime> workTimeList;
        public List<DayOfWeek> workDayOfWeekList;
        public ClientManagement clientManagement;
        public DataNotationModel dataNotationModel;

        private enum Actions : int {
            OPEN_CLIENT_LIST = 1,
            ADD_CLIENT = 2,
            SHOW_WORK_TIME = 3,
            EXIT_PROGRAM = 4
        };
        private bool mainLoopRunningFlag = true;

        private static void Main(string[] args) {
            var program = new WorkFlow();
            program.Start();
        }

        public WorkFlow() {
            workTimeList = new List<WorkTime>();
            workDayOfWeekList = new List<DayOfWeek>();
            clientManagement = new ClientManagement(this);
            try {
                dataNotationModel = ReadDNModelFromFile(FILE_PATH);
                FillWeekDays();
                FillWorkTime();
                FillClientsFromDNObject(dataNotationModel.GetNotationObject("clients"));
            } catch (Exception exception) { Console.WriteLine("Exception: {0}", exception.Message); }
        }

        public void Start() {
            if (!mainLoopRunningFlag) return;

            Console.WriteLine("Hi, what do you want?\n");
            while (mainLoopRunningFlag) {
                try {
                    Console.WriteLine("1. Open client list.\n2. Add new client.\n3. Show works time.\n4. Exit.\n");
                    Console.Write("> ");

                    int userAction = int.Parse(Console.ReadLine());

                    HandlerAction(userAction);

                    Console.WriteLine("\n-----------------\n");
                } catch (Exception exception) {
                    Console.WriteLine($"Error message: {exception.Message}\n");
                }
            }

            Console.ReadLine();
        }

        public void HandlerAction(int actionNumber) {
            switch (actionNumber) {
                case (int)Actions.OPEN_CLIENT_LIST:
                    Console.WriteLine("Printing clients...");

                    clientManagement
                        .clientList
                        .ForEach(curClient => {
                            Console.WriteLine(
                                "Client: \n\tTimeReception: {0};\n\tPastimes: {1}; \n\tPhone: {2}.",
                                curClient.receptionDate.ToString(),
                                curClient.pastimesInMinutes,
                                curClient.phoneNumber
                                );
                        });
                    break;

                case (int)Actions.ADD_CLIENT:
                    Console.WriteLine("Adding client...\n");

                    var month = UserIO.QueryNumber("Enter a month");
                    var day = UserIO.QueryNumber("Enter a day");
                    var timeString = UserIO.Query("Enter a time");
                    var interval = UserIO.QueryNumber("Enter a work time");
                    var clientPhone = UserIO.Query("Enter youre phone number");

                    var parsedTime = Helpers.ExtractTimeFromString(timeString);
                    var dateTime = new DateTime(DateTime.Now.Year, month, day, parsedTime.Hours, parsedTime.Minutes, 0);
                    var client = new Client(dateTime, interval, clientPhone);

                    clientManagement.AddClient(client);
                    break;

                case (int)Actions.SHOW_WORK_TIME:
                    Console.WriteLine("Work time:");
                    workTimeList
                        .ForEach(workTime => {
                            Console.WriteLine("\tStart time: {0},\n\tEnd time: {1};",
                                workTime.startTime.ToString(), workTime.endTime.ToString());
                        });
                    break;

                case (int)Actions.EXIT_PROGRAM:
                    mainLoopRunningFlag = false;
                    Console.WriteLine("Bye, bye!");
                    break;

                default:
                    Console.WriteLine("Error: Incorrect action number.");
                    break;
            }
        }

        public void FillWeekDays() {
            workDayOfWeekList.Add(DayOfWeek.Monday);
            workDayOfWeekList.Add(DayOfWeek.Thursday);
            workDayOfWeekList.Add(DayOfWeek.Wednesday);
            workDayOfWeekList.Add(DayOfWeek.Friday);
        }

        public void FillWorkTime() {
            var wtObject = dataNotationModel.GetNotationObject("worksTime");

            wtObject?.ForEach(collection => {
                if (!collection.HasProperties("startTime", "endTime")) throw new Exception("The works time object is't found");

                string startTimeString = collection.GetProperty("startTime").GetStringValue();
                string endTimeString = collection.GetProperty("endTime").GetStringValue();
                TimeSpan startTime = Helpers.ExtractTimeFromString(startTimeString);
                TimeSpan endTime = Helpers.ExtractTimeFromString(endTimeString);
                WorkTime workTime = new WorkTime(startTime, endTime);

                workTimeList.Add(workTime);
            });
        }

        public DataNotationModel ReadDNModelFromFile(string filePath) {
            var streamReader = new StreamReader(filePath);
            var buffer = streamReader.ReadToEnd();
            return Deserializer.ParseModel(buffer);
        }

        public void FillClientsFromDNObject(DataNotationObject dnObject) {
            if (dnObject == null) RaiseCriticalError("The clients object not found");

            dnObject?.ForEach(collection => {
                if (collection.HasProperties("day", "month", "time", "interval", "phone")) RaiseCriticalError("Invalid params of 'clients' object");

                var dayProperty = collection.GetProperty("day").GetIntValue();
                var monthProperty = collection.GetProperty("month").GetIntValue();
                var timeProperty = collection.GetProperty("time").GetStringValue();
                var intervalProperty = collection.GetProperty("interval").GetIntValue();
                var phoneProperty = collection.GetProperty("phone").GetStringValue();

                var time = Helpers.ExtractTimeFromString(timeProperty);
                var dateTime = new DateTime(
                    DateTime.Now.Year,
                    monthProperty, dayProperty,
                    time.Hours, time.Minutes, time.Seconds);

                var client = new Client(dateTime, intervalProperty, phoneProperty);

                clientManagement.AddClient(client);
            });
        }

        public void PrintClientsObject(DataNotationObject dnObject) {
            Console.WriteLine($"Object: {dnObject.name}");

            dnObject?.ForEach(collection => {
                Console.WriteLine("\t{0}", collection.GetProperty("day").GetStringValue());
                Console.WriteLine("\t{0}", collection.GetProperty("time").GetStringValue());
                Console.WriteLine("\t{0}", collection.GetProperty("interval").GetStringValue());
                Console.WriteLine("\t{0}", collection.GetProperty("phone").GetStringValue());
            });
        }

        public void RaiseCriticalError(string message) {
            // mainLoopRunningFlag = false;
            throw new Exception(message);
        }
    }
}