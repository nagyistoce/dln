// -----------------------------------------------------------------------
// <copyright file="ContrastiveDivergenceLearningMode.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Learning
{
	using System;

	/// <summary>
	/// Specifies which variant of Contrastive Divergence to use for supervised training
	/// </summary>
	public enum ContrastiveDivergenceLearningMode
	{
		/// <summary>
		/// Feed-forward training
		/// </summary>
		FeedForward,

		/// <summary>
		/// The normal method for learning the joint image-label distribution.
		/// </summary>
		JointNormal,

		/// <summary>
		/// Joint learning of image-label pairs using an alternative method for gathering the negative statistics.
		/// </summary>
		JointAlternative
	}
}
