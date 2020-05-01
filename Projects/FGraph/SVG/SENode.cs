using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace FGraph
{
    [DebuggerDisplay("{AllText()}{Annotations()}")]
    public class SENode
    {
        public List<SEText> TextLines = new List<SEText>();
        public Color FillColor { get; set; } = Color.White;
        public String Class { get; set; }

        public float Width { get; set; } = 0;

        public String HRef { get; set; }


        /// <summary>
        /// Annotation on the line coming into the node (at line end);
        /// </summary>
        public String LhsAnnotation { get; set; }

        /// <summary>
        /// Annotation on the line leaving the node (at start of outgoing line);
        /// </summary>
        public String RhsAnnotation { get; set; }

        public String AllText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (SEText t in this.TextLines)
                sb.Append($"{t.Text} ");
            return sb.ToString();
        }

        public SENode AddTextLine(SEText text)
        {
            if (this.Width < text.Text.Length)
                this.Width = text.Text.Length;
            this.TextLines.Add(text);
            return this;
        }

        public SENode AddTextLine(String text, String hRef = null, String title = null)
        {
            return this.AddTextLine(new SEText(text, hRef, title));
        }
    }
}