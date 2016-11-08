using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TimeMySlides
{

    [XmlRoot("Options")]
    public class Options
    {
        [XmlElement("StartTime")]
        public string StartTime { get; set; }

        [XmlArray("SubTitles")]
        [XmlArrayItem("SubTitle")]
        public List<SubTitle> subTitles = new List<SubTitle>();
    }

    [XmlRoot("SubTitles")]
    public class SubTitle
    {
        [XmlElement("Time")]
        public String Time { get; set; }

        [XmlElement("Text")]
        public String Text { get; set; }

        [XmlElement("Color")]
        public String Color { get; set; }

    }
}
