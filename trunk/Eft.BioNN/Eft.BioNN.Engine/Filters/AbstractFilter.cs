// -----------------------------------------------------------------------
// <copyright file="AbstractFilter.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013-2014 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Filters
{
	using System.Threading.Tasks;
	using Eft.BioNN.Engine.Data;

	/// <summary>
	/// An abstract base class for input filters
	/// </summary>
	public abstract class AbstractFilter : IFilter
	{
		/// <summary>
		/// Gets the data blowup factor for this particular filter.
		/// </summary>
		/// <value>
		/// The blowup factor.
		/// </value>
		public virtual int BlowupFactor 
		{ 
			get
			{
				return 1;
			}
		}

		/// <summary>
		/// Applies the filter to the supplied data.
		/// </summary>
		/// <param name="inputData">The input data.</param>
		public void Process(Sample[] inputData)
		{
			//
			// Process each sample in parallel and delegate the actual filtering to the subclass.
			//
			Parallel.ForEach(
				inputData,
				(sample) =>
				{
					this.ProcessSampleData(sample);
				});
		}

		/// <summary>
		/// Applies the filter to an individual sample from the input data set.
		/// </summary>
		/// <param name="sample">The sample from the data set.</param>
		protected abstract void ProcessSampleData(Sample sample);

		/// <summary>
		/// Concatenates two arrays.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the arrays.</typeparam>
		/// <param name="arrayA">The first array.</param>
		/// <param name="arrayB">The second array.</param>
		/// <returns>A concatenation of <paramref name="arrayA"/> and <paramref name="arrayB"/></returns>
		protected T[] ConcatenateArrays<T>(T[] arrayA, T[] arrayB)
		{
			T[] result = new T[arrayA.Length + arrayB.Length];
			arrayA.CopyTo(result, 0);
			arrayB.CopyTo(result, arrayA.Length);
			return result;
		}
	}
}
