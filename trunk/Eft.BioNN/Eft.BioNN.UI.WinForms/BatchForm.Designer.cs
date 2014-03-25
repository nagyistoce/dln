namespace Eft.BioNN.UI.WinForms
{
	partial class BatchForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.supervisedEpochsUpDown = new System.Windows.Forms.NumericUpDown();
			this.unsupervisedEpochsUpDown = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.addExperimentButton = new System.Windows.Forms.Button();
			this.experimentDataGridView = new System.Windows.Forms.DataGridView();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.label3 = new System.Windows.Forms.Label();
			this.outputDirectoryTextBox = new System.Windows.Forms.TextBox();
			this.changeDirectoryButton = new System.Windows.Forms.Button();
			this.runButton = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.learningProbabilityUpDown = new System.Windows.Forms.NumericUpDown();
			this.createNetworkControl = new Eft.BioNN.UI.WinForms.CreateNetworkControl();
			this.label5 = new System.Windows.Forms.Label();
			this.feedForwardRadioButton = new System.Windows.Forms.RadioButton();
			this.jointNormalRadioButton = new System.Windows.Forms.RadioButton();
			this.jointAlternativeRdioButton = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.supervisedEpochsUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.unsupervisedEpochsUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.experimentDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.learningProbabilityUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.jointAlternativeRdioButton);
			this.groupBox1.Controls.Add(this.jointNormalRadioButton);
			this.groupBox1.Controls.Add(this.feedForwardRadioButton);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.learningProbabilityUpDown);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.supervisedEpochsUpDown);
			this.groupBox1.Controls.Add(this.unsupervisedEpochsUpDown);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(558, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(201, 184);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Execution";
			// 
			// supervisedEpochsUpDown
			// 
			this.supervisedEpochsUpDown.Location = new System.Drawing.Point(133, 45);
			this.supervisedEpochsUpDown.Name = "supervisedEpochsUpDown";
			this.supervisedEpochsUpDown.Size = new System.Drawing.Size(58, 20);
			this.supervisedEpochsUpDown.TabIndex = 4;
			this.supervisedEpochsUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// unsupervisedEpochsUpDown
			// 
			this.unsupervisedEpochsUpDown.Location = new System.Drawing.Point(133, 18);
			this.unsupervisedEpochsUpDown.Name = "unsupervisedEpochsUpDown";
			this.unsupervisedEpochsUpDown.Size = new System.Drawing.Size(58, 20);
			this.unsupervisedEpochsUpDown.TabIndex = 2;
			this.unsupervisedEpochsUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Supervised epochs:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(113, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Unsupervised epochs:";
			// 
			// addExperimentButton
			// 
			this.addExperimentButton.Location = new System.Drawing.Point(684, 202);
			this.addExperimentButton.Name = "addExperimentButton";
			this.addExperimentButton.Size = new System.Drawing.Size(75, 23);
			this.addExperimentButton.TabIndex = 2;
			this.addExperimentButton.Text = "Add to list";
			this.addExperimentButton.UseVisualStyleBackColor = true;
			this.addExperimentButton.Click += new System.EventHandler(this.addExperimentButton_Click);
			// 
			// experimentDataGridView
			// 
			this.experimentDataGridView.AllowUserToAddRows = false;
			this.experimentDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.experimentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.experimentDataGridView.Location = new System.Drawing.Point(12, 231);
			this.experimentDataGridView.Name = "experimentDataGridView";
			this.experimentDataGridView.ReadOnly = true;
			this.experimentDataGridView.Size = new System.Drawing.Size(747, 156);
			this.experimentDataGridView.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 397);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(211, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Directory in which to save logs and models:";
			// 
			// outputDirectoryTextBox
			// 
			this.outputDirectoryTextBox.Location = new System.Drawing.Point(224, 395);
			this.outputDirectoryTextBox.Name = "outputDirectoryTextBox";
			this.outputDirectoryTextBox.ReadOnly = true;
			this.outputDirectoryTextBox.Size = new System.Drawing.Size(452, 20);
			this.outputDirectoryTextBox.TabIndex = 5;
			// 
			// changeDirectoryButton
			// 
			this.changeDirectoryButton.Location = new System.Drawing.Point(682, 393);
			this.changeDirectoryButton.Name = "changeDirectoryButton";
			this.changeDirectoryButton.Size = new System.Drawing.Size(75, 23);
			this.changeDirectoryButton.TabIndex = 6;
			this.changeDirectoryButton.Text = "Change...";
			this.changeDirectoryButton.UseVisualStyleBackColor = true;
			this.changeDirectoryButton.Click += new System.EventHandler(this.changeDirectoryButton_Click);
			// 
			// runButton
			// 
			this.runButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.runButton.Location = new System.Drawing.Point(634, 439);
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(123, 23);
			this.runButton.TabIndex = 7;
			this.runButton.Text = "Run experiments";
			this.runButton.UseVisualStyleBackColor = true;
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 75);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(101, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Learning probability:";
			// 
			// learningProbabilityUpDown
			// 
			this.learningProbabilityUpDown.DecimalPlaces = 3;
			this.learningProbabilityUpDown.Location = new System.Drawing.Point(133, 73);
			this.learningProbabilityUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.learningProbabilityUpDown.Name = "learningProbabilityUpDown";
			this.learningProbabilityUpDown.Size = new System.Drawing.Size(58, 20);
			this.learningProbabilityUpDown.TabIndex = 5;
			this.learningProbabilityUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
			// 
			// createNetworkControl
			// 
			this.createNetworkControl.Location = new System.Drawing.Point(12, 12);
			this.createNetworkControl.Name = "createNetworkControl";
			this.createNetworkControl.Size = new System.Drawing.Size(551, 213);
			this.createNetworkControl.TabIndex = 0;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 99);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(129, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = "Supervised learning mode";
			// 
			// feedForwardRadioButton
			// 
			this.feedForwardRadioButton.AutoSize = true;
			this.feedForwardRadioButton.Location = new System.Drawing.Point(15, 115);
			this.feedForwardRadioButton.Name = "feedForwardRadioButton";
			this.feedForwardRadioButton.Size = new System.Drawing.Size(87, 17);
			this.feedForwardRadioButton.TabIndex = 8;
			this.feedForwardRadioButton.Text = "Feed forward";
			this.feedForwardRadioButton.UseVisualStyleBackColor = true;
			// 
			// jointNormalRadioButton
			// 
			this.jointNormalRadioButton.AutoSize = true;
			this.jointNormalRadioButton.Location = new System.Drawing.Point(15, 138);
			this.jointNormalRadioButton.Name = "jointNormalRadioButton";
			this.jointNormalRadioButton.Size = new System.Drawing.Size(87, 17);
			this.jointNormalRadioButton.TabIndex = 9;
			this.jointNormalRadioButton.Text = "Joint (normal)";
			this.jointNormalRadioButton.UseVisualStyleBackColor = true;
			// 
			// jointAlternativeRdioButton
			// 
			this.jointAlternativeRdioButton.AutoSize = true;
			this.jointAlternativeRdioButton.Checked = true;
			this.jointAlternativeRdioButton.Location = new System.Drawing.Point(15, 161);
			this.jointAlternativeRdioButton.Name = "jointAlternativeRdioButton";
			this.jointAlternativeRdioButton.Size = new System.Drawing.Size(105, 17);
			this.jointAlternativeRdioButton.TabIndex = 10;
			this.jointAlternativeRdioButton.TabStop = true;
			this.jointAlternativeRdioButton.Text = "Joint (alternative)";
			this.jointAlternativeRdioButton.UseVisualStyleBackColor = true;
			// 
			// BatchForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(769, 474);
			this.Controls.Add(this.runButton);
			this.Controls.Add(this.changeDirectoryButton);
			this.Controls.Add(this.outputDirectoryTextBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.experimentDataGridView);
			this.Controls.Add(this.addExperimentButton);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.createNetworkControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BatchForm";
			this.Text = "Experiment batch";
			this.Load += new System.EventHandler(this.BatchForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.supervisedEpochsUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.unsupervisedEpochsUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.experimentDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.learningProbabilityUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private CreateNetworkControl createNetworkControl;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown supervisedEpochsUpDown;
		private System.Windows.Forms.NumericUpDown unsupervisedEpochsUpDown;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button addExperimentButton;
		private System.Windows.Forms.DataGridView experimentDataGridView;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox outputDirectoryTextBox;
		private System.Windows.Forms.Button changeDirectoryButton;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown learningProbabilityUpDown;
		private System.Windows.Forms.RadioButton jointAlternativeRdioButton;
		private System.Windows.Forms.RadioButton jointNormalRadioButton;
		private System.Windows.Forms.RadioButton feedForwardRadioButton;
		private System.Windows.Forms.Label label5;
	}
}