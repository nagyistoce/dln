// -----------------------------------------------------------------------
// <copyright file="ContrastiveDivergenceSamplingNetwork.cs" company="Eiríkur Fannar Torfason">
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
	using Accord.Math;
	using Eft.BioNN.Engine.Data;
	using Eft.BioNN.Engine.Learning;
	using Eft.BioNN.Engine.Utils;

	// HACK: I copied a lot of code from the original ContrastiveDivergenceNetwork class.

	/// <summary>
	/// An implementation of a neural network that uses single-step Contrastive Divergence for training.
	/// </summary>
	/// <typeparam name="T">The type of class to use for storing connection (edge) information.</typeparam>
	/// <remarks>
	/// This implementation attempts to estimate activation probabilities by sampling from the distribution. 
	/// These probabilities are then used to calculate the learning signal.
	/// This is closer in spirit to a traditional implementation of Contrastive Divergence.
	/// See: http://www.cs.toronto.edu/~hinton/absps/guideTR.pdf
	/// </remarks>
	public class ContrastiveDivergenceSamplingNetwork<T> : AbstractNeuralNetwork<T> where T : AbstractInterLayerConnections
	{
		/// <summary>
		/// The number of samples to process between reporting the reconstruction error.
		/// </summary>
		private const int ErrorInterval = 100;

		/// <summary>
		/// The number of samples to take in order to estimate activation probabilities.
		/// </summary>
		private int probabilityEstimationSamples = 1; // TODO: Persist value when saved?
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ContrastiveDivergenceSamplingNetwork{T}" /> class.
		/// </summary>
		/// <param name="minWeight">The minimum weight.</param>
		/// <param name="maxWeight">The maximum weight.</param>
		/// <param name="weightProbabilities">The weight probabilities.</param>
		/// <param name="synapseSuccessProbability">The probability that a synapse (connection/edge) works.</param>
		/// <param name="maxInputActivations">The maximum number of activated input units.</param>
		/// <param name="activationThreshold">The total 'signal' required for a unit to become active.</param>
		/// <param name="classes">The number of distinct classes.</param>
		/// <param name="unitsPerClass">The number of output units per class.</param>
		/// <param name="activationSamples">The number of samples to take to estimate activation probabilities.</param>
		/// <param name="units">The number of units in each layer, excluding the output layer.</param>
		public ContrastiveDivergenceSamplingNetwork(
			int minWeight,
			int maxWeight,
			double[] weightProbabilities,
			double synapseSuccessProbability,
			int maxInputActivations,
			int activationThreshold,
			int classes,
			int unitsPerClass,
			int activationSamples,
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
			this.probabilityEstimationSamples = activationSamples;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContrastiveDivergenceSamplingNetwork{T}" /> class.
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
		protected ContrastiveDivergenceSamplingNetwork(
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
		/// <returns>An instance of a <see cref="ContrastiveDivergenceSamplingNetwork{T}" />.</returns>
		public static ContrastiveDivergenceSamplingNetwork<T> Load(Stream stream)
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
			AbstractNeuralNetwork<T>.ReadNetworkParametersFromStream(
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
			return new ContrastiveDivergenceSamplingNetwork<T>(
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
			this.Train(samples, false, progressObserver, ContrastiveDivergenceLearningMode.FeedForward);
		}

		/// <summary>
		/// Trains the network in a supervised fashion.
		/// </summary>
		/// <param name="samples">The (labelled) samples to use for training.</param>
		/// <param name="progressObserver">A delegate to an observer function which tracks the training progress.</param>
		/// <param name="options">This parameter can be used to provide additional training options.</param>
		public override void TrainSupervised(LabelledSample[] samples, Action<int> progressObserver, object options)
		{
			this.Train(samples, true, progressObserver, (ContrastiveDivergenceLearningMode)options);
		}

		/// <summary>
		/// Gets the activation probabilities for the negative phase.
		/// </summary>
		/// <param name="activatedHiddenUnits">The positive activations in the hidden layer.</param>
		/// <param name="layer">The index of the connection layer.</param>
		/// <param name="hiddenPartition">The partition which corresponds to the hidden layer.</param>
		/// <param name="negativeProbabilitiesVisible">The activation probabilities in the visible layer.</param>
		/// <param name="negativeProbabilitiesHidden">The activation probabilities in the hidden layer.</param>
		protected virtual void GetNegativePhaseProbabilities(List<int> activatedHiddenUnits, int layer, Partition hiddenPartition, out double[] negativeProbabilitiesVisible, out double[] negativeProbabilitiesHidden)
		{
			int visibleUnits = this.GetUnitCount(layer);
			int hiddenUnits = this.GetUnitCount(layer + 1);
			Partition visiblePartition = Partition.Left;
			//
			// Is the left partitition the hidden layer?
			//
			if (hiddenPartition == Partition.Left)
			{
				//
				// Swap the unit counts
				//
				Swap(ref visibleUnits, ref hiddenUnits);
				visiblePartition = Partition.Right;
			}
			//
			//  First we propagate back to the visible layer...
			//
			List<int>[] negativeActivationsVisible = new List<int>[this.probabilityEstimationSamples];
			for (int i = 0; i < this.probabilityEstimationSamples; i++)
			{
				negativeActivationsVisible[i] = this.PropagateActivations(hiddenPartition, activatedHiddenUnits, layer);
			}
			//
			// ...and estimate the probabilities
			//
			negativeProbabilitiesVisible = this.EstimateProbabilities(negativeActivationsVisible, visibleUnits);
			//
			// Then we propagate again to the hidden layer...
			//
			var negativeActivationsHidden = new List<int>[this.probabilityEstimationSamples];
			for (int i = 0; i < this.probabilityEstimationSamples; i++)
			{
				negativeActivationsHidden[i] = this.PropagateActivations(visiblePartition, negativeActivationsVisible[i], layer);
			}
			//
			// ...and estimate the probabilities.
			//
			negativeProbabilitiesHidden = this.EstimateProbabilities(negativeActivationsHidden, hiddenUnits);
		}

		/// <summary>
		/// Gets the activation probabilities for the negative phase.
		/// </summary>
		/// <param name="activatedHiddenUnits">The positive activations in the hidden layer.</param>
		/// <param name="layer">The index of the connection layer.</param>
		/// <param name="negativeProbabilitiesVisible">The activation probabilities in the visible layer.</param>
		/// <param name="negativeProbabilitiesHidden">The activation probabilities in the hidden layer.</param>
		/// <param name="negativeProbabilitiesLabels">The activation probabilities in the output layer.</param>
		protected virtual void GetNegativePhaseProbabilities(List<int> activatedHiddenUnits, int layer, out double[] negativeProbabilitiesVisible, out double[] negativeProbabilitiesHidden, out double[] negativeProbabilitiesLabels)
		{
			int visibleUnits = this.GetUnitCount(layer);
			int hiddenUnits = this.GetUnitCount(layer + 1);
			int labelUnits = this.GetUnitCount(layer + 2);
			//
			//  First we propagate back to the visible layer...
			//
			List<int>[] negativeActivationsVisible = new List<int>[this.probabilityEstimationSamples];
			for (int i = 0; i < this.probabilityEstimationSamples; i++)
			{
				negativeActivationsVisible[i] = this.PropagateActivations(Partition.Right, activatedHiddenUnits, layer);
			}
			//
			// ...and estimate the probabilities
			//
			negativeProbabilitiesVisible = this.EstimateProbabilities(negativeActivationsVisible, visibleUnits);
			//
			//  Then we propagate to the label layer...
			//
			List<int>[] negativeActivationsLabels = new List<int>[this.probabilityEstimationSamples];
			for (int i = 0; i < this.probabilityEstimationSamples; i++)
			{
				negativeActivationsLabels[i] = this.PropagateActivations(Partition.Left, activatedHiddenUnits, layer + 1);
			}
			//
			// ...and estimate the probabilities
			//
			negativeProbabilitiesLabels = this.EstimateProbabilities(negativeActivationsLabels, labelUnits);
			//
			// Then we propagate again to the hidden layer...
			//
			var negativeActivationsHidden = new List<int>[this.probabilityEstimationSamples];
			for (int i = 0; i < this.probabilityEstimationSamples; i++)
			{
				negativeActivationsHidden[i] = this.PropagateToMiddle(negativeActivationsVisible[i], negativeActivationsLabels[i], layer);
			}
			//
			// ...and estimate the probabilities.
			//
			negativeProbabilitiesHidden = this.EstimateProbabilities(negativeActivationsHidden, hiddenUnits);
		}

		/// <summary>
		/// Stochastically makes edges more excitatory or inhibitory according to the supplied <paramref name="changes" /> matrix.
		/// </summary>
		/// <param name="changes">A 2D matrix indicating where edges (synapses) should potentially be altered.</param>
		/// <param name="layer">The connection layer to alter.</param>
		/// <param name="joint">Should be set to <c>true</c> if joint learning is taking place, otherwise <c>false</c>.</param>
		protected virtual void UpdateEdges(double[,] changes, int layer, bool joint)
		{
			System.Diagnostics.Debug.Assert(
				changes.GetLength(0) == this.GetUnitCount(layer), 
				"changes matrix size doesn't match the size of the weight matrix");
			System.Diagnostics.Debug.Assert(
				changes.GetLength(1) == this.GetUnitCount(layer + 1),
				"changes matrix size doesn't match the size of the weight matrix");
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
					// Flip a coin to see if we should modify this edge
					// Note that the strength of the learning signal affects the probability
					// TODO: Parameterize the pseudo-learning-rate constant
					//
					if (changes[leftUnit, rightUnit] != 0.0 && random.NextDouble() < Math.Abs(changes[leftUnit, rightUnit]) * LearningProbability)
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
		/// Swaps two elements.
		/// </summary>
		/// <typeparam name="U">The type of the elements to swap.</typeparam>
		/// <param name="a">The first element.</param>
		/// <param name="b">The second element.</param>
		private static void Swap<U>(ref U a, ref U b)
		{
			var temp = a;
			a = b;
			b = temp;
		}

		/// <summary>
		/// Trains the network.
		/// </summary>
		/// <param name="samples">The samples to use for training.</param>
		/// <param name="supervised">If set to <c>true</c> then supervised training is performed.</param>
		/// <param name="progressObserver">A delegate to an observer function which tracks the training progress.</param>
		/// <param name="supervisedLearningMode">The supervised learning mode.</param>
		private void Train(Sample[] samples, bool supervised, Action<int> progressObserver, ContrastiveDivergenceLearningMode supervisedLearningMode)
		{
			bool joint = false;
			if (supervised && (
					supervisedLearningMode == ContrastiveDivergenceLearningMode.JointAlternative || 
					supervisedLearningMode == ContrastiveDivergenceLearningMode.JointNormal))
			{
				joint = true;
			}
			//
			// TODO: It might speed things up to re-use activation probability arrays instead of generating new ones all the time 
			// for the positive and negative phases.
			//
			double reconstructionError = 0;
			double labelReconstructionError = 0;
			//
			// Convert the input data, which consists of byte-valued pixel intensities, into linear activation probabilities
			//
			var previousProbabilities = new double[samples.Length][];
			for (int sampleIndex = 0; sampleIndex < samples.Length; sampleIndex++)
			{
				previousProbabilities[sampleIndex] = new double[samples[sampleIndex].SampleData.Length];
				for (int unitIndex = 0; unitIndex < samples[sampleIndex].SampleData.Length; unitIndex++)
				{
					previousProbabilities[sampleIndex][unitIndex] = (double)samples[sampleIndex].SampleData[unitIndex] / byte.MaxValue;
				}
			}

			List<int> labelUnits = null;
			//
			// Train each layer from left to right
			//
			for (int layer = 0; layer < this.LayerCount - 1; layer++)
			{
				//
				// If we are doing unsupervised training, then we skip the last layer
				// TODO: Train top layer as well during unsupervised learning?
				//
				if ((!supervised || joint) && layer == this.LayerCount - 2)
				{
					break;
				}
				//
				// Set a couple of boolean variables which indicate whether or not supervised learning should take place in the current layer
				//
				bool supervisedLayer = supervised && layer == this.LayerCount - 2;
				bool jointSupervisedLayer = supervised && joint && layer == this.LayerCount - 3;
				//
				// Construct an array that will hold the (positive) activation probabilities, 
				// which we will then use for training the subsequent layer
				//
				var currentProbabilities = new double[samples.Length][];
				//
				// Create the matrices that will hold the positive and negative phase data (for the learning signal)
				//
				var positiveProducts = new double[this.GetUnitCount(layer), this.GetUnitCount(layer + 1)];
				var negativeProducts = new double[this.GetUnitCount(layer), this.GetUnitCount(layer + 1)];
				double[,] jointPositiveProducts = null;
				double[,] jointNegativeProducts = null;
				if (jointSupervisedLayer)
				{
					jointPositiveProducts = new double[this.GetUnitCount(layer + 1), this.GetUnitCount(layer + 2)];
					jointNegativeProducts = new double[this.GetUnitCount(layer + 1), this.GetUnitCount(layer + 2)];
				}
				//
				// Go through every training sample
				//
				for (int sampleIndex = 0; sampleIndex < samples.Length; sampleIndex++)
				{
					//
					// Get the positive activation probabilites from the previous layer
					//
					var positiveProbabilitiesLeft = previousProbabilities[sampleIndex];
					//
					// Propagate them to the right
					//
					double[] positiveProbabilitiesRight;
					List<int> positiveActivationsRight;
					if (supervisedLayer)
					{
						//
						// Clamp the activations on the right to correspond to the units that represent the class of this training sample 
						// TODO: It would probably be more realistic to have a union of naturally activated units and those in the label group
						//
						positiveActivationsRight = this.GetLabelUnits(((LabelledSample)samples[sampleIndex]).Label);
						positiveProbabilitiesRight = new double[this.GetUnitCount(layer + 1)];
						foreach (var unit in positiveActivationsRight)
						{
							positiveProbabilitiesRight[unit] = 1.0;
						}
					}
					else
					{
						if (jointSupervisedLayer)
						{
							labelUnits = this.GetLabelUnits(((LabelledSample)samples[sampleIndex]).Label);
						}
						//
						// Perform several propagations (i.e. collect activation samples)
						//
						List<int>[] activationSamplesRight = new List<int>[this.probabilityEstimationSamples];
						for (int i = 0; i < this.probabilityEstimationSamples; i++)
						{
							var positiveActivationsLeft = this.GetRandomActivations(positiveProbabilitiesLeft);
							if (jointSupervisedLayer)
							{
								activationSamplesRight[i] = this.PropagateToMiddle(positiveActivationsLeft, labelUnits, layer);
							}
							else
							{
								activationSamplesRight[i] = this.PropagateRight(positiveActivationsLeft, layer);
							}
						}
						//
						// Estimate the activation probabilities and store them so that we can use them when training the subsequent layer
						//
						positiveProbabilitiesRight = currentProbabilities[sampleIndex] = this.EstimateProbabilities(
							activationSamplesRight,
							this.GetUnitCount(layer + 1));
						//
						// We need to pick a binary activation to use for the negative (reconstruction) phase
						//
						positiveActivationsRight = activationSamplesRight[0];
					}
					//
					// If we are doing supervised learning, then we stop here since we only want to adjust the connections to the 
					// output layer
					//
					if (supervised && !supervisedLayer && !jointSupervisedLayer)
					{
						continue;
					}
					//
					// Gather data for the negative phase
					//
					double[] negativeProbabilitiesLeft;
					double[] negativeProbabilitiesRight;
					double[] negativeProbabilitiesLabels = null;

					if (jointSupervisedLayer && supervisedLearningMode == ContrastiveDivergenceLearningMode.JointNormal)
					{
						this.GetNegativePhaseProbabilities(positiveActivationsRight, layer, out negativeProbabilitiesLeft, out negativeProbabilitiesRight, out negativeProbabilitiesLabels);
					}
					else
					{
						this.GetNegativePhaseProbabilities(positiveActivationsRight, layer, Partition.Right, out negativeProbabilitiesLeft, out negativeProbabilitiesRight);
					}
#if DEBUG
					//
					// Calculate and show the reconstruction error
					//
					reconstructionError += this.CalculateReconstructionError(positiveProbabilitiesLeft, negativeProbabilitiesLeft);
					if ((sampleIndex + 1) % ErrorInterval == 0)
					{
						System.Diagnostics.Debug.WriteLine("Reconstruction error: " + (reconstructionError / ErrorInterval));
						reconstructionError = 0;
					}
#endif
					//
					// Multiply the positive, respectively negative, activation probability vectors to obtain matrices of the size
					// |LEFT PARTITION| x |RIGHT PARTITION|
					//
					this.MultiplyVectors(positiveProbabilitiesLeft, positiveProbabilitiesRight, positiveProducts);
					this.MultiplyVectors(negativeProbabilitiesLeft, negativeProbabilitiesRight, negativeProducts);
					//
					// Use the difference between these two matrices as a "learning signal"
					//
					this.UpdateEdges(positiveProducts.Subtract(negativeProducts, true), layer, joint);

					//this.UpdateThresholdsCD(layer, positiveProbabilitiesLeft.Subtract(negativeProbabilitiesLeft));
					//this.UpdateThresholdsCD(layer + 1, positiveProbabilitiesRight.Subtract(negativeProbabilitiesRight));
					//
					// Should we also adjust the edges going to the label units?
					//
					if (jointSupervisedLayer)
					{
						//
						// Calculate the positive products for the last two layers
						//
						var positiveProbabilitiesLabels = new double[this.ClassCount * this.UnitsPerClass];
						foreach (var labelUnit in labelUnits)
						{
							positiveProbabilitiesLabels[labelUnit] = 1.0;
						}

						this.MultiplyVectors(positiveProbabilitiesRight, positiveProbabilitiesLabels, jointPositiveProducts);
						//
						// Now get the negative products
						//
						if (supervisedLearningMode == ContrastiveDivergenceLearningMode.JointAlternative)
						{
							this.GetNegativePhaseProbabilities(positiveActivationsRight, layer + 1, Partition.Left, out negativeProbabilitiesLabels, out negativeProbabilitiesRight);
						}

						this.MultiplyVectors(negativeProbabilitiesRight, negativeProbabilitiesLabels, jointNegativeProducts);
						//
						// Use the difference between these two matrices as a "learning signal"
						//
						this.UpdateEdges(jointPositiveProducts.Subtract(jointNegativeProducts, true), layer + 1, joint);

						//this.UpdateThresholdsCD(layer + 2, positiveProbabilitiesLabels.Subtract(negativeProbabilitiesLabels));
#if DEBUG
						//
						// Calculate and show the reconstruction error
						//
						labelReconstructionError += this.CalculateReconstructionError(positiveProbabilitiesLabels, negativeProbabilitiesLabels);
						if ((sampleIndex + 1) % ErrorInterval == 0)
						{
							System.Diagnostics.Debug.WriteLine("Label reconstruction error: " + (labelReconstructionError / (ErrorInterval * this.UnitsPerClass)));
							labelReconstructionError = 0;
						}
#endif
					}

					if ((sampleIndex + 1) % 100 == 0)
					{
						//this.UpdateThresholdsEma1();
					}

					//
					// Notify the progress observer function
					//
					if (progressObserver != null)
					{
						progressObserver(sampleIndex + 1);
					}
				}
				//
				// The 'output' activation probabilities of this layer become the 'input' probabilities for the subsequent layer
				//
				previousProbabilities = currentProbabilities;
			}
		}

		/// <summary>
		/// Updates activations thresholds according to the Contrastive Divergence bias update rules.
		/// </summary>
		/// <param name="layer">The layer in which the units reside.</param>
		/// <param name="thresholdDelta">The threshold delta (positive phase activation probabilities - negative phase activation probabilities).</param>
		private void UpdateThresholdsCD(int layer, double[] thresholdDelta)
		{
			System.Diagnostics.Debug.Assert(
				thresholdDelta.Length == this.GetUnitCount(layer), 
				"The array with the threshold deltas should have the same number of elements as there are units in the layer");
			var random = ThreadSafeRandom.GetThreadRandom();
			for (int unit = 0; unit < thresholdDelta.Length; unit++)
			{
				if (thresholdDelta[unit] != 0 && random.NextDouble() < Math.Abs(thresholdDelta[unit]) * LearningProbability)
				{
					if (thresholdDelta[unit] > 0)
					{
						this.IndividualActivationThresholds[layer][unit]--;
#if DEBUG
						//System.Diagnostics.Debug.WriteLine("Unit {0} in layer {1} lowered its threshold to {2}", unit, layer, this.IndividualActivationThresholds[layer][unit]);
#endif
					}
					else if (thresholdDelta[unit] < 0)
					{
						this.IndividualActivationThresholds[layer][unit]++;
#if DEBUG
						//System.Diagnostics.Debug.WriteLine("Unit {0} in layer {1} increased its threshold to {2}", unit, layer, this.IndividualActivationThresholds[layer][unit]);
#endif
					}
				}
			}
		}

		/// <summary>
		/// Multiplies the two vectors and returns a matrix of size |a| x |b|.
		/// </summary>
		/// <param name="a">The first vector.</param>
		/// <param name="b">The second vector.</param>
		/// <param name="result">The matrix which will hold the results.</param>
		/// <returns>
		/// A two dimensional matrix.
		/// </returns>
		private double[,] MultiplyVectors(double[] a, double[] b, double[,] result)
		{
			if (a.Length != result.GetLength(0) || b.Length != result.GetLength(1))
			{
				throw new Exception(
					string.Format(
						"Expected matrix of size {0}x{1} but received matrix of size {2}x{3}.", 
						a.Length, 
						b.Length, 
						result.GetLength(0), 
						result.GetLength(1)));
			}

			Parallel.For(
				0, 
				a.Length, 
				(i) =>
			{
				for (int j = 0; j < b.Length; j++)
				{
					result[i, j] = a[i] * b[j];
				}
			});

			return result;
		}

		/// <summary>
		/// Estimates the activation probabilities based on the supplicate activation samples.
		/// </summary>
		/// <param name="activations">The activation samples.</param>
		/// <param name="numberOfUnits">The number of units in the layer.</param>
		/// <returns>The activation probability of each unit in the layer.</returns>
		private double[] EstimateProbabilities(List<int>[] activations, int numberOfUnits)
		{
			var result = new double[numberOfUnits];
			double incrementationStep = 1.0 / activations.Length;
			foreach (var activation in activations)
			{
				foreach (var unit in activation)
				{
					result[unit] += incrementationStep;
				}
			}

			return result;
		}

		/// <summary>
		/// Calculates the reconstruction error.
		/// </summary>
		/// <param name="originalProbabilities">The original (data driven) probabilities.</param>
		/// <param name="reconstructionProbabilities">The reconstruction probabilities.</param>
		/// <returns>The sum of squared errors.</returns>
		private double CalculateReconstructionError(double[] originalProbabilities, double[] reconstructionProbabilities)
		{
			return originalProbabilities.Zip(reconstructionProbabilities, (a, b) => (a - b) * (a - b)).Sum();
		}
		#endregion
	}
}
