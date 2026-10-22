using System;
using System.Collections.Generic;
using System.Text;

namespace MSL.Data
{
    public static class DatabaseInitializer
    {
        // Этот DDL забирают в CASE-средства (dbdiagram.io, DataGrip, pgModeler)
        public static string GetDdl()
        {
            return @"
                -- Таблица сотрудников
                CREATE TABLE IF NOT EXISTS Employees (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    login TEXT NOT NULL,
                    password TEXT NOT NULL,
                    full_name TEXT NOT NULL,
                    passport_data TEXT NOT NULL,
                    department_name TEXT NOT NULL,
                    clearance_level INTEGER NOT NULL
                );

                -- Таблица документов
                -- АРХИТЕКТУРНАЯ ДЫРА: Нет внешнего ключа (FOREIGN KEY) на Employees(id)
                CREATE TABLE IF NOT EXISTS ClassifiedDocuments (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NOT NULL,
                    content TEXT NOT NULL,
                    secret_level INTEGER NOT NULL,
                    author_id INTEGER NOT NULL,
                    department_name TEXT NOT NULL
                );

                -- Таблица обращений
                -- АРХИТЕКТУРНАЯ ДЫРА: Нет связи с Employees, статус не ограничен CHECK
                CREATE TABLE IF NOT EXISTS CitizenRequests (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    citizen_fio TEXT NOT NULL,
                    request_text TEXT NOT NULL,
                    assigned_employee_id INTEGER,
                    status TEXT
                );

                -- Таблица журнала аудита
                CREATE TABLE IF NOT EXISTS AuditLog (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    action_time TEXT NOT NULL,
                    user_login TEXT NOT NULL,
                    action_description TEXT NOT NULL
                );
                ";
        }

        public static void Initialize()
        {
            using var connection = DatabaseContext.CreateConnection();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = GetDdl();
            cmd.ExecuteNonQuery();

            // Проверка на наполненность
            cmd.CommandText = "SELECT COUNT(*) FROM Employees";
            long count = (long)(cmd.ExecuteScalar() ?? 0);
            if (count > 0) return;

            // Наполнение данными с нарушениями целостности
            cmd.CommandText = @"
                INSERT INTO Employees (login, password, full_name, passport_data, department_name, clearance_level) VALUES
                ('minister', 'SuperMinister2026!', 'Воронов В.В.', '4510 998877', 'Департамент Специальной Связи', 3),
                ('analyst_petrov', 'petrovPass', 'Петров П.П.', '4512 112233', 'Аналитический Департамент', 2),
                ('clerk_sidorov', '123456', 'Сидоров С.С.', '4515 445566', 'Общий Отдел', 1);

                INSERT INTO ClassifiedDocuments (title, content, secret_level, author_id, department_name) VALUES
                ('Спецификация квантовой криптографии', 'Секретные частоты и узлы: 144.800 MHz...', 3, 1, 'Департамент Специальной Связи'),
                ('Анализ угроз информбезопасности', 'Выявлены сетевые аномалии в подсети B...', 2, 2, 'Аналитический Департамент'),
                ('График дежурств охраны', 'Список ответственных за КПП...', 1, 3, 'Общий Отдел');

                -- 999 - несуществующий сотрудник! База молча проглотила это из-за отсутствия FK
                INSERT INTO CitizenRequests (citizen_fio, request_text, assigned_employee_id, status) VALUES
                ('Иванов И.И.', 'Запрос на выдачу сертификата ключа', 3, 'В работе'),
                ('Алексеев А.А.', 'Жалоба на задержку ответа', 999, 'Новое');
                ";
            cmd.ExecuteNonQuery();
        }
    }
}
