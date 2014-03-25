// -----------------------------------------------------------------------
// <copyright file="ClassifierForm.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2014 All Rights Reserved
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
	/// This form can be used to inspect whether the network classifies a sample correctly.
	/// </summary>
	public partial class ClassifierForm : Form
	{
		/// <summary>
		/// The test set.
		/// </summary>
		private LabelledSample[] testSet = null;

		/// <summary>
		/// The width of the images in the training/testing data
		/// </summary>
		private int inputImageWidth;

		/// <summary>
		/// The height of the images in the training/testing data
		/// </summary>
		private int inputImageHeight;

		/// <summary>
		/// The current position in the test set.
		/// </summary>
		private int currentIndex = 0;

		/// <summary>
		/// The neural network.
		/// </summary>
		private INeuralNetwork network;

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassifierForm"/> class.
		/// </summary>
		public ClassifierForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassifierForm"/> class.
		/// </summary>
		/// <param name="testSet">The test set.</param>
		/// <param name="imageWidth">The width of the images in the training/testing data.</param>
		/// <param name="imageHeight">The height of the images in the training/testing data.</param>
		/// <param name="network">The neural network.</param>
		public ClassifierForm(LabelledSample[] testSet, int imageWidth, int imageHeight, INeuralNetwork network)
			: this()
		{
			this.testSet = testSet;
			this.inputImageWidth = imageWidth;
			this.inputImageHeight = imageHeight;
			this.network = network;
		}

		/// <summary>
		/// Handles the Shown event of the ClassifierForm control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void ClassifierForm_Shown(object sender, EventArgs e)
		{
			//
			// 
			//
			if (this.testSet == null || this.network == null)
			{
				return;
			}

			//this.pictureBox.Size = new Size(this.inputImageWidth, this.inputImageHeight);
			this.ShowAndClassifyCurrentImage();
		}

		/// <summary>
		/// Shows the current image and classifies it.
		/// </summary>
		private void ShowAndClassifyCurrentImage()
		{
			//
			// Show the current image
			//
			var currentSample = this.testSet[currentIndex];
			Utils.DrawDigit(currentSample.SampleData, this.pictureBox, this.inputImageWidth, this.inputImageHeight);	
			//
			// Do a series of classifications
			//
			int attempts = 20;
			var guessCounts = new int[10]; // HACK: this shouldn't be hardcoded
			int[] counts;
			int[] totalVotes = null;
			List<Votes> votes = new List<Votes>(attempts);
			for (var i = 0; i < attempts; i++)
			{
				guessCounts[this.network.Classify(currentSample, out counts)]++;
				votes.Add(new Votes(counts));
				if (totalVotes == null)
				{
					totalVotes = new int[counts.Length];
				}

				for (int index = 0; index < totalVotes.Length; index++)
				{
					totalVotes[index] += counts[index];
				}
			}

			votes.Add(new Votes(totalVotes));
			//
			// Look at the two most frequent classes
			//
			var topTwo = guessCounts.Select((value, index) => new { Value = value, Index = index })
				.ToList()
				.AsRandom()
				.OrderByDescending(a => a.Value)
				.Take(2)
				.ToArray();
			//
			// This is a confidence metric. I don't use it currently but it may make sense to keep an eye on it.
			//
			var margin = topTwo[0].Value - topTwo[1].Value;
			System.Diagnostics.Debug.WriteLine("Margin: " + margin);
			//
			// Return the class label that got the most votes
			//
			int guess = topTwo[0].Index;
			//
			// Show whether the classification was correct
			//
			this.correctLabel.Visible = (guess == currentSample.Label);
			this.wrongLabel.Visible = !this.correctLabel.Visible;
			this.guessLabel.Text = string.Format("Guessed {0}. Correct answer is {1}.", guess, currentSample.Label);
			//
			// Show a breakdown of the activations in the output layer
			//
			this.dataGridView.DataSource = votes;
		}

		/// <summary>
		/// Handles the Click event of the prevButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void prevButton_Click(object sender, EventArgs e)
		{
			if (this.currentIndex > 0)
			{
				this.currentIndex--;
				this.ShowAndClassifyCurrentImage();
			}

			this.prevButton.Enabled = (this.currentIndex > 0);
		}

		/// <summary>
		/// Handles the Click event of the nextButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void nextButton_Click(object sender, EventArgs e)
		{
			if (this.currentIndex < this.testSet.Length - 1)
			{
				this.currentIndex++;
				this.ShowAndClassifyCurrentImage();
			}

			this.prevButton.Enabled = (this.currentIndex < this.testSet.Length - 1);
		}

		/// <summary>
		/// This class is here only for the benefit of the DataGridView so that we can display how activations are spread 
		/// among ten label groups in the output layer.
		/// </summary>
		public class Votes
		{
			int[] counts;

			public int Votes0
			{
				get
				{
					return counts[0];
				}
				set
				{
					counts[0] = value;
				}
			}

			public int Votes1
			{
				get
				{
					return counts[1];
				}
				set
				{
					counts[1] = value;
				}
			}

			public int Votes2
			{
				get
				{
					return counts[2];
				}
				set
				{
					counts[2] = value;
				}
			}

			public int Votes3
			{
				get
				{
					return counts[3];
				}
				set
				{
					counts[3] = value;
				}
			}

			public int Votes4
			{
				get
				{
					return counts[4];
				}
				set
				{
					counts[4] = value;
				}
			}

			public int Votes5
			{
				get
				{
					return counts[5];
				}
				set
				{
					counts[5] = value;
				}
			}

			public int Votes6
			{
				get
				{
					return counts[6];
				}
				set
				{
					counts[6] = value;
				}
			}

			public int Votes7
			{
				get
				{
					return counts[7];
				}
				set
				{
					counts[7] = value;
				}
			}

			public int Votes8
			{
				get
				{
					return counts[8];
				}
				set
				{
					counts[8] = value;
				}
			}

			public int Votes9
			{
				get
				{
					return counts[9];
				}
				set
				{
					counts[9] = value;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="Votes"/> class.
			/// </summary>
			/// <param name="counts">The number of activations in each label group.</param>
			public Votes(int[] counts)
			{
				if (counts == null || counts.Length > 10)
				{
					throw new ArgumentException("Invalid number of classes", "counts");
				}

				this.counts = counts;
			}
		}
	}
}
