using System;
using System.Globalization;

namespace DeepLearnCS
{
	public static class ManagedMatrix
	{
		public static CultureInfo ci = new CultureInfo("en-us");

		// ------------------------------------------------------------------------------------
		// Print Matrix Elements
		// ------------------------------------------------------------------------------------
		public static void PrintList(ManagedIntList input, bool vert = false)
		{
			for (int x = 0; x < input.x; x++)
			{
				if (!vert)
				{
					if (x > 0)
						Console.Write(" ");
				}
				else
				{
					Console.Write("{0}: ", x.ToString("D", ci));
				}

				Console.Write("{0}", input[x].ToString("D", ci));

				if (vert)
					Console.WriteLine();
			}

			if (!vert)
				Console.WriteLine();
		}

		public static void Print2D(ManagedArray input)
		{
			for (int y = 0; y < input.y; y++)
			{
				Console.Write("{0}: ", y.ToString("D", ci));

				for (int x = 0; x < input.x; x++)
				{
					if (x > 0)
					{
						Console.Write(" ");
					}

					Console.Write("{0}", input[x, y].ToString("G2", ci));
				}

				Console.WriteLine();
			}
		}

		public static void Print3D(ManagedArray input)
		{
			for (int z = 0; z < input.z; z++)
			{
				Console.Write("[, , {0}]\n", z.ToString("D", ci));

				for (int y = 0; y < input.y; y++)
				{
					Console.Write("{0}: ", y.ToString("D", ci));

					for (int x = 0; x < input.x; x++)
					{
						if (x > 0)
						{
							Console.Write(" ");
						}

						Console.Write("{0}", input[x, y, z].ToString("G2", ci));
					}

					Console.WriteLine();
				}
			}
		}

		public static void Print4D(ManagedArray input, int i)
		{
			for (int z = 0; z < input.z; z++)
			{
				Console.Write("[, , {0}]\n", z.ToString("D", ci));

				for (int y = 0; y < input.y; y++)
				{
					Console.Write("{0}: ", y.ToString("D", ci));

					for (int x = 0; x < input.x; x++)
					{
						if (x > 0)
						{
							Console.Write(" ");
						}

						Console.Write("{0}", input[((i * input.z + z) * input.y + y) * input.x + x].ToString("G2", ci));
					}

					Console.WriteLine();
				}
			}
		}

		public static void Print4DIJ(ManagedArray input, int i, int j)
		{
			var size2D = input.x * input.y;
			var srcoffset = (i * input.j + j) * size2D;

			for (int y = 0; y < input.y; y++)
			{
				Console.Write("{0}: ", y.ToString("D", ci));

				for (int x = 0; x < input.x; x++)
				{
					if (x > 0)
					{
						Console.Write(" ");
					}

					Console.Write("{0}", input[srcoffset + y * input.x + x].ToString("G2", ci));
				}

				Console.WriteLine();
			}
		}

		// ------------------------------------------------------------------------------------
		// Matrix Operations
		// ------------------------------------------------------------------------------------

		// 2D Matrix transposition
		public static void Transpose(ManagedArray dst, ManagedArray src)
		{
			dst.Resize(src.y, src.x, false);

			for (int y = 0; y < src.y; y++)
			{
				for (int x = 0; x < src.x; x++)
				{
					dst[y, x] = src[x, y];
				}
			}
		}

		// 2D Matrix multiplication
		public static void Multiply(ManagedArray result, ManagedArray A, ManagedArray B)
		{
			if (A.x == B.y)
			{
				result.Resize(B.x, A.y, false);

				for (int y = 0; y < A.y; y++)
				{
					for (int x = 0; x < B.x; x++)
					{
						result[x, y] = 0.0;

						for (int k = 0; k < A.x; k++)
						{
							result[x, y] = result[x, y] + A[k, y] * B[x, k];
						}
					}
				}
			}
			else
			{
				Console.WriteLine("Incompatible dimensions");
			}
		}

		// Element by element multiplication
		public static void Multiply(ManagedArray A, ManagedArray B)
		{
			for (int x = 0; x < A.Length(); x++)
			{
				A[x] = A[x] * B[x];
			}
		}

		// Matrix Addition
		public static void Add(ManagedArray A, ManagedArray B, double Scale = 1.0)
		{
			for (int x = 0; x < A.Length(); x++)
			{
				A[x] = A[x] + Scale * B[x];
			}
		}

		// Matrix * Constant Multiplication
		public static void Multiply(ManagedArray A, double B)
		{
			for (int x = 0; x < A.Length(); x++)
			{
				A[x] = A[x] * B;
			}
		}

		// Matrix + Constant Addition
		public static void Add(ManagedArray A, double B)
		{
			for (int x = 0; x < A.Length(); x++)
			{
				A[x] = A[x] + B;
			}
		}

		// Matrix Summation
		public static double Sum(ManagedArray A)
		{
			var sum = 0.0;

			for (int x = 0; x < A.Length(); x++)
			{
				sum += A[x];
			}

			return sum;
		}

		// get sum of squares of each element
		public static double SquareSum(ManagedArray A)
		{
			var sum = 0.0;

			for (int x = 0; x < A.Length(); x++)
			{
				sum += A[x] * A[x];
			}

			return sum;
		}

		// Matrix mean of 2D Array along a dimension
		public static void Mean(ManagedArray dst, ManagedArray src, int dim)
		{
			if (dim == 1)
			{
				dst.Resize(src.x, 1, false);

				for (int x = 0; x < src.x; x++)
				{
					var sum = 0.0;

					for (int y = 0; y < src.y; y++)
					{
						sum += src[x, y];
					}

					dst[x] = sum / src.y;
				}
			}
			else
			{
				dst.Resize(1, src.y, false);

				for (int y = 0; y < src.y; y++)
				{
					double sum = 0.0;

					for (int x = 0; x < src.x; x++)
					{
						sum += src[x, y];
					}

					dst[y] = sum / src.x;
				}
			}
		}

		// sigmoid function
		public static double Sigmoid(double x)
		{
			return 1.0 / (1.0 + Math.Exp(-x));
		}

		// Get element per element difference between arrays
		public static ManagedArray Diff(ManagedArray A, ManagedArray B)
		{
			var result = new ManagedArray(A, false);

			for (int x = 0; x < A.Length(); x++)
			{
				result[x] = A[x] - B[x];
			}

			return result;
		}

		// Apply sigmoid function to matrix
		public static ManagedArray Sigm(ManagedArray A)
		{
			var result = new ManagedArray(A, false);

			for (int x = 0; x < A.Length(); x++)
			{
				result[x] = Sigmoid(A[x]);
			}

			return result;
		}

		// Apply delta sigmoid function to matrix
		public static ManagedArray DSigm(ManagedArray A)
		{
			var result = new ManagedArray(A, false);

			for (int x = 0; x < A.Length(); x++)
			{
				var sigmoid = Sigmoid(A[x]);

				result[x] = sigmoid * (1.0 - sigmoid);
			}

			return result;
		}

		// Combine two arrays column-wise
		public static ManagedArray CBind(ManagedArray A, ManagedArray B)
		{
			if (A.y == B.y)
			{
				var resultx = A.x + B.x;
				var resulty = A.y;

				var result = new ManagedArray(resultx, resulty, false);

				ManagedOps.Copy2DOffset(result, A, 0, 0);
				ManagedOps.Copy2DOffset(result, B, A.x, 0);

				return result;
			}

			return null;
		}

		// Flip Matrix along a dimension
		public static void Flip(ManagedArray dst, ManagedArray src, int FlipDim)
		{
			dst.Resize(src.x, src.y, src.z, false);

			for (int z = 0; z < src.z; z++)
			{
				for (int y = 0; y < src.y; y++)
				{
					for (int x = 0; x < src.x; x++)
					{
						switch (FlipDim)
						{
							case 0:
								dst[x, y, z] = src[src.x - x - 1, y, z];
								break;
							case 1:
								dst[x, y, z] = src[x, src.y - y - 1, z];
								break;
							case 2:
								dst[x, y, z] = src[x, y, src.z - z - 1];
								break;
							default:
								dst[x, y, z] = src[src.x - x - 1, y, z];
								break;
						}
					}
				}
			}
		}

		// Flip 3D Matrix along a dimension
		public static void FlipAll(ManagedArray dst, ManagedArray src)
		{
			dst.Resize(src.x, src.y, src.z, false);

			var tmp = new ManagedArray(src.x, src.y, src.z, false);

			ManagedOps.Copy3D(tmp, src, 0, 0, 0);

			for (int FlipDim = 0; FlipDim < 3; FlipDim++)
			{
				Flip(dst, tmp, FlipDim);

				ManagedOps.Copy3D(tmp, dst, 0, 0, 0);
			}

			ManagedOps.Free(tmp);
		}

		// Rotate a 2D matrix
		public static void Rotate180(ManagedArray dst, ManagedArray src)
		{
			dst.Resize(src.x, src.y);

			var tmp = new ManagedArray(src.x, src.y, false);

			ManagedOps.Copy2D(tmp, src, 0, 0);

			for (int FlipDim = 0; FlipDim < 2; FlipDim++)
			{
				Flip(dst, tmp, FlipDim);

				ManagedOps.Copy2D(tmp, dst, 0, 0);
			}

			ManagedOps.Free(tmp);
		}

		// Expand a matrix A[x][y] by [ex][ey]
		public static void Expand(ManagedArray A, int expandx, int expandy, ManagedArray output)
		{
			var outputx = A.x * expandx;
			var outputy = A.y * expandy;

			output.Resize(outputx, outputy, false);

			for (int y = 0; y < A.y; y++)
			{
				for (int x = 0; x < A.x; x++)
				{
					for (int SZy = 0; SZy < expandy; SZy++)
					{
						for (int SZx = 0; SZx < expandx; SZx++)
						{
							output[x * expandx + SZx, y * expandy + SZy] = A[x, y];
						}
					}
				}
			}
		}

		// Create a 2D Diagonal/Identity matrix of size [dim][dim]
		public static ManagedArray Diag(int dim)
		{
			if (dim > 0)
			{
				var result = new ManagedArray(dim, dim, false);

				for (int y = 0; y < dim; y++)
				{
					for (int x = 0; x < dim; x++)
					{
						result[x, y] = (x == y) ? 1.0 : 0.0;
					}
				}

				return result;
			}

			return null;
		}
	}
}
