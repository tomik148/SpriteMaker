using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            //MouseControler.getInstance().pressed += (o, e) => { if(isInside(e.Location)){ MainForm.instance.setSelected(this); } };

        }

        public static int fixY(int y)
        {
            int height = MainForm.instance.height;
            return Math.Abs(y - height);
        }

        public static bool isInside(Point p, Rectangle rect)
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

        public void render(Graphics g, Pen p, bool selected)
        {
            Rectangle temp = CopyRect(rect);
            temp.Y = fixY(temp.Y) - temp.Height;

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

        public bool isInside(Point p)
        {
            Rectangle temp = CopyRect(rect);
            temp.Y = fixY(temp.Y) - temp.Height;
            return temp.Contains(p);
            //return p.X > this.x && p.X < this.x + this.width 
            //    && p.Y > this.y && (fixY(p.Y,MainForm.instance.height) - this.height) < this.y + this.height;
        }

       
        public void update(Sprite s)
        {
            this.name = s.name;
            this.x = s.x;
            this.y = s.y;
            this.width = s.width;
            this.height = s.height;
            this.pivot = s.pivot;
            this.pixelsPerUnit = s.pixelsPerUnit;

            this.left = s.left;
            this.right = s.right;
            this.top = s.top;
            this.bottom = s.bottom;
            this.rect = s.rect;
        }
        public void select()
        {
            //TODO: Setup callbacs for changeing size
            //MouseControler.getInstance().pressed += (o, e) => { if(isInside(e.Location){ doSomething(); } };

            MouseControler.getInstance().hover -= changeCursor;
            MouseControler.getInstance().hover += changeCursor2;
        }

        public void unSelect()
        {
            //TODO: Setup callbacs for changeing size
            //MouseControler.getInstance().pressed -= (o, e) => { if(isInside(e.Location){ doSomething(); } };
            MouseControler.getInstance().hover += changeCursor;
            MouseControler.getInstance().hover -= changeCursor2;
        }

        private void changeCursor(object o, MouseEventArgs e)
        {
            if(isInside(e.Location) && Cursor.Current != Cursors.Hand)
            {
                Cursor.Current = Cursors.Hand;
                
            }
            if(isInside(e.Location))
            {
                if(MouseControler.getInstance().getSubState() != MouseControler.SubState.Selecting)
                {
                    MouseControler.getInstance().setSubState(MouseControler.SubState.Selecting);
                    //TODO: make it better if there is more sprites 
                }
            }
            else
            {
                if(MouseControler.getInstance().getSubState() != MouseControler.SubState.Drawing)
                {
                    MouseControler.getInstance().setSubState(MouseControler.SubState.Drawing);
                }
                
            }
            
        }

        private void changeCursor2(object o, MouseEventArgs e)
        {
            if(isInside(e.Location) && Cursor.Current != Cursors.Hand)
            {
                Cursor.Current = Cursors.SizeAll;
            }
            if(isInside(e.Location))
            {
                if(MouseControler.getInstance().getSubState() != MouseControler.SubState.Moving)
                {
                    MouseControler.getInstance().setSubState(MouseControler.SubState.Moving);
                    //TODO: make it better if there is more sprites 
                }
            }
            else
            {
                if(MouseControler.getInstance().getSubState() != MouseControler.SubState.Drawing)
                {
                    MouseControler.getInstance().setSubState(MouseControler.SubState.Drawing);
                }

            }

        }

        public override string ToString()
        {
            return this.name;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public Pivot getPivot()
        {
            return pivot;
        }

        public int getPixelsPerUnit()
        {
            return pixelsPerUnit;
        }

        internal void setXY(Point pos)
        {
            x = pos.X;
            y = fixY(pos.Y);
            rect.X = pos.X;
            rect.Y = fixY(pos.Y);
        }
    }
}
