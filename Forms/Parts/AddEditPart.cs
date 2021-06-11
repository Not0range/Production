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
    public partial class AddEditPart : Form
    {
        Models.Part part;

        int orderId;

        List<int> deletedOperation = new List<int>();
        public AddEditPart(int orderId, int id = -1)
        {
            InitializeComponent();
            this.orderId = orderId;
            using (var db = new DatabaseContext())
            {
                comboBox1.Items.AddRange(db.Parts.Where(i => i.OrderID == orderId && i.ID != id).ToArray());

                Models.Part part = db.Parts.FirstOrDefault(i => i.ID == id);
                if (part != null)
                {
                    Text = "Изменение";
                    button5.Visible = false;
                    button7.Visible = true;

                    textBox1.Text = part.Title;
                    numericUpDown1.Value = part.Count;
                    if (part.Details != null)
                    {
                        List<Models.Part> composite = new List<Models.Part>();
                        foreach (var n in part.Details)
                            composite.Add(db.Parts.First(i => i.ID == n));
                        listBox1.Items.AddRange(composite.ToArray());
                    }
                    
                    listBox2.Items.AddRange(db.PartOperations.Where(i => i.PartID == id).ToArray());
                }

                this.part = part;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;
            if (listBox1.Items.Contains(comboBox1.SelectedItem))
                MessageBox.Show("Выбранная деталь уже добавлена в список", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                listBox1.Items.Add(comboBox1.SelectedItem);
            comboBox1.SelectedItem = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new AddEditPartOperation(part != null ? part.ID : 0);
            if (form.ShowDialog() == DialogResult.OK)
                listBox2.Items.Add(form.operation);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;
            if (Orders.guild != 0 && ((Models.PartOperation)listBox2.SelectedItem).Guild != Orders.guild)
                MessageBox.Show("Выбранная операция относится к другому цеху", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (((Models.PartOperation)listBox2.SelectedItem).ID != 0)
                    deletedOperation.Add(((Models.PartOperation)listBox2.SelectedItem).ID);
                listBox2.Items.Remove(listBox2.SelectedItem);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Data.Entity.DbContextTransaction transaction = null;
            try
            {
                UseWaitCursor = true;
                var builder = new StringBuilder();
                if (textBox1.Text == "")
                    builder.AppendLine("Поле 'Наименование' должно быть заполнено");
                if (builder.Length > 0)
                    throw new Exception(builder.ToString());

                using (var db = new DatabaseContext())
                {
                    transaction = db.Database.BeginTransaction();

                    var part = new Models.Part
                    {
                        Title = textBox1.Text,
                        Count = (int)numericUpDown1.Value,
                        OrderID = orderId,
                        Parts = listBox1.Items.Count > 0 ? string.Join(" ", listBox1.Items.Cast<Models.Part>().Select(i => i.ID.ToString())) : null
                    };
                    db.Parts.Add(part);
                    db.SaveChanges();

                    foreach (var o in listBox2.Items.Cast<Models.PartOperation>())
                    {
                        o.PartID = part.ID;
                        db.PartOperations.Add(o);
                    }

                    db.SaveChanges();
                    transaction.Commit();
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                if (transaction != null && transaction.UnderlyingTransaction.Connection != null)
                    transaction.Rollback();
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UseWaitCursor = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Data.Entity.DbContextTransaction transaction = null;
            try
            {
                UseWaitCursor = true;
                var builder = new StringBuilder();
                if (textBox1.Text == "")
                    builder.AppendLine("Поле 'Наименование' должно быть заполнено");
                if (builder.Length > 0)
                    throw new Exception(builder.ToString());

                using (var db = new DatabaseContext())
                {
                    transaction = db.Database.BeginTransaction();

                    var part = db.Parts.First(i => i.ID == this.part.ID);

                    part.Title = textBox1.Text;
                    part.Count = (int)numericUpDown1.Value;
                    part.Parts = listBox1.Items.Count > 0 ? string.Join(" ", listBox1.Items.Cast<Models.Part>().Select(i => i.ID.ToString())) : null;

                    foreach (var o in deletedOperation)
                        db.PartOperations.Remove(db.PartOperations.First(i => i.ID == o));

                    foreach (var o in listBox2.Items.Cast<Models.PartOperation>())
                    {
                        bool newOperation = false;
                        var operation = db.PartOperations.FirstOrDefault(i => i.ID == o.ID);
                        if (operation == null)
                        {
                            operation = new Models.PartOperation();
                            newOperation = true;
                        }
                        operation.Title = o.Title;
                        operation.PartID = part.ID;
                        operation.Guild = o.Guild;
                        operation.Brigade = o.Brigade;
                        operation.WorkPlace = o.WorkPlace;
                        operation.Status = o.Status;
                        operation.DateTime = o.DateTime;
                        if (newOperation)
                            db.PartOperations.Add(operation);
                    }

                    db.SaveChanges();
                    transaction.Commit();
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UseWaitCursor = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;
            if (Orders.guild != 0 && ((Models.PartOperation)listBox2.SelectedItem).Guild != Orders.guild)
            {
                MessageBox.Show("Выбранная операция относится к другому цеху", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var form = new AddEditPartOperation(part != null ? part.ID : 0, listBox2.SelectedItem as Models.PartOperation);
            if (form.ShowDialog() == DialogResult.OK)
            {
                int ind = listBox2.SelectedIndex;
                listBox2.Items.RemoveAt(ind);
                listBox2.Items.Insert(ind, form.operation);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            if (new AddEditPart(orderId, ((Models.Part)listBox1.SelectedItem).ID).ShowDialog() == DialogResult.OK)
            {
                using (var db = new DatabaseContext())
                {
                    List<Models.Part> composite = new List<Models.Part>();
                    foreach (var n in part.Details)
                        composite.Add(db.Parts.First(i => i.ID == n));
                    listBox1.Items.AddRange(composite.ToArray());
                }
            }
        }
    }
}
