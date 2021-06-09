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
    public partial class Orders : Form
    {
        int guild;
        public Orders(int guild)
        {
            InitializeComponent();
            button4.Click += (s, ea) => Close();
            if(guild != 0)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            this.guild = guild;

            UpdateTable();
        }

        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            UseWaitCursor = true;
            Task.Run(() =>
            {
                using (var db = new DatabaseContext())
                {
                    foreach (var order in db.Orders)
                    {
                        var row = new DataGridViewRow();
                        row.Cells.AddRange(
                            new DataGridViewTextBoxCell { Value = order.ID },
                            new DataGridViewTextBoxCell { Value = order.Number },
                            new DataGridViewTextBoxCell { Value = order.Title },
                            new DataGridViewTextBoxCell { Value = order.Characteristics },
                            new DataGridViewTextBoxCell { Value = order.Documentation },
                            new DataGridViewTextBoxCell { Value = order.OrderDate.ToString("dd.MM.yyyy") });
                        Invoke(new Action(() => dataGridView1.Rows.Add(row)));
                    }
                }
                Invoke(new Action(() => UseWaitCursor = false));
            });
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            int id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            using (var db = new DatabaseContext())
            {
                listBox1.Items.AddRange(db.Parts.Where(i => i.OrderID == id).ToArray());
                listBox2.Items.AddRange(db.OrderOperations.Where(i => i.OrderID == id).ToArray());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (new AddEditOrder().ShowDialog() == DialogResult.OK)
                UpdateTable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            if (new AddEditOrder(int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString())).ShowDialog() == DialogResult.OK)
                UpdateTable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("Вы уверены, что желаете удалить выбранную учётную запись?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var db = new DatabaseContext())
                {
                    int id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    db.Orders.Remove(db.Orders.First(i => i.ID == id));
                    db.SaveChanges();
                }
                UpdateTable();
            }
        }
    }
}
