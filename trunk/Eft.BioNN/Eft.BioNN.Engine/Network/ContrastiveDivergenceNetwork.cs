// -----------------------------------------------------------------------
// <copyright file="ContrastiveDivergenceNetwork.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using Eft.BioNN.Engine.Data;
	using Eft.BioNN.Engine.Learning;
	using Eft.BioNN.Engine.Utils;

	/// <summary>
	/// An implementation of a neural network that uses single-step Contrastive Divergence for training.
	/// </summary>
	/// <typeparam name="T">The type of class to use for storing connection (edge) information.</typeparam>
	/// <remarks>
	/// This implementation does not calculate or estimate any activation probabilities. Instead it just uses stochastic unit activations.
	/// This is not the same as in a traditional implementation of Contrastive Divergence.
	/// See: http://www.cs.toronto.edu/~hinton/absps/guideTR.pdf
	/// </remarks>
	public class ContrastiveDivergenceNetwork<T> : AbstractNeuralNetwork<T> where T : AbstractInterLayerConnections
	{
		/// <summary>
		/// The number of samples to process between reporting the reconstruction error.
		/// </summary>
		private const int ErrorInterval = 100;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ContrastiveDivergenceNetwork{T}" /> class.
		/// </summary>
		/// <param name="minWeight">The minimum weight.</param>
		/// <param name="maxWeight">The maximum weight.</param>
		/// <param name="weightProbabilities">The weight probability distribution.</param>
		/// <param name="synapseSuccessProbability">The probability that a synapse (connection/edge) works.</param>
		/// <param name="maxInputActivations">The maximum number of activated input units.</param>
		/// <param name="activationThreshold">The total 'signal' required for a unit to become active.</param>
		/// <param name="classes">The number of distinct classes.</param>
		/// <param name="unitsPerClass">The number of output units per class.</param>
		/// <param name="units">The number of units in each layer, excluding the output layer.</param>
		public ContrastiveDivergenceNetwork(
			int minWeight,
			int maxWeight,
			double[] weightProbabilities, 
			double synapseSuccessProbability, 
			int maxInputActivations, 
			int activationThreshold, 
			int classes, 
			int unitsPerClass, 
			params int[] units)
			: base(
			null, 
			minWeight,
			maxWeight,
			weightProbabilities,
			synapseSuccessProbability, 
			maxInputActivations, 
			activationThreshold, 
			classes, 
			unitsPerClass, 
			units)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContrastiveDivergenceNetwork{T}" /> class.
		/// </summary>
		/// <param name="minWeight">The minimum weight.</param>
		/// <param name="maxWeight">The maximum weight.</param>
		/// <param name="synapseSuccessProbability">The probability that a synapse (connection/edge) works.</param>
		/// <param name="maxInputActivations">The maximum number of activated input units.</param>
		/// <param name="activationThreshold">The total 'signal' required for a unit to become active.</param>
		/// <param name="classes">The number of distinct classes.</param>
		/// <param name="unitsPerClass">The number of output units per class.</param>
		/// <param name="unitCounts">The number of units in each layer, including the output layer.</param>
		/// <param name="connections">The inter-layer connections (edges).</param>
		/// <remarks>
		/// This constructor is intended to be used when loading a network that has been saved to disk.
		/// </remarks>
		private ContrastiveDivergenceNetwork(
			int minWeight,
			int maxWeight,
			double synapseSuccessProbability,
			int maxInputActivations,
			int activationThreshold,
			int classes,
			int unitsPerClass,
			int[] unitCounts,
			T[] connections)
			: base(
			minWeight,
			maxWeight,
			synapseSuccessProbability,
			maxInputActivations,
			activationThreshold,
			classes,
			unitsPerClass,
			unitCounts,
			connections)
		{
		}
		#endregion

		#region Loading
		/// <summary>
		/// Loads a previously saved network from the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>An instance of a <see cref="ContrastiveDivergenceNetwork{T}" />.</returns>
		public static ContrastiveDivergenceNetwork<T> Load(Stream stream)
		{
			//
			// Declare variables for the parameters that describe the network
			//
			int activationThreshold, classCount, maxInputActivations, unitsPerClass, minWeight, maxWeight;
			double synapseSuccessProbability;
			int[] unitCounts;
			T[] connections;
			//
			// Read the parameters from the stream
			//
			ReadNetworkParametersFromStream(
				stream, 
				out activationThreshold, 
				out classCount, 
				out maxInputActivations, 
				out synapseSuccessProbability, 
				out unitsPerClass, 
				out unitCounts, 
				out connections,
				out minWeight,
				out maxWeight);
			//
			// Return the reconstructed neural network
			//
			return new ContrastiveDivergenceNetwork<T>(
				minWeight,
				maxWeight,
				synapseSuccessProbability,
				maxInputActivations,
				activationThreshold,
				classCount,
				unitsPerClass,
				unitCounts,
				connections);
		}
		#endregion

		#region Training methods
		/// <summary>
		/// Trains the network in an unsupervised fashion.
		/// </summary>
		/// <param name="samples">The samples to use for training.</param>
		/// <param name="progressObserver">A delegate to an observer function which tracks the training progress.</param>
		public override void TrainUnsupervised(Sample[] samples, Action<int> progressObserver)
		{
			this.Train(samples, false, progressObserver);
		}

		/// <summary>
		/// Trains the network in a supervised fashion.
		/// </summary>
		/// <param name="samples">The (labelled) samples to use for training.</param>
		/// <param name="progressObserver">A delegate to an observer function which tracks the training progress.</param>
		/// <param name="options">This parameter can be used to provide additional training options.</param>
		public override void TrainSupervised(LabelledSample[] samples, Action<int> progressObserver, object options)
		{
			if (options is ContrastiveDivergenceLearningMode &&
				((ContrastiveDivergenceLearningMode)options) != ContrastiveDivergenceLearningMode.FeedForward)
			{
				throw new NotSupportedException("This neural network doesn't support learning on the joint distribution of images and labels");
			}

			this.Train(samples, true, progressObserver);
		}

		/// <summary>
		/// Trains the network.
		/// </summary>
		/// <param name="samples">The samples to use for training.</param>
		/// <param name="supervised">If set to <c>true</c> then supervised training is performed.</param>
		/// <param name="progressObserver">A delegate to an observer function which tracks the training progress.</param>
		private void Train(Sample[] samples, bool supervised, Action<int> progressObserver)
		{
			int reconstructionError = 0;
			//
			// TODO: I'll use matrices now..but in the sparse case it may make sense to use more succint data structures
			// Start by gathering the input activations
			//
			var previousActivations = new List<int>[samples.Length];
			for (int sampleIndex = 0; sampleIndex < samples.Length; sampleIndex++)
			{
				previousActivations[sampleIndex] = this.GetActivatedInputUnits(samples[sampleIndex].SampleData);
			}
			//
			// Train each layer from left to right
			//
			for (int layer = 0; layer < this.LayerCount - 1; layer++)
			{
				//
				// If we are doing supervised training, then we skip the last layer
				// TODO: Train top layer as well during unsupervised learning?
				//
				if (!supervised && layer == this.LayerCount - 2)
				{
					break;
				}
				//
				// Construct an array that will hold the (positive) activations, which we will then use for training the subsequent layer
				//
				var currentActivations = new List<int>[samples.Length];
				//
				// Go through every training sample
				//
				for (int sampleIndex = 0; sampleIndex < samples.Length; sampleIndex++)
				{
					//
					// Get the positive activations from the previous layer
					//
					var positiveActivationsLeft = previousActivations[sampleIndex];
					//
					// Propagate them to the right
					//
					List<int> positiveActivationsRight;
					if (supervised && layer == this.LayerCount - 2)
					{
						//
						// Clamp the activations on the right to correspond to the units that represent the class of this training sample 
						//
						positiveActivationsRight = this.GetLabelUnits(((LabelledSample)samples[sampleIndex]).Label);
					}
					else
					{
						positiveActivationsRight = this.PropagateRight(positiveActivationsLeft, layer);
					}
					//
					// Store the positive activations so that we can use them when training the subsequent layer
					//
					currentActivations[sampleIndex] = positiveActivationsRight;
					//
					// If we are doing supervised learning, then we stop here since we only want to adjust the connections to the 
					// output layer
					//
					if (supervised && layer < this.LayerCount - 2)
					{
						continue;
					}
					//
					// Gather data for the negative phase
					//
					var negativeActivationsLeft = this.PropagateLeft(positiveActivationsRight, layer);
					var negativeActivationsRight = this.PropagateRight(negativeActivationsLeft, layer);
#if DEBUG
					//
					// Calculate and show the reconstruction error
					//
					reconstructionError += this.CalculateReconstructionError(positiveActivationsLeft, negativeActivationsLeft);
					if ((sampleIndex + 1) % ErrorInterval == 0)
					{
						System.Diagnostics.Debug.WriteLine("Reconstruction error: " + (reconstructionError / ErrorInterval));
						reconstructionError = 0;
					}
#endif
					//
					// Construct 2D matrices, indexed by units in the left, respectively right, partition
					// A cell contains the value 1 if a pair of units were both activated and 0 otherwise.
					//
					short[,] positiveProducts = this.FillMatrix(positiveActivationsLeft, positiveActivationsRight, this.GetUnitCount(layer), this.GetUnitCount(layer + 1));
					short[,] negativeProducts = this.FillMatrix(negativeActivationsLeft, negativeActivationsRight, this.GetUnitCount(layer), this.GetUnitCount(layer + 1));
					//
					// Use the difference between these two matrices as a "learning signal". 
					// Cells containing 1 indicate that a synapse should be made more excitatory.
					// Cells containing -1 indicate that a synapse should be made more inhibitory.
					//
					this.UpdateEdges(positiveProducts.Subtract(negativeProducts, true), layer);
					//
					// Notify the progress observer function
					//
					if (progressObserver != null)
					{
						progressObserver(sampleIndex + 1);
					}
				}
				//
				// The 'output' activations of this layer become the 'input' activations for the subsequent layer
				//
				previousActivations = currentActivations;
			}
		}

		/// <summary>
		/// Calculates the reconstruction error.
		/// </summary>
		/// <param name="positiveActivationsLeft">The original (data driven) activations.</param>
		/// <param name="negativeActivationsLeft">The reconstructed activations.</param>
		/// <returns>The sum of squared errors.</returns>
		private int CalculateReconstructionError(List<int> positiveActivationsLeft, List<int> negativeActivationsLeft)
		{
			//
			// I suspect that this is not terribly efficient...but it is succinct
			// Since all values are binary, there's no need to square the errors.
			//
			return positiveActivationsLeft.Except(negativeActivationsLeft).Count() + negativeActivationsLeft.Except(positiveActivationsLeft).Count();
		}

		/// <summary>
		/// Stochastically makes edges more excitatory or inhibitory according to the supplied <paramref name="changes"/> matrix.
		/// </summary>
		/// <param name="changes">A 2D matrix indicating where edges (synapses) should potentially be altered.</param>
		/// <param name="layer">The connection layer to alter.</param>
		private void UpdateEdges(short[,] changes, int layer)
		{
			//
			// Traverse the matrix
			//
			Parallel.For(
				0, 
				changes.GetLength(0), 
				(leftUnit) =>
			{
				var random = ThreadSafeRandom.GetThreadRandom();
				for (int rightUnit = 0; rightUnit < changes.GetLength(1); rightUnit++)
				{
					//
					// If the learning signal indicates that this edge should be modified, flip a coin
					// The probability of the edge being modified can be considered to be a pseudo-learning-rate
					// TODO: Parameterize the probability of an edge being updated 
					//
					if (changes[leftUnit, rightUnit] != 0 && random.NextDouble() < LearningProbability)
					{
						//
						// Get the current type of edge and, if possible, make it either more excitatory or inhibitory, 
						// depending on the learning signal.
						//
						var edge = this.Connections[layer].GetEdge(leftUnit, rightUnit);
						if (changes[leftUnit, rightUnit] > 0)
						{
							if (edge.Weight < this.MaxWeight)
							{
								this.Connections[layer].SetEdge(leftUnit, rightUnit, edge.Weight + 1);
							}
						}
						else
						{
							if (edge.Weight > this.MinWeight)
							{
								this.Connections[layer].SetEdge(leftUnit, rightUnit, edge.Weight - 1);
							}
						}
					}
				}
			});
		}

		/// <summary>
		/// Fills a 2D matrix with zeroes and ones.
		/// </summary>
		/// <param name="activationsLeft">The activated units in the left partition.</param>
		/// <param name="activationsRight">The activated units in the right partition.</param>
		/// <param name="rows">The number of rows.</param>
		/// <param name="columns">The number of columns.</param>
		/// <returns>A 2D matrix</returns>
		private short[,] FillMatrix(List<int> activationsLeft, List<int> activationsRight, int rows, int columns)
		{
			var result = new short[rows, columns];
			foreach (var row in activationsLeft)
			{
				foreach (var column in activationsRight)
				{
					result[row, column] = 1;
				}
			}

			return result;
		}
		#endregion
	}
}
