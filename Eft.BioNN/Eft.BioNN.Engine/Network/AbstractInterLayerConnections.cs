// -----------------------------------------------------------------------
// <copyright file="AbstractInterLayerConnections.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using Eft.BioNN.Engine.Utils;

	/// <summary>
	/// An abstract base class for inter-layer connections (edges).
	/// </summary>
	public abstract class AbstractInterLayerConnections : IInterLayerConnections
	{
		//
		// TODO: Skip validation checks when compiled in Release mode?
		//

		/// <summary>
		/// The number of units in the left partition (layer)
		/// </summary>
		private int unitCountLeft;

		/// <summary>
		/// The number of units in the right partition (layer)
		/// </summary>
		private int unitCountRight;

		/// <summary>
		/// The number of edges, indexed by type
		/// </summary>
		//private int[] edgeCounts;

		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractInterLayerConnections"/> class.
		/// </summary>
		/// <param name="unitCountLeft">The number of units in the left partition (layer).</param>
		/// <param name="unitCountRight">The number of units in the right partition (layer).</param>
		public AbstractInterLayerConnections(int unitCountLeft, int unitCountRight)
		{
			//
			// Input validation
			//
			if (unitCountLeft <= 0 || unitCountRight <= 0)
			{
				throw new ArgumentException("There must be a positive number of units in the both partitions");
			}

			this.unitCountLeft = unitCountLeft;
			this.unitCountRight = unitCountRight;
			//
			// Initialize the edge-count array
			//
			//this.edgeCounts = new int[((byte[])Enum.GetValues(typeof(EdgeType))).Max() + 1];
			//this.edgeCounts[(int)EdgeType.NoEdge] = unitCountLeft * unitCountRight;
		}

		#region Properties
		/// <summary>
		/// Gets or sets the number of units in the left partition (layer)
		/// </summary>
		/// <value>
		/// The number of units in the left partition (layer)
		/// </value>
		protected int UnitCountLeft
		{
			get
			{
				return this.unitCountLeft;
			}

			set
			{
				this.unitCountLeft = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of units in the right partition (layer)
		/// </summary>
		/// <value>
		/// The number of units in the right partition (layer)
		/// </value>
		protected int UnitCountRight
		{
			get
			{
				return this.unitCountRight;
			}

			set
			{
				this.unitCountRight = value;
			}
		}
		#endregion

		/// <summary>
		/// Gets the number of edges of the specified type.
		/// </summary>
		/// <param name="edgeType">The type of the edges which should be counted.</param>
		/// <returns>The number of edges of the specified count.</returns>
		//public int GetEdgeCount(EdgeType edgeType)
		//{
		//    return this.edgeCounts[(int)edgeType];
		//}

		/// <summary>
		/// Gets the type of edge between the two specified units.
		/// </summary>
		/// <param name="leftIndex">Index of the unit in the left partition.</param>
		/// <param name="rightIndex">Index of the unit in the right partition.</param>
		/// <returns>
		/// The edge type.
		/// </returns>
		public Edge GetEdge(int leftIndex, int rightIndex)
		{
			//
			// Perform validation here, then ask the subclass to finish the work.
			//
			this.ValidateIndices(leftIndex, rightIndex);
			return this.GetEdgeInternal(leftIndex, rightIndex);
		}

		/// <summary>
		/// Sets the type of edge between the two specified units.
		/// </summary>
		/// <param name="leftIndex">Index of the unit in the left partition.</param>
		/// <param name="rightIndex">Index of the unit in the right partition.</param>
		/// <param name="weight">The weight of the edge.</param>
		public void SetEdge(int leftIndex, int rightIndex, int weight)
		{
			//
			// Perform validation and edge counting here, then ask the subclass to finish the work.
			//
			this.ValidateIndices(leftIndex, rightIndex);
			var previousWeight = this.SetEdgeInternal(leftIndex, rightIndex, weight);
			if (previousWeight != weight)
			{
				//Interlocked.Decrement(ref this.edgeCounts[(int)previousWeight]);
				//Interlocked.Increment(ref this.edgeCounts[(int)edgeType]);
			}
		}

		/// <summary>
		/// Gets the degree of the specified unit.
		/// </summary>
		/// <param name="index">The index of the unit.</param>
		/// <param name="partition">The partition that the unit resides in.</param>
		/// <returns>
		/// The degree of the unit.
		/// </returns>
		//public abstract int GetDegree(int index, Partition partition);

		/// <summary>
		/// Stochastically generates connections (edges) according to the specified probabilities.
		/// </summary>
		/// <returns>
		/// The total number of connections (edges) created.
		/// </returns>
		public int Initialize(int minWeight, int maxWeight, double[] weightProbabilities)
		{
			int numberOfWeights = maxWeight - minWeight + 1;
			//
			// TODO: Use an exact number type for the probabilities?
			//
			if (numberOfWeights != weightProbabilities.Length)
			{
				throw new ArgumentException("The number of weights and the size of the weightProbabilities array should match", "weightProbabilities");
			}

			var cumulativeProbabilities = new double[weightProbabilities.Length];
			var runningSum = 0.0;
			for (int i = 0; i < numberOfWeights; i++)
			{
				ValidationUtils.ValidateProbability(weightProbabilities[i], "weightProbabilities[" + i + "]");
				cumulativeProbabilities[i] = weightProbabilities[i] + runningSum;
				runningSum += weightProbabilities[i];
			}

			var random = ThreadSafeRandom.GetThreadRandom();
			//
			// TODO: Clear everything first?
			// Consider all possible edges, then flip a coin (so to speak)
			//
			int edgesCreated = 0;
			for (int leftUnit = 0; leftUnit < this.unitCountLeft; leftUnit++)
			{
				for (int rightUnit = 0; rightUnit < this.unitCountRight; rightUnit++)
				{
					var randomValue = random.NextDouble();

					for (int weightIndex = 0; weightIndex < numberOfWeights; weightIndex++)
					{
						if (randomValue < cumulativeProbabilities[weightIndex])
						{
							this.SetEdge(leftUnit, rightUnit, minWeight + weightIndex);
							edgesCreated++;
							break;
						}
					}
				}
			}
			//
			// Return the number of edges created
			//
			return edgesCreated;
		}

		/// <summary>
		/// Gets the neighbours of the specified unit.
		/// </summary>
		/// <param name="index">The index of the unit.</param>
		/// <param name="partition">The partition that the unit resides in.</param>
		/// <param name="filterOutZeroWeightEdges">If set to <c>true</c> then edges with zero weights are not returned.</param>
		/// <returns>
		/// A sequence of edges incident to the specified unit.
		/// </returns>
		public abstract IEnumerable<Edge> GetNeighbours(int index, Partition partition, bool filterOutZeroWeightEdges);

		/// <summary>
		/// The internal implementation of <see cref="IInterLayerConnections.GetEdge"/>. An inheriting class must implement this.
		/// </summary>
		protected abstract Edge GetEdgeInternal(int leftIndex, int rightIndex);

		/// <summary>
		/// The internal implementation of <see cref="IInterLayerConnections.SetEdge"/>. An inheriting class must implement this.
		/// </summary>
		protected abstract int? SetEdgeInternal(int leftIndex, int rightIndex, int weight);

		/// <summary>
		/// Ensures that unit indices are within bounds.
		/// </summary>
		private void ValidateIndices(int leftIndex, int rightIndex)
		{
			if (leftIndex < 0 || leftIndex >= this.unitCountLeft)
			{
				throw new ArgumentException("The left unit index must be in the range 0 to " + (this.unitCountLeft - 1), "leftIndex");
			}

			if (rightIndex < 0 || rightIndex >= this.unitCountRight)
			{
				throw new ArgumentException("The right unit index must be in the range 0 to " + (this.unitCountRight - 1), "rightIndex");
			}
		}
	}
}
