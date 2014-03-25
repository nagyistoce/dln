// -----------------------------------------------------------------------
// <copyright file="BatchForm.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.UI.WinForms
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Windows.Forms;
	using Eft.BioNN.Engine.Learning;

	/// <summary>
	/// A form used to define a batch of experiments to run.
	/// </summary>
	public partial class BatchForm : Form
	{
		#region Private members
		/// <summary>
		/// The experiments in the batch.
		/// </summary>
		private BindingList<Experiment> experiments = new BindingList<Experiment>();
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="BatchForm"/> class.
		/// </summary>
		public BatchForm()
		{
			this.InitializeComponent();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the list of experiments in the batch.
		/// </summary>
		/// <value>
		/// The experiments.
		/// </value>
		public List<Experiment> Experiments
		{
			get
			{
				return this.experiments.ToList();
			}
		}

		/// <summary>
		/// Gets the path to the directory which should contain all files generated as part of the experiments.
		/// </summary>
		/// <value>
		/// The output directory.
		/// </value>
		public string OutputDirectory
		{
			get
			{
				return this.outputDirectoryTextBox.Text;
			}
		}
		#endregion

		#region Event handlers
		/// <summary>
		/// Handles the Click event of the addExperimentButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void addExperimentButton_Click(object sender, EventArgs e)
		{
			//
			// Figure out wich supervised learning mode was selected
			//
			ContrastiveDivergenceLearningMode supervisedLearningMode = ContrastiveDivergenceLearningMode.FeedForward;
			if (jointAlternativeRdioButton.Checked)
			{
				supervisedLearningMode = ContrastiveDivergenceLearningMode.JointAlternative;
			}
			else if (jointNormalRadioButton.Checked)
			{
				supervisedLearningMode = ContrastiveDivergenceLearningMode.JointNormal;
			}
			//
			// Validation: ensure that joint-learning is only selected if there's a hidden layer
			//
			var networkParameters = this.createNetworkControl.GetNetworkParameters();
			if (supervisedLearningMode != ContrastiveDivergenceLearningMode.FeedForward && networkParameters.HiddenLayerSizes.Length == 0)
			{
				MessageBox.Show("Joint learning requires at least one hidden layer");
				return;
			}
			//
			// Add the experiment to the list
			//
			this.experiments.Add(new Experiment() 
			{ 
				NetworkParameters = networkParameters,
				SupervisedLearningMode = supervisedLearningMode,
				SupervisedEpochs = Convert.ToInt32(this.supervisedEpochsUpDown.Value),
				UnsupervisedEpochs = Convert.ToInt32(this.unsupervisedEpochsUpDown.Value),
				LearningProbability = Convert.ToDouble(this.learningProbabilityUpDown.Value)
			});
			//
			// Resize the columns of the grid
			//
			foreach (var column in this.experimentDataGridView.Columns.Cast<DataGridViewColumn>())
			{
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			}
		}

		/// <summary>
		/// Handles the Load event of the BatchForm control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void BatchForm_Load(object sender, EventArgs e)
		{
			this.experimentDataGridView.DataSource = this.experiments;
			this.outputDirectoryTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}

		/// <summary>
		/// Handles the Click event of the changeDirectoryButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void changeDirectoryButton_Click(object sender, EventArgs e)
		{
			this.folderBrowserDialog.SelectedPath = this.outputDirectoryTextBox.Text;
			if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				this.outputDirectoryTextBox.Text = this.folderBrowserDialog.SelectedPath;
			}
		}

		/// <summary>
		/// Handles the Click event of the runButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void runButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		#endregion
	}
}
