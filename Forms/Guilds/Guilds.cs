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
    public partial class Guilds : Form
    {
        public Guilds()
        {
            InitializeComponent();
            button4.Click += (s, ea) => Close();

            UpdateTable();
        }

        private void UpdateTable()
        {
            UseWaitCursor = true;
            dataGridView1.Rows.Clear();
            Task.Run(() =>
            {
                using (var db = new DatabaseContext())
                {
                    foreach (var account in db.Accounts)
                    {
                        var row = new DataGridViewRow();
                        row.Cells.AddRange(
                            new DataGridViewTextBoxCell { Value = account.ID },
                            new DataGridViewTextBoxCell { Value = account.Guild },
                            new DataGridViewTextBoxCell { Value = new string('*', account.Password.Length) });
                        Invoke(new Action(() => dataGridView1.Rows.Add(row)));
                    }
                }
                Invoke(new Action(() => UseWaitCursor = false));
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(new AddGuild().ShowDialog() == DialogResult.OK)
                UpdateTable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("Вы уверены, что желаете удалить выбранную учётную запись?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var db = new DatabaseContext())
                {
                    int id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    db.Accounts.Remove(db.Accounts.First(i => i.ID == id));
                    db.SaveChanges();
                }
                UpdateTable();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            if (new AddGuild(int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString())).ShowDialog() == DialogResult.OK)
                UpdateTable();
        }
    }
}
