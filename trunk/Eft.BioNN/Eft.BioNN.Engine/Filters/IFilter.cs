// -----------------------------------------------------------------------
// <copyright file="IFilter.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013-2014 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Filters
{
	using Eft.BioNN.Engine.Data;

	/// <summary>
	/// An interface for an input filter.
	/// </summary>
	public interface IFilter
	{
		/// <summary>
		/// Gets the data blowup factor for this particular filter.
		/// </summary>
		/// <value>
		/// The blowup factor.
		/// </value>
		int BlowupFactor { get; }

		/// <summary>
		/// Applies the filter to the supplied data.
		/// </summary>
		/// <param name="inputData">The input data.</param>
		void Process(Sample[] inputData);
	}
}
