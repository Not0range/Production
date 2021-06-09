using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Production.Models
{
    [Table("PartOperations")]
    public class PartOperation
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// ID детали
        /// </summary>
        public int PartID { get; set; }

        /// <summary>
        /// Деталь
        /// </summary>
        [ForeignKey("PartID")]
        public Part Part { get; set; }

        /// <summary>
        /// Название операции
        /// </summary>
        [Required, MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Цех
        /// </summary>
        [Required]
        public int Guild { get; set; }

        /// <summary>
        /// Бригада
        /// </summary>
        [Required]
        public int Brigade { get; set; }

        /// <summary>
        /// Рабочее место
        /// </summary>
        [Required]
        public int WorkPlace { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [Required]
        public Status Status { get; set; }

        /// <summary>
        /// Дата и время операции
        /// </summary>
        [Required]
        public DateTime DateTime { get; set; }
    }

    public enum Status : int
    {
        Wait,
        InWork,
        Completed
    }
}