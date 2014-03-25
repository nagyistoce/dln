// -----------------------------------------------------------------------
// <copyright file="Shape.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Generator.UI.WinForms
{
	using System;
	using System.Drawing;
	using System.Linq;

	/// <summary>
	/// An abstract base class for shapes.
	/// </summary>
	public abstract class Shape
	{
		/// <summary>
		/// The amount of padding around the shape.
		/// </summary>
		private const double Padding = 0.2;

		/// <summary>
		/// Finds all classes in the current assembly that inherit from the Shape class.
		/// </summary>
		/// <returns>An array containing a single instance of every shape.</returns>
		public static Shape[] GetAllShapes()
		{
			var subclasses = typeof(Shape).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Shape))).ToArray();
			var result = new Shape[subclasses.Length];
			for (int i = 0; i < subclasses.Length; i++)
			{
				result[i] = subclasses[i].GetConstructor(new Type[0]).Invoke(new object[0]) as Shape;
			}

			return result;
		}

		/// <summary>
		/// Draws the shape.
		/// </summary>
		/// <param name="area">The area in which to draw the shape.</param>
		/// <param name="graphics">The graphics object to use.</param>
		/// <param name="pen">The pen to use.</param>
		public abstract void Draw(Rectangle area, Graphics graphics, Pen pen);

		/// <summary>
		/// Constructs a new rectangle that is fitted within the specified <paramref name="area"/> such that there's 20% padding 
		/// in all directions.
		/// </summary>
		/// <param name="area">The area within which the rectangle should be placed.</param>
		/// <returns>A smaller rectangle</returns>
		protected RectangleF PadRectangle(Rectangle area)
		{
			double scalingFactor = 1 - Padding - Padding;
			var result = new RectangleF();

			result.Height = Convert.ToSingle(scalingFactor * area.Height);
			result.Width = Convert.ToSingle(scalingFactor * area.Width);

			result.X = Convert.ToSingle(Padding * area.Width);
			result.Y = Convert.ToSingle(Padding * area.Height);

			return result;
		}
	}
}
