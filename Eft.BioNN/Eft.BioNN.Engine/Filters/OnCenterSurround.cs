// -----------------------------------------------------------------------
// <copyright file="OnCenterSurround.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013-2014 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Filters
{
	using System;
	using Eft.BioNN.Engine.Data;

	/// <summary>
	/// A filter that simulates ON center-surround ganglion cells in the retina.
	/// See <a href="http://en.wikipedia.org/wiki/Receptive_field#Retinal_ganglion_cells">Wikipedia article on center-surround cells</a>.
	/// </summary>
	public class OnCenterSurround : AbstractFilter
	{
		/// <summary>
		/// The excitatory multiplication factor (for the center photoreceptor)
		/// </summary>
		private const double Excitatory = 1.7;

		/// <summary>
		/// The inhibitory multiplication factor (for the surround photoreceptors)
		/// </summary>
		private const double Inhibitory = -Excitatory / (1.1 * 8.0);

		/// <summary>
		/// The mask used to simulate the ON center-surround cell.
		/// </summary>
		private double[,] onCenterSurroundMask = new double[,] 
		{ 
			{ Inhibitory, Inhibitory, Inhibitory },
			{ Inhibitory, Excitatory, Inhibitory },
			{ Inhibitory, Inhibitory, Inhibitory }
		};

		/// <summary>
		/// Gets the mask used to simulate the ON center-surround cell.
		/// </summary>
		/// <value>
		/// The mask used to simulate the ON center-surround cell.
		/// </value>
		protected double[,] OnCenterSurroundMask
		{
			get
			{
				return this.onCenterSurroundMask;
			}
		}

		/// <summary>
		/// Applies the filter to an individual sample from the input data set.
		/// </summary>
		/// <param name="sample">The sample from the data set.</param>
		protected override void ProcessSampleData(Sample sample)
		{
			sample.SampleData = this.MaskImage(sample.SampleData, this.onCenterSurroundMask);
		}

		/// <summary>
		/// Applies a mask (matrix) to combine values from a small neighbourhood of pixels into a new pixel value.
		/// </summary>
		/// <param name="imageData">The image data.</param>
		/// <param name="mask">The mask.</param>
		/// <returns>The filtered image.</returns>
		protected byte[] MaskImage(byte[] imageData, double[,] mask)
		{
			//
			// Create an array for the resulting image.
			//
			byte[] result = new byte[imageData.Length];
			//
			// Inspect the size of the mask matrix. 
			// From this deduce the offset from the current output pixel to the top-left input pixel.
			//
			int maskOffsetX = -mask.GetLength(0) / 2;
			int maskOffsetY = -mask.GetLength(1) / 2;
			//
			// Figure out the dimension(s) of the input image. We assume it to be square.
			//
			int widthAndheight = Convert.ToInt32(Math.Sqrt(imageData.Length));
			//
			// For each output Y coordinate...
			//
			for (int imageY = 0; imageY < widthAndheight; imageY++)
			{
				//
				// For each output X coordinate...
				//
				for (int imageX = 0; imageX < widthAndheight; imageX++)
				{
					//
					// Calculate the intensity of the output pixel by applying the mask to the input pixels
					//
					double maskedValue = 0.0;
					for (int maskY = 0; maskY < mask.GetLength(1); maskY++)
					{
						for (int maskX = 0; maskX < mask.GetLength(0); maskX++)
						{
							//
							// Read the intensity of the input pixel
							//
							byte sourceValue = 0;
							int sourceX = imageX + maskX + maskOffsetX;
							int sourceY = imageY + maskY + maskOffsetY;
							if (this.IsValidCoordinate(sourceX, sourceY, widthAndheight))
							{
								sourceValue = imageData[(sourceY * widthAndheight) + sourceX];
							}
							//
							// Multiply the input pixel intensity with the corresponding mask factor
							//
							maskedValue += mask[maskX, maskY] * sourceValue;
						}
					}
					//
					// Ensure the masked value is within the byte range 
					//
					maskedValue = Math.Min(maskedValue, byte.MaxValue);
					maskedValue = Math.Max(maskedValue, byte.MinValue);
					//
					// Store the output value 
					//
					result[(imageY * widthAndheight) + imageX] = Convert.ToByte(Math.Round(maskedValue));
				}
			}
			//
			// Return the masked image
			//
			return result;
		}

		/// <summary>
		/// Determines whether the specified value is between the specified bounds (inclusive)
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="lower">The lower bound.</param>
		/// <param name="upper">The upper bound.</param>
		/// <returns><c>true</c> if the value is within the bounds, <c>false</c> otherwise.</returns>
		private bool IsBetween(int value, int lower, int upper)
		{
			return value >= lower && value <= upper;
		}

		/// <summary>
		/// Determines whether the supplied coordinates are within an image of the specified width and height.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="widthAndheight">The width and height of the image.</param>
		/// <returns><c>true</c> if the coordinate is within the image rectangle, <c>false</c> otherwise.</returns>
		private bool IsValidCoordinate(int x, int y, int widthAndheight)
		{
			return this.IsBetween(x, 0, widthAndheight - 1) && this.IsBetween(y, 0, widthAndheight - 1);
		}
	}
}
