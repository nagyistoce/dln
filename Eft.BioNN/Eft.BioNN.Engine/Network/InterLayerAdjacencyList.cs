// -----------------------------------------------------------------------
// <copyright file="InterLayerAdjacencyList.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// An adjacency list implementation to store edge information.
	/// </summary>
	public class InterLayerAdjacencyList : AbstractInterLayerConnections
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InterLayerAdjacencyList"/> class.
		/// </summary>
		/// <param name="unitCountLeft">The number of units in the left partition (layer).</param>
		/// <param name="unitCountRight">The number of units in the right partition (layer).</param>
		public InterLayerAdjacencyList(int unitCountLeft, int unitCountRight)
			: base(unitCountLeft, unitCountRight)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the degree of the specified unit.
		/// </summary>
		/// <param name="index">The index of the unit.</param>
		/// <param name="partition">The partition that the unit resides in.</param>
		/// <returns>
		/// The degree of the unit.
		/// </returns>
		//public override int GetDegree(int index, Partition partition)
		//{
		//    throw new NotImplementedException();
		//}

		/// <summary>
		/// Gets the neighbours of the specified unit.
		/// </summary>
		/// <param name="index">The index of the unit.</param>
		/// <param name="partition">The partition that the unit resides in.</param>
		/// <param name="filterOutZeroWeightEdges">If set to <c>true</c> then edges with zero weights are not returned.</param>
		/// <returns>
		/// A sequence of edges incident to the specified unit.
		/// </returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public override IEnumerable<Edge> GetNeighbours(int index, Partition partition, bool filterOutZeroWeightEdges)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The internal implementation of <see cref="IInterLayerConnections.GetEdge" />. An inheriting class must implement this.
		/// </summary>
		protected override Edge GetEdgeInternal(int leftIndex, int rightIndex)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// The internal implementation of <see cref="IInterLayerConnections.SetEdge" />. An inheriting class must implement this.
		/// </summary>
		protected override int? SetEdgeInternal(int leftIndex, int rightIndex, int weight)
		{
			//
			// TODO: Optimize for initialization
			//
			throw new NotImplementedException();
		}
	}
}
