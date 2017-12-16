using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace SpriteMaker
{
    public partial class MainForm : Form
    {
        public static MainForm instance;
        public int height;
        string imagePath = "";
        bool multiSprites = false;
        bool dragging = false;
        Sprite selected;
        Point startOfDrag;


        Image image;
        List<Sprite> sprites;

        public MainForm()
        {
            InitializeComponent();
            instance = this;

            sprites = new List<Sprite>();

            selectPivot.DataSource = Enum.GetValues(typeof(Pivot));

            //pictureBoxMain.Cursor = Cursors.Cross;
            Cursor.Current = Cursors.Cross;


            MouseControler.getInstance().pressed += (o, e) => { if(MouseControler.getInstance().getSubState() == MouseControler.SubState.Drawing) updateXY(e.Location); };
            MouseControler.getInstance().pressed += (o, e) => { if(MouseControler.getInstance().getSubState() == MouseControler.SubState.Selecting) setSelected(getSpriteAt(e.Location)); };
            MouseControler.getInstance().pressed += (o, e) => { if(MouseControler.getInstance().getSubState() == MouseControler.SubState.Moving) relative = new Point( e.X - selected.getX(), e.Y - Sprite.fixY(selected.getY()) ); };
            MouseControler.getInstance().pressed += (o, e) => instance.startOfDrag = e.Location;


            MouseControler.getInstance().released += (o, e) => { if(MouseControler.getInstance().getSubState() == MouseControler.SubState.Drawing) updateWH(e, true); };
            MouseControler.getInstance().released += (o, e) => { if(MouseControler.getInstance().getSubState() == MouseControler.SubState.Moving) endMove(e); relative = Point.Empty; };
            MouseControler.getInstance().released += (o, e) => pictureBoxMain.Invalidate();


            MouseControler.getInstance().holding += (o, e) => { if(MouseControler.getInstance().getSubState() == MouseControler.SubState.Drawing) updateWH(e, false); };
            MouseControler.getInstance().holding += (o, e) => { if(MouseControler.getInstance().getSubState() == MouseControler.SubState.Moving) moveSprite(e); };
            MouseControler.getInstance().holding += (o, e) => pictureBoxMain.Invalidate();

            //MouseControler.getInstance().hover += (o, e) => {  if(selected?.isInside(e.Location) == true && Cursor.Current != Cursors.Hand ) { Cursor.Current = Cursors.Hand; } };
        }



        void debugInput()
        {
            textBoxName.Text = "Bla";
            textBoxX.Text = "0";
            textBoxY.Text = "0";
            textBoxW.Text = "10";
            textBoxH.Text = "10";

            selectPivot.SelectedIndex = 0;

            textBox1.Text = "10";
        }//TODO:Delete

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(groupBox6.Visible)
            {
                groupBox6.Visible = false;
                button1.Text = "Make XML";
                multiSprites = false;
            }
            else
            {
                groupBox6.Visible = true;
                button1.Text = "Add Sprite";
                multiSprites = true;
            }
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            imagePath = openFileDialog1.FileName;
            // TODO: Check for XML file and load it
            image = new Bitmap(imagePath);
            height = image.Height;
            pictureBoxMain.Image = image;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            debugInput();//TODO: delete
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(imagePath == "")
            {
                MessageBox.Show("Please load image.");
                return;
            }

            if(multiSprites)
            {
                addSpriteToList();
            }
            else
            {
                if(addSpriteToList())
                {
                    makeXMLFile();
                    resetTextBoxes();
                }
            }
            pictureBoxMain.Invalidate();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(imagePath == "")
            {
                MessageBox.Show("Please load image.");
                return;
            }

            makeXMLFile();
        }


        
        
        void makeXMLFile()
        {
            if(imagePath == "")
            {
                MessageBox.Show("Please load image.");
                return;
            }

            StringBuilder path = new StringBuilder(imagePath);
            path.Replace(".png", ".xml");
            path.Replace(".jpg", ".xml");
            string xmlPath = path.ToString();

            XmlWriter xw = XmlWriter.Create(xmlPath);
            xw.WriteStartDocument();
            xw.WriteStartElement("Sprites");
            foreach(Sprite item in sprites)
            {
                item.getXml(xw);
            }

            xw.WriteEndElement();
            xw.WriteEndDocument();
            xw.Close();
            resetTextBoxes();
        }

        public Sprite getSelected()
        {
            return selected;
        }

        void lodeXMLFile(string xmlPath)
        {
            if(xmlPath == "" && File.Exists(xmlPath))
            {
                return;
            }
            XmlReader xr = XmlReader.Create(xmlPath);
            while(xr.Read())
            {
                if(xr.Name == "Sprite")
                {
                    //TODO: Pivot
                    Sprite s = new Sprite(xr.GetAttribute("name"), int.Parse(xr.GetAttribute("x")), int.Parse(xr.GetAttribute("y")), int.Parse(xr.GetAttribute("w")), int.Parse(xr.GetAttribute("h")), Pivot.Center, int.Parse(xr.GetAttribute("pixelPerUnit")));
                    sprites.Add(s);
                }
            }
        }
        
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            Sprite s = sprites.Find((x) => { return x.name == name; });
            if(s == null)
            {
                if(groupBox6.Visible)
                {
                    button1.Text = "Add Sprite";
                }
                Console.WriteLine("Add Sprite");
                return;
            }
            Console.WriteLine("Update Sprite");
            button1.Text = "Update Sprite";
        }

        void drawRectangle(Sprite s)
        {
            //pictureBoxMain.Invalidate();
            //Graphics g = pictureBoxMain.CreateGraphics();
            //g.Clear(Color.Black);
            //pictureBoxMain.
            //s.render(g, Pens.Black, pictureBoxMain.Height);

            //g.FillRectangle(Brushes.Red, rect.X, rect.Y, 10, 10);
            //g.FillRectangle(Brushes.Blue, rect.X, rect.Y + rect.Height, 10, 10);
            //g.FillRectangle(Brushes.Green, rect.X + rect.Width, rect.Y, 10, 10);
            //g.FillRectangle(Brushes.Yellow, rect.X + rect.Width, rect.Y + rect.Height, 10, 10);

            //g.DrawRectangle(Pens.Black, s.getRect.X , Sprite.fixY(rect.Y,pictureBoxMain.Height) - rect.Height , rect.Width , rect.Height);
        }

        void drawRectangle(Graphics g, Rectangle rect)
        {
            //pictureBoxMain.Invalidate();
            //Graphics g = pictureBoxMain.CreateGraphics();
            //g.Clear(Color.Black);
            //pictureBoxMain.
            //s.render(g, Pens.Black, pictureBoxMain.Height);

            //g.FillRectangle(Brushes.Red, rect.X, rect.Y, 10, 10);
            //g.FillRectangle(Brushes.Blue, rect.X, rect.Y + rect.Height, 10, 10);
            //g.FillRectangle(Brushes.Green, rect.X + rect.Width, rect.Y, 10, 10);
            //g.FillRectangle(Brushes.Yellow, rect.X + rect.Width, rect.Y + rect.Height, 10, 10);

            g.DrawRectangle(Pens.Black, rect.X, Sprite.fixY(rect.Y) - rect.Height, rect.Width, rect.Height);
        }

        private void pictureBoxMain_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            MouseControler.getInstance().Update(MouseControler.State.Down, sender, e);
        }

        private void pictureBoxMain_MouseUp(object sender, MouseEventArgs e)
        {
            MouseControler.getInstance().Update(MouseControler.State.Up, sender, e);
            dragging = false;

        }

        void updateXY(Point e)
        {
            textBoxX.Text = e.X.ToString();
            textBoxY.Text = Sprite.fixY(e.Y).ToString();
        }
        void updateWH(MouseEventArgs e, bool validate)
        {
            int w = e.X - int.Parse(textBoxX.Text);
            int h = Sprite.fixY(e.Y) - int.Parse(textBoxY.Text);
            if(validate)
            {
                validateWH(ref w, ref h);
            }

            textBoxW.Text = w.ToString();
            textBoxH.Text = h.ToString();
        }


        Point relative;
        void moveSprite(MouseEventArgs e)
        {
            Point positon = new Point(e.X - relative.X, e.Y - relative.Y);
            updateXY(positon);
            selected.setXY(positon);
            var s = sprites.Find(x => x.name == selected.name);
            s.update(selected);
            setSelected(s);
        }

        void endMove(MouseEventArgs e)
        {
            //updateXY(e.Location);


        }


        void validateWH(ref int w, ref int h)
        {
            if(w < 0)
            {
                textBoxX.Text = (int.Parse(textBoxX.Text) + w).ToString();
                w = Math.Abs(w);
            }

            if(h < 0)
            {
                textBoxY.Text = (int.Parse(textBoxY.Text) + h).ToString();
                h = Math.Abs(h);
            }
        }

        Rectangle getRect()
        {
            
            // = new Rectangle(int.Parse(textBoxX.Text), int.Parse(textBoxY.Text), int.Parse(textBoxW.Text), int.Parse(textBoxH.Text));
            int x = int.Parse(textBoxX.Text), y = int.Parse(textBoxY.Text), w = int.Parse(textBoxW.Text), h = int.Parse(textBoxH.Text);
            if(w < 0)
            {
                x += w;
                w *= -1; 
            }

            if(h < 0)
            {
                y += h;
                h *= -1;
            }
            Rectangle rect = new Rectangle(x,y,w,h);
            return rect;
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {

            if(dragging)
            {
                MouseControler.getInstance().Update(MouseControler.State.Holding, sender, e);
                //updateWH(e, false);
            }
            else
            {
                MouseControler.getInstance().Update(MouseControler.State.Hovering, sender, e);
            }
        }

        private void pictureBoxMain_Paint(object sender, PaintEventArgs e)
        {
            //if(selected != null && sprites.Contains(selected))
            //{
            //    sprites.Find(x=>x==selected).render(e.Graphics, Pens.Black, pictureBoxMain.Height);
            //}

            foreach(var sprite in sprites)
            {
                sprite.render(e.Graphics, Pens.Black, sprite == selected);
            }

            drawRectangle(e.Graphics, getRect());
        }

        Point lastSelectedPoint;
        List<Sprite> spritesAtLastPoint;
        int index;
        public Sprite getSpriteAt(Point location)
        {
            if(lastSelectedPoint == location)
            {
                if(++index >= spritesAtLastPoint.Count)
                    index = 0;
                return spritesAtLastPoint.ElementAt(index);
            }
            lastSelectedPoint = location;
            spritesAtLastPoint = new List<Sprite>();
            index = 0;
            foreach(var sprite in sprites)
            {
                if(sprite.isInside(location))
                {
                    spritesAtLastPoint.Add(sprite);
                }
            }
            return spritesAtLastPoint.ElementAt(index);
        }

        public bool isSpriteAt(Point location)
        {
            foreach(var sprite in sprites)
            {
                if(sprite.isInside(location))
                {
                    return true;
                }
            }
            return false;
        }

        void resetTextBoxes()
        {
            // TODO: load name from path
            textBoxName.Text = "";
            textBoxX.Text = "0";
            textBoxY.Text = "0";
            textBoxW.Text = "0";
            textBoxH.Text = "0";

            selectPivot.SelectedIndex = 0;

            //if(sprites.Find(x => x.name == selected?.name) != null)
            //{
            //    listBox1.Items[sprites.IndexOf(selected)+1].Selected = true;
            //}

            textBox1.Text = "0";
        }
        void loadSprite(Sprite s)
        {
            textBoxName.Text = "";
            textBoxName.Text = s.name;
            textBoxX.Text = s.getX().ToString();
            textBoxY.Text = s.getY().ToString();
            textBoxW.Text = s.getWidth().ToString();
            textBoxH.Text = s.getHeight().ToString();

            selectPivot.SelectedIndex = (int)s.getPivot();

            textBox1.Text = s.getPixelsPerUnit().ToString();

            //listView2.SelectedIndices.Clear();

            //listBox1.FindItemWithText(s.name).Selected = true;
            //listBox1.FindItemWithText(s.name).Focused = true;


            //TODO: DrawSelected
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            //int[] a = new int[1];
            //listBox1.SelectedIndices.CopyTo(a, 0);
            int index = listBox1.SelectedIndex; // a[0];
            setSelected(sprites[index]);
            loadSprite(selected);

            drawRectangle(selected);  //TODO: remove
        }
        public void setSelected(Sprite s)
        {
            //selected?.unSelect();
            selected = s;
            //s.select();

            loadSprite(s);
            
            listBox1.SetSelected(sprites.IndexOf(sprites.Find((x) => { return x.name == s.name; })), true);
            pictureBoxMain.Invalidate();
        }

        void relodeListView()
        {
            listBox1.Items.Clear();
            foreach(Sprite item in sprites)
            {
                listBox1.Items.Add(item.ToString());
            }
        }
        bool addSpriteToList()
        {
            if(textBoxName.Text == "")
            {
                MessageBox.Show("Name is required.");
                return false;
            }

            if(textBox1.Text == "" || textBox1.Text == "0")
            {
                MessageBox.Show("Pixel per unit is required.");
                return false;
            }

            if(textBoxW.Text == "")
            {
                MessageBox.Show("Width is required.");
                return false;
            }

            if(textBoxH.Text == "")
            {
                MessageBox.Show("Height is required.");
                return false;
            }

            if(textBoxX.Text == "")
            {
                MessageBox.Show("X is required.");
                return false;
            }

            if(textBoxY.Text == "")
            {
                MessageBox.Show("Y is required.");
                return false;
            }

            if(textBoxW.Text == "0" || textBoxH.Text == "0")
            {
                MessageBox.Show("Height or width cant be 0.");
                return false;
            }

            Sprite s = new Sprite(textBoxName.Text, int.Parse(textBoxX.Text), int.Parse(textBoxY.Text), int.Parse(textBoxW.Text), int.Parse(textBoxH.Text), (Pivot)selectPivot.SelectedValue, int.Parse(textBox1.Text));
            s.registerCallbacks();

            var list = sprites.Where(x => x.name == s.name).ToList();
            if(list.Count > 0)
            {
                list.ForEach(y => { y.update(s); s = y; });
            }
            else
            {
                sprites.Add(s);
            }

            Console.WriteLine("addSpriteToList -- num of sprites in list: " + sprites.Count);


            relodeListView();
            setSelected(s);
            return true;
        }

    }
}
