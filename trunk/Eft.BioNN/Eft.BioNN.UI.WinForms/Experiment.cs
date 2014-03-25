// -----------------------------------------------------------------------
// <copyright file="Experiment.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.UI.WinForms
{
	using Eft.BioNN.Engine.Learning;

	/// <summary>
	/// Contains information about an experiment.
	/// </summary>
	public class Experiment
	{
		/// <summary>
		/// Gets or sets the parameters of the neural network.
		/// </summary>
		/// <value>
		/// The neural network parameters.
		/// </value>
		public NeuralNetworkCreationParameters NetworkParameters
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the number of unsupervised training epochs.
		/// </summary>
		/// <value>
		/// The number of unsupervised epochs.
		/// </value>
		public int UnsupervisedEpochs
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the number of supervised training epochs.
		/// </summary>
		/// <value>
		/// The number of supervised training epochs.
		/// </value>
		public int SupervisedEpochs
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the probability that an edge/threshold is updated during training.
		/// </summary>
		/// <value>
		/// The learning probability.
		/// </value>
		public double LearningProbability
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the supervised learning mode.
		/// </summary>
		/// <value>
		/// The supervised learning mode.
		/// </value>
		public ContrastiveDivergenceLearningMode SupervisedLearningMode
		{
			get;
			set;
		}
	}
}
