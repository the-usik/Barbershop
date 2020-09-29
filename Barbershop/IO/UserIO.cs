using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbershop.IO {
    class UserIO {
        public static int QueryNumber(string message) {
            int data;
            try {
                data = int.Parse(Query(message));
            } catch {
                data = QueryNumber(message);
            }

            return data;
        }

        public static string Query(string message) {
            Console.Write($"{message}: ");
            return Console.ReadLine();
        }
    }
}
