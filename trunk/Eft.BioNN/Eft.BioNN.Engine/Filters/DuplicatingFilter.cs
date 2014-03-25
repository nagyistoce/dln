// -----------------------------------------------------------------------
// <copyright file="DuplicatingFilter.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013-2014 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Filters
{
	using System;
	using Eft.BioNN.Engine.Data;

	/// <summary>
	/// A filter that doesn't do anything besides duplicating the input.
	/// </summary>
	public class DuplicatingFilter : AbstractFilter
	{
		/// <summary>
		/// Gets the data blowup factor for this particular filter.
		/// </summary>
		/// <value>
		/// The blowup factor.
		/// </value>
		public override int BlowupFactor
		{
			get
			{
				return 2;
			}
		}

		/// <summary>
		/// Applies the filter to an individual sample from the input data set.
		/// </summary>
		/// <param name="sample">The sample from the data set.</param>
		protected override void ProcessSampleData(Sample sample)
		{
			//
			// Duplicate the image
			//
			sample.SampleData = this.ConcatenateArrays(sample.SampleData, sample.SampleData);
		}
	}
}
