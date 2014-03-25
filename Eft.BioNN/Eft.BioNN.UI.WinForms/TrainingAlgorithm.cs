// -----------------------------------------------------------------------
// <copyright file="TrainingAlgorithm.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.UI.WinForms
{
	/// <summary>
	/// Enumerator for supported training algorithms
	/// </summary>
	public enum TrainingAlgorithm
	{
		/// <summary>
		/// The contrastive divergence (CD-1) training algorithm
		/// </summary>
		ContrastiveDivergence,

		/// <summary>
		/// The contrastive divergence (CD-1) training algorithm which uses sampling to estimate activation probabilities.
		/// </summary>
		ContrastiveDivergenceWithSampling,

		/// <summary>
		/// The persistent contrastive divergence (PCD) training algorithm.
		/// </summary>
		PersistentContrastiveDivergence
	}
}
