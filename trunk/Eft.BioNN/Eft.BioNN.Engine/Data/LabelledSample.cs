// -----------------------------------------------------------------------
// <copyright file="LabelledSample.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Data
{
	using System;

	/// <summary>
	/// A sample with a label
	/// </summary>
	public class LabelledSample : Sample
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LabelledSample"/> class.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="sampleData">The sample data.</param>
		public LabelledSample(int label, byte[] sampleData) : base(sampleData)
		{
			this.Label = label;
		}

		/// <summary>
		/// Gets or sets the label.
		/// </summary>
		/// <value>
		/// The label.
		/// </value>
		public int Label { get; set; }
	}
}
