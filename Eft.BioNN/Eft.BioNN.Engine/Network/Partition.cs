// -----------------------------------------------------------------------
// <copyright file="Partition.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Network
{
	using System;

	/// <summary>
	/// The partitions of a bi-partite graph
	/// </summary>
	public enum Partition : byte
	{
		/// <summary>
		/// The left partition
		/// </summary>
		Left,

		/// <summary>
		/// The right partition
		/// </summary>
		Right
	}
}
