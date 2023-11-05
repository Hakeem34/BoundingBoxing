using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoundingBoxing
{
    public partial class Form1 : Form
    {
        private int mouse_on = 0;
        private int drag_mode = 0;
        private Point pre_location;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 透過色に背景色を設定
            this.TransparencyKey = this.BackColor;
            this.FormBorderStyle = FormBorderStyle.None;
//          this.ControlBox = false;
//          this.Text = "";
            panel2_resize();
        }

        private void panel2_resize()
        {
            this.pnl_trans.Top    = Constants.EDGE_SIZE + Constants.LABEL_SIZE;
            this.pnl_trans.Left   = Constants.EDGE_SIZE;
            this.pnl_trans.Width  = this.Width - (Constants.EDGE_SIZE * 2);
            this.pnl_trans.Height = this.Height - (Constants.LABEL_SIZE + Constants.EDGE_SIZE * 2);

            String text = String.Format("BoundingBoxing({0} x {1})", this.pnl_trans.Width, this.pnl_trans.Height);
            this.lbl_title.Text = text;
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            panel2_resize();
        }


        private bool IsLeftEdge(Point location)
        {
            if (location.X < Constants.EDGE_SIZE)
                return true;

            return false;
        }

        private bool IsRightEdge(Point location)
        {
            if (location.X > this.Width - Constants.EDGE_SIZE)
                return true;

            return false;
        }

        private bool IsTopEdge(Point location)
        {
            if (location.Y < Constants.EDGE_SIZE + Constants.LABEL_SIZE)
                return true;

            return false;
        }

        private bool IsBottomEdge(Point location)
        {
            if (location.Y > this.Height - Constants.EDGE_SIZE)
                return true;

            return false;
        }

        private void pnl_window_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mouse_on = 1;
                pre_location = e.Location;

                if (IsTopEdge(e.Location) && IsLeftEdge(e.Location))
                {
                    this.lbl_title.Text = "TopLeft";
                    this.drag_mode = Constants.DRAG_MODE_TOP | Constants.DRAG_MODE_LEFT;
                }
                else if (IsTopEdge(e.Location) && IsRightEdge(e.Location))
                {
                    this.lbl_title.Text = "TopRight";
                    this.drag_mode = Constants.DRAG_MODE_TOP | Constants.DRAG_MODE_RIGHT;
                }
                else if (IsTopEdge(e.Location))
                {
                    this.lbl_title.Text = "Top";
                    this.drag_mode = Constants.DRAG_MODE_TOP;
                }
                else if (IsBottomEdge(e.Location) && IsLeftEdge(e.Location))
                {
                    this.lbl_title.Text = "BottomLeft";
                    this.drag_mode = Constants.DRAG_MODE_BOTTOM | Constants.DRAG_MODE_LEFT;
                }
                else if (IsBottomEdge(e.Location) && IsRightEdge(e.Location))
                {
                    this.lbl_title.Text = "BottomRight";
                    this.drag_mode = Constants.DRAG_MODE_BOTTOM | Constants.DRAG_MODE_RIGHT;
                    this.Cursor = Cursors.SizeNWSE;
                }
                else if (IsBottomEdge(e.Location))
                {
                    this.lbl_title.Text = "Bottom";
                    this.drag_mode = Constants.DRAG_MODE_BOTTOM;
                    this.Cursor = Cursors.SizeNS;
                }
                else if (IsRightEdge(e.Location))
                {
                    this.lbl_title.Text = "Right";
                    this.drag_mode = Constants.DRAG_MODE_RIGHT;
                    this.Cursor = Cursors.SizeWE;
                }
                else if (IsLeftEdge(e.Location))
                {
                    this.lbl_title.Text = "Left";
                    this.drag_mode = Constants.DRAG_MODE_LEFT;
                }
            }
        }

        private void pnl_window_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.drag_mode = Constants.DRAG_MODE_NONE;
                this.Cursor = Cursors.Default;
            }
        }

        private void pnl_window_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.drag_mode == Constants.DRAG_MODE_NONE)
            {
                if (IsRightEdge(e.Location) && IsBottomEdge(e.Location))
                {
                    this.Cursor = Cursors.SizeNWSE;
                }
                else if (IsRightEdge(e.Location) && !IsTopEdge(e.Location))
                {
                    this.Cursor = Cursors.SizeWE;
                }
                else if (!IsLeftEdge(e.Location) && IsBottomEdge(e.Location))
                {
                    this.Cursor = Cursors.SizeNS;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else if ((this.drag_mode & (Constants.DRAG_MODE_TOP | Constants.DRAG_MODE_LEFT)) != 0)
            {
                this.Left += e.Location.X - this.pre_location.X;
                this.Top += e.Location.Y - this.pre_location.Y;
            }
            else
            {
                if ((this.drag_mode & Constants.DRAG_MODE_BOTTOM) == Constants.DRAG_MODE_BOTTOM)
                {
                    int height = this.Height;
                    int diff = e.Location.Y - this.pre_location.Y;

                    if (height + diff < Constants.MIN_HEIGHT)
                    {
                        this.Height = Constants.MIN_HEIGHT;
                    }
                    else
                    {
                        this.Height += diff;
                    }

                }

                if ((this.drag_mode & Constants.DRAG_MODE_RIGHT) == Constants.DRAG_MODE_RIGHT)
                {
                    int width = this.Width;
                    int diff = e.Location.X - this.pre_location.X;

                    if (width + diff < Constants.MIN_WIDTH)
                    {
                        this.Width = Constants.MIN_WIDTH;
                    }
                    else
                    {
                        this.Width += diff;
                    }
                }

                this.pre_location = e.Location;
            }
        }

        private void lbl_title_MouseDown(object sender, MouseEventArgs e)
        {
            /* 座標が0,0なので、変換無しにイベントをスルーする */
            pnl_window_MouseDown(sender, e);
        }

        private void lbl_title_MouseUp(object sender, MouseEventArgs e)
        {
            /* 座標が0,0なので、変換無しにイベントをスルーする */
            pnl_window_MouseUp(sender, e);
        }

        private void lbl_title_MouseMove(object sender, MouseEventArgs e)
        {
            /* 座標が0,0なので、変換無しにイベントをスルーする */
            pnl_window_MouseMove(sender, e);

        }
    }

    static class Constants
    {
        public const int MIN_SIZE = 200;
        public const int MIN_WIDTH = MIN_SIZE + EDGE_SIZE * 2;
        public const int MIN_HEIGHT = MIN_SIZE + LABEL_SIZE+ EDGE_SIZE * 2;
        public const int EDGE_SIZE = 5;
        public const int LABEL_SIZE = 10;

        public const int DRAG_MODE_NONE   = 0x00;
        public const int DRAG_MODE_MOVE   = 0x01;
        public const int DRAG_MODE_LEFT   = 0x02;
        public const int DRAG_MODE_RIGHT  = 0x04;
        public const int DRAG_MODE_TOP    = 0x08;
        public const int DRAG_MODE_BOTTOM = 0x10;
    }
}
