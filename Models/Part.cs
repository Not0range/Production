using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Production.Models
{
    [Table("Parts")]
    public class Part
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
        /// Наименование детали
        /// </summary>
        [Required, MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Количество деталей
        /// </summary>
        [Required]
        public int Count { get; set; }

        /// <summary>
        /// Составные детали
        /// </summary>
        [RegularExpression("([0-9]+ )+")]
        public string Parts { get; set; }

        /// <summary>
        /// Произведённые операции
        /// </summary>
        [InverseProperty("Part")]
        public List<PartOperation> Operations { get; set; }

        /// <summary>
        /// Преобразование списка составных деталей в числовой массив
        /// </summary>
        [NotMapped]
        public int[] Details
        {
            get
            {
                return Parts.Split(' ').Select(s => int.Parse(s)).ToArray();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} шт.", Title, Count);
        }
    }
}
