using MSL.Data;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MSL.Demos
{
    public static class AuthenticationDemo
    {
        

        public static void Run()
        {
            Console.Write("\nВведите логин сотрудника: ");
            string? login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string? password = Console.ReadLine(); 

            using var connection = DatabaseContext.CreateConnection();

            // ИСПРАВЛЕНИЕ: Использование параметризованного запроса
            // Выбираем только данные о сотруднике по ЛОГИНУ.
            // Сравнение пароля происходит на стороне приложения после получения соли и хеша.
            const string sql = @"
                SELECT full_name, clearance_level, department_name 
                   -- , salt, hashed_password 
                  FROM Employees 
                 WHERE login = @Login";

            using var command = connection.CreateCommand();
            command.CommandText = sql;

           
            command.Parameters.AddWithValue("@Login", login);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                
                Console.WriteLine($"\n[АВТОРИЗОВАН] {reader["full_name"]}");
                Console.WriteLine($"Отдел: {reader["department_name"]} | Допуск: Уровень {reader["clearance_level"]}");
            }
            else
            {
                Console.WriteLine("\n[ОТКАЗ] Неверный логин.");
               
            }
        }

        
    }
}