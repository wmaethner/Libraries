using System;
using System.Drawing;
using System.Windows.Forms;

namespace GridLib
{
	public partial class Grid : UserControl
	{
		#region Private Variables
		private int CellSize;
		private GridCell[,] CellCollection;
		private bool _drawing;
		private bool _painting;
		#endregion

		#region Properties
		public int GridSize { get; set; }
		#endregion

		#region Constructors
		public Grid()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
			this.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		private void CreateCellCollection()
		{
			CellCollection = new GridCell[GridSize, GridSize];
			Graphics g = this.CreateGraphics();

			for (int col = 0; col < GridSize; col++)
			{
				for (int row = 0; row < GridSize; row++)
				{
					GridCell cell = new GridCell(col * CellSize, row * CellSize, CellSize, g);
					cell.BackColor = this.BackColor;
					cell.PaintColor = Color.Red;
					CellCollection[row, col] = cell;
				}
			}
		}
		#endregion

		#region Drawing Functions
		public void DrawGrid()
		{
			ClearGrid();
			DrawGridPrivate();
			CreateCellCollection();
		}

		private void DrawGridPrivate()
		{
			Graphics g = this.CreateGraphics();
			CellSize = (this.Size.Width - 1) / GridSize;

			for (int y = 0; y < GridSize + 1; y++)
			{
				g.DrawLine(Pens.Black, 0, y * CellSize, GridSize * CellSize, y * CellSize);
			}

			for (int x = 0; x < GridSize + 1; x++)
			{
				g.DrawLine(Pens.Black, x * CellSize, 0, x * CellSize, GridSize * CellSize);
			}
		}



		private void Grid_Resize(object sender, EventArgs e)
		{
			int height = this.Size.Width;

			this.Size = new Size(this.Size.Width, height);
		}
		#endregion

		#region Events
		private void Grid_MouseClick(object sender, MouseEventArgs e)
		{
			GridCell cell = GetCellFromClick(e.X, e.Y);
			if (cell == null)
			{
				return;
			}
			cell.ReverseCell();
		}
		private void Grid_MouseDown(object sender, MouseEventArgs e)
		{
			_drawing = true;
			GridCell cell = GetCellFromClick(e.X, e.Y);
			if (cell == null)
			{
				return;
			}
			_painting = !cell.Painted;
		}
		private void Grid_MouseMove(object sender, MouseEventArgs e)
		{
			if (_drawing)
			{
				GridCell cell = GetCellFromClick(e.X, e.Y);
				if (cell == null)
				{
					return;
				}
				if (_painting)
				{
					cell.PaintCell();
				}
				else
				{
					cell.UnpaintCell();
				}
			}
		}
		private void Grid_MouseUp(object sender, MouseEventArgs e)
		{
			_drawing = false;
		}
		#endregion

		#region Public Functions
		public void SaveGridImage()
		{

		}
		public void ClearGrid()
		{
			this.Controls.Clear();
			this.Refresh();
		}
		public void DrawInCell(int row, int col)
		{
			CellCollection[row, col].PaintCell();
		}
		public void DrawInCell(int row, int col, Color color)
		{
			CellCollection[row, col].PaintCell(color);
		}
		#endregion

		#region Private Functions
		private GridCell GetCellFromClick(int x, int y)
		{
			try
			{
				return CellCollection[y / CellSize, x / CellSize];
			}
			catch (Exception)
			{
				return null;
			}
		}
		#endregion

	}
}
