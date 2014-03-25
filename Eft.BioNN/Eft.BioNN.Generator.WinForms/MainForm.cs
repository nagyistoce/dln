// -----------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Generator.UI.WinForms
{
	using System;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Threading;
	using System.Windows.Forms;

	/// <summary>
	/// The main window of the shape generator.
	/// </summary>
	public partial class MainForm : Form
	{
		#region Private members
		/// <summary>
		/// The shape currently visible in the preview panel.
		/// </summary>
		private Shape currentShape = new Circle();

		/// <summary>
		/// The current scaling factor (of the shape).
		/// </summary>
		private float currentScale = 1.0f;

		/// <summary>
		/// The current translation (of the shape).
		/// </summary>
		private PointF currentTranslation = new PointF(0, 0);

		/// <summary>
		/// A random number generator.
		/// </summary>
		private Random random = new Random(6839);

		/// <summary>
		/// An array containing an instance of every possible shape.
		/// </summary>
		private Shape[] allShapes;
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="MainForm"/> class.
		/// </summary>
		public MainForm()
		{
			this.InitializeComponent();
		}
		#endregion

		#region Event handlers
		/// <summary>
		/// Handles the Paint event of the shapeViewPanel control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
		private void shapeViewPanel_Paint(object sender, PaintEventArgs e)
		{
			//
			// Set the stroke width relative to the size of the panel
			//
			var width = Convert.ToDouble(this.sizeUpDown.Value) / 12.0;
			//
			// Create a pen object, used to draw the outlines of the shape
			//
			using (Pen myPen = new Pen(Color.White, (float)width))
			{
				//
				// Anti-alias the image
				//
				var graphics = e.Graphics;
				graphics.SmoothingMode = SmoothingMode.AntiAlias;

				//
				// Translate the origin to the center of the panel's display rectangle
				//
				var displayArea = shapeViewPanel.DisplayRectangle;
				graphics.TranslateTransform(
					displayArea.Width / 2, 
					displayArea.Height / 2);
				//
				// Now we can perform the scaling from the center of the 'image'
				//
				graphics.ScaleTransform(this.currentScale, this.currentScale);
				//
				// Translate the origin back to the upper left corner AND apply the current translation of the image.
				//
				graphics.TranslateTransform(
					this.currentTranslation.X - (displayArea.Width / 2), 
					this.currentTranslation.Y - (displayArea.Height / 2));
				//
				// Draw the shape
				//
				this.currentShape.Draw(displayArea, graphics, myPen);
			}
		}

		/// <summary>
		/// Handles the ValueChanged event of the sizeUpDown control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void sizeUpDown_ValueChanged(object sender, EventArgs e)
		{
			//
			// Resize the panel in which the shape is drawn.
			//
			int length = (int)this.sizeUpDown.Value;
			this.shapeViewPanel.Size = new Size(length, length);
			this.shapeViewPanel.Invalidate();
		}

		/// <summary>
		/// Handles the Load event of the MainForm control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void MainForm_Load(object sender, EventArgs e)
		{
			//
			// Initialize the size of the panel which shows the shape
			//
			this.sizeUpDown_ValueChanged(this, new EventArgs());
			//
			// Restore the path to the output folder
			//
			this.folderTextBox.Text = Properties.Settings.Default.OutputFolder;
			if (string.IsNullOrEmpty(this.folderTextBox.Text))
			{
				this.folderTextBox.Text = Application.StartupPath;
			}
			//
			// Load all shapes into an array.
			//
			this.allShapes = Shape.GetAllShapes();
		}

		/// <summary>
		/// Handles the FormClosed event of the MainForm control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			//
			// Save the path to the output folder.
			//
			Properties.Settings.Default.OutputFolder = this.folderTextBox.Text;
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// Handles the Click event of the selectFolderButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void selectFolderButton_Click(object sender, EventArgs e)
		{
			this.folderBrowserDialog.SelectedPath = this.folderTextBox.Text;
			if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				this.folderTextBox.Text = this.folderBrowserDialog.SelectedPath;
			}
		}

		/// <summary>
		/// Handles the Click event of the animateButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void animateButton_Click(object sender, EventArgs e)
		{
			//
			// Show a short animation sequence
			//
			var animation = this.GetRandomAnimation(100);
			foreach (var frame in animation.GetFrames())
			{
				this.currentTranslation = frame.Item1;
				this.currentScale = frame.Item2;
				this.shapeViewPanel.Invalidate();
				Application.DoEvents();
				Thread.Sleep(33);
			}
		}

		/// <summary>
		/// Handles the Click event of the generateImagesButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void generateImagesButton_Click(object sender, EventArgs e)
		{
			this.GenerateImages();
		}

		#region Shape radio buttons
		//
		// Having a separate event handler for each radio button is a copy/paste hack.
		// Clearly, this can be generalized into a single handler.
		//

		/// <summary>
		/// Handles the CheckedChanged event of the circleRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void circleRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.circleRadioButton.Checked)
			{
				this.SetCurrentShape(new Circle());
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the squareRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void squareRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.squareRadioButton.Checked)
			{
				this.SetCurrentShape(new Square());
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the triangleRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void triangleRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.triangleRadioButton.Checked)
			{
				this.SetCurrentShape(new Triangle());
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the diamondRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void diamondRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.diamondRadioButton.Checked)
			{
				this.SetCurrentShape(new Diamond());
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the circleLeftHalfRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void circleLeftHalfRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.circleLeftHalfRadioButton.Checked)
			{
				this.SetCurrentShape(new CircleLeftHalf());
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the circleRightHalfRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void circleRightHalfRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.circleRightHalfRadioButton.Checked)
			{
				this.SetCurrentShape(new CircleRightHalf());
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the circleTopHalfRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void circleTopHalfRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.circleTopHalfRadioButton.Checked)
			{
				this.SetCurrentShape(new CircleTopHalf());
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the circleBottomHalfRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void circleBottomHalfRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.circleBottomHalfRadioButton.Checked)
			{
				this.SetCurrentShape(new CircleBottomHalf());
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the verticalLineRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void verticalLineRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.verticalLineRadioButton.Checked)
			{
				this.SetCurrentShape(new VerticalLine());
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the horizontalLineRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void horizontalLineRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.horizontalLineRadioButton.Checked)
			{
				this.SetCurrentShape(new HorizontalLine());
			}
		}
		#endregion

		#endregion

		#region Private methods
		/// <summary>
		/// Replaces the current shape with another one and draws it.
		/// </summary>
		/// <param name="shape">The new shape.</param>
		private void SetCurrentShape(Shape shape)
		{
			this.currentShape = shape;
			this.shapeViewPanel.Invalidate();
		}

		/// <summary>
		/// Generates images of the shapes and save to the output folder.
		/// </summary>
		private void GenerateImages()
		{
			//
			// Create a bitmap which has the same size as the panel in which the shapes are drawn.
			//
			int width = this.shapeViewPanel.Size.Width;
			int height = this.shapeViewPanel.Size.Height;
			using (Bitmap bmp = new Bitmap(width, height))
			{
				//
				// Draw each shape
				//
				for (int shapeIndex = 0; shapeIndex < this.allShapes.Length; shapeIndex++)
				{
					//
					// Create a subfolder to contain images of this particular shape
					//
					var folderPath = Path.Combine(this.folderTextBox.Text, shapeIndex.ToString("00"));
					if (!Directory.Exists(folderPath))
					{
						Directory.CreateDirectory(folderPath);
					}
					//
					// Set this as the currently visible shape
					//
					this.SetCurrentShape(this.allShapes[shapeIndex]);
					//
					// Generate several 'runs' of this shape randomly moving from one point to another while also changing in size.
					//
					int index = 0;
					int runs = 10;
					for (int run = 0; run < runs; run++)
					{
						//
						// Generate a random animation sequence consisting of 30 frames
						//
						var animation = this.GetRandomAnimation(30);
						foreach (var frame in animation.GetFrames())
						{
							//
							// Store how the image should be translated in this frame, then redraw the shape
							//
							this.currentTranslation = frame.Item1;
							this.currentScale = frame.Item2;
							this.shapeViewPanel.Invalidate();
							Application.DoEvents();
							//
							// Copy the current frame to a bitmap
							//
							this.shapeViewPanel.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
							//
							// Save the bitmap to disk
							//
							var imagePath = Path.Combine(folderPath, index.ToString("0000") + ".png");
							bmp.Save(imagePath, ImageFormat.Png);
							//
							// Increment the filaname counter
							//
							index++;
						}			
					}
				}
			}
		}

		/// <summary>
		/// Generates a random, linear animation.
		/// </summary>
		/// <param name="steps">The number of steps in the animation.</param>
		/// <returns>The animation</returns>
		private LinearTransformSequence GetRandomAnimation(int steps)
		{
			//
			// Randomly pick the initial and final scaling factors
			//
			var scaleBegin = this.GetRandomFloat(0.6f, 1.25f);
			var scaleEnd = this.GetRandomFloat(0.6f, 1.25f);
			//
			// Estimate how much padding we have, depending on the scaling factors we have drawn
			//
			var paddingBegin = this.CalculateTranslationHeadroom(scaleBegin);
			var paddingEnd = this.CalculateTranslationHeadroom(scaleEnd);
			//
			// Pick random translations that are small enough to prevent the shape from being partially 'off screen'
			//
			var translateBegin = new PointF(
				this.GetRandomFloat(-paddingBegin, paddingBegin), 
				this.GetRandomFloat(-paddingBegin, paddingBegin));
			var translateEnd = new PointF(
				this.GetRandomFloat(-paddingEnd, paddingEnd),
				this.GetRandomFloat(-paddingEnd, paddingEnd));
			//
			// Return the animation sequence
			//
			return new LinearTransformSequence(translateBegin, translateEnd, scaleBegin, scaleEnd, steps);
		}

		/// <summary>
		/// Calculates the translation headroom, given the supplied scaling factor.
		/// </summary>
		/// <param name="scale">The scaling factor.</param>
		/// <returns>The amount of padding available (in pixels).</returns>
		private float CalculateTranslationHeadroom(float scale)
		{
			return (float)(this.shapeViewPanel.Size.Width * (0.2 + ((1 - scale) * 0.3) - 0.08));
		}

		/// <summary>
		/// Gets a random (single precision) floating point number.
		/// </summary>
		/// <param name="from">The lower bound of the interval.</param>
		/// <param name="to">The upper bound of the interval.</param>
		/// <returns>A random floating point number.</returns>
		private float GetRandomFloat(float from, float to)
		{
			//
			// Swap the upper and lower bounds if they are inversed
			//
			if (to < from)
			{
				float temp = to;
				to = from;
				from = temp;
			}
			//
			// Generate a random number.
			//
			return ((float)this.random.NextDouble() * (to - from)) + from;
		}
		#endregion
	}
}
