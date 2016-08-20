using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Drawing;

enum Pivot
{
    Center,
    TopLeft,
    Top,
    TopRight,
    Left,
    Right,
    BottomLeft,
    Bottom,
    BottomRight,
    Custom
}

namespace SpriteMaker
{
    class Sprite
    {
        public string name;

        public int x;
        public int y;
        public int width;
        public int height;

        public int left;
        public int right;
        public int top;
        public int bottom;

        public Pivot pivot;

        public int pixelsPerUnit;

        public Sprite(string name, int x, int y, int w, int h, Pivot p, int ppu, int l = 0, int r = 0, int t = 0, int b = 0)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.width = w;
            this.height = h;
            this.pivot = p;
            this.pixelsPerUnit = ppu;

            this.left = l;
            this.right = r;
            this.top = t;
            this.bottom = b;
        }

        public XmlWriter getXml(XmlWriter xw)
        {
            xw.WriteStartElement("Sprite");
                xw.WriteAttributeString("pixelPerUnit", pixelsPerUnit.ToString());
                xw.WriteAttributeString("h", height.ToString());
                xw.WriteAttributeString("w", width.ToString());
                xw.WriteAttributeString("y", y.ToString());
                xw.WriteAttributeString("x", x.ToString());
                xw.WriteAttributeString("name", name);
            xw.WriteEndElement();

            return xw;
        }

        public Rectangle getRect()
        {
            return new Rectangle(x + height, MainForm.instance.fixY(y), width, height);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
