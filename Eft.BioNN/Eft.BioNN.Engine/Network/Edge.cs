// -----------------------------------------------------------------------
// <copyright file="Edge.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	/// <summary>
	/// A (directed) edge in a graph.
	/// </summary>
	public struct Edge
	{
		/// <summary>
		/// The source vertex.
		/// </summary>
		public int Source;

		/// <summary>
		/// The target vertex.
		/// </summary>
		public int Target;

		/// <summary>
		/// The weight of the edge.
		/// </summary>
		public int Weight;
	}
}
