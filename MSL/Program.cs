using MSL.Data;
using MSL.Demos;

namespace MSL
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseInitializer.Initialize();

            while (true)
            {
                Console.WriteLine("\n=== АИС ВЕДОМСТВЕННОГО КОНТРОЛЯ И ДОКУМЕНТООБОРОТА ===");
                Console.WriteLine("1. Слой аутентификации (AuthenticationDemo)");
                Console.WriteLine("2. Слой поиска и грифования (DocumentSearchDemo)");
                Console.WriteLine("3. Слой сервиса обращений и аудита (CitizenServiceDemo)");
                Console.WriteLine("4. Получить текущую схему DDL для CASE-средств");
                Console.WriteLine("0. Выход");
                Console.Write("\nВыберите раздел для тестирования: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AuthenticationDemo.Run();
                        break;
                    case "2":
                        DocumentSearchDemo.Run();
                        break;
                    case "3":
                        CitizenServiceDemo.Run();
                        break;
                    case "4":
                        Console.WriteLine("\n" + DatabaseInitializer.GetDdl());
                        break;
                    case "0":
                        return;
                }
            }
        }
    }
}
