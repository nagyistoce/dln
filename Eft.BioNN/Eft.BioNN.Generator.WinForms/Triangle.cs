// -----------------------------------------------------------------------
// <copyright file="Triangle.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Generator.UI.WinForms
{
	using System.Drawing;

	/// <summary>
	/// A triangle.
	/// </summary>
	public class Triangle : Shape
	{
		/// <summary>
		/// Draws the shape.
		/// </summary>
		/// <param name="area">The area in which to draw the shape.</param>
		/// <param name="graphics">The graphics object to use.</param>
		/// <param name="pen">The pen to use.</param>
		public override void Draw(Rectangle area, Graphics graphics, Pen pen)
		{
			var shapeArea = this.PadRectangle(area);
			var points = new PointF[] 
			{
				new PointF(shapeArea.X, shapeArea.Y + shapeArea.Height),
				new PointF(shapeArea.X + (shapeArea.Width / 2.0f), shapeArea.Y),
				new PointF(shapeArea.X + shapeArea.Width, shapeArea.Y + shapeArea.Height)
			};
			
			graphics.DrawPolygon(pen, points);
		}
	}
}
