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
    public partial class Menu : Form
    {
        int guild;
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            var form = new Auth();
            if (form.ShowDialog() != DialogResult.OK)
            {
                Close();
                return;
            }

            guild = ((Models.Account)form.comboBox1.SelectedItem).Guild;
            if (guild != 0)
            {
                Hide();
                var orders = new Orders(guild);
                orders.FormClosed += (s, ea) => Close();
                orders.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Orders(guild).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Guilds().Show();
        }
    }
}
