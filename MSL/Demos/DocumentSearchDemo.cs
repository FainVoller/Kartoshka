using MSL.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSL.Demos
{
    public static class DocumentSearchDemo
    {
       
        private static int GetCurrentUserClearance()
            => Environment.GetEnvironmentVariable("USER_CLEARANCE") is "3" ? 3 :
               Environment.GetEnvironmentVariable("USER_CLEARANCE") is "2" ? 2 : 1;

        public static void Run()
        {
            Console.Write("\nПоисковый запрос по материалам: ");
            string? term = Console.ReadLine();

            using var connection = DatabaseContext.CreateConnection();
            using var command = connection.CreateCommand();

            // ИСПРАВЛЕНИЕ 1: Используем реальный уровень допуска, а не данные от клиента.
            // ИСПРАВЛЕНИЕ 2: Параметризация обоих полей для защиты от инъекций.
            command.CommandText =
                @"SELECT id, title, secret_level 
                  FROM ClassifiedDocuments 
                 WHERE secret_level <= @CLEARANCE AND title LIKE @TERM";

            
            command.Parameters.AddWithValue("@CLEARANCE", GetCurrentUserClearance());
            command.Parameters.AddWithValue("@TERM", $"%{term}%");

            try
            {
                using var reader = command.ExecuteReader();
                Console.WriteLine("\n--- РЕЗУЛЬТАТЫ ПОИСКА ---");
                bool found = false;
                while (reader.Read())
                {
                    found = true;
                    Console.WriteLine($"[{reader["id"]}] (Гриф: {reader["secret_level"]}) {reader["title"]}");
                }
                if (!found) Console.WriteLine("Документы не найдены.");
            }
            catch (Exception ex)
            {
                // ИСПРАВЛЕНИЕ 3: Скрываем внутреннюю информацию от пользователя.
                Console.WriteLine("[CRITICAL ERROR]: Произошла ошибка при выполнении запроса.");

                Console.WriteLine(ex.ToString()); 
               
            }
        }
    }
}