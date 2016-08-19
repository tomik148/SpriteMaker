using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace SpriteMaker
{
    public partial class MainForm : Form
    {

        string imagePath = "";
        bool multiSprites = false;
        List<Sprite> sprites;

        public MainForm()
        {
            InitializeComponent();
            selectPivot.DataSource = Enum.GetValues(typeof(Pivot));
            sprites = new List<Sprite>();
        }

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
            //MessageBox.Show(openFileDialog1.FileName);
            imagePath = openFileDialog1.FileName;
            //TODO: Check for XML file and load it
            pictureBoxMain.Image = new Bitmap(imagePath);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

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
                } 
                
            }
            resetTextBoxes();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            makeXMLFile();
        }

        void relodeListView()
        {
            listView2.Items.Clear();
            foreach(Sprite item in sprites)
            {
                listView2.Items.Add(item.ToString());
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

            if(textBoxW.Text == "0" || textBoxH.Text == "0" )
            {
                MessageBox.Show("Height or width cant be 0.");
                return false;
            }


            Sprite s = new Sprite(textBoxName.Text, int.Parse(textBoxX.Text), int.Parse(textBoxY.Text), int.Parse(textBoxW.Text), int.Parse(textBoxH.Text), (Pivot)selectPivot.SelectedValue, int.Parse(textBox1.Text));

            Sprite f = sprites.Find((x) => { return x.name == textBoxName.Text; });
            if(f != null)
            {
                sprites.Remove(f);
            }

            sprites.Add(s);
            relodeListView();
            return true;
        }

        void loadSprite(Sprite s)
        {
            textBoxName.Text = s.name;
            textBoxX.Text = s.x.ToString();
            textBoxY.Text = s.y.ToString();
            textBoxW.Text = s.width.ToString();
            textBoxH.Text = s.height.ToString();

            selectPivot.SelectedIndex = (int)s.pivot;

            textBox1.Text = s.pixelsPerUnit.ToString();
        }

        void resetTextBoxes()
        {
            textBoxName.Text = "";
            textBoxX.Text = "0";
            textBoxY.Text = "0";
            textBoxW.Text = "0";
            textBoxH.Text = "0";

            selectPivot.SelectedIndex = 0;

            textBox1.Text = "0";
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

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] a = new int[1];
            listView2.SelectedIndices.CopyTo(a,0);
            int index = a[0];
            loadSprite(sprites[index]);
            
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
                return;
            }

            //loadSprite(s);
            button1.Text = "Update Sprite";
        }
        /*

Center
Top left
Top
Top right
Left
Right
Bottom left
Bottom
Bottom right
Custom

*/


    }
}
