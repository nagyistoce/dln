// -----------------------------------------------------------------------
// <copyright file="PersistentContrastiveDivergenceSamplingNetwork.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	/// <summary>
	/// A variant which uses Persistent Contrastive Divergence for training.
	/// </summary>
	/// <typeparam name="T">The type of class to use for storing connection (edge) information.</typeparam>
	/// <example>http://www.cs.utoronto.ca/~tijmen/pcd/pcd.pdf</example>
	public class PersistentContrastiveDivergenceSamplingNetwork<T> : ContrastiveDivergenceSamplingNetwork<T> where T : AbstractInterLayerConnections
	{
		/// <summary>
		/// The number of particles to maintain per level.
		/// </summary>
		private int numberOfParticles = 1;

		/// <summary>
		/// The particles, indexed by layer, then particle
		/// </summary>
		private List<int>[,] particles;

		/// <summary>
		/// The current particle to use for the negative phase.
		/// </summary>
		private int[] currentParticle;

		/// <summary>
		/// Initializes a new instance of the <see cref="PersistentContrastiveDivergenceSamplingNetwork{T}" /> class.
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
		/// <param name="numberOfParticles">The number of particles.</param>
		/// <param name="units">The number of units in each layer, excluding the output layer.</param>
		public PersistentContrastiveDivergenceSamplingNetwork(
			int minWeight,
			int maxWeight,
			double[] weightProbabilities,
			double synapseSuccessProbability,
			int maxInputActivations,
			int activationThreshold,
			int classes,
			int unitsPerClass,
			int activationSamples,
			int numberOfParticles,
			params int[] units)
			: base(
			minWeight,
			maxWeight,
			weightProbabilities,
			synapseSuccessProbability,
			maxInputActivations,
			activationThreshold,
			classes,
			unitsPerClass,
			activationSamples,
			units)
		{
			this.Initialize(numberOfParticles);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PersistentContrastiveDivergenceSamplingNetwork{T}" /> class.
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
		private PersistentContrastiveDivergenceSamplingNetwork(
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
			this.Initialize(1); // HACK: The particles should be saved and loaded
		}

		/// <summary>
		/// Loads a previously saved network from the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>An instance of a <see cref="PersistentContrastiveDivergenceSamplingNetwork{T}" />.</returns>
		public static new ContrastiveDivergenceSamplingNetwork<T> Load(Stream stream)
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
			PersistentContrastiveDivergenceSamplingNetwork<T>.ReadNetworkParametersFromStream(
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
			return new PersistentContrastiveDivergenceSamplingNetwork<T>(
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

		/// <summary>
		/// Gets the negative phase probabilities.
		/// </summary>
		/// <param name="activatedHiddenUnits">The activated hidden units.</param>
		/// <param name="layer">The layer.</param>
		/// <param name="hiddenPartition">The hidden partition.</param>
		/// <param name="negativeProbabilitiesVisible">The negative probabilities visible.</param>
		/// <param name="negativeProbabilitiesHidden">The negative probabilities hidden.</param>
		protected override void GetNegativePhaseProbabilities(List<int> activatedHiddenUnits, int layer, Partition hiddenPartition, out double[] negativeProbabilitiesVisible, out double[] negativeProbabilitiesHidden)
		{
			//
			// Restart the chain from the current activations if it has died out
			//
			this.RestartChainIfDead(layer, activatedHiddenUnits);
			//
			// TODO: Average over all particles
			// Use a particle rather than the positive activation in the right partition to generate the negative learning signal
			//
			if (hiddenPartition == Partition.Right)
			{
				base.GetNegativePhaseProbabilities(this.particles[layer, this.currentParticle[layer]], layer, Partition.Right, out negativeProbabilitiesVisible, out negativeProbabilitiesHidden);
			}
			else
			{
				throw new NotSupportedException("Joint-alternative learning is currently not supported for this type of network");
			}
			//
			// Increment the index of the current particle (wrapping around the number of particles)
			// (particles are used in a round-robin fashion)
			//
			this.IncrementCurrentParticle(layer);
		}

		/// <summary>
		/// Gets the activation probabilities for the negative phase.
		/// </summary>
		/// <param name="activatedHiddenUnits">The positive activations in the hidden layer.</param>
		/// <param name="layer">The index of the connection layer.</param>
		/// <param name="negativeProbabilitiesVisible">The activation probabilities in the visible layer.</param>
		/// <param name="negativeProbabilitiesHidden">The activation probabilities in the hidden layer.</param>
		/// <param name="negativeProbabilitiesLabels">The activation probabilities in the output layer.</param>
		protected override void GetNegativePhaseProbabilities(List<int> activatedHiddenUnits, int layer, out double[] negativeProbabilitiesVisible, out double[] negativeProbabilitiesHidden, out double[] negativeProbabilitiesLabels)
		{
			//
			// Restart the chain from the current activations if it has died out
			//
			this.RestartChainIfDead(layer, activatedHiddenUnits);
			//
			// TODO: Average over all particles
			//
			base.GetNegativePhaseProbabilities(this.particles[layer, this.currentParticle[layer]], layer, out negativeProbabilitiesVisible, out negativeProbabilitiesHidden, out negativeProbabilitiesLabels);

			this.IncrementCurrentParticle(layer);
		}

		/// <summary>
		/// Stochastically makes edges more excitatory or inhibitory according to the supplied <paramref name="changes" /> matrix.
		/// </summary>
		/// <param name="changes">A 2D matrix indicating where edges (synapses) should potentially be altered.</param>
		/// <param name="layer">The connection layer to alter.</param>
		/// <param name="joint">Should be set to <c>true</c> if joint learning is taking place, otherwise <c>false</c>.</param>
		protected override void UpdateEdges(double[,] changes, int layer, bool joint)
		{
			//
			// Update the edges
			//
			base.UpdateEdges(changes, layer, joint);
			//
			// Perform one Gibbs sampling step for the particles in this layer
			// TODO: For joint learning, this step needs to be a bit different for the final hidden layer
			// We will have to propagate left to the previous hidden layer and to the right to the output layer, then sandwich 
			// these back to the middle layer.
			//
			for (int particleIndex = 0; particleIndex < this.numberOfParticles; particleIndex++)
			{
				if (joint && layer == this.LayerCount - 3)
				{
					//
					// This is the second to last connection layer.
					// Wait until the last connection layer has been updated.
					//
					return;
				}
				else if (joint && layer == this.LayerCount - 2)
				{
					//
					// The last connection layer has been updated, now it's safe to update the particle 
					//
					var activationsLeft = this.PropagateLeft(this.particles[layer - 1, particleIndex], layer - 1);
					var activationsLabels = this.PropagateRight(this.particles[layer - 1, particleIndex], layer);
					this.particles[layer - 1, particleIndex] = this.PropagateToMiddle(activationsLeft, activationsLabels, layer - 1);
				}
				else
				{
					var activationsLeft = this.PropagateLeft(this.particles[layer, particleIndex], layer);
					this.particles[layer, particleIndex] = this.PropagateRight(activationsLeft, layer);
				}
			}
		}

		/// <summary>
		/// Restarts the chain if the particle has become dead (no activations).
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="activatedHiddenUnits">The activations that will replace the particle, if it's dead.</param>
		private void RestartChainIfDead(int layer, List<int> activatedHiddenUnits)
		{
			//
			// Has the set of activated neurons become empty? 
			//
			if (this.particles[layer, this.currentParticle[layer]].Count == 0)
			{
				//
				// Restart from the activations that were handed to us 
				//
				System.Diagnostics.Debug.WriteLine("Warning! Restarting particle chain. Activation threshold may be too high.");
				this.particles[layer, this.currentParticle[layer]] = activatedHiddenUnits;
			}
		}

		/// <summary>
		/// Increments the current particle index.
		/// </summary>
		/// <param name="layer">The layer.</param>
		private void IncrementCurrentParticle(int layer)
		{
			//
			// Increment the index of the current particle (wrapping around the number of particles)
			// (particles are used in a round-robin fashion)
			//
			this.currentParticle[layer] = (this.currentParticle[layer] + 1) % this.numberOfParticles;
		}

		/// <summary>
		/// Initializes the network with the specified number of particles.
		/// </summary>
		/// <param name="numberOfParticles">The number of particles.</param>
		private void Initialize(int numberOfParticles)
		{
			this.numberOfParticles = numberOfParticles;
			this.particles = new List<int>[this.LayerCount - 1, this.numberOfParticles];
			for (int layer = 0; layer < this.particles.GetLength(0); layer++)
			{
				for (int particleIndex = 0; particleIndex < this.numberOfParticles; particleIndex++)
				{
					//
					// Start with empty (dead) particles. 
					// Dead particles will be revived using actual data-driven activations once training starts.
					//
					this.particles[layer, particleIndex] = new List<int>(0);
				}
			}

			this.currentParticle = new int[this.LayerCount - 1];
		}
	}
}
