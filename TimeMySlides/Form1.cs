using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Utilities;
using System.IO;

namespace TimeMySlides
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Object used to hook the 'B' key
        /// </summary>
        private globalKeyboardHook gkh;
        
        /// <summary>
        /// Stopwatch
        /// </summary>
        private Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Time offset for training
        /// </summary>
        private TimeSpan offset;

        /// <summary>
        /// Running state of the stopwatch
        /// </summary>
        private bool isRunning = false;

        /// <summary>
        /// List of subtitles
        /// </summary>
        private List<SubTitle> subTitles = new List<SubTitle>();

        /// <summary>
        /// Start time
        /// </summary>
        private string StartTime = "";

        /// <summary>
        /// Represent the xml options deserialized
        /// </summary>
        private Options options;

        /// <summary>
        /// Initialise component and Form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Start button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Start timer if not running
            if (!isRunning)
            {
                timer1.Enabled = true;
                stopwatch.Start();
                btnStart.Text = "Stop";
            }
            // Stop timer if running
            else
            {
                stopwatch.Stop();
                timer1.Enabled = false;
                btnStart.Text = "Start";
            }

            isRunning = !isRunning;

        }

        /// <summary>
        /// Timer tick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed + offset.Duration();
            String ElapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            lblTime.Text = ElapsedTime;

            var nearest = findNearestSubtitle(ts);

            lblSubTitle.Text = nearest.Text;

            switch (nearest.Color.ToLower())
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

        /// <summary>
        /// Reset button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                TimeSpan ts = stopwatch.Elapsed + offset.Duration();
                String ElapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                lblTime.Text = ElapsedTime;
            }

            lblSubTitle.Text = "";
            lblSubTitle.ForeColor = Color.White;
            lblTime.ForeColor = Color.White;
        }

        /// <summary>
        /// Form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Hook the 'B' key to start/pause the timer (this key is used by Microsft powerpoint for black screen and is used every commons wireless presenter)
            gkh = new globalKeyboardHook();
            gkh.HookedKeys.Add(Keys.B);
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);

            // Deserialize XML file
            loadXML();

            //Set the size of the window with xml options
            this.Size = new System.Drawing.Size(options.ScreenXSize, options.ScreenYSize);

            // Rebuild list with warning times subtitles
            buildWarningSubtitles();
        }

        /// <summary>
        /// Bind 'B' key to the start button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Down\t" + e.KeyCode.ToString());

            if (e.KeyCode.ToString() == "B")
            {
                btnStart.PerformClick();
            }
            e.Handled = true;
        }

        /// <summary>
        /// Rebuild subtitles list taking into account warning subtitles
        /// </summary>
        void buildWarningSubtitles()
        {
            List<SubTitle> newSubTitles = new List<SubTitle>();
            for (int i = 0; i < subTitles.Count; i++)
            {
                newSubTitles.Add(new SubTitle(subTitles[i]));
                if (subTitles[i].WarningTime != null && i < subTitles.Count - 1)
                {
                    var offset = TimeSpan.Parse(subTitles[i].WarningTime);
                    var endTime = TimeSpan.Parse(subTitles[i + 1].Time);
                    var calc = endTime.Duration() - offset.Duration();
                    SubTitle warning = new SubTitle(subTitles[i]);
                    warning.Color = warning.WarningColor;
                    warning.Time = String.Format("{0:00}:{1:00}:{2:00}", calc.Hours, calc.Minutes, calc.Seconds);
                    newSubTitles.Add(new SubTitle(warning));
                }

            }

            subTitles = new List<SubTitle>(newSubTitles);
        }

        /// <summary>
        /// Compare the parameter time to subtitles one by one.
        /// </summary>
        /// <param name="actualTime">Stopwatch actuel timer to compare w</param>
        /// <returns>Return the nearest subtitle</returns>
        SubTitle findNearestSubtitle (TimeSpan actualTime)
        {
            SubTitle nearest = new SubTitle();

            foreach (SubTitle subTitle in subTitles)
            {
                var time = TimeSpan.Parse(subTitle.Time);
                int result = TimeSpan.Compare(actualTime, time);
                // if actual time is greater or equal to the subtitle time, the subtitle is actually  the nearest.
                if (result == 1 || result == 0)
                {
                    nearest = subTitle;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Deserialize the xml file into an "options" object
        /// </summary>
        void loadXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("TimeMySlides.xml");
            string xmlText = xmlDoc.OuterXml;

            XmlSerializer serializer = new XmlSerializer(typeof(Options));
            using (var reader = new StringReader(xmlText))
            {
                options = (Options)serializer.Deserialize(reader);

                this.subTitles = options.subTitles;
                this.StartTime = options.StartTime;
                this.offset = TimeSpan.Parse(StartTime);
                TimeSpan ts = offset.Duration();
                String ElapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                lblTime.Text = ElapsedTime;
            }
        }

    }
}
