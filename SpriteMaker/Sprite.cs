using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

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
        string name;

        int x;
        int y;
        int width;
        int height;

        int left;
        int right;
        int top;
        int bottom;

        Pivot pivot;

        int pixelsPerUnit;

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

        public override string ToString()
        {
            return name;
        }
    }
}
