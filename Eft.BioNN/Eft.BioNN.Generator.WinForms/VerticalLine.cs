// -----------------------------------------------------------------------
// <copyright file="VerticalLine.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Generator.UI.WinForms
{
	using System.Drawing;

	/// <summary>
	/// A vertical line.
	/// </summary>
	public class VerticalLine : Shape
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
			float x = shapeArea.X + (shapeArea.Width / 2.0f);
			graphics.DrawLine(
				pen,
				new PointF(x, shapeArea.Y),
				new PointF(x, shapeArea.Y + shapeArea.Height));
		}
	}
}
