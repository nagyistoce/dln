// -----------------------------------------------------------------------
// <copyright file="ValidationUtils.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Utils
{
	using System;

	/// <summary>
	/// A container for input validation utility methods.
	/// </summary>
	public class ValidationUtils
	{
		/// <summary>
		/// Ensures that a given probability parameter is within the [0-1] range. Throws an <see cref="ArgumentException"/> if it isn't.
		/// </summary>
		/// <param name="probability">The probability.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		public static void ValidateProbability(double probability, string parameterName)
		{
			if (probability < 0.0 || probability > 1.0)
			{
				throw new ArgumentException("Probabilities must be in the range from 0.0 to 1.0", parameterName);
			}
		}
	}
}
