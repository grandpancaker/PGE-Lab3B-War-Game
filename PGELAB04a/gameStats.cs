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
    public partial class gameStats : Form
    {
        
        public gameStats()
        {
            InitializeComponent();
            fillChart();
        }
       
        private void fillChart()
        {
            historyElement a;
            for (int i = 0; i < Form1.history.Count; i++)
            {
                 a = Form1.history[i];
                statsChart.Series[0].Name = Form1.userName;
                statsChart.Series[1].Name = Form1.cpuName;
                statsChart.ChartAreas[0].AxisX.Maximum = Form1.maxRound;
                statsChart.ChartAreas[0].AxisY.Maximum = 10*Form1.sets;

                statsChart.Series[0].Points.AddXY(i+1, a.userPoints);
                statsChart.Series[1].Points.AddXY(i+1, a.cpuPoints);
            }
            
        }
    }
}
