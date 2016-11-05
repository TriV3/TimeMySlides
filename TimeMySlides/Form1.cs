using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Utilities;

namespace TimeMySlides
{
    public partial class Form1 : Form
    {
        globalKeyboardHook gkh;
        Stopwatch stopwatch = new Stopwatch();

        bool isRunning = false;
        List<SubTitle> subTitles = new List<SubTitle>();

        public Form1()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(1200, 600);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                timer1.Enabled = true;
                stopwatch.Start();
                btnStart.Text = "Stop";
            }
            else
            {
                stopwatch.Stop();
                timer1.Enabled = false;
                btnStart.Text = "Start";
            }

            isRunning = !isRunning;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed;
            //String ElapsedTime = String.Format("{:hh\\:mm\\:ss}", stopwatch.Elapsed);
            String ElapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            lblTime.Text = ElapsedTime;

            foreach (SubTitle subTitle in subTitles)
            {
                if (subTitle.Time == ElapsedTime)
                {
                    lblSubTitle.Text = subTitle.Text;
                    switch (subTitle.Color)
                    {
                        case "red":
                            lblTime.ForeColor = System.Drawing.Color.Red;
                            break;
                        case "orange":
                            lblTime.ForeColor = System.Drawing.Color.Orange;
                            break;
                        case "yellow":
                            lblTime.ForeColor = System.Drawing.Color.Yellow;
                            break;
                        case "blue":
                            lblTime.ForeColor = System.Drawing.Color.Blue;
                            break;
                        default:
                            lblTime.ForeColor = System.Drawing.Color.White;
                            break;
                    }
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if(stopwatch.IsRunning)
            {
                stopwatch.Reset();
                stopwatch.Start();
            }
            else
            {
                stopwatch.Reset();
                TimeSpan ts = stopwatch.Elapsed;
                String ElapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                lblTime.Text = ElapsedTime;
            }

            lblSubTitle.Text = "";
            lblSubTitle.ForeColor = Color.White;
            lblTime.ForeColor = Color.White;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gkh = new globalKeyboardHook();

            XmlSerializer serializer = new XmlSerializer(typeof(List<SubTitle>), new XmlRootAttribute("SubTitles"));
            XmlReader reader = XmlReader.Create("TimeMySlides.xml");
            subTitles = (List<SubTitle>)serializer.Deserialize(reader);
            System.Diagnostics.Debug.WriteLine("Xml file loaded");

            gkh.HookedKeys.Add(Keys.B);
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Down\t" + e.KeyCode.ToString());

            if (e.KeyCode.ToString() == "B")
            {
                btnStart.PerformClick();
            }
            e.Handled = true;
        }
    }
}
