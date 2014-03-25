// -----------------------------------------------------------------------
// <copyright file="InterLayerAdjacencyMatrix.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// An adjacency matrix implementation to store edge information.
	/// </summary>
	public class InterLayerAdjacencyMatrix : AbstractInterLayerConnections
	{
		/// <summary>
		/// The adjacency matrix
		/// </summary>
		private int[][] adjacencyMatrix;

		/// <summary>
		/// The adjacency matrix transposed. 
		/// Since .NET stores arrays in row-major format, traversing rows is faster than columns due to memory alignment.
		/// By maintaining the transpose of the adjacency matrix, we can much more efficiently return the neighbours 
		/// of a unit in the right partition (which requires traversing a column in the original adjacency matrix).
		/// </summary>
		private int[][] adjacencyMatrixTranspose;

		/// <summary>
		/// Initializes a new instance of the <see cref="InterLayerAdjacencyMatrix"/> class.
		/// </summary>
		/// <remarks>
		/// This constructor is here only to satisfy the requirement for a parameterless constructor
		/// </remarks>
		public InterLayerAdjacencyMatrix() : base(1, 1) 
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InterLayerAdjacencyMatrix"/> class.
		/// </summary>
		/// <param name="unitCountLeft">The number of units in the left partition (layer).</param>
		/// <param name="unitCountRight">The number of units in the right partition (layer).</param>
		public InterLayerAdjacencyMatrix(int unitCountLeft, int unitCountRight)
			: base(unitCountLeft, unitCountRight)
		{
			this.adjacencyMatrix = this.InitializeMatrix<int>(unitCountLeft, unitCountRight);
			this.adjacencyMatrixTranspose = this.InitializeMatrix<int>(unitCountRight, unitCountLeft);
		}

		/// <summary>
		/// Gets the degree of the specified unit.
		/// </summary>
		/// <param name="index">The index of the unit.</param>
		/// <param name="partition">The partition that the unit resides in.</param>
		/// <param name="typeMask">A bitmask which specifies which type of edges to count.</param>
		/// <returns>
		/// The degree of the unit.
		/// </returns>
		//public override int GetDegree(int index, Partition partition, EdgeType typeMask)
		//{
		//    //
		//    // Get the adjacency vector for this unit
		//    //
		//    EdgeType[] otherUnits = this.GetAdjacencyVector(index, partition);
		//    //
		//    // Count the edges that match the bitmask 
		//    //
		//    return otherUnits.Count(x => (x & typeMask) > 0);
		//}

		/// <summary>
		/// Gets the neighbours of the specified unit.
		/// </summary>
		/// <param name="sourceUnit">The index of the unit.</param>
		/// <param name="partition">The partition that the unit resides in.</param>
		/// <returns>
		/// A sequence of edges incident to the specified unit.
		/// </returns>
		public override IEnumerable<Edge> GetNeighbours(int sourceUnit, Partition partition, bool filterOutZeroWeightEdges)
		{
			//
			// Get the adjacency vector for this unit
			//
			int[] otherUnits = this.GetAdjacencyVector(sourceUnit, partition);
			//
			// Iterate through the vector and return edges of the desired type
			//
			for (int otherIndex = 0; otherIndex < otherUnits.Length; otherIndex++)
			{
				int weight = otherUnits[otherIndex];
				if (!filterOutZeroWeightEdges || weight != 0)
				{
					yield return new Edge() { Source = sourceUnit, Target = otherIndex, Weight = weight };
				}
			}
		}

		/// <summary>
		/// The internal implementation of <see cref="IInterLayerConnections.GetEdge" />. An inheriting class must implement this.
		/// </summary>
		protected override Edge GetEdgeInternal(int leftIndex, int rightIndex)
		{
			return new Edge() { Source = leftIndex, Target = rightIndex, Weight = this.adjacencyMatrix[leftIndex][rightIndex] };
		}

		/// <summary>
		/// The internal implementation of <see cref="IInterLayerConnections.SetEdge" />. An inheriting class must implement this.
		/// </summary>
		protected override int? SetEdgeInternal(int leftIndex, int rightIndex, int weight)
		{
			var oldValue = this.adjacencyMatrix[leftIndex][rightIndex];
			this.adjacencyMatrix[leftIndex][rightIndex] = weight;
			this.adjacencyMatrixTranspose[rightIndex][leftIndex] = weight;
			return oldValue;
		}

		/// <summary>
		/// Initializes a matrix as a jagged array.
		/// </summary>
		/// <param name="rows">The number of rows.</param>
		/// <param name="columns">The number of columns.</param>
		/// <returns>A jagged array.</returns>
		private T[][] InitializeMatrix<T>(int rows, int columns)
		{
			var result = new T[rows][];
			for (int row = 0; row < rows; row++)
			{
				result[row] = new T[columns];
			}

			return result;
		}

		/// <summary>
		/// Gets the adjacency vector of a single unit (vertex).
		/// </summary>
		/// <param name="index">The index of the unit.</param>
		/// <param name="partition">The partition that the unit resides in.</param>
		/// <returns>A vector (array) of EdgeType objects</returns>
		private int[] GetAdjacencyVector(int index, Partition partition)
		{
			if (partition == Partition.Right)
			{
				return this.adjacencyMatrixTranspose[index];
			}
			else
			{
				return this.adjacencyMatrix[index];
			}
		}
	}
}