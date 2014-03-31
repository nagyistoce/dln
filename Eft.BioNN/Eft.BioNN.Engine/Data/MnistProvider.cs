// -----------------------------------------------------------------------
// <copyright file="MnistProvider.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Data
{
	using System.IO;
	using System.Reflection;

	/// <summary>
	/// A data provider for the MNIST data set of handwritten digits.
	/// </summary>
	/// <remarks>
	/// Code is adapted from Convolutional Neural Network Workbench by Filip D'haene 
	/// (http://www.codeproject.com/Articles/140631/Convolutional-Neural-Network-MNIST-Workbench)
	/// </remarks>
	public class MnistProvider : IDataProvider
	{	
		/// <summary>
		/// The name of the file with the training images
		/// </summary>
		private const string TrainingImageFileName = "train-images-idx3-ubyte";
		
		/// <summary>
		/// The name of the file with the labels for the training images
		/// </summary>
		private const string TrainingLabelFileName = "train-labels-idx1-ubyte";
		
		/// <summary>
		/// The name of the file with the testing images
		/// </summary>
		private const string TestingImageFileName = "t10k-images-idx3-ubyte";
		
		/// <summary>
		/// The name of the file with the labels for the testing images
		/// </summary>
		private const string TestingLabelFileName = "t10k-labels-idx1-ubyte";

		/// <summary>
		/// The mnist data directory
		/// </summary>
		private static readonly string MnistDataDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", "..", "MNIST");

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
		/// <returns>An array with the 60k labelled training samples. Each sample image is 28 * 28 pixels greyscale.</returns>
		public LabelledSample[] GetTrainingSet()
		{
			//
			// Load the images and their labels
			//
			var imagePath = Path.Combine(MnistDataDirectory, TrainingImageFileName);
			var labelPath = Path.Combine(MnistDataDirectory, TrainingLabelFileName);
			var trainingSet = LoadMnistSetFromFile(imagePath, labelPath);
			//
			// Fix wrongly assigned numbers in the MNIST training dataset
			//
			trainingSet[7080].Label = 5;
			trainingSet[10994].Label = 9;
			trainingSet[30751].Label = 0;
			trainingSet[35310].Label = 6;
			trainingSet[40144].Label = 3;
			trainingSet[43454].Label = 3;
			trainingSet[59915].Label = 7;
			//
			// Return the training set
			//
			return trainingSet;
		}

		/// <summary>
		/// Gets the testing set.
		/// </summary>
		/// <returns>An array with the 10k labelled testing samples. Each sample image is 28 * 28 pixels greyscale.</returns>
		public LabelledSample[] GetTestingSet()
		{
			var imagePath = Path.Combine(MnistDataDirectory, TestingImageFileName);
			var labelPath = Path.Combine(MnistDataDirectory, TestingLabelFileName);
			return LoadMnistSetFromFile(imagePath, labelPath);
		}

		/// <summary>
		/// Reads a big endian 32-bit integer from a byte array
		/// </summary>
		/// <param name="buffer">The byte array.</param>
		/// <param name="startPosition">The start position.</param>
		/// <returns>A 32-bit integer</returns>
		private static int ReadBigEndianInt32(byte[] buffer, int startPosition)
		{
			int result = ((int)buffer[startPosition]) << 24;
			result += ((int)buffer[startPosition + 1]) << 16;
			result += ((int)buffer[startPosition + 2]) << 8;
			return result + buffer[startPosition + 3];
		}

		/// <summary>
		/// Loads a set of images and labels.
		/// </summary>
		/// <param name="imagePath">The path to the file containing the images.</param>
		/// <param name="labelPath">The path to the file containing the labels.</param>
		/// <returns>An array where each element consists of an image and a label.</returns>
		private static LabelledSample[] LoadMnistSetFromFile(string imagePath, string labelPath)
		{
			//
			// Note: The MNIST file formats are described at the bottom of this web page: http://yann.lecun.com/exdb/mnist/
			// Open the image file
			//
			using (FileStream imageStream = File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				//
				// Read the header to find out how many images there are and the dimensions of each image.
				//
				byte[] headerBuffer = new byte[16];
				imageStream.Read(headerBuffer, 0, 16);
				int imageCount = ReadBigEndianInt32(headerBuffer, 4);
				int height = ReadBigEndianInt32(headerBuffer, 8);
				int width = ReadBigEndianInt32(headerBuffer, 12);
				int singleImageSize = width * height;
				//
				// Proceed to where the image data begins
				//
				imageStream.Seek(16, SeekOrigin.Begin);
				//
				// Construct the resulting array of labelled images
				//
				var result = new LabelledSample[imageCount];
				//
				// Open the file with the labels
				//
				using (FileStream labelStream = File.Open(labelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					//
					// Skip to where the label data begins
					//
					labelStream.Seek(8, SeekOrigin.Begin);
					//
					// Read the images and their corresponding labels
					//
					for (int index = 0; index < imageCount; index++)
					{
						var imageData = new byte[singleImageSize];
						imageStream.Read(imageData, 0, singleImageSize);
						result[index] = new LabelledSample(labelStream.ReadByte(), imageData);
					}
				}
				//
				// Return the labelled images
				//
				return result;
			}
		}
	}
}
