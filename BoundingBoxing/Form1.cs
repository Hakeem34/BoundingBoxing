using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices; //DllImport属性を使用するために必要

namespace BoundingBoxing
{
    public partial class Form1 : Form
    {
        delegate void CmdDelegate();

        private int mouse_on = 0;
        private int drag_mode = 0;
        private Point pre_location;
        private Thread console_thread;
        private Boxs boxs;
        private Color BoundingBoxColor = Color.Red;


        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(uint dwProcessId);

        [DllImport("Kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        void BB_Update()
        {
            pnl_window.Invalidate();
        }

        private void ConsoleThreadEntry()
        {

            bool attach = AttachConsole(0xFFFFFFFF);
            if (attach == false)
            {
                bool ret = AllocConsole();
            }
/*
            // stdoutのストリームを取得
            System.IO.Stream stream = System.Console.OpenStandardOutput();
            System.IO.StreamWriter stdout = new System.IO.StreamWriter(stream);

            if (attach == false)
            {
                stdout.WriteLine("MessgageB!");
            }
            else
            {
                stdout.WriteLine("MessgageA!");
            }
            stdout.Flush();
*/
            while (true)
            {
                Match result;
                int X,Y,W,H;
                Color col;
                String class_name;
                int prob;
                int tag;
                String disp = "";

                var line = System.Console.ReadLine();
                var regex_BB1 = new Regex(@"^[bB][bB]\s+([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\s*,\s*([^,]+),\s*0x([0-9a-fA-F]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\s*$");
                var regex_BB2 = new Regex(@"^[bB][bB]\s+([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\s*,\s*([^,]+),\s*0x([0-9a-fA-F]+)\s*,\s*([0-9]+)\s*$");
                var regex_BB3 = new Regex(@"^[bB][bB]\s+([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\s*,\s*([0-9]+)\s*$");
                var regex_UP = new Regex(@"^UPDATE\s*$", RegexOptions.IgnoreCase);
                var regex_CLR = new Regex(@"^CLEAR\s*$", RegexOptions.IgnoreCase);

                if ((result = regex_BB1.Match(line)).Success)
                {
                    X = Int32.Parse(result.Groups[1].ToString());
                    Y = Int32.Parse(result.Groups[2].ToString());
                    W = Int32.Parse(result.Groups[3].ToString());
                    H = Int32.Parse(result.Groups[4].ToString());
                    class_name = result.Groups[5].ToString();
                    col = Color.FromArgb(Int32.Parse(result.Groups[6].ToString(), System.Globalization.NumberStyles.HexNumber));
                    prob = Int32.Parse(result.Groups[7].ToString());
                    tag = Int32.Parse(result.Groups[8].ToString());
                    disp = String.Format("BoundingBox X={0}, Y={1}, W={2}, H={3}, Class:{4}, {5}, {6}%, TAG:{7}", X, Y, W, H, class_name, col, prob, tag);
                }
                else if ((result = regex_BB2.Match(line)).Success)
                {
                    X = Int32.Parse(result.Groups[1].ToString());
                    Y = Int32.Parse(result.Groups[2].ToString());
                    W = Int32.Parse(result.Groups[3].ToString());
                    H = Int32.Parse(result.Groups[4].ToString());
                    class_name = result.Groups[5].ToString();
                    col = Color.FromArgb(Int32.Parse(result.Groups[6].ToString(), System.Globalization.NumberStyles.HexNumber));
                    prob = Int32.Parse(result.Groups[7].ToString());
                    tag = -1;

                    disp = String.Format("BoundingBox X={0}, Y={1}, W={2}, H={3}, Class:{4}, {5}, {6}%", X, Y, W, H, class_name, col, prob);
                }
                else if ((result = regex_BB3.Match(line)).Success)
                {
                    X = Int32.Parse(result.Groups[1].ToString());
                    Y = Int32.Parse(result.Groups[2].ToString());
                    W = Int32.Parse(result.Groups[3].ToString());
                    H = Int32.Parse(result.Groups[4].ToString());
                    class_name = "";
                    col = Color.Red;
                    prob = 100;
                    tag = -1;

                    disp = String.Format("BoundingBox X={0}, Y={1}, W={2}, H={3}", X, Y, W, H);
                }
                else if ((result = regex_UP.Match(line)).Success)
                {
                    System.Console.WriteLine("UPDATE!");
                    Invoke(new CmdDelegate(BB_Update));
                    continue;
                }
                else if ((result = regex_CLR.Match(line)).Success)
                {
                    System.Console.WriteLine("CLEAR!");
                    boxs.ClearAll();
                    continue;
                }
                else
                {
                    System.Console.WriteLine("Invalid Sentence");
                    continue;
                }

                System.Console.WriteLine(disp);
                boxs.AddBox(X, Y, W, H, class_name, prob, col, tag);
            }
        }


        public Form1()
        {
            InitializeComponent();
            boxs = new BoundingBoxing.Boxs();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 透過色に背景色を設定
            this.TransparencyKey = this.BackColor;

            this.FormBorderStyle = FormBorderStyle.None;
            panel2_resize();

            this.console_thread = new Thread(new ThreadStart(this.ConsoleThreadEntry));
            this.console_thread.IsBackground = true;
            this.console_thread.Start();
//          this.pnl_trans.BackColor = Color.Transparent;
        }

        private void panel2_resize()
        {
            String text = String.Format("BoundingBoxing({0} x {1})", this.pnl_window.Width, this.pnl_window.Height);
            this.lbl_title.Text = text;
            boxs.SetPhysicalSize(new Size(this.pnl_window.Width, this.pnl_window.Height));
            pnl_window.Invalidate();
        }


        private void add_bounding_box(Point location, Size size, String class_name, int probability, Color color, int tag)
        {
            this.boxs.AddBox(location.X, location.Y, size.Width, size.Height, class_name, probability, color, tag);
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
                    this.drag_mode = Constants.DRAG_MODE_TOP | Constants.DRAG_MODE_LEFT;
                }
                else if (IsTopEdge(e.Location) && IsRightEdge(e.Location))
                {
                    this.drag_mode = Constants.DRAG_MODE_TOP | Constants.DRAG_MODE_RIGHT;
                }
                else if (IsTopEdge(e.Location))
                {
                    this.drag_mode = Constants.DRAG_MODE_TOP;
                }
                else if (IsBottomEdge(e.Location) && IsLeftEdge(e.Location))
                {
                    this.drag_mode = Constants.DRAG_MODE_BOTTOM | Constants.DRAG_MODE_LEFT;
                }
                else if (IsBottomEdge(e.Location) && IsRightEdge(e.Location))
                {
                    this.drag_mode = Constants.DRAG_MODE_BOTTOM | Constants.DRAG_MODE_RIGHT;
                    this.Cursor = Cursors.SizeNWSE;
                }
                else if (IsBottomEdge(e.Location))
                {
                    this.drag_mode = Constants.DRAG_MODE_BOTTOM;
                    this.Cursor = Cursors.SizeNS;
                }
                else if (IsRightEdge(e.Location))
                {
                    this.drag_mode = Constants.DRAG_MODE_RIGHT;
                    this.Cursor = Cursors.SizeWE;
                }
                else if (IsLeftEdge(e.Location))
                {
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

        private void pnl_window_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush b_back = new SolidBrush(Color.DarkGray);
            SolidBrush b_trans = new SolidBrush(this.BackColor);
            Rectangle window_rect = new Rectangle(0, 0, this.Width, this.Height);
            e.Graphics.FillRectangle(b_back, window_rect);

            Rectangle trans_rect = new Rectangle(Constants.EDGE_SIZE, Constants.EDGE_SIZE + Constants.LABEL_SIZE, this.Width - Constants.EDGE_SIZE * 2, this.Height - Constants.LABEL_SIZE - Constants.EDGE_SIZE * 2);
            e.Graphics.FillRectangle(b_trans, trans_rect);

            boxs.DrawAllBoxs(e.Graphics);
        }

        private void pnl_window_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                add_bounding_box(new Point(0, 100), new Size(100, 100), "Test", 100, this.BoundingBoxColor, 1);
                add_bounding_box(new Point(this.Width - 100, 100), new Size(100, 100), "Test2", 100, Color.Blue, 2);
//              add_bounding_box(new Point(150, 150), new Size(2, 2), "Test3", 100);
//              add_bounding_box(new Point(200, 200), new Size(1, 1), "Test4", 100);
//              add_bounding_box(new Point(250, 250), new Size(0, 0), "Test4", 100);
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
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
