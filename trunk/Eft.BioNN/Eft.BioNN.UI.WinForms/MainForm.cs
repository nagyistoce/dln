// -----------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.UI.WinForms
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using Accord.Math;
	using Accord.Statistics;
	using Accord.Statistics.Analysis;
	using Eft.BioNN.Engine.Data;
	using Eft.BioNN.Engine.Filters;
	using Eft.BioNN.Engine.Learning;
	using Eft.BioNN.Engine.Network;

	/// <summary>
	/// The main window of the application
	/// </summary>
	public partial class MainForm : Form
	{
		#region Private members

		/// <summary>
		/// The number of epochs in the grace period 
		/// (training will stop if the classification error doesn't improve in the grace period)
		/// </summary>
		private const int GracePeriod = 7; // TODO: Make configurable in the UI

		/// <summary>
		/// Mini-batches. This name is actually misleading, as learning is on-line. 
		/// These batches can be useful if one wants to gauge the test error at more frequent intervals 
		/// than after a single training epoch.
		/// </summary>
		private LabelledSample[][] miniBatches;
	
		/// <summary>
		/// The training data provider
		/// </summary>
		//private IDataProvider dataProvider = new MovingShapesProvider();
		private IDataProvider dataProvider = new MnistProvider();
		//private IDataProvider dataProvider = new StationaryShapesProvider();
		//private IDataProvider dataProvider = new XorDataProvider();

		/// <summary>
		/// The width of the images in the training/testing data
		/// </summary>
		private int inputImageWidth;
		
		/// <summary>
		/// The height of the images in the training/testing data
		/// </summary>
		private int inputImageHeight;

		/// <summary>
		/// A collection of filters to apply to the input data before training the neural network.
		/// </summary>
		private IFilter[] filters = new IFilter[0];// { new BlackAndWhiteFilter() };

		/// <summary>
		/// Specifies how supervised learning should be conducted.
		/// </summary>
		private ContrastiveDivergenceLearningMode supervisedLearningMode = ContrastiveDivergenceLearningMode.JointAlternative;

		/// <summary>
		/// The training set
		/// </summary>
		private LabelledSample[] trainingSet;

		/// <summary>
		/// The testing set
		/// </summary>
		private LabelledSample[] testingSet;

		/// <summary>
		/// The token source, used to cancel asynchronous tasks.
		/// </summary>
		private CancellationTokenSource tokenSource = new CancellationTokenSource();

		/// <summary>
		/// The current neural network.
		/// </summary>
		private INeuralNetwork currentNetwork;
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
		/// Handles the Load event of the MainForm control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void MainForm_Load(object sender, EventArgs e)
		{
			//
			// Hook up the enabled state of the items in the main menu with the corresonding buttons. 
			// That way, if a button becomes enabled/disabled, then the menu item for the same action also becomes enabled/disabled.
			//
			this.PairEnabledState(this.createNetworkButton, this.newToolStripMenuItem);
			this.PairEnabledState(this.loadNetworkButton, this.openToolStripMenuItem);
			this.PairEnabledState(this.saveNetworkButton, this.saveToolStripMenuItem);
			this.PairEnabledState(this.unsupervisedTrainingButton, this.startUnsupervisedToolStripMenuItem);
			this.PairEnabledState(this.supervisedTrainingButton, this.startSupervisedToolStripMenuItem);
			this.PairEnabledState(this.testButton, this.testToolStripMenuItem);
			this.PairEnabledState(this.stopButton, this.stopToolStripMenuItem);
			//
			// Load the training and testing data (asynchronously)
			//
			this.AppendInfoLine("Loading training data");
			var t = Task.Factory.StartNew(() => this.LoadTrainingData());
			t.Wait();
			this.AppendInfoLine("Training data loaded");
			
			this.trainingProgressBar.Maximum = this.trainingSet.Length;
			//
			// Adjust the column chart that shows the classification metrics
			//
			this.accuracyColumnChart.ChartAreas[0].AxisX.Maximum = this.dataProvider.NumberOfClasses - 0.5;
			this.ResetAccuracyChart();
		}

		/// <summary>
		/// Handles the Click event of the stopButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void stopButton_Click(object sender, EventArgs e)
		{
			this.tokenSource.Cancel();
		}

		/// <summary>
		/// Handles the Click event of the createNetworkButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void createNetworkButton_Click(object sender, EventArgs e)
		{
			this.CreateNetwork();
		}

		/// <summary>
		/// Handles the Click event of the unsupervisedTrainingButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void unsupervisedTrainingButton_Click(object sender, EventArgs e)
		{
			this.StartTraining(false);
		}

		/// <summary>
		/// Handles the Click event of the supervisedTrainingButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void supervisedTrainingButton_Click(object sender, EventArgs e)
		{
			this.StartTraining(true);
		}

		/// <summary>
		/// Handles the Click event of the testButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void testButton_Click(object sender, EventArgs e)
		{
			//
			// Temporary hack, measuring the ability of label units to reconstruct themselves
			//
			//var abstractNetwork = this.currentNetwork as AbstractNeuralNetwork<InterLayerAdjacencyMatrix>;
			//var labelReconstructionErrorMatrix = new double[abstractNetwork.GetUnitCount(abstractNetwork.LayerCount - 1) / this.dataProvider.NumberOfClasses, this.dataProvider.NumberOfClasses];
			//for (int seedSize = 1; seedSize <= abstractNetwork.GetUnitCount(abstractNetwork.LayerCount - 1) / this.dataProvider.NumberOfClasses; seedSize++)
			//{
			//    for (int classIndex = 0; classIndex < this.dataProvider.NumberOfClasses; classIndex++)
			//    {
			//        //this.AppendInfoLine(string.Format("Label reconstruction error for {0} is {1} (seed size: {2})", classIndex, abstractNetwork.CheckLabelReconstruction(classIndex, seedSize), seedSize));
			//        labelReconstructionErrorMatrix[seedSize - 1, classIndex] = abstractNetwork.CheckLabelReconstruction(classIndex, seedSize);
			//    }
			//}
			//this.AppendInfoLine("--Label reconstruction error begins--");
			//this.AppendInfoLine(labelReconstructionErrorMatrix.ToString(DefaultMatrixFormatProvider.CurrentCulture));
			//this.AppendInfoLine("--Label reconstruction error ends--");
			//
			// Another temp hack, look at the average weight in the output layer
			//
			//var allOutputWeights = this.currentNetwork.GetAllWeights(this.currentNetwork.LayerCount - 2);
			//this.AppendInfoLine(string.Format("Output weight average: {0} - StdDev: {1}", allOutputWeights.Average(), allOutputWeights.StandardDeviation()));
			//
			// Temporary hack, measuring the pairwise distances between label units, see how the learning probability affects this metric.
			//
			/*
			var distancesMatrices = this.currentNetwork.GetPairwiseWeightDistancesBetweenLabelUnits();
			for (int classIndex = 0; classIndex < distancesMatrices.Length; classIndex++)
			{
				this.AppendInfoLine("Distance matrix for class " + classIndex);
				this.AppendInfoLine(distancesMatrices[classIndex].ToString(DefaultMatrixFormatProvider.CurrentCulture));
			}
			*/
			//
			// Test the network asynchronously
			//
			Task.Factory.StartNew(() =>
			{
				this.testButton.BeginInvoke((Action)(() => this.testButton.Enabled = false));
				this.ShowError(this.currentNetwork, null);
				this.testButton.BeginInvoke((Action)(() => this.testButton.Enabled = true));
			});
		}

		/// <summary>
		/// Handles the Click event of the saveNetworkButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void saveNetworkButton_Click(object sender, EventArgs e)
		{
			this.SaveNetwork();
		}

		/// <summary>
		/// Handles the Click event of the loadNetworkButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void loadNetworkButton_Click(object sender, EventArgs e)
		{
			this.LoadNetwork();
		}

		/// <summary>
		/// Handles the Click event of the dreamButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void dreamButton_Click(object sender, EventArgs e)
		{
			using (var form = new GenerateImagesForm(this.currentNetwork, this.inputImageWidth, this.inputImageHeight))
			{
				form.ShowDialog();
			}
		}

		/// <summary>
		/// Handles the Click event of the newToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.CreateNetwork();
		}

		/// <summary>
		/// Handles the Click event of the openToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.LoadNetwork();
		}

		/// <summary>
		/// Handles the Click event of the saveToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SaveNetwork();
		}

		/// <summary>
		/// Handles the Click event of the exitToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		/// <summary>
		/// Handles the Click event of the experimentBatchToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void experimentBatchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var form = new BatchForm())
			{
				if (form.ShowDialog() == DialogResult.OK)
				{
					this.RunExperimentBatch(form.Experiments, form.OutputDirectory);
				}
			}
		}

		/// <summary>
		/// Handles the Click event of the startUnsupervisedToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void startUnsupervisedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.unsupervisedTrainingButton_Click(sender, e);
		}

		/// <summary>
		/// Handles the Click event of the startSupervisedToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void startSupervisedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.supervisedTrainingButton_Click(sender, e);
		}

		/// <summary>
		/// Handles the Click event of the stopToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void stopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.stopButton_Click(sender, e);
		}

		/// <summary>
		/// Handles the Click event of the testToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void testToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.testButton_Click(sender, e);
		}

		/// <summary>
		/// Handles the Click event of the trainingDataToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void trainingDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ShowData(this.trainingSet);
		}

		/// <summary>
		/// Handles the Click event of the testingDataToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void testingDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ShowData(this.testingSet);
		}

		/// <summary>
		/// Handles the Click event of the visualTesterToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void visualTesterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var form = new ClassifierForm(this.testingSet, this.inputImageWidth, this.inputImageHeight, this.currentNetwork))
			{
				form.ShowDialog();
			}
		}
		#endregion

		#region Visualization of the network
		/// <summary>
		/// Initializes the visualization.
		/// </summary>
		/// <param name="network">The network.</param>
		private void InitializeVisualization(INeuralNetwork network)
		{
			//
			// Get the number of units in all layers that come after the input layer
			//
			int[] unitCounts = new int[network.LayerCount - 1];
			for (int index = 0; index < unitCounts.Length; index++)
			{
				unitCounts[index] = network.GetUnitCount(index + 1);
			}
			//
			// Schedule the remainder of the work to be done on the UI thread.
			//
			this.containerLayoutPanel.BeginInvoke((Action)(() => this.PrepareVisualizationContainer(unitCounts)));
		}

		/// <summary>
		/// Creates picture boxes for all of the neurons, grouped by layer.
		/// </summary>
		/// <param name="unitCounts">The number of units in each layer.</param>
		private void PrepareVisualizationContainer(int[] unitCounts)
		{
			//
			// Start by clearing the container of any existing controls (pictureBoxes)
			//
			if (this.containerLayoutPanel.Controls.Count > 0)
			{
				//
				// TODO: Dispose first?
				//
				this.containerLayoutPanel.Controls.Clear();
			}

			this.containerLayoutPanel.SuspendLayout();
			//
			// Iterate through the layers
			//
			for (int layer = 0; layer < unitCounts.Length; layer++)
			{
				//
				// Create a label for this layer
				//
				var label = new Label();
				label.Text = "Layer " + (layer + 1).ToString();
				label.Dock = DockStyle.Top;
				this.containerLayoutPanel.Controls.Add(label);
				//
				// Add a picture box for each unit (neuron) in this layer
				//
				for (int unit = 0; unit < unitCounts[layer]; unit++)
				{
					var pictureBox = new PictureBox();
					pictureBox.Tag = new int[] { layer, unit };
					pictureBox.BorderStyle = BorderStyle.FixedSingle;
					pictureBox.Size = new Size(28, 28); // TODO: Should I not make this hardcoded?
					pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
					this.containerLayoutPanel.Controls.Add(pictureBox);
				}
			}

			this.containerLayoutPanel.ResumeLayout();
		}

		/// <summary>
		/// Creates and shows a visualization of the neural network.
		/// </summary>
		/// <param name="network">The neural network.</param>
		private void VisualizeNetwork(INeuralNetwork network)
		{
			//
			// Create an array to store the reconstructions, indexed by layer, then unit.
			// The reconstructions themselves are integer arrays, hence the three dimensional array.
			//
			int[][][] reconstructions = new int[network.LayerCount - 1][][];
			//
			// For each layer, we will track the minimum, mean and maximum value encountered in the reconstruction.
			//
			int[] min = new int[network.LayerCount - 1];
			int[] max = new int[network.LayerCount - 1];
			double[] mean = new double[network.LayerCount - 1];
			//
			// TODO: Group the output neurons together by class?
			// Iterate over the layers
			//
			for (int layer = 0; layer < reconstructions.Length; layer++)
			{
				//
				// Initialize variables
				//
				max[layer] = int.MinValue;
				min[layer] = int.MaxValue;
				long sum = 0;
				reconstructions[layer] = new int[network.GetUnitCount(layer + 1)][];
				int[] mask = new int[network.GetUnitCount(layer + 1)];
				//
				// Iterate over the units in this layer
				//
				for (int unit = 0; unit < reconstructions[layer].Length; unit++)
				{
					//
					// Generate a reconstruction for this unit
					//
					mask[unit] = 1;
					reconstructions[layer][unit] = network.Reconstruct(layer + 1, mask);
					mask[unit] = 0;
					//
					// Update the statistics
					//
					min[layer] = Math.Min(min[layer], reconstructions[layer][unit].Min());
					max[layer] = Math.Max(min[layer], reconstructions[layer][unit].Max());
					sum += reconstructions[layer][unit].Sum();
				}
				//
				// Calculate the mean
				//
				mean[layer] = (double)sum / (reconstructions[layer].Length * reconstructions[layer][0].Length);
			}
			//
			// Schedule the drawing of these reconstructions to be performed on the main UI thread
			// 
			this.containerLayoutPanel.Invoke((Action)(() => this.ShowReconstructions(reconstructions, min, max, mean)));
		}

		/// <summary>
		/// Shows the reconstructions of a neural network.
		/// </summary>
		/// <param name="reconstructions">The reconstruction data.</param>
		/// <param name="min">The minimum value in each layer.</param>
		/// <param name="max">The maximum value in each layer.</param>
		/// <param name="mean">The mean value in each layer.</param>
		private void ShowReconstructions(int[][][] reconstructions, int[] min, int[] max, double[] mean)
		{
			//
			// We want to normalize the data so that the a grey pixel represents the mean value in the reconstruction data
			// and that either of the extreme values is totally black or white (depending on which is further away from the mean).
			//
			var scaleFrom = new double[min.Length];
			var scaleTo = new double[min.Length];
			for (int layer = 0; layer < min.Length; layer++)
			{
				var halfSpan = Math.Max(max[layer] - mean[layer], mean[layer] - min[layer]);
				scaleFrom[layer] = mean[layer] - halfSpan;
				scaleTo[layer] = mean[layer] + halfSpan;
			}

			this.containerLayoutPanel.SuspendLayout();
			//
			// Iterate over the controls in the visualization container
			//
			foreach (var control in this.containerLayoutPanel.Controls)
			{
				//
				// Ensure that this control is a picture box before proceeding
				//
				var pictureBox = control as PictureBox;
				if (pictureBox != null)
				{
					//
					// Figure out which unit (neuron) this picturebox represents.
					//
					var parts = (int[])pictureBox.Tag;
					int layer = parts[0];
					int unit = parts[1];
					//
					// Normalize the reconstruction data
					//
					byte[] normalizedData = new byte[reconstructions[layer][unit].Length];
					for (int index = 0; index < normalizedData.Length; index++)
					{
						normalizedData[index] = (byte)Math.Round(Accord.Math.Tools.Scale(scaleFrom[layer], scaleTo[layer], byte.MinValue, byte.MaxValue, reconstructions[layer][unit][index]));
					}
					//
					// Draw what this unit likes to see
					//
					Utils.DrawDigit(normalizedData, pictureBox, this.inputImageWidth, this.inputImageHeight);
				}
			}

			this.containerLayoutPanel.ResumeLayout();
		}
		#endregion

		#region Textual info
		/// <summary>
		/// Appends a line to the information textbox.
		/// </summary>
		/// <param name="text">The text to place in the line.</param>
		/// <remarks>It is safe to call this method from non-UI threads.</remarks>
		private void AppendInfoLine(string text)
		{
			//
			// Ensure we don't interact with the textbox unless we're running on the main UI thread.
			//
			if (this.infoTextBox.InvokeRequired)
			{
				this.infoTextBox.BeginInvoke((Action)(() => this.AppendInfoLine(text)));
			}
			else
			{
				this.infoTextBox.AppendText(text + Environment.NewLine);
			}
		}

		/// <summary>
		/// Shows the number of excitatory edges going into the groups of units representing each of the class labels.
		/// </summary>
		/// <param name="network">The neural network.</param>
		private void ShowLabelDegreeDistribution(INeuralNetwork network)
		{
			//
			// TODO: Remove? I rarely look at this statistic any more
			//
			//int[] degrees = network.GetAggregateLabelDegrees();
			//this.AppendInfoLine("Label degrees: [" + string.Join("; ", degrees) + "]");
		}

		/// <summary>
		/// Shows the number of edges, grouped by type, in each connection layer.
		/// </summary>
		/// <param name="prefix">A text prefix.</param>
		/// <param name="network">The neural network.</param>
		private void ShowEdgeCount(string prefix, INeuralNetwork network)
		{
			//
			// TODO: Show edge distribution by weight
			//
			//string edgeCountMessage = prefix + " (excitatory/neutral/inhibitory):";
			//for (int layer = 0; layer < network.LayerCount - 1; layer++)
			//{
			//    edgeCountMessage += string.Format(
			//        " {0}/{1}/{2} ;", 
			//        network.GetEdgeCount(layer, EdgeType.ExcitatoryEdge), 
			//        network.GetEdgeCount(layer, EdgeType.NeutralEdge),
			//        network.GetEdgeCount(layer, EdgeType.InhibitoryEdge));
			//}

			//this.AppendInfoLine(edgeCountMessage);
		}

		/// <summary>
		/// Displays textual information about the current neural network.
		/// </summary>
		/// <param name="neuralNetwork">The neural network.</param>
		private void ShowNetworkInfo(INeuralNetwork neuralNetwork)
		{
			this.networkInfoTextBox.Clear();
			this.networkInfoTextBox.AppendText(string.Format("Type: {0}\n", neuralNetwork.GetType().Name));
			this.networkInfoTextBox.AppendText(string.Format("Input units: {0}\n", neuralNetwork.GetUnitCount(0)));
			for (int layerIndex = 1; layerIndex < neuralNetwork.LayerCount - 1; layerIndex++)
			{
				this.networkInfoTextBox.AppendText(string.Format("Hidden units: {0}\n", neuralNetwork.GetUnitCount(layerIndex)));
			}

			this.networkInfoTextBox.AppendText(string.Format("Output units: {0}\n", neuralNetwork.GetUnitCount(neuralNetwork.LayerCount - 1)));
			this.networkInfoTextBox.AppendText(string.Format("Activation threshold: {0}\n", neuralNetwork.ActivationThreshold));
			this.networkInfoTextBox.AppendText(string.Format("Synapse reliability: {0}\n", neuralNetwork.SynapseSuccessProbability));
			
			var weights = Enumerable.Range(neuralNetwork.MinWeight, neuralNetwork.MaxWeight - neuralNetwork.MinWeight + 1);
			this.networkInfoTextBox.AppendText(string.Format("Weights: {0}",  string.Join(";", weights.ToArray())));
		}

		/// <summary>
		/// Tests the network using the testing set and reports the classification error.
		/// </summary>
		/// <param name="network">The neural network.</param>
		private void ShowError(INeuralNetwork network)
		{
			//
			// Delegate to overload
			//
			this.ShowError(network, null);
		}

		/// <summary>
		/// Tests the network using the testing set and reports the classification error.
		/// </summary>
		/// <param name="network">The neural network.</param>
		/// <param name="errorHistory">A list containing the error history, to which the error will be appended.</param>
		private void ShowError(INeuralNetwork network, List<double> errorHistory)
		{
			//
			// TODO: This metdhod is almost pointless. Either remove it or make it responsible for analyzing the classification results.
			//
			var error = this.TestNetwork(network);
			if (errorHistory != null)
			{
				errorHistory.Add(error);
			}

			this.AppendInfoLine(string.Format("Classification error: {0:p}", error));
		}	
		#endregion

		#region Create, save, load
		/// <summary>
		/// Displays the dialog for creating a new neural network.
		/// </summary>
		private void CreateNetwork()
		{
			using (var newNetworkForm = new CreateNetworkForm(this.trainingSet[0].SampleData.Length, this.dataProvider.NumberOfClasses))
			{
				if (newNetworkForm.ShowDialog(this) == DialogResult.OK)
				{
					this.currentNetwork = newNetworkForm.NeuralNetwork;
					this.NetworkAvailable();
				}
			}
		}

		/// <summary>
		/// Displays a dialog for saving the neural network.
		/// </summary>
		private void SaveNetwork()
		{
			var result = this.saveFileDialog.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				using (var stream = this.saveFileDialog.OpenFile())
				{
					this.currentNetwork.Save(stream);
				}
			}
		}

		/// <summary>
		/// Displays a dialog for loading a neural network.
		/// </summary>
		private void LoadNetwork()
		{
			var result = this.openFileDialog.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				using (var stream = this.openFileDialog.OpenFile())
				{
					this.currentNetwork = ContrastiveDivergenceSamplingNetwork<InterLayerAdjacencyMatrix>.Load(stream);
					this.NetworkAvailable();
				}
			}
		}

		/// <summary>
		/// Updates the UI to reflect that a neural network is available for training and testing.
		/// </summary>
		private void NetworkAvailable()
		{
			if (this.currentNetwork == null)
			{
				return;
			}

			this.ShowNetworkInfo(this.currentNetwork);
			this.InitializeVisualization(this.currentNetwork);
			this.VisualizeNetwork(this.currentNetwork);
			this.saveNetworkButton.Enabled = true;
			this.unsupervisedTrainingButton.Enabled = true;
			this.supervisedTrainingButton.Enabled = true;
			this.testButton.Enabled = true;
			this.visualTesterToolStripMenuItem.Enabled = true;

			this.ResetAccuracyChart();
		}
		#endregion

		#region Training and testing
		/// <summary>
		/// Loads the training and testing data into memory.
		/// </summary>
		private void LoadTrainingData()
		{
			this.trainingSet = this.dataProvider.GetTrainingSet();
			this.testingSet = this.dataProvider.GetTestingSet();

			if (this.trainingSet.Length == 0 || this.testingSet.Length == 0)
			{
				throw new Exception("The training and testing data sets must be non-empty");
			}
			//
			// Assume the image is square. Figure out its height and width.
			//
			this.inputImageHeight = this.inputImageWidth = (int)Math.Sqrt(this.trainingSet[0].SampleData.Length);
			//
			// Filter the training and testing data
			//
			int dataBlowUpFactor = 1;
			foreach (var filter in this.filters)
			{
				this.AppendInfoLine("Applying " + filter.GetType().Name + " to training and test data");
				filter.Process(this.trainingSet);
				filter.Process(this.testingSet);
				dataBlowUpFactor *= filter.BlowupFactor;
			}
			//
			// If the filters increased the size of the input image, increase the height accordingly
			//
			this.inputImageHeight = this.inputImageWidth * dataBlowUpFactor;
			//
			// This temporary addition is for the "learning speed" experiment
			//
			//this.miniBatches = this.MakeBatches(this.trainingSet, 10);
		}

		/// <summary>
		/// Partitions a data set into smaller batches.
		/// </summary>
		/// <param name="dataSet">The data set.</param>
		/// <param name="samplesPerLabelPerBatch">The number of samples from each class that should be in a batch.</param>
		/// <returns>A two-dimensional array, indexed by batch and then sample.</returns>
		private LabelledSample[][] MakeBatches(LabelledSample[] dataSet, int samplesPerLabelPerBatch)
		{
			//
			// Start by grouping the samples by their label
			//
			var groups = dataSet.GroupBy(x => x.Label).OrderBy(g => g.Key);
			int numberOfLabels = groups.Count();
			LabelledSample[][] samplesByLabel = new LabelledSample[numberOfLabels][];
			int index = 0;
			foreach (var group in groups)
			{
				samplesByLabel[index] = group.ToArray();
				index++;
			}
			//
			// Create the batches
			//
			int minGroupSize = samplesByLabel.Min(x => x.Length);
			int numberOfBatches = minGroupSize / samplesPerLabelPerBatch;
			LabelledSample[][] result = new LabelledSample[numberOfBatches][];
			for (int batch = 0; batch < numberOfBatches; batch++)
			{
				result[batch] = new LabelledSample[numberOfLabels * samplesPerLabelPerBatch];
				for (int sample = 0; sample < samplesPerLabelPerBatch; sample++)
				{
					for (int label = 0; label < samplesByLabel.Length; label++)
					{
						//System.Diagnostics.Debug.WriteLine("Batch: {0}.{1} <- Label-sample: {2}.{3}", batch, sample * numberOfLabels + label, label, batch * samplesPerLabelPerBatch + sample);
						result[batch][(sample * numberOfLabels) + label] = samplesByLabel[label][(batch * samplesPerLabelPerBatch) + sample];
					}
				}
			}
			//
			// Return the batches
			//
			return result;
		}

		/// <summary>
		/// Tests the network using the testing set.
		/// </summary>
		/// <param name="network">The neural network.</param>
		/// <returns>The classification error.</returns>
		private double TestNetwork(INeuralNetwork network)
		{
			//
			// Reset the progress bar
			//
			this.trainingProgressBar.BeginInvoke((Action)(() => 
			{
				this.trainingProgressBar.Maximum = this.testingSet.Length;
				this.trainingProgressBar.Value = 0;
			}));
			//
			// Create a (confusion) matrix to store the class predictions
			//
			var classPredictions = new int[this.dataProvider.NumberOfClasses, this.dataProvider.NumberOfClasses];
			int sampleIndex = 0;
			var activationStatistics = new List<double>(this.testingSet.Length);
			int alternativeClassificationErrors = 0;
			
			foreach (var sample in this.testingSet)
			//foreach (var sample in this.testingSet.AsRandom().Take(1000))
			{
				//
				// Classify this sample
				//
				double activatedOutputFraction;
				int alternativeGuess;
				int guess = this.ClassifyByProbabilitySampling(network, sample, out activatedOutputFraction, out alternativeGuess);
				classPredictions[sample.Label, guess]++;
				activationStatistics.Add(activatedOutputFraction);

				if (alternativeGuess != sample.Label)
				{
					alternativeClassificationErrors++;
				}
#if DEBUG
				//
				// Is the classification correct?
				//
				if (guess != sample.Label)
				{
					////Debug.WriteLine("Misclassified {0} as {1}. Margin was {2}.", sample.Label, guess, margin);
					System.Diagnostics.Debug.WriteLine("Misclassified {0} as {1}", sample.Label, guess);
				}
				else
				{
					////Debug.WriteLine("Correctly classified {0}. Margin was {1}.", sample.Label, margin);
					System.Diagnostics.Debug.WriteLine("Correctly classified {0}", sample.Label);
				}
#endif
				//
				// Update the progress bar
				//
				sampleIndex++;
				this.trainingProgressBar.BeginInvoke((Action)(() => this.trainingProgressBar.Value = sampleIndex));
			}
			//
			// Calculate the column and row totals for the confusion matrix
			//
			var confusionMatrix = new GeneralConfusionMatrix(classPredictions);
			var rowTotals = confusionMatrix.RowTotals;
			var columnTotals = confusionMatrix.ColumnTotals;
			//
			// Display the distribution of the predictions by class
			//
			var normalizedHistogram = from count in columnTotals
									  select ((double)count / this.testingSet.Length).ToString("p");
			this.AppendInfoLine("Guesses: [" + string.Join("; ", normalizedHistogram.ToArray()) + "]");
			//
			// Calculate the precision, recall and specificity for each class.
			// See http://en.wikipedia.org/wiki/Precision_and_recall#Definition_.28classification_context.29
			//
			var chartData = new double[this.dataProvider.NumberOfClasses][];
			for (int classIndex = 0; classIndex < this.dataProvider.NumberOfClasses; classIndex++)
			{
				int truePositives = classPredictions[classIndex, classIndex];
				int trueNegatives = this.testingSet.Length - rowTotals[classIndex] - columnTotals[classIndex] + truePositives;
				int falseNegatives = rowTotals[classIndex] - truePositives;
				int falsePositives = columnTotals[classIndex] - truePositives;

				double precision = (double)truePositives / (truePositives + falsePositives);
				double recall = (double)truePositives / (truePositives + falseNegatives);
				double specificity = (double)trueNegatives / (trueNegatives + falsePositives);

				chartData[classIndex] = new double[] { precision, recall, specificity };

				this.AppendInfoLine(string.Format(
					"Class: {0}. Precision: {1:p}. Recall: {2:p}. Specificity: {3:p}.",
					classIndex,
					precision,
					recall,
					specificity));
			}
			//
			// Show the classification metrics in a chart
			//
			this.BeginInvoke((Action)(() => this.ShowClassificationMetrics(chartData)));

			this.AppendInfoLine(string.Format("Activated output fraction - Avg: {0:P4}. StdDev: {1:P4}.", activationStatistics.Average(), activationStatistics.ToArray().StandardDeviation()));
			this.AppendInfoLine(string.Format("Alternative error rate: {0:P2}", (double)alternativeClassificationErrors / sampleIndex));
			//
			// Return the error rate
			//
			return ((double)sampleIndex - confusionMatrix.Diagonal.Sum()) / sampleIndex;
		}

		/// <summary>
		/// Shows the classification metrics.
		/// </summary>
		/// <param name="chartData">The chart data, indexed by class and series respectively.</param>
		private void ShowClassificationMetrics(double[][] chartData)
		{
			//
			// Update the data points
			//
			for (int classIndex = 0; classIndex < chartData.Length; classIndex++)
			{
				for (int seriesIndex = 0; seriesIndex < chartData[classIndex].Length; seriesIndex++)
				{
					this.accuracyColumnChart.Series[seriesIndex].Points[classIndex].SetValueY(chartData[classIndex][seriesIndex]);
				}
			}
			//
			// Redraw the chart 
			//
			this.accuracyColumnChart.Invalidate();
		}

		/// <summary>
		/// Performs several classifications and returns the most frequent result.
		/// </summary>
		/// <param name="network">The network.</param>
		/// <param name="sample">The sample to classify.</param>
		/// <param name="activatedFraction">The average fraction of the output layer that gets activated.</param>
		/// <param name="alternativePrediction">A prediction based on total activations, accumulated over all the classification attempts.</param>
		/// <returns>
		/// The most frequent class
		/// </returns>
		private int ClassifyByProbabilitySampling(INeuralNetwork network, Sample sample, out double activatedFraction, out int alternativePrediction)
		{
			activatedFraction = 0.0;
			int attempts = 20;
			//
			// If the network is deterministic, we only need a single attempt
			//
			//if (network.MaxInputActivations >= network.GetUnitCount(0) && network.SynapseSuccessProbability >= 1.0)
			//{
			//    attempts = 1;
			//}
			//
			// Do a series of classifications
			//
			var counts = new int[this.dataProvider.NumberOfClasses];
			int[] votes;
			int[] totalVotes = new int[this.dataProvider.NumberOfClasses];
			double attemptsTimesOutputLayerSize = Convert.ToDouble(attempts * network.GetUnitCount(network.LayerCount - 1));
			for (var i = 0; i < attempts; i++)
			{
				counts[network.Classify(sample, out votes)]++;
				activatedFraction += votes.Sum() / attemptsTimesOutputLayerSize;
				for (int classIndex = 0; classIndex < totalVotes.Length; classIndex++)
				{
					totalVotes[classIndex] += votes[classIndex];
				}
			}
			//
			// Look at the two most frequent classes
			//
			var topTwo = counts.Select((value, index) => new { Value = value, Index = index })
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
			// Perform the alternative classification, based on the total number of activations we have seen in each label group
			//
			var topVote = totalVotes.Select((value, index) => new { Value = value, Index = index })
				.ToList()
				.AsRandom()
				.OrderByDescending(a => a.Value)
				.ToArray();
			alternativePrediction = topVote[0].Index;
			//
			// Return the class label that got selected most often
			//
			return topTwo[0].Index;
		}

		/// <summary>
		/// Starts a (background) task which trains the network.
		/// </summary>
		/// <param name="supervised">if set to <c>true</c> then supervised learning is performed.</param>
		private void StartTraining(bool supervised)
		{
			//
			// Start the training task
			//
			Task.Factory.StartNew(() => this.Train(this.tokenSource.Token, supervised, this.currentNetwork), this.tokenSource.Token);
			//
			// Enable/disable the appropriate UI controls
			//
			this.stopButton.Enabled = true;
			this.unsupervisedTrainingButton.Enabled = false;
			this.supervisedTrainingButton.Enabled = false;
			this.networkGroupBox.Enabled = false;
			this.testButton.Enabled = false;
		}

		/// <summary>
		/// Adjusts the UI after training has stopped.
		/// </summary>
		private void TrainingComplete()
		{
			//
			// Ensure this method is run on the UI thread.
			//
			if (this.InvokeRequired)
			{
				this.BeginInvoke((Action)(() => this.TrainingComplete()));
				return;
			}
			//
			// Enable/disable the appropriate UI controls
			//
			this.stopButton.Enabled = false;
			this.unsupervisedTrainingButton.Enabled = true;
			this.supervisedTrainingButton.Enabled = true;
			this.networkGroupBox.Enabled = true;
			this.testButton.Enabled = true;
			//
			// Create a new cancellation token (we can no longer use the old one)
			//
			this.tokenSource.Dispose();
			this.tokenSource = new CancellationTokenSource();
			//
			// Reset the progress bar
			//
			this.trainingProgressBar.Value = 0;
		}

		/// <summary>
		/// Trains a neural network.
		/// </summary>
		/// <param name="token">The cancellation token.</param>
		/// <param name="supervised">if set to <c>true</c> then supervised learning is performed.</param>
		/// <param name="network">The neural network.</param>
		private void Train(CancellationToken token, bool supervised, INeuralNetwork network)
		{
			int numberOfEpochs = (int)this.epochsUpDown.Value;
			List<double> errorHistory = new List<double>(numberOfEpochs);
			this.currentNetwork.LearningProbability = Convert.ToDouble(this.learningProbabilityUpDown.Value);
			for (int epoch = 1; epoch <= numberOfEpochs; epoch++)
			{
				//
				// Adjust the maximum value of the progress bar
				//
				this.trainingProgressBar.BeginInvoke((Action)(() => this.trainingProgressBar.Maximum = this.trainingSet.Length));
				//
				// Take note of the current time
				//
				DateTime start = DateTime.Now;
				//
				// Show some basic information about the network
				//
				this.AppendInfoLine("Epoch " + epoch);
				this.ShowLabelDegreeDistribution(network);
				this.ShowEdgeCount("Before training", network);
				//
				// Reset the progress bar
				//
				this.trainingProgressBar.BeginInvoke((Action)(() => this.trainingProgressBar.Value = 0));
				//
				// Create an observer function which updates the progress bar as each sample is processed
				//
				Action<int> observer = (x) => this.trainingProgressBar.BeginInvoke((Action)(() => this.trainingProgressBar.Value = x));
				//
				// Pick the appropriate training method
				//
				if (supervised)
				{
					network.TrainSupervised(this.trainingSet.AsRandom().ToArray(), observer, this.supervisedLearningMode);
					//network.TrainSupervised(this.miniBatches[epoch], observer, this.jointLearning);
					this.ShowEdgeCount("After supervised training", network);
					this.ShowError(network, errorHistory);
				}
				else
				{
					network.TrainUnsupervised(this.trainingSet.AsRandom().ToArray(), observer);
					//network.TrainUnsupervised(this.miniBatches[epoch], observer);
					this.ShowEdgeCount("After unsupervised training", network);
				}
				//
				// Visualize the network (i.e. show what individual neurons like)
				//
				this.VisualizeNetwork(network);
				//
				// Print out the duration of this training epoch
				//
				this.AppendInfoLine(string.Format("Epoch duration: {0}", DateTime.Now - start));
				//
				// Check if the user has pressed the Stop button
				//
				if (token.IsCancellationRequested)
				{
					this.AppendInfoLine("Stopped at the request of the user");
					this.TrainingComplete();
					return;
				}
				//
				// If the classification error hasn't improved in the last X epochs, stop training.
				//
				if (epoch > GracePeriod && errorHistory.Min() < errorHistory.SkipWhile((val, index) => index < epoch - GracePeriod).Min())
				{
					this.AppendInfoLine("Stopping early since the classification error hasn't improved for " + GracePeriod + " epochs");
					this.TrainingComplete();
					return;
				}
			}

			this.AppendInfoLine(this.currentNetwork.ThresholdsToString());
			this.TrainingComplete();
		}

		/// <summary>
		/// Resets the accuracy chart to have all zero values.
		/// </summary>
		private void ResetAccuracyChart()
		{
			//
			// Remove any existing points
			//
			foreach (var series in this.accuracyColumnChart.Series)
			{
				series.Points.Clear();
			}
			//
			// Set all metrics to 0
			//
			for (int classIndex = 0; classIndex < this.dataProvider.NumberOfClasses; classIndex++)
			{
				for (int seriesIndex = 0; seriesIndex < 3; seriesIndex++)
				{
					this.accuracyColumnChart.Series[seriesIndex].Points.AddXY(classIndex, 0);
				}
			}
		}
		#endregion

		#region Experiment batches
		/// <summary>
		/// Runs the experiment batch.
		/// </summary>
		/// <param name="batch">The batch of experiments to run.</param>
		/// <param name="outputDirectory">The directory in which to save logs and neural networks.</param>
		private void RunExperimentBatch(List<Experiment> batch, string outputDirectory)
		{
			int currentExperimentNumber = 1;
			foreach (var experiment in batch)
			{
				this.AppendInfoLine(string.Format("Starting experiment {0} out of {1}", currentExperimentNumber, batch.Count));
				this.RunExperiment(experiment, outputDirectory);
				currentExperimentNumber++;
			}

			this.AppendInfoLine("Finished performing experiment batch");
		}

		/// <summary>
		/// Runs a single experiment.
		/// </summary>
		/// <param name="experiment">The experiment to run.</param>
		/// <param name="outputDirectory">The directory in which to save logs and neural network.</param>
		private void RunExperiment(Experiment experiment, string outputDirectory)
		{
			//
			// Take note of how long the log currently is. This will mark the start of the log lines which we later copy to a file.
			//
			int logStart = this.infoTextBox.Lines.Length - 1;
			//
			// Construct a neural network as per the experiment specifications
			//
			this.currentNetwork = Utils.ConstructNetwork(experiment.NetworkParameters, this.dataProvider.NumberOfClasses, this.trainingSet[0].SampleData.Length);
			this.NetworkAvailable();
			//
			// Set a member variable which indicates whether supervised learning should be joint
			//
			this.supervisedLearningMode = experiment.SupervisedLearningMode;
			//
			// Set the learning probability
			//
			this.learningProbabilityUpDown.Value = Convert.ToDecimal(experiment.LearningProbability);
			//
			// Perform the training
			//
			this.TrainAndWait(experiment.UnsupervisedEpochs, false);
			this.TrainAndWait(experiment.SupervisedEpochs, true);
			//
			// Again, we take note of the length of the log. This will mark the end of the log lines we later copy to a file.
			//
			int logEnd = this.infoTextBox.Lines.Length;
			//
			// Pick an appropriate abbreviation of the supervised learning mode.
			//
			var learningModeAbbreviation = string.Empty;
			switch (experiment.SupervisedLearningMode)
			{
				case ContrastiveDivergenceLearningMode.FeedForward:
					learningModeAbbreviation = "FF";
					break;
				case ContrastiveDivergenceLearningMode.JointAlternative:
					learningModeAbbreviation = "JA";
					break;
				case ContrastiveDivergenceLearningMode.JointNormal:
					learningModeAbbreviation = "JN";
					break;
			}
			//
			// Construct a prefix which will be shared by all the files we save.
			//
			var fileNameWithoutExtension = string.Format(
				"{0} {5} lp{4:0.000} [{1}u {2}s] ({3:yyyy-MM-dd HHmmss})", 
				experiment.NetworkParameters.ToString().Replace("..", "_"), 
				experiment.UnsupervisedEpochs, 
				experiment.SupervisedEpochs,
				DateTime.Now,
				experiment.LearningProbability,
				learningModeAbbreviation);
			var fullPathWithoutExtension = Path.Combine(outputDirectory, fileNameWithoutExtension);
			//
			// Save the log and classification error files
			//
			using (var logStream = File.CreateText(fullPathWithoutExtension + ".log"))
			{
				using (var classificationErrorStream = File.CreateText(fullPathWithoutExtension + ".err"))
				{
					for (int logIndex = logStart; logIndex < logEnd; logIndex++)
					{
						//
						// Copy this line to the log file
						//
						var currentLine = this.infoTextBox.Lines[logIndex];
						logStream.WriteLine(currentLine);
						//
						// Does this line contain classification error information?
						//
						if (currentLine.StartsWith("Classification error:"))
						{
							//
							// Copy the error to the classification error file. 
							//
							classificationErrorStream.WriteLine(currentLine.Substring(currentLine.LastIndexOf(':') + 2));
						}
					}
				}
			}

			this.AppendInfoLine("Log saved to " + fullPathWithoutExtension + ".log");
			//
			// Save the neural network
			//
			using (var stream = File.Create(fullPathWithoutExtension + ".nn"))
			{
				this.currentNetwork.Save(stream);
			}

			this.AppendInfoLine("Neural network saved to " + fullPathWithoutExtension + ".nn");
		}

		/// <summary>
		/// Starts training a neural network and synchronously waits until training is completed.
		/// </summary>
		/// <param name="epochs">The number of training epochs.</param>
		/// <param name="supervised">If set to <c>true</c> performs supervised learning.</param>
		/// <remarks>This method relies on UI automation. It's not ideal but it gets the job done.</remarks>
		private void TrainAndWait(int epochs, bool supervised)
		{
			//
			// Bail out if there's no training to be done
			//
			if (epochs <= 0)
			{
				return;
			}
			//
			// Set the number of training epochs
			//
			this.epochsUpDown.Value = epochs;
			//
			// Figure out which button to press
			//
			var button = supervised ? this.supervisedTrainingButton : this.unsupervisedTrainingButton;
			button.PerformClick();
			//
			// Clicking the button will disable it while training takes place. We wait until it becomes enabled again.
			//
			do
			{
				Application.DoEvents();
				Thread.Sleep(100);
			} 
			while (!button.Enabled);
		}
		#endregion

		#region Utility methods
		/// <summary>
		/// Pairs together a control (e.g. button) and a menu item so that the enabled state of the menu item is always the same as 
		/// that of the <paramref name="source"/> control.
		/// </summary>
		/// <param name="source">The source control.</param>
		/// <param name="target">The target menu item.</param>
		private void PairEnabledState(Control source, ToolStripMenuItem target)
		{
			//
			// Start by copying the current state
			//
			target.Enabled = source.Enabled;
			//
			// Ensure that the state is copied whenever the enabled state of the source control changes.
			//
			source.EnabledChanged += delegate(object sender, EventArgs eventArgs)
			{
				target.Enabled = source.Enabled;
			};
		}

		/// <summary>
		/// Shows (a subset) of the supplied training/testing data.
		/// </summary>
		/// <param name="data">The data.</param>
		private void ShowData(LabelledSample[] data)
		{
			using (var form = new ShowDataForm(data, this.inputImageWidth, this.inputImageHeight))
			{
				form.ShowDialog();
			}
		}
		#endregion
	}
}