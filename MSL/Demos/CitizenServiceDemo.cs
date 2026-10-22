using MSL.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSL.Demos
{
    public static class CitizenServiceDemo
    {
       
        private static string GetCurrentUserLogin()
            => Environment.GetEnvironmentVariable("USERNAME") ?? "SYSTEM";

        public static void Run()
        {
            Console.Write("\nФИО Гражданина: ");
            string? fio = Console.ReadLine();
            Console.Write("Суть обращения: ");
            string? text = Console.ReadLine();
            Console.Write("Назначить сотрудника (ID): ");
            int empId;
            if (!int.TryParse(Console.ReadLine(), out empId))
            {
                Console.WriteLine("[ОШИБКА] Неверно указан ID сотрудника.");
                return;
            }

            using var connection = DatabaseContext.CreateConnection();

            // ИСПРАВЛЕНИЕ: Начало транзакции
            using var transaction = connection.BeginTransaction();

            try
            {
                // ИСПРАВЛЕНИЕ: Параметризация запроса
                using var cmdInsert = connection.CreateCommand();
                cmdInsert.Transaction = transaction;
                cmdInsert.CommandText =
                    @"INSERT INTO CitizenRequests 
                        (citizen_fio, request_text, assigned_employee_id, status) 
                      VALUES (@FIO, @TEXT, @EMP_ID, 'В работе')";

                cmdInsert.Parameters.AddWithValue("@FIO", fio);
                cmdInsert.Parameters.AddWithValue("@TEXT", text);
                cmdInsert.Parameters.AddWithValue("@EMP_ID", empId);
                cmdInsert.ExecuteNonQuery();

                // ИСПРАВЛЕНИЕ: Безопасная вставка и получение реального логина
                using var cmdAudit = connection.CreateCommand();
                cmdAudit.Transaction = transaction;
                cmdAudit.CommandText =
                    @"INSERT INTO AuditLog 
                        (action_time, user_login, action_description) 
                      VALUES (@TIME, @LOGIN, @DESC)";

                cmdAudit.Parameters.AddWithValue("@TIME", DateTime.UtcNow);
                cmdAudit.Parameters.AddWithValue("@LOGIN", GetCurrentUserLogin());
                cmdAudit.Parameters.AddWithValue(
                    "@DESC",
                    $"Создано обращение гражданина: {fio}" 
                );
                cmdAudit.ExecuteNonQuery();

               
                transaction.Commit();
                Console.WriteLine("[УСПЕХ] Обращение успешно зафиксировано.");
            }
            catch (Exception ex)
            {
              
                transaction.Rollback();
                Console.WriteLine($"[ОШИБКА] Не удалось зафиксировать обращение: {ex.Message}");
            }
        }
    }
}