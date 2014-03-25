// -----------------------------------------------------------------------
// <copyright file="AbstractLearningAlgorithm.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Learning
{
	using System.Collections.Generic;
	using Eft.BioNN.Engine.Network;

	/// <summary>
	/// An abstract base class for implementations of a learning algorithm.
	/// </summary>
	public abstract class AbstractLearningAlgorithm : ILearningAlgorithm
	{
		/// <summary>
		/// The maximum number of activated input units.
		/// </summary>
		private int maxInputActivations;

		/// <summary>
		/// Gets or sets the maximum number of activated input units.
		/// </summary>
		/// <value>
		/// The maximum number of activated input units.
		/// </value>
		protected int MaxInputActivations
		{
			get 
			{ 
				return this.maxInputActivations; 
			}

			set 
			{ 
				this.maxInputActivations = value; 
			}
		}

		/// <summary>
		/// Processes a training sample.
		/// </summary>
		/// <param name="connections">The inter-layer connections.</param>
		/// <param name="activatedLeft">The activated units in the left partition.</param>
		/// <param name="activatedRight">The activated units in the right partition.</param>
		public abstract void ProcessSample(IInterLayerConnections connections, List<int> activatedLeft, List<int> activatedRight);

		/// <summary>
		/// Initializes the learning algorithm.
		/// </summary>
		/// <param name="maxInputActivations">The maximum number of activated input units.</param>
		public void Initialize(int maxInputActivations)
		{
			this.maxInputActivations = maxInputActivations;
		}
	}
}
