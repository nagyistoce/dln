// -----------------------------------------------------------------------
// <copyright file="Sample.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Data
{
	using System;

	/// <summary>
	/// An unlabelled sample.
	/// </summary>
	public class Sample
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Sample"/> class.
		/// </summary>
		/// <param name="sampleData">The sample data.</param>
		public Sample(byte[] sampleData)
		{
			this.SampleData = sampleData;
		}

		/// <summary>
		/// Gets or sets the sample data.
		/// </summary>
		/// <value>
		/// The sample data.
		/// </value>
		public byte[] SampleData { get; set; }
	}
}
