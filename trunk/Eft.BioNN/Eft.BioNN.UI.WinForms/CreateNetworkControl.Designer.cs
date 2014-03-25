namespace Eft.BioNN.UI.WinForms
{
	partial class CreateNetworkControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.unitsPerClassUpDown = new System.Windows.Forms.NumericUpDown();
			this.hiddenLayerListBox = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.hiddenUnitsUpDown = new System.Windows.Forms.NumericUpDown();
			this.addHiddenLayerButton = new System.Windows.Forms.Button();
			this.removeHiddenLayerButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.maxWeightUpDown = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.minWeightUpDown = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.synapseProbabilityUpDown = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.activationDegreeUpDown = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.samplesUpDown = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.pcdRadioButton = new System.Windows.Forms.RadioButton();
			this.cdSamplingRadioButton = new System.Windows.Forms.RadioButton();
			this.cdRadioButton = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.unitsPerClassUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.hiddenUnitsUpDown)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.maxWeightUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.minWeightUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.synapseProbabilityUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.activationDegreeUpDown)).BeginInit();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.samplesUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(95, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Neurons per class:";
			// 
			// unitsPerClassUpDown
			// 
			this.unitsPerClassUpDown.Location = new System.Drawing.Point(141, 19);
			this.unitsPerClassUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.unitsPerClassUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.unitsPerClassUpDown.Name = "unitsPerClassUpDown";
			this.unitsPerClassUpDown.Size = new System.Drawing.Size(58, 20);
			this.unitsPerClassUpDown.TabIndex = 1;
			this.unitsPerClassUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
			// 
			// hiddenLayerListBox
			// 
			this.hiddenLayerListBox.FormattingEnabled = true;
			this.hiddenLayerListBox.Location = new System.Drawing.Point(9, 74);
			this.hiddenLayerListBox.Name = "hiddenLayerListBox";
			this.hiddenLayerListBox.Size = new System.Drawing.Size(268, 121);
			this.hiddenLayerListBox.TabIndex = 6;
			this.hiddenLayerListBox.SelectedIndexChanged += new System.EventHandler(this.hiddenLayerListBox_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(85, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Hidden neurons:";
			// 
			// hiddenUnitsUpDown
			// 
			this.hiddenUnitsUpDown.Location = new System.Drawing.Point(91, 48);
			this.hiddenUnitsUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.hiddenUnitsUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.hiddenUnitsUpDown.Name = "hiddenUnitsUpDown";
			this.hiddenUnitsUpDown.Size = new System.Drawing.Size(48, 20);
			this.hiddenUnitsUpDown.TabIndex = 3;
			this.hiddenUnitsUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			// 
			// addHiddenLayerButton
			// 
			this.addHiddenLayerButton.Location = new System.Drawing.Point(145, 45);
			this.addHiddenLayerButton.Name = "addHiddenLayerButton";
			this.addHiddenLayerButton.Size = new System.Drawing.Size(63, 23);
			this.addHiddenLayerButton.TabIndex = 4;
			this.addHiddenLayerButton.Text = "Add";
			this.addHiddenLayerButton.UseVisualStyleBackColor = true;
			this.addHiddenLayerButton.Click += new System.EventHandler(this.addHiddenLayerButton_Click);
			// 
			// removeHiddenLayerButton
			// 
			this.removeHiddenLayerButton.Enabled = false;
			this.removeHiddenLayerButton.Location = new System.Drawing.Point(214, 45);
			this.removeHiddenLayerButton.Name = "removeHiddenLayerButton";
			this.removeHiddenLayerButton.Size = new System.Drawing.Size(63, 23);
			this.removeHiddenLayerButton.TabIndex = 5;
			this.removeHiddenLayerButton.Text = "Remove";
			this.removeHiddenLayerButton.UseVisualStyleBackColor = true;
			this.removeHiddenLayerButton.Click += new System.EventHandler(this.removeHiddenLayerButton_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.unitsPerClassUpDown);
			this.groupBox1.Controls.Add(this.removeHiddenLayerButton);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.addHiddenLayerButton);
			this.groupBox1.Controls.Add(this.hiddenLayerListBox);
			this.groupBox1.Controls.Add(this.hiddenUnitsUpDown);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(285, 207);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Structure";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.maxWeightUpDown);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.minWeightUpDown);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.synapseProbabilityUpDown);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.activationDegreeUpDown);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(291, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(249, 122);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Behavior";
			// 
			// maxWeightUpDown
			// 
			this.maxWeightUpDown.Location = new System.Drawing.Point(172, 93);
			this.maxWeightUpDown.Name = "maxWeightUpDown";
			this.maxWeightUpDown.Size = new System.Drawing.Size(58, 20);
			this.maxWeightUpDown.TabIndex = 7;
			this.maxWeightUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 95);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 13);
			this.label6.TabIndex = 6;
			this.label6.Text = "Max weight:";
			// 
			// minWeightUpDown
			// 
			this.minWeightUpDown.Location = new System.Drawing.Point(172, 67);
			this.minWeightUpDown.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.minWeightUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
			this.minWeightUpDown.Name = "minWeightUpDown";
			this.minWeightUpDown.Size = new System.Drawing.Size(58, 20);
			this.minWeightUpDown.TabIndex = 5;
			this.minWeightUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 69);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(61, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Min weight:";
			// 
			// synapseProbabilityUpDown
			// 
			this.synapseProbabilityUpDown.Location = new System.Drawing.Point(172, 41);
			this.synapseProbabilityUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.synapseProbabilityUpDown.Name = "synapseProbabilityUpDown";
			this.synapseProbabilityUpDown.Size = new System.Drawing.Size(58, 20);
			this.synapseProbabilityUpDown.TabIndex = 3;
			this.synapseProbabilityUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 43);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(160, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Synapse success probability (%):";
			// 
			// activationDegreeUpDown
			// 
			this.activationDegreeUpDown.Location = new System.Drawing.Point(172, 15);
			this.activationDegreeUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.activationDegreeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.activationDegreeUpDown.Name = "activationDegreeUpDown";
			this.activationDegreeUpDown.Size = new System.Drawing.Size(58, 20);
			this.activationDegreeUpDown.TabIndex = 1;
			this.activationDegreeUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 17);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(103, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Activation threshold:";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.samplesUpDown);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.pcdRadioButton);
			this.groupBox3.Controls.Add(this.cdSamplingRadioButton);
			this.groupBox3.Controls.Add(this.cdRadioButton);
			this.groupBox3.Location = new System.Drawing.Point(291, 128);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(249, 79);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Training";
			// 
			// samplesUpDown
			// 
			this.samplesUpDown.Enabled = false;
			this.samplesUpDown.Location = new System.Drawing.Point(172, 48);
			this.samplesUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.samplesUpDown.Name = "samplesUpDown";
			this.samplesUpDown.Size = new System.Drawing.Size(58, 20);
			this.samplesUpDown.TabIndex = 8;
			this.samplesUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 50);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(98, 13);
			this.label7.TabIndex = 3;
			this.label7.Text = "Activation samples:";
			// 
			// pcdRadioButton
			// 
			this.pcdRadioButton.AutoSize = true;
			this.pcdRadioButton.Location = new System.Drawing.Point(151, 19);
			this.pcdRadioButton.Name = "pcdRadioButton";
			this.pcdRadioButton.Size = new System.Drawing.Size(89, 17);
			this.pcdRadioButton.TabIndex = 2;
			this.pcdRadioButton.Text = "Persistent CD";
			this.pcdRadioButton.UseVisualStyleBackColor = true;
			// 
			// cdSamplingRadioButton
			// 
			this.cdSamplingRadioButton.AutoSize = true;
			this.cdSamplingRadioButton.Location = new System.Drawing.Point(55, 19);
			this.cdSamplingRadioButton.Name = "cdSamplingRadioButton";
			this.cdSamplingRadioButton.Size = new System.Drawing.Size(90, 17);
			this.cdSamplingRadioButton.TabIndex = 1;
			this.cdSamplingRadioButton.Text = "CD (sampling)";
			this.cdSamplingRadioButton.UseVisualStyleBackColor = true;
			// 
			// cdRadioButton
			// 
			this.cdRadioButton.AutoSize = true;
			this.cdRadioButton.Checked = true;
			this.cdRadioButton.Location = new System.Drawing.Point(9, 19);
			this.cdRadioButton.Name = "cdRadioButton";
			this.cdRadioButton.Size = new System.Drawing.Size(40, 17);
			this.cdRadioButton.TabIndex = 0;
			this.cdRadioButton.TabStop = true;
			this.cdRadioButton.Text = "CD";
			this.cdRadioButton.UseVisualStyleBackColor = true;
			this.cdRadioButton.CheckedChanged += new System.EventHandler(this.cdRadioButton_CheckedChanged);
			// 
			// CreateNetworkControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "CreateNetworkControl";
			this.Size = new System.Drawing.Size(544, 211);
			((System.ComponentModel.ISupportInitialize)(this.unitsPerClassUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.hiddenUnitsUpDown)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.maxWeightUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.minWeightUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.synapseProbabilityUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.activationDegreeUpDown)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.samplesUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown unitsPerClassUpDown;
		private System.Windows.Forms.ListBox hiddenLayerListBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown hiddenUnitsUpDown;
		private System.Windows.Forms.Button addHiddenLayerButton;
		private System.Windows.Forms.Button removeHiddenLayerButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown activationDegreeUpDown;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown synapseProbabilityUpDown;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown maxWeightUpDown;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown minWeightUpDown;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.NumericUpDown samplesUpDown;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.RadioButton pcdRadioButton;
		private System.Windows.Forms.RadioButton cdSamplingRadioButton;
		private System.Windows.Forms.RadioButton cdRadioButton;
	}
}
