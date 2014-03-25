// -----------------------------------------------------------------------
// <copyright file="CreateNetworkControl.cs" company="Eiríkur Fannar Torfason">
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

	/// <summary>
	/// A user control for specifying the parameters of a neural network.
	/// </summary>
	public partial class CreateNetworkControl : UserControl
	{
		#region Private members
		/// <summary>
		/// A list with information about each of the hidden layers.
		/// </summary>
		private List<HiddenLayer> hiddenLayers = new List<HiddenLayer>();
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateNetworkControl"/> class.
		/// </summary>
		public CreateNetworkControl()
		{
			this.InitializeComponent();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the training algorithm which should be used to train the network.
		/// </summary>
		/// <value>
		/// The training algorithm.
		/// </value>
		private TrainingAlgorithm TrainingAlgorithm
		{
			get
			{
				if (this.cdRadioButton.Checked)
				{
					return TrainingAlgorithm.ContrastiveDivergence;
				}
				else if (this.cdSamplingRadioButton.Checked)
				{
					return TrainingAlgorithm.ContrastiveDivergenceWithSampling;
				}
				else
				{
					return WinForms.TrainingAlgorithm.PersistentContrastiveDivergence;
				}
			}
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Gets the parameter values, as selected by the user, for the creation of a neural network.
		/// </summary>
		/// <returns>The network creation parameter values.</returns>
		public NeuralNetworkCreationParameters GetNetworkParameters()
		{
			return new NeuralNetworkCreationParameters() 
			{ 
				UnitsPerClass = (int)this.unitsPerClassUpDown.Value,
				ActivationThreshold = Convert.ToInt32(this.activationDegreeUpDown.Value),
				HiddenLayerSizes = (from layer in this.hiddenLayers select layer.Units).ToArray(),
				SynapseSuccessProbability = Convert.ToDouble(this.synapseProbabilityUpDown.Value) / 100.0,
				MinWeight = Convert.ToInt32(this.minWeightUpDown.Value),
				MaxWeight = Convert.ToInt32(this.maxWeightUpDown.Value),
				TrainingAlgorithm = this.TrainingAlgorithm,
				ActivationSamples = Convert.ToInt32(this.samplesUpDown.Value)
			};
		}
		#endregion

		#region Event handlers
		/// <summary>
		/// Handles the SelectedIndexChanged event of the hiddenLayerListBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void hiddenLayerListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.removeHiddenLayerButton.Enabled = this.hiddenLayerListBox.SelectedIndex >= 0;
		}

		/// <summary>
		/// Handles the Click event of the addHiddenLayerButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void addHiddenLayerButton_Click(object sender, EventArgs e)
		{
			this.hiddenLayers.Add(new HiddenLayer()
			{
				Units = (int)this.hiddenUnitsUpDown.Value,
				Index = this.hiddenLayers.Count + 1
			});
			//
			// HACK: Redraw listbox
			//
			this.hiddenLayerListBox.DataSource = this.hiddenLayers.ToArray();
		}

		/// <summary>
		/// Handles the Click event of the removeHiddenLayerButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void removeHiddenLayerButton_Click(object sender, EventArgs e)
		{
			if (this.hiddenLayerListBox.SelectedIndex >= 0)
			{
				this.hiddenLayers.RemoveAt(this.hiddenLayerListBox.SelectedIndex);
				//
				// Re-index the hidden layers
				//
				for (int index = 1; index <= this.hiddenLayers.Count; index++)
				{
					var layer = this.hiddenLayers[index - 1];
					if (layer != null)
					{
						layer.Index = index;
					}
				}
				//
				// HACK: Redraw listbox
				//
				this.hiddenLayerListBox.DataSource = this.hiddenLayers.ToArray();
			}
		}

		/// <summary>
		/// Handles the CheckedChanged event of the cdRadioButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void cdRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			this.samplesUpDown.Enabled = !cdRadioButton.Checked;
		}
		#endregion

		/// <summary>
		/// A container for hidden layer properties
		/// </summary>
		private class HiddenLayer
		{
			/// <summary>
			/// The number of units in this layer
			/// </summary>
			private int units;

			/// <summary>
			/// The index of this layer.
			/// </summary>
			private int index;

			/// <summary>
			/// Gets or sets the number of units in this layer.
			/// </summary>
			/// <value>
			/// The number of units.
			/// </value>
			public int Units
			{
				get
				{
					return this.units;
				}

				set
				{
					this.units = value;
				}
			}

			/// <summary>
			/// Gets or sets the index (i.e. relative ordering) of this layer.
			/// </summary>
			/// <value>
			/// The index.
			/// </value>
			public int Index
			{
				get
				{
					return this.index;
				}

				set
				{
					this.index = value;
				}
			}

			/// <summary>
			/// Returns a <see cref="System.String" /> that represents this instance.
			/// </summary>
			/// <returns>
			/// A <see cref="System.String" /> that represents this instance.
			/// </returns>
			public override string ToString()
			{
				return string.Format("Layer: {0}. Units: {1}", this.Index, this.Units);
			}
		}
	}
}
