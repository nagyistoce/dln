// -----------------------------------------------------------------------
// <copyright file="IInterLayerConnections.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using System.Collections.Generic;

	/// <summary>
	/// An interface for a class that contains information about connections (edges) between 
	/// units (neurons/vertices) in two separate layers (partitions).
	/// </summary>
	public interface IInterLayerConnections
	{
		/// <summary>
		/// Gets the type of edge between the two specified units.
		/// </summary>
		/// <param name="leftIndex">Index of the unit in the left partition.</param>
		/// <param name="rightIndex">Index of the unit in the right partition.</param>
		/// <returns>The edge type.</returns>
		Edge GetEdge(int leftIndex, int rightIndex);

		/// <summary>
		/// Sets the type of edge between the two specified units.
		/// </summary>
		/// <param name="leftIndex">Index of the unit in the left partition.</param>
		/// <param name="rightIndex">Index of the unit in the right partition.</param>
		/// <param name="weight">The weight of the edge.</param>
		void SetEdge(int leftIndex, int rightIndex, int weight);

		/// <summary>
		/// Gets the degree of the specified unit.
		/// </summary>
		/// <param name="index">The index of the unit.</param>
		/// <param name="partition">The partition that the unit resides in.</param>
		/// <returns>The degree of the unit.</returns>
		//int GetDegree(int index, Partition partition); // TODO: Add support for predicate?

		/// <summary>
		/// Stochastically generates connections (edges) according to the specified probabilities.
		/// </summary>
		/// <returns>The total number of connections (edges) created.</returns>
		int Initialize(int minWeight, int maxWeight, double[] weightProbabilities);

		/// <summary>
		/// Gets the neighbours of the specified unit.
		/// </summary>
		/// <param name="index">The index of the unit.</param>
		/// <param name="partition">The partition that the unit resides in.</param>
		/// <param name="filterOutZeroWeightEdges">If set to <c>true</c> then edges with zero weights are not returned.</param>
		/// <returns>A sequence of edges incident to the specified unit.</returns>
		IEnumerable<Edge> GetNeighbours(int index, Partition partition, bool filterOutZeroWeightEdges);

		//
		// TODO: Add Clear() method?
		//
	}
}
