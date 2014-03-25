// -----------------------------------------------------------------------
// <copyright file="TargetedRightDegree.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Learning
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Eft.BioNN.Engine.Network;
	using Eft.BioNN.Engine.Utils;

	/// <summary>
	/// A learning algorithm based on the approach in the code that Angelika sent me.
	/// Edges between concurrently active units are stochastically strengthened/weakened depending on the strong-degree of 
	/// the units in the right partition.
	/// </summary>
	/// <remarks>As is, this algorithm isn't really fit for anything other than shallow networks (with no hidden layers)</remarks>
	public class TargetedRightDegree : AbstractLearningAlgorithm
	{
		/// <summary>
		/// The target degree of a unit in the right partition.
		/// </summary>
		private int targetDegree;

		/// <summary>
		/// Initializes a new instance of the <see cref="TargetedRightDegree"/> class.
		/// </summary>
		/// <param name="targetDegree">The target degree.</param>
		public TargetedRightDegree(int targetDegree)
		{
			this.targetDegree = targetDegree;
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
			foreach (var outputUnit in activatedRight)
			{
				//
				// Find out how many strong (excitatory) edges are connected to this unit
				//
				int degree = connections.GetNeighbours(outputUnit, Partition.Right, true).Count();
				//
				// If it's equal to the target degree, move on to the next output unit
				//
				if (degree == this.targetDegree)
				{
					////Debug.WriteLine("Skipping output unit " + outputUnit + " because it has reached its target degree");
					continue;
				}
				//
				// Calculate the probability of an edge strengthening/weakening
				//
				double p = (double)(this.targetDegree - degree) / (2.0 * this.MaxInputActivations);
				//
				// Stochastically modify edges which connect concurrently active units
				//
				foreach (var inputUnit in activatedLeft)
				{
					var edge = connections.GetEdge(inputUnit, outputUnit);
					if (edge.Weight == 0 && p > 0.0 && random.NextDouble() < p)
					{
						connections.SetEdge(inputUnit, outputUnit, 1);
#if DEBUG
						Debug.WriteLine("Adding edge between " + inputUnit + " and " + outputUnit);
#endif
					}
					else if (edge.Weight == 1 && p < 0.0 && random.NextDouble() < -p)
					{
						connections.SetEdge(inputUnit, outputUnit, 0);
#if DEBUG
						Debug.WriteLine("Removing edge between " + inputUnit + " and " + outputUnit);
#endif
					}
				}
			}
			//
			// TODO: Prune from the left as well?
			//
		}
	}
}
