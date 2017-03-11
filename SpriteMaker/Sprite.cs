using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

public enum Pivot
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
    public class Sprite
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

        Rectangle rect;

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
            this.rect = new Rectangle(x, y, w, h);

            MouseControler.getInstance().pressed += (o, e) => { if(isInside(e.Location)){ MainForm.instance.setSelected(this); } };

        }

        public static int fixY(int y, int height)
        {
            return Math.Abs(y - height);
        }

        static bool isInside(Point p, Rectangle rect)
        {
            return p.X > rect.X && p.X < rect.X + rect.Width
                && p.Y > rect.Y && p.Y < rect.Y + rect.Height;
        }

        public XmlWriter getXml(XmlWriter xw)
        {
            //TODO: Pivot? l? r? t? b?
            xw.WriteStartElement("Sprite");
            xw.WriteAttributeString("pixelPerUnit", this.pixelsPerUnit.ToString());
            xw.WriteAttributeString("h", this.height.ToString());
            xw.WriteAttributeString("w", this.width.ToString());
            xw.WriteAttributeString("y", this.y.ToString());
            xw.WriteAttributeString("x", this.x.ToString());
            xw.WriteAttributeString("name", this.name);
            xw.WriteEndElement();

            return xw;
        }

        public Rectangle getRect()
        {
            return rect;
        }

        Rectangle CopyRect(Rectangle rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void render(Graphics g, Pen p, int height, bool selected)
        {
            Rectangle temp = CopyRect(rect);
            temp.Y = fixY(temp.Y, height) - temp.Height;

            g.DrawRectangle(p, temp);

            if(selected)
            {
                temp.Height = (int)(temp.Height * 0.1);
                temp.Y += (rect.Height - temp.Height);
                g.DrawRectangle(Pens.Red, temp);

                temp.Y += (temp.Height - rect.Height);
                g.DrawRectangle(Pens.Green, temp);

                temp.Height = rect.Height;
                temp.Width = (int)(temp.Width * 0.1);
                g.DrawRectangle(Pens.Blue, temp);

                temp.X += (rect.Width - temp.Width);
                g.DrawRectangle(Pens.Yellow, temp);
            }
        }

        bool isInside(Point p)
        {
            return p.X > this.x && p.X < this.x + this.width 
                && p.Y > this.y && p.Y < this.y + this.height;
        }

       

        void select()
        {
            //TODO: Setup callbacs for changeing size
            //MouseControler.getInstance().pressed += (o, e) => { if(isInside(e.Location){ doSomething(); } };
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
