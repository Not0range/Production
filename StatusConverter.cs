using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production
{
    public static class StatusConverter
    {
        public static string Convert(this Models.Status status)
        {
            switch(status)
            {
                case Models.Status.Wait:
                    return "Ожидает";
                case Models.Status.InWork:
                    return "В работе";
                case Models.Status.Completed:
                    return "Завершено";
            }
            return "";
        }
    }
}
