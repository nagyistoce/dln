// -----------------------------------------------------------------------
// <copyright file="ShowDataForm.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013-2014 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.UI.WinForms
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Forms;
	using Eft.BioNN.Engine.Data;
	using Eft.BioNN.Engine.Network;

	/// <summary>
	/// A form that can be used to visually inspect the training or testing data.
	/// </summary>
	public partial class ShowDataForm : Form
	{
		/// <summary>
		/// The data to display.
		/// </summary>
		private LabelledSample[] data = null;

		/// <summary>
		/// The width of the images in the training/testing data
		/// </summary>
		private int inputImageWidth;

		/// <summary>
		/// The height of the images in the training/testing data
		/// </summary>
		private int inputImageHeight;

		/// <summary>
		/// Initializes a new instance of the <see cref="ShowDataForm"/> class.
		/// </summary>
		public ShowDataForm()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ShowDataForm" /> class.
		/// </summary>
		/// <param name="data">The data to display.</param>
		/// <param name="imageWidth">The width of the images in the training/testing data.</param>
		/// <param name="imageHeight">The height of the images in the training/testing data.</param>
		public ShowDataForm(LabelledSample[] data, int imageWidth, int imageHeight)
			: this()
		{
			this.data = data;
			this.inputImageWidth = imageWidth;
			this.inputImageHeight = imageHeight;
		}

		/// <summary>
		/// Handles the Shown event of the ShowDataForm control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void ShowDataForm_Shown(object sender, EventArgs e)
		{
			//
			// Ensure that we have some data to display
			//
			if (this.data == null)
			{
				return;
			}
			//
			// Suspend layout of the flow panel while we generate the picture boxes. 
			//
			this.Cursor = Cursors.WaitCursor;
			this.flowLayoutPanel.SuspendLayout();
			//
			// Iterate over a 'reasonably sized' subset of the data
			//
			foreach (var sample in this.GetOrderedSamples(this.data, 5))
			{
				//
				// Generate a picture box for the image.
				//
				var pictureBox = new PictureBox();
				pictureBox.BorderStyle = BorderStyle.None;
				pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
				//
				// Generate the image and place it in the picture box.
				//
				Utils.DrawDigit(sample.SampleData, pictureBox, this.inputImageWidth, this.inputImageHeight);
				//
				// Add the picture box to the flow layout panel.
				//
				this.flowLayoutPanel.Controls.Add(pictureBox);
			}
			//
			// Resume laying out the controls in the flow panel.
			//
			this.flowLayoutPanel.ResumeLayout();
			this.Cursor = Cursors.Default;
		}

		/// <summary>
		/// Groups the training samples together so that each group contains a single sample from each class.
		/// </summary>
		/// <param name="samples">The samples.</param>
		/// <param name="howMany">How many groups to return in total.</param>
		/// <returns>Samples ordered so that the first sample is from the first class, the second sample from the second class and so on and so forth.</returns>
		private IEnumerable<LabelledSample> GetOrderedSamples(LabelledSample[] samples, int howMany)
		{
			var groups = samples.AsRandom().GroupBy(x => x.Label).OrderBy(g => g.Key).ToArray();
			for (int i = 0; i < howMany; i++)
			{
				foreach (var group in groups)
				{
					yield return group.ElementAt(i);
				}
			}
		}
	}
}
