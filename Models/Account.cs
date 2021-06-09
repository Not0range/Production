using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Production.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Цех
        /// </summary>
        [Required]
        public int Guild { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required, MaxLength(32)]
        public string Password { get; set; }

        public override string ToString()
        {
            if (Guild == 0)
                return "Администратор";
            return Guild.ToString();
        }
    }
}
