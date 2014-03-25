// -----------------------------------------------------------------------
// <copyright file="Utils.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.UI.WinForms
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using Eft.BioNN.Engine.Network;

	/// <summary>
	/// Utility methods for the UI.
	/// </summary>
	internal class Utils
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="Utils"/> class from being created.
		/// </summary>
		private Utils()
		{
		}

		/// <summary>
		/// Draws a digit image and places in the specified picture box.
		/// </summary>
		/// <param name="data">The pixel data.</param>
		/// <param name="pictureBox">The picture box.</param>
		/// <param name="width">The width of the image.</param>
		/// <param name="height">The height of the image.</param>
		public static void DrawDigit(byte[] data, PictureBox pictureBox, int width, int height)
		{
			//
			// Grab hold of the current image
			//
			var oldImage = pictureBox.Image;
			//
			// See if we can re-use the existing image
			//
			var bitmap = oldImage as Bitmap;
			bool reusingBitmap = false;
			if (bitmap == null || bitmap.Width != width || bitmap.Height != height)
			{
				//
				// Nope, create a new bitmap.
				//
				bitmap = new Bitmap(width, height);
			}
			else
			{
				//
				// Re-use the existing bitmap
				//
				reusingBitmap = true;
			}
			//
			// Draw the image
			//
			var index = 0;
			for (var y = 0; y < bitmap.Height; y++)
			{
				for (var x = 0; x < bitmap.Width; x++)
				{
					bitmap.SetPixel(x, y, Color.FromArgb(data[index], data[index], data[index]));
					index++;
				}
			}
			//
			// If we created a new bitmap, replace the existing image
			//
			if (!reusingBitmap)
			{
				pictureBox.Image = bitmap;
				//
				// Dispose of the previous image
				//
				if (oldImage != null)
				{
					oldImage.Dispose();
				}
			}
			else
			{
				//
				// Otherwise just invalidate the picture box in order to cause it to be repainted
				//
				pictureBox.Invalidate();
			}
		}

		/// <summary>
		/// Creates a weight probability distribution where the total probability of inhibitory (respectively excitatory) weights is divide equally 
		/// among the inhibitory (respectively excitatory) weights.
		/// </summary>
		/// <param name="minWeight">The minimum weight.</param>
		/// <param name="maxWeight">The maximum weight.</param>
		/// <param name="inhibitoryProbability">The total probability of inhibitory weights.</param>
		/// <param name="excitatoryProbability">The total probability of excitatory weights.</param>
		/// <returns>A weight probability distribution.</returns>
		internal static double[] SpreadWeightProbabilities(int minWeight, int maxWeight, double inhibitoryProbability, double excitatoryProbability)
		{
			double neutralProbability = 1.0 - inhibitoryProbability - excitatoryProbability;
			var result = new double[maxWeight - minWeight + 1];
			if (minWeight >= 0)
			{
				excitatoryProbability += inhibitoryProbability;
			}
			else if (maxWeight <= 0)
			{
				inhibitoryProbability += excitatoryProbability;
			}

			for (int i = 0; i < result.Length; i++)
			{
				int weight = minWeight + i;
				if (weight < 0)
				{
					result[i] = inhibitoryProbability / -minWeight;
				}
				else if (weight > 0)
				{
					result[i] = excitatoryProbability / maxWeight;
				}
				else
				{
					result[i] = neutralProbability;
				}
			}

			return result;
		}

		/// <summary>
		/// Constructs a neural network.
		/// </summary>
		/// <param name="networkParameters">The construction parameter values.</param>
		/// <param name="numberOfClasses">The number of classes in the training/testing data set.</param>
		/// <param name="numberOfInputs">The number of inputs units (i.e. the size of the input data vectors).</param>
		/// <returns>A neural network.</returns>
		internal static INeuralNetwork ConstructNetwork(NeuralNetworkCreationParameters networkParameters, int numberOfClasses, int numberOfInputs)
		{
			//
			// TODO: Just pass the dataprovider to this method instead of the last two parameters
			//
			const int maxActivations = 10000;
			//
			// Get the number of units in each of the hidden layers
			//
			var hiddenUnits = networkParameters.HiddenLayerSizes;
			//
			// Generate the weight distribution
			//
			var weightDistribution = Utils.SpreadWeightProbabilities(networkParameters.MinWeight, networkParameters.MaxWeight, 0.05, 0.15);
			//
			// Prepend the input units
			//
			var allUnits = new int[hiddenUnits.Length + 1];
			allUnits[0] = numberOfInputs;
			Array.Copy(hiddenUnits, 0, allUnits, 1, hiddenUnits.Length);

			switch (networkParameters.TrainingAlgorithm)
			{
				case TrainingAlgorithm.ContrastiveDivergence:
					return new ContrastiveDivergenceNetwork<InterLayerAdjacencyMatrix>(
						networkParameters.MinWeight,
						networkParameters.MaxWeight,
						weightDistribution,
						networkParameters.SynapseSuccessProbability,
						maxActivations,
						networkParameters.ActivationThreshold,
						numberOfClasses,
						networkParameters.UnitsPerClass,
						allUnits);
				case TrainingAlgorithm.ContrastiveDivergenceWithSampling:
					return new ContrastiveDivergenceSamplingNetwork<InterLayerAdjacencyMatrix>(
						networkParameters.MinWeight,
						networkParameters.MaxWeight,
						weightDistribution,
						networkParameters.SynapseSuccessProbability,
						maxActivations,
						networkParameters.ActivationThreshold,
						numberOfClasses,
						networkParameters.UnitsPerClass,
						networkParameters.ActivationSamples,
						allUnits);
				case TrainingAlgorithm.PersistentContrastiveDivergence:
					return new PersistentContrastiveDivergenceSamplingNetwork<InterLayerAdjacencyMatrix>(
						networkParameters.MinWeight,
						networkParameters.MaxWeight,
						weightDistribution,
						networkParameters.SynapseSuccessProbability,
						maxActivations,
						networkParameters.ActivationThreshold,
						numberOfClasses,
						networkParameters.UnitsPerClass,
						networkParameters.ActivationSamples,
						1,
						allUnits);
			}

			return null;
		}
	}
}
