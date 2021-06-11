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
        public static int guild;
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

            listBox1.Items.Clear();
            listBox2.Items.Clear();
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
                bool b = true;
                if (guild != 0)
                    using (var db = new DatabaseContext())
                        b = db.PartOperations.All(i => i.Guild == guild);
                if (b)
                {
                    using (var db = new DatabaseContext())
                    {
                        db.Parts.Remove(db.Parts.FirstOrDefault(i => i.ID == ((Models.Part)listBox1.SelectedItem).ID));
                        db.SaveChanges();
                    }
                }
                else
                    MessageBox.Show("Невозможно удалить данную запись, так как она имеет отношения с другими цехами", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
