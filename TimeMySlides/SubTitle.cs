using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TimeMySlides
{
    /// <summary>
    /// Class used to store the xml options
    /// </summary>
    [XmlRoot("Options")]
    public class Options
    {

        [XmlElement("ScreenXSize")]
        public int ScreenXSize { get; set; }

        [XmlElement("ScreenYSize")]
        public int ScreenYSize { get; set; }


        [XmlElement("StartTime")]
        public string StartTime { get; set; }

        [XmlArray("SubTitles")]
        [XmlArrayItem("SubTitle")]
        public List<SubTitle> subTitles = new List<SubTitle>();
    }

    /// <summary>
    /// Class used to store all the subtitles
    /// </summary>
    [XmlRoot("SubTitles")]
    public class SubTitle
    {

        public SubTitle()
        {

        }

        public SubTitle(SubTitle sub)
        {
            this.Time = sub.Time;
            this.Text = sub.Text;
            this.Color = sub.Color;
            this.WarningColor = sub.WarningColor;
            this.WarningTime = sub.WarningTime;
        }

        [XmlElement("Time")]
        public String Time { get; set; }

        [XmlElement("Text")]
        public String Text { get; set; }

        [XmlElement("Color")]
        public String Color { get; set; }


        [XmlElement("WarningTime")]
        public String WarningTime { get; set; }

        [XmlElement("WarningColor")]
        public String WarningColor { get; set; }

    }
}
