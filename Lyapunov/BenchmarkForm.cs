using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lyapunov
{
    public partial class BenchmarkForm : Form
    {
        LyapunovGenerator mylyap;
        int todo;
        Configuration conf;

        public BenchmarkForm()
        {
            InitializeComponent();
            mylyap = new LyapunovGenerator();
            mylyap.Completed += mylyap_PicCompleted;
        }

        private void run_btn_Click(object sender, EventArgs e)
        {
            if (run_btn.Text == "Start")
            {
                todo = int.Parse(numruns_txt.Text);
                if (todo < 1)
                {
                    todo = 1;
                    numruns_txt.Text = "1";
                }
                int iter = int.Parse(iter_txt.Text);
                conf = new Configuration(2, 4, 2, 4, new char[] { 'a', 'b' }, iter, 0.5, 256, 256);
                mylyap.Initialise(conf);
                mylyap.Generate();
                run_btn.Text = "Stop";
            }
            else
            {
                mylyap.Stop();
                run_btn.Text = "Start";
            }
        }

        void mylyap_PicCompleted(object src, EventArgs e)
        {
            if (todo > 0)
            {
                todo--;
                numruns_txt.Text = todo.ToString();
                result_lst.Items.Add(mylyap.Duration.TotalMilliseconds.ToString());
                mylyap.Generate();
            }
            else
            {
                run_btn.Text = "Start";
            }
            calcAverage();
        }

        private void clear_btn_Click(object sender, EventArgs e)
        {
            result_lst.Items.Clear();
            numruns_txt.Text = "10";
        }

        private void calcAverage()
        {
            double sum = 0;
            foreach (ListViewItem Item in result_lst.Items)
            {
                double i = double.Parse(Item.Text);
                sum += i;
            }
            double avg = sum / (double)result_lst.Items.Count;
            status_lbl.Text = avg.ToString()+ " milliseconds";
        }
    }
}
