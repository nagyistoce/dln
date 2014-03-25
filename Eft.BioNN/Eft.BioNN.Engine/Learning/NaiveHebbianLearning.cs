// -----------------------------------------------------------------------
// <copyright file="NaiveHebbianLearning.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Learning
{
	using System;
	using System.Collections.Generic;
	using Eft.BioNN.Engine.Network;
	using Eft.BioNN.Engine.Utils;

	/// <summary>
	/// A naive Hebbian Learning algorithm.
	/// It stochastically turns neutral edges into excitatory edges if they connect two neurons that are simultaneously active.
	/// </summary>
	/// <remarks>
	/// This algorithm is almost useless since it
	/// a) only adds edges and thus is not stable and
	/// b) causes all neurons to like all inputs during unsupervised learning.
	/// </remarks>
	public class NaiveHebbianLearning : AbstractLearningAlgorithm
	{
		/// <summary>
		/// The strengthening probability
		/// </summary>
		private double strengtheningProbability;

		/// <summary>
		/// Initializes a new instance of the <see cref="NaiveHebbianLearning"/> class.
		/// </summary>
		/// <param name="strengtheningProbability">The strengthening probability.</param>
		public NaiveHebbianLearning(double strengtheningProbability)
		{
			if (strengtheningProbability < 0.0 || strengtheningProbability > 1.0)
			{
				throw new ArgumentException("Probabilities must be in the range from 0 to 1", "strengtheningProbability");
			}

			this.strengtheningProbability = strengtheningProbability;
		}

		/// <summary>
		/// Processes a training sample.
		/// </summary>
		/// <param name="connections">The inter-layer connections.</param>
		/// <param name="activatedLeft">The activated units in the left partition.</param>
		/// <param name="activatedRight">The activated units in the right partition.</param>
		public override void ProcessSample(IInterLayerConnections connections, List<int> activatedLeft, List<int> activatedRight)
		{
			var random = ThreadSafeRandom.GetThreadRandom();
			foreach (var leftUnit in activatedLeft)
			{
				foreach (var rightUnit in activatedRight)
				{
					if (connections.GetEdge(leftUnit, rightUnit).Weight == 0)
					{
						if (random.NextDouble() < this.strengtheningProbability)
						{
							connections.SetEdge(leftUnit, rightUnit, 1);
						}
					}
				}
			}
		}
	}
}
