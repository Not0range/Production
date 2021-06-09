using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

namespace Production
{
    static class Program
    {
        public static string Address { get; private set; }

        public static string Instance { get; private set; }

        public static int? Port { get; private set; }

        public static string UserID { get; private set; }

        public static string Password { get; private set; }

        private static string _connectionString;
        public static string ConnectionString 
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(_connectionString))
                    return _connectionString;

                StreamReader reader = new StreamReader("config.ini");
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.StartsWith("#"))
                        continue;
                    string[] strs = line.Split('=');
                    switch (strs[0].ToLower())
                    {
                        case "address":
                            Address = strs[1];
                            break;
                        case "instance":
                            Instance = strs[1];
                            break;
                        case "port":
                            int p;
                            if (int.TryParse(strs[1], out p))
                                Port = p;
                            break;
                        case "userid":
                            UserID = strs[1];
                            break;
                        case "password":
                            Password = strs[1];
                            break;
                    }
                }
                reader.Close();

                StringBuilder builder = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(Address))
                    builder.Append(String.Format("Server={0}", Address));
                else
                    builder.Append("Server=.");

                if(!string.IsNullOrWhiteSpace(Instance))
                    builder.Append(String.Format("\\{0}", Instance));

                if(Port.HasValue)
                    builder.Append(String.Format(",{0}", Port.Value));

                builder.Append("; Database=Production; ");

                if (!string.IsNullOrWhiteSpace(UserID) && !string.IsNullOrWhiteSpace(Password))
                    builder.Append(String.Format("User Id={0}; Password={1}; ", UserID, Password));
                else
                    builder.Append("Trusted_Connection=True;");

                _connectionString = builder.ToString();
                return builder.ToString();
            }
        }

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.Menu());
        }
    }
}
