// -----------------------------------------------------------------------
// <copyright file="INeuralNetwork.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using Eft.BioNN.Engine.Data;

	/// <summary>
	/// An interface for a neural network.
	/// </summary>
	public interface INeuralNetwork
	{
		/// <summary>
		/// Gets the number of layers in the network, including the input and output layers.
		/// </summary>
		/// <value>
		/// The total number of layers.
		/// </value>
		int LayerCount { get; }

		/// <summary>
		/// Gets the total 'signal' required for a unit to become active.
		/// </summary>
		/// <value>
		/// The activation threshold.
		/// </value>
		int ActivationThreshold { get; }

		/// <summary>
		/// Gets the probability that a synapse (connection/edge) works.
		/// </summary>
		/// <value>
		/// The synapse success probability.
		/// </value>
		double SynapseSuccessProbability { get; }

		/// <summary>
		/// Gets the maximum weight that can be assigned to a connection.
		/// </summary>
		/// <value>
		/// The maximum weight.
		/// </value>
		int MaxWeight { get; }

		/// <summary>
		/// Gets the minimum weight that can be assigned to a connection.
		/// </summary>
		/// <value>
		/// The minimum weight.
		/// </value>
		int MinWeight { get; }

		/// <summary>
		/// Gets the maximum number of activated input units.
		/// </summary>
		/// <value>
		/// The maximum number of activated input units.
		/// </value>
		int MaxInputActivations { get; }

		/// <summary>
		/// Gets or sets the probability that an edge/threshold is updated during training.
		/// </summary>
		/// <value>
		/// The learning probability.
		/// </value>
		double LearningProbability { get; set; }

		/// <summary>
		/// Gets the number of units in the specified <paramref name="layer"/>
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <returns>The number of units.</returns>
		int GetUnitCount(int layer);

		/// <summary>
		/// Trains the network in an unsupervised fashion.
		/// </summary>
		/// <param name="samples">The samples to use for training.</param>
		/// <param name="progressObserver">A delegate to an observer function which tracks the training progress.</param>
		void TrainUnsupervised(Sample[] samples, Action<int> progressObserver);

		/// <summary>
		/// Trains the network in a supervised fashion.
		/// </summary>
		/// <param name="samples">The (labelled) samples to use for training.</param>
		/// <param name="progressObserver">A delegate to an observer function which tracks the training progress.</param>
		/// <param name="options">This parameter can be used to provide additional training options.</param>
		void TrainSupervised(LabelledSample[] samples, Action<int> progressObserver, object options);

		/// <summary>
		/// Classifies the specified sample.
		/// </summary>
		/// <param name="sample">The sample.</param>
		/// <returns>The class that the network believes the sample belongs to.</returns>
		int Classify(Sample sample);

		/// <summary>
		/// Classifies the specified sample.
		/// </summary>
		/// <param name="sample">The sample.</param>
		/// <param name="counts">The number of neurons active in each label group.</param>
		/// <returns>The class that the network believes the sample belongs to.</returns>
		int Classify(Sample sample, out int[] counts);

		/// <summary>
		/// Construct a 'heat map' of sorts that can be used to visualize how much the specified unit(s) like or dislike each an every input unit.
		/// </summary>
		/// <param name="layer">The layer of the unit(s) for which to generate the reconstruction.</param>
		/// <param name="mask">A mask which specifies for which neuron(s) we wish to generate the reconstruction.</param>
		/// <returns>An array which has the same size and the input layer. The higher the number, the more the specified units like that particular input unit to be on.</returns>
		int[] Reconstruct(int layer, int[] mask);

		/// <summary>
		/// Saves the neural network to the supplied stream.
		/// </summary>
		/// <param name="stream">The stream in which to write the neural network.</param>
		void Save(Stream stream);

		/// <summary>
		/// Generates the visible states by performing a random walk, which consists of propagating activations right and left
		/// starting at the specified <paramref name="generationLayer" />.
		/// </summary>
		/// <param name="generationLayer">The generation layer.</param>
		/// <param name="numberOfSamples">The number of samples to take in order to estimate the activation probabilities in the visible (i.e. input) layer.</param>
		/// <returns>An infinite sequence of activation probabilities for units in the visible layer.</returns>
		IEnumerable<double[]> GenerateVisibleStates(int generationLayer, int numberOfSamples);

		/// <summary>
		/// Returns a single string that contains the activation threshold of every individual unit. 
		/// The string is formatted in such a way that it can be copied into Matlab.
		/// </summary>
		/// <returns>A string with the activation thresholds.</returns>
		string ThresholdsToString();

		/// <summary>
		/// Gets the Euclidian distance between the weight vectors of pairs of output neurons belonging to the same class.
		/// </summary>
		/// <returns>A three dimensional array, indexed by class, unit, unit</returns>
		double[][][] GetPairwiseWeightDistancesBetweenLabelUnits();

		/// <summary>
		/// Calculates the average reconstruction error when an attempt is made to reconstruct the entire label by 
		/// activating a portion of it (the seed) and then doing one full propagation step.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="seedSize">The size of the seed.</param>
		/// <returns>The average reconstruction error.</returns>
		double CheckLabelReconstruction(int label, int seedSize);

		/// <summary>
		/// Gets all connection weights in a single connection layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <returns>An array of weights.</returns>
		/// <remarks>
		/// It would be more appropriate to return integers. Double is used for convenience when doing standard deviation calculations.
		/// </remarks>
		double[] GetAllWeights(int layer);
	}
}
