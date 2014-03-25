// -----------------------------------------------------------------------
// <copyright file="MultiLayerNetwork.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using Eft.BioNN.Engine.Learning;

	/// <summary>
	/// A multi-layer neural network, trainable with an arbitrary training method.
	/// </summary>
	/// <typeparam name="T">The type of class to use for storing connection (edge) information.</typeparam>
	/// <remarks>
	/// This class was created before inhibitory edges and faulty synapses were added to the model.
	/// </remarks>
	public class MultiLayerNetwork<T> : AbstractNeuralNetwork<T> where T : AbstractInterLayerConnections
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MultiLayerNetwork{T}" /> class.
		/// </summary>
		/// <param name="learningAlgorithm">The learning algorithm.</param>
		/// <param name="minWeight">The minimum weight.</param>
		/// <param name="maxWeight">The maximum weight.</param>
		/// <param name="weightProbabilities">The weight probability distribution.</param>
		/// <param name="maxInputActivations">The maximum number of activated input units.</param>
		/// <param name="activationThreshold">The total 'signal' required for a unit to become active.</param>
		/// <param name="classes">The number of distinct classes.</param>
		/// <param name="unitsPerClass">The number of output units per class.</param>
		/// <param name="units">The number of units in each layer, excluding the output layer.</param>
		public MultiLayerNetwork(
			ILearningAlgorithm learningAlgorithm, 
			int minWeight,
			int maxWeight,
			double[] weightProbabilities,
			int maxInputActivations, 
			int activationThreshold, 
			int classes, 
			int unitsPerClass, 
			params int[] units)
			: base(
			learningAlgorithm, 
			minWeight,
			maxWeight,
			weightProbabilities,
			1.0,						// Synapse success probability
			maxInputActivations, 
			activationThreshold, 
			classes, 
			unitsPerClass, 
			units)
		{
		}
	}
}
