using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ShapedForms {
    public partial class Form1 : Form {
        private Point m_DownPoint = Point.Empty;

        public Form1() {
            InitializeComponent();
            Load += delegate { SetEllipseRegion(); };
            SizeChanged += delegate { SetEllipseRegion(); };
        }

        private void SetEllipseRegion() {
            Rectangle rect = ClientRectangle;
            using (GraphicsPath path = new GraphicsPath()) {
                path.AddEllipse(rect);
                Region = new Region(path);
            }
        }

        #region Handle Moving The Form
        private void Form1_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) return;
            m_DownPoint = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e) {
            if (m_DownPoint == Point.Empty) return;

            Point location = new Point(Left + e.X - m_DownPoint.X, Top + e.Y - m_DownPoint.Y);
            Location = location;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) return;
            m_DownPoint = Point.Empty;
        }
        #endregion
    }
}