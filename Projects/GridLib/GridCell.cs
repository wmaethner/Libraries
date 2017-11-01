using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridLib
{
	internal class GridCell
	{
		#region Private Variables
		private int MinX;
		private int MaxX;
		private int MinY;
		private int MaxY;

		private Color _paintColor;
		private Color _backColor;

		Brush paintBrush;
		Brush backcolorBrush;
		#endregion

		#region Properties
		public bool Painted { get; set; }

		public Graphics Graphics { get; set; }

		public Color PaintColor
		{
			get { return _paintColor; }
			set
			{
				_paintColor = value;
				paintBrush = new SolidBrush(_paintColor);
			}
		}
		public Color BackColor
		{
			get { return _backColor; }
			set
			{
				_backColor = value;
				backcolorBrush = new SolidBrush(_backColor);
			}
		}
		#endregion

		#region Constructors
		public GridCell(int x_0, int y_0, int size, Graphics g)
		{
			MinX = x_0;
			MaxX = MinX + size;

			MinY = y_0;
			MaxY = MinY + size;

			Graphics = g;
			PaintColor = Color.Black;
			BackColor = Color.White;

			Painted = false;
		}
		#endregion

		#region Paint Functions
		public void PaintCell()
		{
			ErrorCheck();

			Graphics.FillRectangle(paintBrush, new Rectangle(MinX, MinY, MaxX - MinX, MaxY - MinY));
			DrawBorder();
			Painted = true;
		}
		public void PaintCell(Color color)
		{
			Graphics.FillRectangle(new SolidBrush(color), new Rectangle(MinX, MinY, MaxX - MinX, MaxY - MinY));
			DrawBorder();
			Painted = true;
		}
		public void UnpaintCell()
		{
			ErrorCheck();

			Graphics.FillRectangle(backcolorBrush, new Rectangle(MinX, MinY, MaxX - MinX, MaxY - MinY));
			DrawBorder();
			Painted = false;
		}
		public bool ReverseCell()
		{
			ErrorCheck();
			Painted = !Painted;

			Graphics.FillRectangle(Painted ? paintBrush : backcolorBrush, new Rectangle(MinX, MinY, MaxX - MinX, MaxY - MinY));
			DrawBorder();

			return Painted;
		}

		private void DrawBorder()
		{
			Graphics.DrawRectangle(Pens.Black, new Rectangle(MinX, MinY, MaxX - MinX, MaxY - MinY));
		}

		private void ErrorCheck()
		{
			if (Graphics == null)
			{
				throw new Exception("No graphics set.");
			}

		}
		#endregion
	}
}
