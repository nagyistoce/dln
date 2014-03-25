// -----------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Eft.BioNN.Engine.Utils;

	/// <summary>
	/// Utility extension methods for lists (vectors) and matrices.
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Randomly shuffles a list.
		/// </summary>
		/// <typeparam name="T">The type of elements in the list.</typeparam>
		/// <param name="list">The list to shuffle.</param>
		/// <returns>An enumerator that traverses the list in a random order.</returns>
		/// <remarks>
		/// The following code is courtesy of Michael Herman (foson). See:
		/// http://stackoverflow.com/questions/375351/most-efficient-way-to-randomly-sort-shuffle-a-list-of-integers-in-c-sharp/
		/// </remarks>
		public static IEnumerable<T> AsRandom<T>(this IList<T> list)
		{
			int[] indexes = Enumerable.Range(0, list.Count).ToArray();
			Random generator = ThreadSafeRandom.GetThreadRandom();

			for (int i = 0; i < list.Count; ++i)
			{
				int position = generator.Next(i, list.Count);
				yield return list[indexes[position]];
				indexes[position] = indexes[i];
			}
		}

		/// <summary>
		/// Element-wise matrix subtraction.
		/// </summary>
		/// <param name="a">The first matrix.</param>
		/// <param name="b">The second matrix.</param>
		/// <param name="inPlace">If set to <c>true</c> then the matrix <paramref name="a"/> is modified.</param>
		/// <returns>A matrix containing the results.</returns>
		public static short[,] Subtract(this short[,] a, short[,] b, bool inPlace = false)
		{
			short[,] result;
			if (inPlace)
			{
				result = a;
			}
			else
			{
				result = new short[a.GetLength(0), a.GetLength(1)];
			}

			Parallel.For(0, a.GetLength(0), (i) => 
			{
				for (var j = 0; j < a.GetLength(1); j++)
				{
					result[i, j] = (short)(a[i, j] - b[i, j]);
				}
			});

			return result;
		}
	}
}
