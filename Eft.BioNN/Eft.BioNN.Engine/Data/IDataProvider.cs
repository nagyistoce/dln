// -----------------------------------------------------------------------
// <copyright file="IDataProvider.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Data
{
	/// <summary>
	/// An interface for providers of training and testing data.
	/// </summary>
	public interface IDataProvider
	{
		/// <summary>
		/// Gets the number of classes in the data set.
		/// </summary>
		/// <value>
		/// The number of classes.
		/// </value>
		int NumberOfClasses { get; }

		/// <summary>
		/// Gets the training set.
		/// </summary>
		/// <returns>An array of samples to use for training.</returns>
		LabelledSample[] GetTrainingSet();

		/// <summary>
		/// Gets the testing set.
		/// </summary>
		/// <returns>An array of samples to use for testing.</returns>
		LabelledSample[] GetTestingSet();
	}
}
