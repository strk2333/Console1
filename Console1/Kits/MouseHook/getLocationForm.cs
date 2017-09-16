//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Console1.Kits.NewFolder1
//{
//    public partial class getLocationForm : Form
//    {
//        public getLocationForm(int[] location)
//        {
//            InitializeComponent();
//        }

//        MouseHook mh;
//        private void getLocationForm_Load(object sender, EventArgs e)
//        {
//            mh = new MouseHook();
//            mh.SetHook();
//            mh.MouseMoveEvent += mh_MouseMoveEvent;
//            mh.MouseClickEvent += mh_MouseClickEvent;

//        }

//        private void mh_MouseClickEvent(object sender, MouseEventArgs e)
//        {
//            MessageBox.Show(e.X + "-" + e.Y);
//        }

//        private void mh_MouseMoveEvent(object sender, MouseEventArgs e)
//        {
//            int x = e.Location.X;
//            int y = e.Location.Y;
//            textBox1.Text = x + "";
//            textBox2.Text = y + "";
//        }
//        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
//        {
//            mh.UnHook();
//        }

//    }
//}
