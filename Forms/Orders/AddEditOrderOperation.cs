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
    public partial class AddEditOrderOperation : Form
    {
        int orderId;
        Models.OrderOperation operation;
        public AddEditOrderOperation(int orderId, Models.OrderOperation operation = null)
        {
            InitializeComponent();

            this.orderId = orderId;
            using (var db = new DatabaseContext())
            {
                if (Orders.guild != 0)
                {
                    numericUpDown1.Value = Orders.guild;
                    numericUpDown1.Enabled = false;
                }
                if (operation != null)
                {
                    Text = "Изменение";
                    button1.Visible = false;
                    button3.Visible = true;

                    textBox1.Text = operation.Title;
                    numericUpDown1.Value = operation.Guild;
                    numericUpDown2.Value = operation.Brigade;
                    numericUpDown3.Value = operation.WorkPlace;
                    comboBox1.SelectedIndex = (int)operation.Status;
                    dateTimePicker1.Value = operation.DateTime;
                }

                this.operation = operation;
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
                if (comboBox1.SelectedItem == null)
                    builder.AppendLine("Поле 'Статус' должно быть заполнено");
                if (builder.Length > 0)
                    throw new Exception(builder.ToString());
                using (var db = new DatabaseContext())
                {
                    operation = new Models.OrderOperation
                    {
                        OrderID = orderId,
                        Title = textBox1.Text,
                        Guild = (int)numericUpDown1.Value,
                        Brigade = (int)numericUpDown2.Value,
                        WorkPlace = (int)numericUpDown3.Value,
                        Status = (Models.Status)comboBox1.SelectedIndex,
                        DateTime = dateTimePicker1.Value
                    };
                    db.OrderOperations.Add(operation);
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
                if (comboBox1.SelectedItem == null)
                    builder.AppendLine("Поле 'Статус' должно быть заполнено");
                if (builder.Length > 0)
                    throw new Exception(builder.ToString());
                using (var db = new DatabaseContext())
                {
                    operation = db.OrderOperations.First(i => i.ID == operation.ID);
                    operation.OrderID = orderId;
                    operation.Title = textBox1.Text;
                    operation.Guild = (int)numericUpDown1.Value;
                    operation.Brigade = (int)numericUpDown2.Value;
                    operation.WorkPlace = (int)numericUpDown3.Value;
                    operation.Status = (Models.Status)comboBox1.SelectedIndex;
                    operation.DateTime = dateTimePicker1.Value;
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
