namespace Eft.BioNN.UI.WinForms
{
	partial class MainForm
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.infoTextBox = new System.Windows.Forms.TextBox();
			this.stopButton = new System.Windows.Forms.Button();
			this.containerLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.createNetworkButton = new System.Windows.Forms.Button();
			this.unsupervisedTrainingButton = new System.Windows.Forms.Button();
			this.supervisedTrainingButton = new System.Windows.Forms.Button();
			this.saveNetworkButton = new System.Windows.Forms.Button();
			this.networkGroupBox = new System.Windows.Forms.GroupBox();
			this.dreamButton = new System.Windows.Forms.Button();
			this.networkInfoTextBox = new System.Windows.Forms.TextBox();
			this.loadNetworkButton = new System.Windows.Forms.Button();
			this.trainingGroupBox = new System.Windows.Forms.GroupBox();
			this.learningProbabilityUpDown = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.epochsUpDown = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.trainingProgressBar = new System.Windows.Forms.ProgressBar();
			this.testButton = new System.Windows.Forms.Button();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.accuracyColumnChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.trainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startUnsupervisedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.startSupervisedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.visualTesterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.experimentBatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.trainingDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.testingDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.networkGroupBox.SuspendLayout();
			this.trainingGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.learningProbabilityUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.epochsUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.accuracyColumnChart)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// infoTextBox
			// 
			this.infoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.infoTextBox.Location = new System.Drawing.Point(12, 491);
			this.infoTextBox.Multiline = true;
			this.infoTextBox.Name = "infoTextBox";
			this.infoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.infoTextBox.Size = new System.Drawing.Size(392, 105);
			this.infoTextBox.TabIndex = 2;
			// 
			// stopButton
			// 
			this.stopButton.Enabled = false;
			this.stopButton.Location = new System.Drawing.Point(198, 19);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(90, 23);
			this.stopButton.TabIndex = 2;
			this.stopButton.Text = "Stop!";
			this.stopButton.UseVisualStyleBackColor = true;
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// containerLayoutPanel
			// 
			this.containerLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.containerLayoutPanel.AutoScroll = true;
			this.containerLayoutPanel.Location = new System.Drawing.Point(410, 55);
			this.containerLayoutPanel.Name = "containerLayoutPanel";
			this.containerLayoutPanel.Size = new System.Drawing.Size(557, 541);
			this.containerLayoutPanel.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(410, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Visualization";
			// 
			// createNetworkButton
			// 
			this.createNetworkButton.Location = new System.Drawing.Point(6, 19);
			this.createNetworkButton.Name = "createNetworkButton";
			this.createNetworkButton.Size = new System.Drawing.Size(90, 23);
			this.createNetworkButton.TabIndex = 0;
			this.createNetworkButton.Text = "New...";
			this.createNetworkButton.UseVisualStyleBackColor = true;
			this.createNetworkButton.Click += new System.EventHandler(this.createNetworkButton_Click);
			// 
			// unsupervisedTrainingButton
			// 
			this.unsupervisedTrainingButton.Enabled = false;
			this.unsupervisedTrainingButton.Location = new System.Drawing.Point(6, 19);
			this.unsupervisedTrainingButton.Name = "unsupervisedTrainingButton";
			this.unsupervisedTrainingButton.Size = new System.Drawing.Size(90, 23);
			this.unsupervisedTrainingButton.TabIndex = 0;
			this.unsupervisedTrainingButton.Text = "Unsupervised";
			this.unsupervisedTrainingButton.UseVisualStyleBackColor = true;
			this.unsupervisedTrainingButton.Click += new System.EventHandler(this.unsupervisedTrainingButton_Click);
			// 
			// supervisedTrainingButton
			// 
			this.supervisedTrainingButton.Enabled = false;
			this.supervisedTrainingButton.Location = new System.Drawing.Point(102, 19);
			this.supervisedTrainingButton.Name = "supervisedTrainingButton";
			this.supervisedTrainingButton.Size = new System.Drawing.Size(90, 23);
			this.supervisedTrainingButton.TabIndex = 1;
			this.supervisedTrainingButton.Text = "Supervised";
			this.supervisedTrainingButton.UseVisualStyleBackColor = true;
			this.supervisedTrainingButton.Click += new System.EventHandler(this.supervisedTrainingButton_Click);
			// 
			// saveNetworkButton
			// 
			this.saveNetworkButton.Enabled = false;
			this.saveNetworkButton.Location = new System.Drawing.Point(198, 19);
			this.saveNetworkButton.Name = "saveNetworkButton";
			this.saveNetworkButton.Size = new System.Drawing.Size(90, 23);
			this.saveNetworkButton.TabIndex = 2;
			this.saveNetworkButton.Text = "Save...";
			this.saveNetworkButton.UseVisualStyleBackColor = true;
			this.saveNetworkButton.Click += new System.EventHandler(this.saveNetworkButton_Click);
			// 
			// networkGroupBox
			// 
			this.networkGroupBox.Controls.Add(this.dreamButton);
			this.networkGroupBox.Controls.Add(this.networkInfoTextBox);
			this.networkGroupBox.Controls.Add(this.loadNetworkButton);
			this.networkGroupBox.Controls.Add(this.createNetworkButton);
			this.networkGroupBox.Controls.Add(this.saveNetworkButton);
			this.networkGroupBox.Location = new System.Drawing.Point(12, 36);
			this.networkGroupBox.Name = "networkGroupBox";
			this.networkGroupBox.Size = new System.Drawing.Size(392, 151);
			this.networkGroupBox.TabIndex = 0;
			this.networkGroupBox.TabStop = false;
			this.networkGroupBox.Text = "Network";
			// 
			// dreamButton
			// 
			this.dreamButton.Location = new System.Drawing.Point(294, 19);
			this.dreamButton.Name = "dreamButton";
			this.dreamButton.Size = new System.Drawing.Size(90, 23);
			this.dreamButton.TabIndex = 4;
			this.dreamButton.Text = "Dream...";
			this.dreamButton.UseVisualStyleBackColor = true;
			this.dreamButton.Click += new System.EventHandler(this.dreamButton_Click);
			// 
			// networkInfoTextBox
			// 
			this.networkInfoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.networkInfoTextBox.Location = new System.Drawing.Point(6, 48);
			this.networkInfoTextBox.Multiline = true;
			this.networkInfoTextBox.Name = "networkInfoTextBox";
			this.networkInfoTextBox.ReadOnly = true;
			this.networkInfoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.networkInfoTextBox.Size = new System.Drawing.Size(379, 97);
			this.networkInfoTextBox.TabIndex = 3;
			this.networkInfoTextBox.Text = "No network loaded";
			// 
			// loadNetworkButton
			// 
			this.loadNetworkButton.Location = new System.Drawing.Point(102, 19);
			this.loadNetworkButton.Name = "loadNetworkButton";
			this.loadNetworkButton.Size = new System.Drawing.Size(90, 23);
			this.loadNetworkButton.TabIndex = 1;
			this.loadNetworkButton.Text = "Open...";
			this.loadNetworkButton.UseVisualStyleBackColor = true;
			this.loadNetworkButton.Click += new System.EventHandler(this.loadNetworkButton_Click);
			// 
			// trainingGroupBox
			// 
			this.trainingGroupBox.Controls.Add(this.learningProbabilityUpDown);
			this.trainingGroupBox.Controls.Add(this.label3);
			this.trainingGroupBox.Controls.Add(this.epochsUpDown);
			this.trainingGroupBox.Controls.Add(this.label1);
			this.trainingGroupBox.Controls.Add(this.trainingProgressBar);
			this.trainingGroupBox.Controls.Add(this.testButton);
			this.trainingGroupBox.Controls.Add(this.unsupervisedTrainingButton);
			this.trainingGroupBox.Controls.Add(this.supervisedTrainingButton);
			this.trainingGroupBox.Controls.Add(this.stopButton);
			this.trainingGroupBox.Location = new System.Drawing.Point(12, 193);
			this.trainingGroupBox.Name = "trainingGroupBox";
			this.trainingGroupBox.Size = new System.Drawing.Size(392, 77);
			this.trainingGroupBox.TabIndex = 1;
			this.trainingGroupBox.TabStop = false;
			this.trainingGroupBox.Text = "Training";
			// 
			// learningProbabilityUpDown
			// 
			this.learningProbabilityUpDown.DecimalPlaces = 3;
			this.learningProbabilityUpDown.Location = new System.Drawing.Point(181, 48);
			this.learningProbabilityUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.learningProbabilityUpDown.Name = "learningProbabilityUpDown";
			this.learningProbabilityUpDown.Size = new System.Drawing.Size(55, 20);
			this.learningProbabilityUpDown.TabIndex = 8;
			this.learningProbabilityUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(119, 51);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "P(update):";
			// 
			// epochsUpDown
			// 
			this.epochsUpDown.Location = new System.Drawing.Point(58, 49);
			this.epochsUpDown.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
			this.epochsUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.epochsUpDown.Name = "epochsUpDown";
			this.epochsUpDown.Size = new System.Drawing.Size(55, 20);
			this.epochsUpDown.TabIndex = 6;
			this.epochsUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Epochs:";
			// 
			// trainingProgressBar
			// 
			this.trainingProgressBar.Location = new System.Drawing.Point(242, 48);
			this.trainingProgressBar.Name = "trainingProgressBar";
			this.trainingProgressBar.Size = new System.Drawing.Size(142, 23);
			this.trainingProgressBar.TabIndex = 4;
			// 
			// testButton
			// 
			this.testButton.Enabled = false;
			this.testButton.Location = new System.Drawing.Point(294, 19);
			this.testButton.Name = "testButton";
			this.testButton.Size = new System.Drawing.Size(90, 23);
			this.testButton.TabIndex = 3;
			this.testButton.Text = "Test";
			this.testButton.UseVisualStyleBackColor = true;
			this.testButton.Click += new System.EventHandler(this.testButton_Click);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "nn";
			this.saveFileDialog.Filter = "Neural networks|*.nn";
			this.saveFileDialog.Title = "Save network";
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Neural networks|*.nn";
			this.openFileDialog.Title = "Open neural network";
			// 
			// accuracyColumnChart
			// 
			this.accuracyColumnChart.BorderlineColor = System.Drawing.SystemColors.ControlLight;
			this.accuracyColumnChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
			chartArea1.AxisX.Interval = 1D;
			chartArea1.AxisX.IntervalOffset = 0.5D;
			chartArea1.AxisX.LabelAutoFitMaxFontSize = 8;
			chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
			chartArea1.AxisX.Minimum = -0.5D;
			chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
			chartArea1.AxisX.Title = "Class";
			chartArea1.AxisY.LabelAutoFitMaxFontSize = 8;
			chartArea1.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
			chartArea1.AxisY.Maximum = 1D;
			chartArea1.AxisY.Minimum = 0D;
			chartArea1.Name = "ChartArea1";
			this.accuracyColumnChart.ChartAreas.Add(chartArea1);
			legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
			legend1.Name = "Legend1";
			this.accuracyColumnChart.Legends.Add(legend1);
			this.accuracyColumnChart.Location = new System.Drawing.Point(12, 276);
			this.accuracyColumnChart.Name = "accuracyColumnChart";
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.Name = "Precision";
			series2.ChartArea = "ChartArea1";
			series2.Legend = "Legend1";
			series2.Name = "Recall";
			series3.ChartArea = "ChartArea1";
			series3.Legend = "Legend1";
			series3.Name = "Specificity";
			this.accuracyColumnChart.Series.Add(series1);
			this.accuracyColumnChart.Series.Add(series2);
			this.accuracyColumnChart.Series.Add(series3);
			this.accuracyColumnChart.Size = new System.Drawing.Size(392, 209);
			this.accuracyColumnChart.TabIndex = 5;
			title1.Name = "Classification Metrics";
			title1.Text = "Classification Metrics";
			title1.Visible = false;
			this.accuracyColumnChart.Titles.Add(title1);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.trainToolStripMenuItem,
            this.viewToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(979, 24);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.newToolStripMenuItem.Text = "&New...";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.openToolStripMenuItem.Text = "&Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(109, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.saveToolStripMenuItem.Text = "&Save...";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(109, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// trainToolStripMenuItem
			// 
			this.trainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startUnsupervisedToolStripMenuItem,
            this.startSupervisedToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.toolStripMenuItem3,
            this.testToolStripMenuItem,
            this.visualTesterToolStripMenuItem,
            this.toolStripMenuItem4,
            this.experimentBatchToolStripMenuItem});
			this.trainToolStripMenuItem.Name = "trainToolStripMenuItem";
			this.trainToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			this.trainToolStripMenuItem.Text = "&Train";
			// 
			// startUnsupervisedToolStripMenuItem
			// 
			this.startUnsupervisedToolStripMenuItem.Name = "startUnsupervisedToolStripMenuItem";
			this.startUnsupervisedToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.startUnsupervisedToolStripMenuItem.Text = "Start &unsupervised";
			this.startUnsupervisedToolStripMenuItem.Click += new System.EventHandler(this.startUnsupervisedToolStripMenuItem_Click);
			// 
			// startSupervisedToolStripMenuItem
			// 
			this.startSupervisedToolStripMenuItem.Name = "startSupervisedToolStripMenuItem";
			this.startSupervisedToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.startSupervisedToolStripMenuItem.Text = "Start &supervised";
			this.startSupervisedToolStripMenuItem.Click += new System.EventHandler(this.startSupervisedToolStripMenuItem_Click);
			// 
			// stopToolStripMenuItem
			// 
			this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
			this.stopToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.stopToolStripMenuItem.Text = "Sto&p";
			this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(172, 6);
			// 
			// testToolStripMenuItem
			// 
			this.testToolStripMenuItem.Name = "testToolStripMenuItem";
			this.testToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.testToolStripMenuItem.Text = "&Test";
			this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
			// 
			// visualTesterToolStripMenuItem
			// 
			this.visualTesterToolStripMenuItem.Enabled = false;
			this.visualTesterToolStripMenuItem.Name = "visualTesterToolStripMenuItem";
			this.visualTesterToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.visualTesterToolStripMenuItem.Text = "Visual tester...";
			this.visualTesterToolStripMenuItem.Click += new System.EventHandler(this.visualTesterToolStripMenuItem_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(172, 6);
			// 
			// experimentBatchToolStripMenuItem
			// 
			this.experimentBatchToolStripMenuItem.Name = "experimentBatchToolStripMenuItem";
			this.experimentBatchToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
			this.experimentBatchToolStripMenuItem.Text = "&Batch experiment...";
			this.experimentBatchToolStripMenuItem.Click += new System.EventHandler(this.experimentBatchToolStripMenuItem_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trainingDataToolStripMenuItem,
            this.testingDataToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// trainingDataToolStripMenuItem
			// 
			this.trainingDataToolStripMenuItem.Name = "trainingDataToolStripMenuItem";
			this.trainingDataToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.trainingDataToolStripMenuItem.Text = "Training data";
			this.trainingDataToolStripMenuItem.Click += new System.EventHandler(this.trainingDataToolStripMenuItem_Click);
			// 
			// testingDataToolStripMenuItem
			// 
			this.testingDataToolStripMenuItem.Name = "testingDataToolStripMenuItem";
			this.testingDataToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.testingDataToolStripMenuItem.Text = "Testing data";
			this.testingDataToolStripMenuItem.Click += new System.EventHandler(this.testingDataToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(979, 608);
			this.Controls.Add(this.accuracyColumnChart);
			this.Controls.Add(this.trainingGroupBox);
			this.Controls.Add(this.networkGroupBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.containerLayoutPanel);
			this.Controls.Add(this.infoTextBox);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "My MSc Project";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.networkGroupBox.ResumeLayout(false);
			this.networkGroupBox.PerformLayout();
			this.trainingGroupBox.ResumeLayout(false);
			this.trainingGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.learningProbabilityUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.epochsUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.accuracyColumnChart)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox infoTextBox;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.FlowLayoutPanel containerLayoutPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button createNetworkButton;
		private System.Windows.Forms.Button unsupervisedTrainingButton;
		private System.Windows.Forms.Button supervisedTrainingButton;
		private System.Windows.Forms.Button saveNetworkButton;
		private System.Windows.Forms.GroupBox networkGroupBox;
		private System.Windows.Forms.TextBox networkInfoTextBox;
		private System.Windows.Forms.Button loadNetworkButton;
		private System.Windows.Forms.GroupBox trainingGroupBox;
		private System.Windows.Forms.Button testButton;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ProgressBar trainingProgressBar;
		private System.Windows.Forms.DataVisualization.Charting.Chart accuracyColumnChart;
		private System.Windows.Forms.Button dreamButton;
		private System.Windows.Forms.NumericUpDown epochsUpDown;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem trainToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem startUnsupervisedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem startSupervisedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem experimentBatchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem trainingDataToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem testingDataToolStripMenuItem;
		private System.Windows.Forms.NumericUpDown learningProbabilityUpDown;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ToolStripMenuItem visualTesterToolStripMenuItem;
	}
}

