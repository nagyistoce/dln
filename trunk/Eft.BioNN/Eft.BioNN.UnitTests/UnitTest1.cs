using System;
using Accord.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eft.BioNN.UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			double[] x = { 1.2, 1.4, 1.8 };
			double[] z = x.Add(0.3);
			Console.WriteLine(z.ToString(DefaultMatrixFormatProvider.CurrentCulture));
		}

		[TestMethod]
		public void TestComputeActivationProbabilities()
		{
			int n = 29;
			var result = ComputeActivationProbabilities(n, 0.6, 9);
			Console.WriteLine(result.ToString(DefaultMatrixFormatProvider.CurrentCulture));
			Assert.AreEqual(n + 1, result.Length);
			for (int i = 0; i < result.Length; i++)
			{
				Assert.IsTrue(result[i] >= 0.0, "Probability cannot be negative");
				Assert.IsTrue(result[i] <= 1.0, "Probability cannot be larger than 1");
				if (i > 0)
				{
					Assert.IsTrue(result[i] >= result[i - 1], "The probabilities should be non-decreasing");
				}
			}
			
		}

		private double[] ComputeActivationProbabilities(int n, double p, int threshold)
		{
			var result = new double[n + 1];
			for (int i = threshold; i < result.Length; i++)
			{
				for (int j = threshold; j <= i; j++)
				{
					var a = Choose(i, j);
					var b = Math.Pow(p, j);
					var c = Math.Pow(1 - p, i - j);
					Assert.IsTrue(a > 0);
					Assert.IsTrue(b > 0);
					Assert.IsTrue(c > 0);
					var delta = a * b * c;
					result[i] += delta;
				}
			}
			return result;
		}

		private long Choose(long n, long k)
		{
			long result = 1;

			for (long i = Math.Max(k, n - k) + 1; i <= n; ++i)
				result *= i;

			for (long i = 2; i <= Math.Min(k, n - k); ++i)
				result /= i;

			return result;
		}
	}
}
