// -----------------------------------------------------------------------
// <copyright file="OnAndOffCenterSurround.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013-2014 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Filters
{
	using System;
	using Eft.BioNN.Engine.Data;

	/// <summary>
	/// A filter that simulates both the ON and OFF center-surround ganglion cells in the retina.
	/// See <a href="http://en.wikipedia.org/wiki/Receptive_field#Retinal_ganglion_cells">Wikipedia article on center-surround cells</a>.
	/// </summary>
	public class OnAndOffCenterSurround : OnCenterSurround
	{
		/// <summary>
		/// The excitatory multiplication factor (for the surround photoreceptors)
		/// </summary>
		private const double Excitatory = 0.3;

		/// <summary>
		/// The inhibitory multiplication factor (for the center photoreceptors)
		/// </summary>
		private const double Inhibitory = -Excitatory * 8.0 / 1.1;

		/// <summary>
		/// The mask used to simulate the OFF center-surround cell.
		/// </summary>
		private double[,] offCenterSurroundMask = new double[,] 
		{ 
			{ Excitatory, Excitatory, Excitatory },
			{ Excitatory, Inhibitory, Excitatory },
			{ Excitatory, Excitatory, Excitatory }
		};

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
			// First simulate the ON cells (using the mask from the superclass)
			//
			var onData = this.MaskImage(sample.SampleData, this.OnCenterSurroundMask);
			//
			// Now simulate the OFF cells (using the mask from this class)
			//
			var offData = this.MaskImage(sample.SampleData, this.offCenterSurroundMask);
			//
			// Combine the two filtered images (NOTE: this doubles the input size!)
			//
			sample.SampleData = this.ConcatenateArrays(onData, offData);
		}
	}
}
