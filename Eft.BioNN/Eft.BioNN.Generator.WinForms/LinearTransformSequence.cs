// -----------------------------------------------------------------------
// <copyright file="LinearTransformSequence.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Generator.UI.WinForms
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;

	/// <summary>
	/// A linear progression from one transformation to another in a sequence of steps (frames).
	/// </summary>
	public class LinearTransformSequence
	{
		/// <summary>
		/// The translation at the beginning.
		/// </summary>
		private PointF translateBegin;

		/// <summary>
		/// The translation at the end.
		/// </summary>
		private PointF translateEnd;

		/// <summary>
		/// The scaling factor at the beginning.
		/// </summary>
		private float scaleBegin;

		/// <summary>
		/// The scaling factor at the end.
		/// </summary>
		private float scaleEnd;

		/// <summary>
		/// The number of steps in the sequence.
		/// </summary>
		private int steps;

		/// <summary>
		/// Initializes a new instance of the <see cref="LinearTransformSequence"/> class.
		/// </summary>
		/// <param name="translateBegin">The translation at the beginning.</param>
		/// <param name="translateEnd">The translation at the end.</param>
		/// <param name="scaleBegin">The scaling factor at the beginning.</param>
		/// <param name="scaleEnd">The scaling factor at the end.</param>
		/// <param name="steps">The number of steps in the sequence.</param>
		public LinearTransformSequence(PointF translateBegin, PointF translateEnd, float scaleBegin, float scaleEnd, int steps)
		{
			this.translateBegin = translateBegin;
			this.translateEnd = translateEnd;
			this.scaleBegin = scaleBegin;
			this.scaleEnd = scaleEnd;
			this.steps = steps;
		}

		/// <summary>
		/// Gets the frames (i.e. the steps) in the sequence.
		/// </summary>
		/// <returns>A sequence of (translation; scalingFactor) tuples.</returns>
		public IEnumerable<Tuple<PointF, float>> GetFrames()
		{
			for (int i = 0; i < this.steps; i++)
			{
				//
				// Calculate the coefficients
				//
				var b = i / (this.steps - 1f);
				var a = 1 - b;
				//
				// Calculate the linear combination of the scaling factor and the translation
				//
				var scale = (a * this.scaleBegin) + (b * this.scaleEnd);
				var translation = new PointF(
					(a * this.translateBegin.X) + (b * this.translateEnd.X), 
					(a * this.translateBegin.Y) + (b * this.translateEnd.Y));
				//
				// Return the pair
				//
				yield return new Tuple<PointF, float>(translation, scale);
			}
		}
	}
}
