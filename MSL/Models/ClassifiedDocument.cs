using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MSL.Models
{
    public class ClassifiedDocument
    {
        public int Id { get; set; }

        [StringLength(256)]
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        // ИСПРАВЛЕНИЕ ОШИБКИ 3: Строгий тип вместо примитивного числа
        public SecretLevelEnum SecretLevel { get; set; }

        // ИСПРАВЛЕНИЕ ОШИБКИ 2: Обязательность внешнего ключа
        [Required]
        public int AuthorId { get; set; }

        // ИСПРАВЛЕНИЕ ОШИБКИ 1: Навигационное свойство
        public virtual Employee Author { get; set; } // Связь с автором

        public string DepartmentName { get; set; } = null!;
    }

    public enum SecretLevelEnum : byte 
    {
        Service = 1,
        Confidential = 2,
        TopSecret = 3
    }
}