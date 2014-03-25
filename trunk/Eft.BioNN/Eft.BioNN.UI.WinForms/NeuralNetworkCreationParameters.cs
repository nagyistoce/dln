// -----------------------------------------------------------------------
// <copyright file="NeuralNetworkCreationParameters.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.UI.WinForms
{
	/// <summary>
	/// Contains the parameter values used in the construction of a neural network.
	/// </summary>
	public class NeuralNetworkCreationParameters
	{
		/// <summary>
		/// Gets or sets the number of output units per class.
		/// </summary>
		/// <value>
		/// The number of output units per class.
		/// </value>
		public int UnitsPerClass
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets an array with the number of units in each of the hidden layers.
		/// </summary>
		/// <value>
		/// An array with the number of units in each of the hidden layers.
		/// </value>
		public int[] HiddenLayerSizes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the activation threshold.
		/// </summary>
		/// <value>
		/// The activation threshold.
		/// </value>
		public int ActivationThreshold
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the probability that a synapse (connection/edge) works.
		/// </summary>
		/// <value>
		/// The probability that a synapse (connection/edge) works.
		/// </value>
		public double SynapseSuccessProbability
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the minimum weight.
		/// </summary>
		/// <value>
		/// The minimum weight.
		/// </value>
		public int MinWeight
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the maximum weight.
		/// </summary>
		/// <value>
		/// The maximum weight.
		/// </value>
		public int MaxWeight
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the training algorithm to use for training the network.
		/// </summary>
		/// <value>
		/// The training algorithm.
		/// </value>
		public TrainingAlgorithm TrainingAlgorithm
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the number of samples to take in order to estimate activation probabilities.
		/// </summary>
		/// <value>
		/// The number of samples to take in order to estimate activation probabilities.
		/// </value>
		public int ActivationSamples
		{
			get;
			set;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			string prefix = null;
			switch (this.TrainingAlgorithm)
			{
				case WinForms.TrainingAlgorithm.ContrastiveDivergence:
					prefix = "CD";
					break;
				case WinForms.TrainingAlgorithm.ContrastiveDivergenceWithSampling:
					prefix = "CDS";
					break;
				case WinForms.TrainingAlgorithm.PersistentContrastiveDivergence:
					prefix = "PCD";
					break;
			}

			return string.Format(
				"{7} {0}l {1} upc{2} p{3:0.0} t{4} w{5}..{6}", 
				1 + this.HiddenLayerSizes.Length, 
				string.Join("-", this.HiddenLayerSizes),
				this.UnitsPerClass,
				this.SynapseSuccessProbability,
				this.ActivationThreshold,
				this.MinWeight,
				this.MaxWeight,
				prefix);
		}
	}
}
