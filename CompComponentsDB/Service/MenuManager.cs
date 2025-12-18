using CompComponentsDB.Models;
using CompComponentsDB.Service;
using System;
using System.Collections.Generic;

namespace CompComponentsDB
{
    public class MenuManager
    {
        private readonly ComponentService _service;

        public MenuManager(ComponentService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                PrintMenu();

                var choice = Console.ReadLine()?.Trim();

                Console.Clear();

                switch (choice)
                {
                    case "1": AddComponent(); break;
                    case "2": ShowAll(); break;
                    case "3": UpdateQuantity(); break;
                    case "4": DeleteComponent(); break;
                    case "5": ShowAllNames(); break;
                    case "6": ShowAllSuppliers(); break;
                    case "7": ShowMaxCost(); break;
                    case "8": ShowMinCost(); break;
                    case "9": ShowAverageCost(); break;
                    case "10": ShowCountByType(); break;
                    case "11": ShowCountBySupplier(); break;
                    case "12": ShowCountBySpecificSupplier(); break;
                    case "13": ShowBelowCost(); break;
                    case "14": ShowAboveCost(); break;
                    case "15": ShowCostRange(); break;
                    case "16": ShowIntelOrAMD(); break;
                    case "17": Exit(); return;
                    default: Error("Invalid choice"); break;
                }
            }
        }

        private void PrintMenu()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("===== CСWAREHOUSE =====");
            Console.ResetColor();
            Console.WriteLine("1  - Add component");
            Console.WriteLine("2  - Show all components");
            Console.WriteLine("3  - Update quantity");
            Console.WriteLine("4  - Delete component");
            Console.WriteLine("5  - Show all names");
            Console.WriteLine("6  - Show all suppliers");
            Console.WriteLine("7  - Show max cost");
            Console.WriteLine("8  - Show min cost");
            Console.WriteLine("9  - Show average cost");
            Console.WriteLine("10 - Count by type");
            Console.WriteLine("11 - Count by each supplier");
            Console.WriteLine("12 - Count by specific supplier");
            Console.WriteLine("13 - Components cheaper than...");
            Console.WriteLine("14 - Components more expensive than...");
            Console.WriteLine("15 - Components in cost range");
            Console.WriteLine("16 - Components from Intel or AMD");
            Console.WriteLine("17 - Exit");
            Console.Write("\nYour choice: ");
        }

        private void AddComponent()
        {
            var c = new Component
            {
                Name = ReadString("Name: "),
                Type = ReadString("Type: "),
                Supplier = ReadString("Supplier: "),
                Quantity = ReadInt("Quantity: "),
                Cost = ReadDecimal("Cost: "),
                SupplyDate = ReadString("Supply date (yyyy-mm-dd): ")
            };
            _service.Add(c);
            Success("Component added");
        }

        private void ShowAll()
        {
            Print(_service.GetAll(), "All components");
        }

        private void UpdateQuantity()
        {
            _service.UpdateQuantity(ReadInt("Component ID: "), ReadInt("New quantity: "));
            Success("Quantity updated");
        }

        private void DeleteComponent()
        {
            _service.Delete(ReadInt("Component ID: "));
            Success("Component deleted");
        }

        private void ShowAllNames()
        {
            foreach (var n in _service.GetAllNames()) Console.WriteLine(n);
            Pause();
        }

        private void ShowAllSuppliers()
        {
            foreach (var s in _service.GetAllSuppliers()) Console.WriteLine(s);
            Pause();
        }

        private void ShowMaxCost()
        {
            Console.WriteLine(_service.GetMaxCost().ToString("F2"));
            Pause();
        }

        private void ShowMinCost()
        {
            Console.WriteLine(_service.GetMinCost().ToString("F2"));
            Pause();
        }

        private void ShowAverageCost()
        {
            Console.WriteLine(_service.GetAverageCost().ToString("F2"));
            Pause();
        }

        private void ShowCountByType()
        {
            Console.WriteLine(_service.GetCountByType(ReadString("Type: ")));
            Pause();
        }

        private void ShowCountBySupplier()
        {
            foreach (var kv in _service.GetCountBySupplier())
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            Pause();
        }

        private void ShowCountBySpecificSupplier()
        {
            Console.WriteLine(_service.GetCountBySupplier(ReadString("Supplier: ")));
            Pause();
        }

        private void ShowBelowCost()
        {
            var c = ReadDecimal("Max cost: ");
            Print(_service.GetComponentsBelowCost(c), $"Cheaper than {c:F2}");
        }

        private void ShowAboveCost()
        {
            var c = ReadDecimal("Min cost: ");
            Print(_service.GetComponentsAboveCost(c), $"More expensive than {c:F2}");
        }

        private void ShowCostRange()
        {
            var min = ReadDecimal("Min cost: ");
            var max = ReadDecimal("Max cost: ");
            Print(_service.GetComponentsInCostRange(min, max), $"{min:F2} - {max:F2}");
        }

        private void ShowIntelOrAMD()
        {
            Print(_service.GetComponentsBySuppliers(new List<string> { "Intel", "AMD" }),
                "Components from Intel or AMD");
        }

        private void Print(List<Component> list, string title)
        {
            Console.WriteLine(title);

            if (list.Count == 0)
            {
                Console.WriteLine("No data");
                Pause();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Id | Name                 | Type | Supplier | Qty | Cost   | Date");
            Console.WriteLine("------------------------------------------------------------------");
            Console.ResetColor();

            foreach (var c in list)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{c.Id,-3} | {c.Name,-20} | {c.Type,-4} | {c.Supplier,-8} | {c.Quantity,-3} | {c.Cost,6:F2} | {c.SupplyDate}");
                Console.ResetColor();
            }

            Pause();
        }

        private void Exit()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Bye bye");
            Console.ResetColor();
        }

        private void Success(string m)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m);
            Console.ResetColor();
            Pause();
        }

        private void Error(string m)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(m);
            Console.ResetColor();
            Pause();
        }

        private void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private int ReadInt(string m)
        {
            while (true)
            {
                Console.Write(m);
                if (int.TryParse(Console.ReadLine(), out var v)) return v;
            }
        }

        private decimal ReadDecimal(string m)
        {
            while (true)
            {
                Console.Write(m);
                if (decimal.TryParse(Console.ReadLine(), out var v)) return v;
            }
        }

        private string ReadString(string m)
        {
            while (true)
            {
                Console.Write(m);
                var v = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(v)) return v;
            }
        }
    }
}
