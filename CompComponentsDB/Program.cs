using CompComponentsDB.Data;
using CompComponentsDB.Service;
using System;

namespace CompComponentsDB
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "CCWarehouse";

            if (!DbConnector.TestConnection())
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            var service = new ComponentService();
            var menu = new MenuManager(service);
            menu.Run();
        }
    }
}