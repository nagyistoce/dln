// -----------------------------------------------------------------------
// <copyright file="CreateNetworkForm.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.UI.WinForms
{
	using System;
	using System.Windows.Forms;
	using Eft.BioNN.Engine.Network;

	/// <summary>
	/// A dialog for providing the parameters for a new neural network.
	/// </summary>
	public partial class CreateNetworkForm : Form
	{
		/// <summary>
		/// The neural network.
		/// </summary>
		private INeuralNetwork network;

		/// <summary>
		/// The number of input units.
		/// </summary>
		private int numberOfInputUnits;

		/// <summary>
		/// The number of classes in the training/testing data set.
		/// </summary>
		private int numberOfClasses;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateNetworkForm"/> class.
		/// </summary>
		public CreateNetworkForm()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateNetworkForm" /> class.
		/// </summary>
		/// <param name="numberOfInputUnits">The number of input units.</param>
		/// <param name="numberOfClasses">The number of classes in the training/testing data set.</param>
		public CreateNetworkForm(int numberOfInputUnits, int numberOfClasses) : this()
		{
			this.numberOfInputUnits = numberOfInputUnits;
			this.numberOfClasses = numberOfClasses;
		}

		/// <summary>
		/// Gets the newly created neural network.
		/// </summary>
		/// <value>
		/// The neural network.
		/// </value>
		public INeuralNetwork NeuralNetwork
		{
			get
			{
				return this.network;
			}
		}

		/// <summary>
		/// Handles the Click event of the okButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void okButton_Click(object sender, EventArgs e)
		{
			//
			// Construct the neural network
			//
			this.network = Utils.ConstructNetwork(this.createNetworkControl.GetNetworkParameters(), this.numberOfClasses, this.numberOfInputUnits);
			//
			// Indicate success and close the window. 
			//
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
