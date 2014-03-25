// -----------------------------------------------------------------------
// <copyright file="StationaryShapesProvider.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Data
{
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Reflection;

	/// <summary>
	/// A data provider for the Simple Shapes data set.
	/// </summary>
	public class StationaryShapesProvider : IDataProvider
	{
		/// <summary>
		/// The path to the data directory.
		/// </summary>
		private static readonly string DataDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", "..", "StationaryShapes");

		/// <summary>
		/// Gets the number of classes in the data set.
		/// </summary>
		/// <value>
		/// The number of classes.
		/// </value>
		public int NumberOfClasses
		{
			get
			{
				return 10;
			}
		}

		/// <summary>
		/// Gets the training set.
		/// </summary>
		/// <returns>
		/// An array of samples to use for training.
		/// </returns>
		public LabelledSample[] GetTrainingSet()
		{
			//
			// Load the training data form disk
			//
			var data = LoadData();
			//
			// Currently, there are only 10 images in my training set. Duplicate them to make the training set slightly larger.
			//
			int duplicates = 10;
			var duplicatedData = new LabelledSample[data.Length * duplicates];
			for (int i = 0; i < duplicatedData.Length; i++)
			{
				var original = data[i / duplicates];
				duplicatedData[i] = new LabelledSample(original.Label, original.SampleData);
			}
			//
			// Return the enlarged training set
			//
			return duplicatedData;
		}

		/// <summary>
		/// Gets the testing set.
		/// </summary>
		/// <returns>
		/// An array of samples to use for testing.
		/// </returns>
		public LabelledSample[] GetTestingSet()
		{
			//
			// TODO: Generate a separate testing set
			//
			return LoadData();
		}

		/// <summary>
		/// Loads the shape data from disk.
		/// </summary>
		/// <returns>An array with labelled images.</returns>
		private static LabelledSample[] LoadData()
		{
			var result = new List<LabelledSample>();
			//
			// Each class of images should be in a separate subdirectory of the data directory
			//
			foreach (var classDirectory in Directory.GetDirectories(DataDirectory))
			{
				//
				// Attempt to convert the subdirectory name into an integer (which corresponds to the class label).
				//
				var labelString = classDirectory.Substring(classDirectory.LastIndexOf(Path.DirectorySeparatorChar) + 1);
				int label;
				if (!int.TryParse(labelString, out label))
				{
					//
					// Hmm...this may be some "system" folder (e.g. svn)
					// Skip to the next folder
					//
					continue;
				}
				//
				// Iterate over the png images in this subfolder
				//
				foreach (var imagePath in Directory.GetFiles(classDirectory, "*.png"))
				{
					//
					// Load the png file into a bitmap object
					//
					using (var image = new Bitmap(imagePath))
					{
						//
						// Read the pixels into a 1D array
						//
						var sampleData = new byte[image.Width * image.Height];
						int index = 0;
						for (int y = 0; y < image.Height; y++)
						{
							for (int x = 0; x < image.Width; x++)	
							{
								//
								// I expect the images to be grayscale, but just to be sure, average the RGB color values
								//
								var color = image.GetPixel(x, y);
								var intensity = (color.R + color.G + color.B) / 3;
								sampleData[index++] = (byte)intensity;
							}
						}
						//
						// Add this image to the list of samples
						//
						result.Add(new LabelledSample(label, sampleData));
					}
				}
			}
			//
			// Return the samples as an array
			//
			return result.ToArray();
		}
	}
}
