using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StishBoard
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = (250); // 250 milisecs
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //refresh here...
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Coordinate Button2Co = new Coordinate(0,0);
            if (StishBoard.Instance.getSquare(Button2Co).Dep.DepType == "Unit" && StishBoard.Instance.getSquare(Button2Co).Dep.OwnedBy.GetPlayerNum == StishBoard.Instance.GamePlayersTurn.ToString())
            {
                button2.Text = "Unit";
                button2.BackColor = System.Drawing.Color.FromArgb(255, 255, 153);
            }
        }
    }
}