// -----------------------------------------------------------------------
// <copyright file="MovingShapesProvider.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Data
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using Ionic.Zip;

	/// <summary>
	/// A data provider for images of moving shapes.
	/// </summary>
	public class MovingShapesProvider : IDataProvider
	{
		/// <summary>
		/// The path to the data directory.
		/// </summary>
		private static readonly string DataDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", "..", "MovingShapes");

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
			return LoadData(Path.Combine(DataDirectory, "Training.zip"));
		}

		/// <summary>
		/// Gets the testing set.
		/// </summary>
		/// <returns>
		/// An array of samples to use for testing.
		/// </returns>
		public LabelledSample[] GetTestingSet()
		{
			return LoadData(Path.Combine(DataDirectory, "Testing.zip"));
		}

		/// <summary>
		/// Loads the shape data from a zip archive.
		/// </summary>
		/// <param name="dataFilePath">The path to the zip file.</param>
		/// <returns>
		/// An array with labelled images.
		/// </returns>
		private static LabelledSample[] LoadData(string dataFilePath)
		{
			var result = new List<LabelledSample>();
			//
			// Open the zip file
			//
			using (var zipFile = ZipFile.Read(dataFilePath))
			{
				//
				// Iterate over all PNG images in the zip file
				//
				foreach (var entry in zipFile.Where(e => e.IsDirectory == false && e.FileName.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase)))
				{
					//
					// Attempt to convert the subdirectory name into an integer (which corresponds to the class label).
					//
					var labelString = entry.FileName.Substring(0, entry.FileName.IndexOf('/'));
					int label;
					if (!int.TryParse(labelString, out label))
					{
						//
						// Hmm...this may be some "system" folder (e.g. svn)
						// Skip to the next entry
						//
						continue;
					}
					//
					// Extract this entry into a memory stream (i.e. buffer)
					//
					using (MemoryStream stream = new MemoryStream((int)entry.UncompressedSize))
					{
						entry.Extract(stream);
						//
						// Rewind the tape
						//
						stream.Seek(0, SeekOrigin.Begin);
						//
						// Load the image from the memory stream
						//
						using (var image = new Bitmap(stream))
						{
							//
							// Read the pixels into a 1D array
							//
							var sampleData = ConvertBitmapToByteArray(image);
							//
							// Add this image to the list of samples
							//
							result.Add(new LabelledSample(label, sampleData));
						}
					}
				}
			}
			//
			// Return the samples as an array
			//
			return result.ToArray();
		}

		/// <summary>
		/// Converts a bitmap image to one-dimensional byte array where each value corresponds to the intensity of a single pixel.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <returns>An array of byte-valued pixel intensities.</returns>
		private static byte[] ConvertBitmapToByteArray(Bitmap image)
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
			// Return the array
			//
			return sampleData;
		}
	}
}
