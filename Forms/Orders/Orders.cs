using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production.Forms
{
    public partial class Orders : Form
    {
        public static int guild;
        private Func<Models.Order, bool> filter = i => true;
        bool filtered = false;
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
            Orders.guild = guild;

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
                    foreach (var order in db.Orders.Where(filter))
                    {
                        var row = new DataGridViewRow();
                        row.Cells.AddRange(
                            new DataGridViewTextBoxCell { Value = order.ID },
                            new DataGridViewTextBoxCell { Value = order.Number },
                            new DataGridViewTextBoxCell { Value = order.Title },
                            new DataGridViewTextBoxCell { Value = order.Characteristics },
                            new DataGridViewTextBoxCell { Value = order.Documentation },
                            new DataGridViewTextBoxCell { Value = order.OrderDate.ToString("dd.MM.yyyy") },
                            new DataGridViewTextBoxCell { Value = order.ShippingDate.ToString("dd.MM.yyyy") });
                        Invoke(new Action(() => dataGridView1.Rows.Add(row)));
                    }
                }
                Invoke(new Action(() => UseWaitCursor = false));
            });
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            if (dataGridView1.SelectedRows.Count == 0)
                return;
            int id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            using (var db = new DatabaseContext())
            {
                var p = db.Database.SqlQuery<Models.Part>("exec GetDetails @p0", id);
                listBox1.Items.AddRange(p.ToArray());
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
            if (MessageBox.Show("Вы уверены, что желаете удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (new AddEditPart(int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString())).ShowDialog() == DialogResult.OK)
                dataGridView1_SelectionChanged(null, null);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            if (new AddEditPart(int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()),
                ((Models.Part)listBox1.SelectedItem).ID).ShowDialog() == DialogResult.OK)
                dataGridView1_SelectionChanged(null, null);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            if (MessageBox.Show("Вы уверены, что желаете удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = ((Models.Part)listBox1.SelectedItem).ID;
                bool b = true;
                if (guild != 0)
                    using (var db = new DatabaseContext())
                        b = db.PartOperations.Where(i => i.PartID == id).All(i => i.Guild == guild);
                if (b)
                {
                    try
                    {
                        using (var db = new DatabaseContext())
                        {
                            db.Parts.Remove(db.Parts.FirstOrDefault(i => i.ID == id));
                            db.SaveChanges();
                        }
                        dataGridView1_SelectionChanged(null, null);
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        string msg = ex.InnerException.InnerException.Message;
                        MessageBox.Show(msg.Split('\n')[1], "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show("Невозможно удалить данную запись, так как она имеет отношения с другими цехами", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (new AddEditOrderOperation(int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString())).ShowDialog() == DialogResult.OK)
                dataGridView1_SelectionChanged(null, null);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;
            if (new AddEditOrderOperation(int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()),
                (Models.OrderOperation)listBox2.SelectedItem).ShowDialog() == DialogResult.OK)
                dataGridView1_SelectionChanged(null, null);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;

            if (MessageBox.Show("Вы уверены, что желаете удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (guild == 0 || ((Models.OrderOperation)listBox2.SelectedItem).Guild == guild)
                {
                    using (var db = new DatabaseContext())
                    {
                        db.OrderOperations.Remove(db.OrderOperations.FirstOrDefault(i => i.ID == ((Models.OrderOperation)listBox2.SelectedItem).ID));
                        db.SaveChanges();
                    }
                    dataGridView1_SelectionChanged(null, null);
                }
                else
                    MessageBox.Show("Невозможно удалить данную запись, так как она имеет отношения с другими цехами", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 3)
            {
                filtered = true;
                filter = i => i.Title.ToLower().Contains(textBox1.Text.ToLower());
            }
            else
                filter = i => true;
            if (filtered)
            {
                UpdateTable();
                if (textBox1.Text.Length <= 3)
                    filtered = false;
            }
        }
    }
}
