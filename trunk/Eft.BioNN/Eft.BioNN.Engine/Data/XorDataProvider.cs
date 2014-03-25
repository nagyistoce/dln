// -----------------------------------------------------------------------
// <copyright file="XorDataProvider.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Data
{
	using System;
	using Eft.BioNN.Engine.Utils;

	/// <summary>
	/// Generates random XOR data samples.
	/// </summary>
	public class XorDataProvider : IDataProvider
	{
		/// <summary>
		/// Gets the number of classes in the data set.
		/// </summary>
		/// <value>
		/// The number of classes.
		/// </value>
		public int NumberOfClasses
		{
			get 
			{
				return 2;
			}
		}

		/// <summary>
		/// Gets the training set.
		/// </summary>
		/// <returns>
		/// An array of samples to use for training.
		/// </returns>
		public LabelledSample[] GetTrainingSet()
		{
			return this.GenerateSamples(1000);
		}

		/// <summary>
		/// Gets the testing set.
		/// </summary>
		/// <returns>
		/// An array of samples to use for testing.
		/// </returns>
		public LabelledSample[] GetTestingSet()
		{
			return this.GenerateSamples(100);
		}

		/// <summary>
		/// Generates random XOR samples.
		/// </summary>
		/// <param name="count">The number of samples to generate.</param>
		/// <returns>The generated samples, labeled as 0 if the XOR of the two bits is false and 1 otherwise.</returns>
		/// <remarks>
		/// The visualization mechanism in the GUI assumes data to consist of square pictures. Each bit is therefore duplicated, 
		/// resulting in each sample having size 4. Since the sample data is of type <code>byte[]</code>, every 1-bit is represented with 
		/// <see cref="byte.MaxValue"/>.</remarks>
		private LabelledSample[] GenerateSamples(int count)
		{
			var random = ThreadSafeRandom.GetThreadRandom();
			var result = new LabelledSample[count];
			var bits = new bool[2];
			for (int i = 0; i < count; i++)
			{
				//
				// Get two random bits
				//
				bits[0] = random.NextDouble() > 0.5 ? true : false;
				bits[1] = random.NextDouble() > 0.5 ? true : false;
				//
				// Are both bits zero?
				//
				if (!bits[0] && !bits[1])
				{
					//
					// The current design of our neural nets makes it impossible to recognize an all-zero data vector since no 
					// activations can result from it. For now, we work around this issue by 
					//
					bits[0] = true;
					bits[1] = true;
				}
				//
				// Populate the sample data array
				//
				var sampleData = new byte[4];
				sampleData[0] = sampleData[1] = bits[0] ? byte.MaxValue : byte.MinValue;
				sampleData[2] = sampleData[3] = bits[1] ? byte.MaxValue : byte.MinValue;
				//
				// Calculate the label
				//
				int label = bits[0] ^ bits[1] ? 1 : 0;
				//
				// Add this sample to the resulting array
				//
				result[i] = new LabelledSample(label, sampleData);
			}

			return result;
		}
	}
}
