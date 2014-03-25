namespace Eft.BioNN.Generator.UI.WinForms
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
			this.shapeViewPanel = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.horizontalLineRadioButton = new System.Windows.Forms.RadioButton();
			this.verticalLineRadioButton = new System.Windows.Forms.RadioButton();
			this.circleBottomHalfRadioButton = new System.Windows.Forms.RadioButton();
			this.circleTopHalfRadioButton = new System.Windows.Forms.RadioButton();
			this.circleRightHalfRadioButton = new System.Windows.Forms.RadioButton();
			this.circleLeftHalfRadioButton = new System.Windows.Forms.RadioButton();
			this.diamondRadioButton = new System.Windows.Forms.RadioButton();
			this.triangleRadioButton = new System.Windows.Forms.RadioButton();
			this.squareRadioButton = new System.Windows.Forms.RadioButton();
			this.circleRadioButton = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.sizeUpDown = new System.Windows.Forms.NumericUpDown();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.label2 = new System.Windows.Forms.Label();
			this.folderTextBox = new System.Windows.Forms.TextBox();
			this.selectFolderButton = new System.Windows.Forms.Button();
			this.generateImagesButton = new System.Windows.Forms.Button();
			this.animateButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sizeUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// shapeViewPanel
			// 
			this.shapeViewPanel.BackColor = System.Drawing.Color.Black;
			this.shapeViewPanel.Location = new System.Drawing.Point(218, 111);
			this.shapeViewPanel.Name = "shapeViewPanel";
			this.shapeViewPanel.Size = new System.Drawing.Size(28, 28);
			this.shapeViewPanel.TabIndex = 6;
			this.shapeViewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.shapeViewPanel_Paint);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.horizontalLineRadioButton);
			this.groupBox1.Controls.Add(this.verticalLineRadioButton);
			this.groupBox1.Controls.Add(this.circleBottomHalfRadioButton);
			this.groupBox1.Controls.Add(this.circleTopHalfRadioButton);
			this.groupBox1.Controls.Add(this.circleRightHalfRadioButton);
			this.groupBox1.Controls.Add(this.circleLeftHalfRadioButton);
			this.groupBox1.Controls.Add(this.diamondRadioButton);
			this.groupBox1.Controls.Add(this.triangleRadioButton);
			this.groupBox1.Controls.Add(this.squareRadioButton);
			this.groupBox1.Controls.Add(this.circleRadioButton);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 257);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Shape";
			// 
			// horizontalLineRadioButton
			// 
			this.horizontalLineRadioButton.AutoSize = true;
			this.horizontalLineRadioButton.Location = new System.Drawing.Point(6, 225);
			this.horizontalLineRadioButton.Name = "horizontalLineRadioButton";
			this.horizontalLineRadioButton.Size = new System.Drawing.Size(91, 17);
			this.horizontalLineRadioButton.TabIndex = 9;
			this.horizontalLineRadioButton.Text = "Horizontal line";
			this.horizontalLineRadioButton.UseVisualStyleBackColor = true;
			this.horizontalLineRadioButton.CheckedChanged += new System.EventHandler(this.horizontalLineRadioButton_CheckedChanged);
			// 
			// verticalLineRadioButton
			// 
			this.verticalLineRadioButton.AutoSize = true;
			this.verticalLineRadioButton.Location = new System.Drawing.Point(6, 202);
			this.verticalLineRadioButton.Name = "verticalLineRadioButton";
			this.verticalLineRadioButton.Size = new System.Drawing.Size(79, 17);
			this.verticalLineRadioButton.TabIndex = 8;
			this.verticalLineRadioButton.Text = "Vertical line";
			this.verticalLineRadioButton.UseVisualStyleBackColor = true;
			this.verticalLineRadioButton.CheckedChanged += new System.EventHandler(this.verticalLineRadioButton_CheckedChanged);
			// 
			// circleBottomHalfRadioButton
			// 
			this.circleBottomHalfRadioButton.AutoSize = true;
			this.circleBottomHalfRadioButton.Location = new System.Drawing.Point(6, 110);
			this.circleBottomHalfRadioButton.Name = "circleBottomHalfRadioButton";
			this.circleBottomHalfRadioButton.Size = new System.Drawing.Size(112, 17);
			this.circleBottomHalfRadioButton.TabIndex = 4;
			this.circleBottomHalfRadioButton.Text = "Circle (bottom half)";
			this.circleBottomHalfRadioButton.UseVisualStyleBackColor = true;
			this.circleBottomHalfRadioButton.CheckedChanged += new System.EventHandler(this.circleBottomHalfRadioButton_CheckedChanged);
			// 
			// circleTopHalfRadioButton
			// 
			this.circleTopHalfRadioButton.AutoSize = true;
			this.circleTopHalfRadioButton.Location = new System.Drawing.Point(6, 87);
			this.circleTopHalfRadioButton.Name = "circleTopHalfRadioButton";
			this.circleTopHalfRadioButton.Size = new System.Drawing.Size(95, 17);
			this.circleTopHalfRadioButton.TabIndex = 3;
			this.circleTopHalfRadioButton.Text = "Circle (top half)";
			this.circleTopHalfRadioButton.UseVisualStyleBackColor = true;
			this.circleTopHalfRadioButton.CheckedChanged += new System.EventHandler(this.circleTopHalfRadioButton_CheckedChanged);
			// 
			// circleRightHalfRadioButton
			// 
			this.circleRightHalfRadioButton.AutoSize = true;
			this.circleRightHalfRadioButton.Location = new System.Drawing.Point(6, 64);
			this.circleRightHalfRadioButton.Name = "circleRightHalfRadioButton";
			this.circleRightHalfRadioButton.Size = new System.Drawing.Size(100, 17);
			this.circleRightHalfRadioButton.TabIndex = 2;
			this.circleRightHalfRadioButton.Text = "Circle (right half)";
			this.circleRightHalfRadioButton.UseVisualStyleBackColor = true;
			this.circleRightHalfRadioButton.CheckedChanged += new System.EventHandler(this.circleRightHalfRadioButton_CheckedChanged);
			// 
			// circleLeftHalfRadioButton
			// 
			this.circleLeftHalfRadioButton.AutoSize = true;
			this.circleLeftHalfRadioButton.Location = new System.Drawing.Point(6, 42);
			this.circleLeftHalfRadioButton.Name = "circleLeftHalfRadioButton";
			this.circleLeftHalfRadioButton.Size = new System.Drawing.Size(94, 17);
			this.circleLeftHalfRadioButton.TabIndex = 1;
			this.circleLeftHalfRadioButton.Text = "Circle (left half)";
			this.circleLeftHalfRadioButton.UseVisualStyleBackColor = true;
			this.circleLeftHalfRadioButton.CheckedChanged += new System.EventHandler(this.circleLeftHalfRadioButton_CheckedChanged);
			// 
			// diamondRadioButton
			// 
			this.diamondRadioButton.AutoSize = true;
			this.diamondRadioButton.Location = new System.Drawing.Point(6, 179);
			this.diamondRadioButton.Name = "diamondRadioButton";
			this.diamondRadioButton.Size = new System.Drawing.Size(67, 17);
			this.diamondRadioButton.TabIndex = 7;
			this.diamondRadioButton.Text = "Diamond";
			this.diamondRadioButton.UseVisualStyleBackColor = true;
			this.diamondRadioButton.CheckedChanged += new System.EventHandler(this.diamondRadioButton_CheckedChanged);
			// 
			// triangleRadioButton
			// 
			this.triangleRadioButton.AutoSize = true;
			this.triangleRadioButton.Location = new System.Drawing.Point(6, 156);
			this.triangleRadioButton.Name = "triangleRadioButton";
			this.triangleRadioButton.Size = new System.Drawing.Size(63, 17);
			this.triangleRadioButton.TabIndex = 6;
			this.triangleRadioButton.Text = "Triangle";
			this.triangleRadioButton.UseVisualStyleBackColor = true;
			this.triangleRadioButton.CheckedChanged += new System.EventHandler(this.triangleRadioButton_CheckedChanged);
			// 
			// squareRadioButton
			// 
			this.squareRadioButton.AutoSize = true;
			this.squareRadioButton.Location = new System.Drawing.Point(6, 133);
			this.squareRadioButton.Name = "squareRadioButton";
			this.squareRadioButton.Size = new System.Drawing.Size(59, 17);
			this.squareRadioButton.TabIndex = 5;
			this.squareRadioButton.Text = "Square";
			this.squareRadioButton.UseVisualStyleBackColor = true;
			this.squareRadioButton.CheckedChanged += new System.EventHandler(this.squareRadioButton_CheckedChanged);
			// 
			// circleRadioButton
			// 
			this.circleRadioButton.AutoSize = true;
			this.circleRadioButton.Checked = true;
			this.circleRadioButton.Location = new System.Drawing.Point(6, 19);
			this.circleRadioButton.Name = "circleRadioButton";
			this.circleRadioButton.Size = new System.Drawing.Size(51, 17);
			this.circleRadioButton.TabIndex = 0;
			this.circleRadioButton.TabStop = true;
			this.circleRadioButton.Text = "Circle";
			this.circleRadioButton.UseVisualStyleBackColor = true;
			this.circleRadioButton.CheckedChanged += new System.EventHandler(this.circleRadioButton_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(218, 87);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Image size:";
			// 
			// sizeUpDown
			// 
			this.sizeUpDown.Location = new System.Drawing.Point(284, 85);
			this.sizeUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
			this.sizeUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.sizeUpDown.Name = "sizeUpDown";
			this.sizeUpDown.Size = new System.Drawing.Size(67, 20);
			this.sizeUpDown.TabIndex = 5;
			this.sizeUpDown.Value = new decimal(new int[] {
            28,
            0,
            0,
            0});
			this.sizeUpDown.ValueChanged += new System.EventHandler(this.sizeUpDown_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(218, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(83, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Save images to:";
			// 
			// folderTextBox
			// 
			this.folderTextBox.Location = new System.Drawing.Point(218, 31);
			this.folderTextBox.Name = "folderTextBox";
			this.folderTextBox.ReadOnly = true;
			this.folderTextBox.Size = new System.Drawing.Size(350, 20);
			this.folderTextBox.TabIndex = 2;
			// 
			// selectFolderButton
			// 
			this.selectFolderButton.Location = new System.Drawing.Point(218, 57);
			this.selectFolderButton.Name = "selectFolderButton";
			this.selectFolderButton.Size = new System.Drawing.Size(109, 23);
			this.selectFolderButton.TabIndex = 3;
			this.selectFolderButton.Text = "Pick folder...";
			this.selectFolderButton.UseVisualStyleBackColor = true;
			this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click);
			// 
			// generateImagesButton
			// 
			this.generateImagesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.generateImagesButton.Location = new System.Drawing.Point(459, 246);
			this.generateImagesButton.Name = "generateImagesButton";
			this.generateImagesButton.Size = new System.Drawing.Size(109, 23);
			this.generateImagesButton.TabIndex = 7;
			this.generateImagesButton.Text = "Generate images";
			this.generateImagesButton.UseVisualStyleBackColor = true;
			this.generateImagesButton.Click += new System.EventHandler(this.generateImagesButton_Click);
			// 
			// animateButton
			// 
			this.animateButton.Location = new System.Drawing.Point(459, 208);
			this.animateButton.Name = "animateButton";
			this.animateButton.Size = new System.Drawing.Size(75, 23);
			this.animateButton.TabIndex = 8;
			this.animateButton.Text = "Animate";
			this.animateButton.UseVisualStyleBackColor = true;
			this.animateButton.Click += new System.EventHandler(this.animateButton_Click);
			// 
			// MainForm
			// 
			this.AcceptButton = this.generateImagesButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(580, 278);
			this.Controls.Add(this.animateButton);
			this.Controls.Add(this.generateImagesButton);
			this.Controls.Add(this.selectFolderButton);
			this.Controls.Add(this.folderTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.sizeUpDown);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.shapeViewPanel);
			this.Name = "MainForm";
			this.Text = "Simple shape generator";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.sizeUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel shapeViewPanel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton squareRadioButton;
		private System.Windows.Forms.RadioButton circleRadioButton;
		private System.Windows.Forms.RadioButton triangleRadioButton;
		private System.Windows.Forms.RadioButton diamondRadioButton;
		private System.Windows.Forms.RadioButton circleLeftHalfRadioButton;
		private System.Windows.Forms.RadioButton circleBottomHalfRadioButton;
		private System.Windows.Forms.RadioButton circleTopHalfRadioButton;
		private System.Windows.Forms.RadioButton circleRightHalfRadioButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown sizeUpDown;
		private System.Windows.Forms.RadioButton horizontalLineRadioButton;
		private System.Windows.Forms.RadioButton verticalLineRadioButton;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox folderTextBox;
		private System.Windows.Forms.Button selectFolderButton;
		private System.Windows.Forms.Button generateImagesButton;
		private System.Windows.Forms.Button animateButton;
	}
}

