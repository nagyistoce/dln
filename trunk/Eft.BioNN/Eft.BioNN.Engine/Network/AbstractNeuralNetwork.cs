// -----------------------------------------------------------------------
// <copyright file="AbstractNeuralNetwork.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Accord.Math;
	using Eft.BioNN.Engine.Data;
	using Eft.BioNN.Engine.Learning;
	using Eft.BioNN.Engine.Utils;

	/// <summary>
	/// An abstract base class for neural networks
	/// </summary>
	/// <typeparam name="T">The type of class to use for storing connection (edge) information.</typeparam>
	public abstract class AbstractNeuralNetwork<T> : INeuralNetwork where T : AbstractInterLayerConnections
	{
		#region Private member variables
		/// <summary>
		/// The alpha factor in the exponential moving average (EMA) of unit activations.
		/// </summary>
		private const double Alpha = 0.05;

		/// <summary>
		/// If the EMA of activations drops below this limit, the activation threshold should be lowered.
		/// </summary>
		private const double ActivationsLow = 0.01;

		/// <summary>
		/// If the EMA of activations rises above this limit, the activation threshold should be increased.
		/// </summary>
		private const double ActivationsHigh = 0.5;

		/// <summary>
		/// The weight offset used when saving/loading networks.
		/// </summary>
		/// <remarks>
		/// The file format uses unsigned bytes to store weights. They are hence shifted by this constant to allow for negative weights.
		/// </remarks>
		private const int SavedWeightOffset = 66;

		/// <summary>
		/// The probability that an edge/threshold is updated.
		/// </summary>
		private double learningProbability = 0.05;

		/// <summary>
		/// The number of units in each layer.
		/// </summary>
		private int[] unitCounts;

		/// <summary>
		/// The inter-layer connections (edges).
		/// </summary>
		private T[] connections;

		/// <summary>
		/// The learning algorithm.
		/// </summary>
		private ILearningAlgorithm learningAlgorithm;

		/// <summary>
		/// The maximum number of activated input units.
		/// </summary>
		private int maxInputActivations;

		/// <summary>
		/// The total 'signal' required for a unit to become active.
		/// </summary>
		private int activationThreshold;

		/// <summary>
		/// The activation thresholds of individual neurons, indexed by layer, then neuron.
		/// </summary>
		private int[][] individualActivationThresholds; // TODO: Save/Load

		/// <summary>
		/// The number of distinct classes.
		/// </summary>
		private int classCount;

		/// <summary>
		/// The number of output units per class.
		/// </summary>
		private int unitsPerClass;

		/// <summary>
		/// The probability that a synapse (connection/edge) works.
		/// </summary>
		private double synapseSuccessProbability;

		/// <summary>
		/// A (cached) array with the indices of the units corresponding to the class labels.
		/// </summary>
		private List<int>[] labelUnits;

		/// <summary>
		/// The minimum weight that can be assigned to a connection.
		/// </summary>
		private int minWeight = -1;

		/// <summary>
		/// The maximum weight that can be assigned to a connection.
		/// </summary>
		private int maxWeight = 1;

		/// <summary>
		/// The exponential moving averages of unit activations, indexed by layer, then unit.
		/// </summary>
		private double[][] activationAverages;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractNeuralNetwork{T}" /> class.
		/// </summary>
		/// <param name="learningAlgorithm">The learning algorithm.</param>
		/// <param name="minWeight">The minimum weight.</param>
		/// <param name="maxWeight">The maximum weight.</param>
		/// <param name="weightProbabilities">The weight probability distribution.</param>
		/// <param name="synapseSuccessProbability">The probability that a synapse (connection/edge) works.</param>
		/// <param name="maxInputActivations">The maximum number of activated input units.</param>
		/// <param name="activationThreshold">The total 'signal' required for a unit to become active.</param>
		/// <param name="classes">The number of distinct classes.</param>
		/// <param name="unitsPerClass">The number of output units per class.</param>
		/// <param name="units">The number of units in each layer, excluding the output layer.</param>
		public AbstractNeuralNetwork(
			ILearningAlgorithm learningAlgorithm,
			int minWeight,
			int maxWeight,
			double[] weightProbabilities,
			double synapseSuccessProbability,
			int maxInputActivations,
			int activationThreshold,
			int classes, 
			int unitsPerClass, 
			params int[] units)
		{
			//
			// Validate input (partially)
			//
			this.ValidateConstructorArguments(units, synapseSuccessProbability, minWeight, maxWeight);
			//
			// Store parameter values in member variables
			//
			this.minWeight = minWeight;
			this.maxWeight = maxWeight;
			this.learningAlgorithm = learningAlgorithm;
			this.maxInputActivations = maxInputActivations;
			this.activationThreshold = activationThreshold;
			this.classCount = classes;
			this.unitsPerClass = unitsPerClass;
			this.synapseSuccessProbability = synapseSuccessProbability;
			//
			// Store the unit counts and append the count for the output layer
			//
			this.unitCounts = new int[units.Length + 1];
			Array.Copy(units, this.unitCounts, units.Length);
			this.unitCounts[units.Length] = classes * unitsPerClass;
			//
			// Initialize the learning algorithm, if one has been supplied
			//
			if (this.learningAlgorithm != null)
			{
				this.learningAlgorithm.Initialize(this.maxInputActivations);
			}
			//
			// Initialize connections between all layers
			//
			this.connections = new T[this.unitCounts.Length - 1];
			for (int layer = 0; layer < this.connections.Length; layer++)
			{
				this.connections[layer] = (T)Activator.CreateInstance(typeof(T), this.unitCounts[layer], this.unitCounts[layer + 1]);
				this.connections[layer].Initialize(this.minWeight, this.maxWeight, weightProbabilities);
			}
			//
			// Populate the cache of label units
			//
			this.GenerateLabelUnitLists(classes, unitsPerClass);

			this.InitializeActivationData(activationThreshold);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractNeuralNetwork{T}" /> class.
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
		protected AbstractNeuralNetwork(
			int minWeight,
			int maxWeight,
			double synapseSuccessProbability,
			int maxInputActivations,
			int activationThreshold,
			int classes,
			int unitsPerClass,
			int[] unitCounts,
			T[] connections)
		{
			//
			// Validation
			//
			if (unitCounts == null)
			{
				throw new ArgumentException("Unit count array cannot be null", "unitCounts");
			}

			if (connections == null)
			{
				throw new ArgumentException("Connection array cannot be null", "connections");
			}

			if (unitCounts.Length != connections.Length + 1)
			{
				throw new ArgumentException("The number of connection layers should be exactly one less than the number of unit layers");
			}

			if (unitCounts[unitCounts.Length - 1] != classes * unitsPerClass)
			{
				throw new ArgumentException("The number of output units should be equal to classes * unitsPerClass");
			}

			this.ValidateConstructorArguments(unitCounts, synapseSuccessProbability, minWeight, maxWeight);
			//
			// Store parameter values in member variables
			//
			this.minWeight = minWeight;
			this.maxWeight = maxWeight;
			this.synapseSuccessProbability = synapseSuccessProbability;
			this.maxInputActivations = maxInputActivations;
			this.activationThreshold = activationThreshold;
			this.classCount = classes;
			this.unitsPerClass = unitsPerClass;
			this.unitCounts = unitCounts;
			this.connections = connections;
			//
			// Populate the cache of label units
			//
			this.GenerateLabelUnitLists(classes, unitsPerClass);

			// TODO: Don't re-initialize, read activation thresholds from file
			this.InitializeActivationData(activationThreshold); 
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the number of layers in the network, including the input and output layers.
		/// </summary>
		/// <value>
		/// The total number of layers.
		/// </value>
		public int LayerCount
		{
			get
			{
				return this.unitCounts.Length;
			}
		}

		/// <summary>
		/// Gets the total 'signal' required for a unit to become active.
		/// </summary>
		/// <value>
		/// The activation threshold.
		/// </value>
		public int ActivationThreshold
		{
			get
			{
				return this.activationThreshold;
			}
		}

		/// <summary>
		/// Gets the probability that a synapse (connection/edge) works.
		/// </summary>
		/// <value>
		/// The synapse success probability.
		/// </value>
		public double SynapseSuccessProbability 
		{
			get
			{
				return this.synapseSuccessProbability;
			}
		}

		/// <summary>
		/// Gets or sets the probability that an edge/threshold is updated during training.
		/// </summary>
		/// <value>
		/// The learning probability.
		/// </value>
		public double LearningProbability
		{
			get
			{
				return this.learningProbability;
			}

			set
			{
				if (value < 0 || value > 1)
				{
					throw new ArgumentException("Learning probability must be in the interval [0-1]");
				}

				this.learningProbability = value;
			}
		}

		/// <summary>
		/// Gets or sets the minimum weight that can be assigned to a connection.
		/// </summary>
		/// <value>
		/// The minimum weight.
		/// </value>
		public int MinWeight
		{
			get
			{ 
				return this.minWeight; 
			}

			set
			{
				this.minWeight = value; 
			}
		}

		/// <summary>
		/// Gets or sets the maximum weight that can be assigned to a connection.
		/// </summary>
		/// <value>
		/// The maximum weight.
		/// </value>
		public int MaxWeight
		{
			get 
			{ 
				return this.maxWeight; 
			}

			set 
			{ 
				this.maxWeight = value; 
			}
		}

		/// <summary>
		/// Gets the maximum number of activated input units.
		/// </summary>
		/// <value>
		/// The maximum number of activated input units.
		/// </value>
		public int MaxInputActivations
		{
			get
			{
				return this.maxInputActivations;
			}
		}

		/// <summary>
		/// Gets the activation thresholds of individual neurons.
		/// </summary>
		/// <value>
		/// The activation thresholds of individual neurons, indexed by layer, then neuron.
		/// </value>
		protected int[][] IndividualActivationThresholds
		{
			get
			{
				return this.individualActivationThresholds;
			}
		}

		/// <summary>
		/// Gets or sets the learning algorithm.
		/// </summary>
		/// <value>
		/// The learning algorithm.
		/// </value>
		protected ILearningAlgorithm LearningAlgorithm
		{
			get
			{
				return this.learningAlgorithm;
			}

			set
			{
				this.learningAlgorithm = value;
			}
		}

		/// <summary>
		/// Gets the array of inter-layer connection containers.
		/// </summary>
		/// <value>
		/// The connection array.
		/// </value>
		protected T[] Connections
		{
			get 
			{ 
				return this.connections; 
			}
		}

		/// <summary>
		/// Gets the number of classes.
		/// </summary>
		/// <value>
		/// The number of classes.
		/// </value>
		protected int ClassCount
		{
			get 
			{
				return this.classCount; 
			}
		}

		/// <summary>
		/// Gets the number of output units per class.
		/// </summary>
		/// <value>
		/// The number of output units per class.
		/// </value>
		protected int UnitsPerClass
		{
			get 
			{ 
				return this.unitsPerClass; 
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Calculates the average reconstruction error when an attempt is made to reconstruct the entire label by 
		/// activating a portion of it (the seed) and then doing one full propagation step.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <param name="seedSize">The size of the seed.</param>
		/// <returns>The average reconstruction error.</returns>
		public double CheckLabelReconstruction(int label, int seedSize)
		{
			var labelUnits = this.GetLabelUnits(label);
			var errors = new List<int>();
			//
			// Do this for all possible seeds of the specified size.
			//
			foreach (var seed in Combinatorics.Combinations(labelUnits.ToArray(), seedSize))
			{
				var hiddenActivations = this.PropagateLeft(seed.ToList(), this.LayerCount - 2);
				var reconstuctedLabel = this.PropagateRight(hiddenActivations, this.LayerCount - 2);
				errors.Add(labelUnits.Except(reconstuctedLabel).Count() + reconstuctedLabel.Except(labelUnits).Count());
			}

			return errors.Average();
		}

		/// <summary>
		/// Gets all connection weights in a single connection layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <returns>An array of weights.</returns>
		/// <remarks>
		/// It would be more appropriate to return integers. Double is used for convenience when doing standard deviation calculations.
		/// </remarks>
		public double[] GetAllWeights(int layer)
		{
			var result = new List<double>(this.GetUnitCount(layer) * this.GetUnitCount(layer + 1));
			for (int i = 0; i < this.GetUnitCount(layer); i++)
			{
				for (int j = 0; j < this.GetUnitCount(layer + 1); j++)
				{
					result.Add(this.connections[layer].GetEdge(i, j).Weight);
				}
			}

			return result.ToArray();
		}

		/// <summary>
		/// Returns a single string that contains the activation threshold of every individual unit. 
		/// The string is formatted in such a way that it can be copied into Matlab.
		/// </summary>
		/// <returns>A string with the activation thresholds.</returns>
		public string ThresholdsToString()
		{
			string result = string.Empty;
			for (int layer = 0; layer < this.LayerCount; layer++)
			{
				result += string.Format("layer{0} = [{1}];\r\n", layer, string.Join(" ", this.individualActivationThresholds[layer]));
			}

			return result;
		}

		/// <summary>
		/// Gets the number of units in the specified <paramref name="layer" />
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <returns>
		/// The number of units.
		/// </returns>
		public int GetUnitCount(int layer)
		{
			// TODO: Check if negative
			if (layer >= this.unitCounts.Length)
			{
				throw new ArgumentException("Network only has " + this.unitCounts.Length + " unit layers");
			}

			return this.unitCounts[layer];
		}

		/// <summary>
		/// Trains the network in an unsupervised fashion.
		/// </summary>
		/// <param name="samples">The samples to use for training.</param>
		/// <param name="progressObserver">A delegate to an observer function which tracks the training progress.</param>
		public virtual void TrainUnsupervised(Sample[] samples, Action<int> progressObserver)
		{
			//
			// TODO: Batches?
			// TODO: Variable probability?
			// TODO: Where to start negative phase?
			// TODO: Learn layers one at a time instead of propagating each sample all the way through?
			//
			foreach (var sample in samples)
			{
				//
				// Find which input units are activated by this training sample
				//
				var activatedLeft = this.GetActivatedInputUnits(sample.SampleData);
				for (int layer = 0; layer < this.LayerCount - 1; layer++)
				{
					//
					// Propagate the activations from left to right
					//
					var activatedRight = this.PropagateRight(activatedLeft, layer);
					//
					// Allow the learning algorithm to adjust the edges based on the activations
					//
					this.learningAlgorithm.ProcessSample(this.connections[layer], activatedLeft, activatedRight);
					//
					// Remember which units where activated in the current layer before we move on to the next one
					//
					activatedLeft = activatedRight;
				}
			}
		}

		/// <summary>
		/// Trains the network in a supervised fashion.
		/// </summary>
		/// <param name="samples">The (labelled) samples to use for training.</param>
		/// <param name="progressObserver">A delegate to an observer function which tracks the training progress.</param>
		/// <param name="options">This parameter can be used to provide additional training options.</param>
		public virtual void TrainSupervised(LabelledSample[] samples, Action<int> progressObserver, object options)
		{
			foreach (var sample in samples)
			{
				//
				// Find which input units are activated by this training sample
				//
				var activatedUnits = this.GetActivatedInputUnits(sample.SampleData);
				//
				// Propagate the activations, all the way to the last hidden layer
				//
				for (int layer = 0; layer < this.LayerCount - 2; layer++)
				{
					activatedUnits = this.PropagateRight(activatedUnits, layer);
				}
				//
				// "Clamp" the activations in the output layer to correspond to the group of units representing the label of the sample
				//
				var labelUnits = this.GetLabelUnits(sample.Label);
				//
				// Allow the learning algorithm to adjust the edges to the output layer based on these activations
				//
				this.learningAlgorithm.ProcessSample(this.connections[this.connections.Length - 1], activatedUnits, labelUnits);
			}
		}

		/// <summary>
		/// Classifies the specified sample.
		/// </summary>
		/// <param name="sample">The sample.</param>
		/// <returns>
		/// The class that the network believes the sample belongs to.
		/// </returns>
		public int Classify(Sample sample)
		{
			int[] counts;
			return this.Classify(sample, out counts);
		}

		/// <summary>
		/// Classifies the specified sample.
		/// </summary>
		/// <param name="sample">The sample.</param>
		/// <param name="counts">The number of neurons active in each label group.</param>
		/// <returns>
		/// The class that the network believes the sample belongs to.
		/// </returns>
		public int Classify(Sample sample, out int[] counts)
		{
			//
			// Do a feed-forward pass through the network to see which units in the output layer get activated.
			//
			var activatedUnits = this.GetActivatedInputUnits(sample.SampleData);
			for (int layer = 0; layer < this.connections.Length; layer++)
			{
				activatedUnits = this.PropagateRight(activatedUnits, layer);
			}
			//
			// Temporary hack for joint-networks, clamp visible vector, do a few propagation rounds
			//
			/*for (int k = 0; k < 5; k++)
			{
				var hiddenActivations = this.PropagateToMiddle(this.GetActivatedInputUnits(sample.SampleData), activatedUnits, 0);
				activatedUnits = this.PropagateRight(hiddenActivations, 1);
			}*/
			//
			// Check what the majority vote is
			//
			return this.DeduceLabelFromActivations(activatedUnits, out counts);
		}

		/// <summary>
		/// Construct a 'heat map' of sorts that can be used to visualize how much the specified unit(s) like or dislike each an every input unit.
		/// </summary>
		/// <param name="layer">The layer of the unit(s) for which to generate the reconstruction.</param>
		/// <param name="mask">A mask which specifies for which neuron(s) we wish to generate the reconstruction.</param>
		/// <returns>
		/// An array which has the same size and the input layer. The higher the number, the more the specified units like that particular input unit to be on.
		/// </returns>
		public int[] Reconstruct(int layer, int[] mask)
		{
			//
			// Input validation
			//
			if (layer >= this.LayerCount)
			{
				throw new ArgumentException("Network only has " + this.unitCounts.Length + " unit layers", "layer");
			}

			if (mask == null || mask.Length != this.unitCounts[layer])
			{
				throw new ArgumentException("Mask size should equal the number of units in the specified layer", "mask");
			}
			//
			// To build our reconstruction, we traverse paths from right to left
			// We count the number of paths from the 'source' units to the input units
			// This is done incrementally, one layer at a time.
			// If an inhibitory edge is encountered, the sign of the count is flipped.
			//
			int[] result = null;
			for (int layerIndex = layer; layerIndex > 0; layerIndex--)
			{
				var pathCounts = new int[this.unitCounts[layerIndex - 1]];
				for (int unitIndex = 0; unitIndex < mask.Length; unitIndex++)
				{
					if (mask[unitIndex] != 0)
					{
						var neighbours = this.connections[layerIndex - 1].GetNeighbours(unitIndex, Partition.Right, true);
						foreach (var neighbour in neighbours)
						{
							//
							// These reconstructions get a bit weird once we are past the first hidden layer.
							// If a unit dislikes a preceding unit that dislikes a pixel, does that mean that the unit likes the pixel?
							// (i.e. is a double negative a positive?)
							// Not necessarily, yet that is what the reconstruction suggests.
							//
							pathCounts[neighbour.Target] += mask[unitIndex] * neighbour.Weight;
						}
					}
				}
				//
				// The intermediate path counts become the 'mask' for the next layer
				//
				result = mask = pathCounts;
			}

			return result;
		}

		/// <summary>
		/// Saves the neural network to the supplied stream.
		/// </summary>
		/// <param name="stream">The stream in which to write the neural network.</param>
		public void Save(Stream stream)
		{
			//
			// Start by writing out the property values
			//
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(this.ActivationThreshold);
			writer.Write(this.ClassCount);
			writer.Write(this.MaxInputActivations);
			writer.Write(this.synapseSuccessProbability);
			writer.Write(this.UnitsPerClass);
			writer.Write(this.LayerCount);
			//
			// Now write the number of units in each layer
			//
			for (int layer = 0; layer < this.LayerCount; layer++)
			{
				writer.Write(this.GetUnitCount(layer));
			}
			//
			// Finally, write out the adjacency lists for each set of inter-layer connections
			//
			for (int layer = 0; layer < this.LayerCount - 1; layer++)
			{
				for (int leftUnit = 0; leftUnit < this.GetUnitCount(layer); leftUnit++)
				{
					var edges = this.connections[layer].GetNeighbours(leftUnit, Partition.Left, false).ToArray();
					writer.Write(edges.Length);
					foreach (var edge in edges)
					{
						writer.Write(edge.Target);
						writer.Write((byte)(edge.Weight + SavedWeightOffset));
					}
				}
			}
			//
			// PS, write min and max weights
			//
			writer.Write(this.minWeight);
			writer.Write(this.maxWeight);
		}

		/// <summary>
		/// Generates the visible states by performing a random walk, which consists of propagating activations right and left
		/// starting at the specified <paramref name="generationLayer" />.
		/// </summary>
		/// <param name="generationLayer">The generation layer.</param>
		/// <param name="numberOfSamples">The number of samples to take in order to estimate the activation probabilities in the visible (i.e. input) layer.</param>
		/// <returns>
		/// An infinite sequence of activation probabilities for units in the visible layer.
		/// </returns>
		public IEnumerable<double[]> GenerateVisibleStates(int generationLayer, int numberOfSamples)
		{
			//
			// Validation
			//
			if (generationLayer >= this.LayerCount)
			{
				throw new ArgumentException("Layer parameter should have a value between 0 and " + (this.LayerCount - 1), "layer");
			}
			//
			// Select a random starting configuration for the right partition
			// TODO: Make it possible to clamp if we're starting from output layer
			//
			var activationsRight = this.GetRandomActivations(0.5, this.GetUnitCount(generationLayer + 1));

			double increment = 1.0 / numberOfSamples;
			//
			// Since this method simply returns an enumerator, we can generate an infinite series on the fly
			//
			while (true)
			{
				//
				// TODO: Step through this code, ensure there are no +/-1 errors
				// Start by propagating the activations to the left
				//
				var activationsLeft = this.PropagateLeft(activationsRight, generationLayer);
				//
				// Now do a backwards propagation pass all the way to the input layer
				// Do several passes and return estimated input activation probabilities
				//
				var inputActivationProbabilities = new double[this.GetUnitCount(0)];
				for (int sampleIndex = 0; sampleIndex < numberOfSamples; sampleIndex++)
				{
					var inputActivations = this.PropagateToInputLayer(activationsRight, generationLayer);
					foreach (var unit in inputActivations)
					{
						inputActivationProbabilities[unit] += increment;
					}
				}
				//
				// Return the input activation probabilities
				//
				yield return inputActivationProbabilities;
				//
				// Now, propagate the activations to the right
				//
				activationsRight = this.PropagateRight(activationsLeft, generationLayer);
				//
				// In order to 'clamp' a particular label, we can do it like so:
				//
				//activationsRight = this.PropagateToMiddle(activationsLeft, this.GetLabelUnits(3), generationLayer);
			}
		}

		/// <summary>
		/// Gets the Euclidian distance between the weight vectors of pairs of output neurons belonging to the same class.
		/// </summary>
		/// <returns>A three dimensional array, indexed by class, unit, unit</returns>
		public double[][][] GetPairwiseWeightDistancesBetweenLabelUnits()
		{
			var result = new double[this.ClassCount][][];

			var labelWeights = this.Connections[this.LayerCount - 2];

			for (int classIndex = 0; classIndex < this.ClassCount; classIndex++)
			{
				result[classIndex] = new double[this.UnitsPerClass][];

				int firstUnit = classIndex * this.UnitsPerClass;
				for (int i = 0; i < this.UnitsPerClass; i++)
				{
					var iWeightVector = labelWeights.GetNeighbours(firstUnit + i, Partition.Right, false).Select(x => x.Weight).ToArray();
					
					for (int j = i + 1; j < this.UnitsPerClass; j++)
					{
						if (result[classIndex][i] == null)
						{
							result[classIndex][i] = new double[this.UnitsPerClass];
						}

						if (result[classIndex][j] == null)
						{
							result[classIndex][j] = new double[this.UnitsPerClass];
						}

						var jWeightVector = labelWeights.GetNeighbours(firstUnit + j, Partition.Right, false).Select(x => x.Weight).ToArray();
						var distance = this.EuclidianDistance(iWeightVector, jWeightVector);
						result[classIndex][i][j] = result[classIndex][j][i] = distance;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Reads the neural network parameters from the supplied <paramref name="stream" />.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="activationThreshold">The total 'signal' required for a unit to become active.</param>
		/// <param name="classCount">The number of distinct classes.</param>
		/// <param name="maxInputActivations">The maximum number of activated input units.</param>
		/// <param name="synapseSuccessProbability">The probability that a synapse (connection/edge) works.</param>
		/// <param name="unitsPerClass">The number of output units per class.</param>
		/// <param name="unitCounts">The number of units in each layer.</param>
		/// <param name="connections">The inter-layer connections (edges).</param>
		/// <param name="minWeight">The minimum weight.</param>
		/// <param name="maxWeight">The maximum weight.</param>
		protected static void ReadNetworkParametersFromStream(
			Stream stream, 
			out int activationThreshold,
			out int classCount,
			out int maxInputActivations,
			out double synapseSuccessProbability,
			out int unitsPerClass,
			out int[] unitCounts,
			out T[] connections,
			out int minWeight,
			out int maxWeight)
		{
			BinaryReader reader = new BinaryReader(stream);
			//
			// Read basic properties 
			//
			activationThreshold = reader.ReadInt32();
			classCount = reader.ReadInt32();
			maxInputActivations = reader.ReadInt32();
			synapseSuccessProbability = reader.ReadDouble();
			unitsPerClass = reader.ReadInt32();
			var layerCount = reader.ReadInt32();
			//
			// Read the unit counts
			//
			unitCounts = new int[layerCount];
			for (int layer = 0; layer < layerCount; layer++)
			{
				unitCounts[layer] = reader.ReadInt32();
			}
			//
			// Read which units are connected by what kind of an edge
			//
			connections = new T[layerCount - 1];
			for (int layer = 0; layer < layerCount - 1; layer++)
			{
				connections[layer] = (T)Activator.CreateInstance(typeof(T), unitCounts[layer], unitCounts[layer + 1]);
				for (int leftUnit = 0; leftUnit < unitCounts[layer]; leftUnit++)
				{
					var edgeCount = reader.ReadInt32();
					for (int edgeIndex = 0; edgeIndex < edgeCount; edgeIndex++)
					{
						var target = reader.ReadInt32();
						var edgeType = reader.ReadByte();

						int weight = 0;
						//
						// This may seem a bit weird. This is to keep backwards compatiblity with files created before 'arbitrary' 
						// edge weights were introduced.
						//
						if (edgeType < 5)
						{
							if (edgeType == 2)
							{
								weight = 1;
							}
							else if (edgeType == 4)
							{
								weight = -1;
							}
						}
						else
						{
							weight = edgeType - SavedWeightOffset;
						}

						connections[layer].SetEdge(leftUnit, target, weight);
					}
				}
			}
			//
			// TODO: Attempt to read min/max weight
			//
			minWeight = -1;
			maxWeight = 1;
			try
			{
				minWeight = reader.ReadInt32();
				maxWeight = reader.ReadInt32();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Warning! Could not read min and max weight. The file is probably in an older format.");
				System.Diagnostics.Debug.WriteLine(ex.ToString());
			}
		}

		/// <summary>
		/// Gets the indices of the output units corresponding to the specified <paramref name="label"/>.
		/// </summary>
		/// <param name="label">The class label.</param>
		/// <returns>A list of unit indices.</returns>
		protected List<int> GetLabelUnits(int label)
		{
			//
			// Return the list of unit indices from cache.
			//
			return this.labelUnits[label];
		}

		/// <summary>
		/// Stochastically selects units to be activated given the specified <paramref name="activationProbability" />
		/// </summary>
		/// <param name="activationProbability">The activation probability.</param>
		/// <param name="numberOfUnits">The number of units.</param>
		/// <returns>The indices of the activated units.</returns>
		protected List<int> GetRandomActivations(double activationProbability, int numberOfUnits)
		{
			//
			// Fill an array, which matches the size of the layer, with the same activation probability
			//
			var activationProbabilityArray = new double[numberOfUnits];
			for (int i = 0; i < activationProbabilityArray.Length; i++)
			{
				activationProbabilityArray[i] = activationProbability;
			}
			//
			// Delegate to overload
			//
			return this.GetRandomActivations(activationProbabilityArray);
		}

		/// <summary>
		/// Stochastically selects units to be activated given their activation probabilities
		/// </summary>
		/// <param name="activationProbabilities">The activation probability of each unit.</param>
		/// <returns>The indices of the activated units.</returns>
		protected List<int> GetRandomActivations(double[] activationProbabilities)
		{
			List<int> results = new List<int>(activationProbabilities.Length / 2);
			var random = ThreadSafeRandom.GetThreadRandom();
			for (int i = 0; i < activationProbabilities.Length; i++)
			{
				//
				// Flip a random coin
				//
				if (activationProbabilities[i] > random.NextDouble())
				{
					//
					// This unit is activated. Append it to the list. 
					//
					results.Add(i);
				}
			}

			return results;
		}

		/// <summary>
		/// Deduces the label from activations in the output layer.
		/// </summary>
		/// <param name="activatedUnits">The activated output units.</param>
		/// <param name="counts">The number of neurons active in each label group.</param>
		/// <returns>
		/// A class label
		/// </returns>
		protected int DeduceLabelFromActivations(List<int> activatedUnits, out int[] counts)
		{
			//
			// Count how many units are active in each label group
			//
			counts = new int[this.ClassCount];
			foreach (var unit in activatedUnits)
			{
				counts[unit / this.UnitsPerClass]++;
			}
			//
			// Pick the two labels with the hightest counts
			//
			var topTwo = counts.Select((value, index) => new { Value = value, Index = index })
				.ToList()
				.AsRandom()
				.OrderByDescending(a => a.Value)
				.Take(2)
				.ToArray();
			//
			// This is a confidence metric. I don't use it currently but it may make sense to keep an eye on it.
			//
			var margin = topTwo[0].Value - topTwo[1].Value;
			//
			// Return the label that got the most votes (activations)
			//
			return topTwo[0].Index;
		}

		/// <summary>
		/// Propagates activations from left to right.
		/// </summary>
		/// <param name="activatedUnits">The units that are active in the left partition.</param>
		/// <param name="layer">The layer.</param>
		/// <returns>The units that get activated in the right layer</returns>
		protected List<int> PropagateRight(List<int> activatedUnits, int layer)
		{
			return this.PropagateActivations(Partition.Left, activatedUnits, layer);
		}

		/// <summary>
		/// Propagates activations from right to left.
		/// </summary>
		/// <param name="activatedUnits">The units that are active in the right partition.</param>
		/// <param name="layer">The layer.</param>
		/// <returns>The units that get activated in the left layer</returns>
		protected List<int> PropagateLeft(List<int> activatedUnits, int layer)
		{
			return this.PropagateActivations(Partition.Right, activatedUnits, layer);
		}

		/// <summary>
		/// Propagates activations to a hidden layer from both of its neighboring layers.
		/// </summary>
		/// <param name="leftActivatedUnits">The activated units in the left layer.</param>
		/// <param name="rightActivatedUnits">The activated units in the right layer.</param>
		/// <param name="leftLayer">The index of the left layer.</param>
		/// <returns>The units that get activated in the middle layer</returns>
		protected List<int> PropagateToMiddle(List<int> leftActivatedUnits, List<int> rightActivatedUnits, int leftLayer)
		{
			//
			// Create an array which will store the signal counters for the middle layer
			//
			var counts = new int[this.GetUnitCount(leftLayer + 1)];
			//
			// First count the signals coming from the left
			//
			this.CountSignals(leftActivatedUnits, leftLayer, Partition.Left, counts);
			//
			// Then count the signals coming from the right
			//
			int middleLayer = leftLayer + 1;
			this.CountSignals(rightActivatedUnits, middleLayer, Partition.Right, counts);
			//
			// Return the indices of those units in the middle layer whose signal strength exceeds the activation threshold
			//
			return this.GetActivatedUnits(counts, middleLayer);
		}

		/// <summary>
		/// Propagates activations from right to left, all the way to the input layer.
		/// </summary>
		/// <param name="activations">The initial activations.</param>
		/// <param name="startingLayer">The starting layer.</param>
		/// <returns>The units that get activated in the input layer</returns>
		protected List<int> PropagateToInputLayer(List<int> activations, int startingLayer)
		{
			var result = activations;
			for (int layer = startingLayer; layer >= 0; layer--)
			{
				result = this.PropagateLeft(result, layer);
			}

			return result;
		}

		/// <summary>
		/// Gets the activated input units.
		/// </summary>
		/// <param name="data">The sample data.</param>
		/// <returns>A list with the indices of the activated input units.</returns>
		protected List<int> GetActivatedInputUnits(byte[] data)
		{
			//
			// Create a list for the results
			//
			var activatedUnits = new List<int>(data.Length / 2); // Is this too generous a capacity?

			var random = ThreadSafeRandom.GetThreadRandom();
			for (int index = 0; index < data.Length; index++)
			{
				//
				// Stochastically activate units depending on their input strength
				// In ML terms, the input units are linear-stochastic
				//
				if (data[index] > random.NextDouble() * byte.MaxValue)
				{
					activatedUnits.Add(index);
				}
			}
#if DEBUG
			//System.Diagnostics.Debug.Write(activatedUnits.Count + " units activated");
#endif
			//
			// Trim the list if we've exceeded the maximum number of input activations
			//
			if (activatedUnits.Count > this.maxInputActivations)
			{
				//
				// Shuffle the activated units (in random order) and then select to first X
				//
				activatedUnits = activatedUnits.AsRandom().Take(this.maxInputActivations).ToList();
			}
#if DEBUG
			//System.Diagnostics.Debug.WriteLine(" (" + activatedUnits.Count + " after filtering)");
#endif
			//
			// Return the activated units 
			//
			return activatedUnits;
		}

		/// <summary>
		/// Propagates activations from one partition to another.
		/// </summary>
		/// <param name="sourcePartition">The source partition.</param>
		/// <param name="activatedUnits">The units active in the source partition.</param>
		/// <param name="layer">The layer.</param>
		/// <returns>The units that get activated in the other partition.</returns>
		protected List<int> PropagateActivations(Partition sourcePartition, List<int> activatedUnits, int layer)
		{
			//
			// Figure out the size of the target partition
			//
			int targetPartitionSize;
			if (sourcePartition == Partition.Left)
			{
				targetPartitionSize = this.unitCounts[layer + 1];
			}
			else
			{
				targetPartitionSize = this.unitCounts[layer];
			}
			//
			// Create an array which will store counts, which roughly correspond to the accumulated signal received by 
			// a particular target unit. If the signal is strong enough, it will fire (i.e. become active).
			//
			var counts = new int[targetPartitionSize];
			//
			// Aggregate the signals coming from the neighbouring layer
			//
			this.CountSignals(activatedUnits, layer, sourcePartition, counts);
			//
			// Return the indices of those units receiving a signal exceeding the activation threshold
			//
			int targetLayer = layer;
			if (sourcePartition == Partition.Left)
			{
				targetLayer++;
			}

			var result = this.GetActivatedUnits(counts, targetLayer);
			//
			// TODO: Only bother to do this if the activation threshold is variable
			//
			//this.UpdateActivationAverages(result, targetLayer);

			return result;
		}

		/// <summary>
		/// Updates the activation thresholds according to the EMA1 rule.
		/// </summary>
		protected void UpdateThresholdsEma1()
		{
			//
			// TODO: Parallelize
			// Iterate over the layers
			//
			for (var layer = 0; layer < this.LayerCount; layer++)
			{
				//
				// Iterate over the units in the current layer
				//
				for (var unit = 0; unit < this.GetUnitCount(layer); unit++)
				{
					//
					// Should this threshold be lowered? Is it still above the bare minimum (t/2)?
					//
					if (this.activationAverages[layer][unit] < ActivationsLow && 
						this.individualActivationThresholds[layer][unit] > this.ActivationThreshold / 2)
					{
						this.individualActivationThresholds[layer][unit]--;
						this.activationAverages[layer][unit] = 0.1;
#if DEBUG
						System.Diagnostics.Debug.WriteLine("Unit {0} in layer {1} lowered its threshold to {2}", unit, layer, this.individualActivationThresholds[layer][unit]);
#endif
					}
					//
					// Should this threshold be increased? Is it still below the absolute maximum (t*2)?
					//
					else if (this.activationAverages[layer][unit] > ActivationsHigh && 
						this.individualActivationThresholds[layer][unit] < this.ActivationThreshold * 2)
					{
						this.individualActivationThresholds[layer][unit]++;
						this.activationAverages[layer][unit] = 0.1;
#if DEBUG
						System.Diagnostics.Debug.WriteLine("Unit {0} in layer {1} increased its threshold to {2}", unit, layer, this.individualActivationThresholds[layer][unit]);
#endif
					}
				}
			}
		}

		/// <summary>
		/// Updates the activation averages.
		/// </summary>
		/// <param name="activatedUnits">The activated units.</param>
		/// <param name="layer">The layer in which the units reside.</param>
		private void UpdateActivationAverages(List<int> activatedUnits, int layer)
		{
			//
			// Construct a boolean array equal to the size of the layer.
			// Set its elements to true if the unit at the corresponding index is activated.
			//
			var activated = new bool[this.GetUnitCount(layer)];
			foreach (var unit in activatedUnits)
			{
				activated[unit] = true;
			}
			//
			// Iterate over the averages for the units in this layer
			//
			for (int unit = 0; unit < activated.Length; unit++)
			{
				//
				// Update the exponential moving average for the current unit
				//
				double increment = activated[unit] ? 1.0 : 0.0;
				var previousValue = this.activationAverages[layer][unit];
				this.activationAverages[layer][unit] = previousValue + (Alpha * (increment - previousValue));
			}
		}

		/// <summary>
		/// Gets the indices of the units that have received a signal exceeding the activation threshold.
		/// </summary>
		/// <param name="signalCounts">An array containing the overall signal received by each unit.</param>
		/// <param name="layer">The layer.</param>
		/// <returns>
		/// The indices of those units which get activated.
		/// </returns>
		private List<int> GetActivatedUnits(int[] signalCounts, int layer)
		{
			//
			// Allocate a list of units activated in the target partition
			//
			var result = new List<int>(signalCounts.Length / 2); // Is this too generous?
			//
			// Find the indices of the units in the target partition that will be activated
			//
			for (int index = 0; index < signalCounts.Length; index++)
			{
				if (signalCounts[index] >= this.individualActivationThresholds[layer][index])
				{
					result.Add(index);
				}
			}
			//
			// Return the list of activated units
			//
			return result;
		}

		/// <summary>
		/// Aggregates signals received by neighboring units.
		/// </summary>
		/// <param name="activatedUnits">The activated units in the neighboring layer.</param>
		/// <param name="layer">The index of the connection layer.</param>
		/// <param name="sourcePartition">The source partition (i.e. layer) from which the signals emanate.</param>
		/// <param name="counts">An array to store the signal counters.</param>
		private void CountSignals(List<int> activatedUnits, int layer, Partition sourcePartition, int[] counts)
		{
			Parallel.ForEach(
				activatedUnits,
				(sourceUnit) =>
				{
					var random = ThreadSafeRandom.GetThreadRandom();
					//
					// Fetch the neighbours who receive a signal from the current unit
					//
					var neighbours = this.connections[layer].GetNeighbours(sourceUnit, sourcePartition, true);
					foreach (var neighbour in neighbours)
					{
						//
						// Throw a coin to determine whether the synapse works
						//
						if (this.synapseSuccessProbability >= 1.0 || random.NextDouble() < this.synapseSuccessProbability)
						{
							//
							// Increment/decrement the counter for the target unit
							//
							Interlocked.Add(ref counts[neighbour.Target], neighbour.Weight);
						}
					}
				});
		}

		/// <summary>
		/// Validates the constructor arguments.
		/// </summary>
		private void ValidateConstructorArguments(int[] units, double synapseSuccessProbability, int minWeight, int maxWeight)
		{
			if (units.Length < 1)
			{
				throw new ArgumentException("Neural network must have at least two layers");
			}

			for (int layer = 0; layer < units.Length; layer++)
			{
				if (units[layer] <= 0)
				{
					throw new ArgumentException("Each layer must have a positive number of units");
				}
			}

			if (minWeight >= maxWeight)
			{
				throw new ArgumentException("Minimum weight must be smaller than maximum weight.");
			}

			ValidationUtils.ValidateProbability(synapseSuccessProbability, "synapseSuccessProbability");
		}

		/// <summary>
		/// Generates the cached lists of label unit indices.
		/// </summary>
		/// <param name="classCount">The number of classes.</param>
		/// <param name="unitsPerClass">The number of output units per class.</param>
		private void GenerateLabelUnitLists(int classCount, int unitsPerClass)
		{
			//
			// Create an array with one element per class
			//
			this.labelUnits = new List<int>[classCount];
			//
			// Populate the array
			//
			for (int classIndex = 0; classIndex < classCount; classIndex++)
			{
				this.labelUnits[classIndex] = new List<int>(unitsPerClass);
				int from = classIndex * unitsPerClass;
				int to = from + unitsPerClass;
				for (int unitIndex = from; unitIndex < to; unitIndex++)
				{
					this.labelUnits[classIndex].Add(unitIndex);
				}
			}
		}

		/// <summary>
		/// Initializes activation thresholds and averages.
		/// </summary>
		/// <param name="threshold">The initial activation threshold.</param>
		private void InitializeActivationData(int threshold)
		{
			//
			// Initialize the (jagged) array of averages
			//
			this.activationAverages = new double[this.LayerCount][];
			for (int layer = 0; layer < this.LayerCount; layer++)
			{
				this.activationAverages[layer] = new double[this.GetUnitCount(layer)];
			}
			//
			// Initialize the (jagged) array of thresholds
			//
			this.individualActivationThresholds = new int[this.LayerCount][];
			for (int layer = 0; layer < this.LayerCount; layer++)
			{
				this.individualActivationThresholds[layer] = new int[this.GetUnitCount(layer)];

				int layerThreshold = threshold;
				//
				// Uncomment the following for layer-wise thresholds in networks with one hidden layer (joint learning)
				//
				/*
				if (layer == 0)
				{
				    layerThreshold = Convert.ToInt32(Math.Round(threshold * this.GetUnitCount(layer + 1) / 1000.0));
				}
				else if (layer == this.LayerCount - 1)
				{
				    layerThreshold = this.individualActivationThresholds[layer - 2][0];
				}
				*/
				for (int unit = 0; unit < this.individualActivationThresholds[layer].Length; unit++)
				{
					this.individualActivationThresholds[layer][unit] = layerThreshold;
				}
			}
		}

		/// <summary>
		/// Calculates the Euclidian distance between two points.
		/// </summary>
		/// <param name="pointA">The coordinates of point a.</param>
		/// <param name="pointB">The coordinates of point b.</param>
		/// <returns>The Euclidian distance between the two points.</returns>
		private double EuclidianDistance(int[] pointA, int[] pointB)
		{
			if (pointA.Length != pointB.Length)
			{
				throw new ArgumentException("The number of dimensions don't match between points A and B.");
			}

			long squaredDistance = 0;
			for (int index = 0; index < pointA.Length; index++)
			{
				int distanceInCurrentDimension = pointA[index] - pointB[index];
				squaredDistance += distanceInCurrentDimension * distanceInCurrentDimension;
			}

			return Math.Sqrt(Convert.ToDouble(squaredDistance));
		}
		#endregion
	}
}
