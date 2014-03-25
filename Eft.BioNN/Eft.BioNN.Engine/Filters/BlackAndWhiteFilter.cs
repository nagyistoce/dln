// -----------------------------------------------------------------------
// <copyright file="BlackAndWhiteFilter.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013-2014 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Filters
{
	using System;
	using Eft.BioNN.Engine.Data;

	/// <summary>
	/// Converts the images to black-and-white by changing the pixel intensities to be either 0 or 255, 
	/// depending on which value the original pixel intensity is closer to.
	/// </summary>
	public class BlackAndWhiteFilter : AbstractFilter
	{
		/// <summary>
		/// Applies the filter to an individual sample from the input data set.
		/// </summary>
		/// <param name="sample">The sample from the data set.</param>
		protected override void ProcessSampleData(Sample sample)
		{
			for (int i = 0; i < sample.SampleData.Length; i++)
			{
				sample.SampleData[i] = (sample.SampleData[i] > byte.MaxValue / 2) ? byte.MaxValue : byte.MinValue;
			}
		}
	}
}
