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
    public partial class AddEditOrder : Form
    {
        Models.Order order;

        public AddEditOrder(int id = -1)
        {
            InitializeComponent();

            using (var db = new DatabaseContext())
            {
                Models.Order order = db.Orders.FirstOrDefault(i => i.ID == id);
                if (order != null)
                {
                    Text = "Изменение";
                    button1.Visible = false;
                    button3.Visible = true;

                    numericUpDown1.Value = order.Number;
                    textBox1.Text = order.Title;
                    textBox2.Text = order.Characteristics;
                    textBox3.Text = order.Documentation;
                    dateTimePicker1.Value = order.OrderDate;
                }

                this.order = order;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                var builder = new StringBuilder();
                if (textBox1.Text == "")
                    builder.AppendLine("Поле 'Наименование' должно быть заполнено");
                if (textBox2.Text == "")
                    builder.AppendLine("Поле 'Характеристики' должно быть заполнено");
                if (textBox3.Text == "")
                    builder.AppendLine("Поле 'Документация' должно быть заполнено");
                if (builder.Length > 0)
                    throw new Exception(builder.ToString());

                using (var db = new DatabaseContext())
                {
                    
                    var order = new Models.Order 
                    { 
                        Number = (int)numericUpDown1.Value, 
                        Title = textBox1.Text,
                        Characteristics = textBox2.Text,
                        Documentation = textBox3.Text,
                        OrderDate = dateTimePicker1.Value
                    };
                    db.Orders.Add(order);
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
                    builder.AppendLine("Поле 'Наименование' должно быть заполнено");
                if (textBox2.Text == "")
                    builder.AppendLine("Поле 'Характеристики' должно быть заполнено");
                if (textBox3.Text == "")
                    builder.AppendLine("Поле 'Документация' должно быть заполнено");
                if (builder.Length > 0)
                    throw new Exception(builder.ToString());

                using (var db = new DatabaseContext())
                {
                    var order = db.Orders.First(i => i.ID == this.order.ID);
                    order.Number = (int)numericUpDown1.Value;
                    order.Title = textBox1.Text;
                    order.Characteristics = textBox2.Text;
                    order.Documentation = textBox3.Text;
                    order.OrderDate = dateTimePicker1.Value;
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
