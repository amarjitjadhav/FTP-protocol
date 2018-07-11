using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public class Color
    {
        public int r = 255;
        public int g = 255;
        public int b = 255;
        private int bg_r = 0;
        private int bg_g = 0;
        private int bg_b = 0;

        public Color(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color(int r, int g, int b, int bg_r, int bg_g, int bg_b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.bg_r = bg_r;
            this.bg_g = bg_g;
            this.bg_b = bg_b;
        }
        
        public Color Invert()
        {
            return new Color(bg_r, bg_g, bg_b, r, g, b);
        }

        public String ToCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\u001b[38;2;");
            sb.Append(r);
            sb.Append(";");
            sb.Append(g);
            sb.Append(";");
            sb.Append(b);
            sb.Append("m\u001b[48;2;");
            sb.Append(bg_r);
            sb.Append(";");
            sb.Append(bg_g);
            sb.Append(";");
            sb.Append(bg_b);
            sb.Append("m");
            return sb.ToString();
        }

        public void SetForeground(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        public void SetBackground(int r, int g, int b)
        {
            this.bg_r = r;
            this.bg_g = g;
            this.bg_b = b;
        }
    }
}
