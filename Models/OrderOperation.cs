using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Production.Models
{
    [Table("OrderOperations")]
    public class OrderOperation
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// ID заказа
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// Заказ
        /// </summary>
        [ForeignKey("OrderID")]
        public Order Order { get; set; }

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

        public override string ToString()
        {
            return string.Format("{0} - {1}/{2}/{3} - {4} - {5}", Title, Guild, Brigade, WorkPlace, DateTime.ToString("dd.MM.yy"), Status.Convert());
        }
    }
}
