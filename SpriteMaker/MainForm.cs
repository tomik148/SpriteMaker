using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpriteMaker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(groupBox6.Visible)
            {
                groupBox6.Visible = false;
                button1.Text = "Make XML";
            }
            else
            {
                groupBox6.Visible = true;
                button1.Text = "Add Sprite";
            }
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //MessageBox.Show(openFileDialog1.FileName);
            //TODO: Check for XML file and load it
            pictureBoxMain.Image = new Bitmap(openFileDialog1.FileName);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
