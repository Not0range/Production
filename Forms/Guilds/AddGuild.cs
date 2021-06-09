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
    public partial class AddGuild : Form
    {
        int id;
        public AddGuild(int id = -1)
        {
            InitializeComponent();
            if(id != -1)
            {
                button1.Visible = false;
                button3.Visible = true;
                Text = "Смена пароля";
                numericUpDown1.Value = id;
                numericUpDown1.Enabled = false;
            }
            this.id = id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                var builder = new StringBuilder();
                if (textBox1.Text == "")
                    builder.AppendLine("Пароль не должен быть пуст");
                if (textBox1.Text != textBox2.Text)
                    builder.AppendLine("Введённые пароли не совпадают");
                if (builder.Length > 0)
                    throw new Exception(builder.ToString());

                using (var db = new DatabaseContext())
                {
                    if (db.Accounts.AsNoTracking().Count(i => i.Guild == (int)numericUpDown1.Value) > 0)
                        throw new Exception("Учётная запись выбранного цеха уже существует");
                    var account = new Models.Account { Guild = (int)numericUpDown1.Value, Password = textBox1.Text };
                    db.Accounts.Add(account);
                    db.SaveChanges();
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UseWaitCursor = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                var builder = new StringBuilder();
                if (textBox1.Text == "")
                    builder.AppendLine("Пароль не должен быть пуст");
                if (textBox1.Text != textBox2.Text)
                    builder.AppendLine("Введённые пароли не совпадают");
                if (builder.Length > 0)
                    throw new Exception(builder.ToString());

                using (var db = new DatabaseContext())
                {
                    db.Accounts.First(i => i.Guild == id).Password = textBox1.Text;
                    db.SaveChanges();
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UseWaitCursor = false;
            }
        }
    }
}
