using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatrixLib
{
	public class Matrix
	{
		struct ValueAndPositionStruct
		{
			public double Value;
			public int Row;
			public int Column;
		}

		#region Properties
		public double[,] MatrixDefinition { get; set; }
		public int RowCount { get; private set; }
		public int ColumnCount { get; private set; } 
		#endregion

		#region Constructors
		public Matrix(int rows, int cols)
		{
			MatrixDefinition = new double[rows, cols];
			RowCount = rows;
			ColumnCount = cols;
		}
		public Matrix(double[,] matrix)
		{
			MatrixDefinition = matrix;
			RowCount = MatrixDefinition.GetLength(0);
			ColumnCount = MatrixDefinition.GetLength(1);
		} 
		/// <summary>
		/// Initializes the matrix with the passed in value
		/// </summary>
		/// <param name="value">Value to initialize the matrix with</param>
		public void Initialize(double value)
		{
			for (int row = 0; row < RowCount; row++)
			{
				for (int col = 0; col < ColumnCount; col++)
				{
					MatrixDefinition[row, col] = value;
				}
			}
		}
		#endregion

		#region Private Functions
		private bool CanGetDotProduct(Matrix mat1, Matrix mat2)
		{
			if (mat1.MatrixDefinition.GetLength(1) != mat2.MatrixDefinition.GetLength(0))
			{
				return false;
			}

			return true;
		}

		private bool CanAddMatrices(Matrix mat1, Matrix mat2)
		{
			if ((mat1.RowCount != mat2.RowCount) || (mat1.ColumnCount != mat2.ColumnCount))
			{
				return false;
			}

			return true;
		}
		#endregion

		#region Public Functions
		#region Matrix Functions
		public Matrix DotProduct(Matrix matrix2)
		{
			if (!CanGetDotProduct(this, matrix2))
			{
				throw new Exception($"Invalid inner sizes of matrices.\nThis matrice's columns ({MatrixDefinition.GetLength(1)}) does not equal the second matrice's rows ({matrix2.MatrixDefinition.GetLength(0)}).");
			}

			double[,] dotProd = new double[MatrixDefinition.GetLength(0), matrix2.MatrixDefinition.GetLength(1)];
			int innerSize = MatrixDefinition.GetLength(1);

			for (int row = 0; row < dotProd.GetLength(0); row++)
			{
				for (int col = 0; col < dotProd.GetLength(1); col++)
				{
					double sum = 0.0;
					for (int index = 0; index < innerSize; index++)
					{
						sum += (MatrixDefinition[row, index] * matrix2.MatrixDefinition[index, col]);
					}
					dotProd[row, col] = sum;
				}
			}

			return new Matrix(dotProd);
		}

		public Matrix Transpose()
		{
			Matrix transpose = new MatrixLib.Matrix(ColumnCount, RowCount);

			for (int row = 0; row < RowCount; row++)
			{
				for (int col = 0; col < ColumnCount; col++)
				{
					transpose.MatrixDefinition[col, row] = MatrixDefinition[row, col];
				}
			}

			return transpose;
		} 
		#endregion

		public Matrix CopyMatrix()
		{
			Matrix newMatrix = new MatrixLib.Matrix(RowCount, ColumnCount);
			for (int row = 0; row < RowCount; row++)
			{
				for (int col = 0; col < ColumnCount; col++)
				{
					newMatrix.MatrixDefinition[row, col] = MatrixDefinition[row, col];
				}
			}
			return newMatrix;
		}

		public double GetMaxValueAndPosition(out int row, out int col)
		{
			ValueAndPositionStruct max = new MatrixLib.Matrix.ValueAndPositionStruct() { Value = MatrixDefinition[0, 0] };

			foreach (ValueAndPositionStruct valueAndPosition in EnumerateMatrix())
			{
				if(valueAndPosition.Value > max.Value) { max = valueAndPosition; }
			}

			row = max.Row;
			col = max.Column;
			return max.Value;
		}

		public void TestYield()
		{
			foreach (ValueAndPositionStruct var in EnumerateMatrix())
			{
				MessageBox.Show($"Value: {var.Value}, Row: {var.Row}, Col: {var.Column}");
			}
		}

		private IEnumerable<ValueAndPositionStruct> EnumerateMatrix()
		{
			for (int row = 0; row < RowCount; row++)
			{
				for (int col = 0; col < ColumnCount; col++)
				{
					yield return new MatrixLib.Matrix.ValueAndPositionStruct() { Value = MatrixDefinition[row, col], Row = row, Column = col };
				}
			}
		}
		#endregion

		#region Operands
		public static Matrix operator +(Matrix mat1, Matrix mat2)
		{
			if ((mat1.RowCount != mat2.RowCount) || (mat1.ColumnCount != mat2.ColumnCount))
			{
				throw new Exception("Matrices aren't of equal size.");
			}

			Matrix result = new Matrix(mat1.RowCount, mat1.ColumnCount);

			for (int row = 0; row < mat1.RowCount; row++)
			{
				for (int col = 0; col < mat1.ColumnCount; col++)
				{
					result.MatrixDefinition[row, col] = mat1.MatrixDefinition[row, col] + mat2.MatrixDefinition[row, col];
				}
			}
			return result;
		}

		public static Matrix operator -(Matrix mat1, Matrix mat2)
		{
			if ((mat1.RowCount != mat2.RowCount) || (mat1.ColumnCount != mat2.ColumnCount))
			{
				throw new Exception("Matrices aren't of equal size.");
			}

			Matrix result = new Matrix(mat1.RowCount, mat1.ColumnCount);

			for (int row = 0; row < mat1.RowCount; row++)
			{
				for (int col = 0; col < mat1.ColumnCount; col++)
				{
					result.MatrixDefinition[row, col] = mat1.MatrixDefinition[row, col] - mat2.MatrixDefinition[row, col];
				}
			}
			return result;
		}

		public static Matrix operator *(Matrix mat1, Matrix mat2)
		{
			if ((mat1.RowCount != mat2.RowCount) || (mat1.ColumnCount != mat2.ColumnCount))
			{
				throw new Exception("Matrices aren't of equal size.");
			}

			Matrix result = new Matrix(mat1.RowCount, mat1.ColumnCount);

			for (int row = 0; row < mat1.RowCount; row++)
			{
				for (int col = 0; col < mat1.ColumnCount; col++)
				{
					result.MatrixDefinition[row, col] = mat1.MatrixDefinition[row, col] * mat2.MatrixDefinition[row, col];
				}
			}
			return result;
		}

		public static Matrix operator *(Matrix mat1, double doub)
		{
			Matrix result = new Matrix(mat1.RowCount, mat1.ColumnCount);

			for (int row = 0; row < mat1.RowCount; row++)
			{
				for (int col = 0; col < mat1.ColumnCount; col++)
				{
					result.MatrixDefinition[row, col] = mat1.MatrixDefinition[row, col] * doub;
				}
			}
			return result;
		}
		public static Matrix operator *(double doub, Matrix mat1)
		{
			Matrix result = new Matrix(mat1.RowCount, mat1.ColumnCount);

			for (int row = 0; row < mat1.RowCount; row++)
			{
				for (int col = 0; col < mat1.ColumnCount; col++)
				{
					result.MatrixDefinition[row, col] = mat1.MatrixDefinition[row, col] * doub;
				}
			}
			return result;
		}
		#endregion

	}
}
