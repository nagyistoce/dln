// -----------------------------------------------------------------------
// <copyright file="GenerateImagesForm.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.UI.WinForms
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Eft.BioNN.Engine.Network;

	/// <summary>
	/// A form used to display images created by the generative model.
	/// </summary>
	public partial class GenerateImagesForm : Form
	{
		/// <summary>
		/// The neural network used to generate the images (i.e. inputs).
		/// </summary>
		private INeuralNetwork network;

		/// <summary>
		/// The token source, used to cancel asynchronous tasks.
		/// </summary>
		private CancellationTokenSource tokenSource = new CancellationTokenSource();

		/// <summary>
		/// The width of the images in the training/testing data
		/// </summary>
		private int inputImageWidth;

		/// <summary>
		/// The height of the images in the training/testing data
		/// </summary>
		private int inputImageHeight;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateImagesForm"/> class.
		/// </summary>
		public GenerateImagesForm()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateImagesForm" /> class.
		/// </summary>
		/// <param name="network">The neural network to use for generating the images.</param>
		/// <param name="imageWidth">The width of the images in the training/testing data.</param>
		/// <param name="imageHeight">The height of the images in the training/testing data.</param>
		public GenerateImagesForm(INeuralNetwork network, int imageWidth, int imageHeight) : this()
		{
			this.network = network;
			this.layerUpDown.Maximum = this.network.LayerCount - 1;
			this.inputImageWidth = imageWidth;
			this.inputImageHeight = imageHeight;
		}

		/// <summary>
		/// Handles the Click event of the startButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void startButton_Click(object sender, EventArgs e)
		{
			//
			// Find out how big the input buffer should be (i.e. how many generated inputs to combine into a single image).
			//
			var selectedBufferSizeRadio = this.GetSelectedRadioButton(this.bufferSizeGroupBox);
			var bufferSize = Convert.ToInt32(selectedBufferSizeRadio.Text);
			//
			// Start the image generation task
			//
			Task.Factory.StartNew(() => this.GenerateImages(this.tokenSource.Token, (int)this.layerUpDown.Value - 1, bufferSize), this.tokenSource.Token);
			//
			// Enable/disable the appropriate UI controls
			//
			this.layerUpDown.Enabled = false;
			this.startButton.Enabled = false;
			this.stopButton.Enabled = true;
			this.samplesGroupBox.Enabled = false;
			this.bufferSizeGroupBox.Enabled = false;
		}

		/// <summary>
		/// Handles the Click event of the stopButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void stopButton_Click(object sender, EventArgs e)
		{
			//
			// Enable/disable the appropriate UI controls
			//
			this.layerUpDown.Enabled = true;
			this.startButton.Enabled = true;
			this.stopButton.Enabled = false;
			this.samplesGroupBox.Enabled = true;
			this.bufferSizeGroupBox.Enabled = true;
			//
			// Cancel the image generation task.
			//
			this.tokenSource.Cancel();
		}

		/// <summary>
		/// Generates images from a neural network.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <param name="layer">The layer at which to perform the random walk.</param>
		/// <param name="bufferSize">The number of inputs to combine into a single image.</param>
		private void GenerateImages(CancellationToken cancellationToken, int layer, int bufferSize)
		{
			//
			// Create the buffer
			//
			var buffer = new double[this.network.GetUnitCount(0)];
			var bufferIndex = 0;
			var inputVector = new byte[this.network.GetUnitCount(0)];
			//
			// Find out how many samples to take
			//
			var samplesRadioButton = this.GetSelectedRadioButton(this.samplesGroupBox);
			var numberOfSamples = Convert.ToInt32(samplesRadioButton.Text);
			//
			// Process each visible state (i.e. generated input) in the infinite series
			//
			foreach (var inputActivationProbabilities in this.network.GenerateVisibleStates(layer, numberOfSamples))
			{
				//
				// Store the visible state in the buffer
				//
				for (int unit = 0; unit < inputActivationProbabilities.Length; unit++)
				{
					buffer[unit] += inputActivationProbabilities[unit];
				}

				bufferIndex++;
				//
				// Is the buffer full?
				//
				if (bufferIndex >= bufferSize)
				{
					//
					// Combine the buffered inputs into a single byte array
					//
					for (int unit = 0; unit < inputActivationProbabilities.Length; unit++)
					{
						inputVector[unit] = (byte)Math.Min((int)Math.Round(byte.MaxValue * buffer[unit] / bufferSize), 255);
					}
					//
					// Convert the byte array into an image and display it. 
					// Also, find out for how long we should wait until generating the next image.
					//
					int sleepDuration = 0;
					this.Invoke((Action)(() =>
					{
						Utils.DrawDigit(inputVector, this.pictureBox, this.inputImageWidth, this.inputImageHeight);
						sleepDuration = this.GetSleepDuration();
					}));
					//
					// Wait a bit before generating the next frame
					//
					if (sleepDuration > 0)
					{
						Thread.Sleep(sleepDuration);
					}
					//
					// Reset buffer
					//
					Array.Clear(buffer, 0, buffer.Length);
					bufferIndex = 0;
				}
				//
				// Has this task been cancelled?
				//
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}
			}
			//
			// Create a new cancellation token (we can no longer use the old one)
			//
			this.tokenSource.Dispose();
			this.tokenSource = new CancellationTokenSource();
		}

		/// <summary>
		/// Gets the amount of time which should pass between generating frames.
		/// </summary>
		/// <returns>The sleep duration in milliseconds.</returns>
		private int GetSleepDuration()
		{
			var selectedBufferSizeRadio = this.GetSelectedRadioButton(this.speedGroupBox);
			return Convert.ToInt32(selectedBufferSizeRadio.Tag);
		}

		/// <summary>
		/// Gets the selected (i.e. checked) radio button in the specified <paramref name="groupBox"/>
		/// </summary>
		/// <param name="groupBox">The group box containing the radio buttons.</param>
		/// <returns>The selected radio button.</returns>
		private RadioButton GetSelectedRadioButton(GroupBox groupBox)
		{
			return groupBox.Controls.OfType<RadioButton>().Where(x => x.Checked).FirstOrDefault();
		}
	}
}
