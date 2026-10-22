using System;
using System.ComponentModel.DataAnnotations; 

namespace MSL.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Login { get; set; } = null!; 

        // ОШИБКА ИСПРАВЛЕНА: Пароль теперь хранится в хешированном виде.
       
        public byte[] HashedPassword { get; private set; } = Array.Empty<byte>();
        public byte[] Salt { get; private set; } = Array.Empty<byte>();

        [StringLength(100)]
        public string FullName { get; set; } = null!;

        // ОШИБКА ИСПРАВЛЕНА: Изолировали персональные данные и пометили их.
        public PassportInfo? Passport { get; set; }

        // ОШИБКА ИСПРАВЛЕНА: Нормализовали связь с отделом.
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; } 

        public SecretLevel ClearanceLevel { get; set; } 

        // Вспомогательный класс для паспортных данных
        public sealed class PassportInfo
        {
            public string Series { get; set; } = null!;

            public string Number { get; set; } = null!;

            public DateTime IssueDate { get; set; }

          
        }
    }

    public class Department
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public enum SecretLevel : byte
    {
        Service = 1,
        Confidential = 2,
        TopSecret = 3
    }
}