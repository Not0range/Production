using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Production.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Номер заказа
        /// </summary>
        [Required, Index]
        public long Number { get; set; }

        /// <summary>
        /// Наименование продукции
        /// </summary>
        [Required, MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Характеристики
        /// </summary>
        [Required, MaxLength(300)]
        public string Characteristics { get; set; }

        /// <summary>
        /// Документация
        /// </summary>
        [Required, MaxLength(100)]
        public string Documentation { get; set; }

        /// <summary>
        /// Дата заказа
        /// </summary>
        [Column(TypeName = "date"), Required]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Дата отгрузки
        /// </summary>
        [Column(TypeName = "date"), Required]
        public DateTime ShippingDate { get; set; }

        /// <summary>
        /// Составные детали
        /// </summary>
        [InverseProperty("Order")]
        public List<Part> Parts { get; set; }

        /// <summary>
        /// Произведённые операции
        /// </summary>
        [InverseProperty("Order")]
        public List<OrderOperation> Operations { get; set; }
    }
}
