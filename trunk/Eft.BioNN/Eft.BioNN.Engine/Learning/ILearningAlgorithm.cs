// -----------------------------------------------------------------------
// <copyright file="ILearningAlgorithm.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Learning
{
	using System;
	using System.Collections.Generic;
	using Eft.BioNN.Engine.Network;

	/// <summary>
	/// An interface for a learning algorithm.
	/// </summary>
	public interface ILearningAlgorithm
	{
		/// <summary>
		/// Processes a training sample.
		/// </summary>
		/// <param name="connections">The inter-layer connections.</param>
		/// <param name="activatedLeft">The activated units in the left partition.</param>
		/// <param name="activatedRight">The activated units in the right partition.</param>
		void ProcessSample(IInterLayerConnections connections, List<int> activatedLeft, List<int> activatedRight);

		/// <summary>
		/// Initializes the learning algorithm.
		/// </summary>
		/// <param name="maxInputActivations">The maximum number of activated input units.</param>
		void Initialize(int maxInputActivations);
	}
}
