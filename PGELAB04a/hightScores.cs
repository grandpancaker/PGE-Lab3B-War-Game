using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PGELAB04a
{
    public partial class hightScores : Form
    {
        public hightScores()
        {
            InitializeComponent();
            GetFroFile();
        }

        void GetFroFile()
        {
            string[] words = new string[3];
            foreach (string line in File.ReadLines(@"scores.hights", Encoding.UTF8))
            {
                words = line.Split(';');
                listView1.Items.Add(new ListViewItem(words));
            }
            listView1.ListViewItemSorter = new ListViewItemComparer(1);
        }
    }
    class ListViewItemComparer : System.Collections.IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            return Int32.Parse(((ListViewItem)x).SubItems[col].Text ).CompareTo(Int32.Parse(((ListViewItem)y).SubItems[col].Text))*(-1);
        }
    }
}
