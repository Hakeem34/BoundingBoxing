using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BoundingBoxing
{
    class Boxs
    {

        private List<BoundingBox> box_list;
        private int current_index;
        private Size logical_window;
        private Size physical_window;

        public Boxs()
        {
            box_list = new List<BoundingBox>();
            current_index = 0;
            logical_window = new Size(1000, 1000);
            physical_window = new Size(1920, 1080);
        }

        public void SetLogicalSize(Size size)
        {
            logical_window = size;
        }

        public void SetPhysicalSize(Size size)
        {
            physical_window = size;
        }

        int TransL2P_X(int X)
        {
            return (X * physical_window.Width) / logical_window.Width;
        }

        int TransL2P_Y(int Y)
        {
            return (Y * physical_window.Height) / logical_window.Height;
        }

        Size TransL2P_Size(Size size)
        {
            return new Size(TransL2P_X(size.Width), TransL2P_Y(size.Height));
        }

        Point TransL2P_Point(Point point)
        {
            return new Point(TransL2P_X(point.X), TransL2P_Y(point.Y));
        }


        public void AddBox(BoundingBox box)
        {
            box_list.Add(box);
        }

        public void AddBox(int X, int Y, int W, int H, String class_name, int probability, Color color, int tag)
        {
            BoundingBox box = new BoundingBox(X, Y, W, H, class_name, probability, color, tag);
            this.AddBox(box);
        }

        public BoundingBox GetBoxByTag(int tag)
        {
            foreach (BoundingBox box in box_list)
            {
                if (box.tag == tag)
                {
                    return box;
                }
            }

            return null;
        }

        public BoundingBox GetBoxAt(int index)
        {
            if (box_list.Count > index)
            {
                return box_list[index];
            }

            return null;
        }

        public int GetSize()
        {
            return box_list.Count;
        }

        public bool RemoveBox(BoundingBox box)
        {
            return box_list.Remove(box);
        }

        public void RemoveAllBoxs()
        {
            box_list.RemoveRange(0,box_list.Count);
            current_index = 0;
        }

        public void DrawBox(BoundingBox box, Graphics graph)
        {
            int phys_width = TransL2P_X(box.Width);
            int phys_height = TransL2P_Y(box.Height);
            int phys_x = TransL2P_X(box.X);
            int phys_y = TransL2P_Y(box.Y);
            Rectangle phys_rect = new Rectangle(phys_x, phys_y, phys_width, phys_height);

            SolidBrush brush = new SolidBrush(box.color);
            Font f = new Font("Yu Gothic UI", 12);
            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            graph.DrawString(box.class_name, f, brush, phys_x, phys_y);
            graph.DrawRectangle(new Pen(box.color, 1), phys_rect);
        }

        public void DrawAllBoxs(Graphics graph)
        {
            foreach (BoundingBox box in this.box_list)
            {
                DrawBox(box, graph);
            }
        }
    }

    public class BoundingBox
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public String class_name;
        public int probability;
        public Color color;
        public int tag;

        public BoundingBox(int X, int Y, int W, int H) : this(X,Y,W,H,"",100,Color.Red,0)
        {
        }

        public BoundingBox(int X, int Y, int W, int H, String class_name, int probability) : this(X, Y, W, H, class_name, probability, Color.Red, 0)
        {
        }

        public BoundingBox(int X, int Y, int W, int H, String class_name, int probability, Color color, int tag)
        {
            this.X = X;
            this.Y = Y;
            this.Width = W;
            this.Height = H;
            this.class_name = class_name;
            this.probability = probability;
            this.color = color;
            this.tag = tag;
        }
    }
}
