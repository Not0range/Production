using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production.Forms
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                try
                {
                    using (var db = new DatabaseContext())
                        Invoke(new Action(() =>
                        {
                            comboBox1.Items.AddRange(db.Accounts.ToArray());
                            UseWaitCursor = false;
                        }));
                }
                catch (Exception)
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, "Невозможно подключиться к SQL-серверу", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close();
                    }));
                    
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var builder = new StringBuilder();
                if (comboBox1.SelectedItem == null)
                    builder.AppendLine("Необходимо выбрать учётную запись");
                if (textBox1.Text == "")
                    builder.AppendLine("Необходимо ввести пароль");
                if (builder.Length > 0)
                    throw new Exception(builder.ToString());

                using (var db = new DatabaseContext())
                    if (db.Accounts.AsNoTracking().FirstOrDefault(i => i.Guild == ((Models.Account)comboBox1.SelectedItem).Guild &&
                         i.Password == textBox1.Text) == null)
                        throw new Exception("Указаны неверные логин и/или пароль");

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
