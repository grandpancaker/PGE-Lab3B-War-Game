using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PGELAB04a
{
    public partial class newGame : Form
    {
       public static bool allGood = false;
        public string _user { get; set; }
        public string _cpu { get; set; }
        public int _rounds { get; set; }
        public int _sets { get; set; }
        string user = "", cpu = "";
        int rounds = 10 , sets =2;
        public newGame()
        {
            InitializeComponent();
            textBox2.Text = "cpu";
            textBox3.Text = rounds.ToString();
            textBox4.Text = sets.ToString();
        }

        private void newGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.CloseGame();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ok_Click(object sender, EventArgs e)
        {
            
            if (!allGood)
            {
                allGood = true;
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                    user = textBox1.Text;
                else {
                    MessageBox.Show("there is no user", "wo ist user?!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    allGood = false;
                }

                if (!string.IsNullOrWhiteSpace(textBox2.Text))
                    cpu = textBox2.Text;
                else {
                    MessageBox.Show("there is no cpu", "wo ist cpu?!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    allGood = false;

                }

                if (!int.TryParse(textBox3.Text, out rounds))
                {

                    MessageBox.Show("there is no int", "wo ist int?!",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                    allGood = false;

                }
                else
                if (rounds <= 1)
                {
                    MessageBox.Show("round less then zero", "wo ist int?!",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                    allGood = false;
                }
                if (!int.TryParse(textBox4.Text, out sets))
                {

                    MessageBox.Show("there is no int", "wo ist int?!",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                    allGood = false;

                }
                else
                if (sets <= 1)
                    {
                    MessageBox.Show("sets less then zero", "wo ist int?!",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                    allGood = false;
                }
                    


                if (allGood)
                {
                    Hide();
                    _user = user;
                    _cpu = cpu;
                    _rounds = rounds;
                    _sets = sets;
                    allGood = false;
                }
            }
            else
            {
                Hide();
                _user = user;
                _cpu = cpu;
                _rounds = rounds;
                _sets = sets;
            }
                   
            
            
        }
    }
}
