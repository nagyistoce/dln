namespace Eft.BioNN.UI.WinForms
{
	partial class GenerateImagesForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.layerUpDown = new System.Windows.Forms.NumericUpDown();
			this.startButton = new System.Windows.Forms.Button();
			this.stopButton = new System.Windows.Forms.Button();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.bufferSizeGroupBox = new System.Windows.Forms.GroupBox();
			this.radioButton6 = new System.Windows.Forms.RadioButton();
			this.radioButton5 = new System.Windows.Forms.RadioButton();
			this.radioButton4 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.speedGroupBox = new System.Windows.Forms.GroupBox();
			this.radioButton9 = new System.Windows.Forms.RadioButton();
			this.radioButton10 = new System.Windows.Forms.RadioButton();
			this.radioButton11 = new System.Windows.Forms.RadioButton();
			this.radioButton12 = new System.Windows.Forms.RadioButton();
			this.samplesGroupBox = new System.Windows.Forms.GroupBox();
			this.radioButton8 = new System.Windows.Forms.RadioButton();
			this.radioButton13 = new System.Windows.Forms.RadioButton();
			this.radioButton14 = new System.Windows.Forms.RadioButton();
			this.radioButton15 = new System.Windows.Forms.RadioButton();
			this.radioButton16 = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.layerUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.bufferSizeGroupBox.SuspendLayout();
			this.speedGroupBox.SuspendLayout();
			this.samplesGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Layer:";
			// 
			// layerUpDown
			// 
			this.layerUpDown.Location = new System.Drawing.Point(54, 7);
			this.layerUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.layerUpDown.Name = "layerUpDown";
			this.layerUpDown.Size = new System.Drawing.Size(44, 20);
			this.layerUpDown.TabIndex = 1;
			this.layerUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(104, 4);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(75, 23);
			this.startButton.TabIndex = 2;
			this.startButton.Text = "Start";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// stopButton
			// 
			this.stopButton.Enabled = false;
			this.stopButton.Location = new System.Drawing.Point(185, 4);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(75, 23);
			this.stopButton.TabIndex = 3;
			this.stopButton.Text = "Stop";
			this.stopButton.UseVisualStyleBackColor = true;
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(15, 183);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(252, 252);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox.TabIndex = 4;
			this.pictureBox.TabStop = false;
			// 
			// bufferSizeGroupBox
			// 
			this.bufferSizeGroupBox.Controls.Add(this.radioButton6);
			this.bufferSizeGroupBox.Controls.Add(this.radioButton5);
			this.bufferSizeGroupBox.Controls.Add(this.radioButton4);
			this.bufferSizeGroupBox.Controls.Add(this.radioButton3);
			this.bufferSizeGroupBox.Controls.Add(this.radioButton2);
			this.bufferSizeGroupBox.Controls.Add(this.radioButton1);
			this.bufferSizeGroupBox.Location = new System.Drawing.Point(15, 83);
			this.bufferSizeGroupBox.Name = "bufferSizeGroupBox";
			this.bufferSizeGroupBox.Size = new System.Drawing.Size(252, 44);
			this.bufferSizeGroupBox.TabIndex = 5;
			this.bufferSizeGroupBox.TabStop = false;
			this.bufferSizeGroupBox.Text = "Buffer size";
			// 
			// radioButton6
			// 
			this.radioButton6.AutoSize = true;
			this.radioButton6.Location = new System.Drawing.Point(197, 19);
			this.radioButton6.Name = "radioButton6";
			this.radioButton6.Size = new System.Drawing.Size(37, 17);
			this.radioButton6.TabIndex = 5;
			this.radioButton6.Text = "32";
			this.radioButton6.UseVisualStyleBackColor = true;
			// 
			// radioButton5
			// 
			this.radioButton5.AutoSize = true;
			this.radioButton5.Location = new System.Drawing.Point(154, 19);
			this.radioButton5.Name = "radioButton5";
			this.radioButton5.Size = new System.Drawing.Size(37, 17);
			this.radioButton5.TabIndex = 4;
			this.radioButton5.Text = "16";
			this.radioButton5.UseVisualStyleBackColor = true;
			// 
			// radioButton4
			// 
			this.radioButton4.AutoSize = true;
			this.radioButton4.Location = new System.Drawing.Point(117, 19);
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.Size = new System.Drawing.Size(31, 17);
			this.radioButton4.TabIndex = 3;
			this.radioButton4.Text = "8";
			this.radioButton4.UseVisualStyleBackColor = true;
			// 
			// radioButton3
			// 
			this.radioButton3.AutoSize = true;
			this.radioButton3.Location = new System.Drawing.Point(80, 19);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(31, 17);
			this.radioButton3.TabIndex = 2;
			this.radioButton3.Text = "4";
			this.radioButton3.UseVisualStyleBackColor = true;
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(43, 19);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(31, 17);
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "2";
			this.radioButton2.UseVisualStyleBackColor = true;
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(6, 19);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(31, 17);
			this.radioButton1.TabIndex = 0;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "1";
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// speedGroupBox
			// 
			this.speedGroupBox.Controls.Add(this.radioButton9);
			this.speedGroupBox.Controls.Add(this.radioButton10);
			this.speedGroupBox.Controls.Add(this.radioButton11);
			this.speedGroupBox.Controls.Add(this.radioButton12);
			this.speedGroupBox.Location = new System.Drawing.Point(15, 133);
			this.speedGroupBox.Name = "speedGroupBox";
			this.speedGroupBox.Size = new System.Drawing.Size(252, 44);
			this.speedGroupBox.TabIndex = 6;
			this.speedGroupBox.TabStop = false;
			this.speedGroupBox.Text = "Speed";
			// 
			// radioButton9
			// 
			this.radioButton9.AutoSize = true;
			this.radioButton9.Location = new System.Drawing.Point(179, 19);
			this.radioButton9.Name = "radioButton9";
			this.radioButton9.Size = new System.Drawing.Size(57, 17);
			this.radioButton9.TabIndex = 3;
			this.radioButton9.Tag = "0";
			this.radioButton9.Text = "Insane";
			this.radioButton9.UseVisualStyleBackColor = true;
			// 
			// radioButton10
			// 
			this.radioButton10.AutoSize = true;
			this.radioButton10.Location = new System.Drawing.Point(128, 19);
			this.radioButton10.Name = "radioButton10";
			this.radioButton10.Size = new System.Drawing.Size(45, 17);
			this.radioButton10.TabIndex = 2;
			this.radioButton10.Tag = "30";
			this.radioButton10.Text = "Fast";
			this.radioButton10.UseVisualStyleBackColor = true;
			// 
			// radioButton11
			// 
			this.radioButton11.AutoSize = true;
			this.radioButton11.Checked = true;
			this.radioButton11.Location = new System.Drawing.Point(60, 19);
			this.radioButton11.Name = "radioButton11";
			this.radioButton11.Size = new System.Drawing.Size(62, 17);
			this.radioButton11.TabIndex = 1;
			this.radioButton11.TabStop = true;
			this.radioButton11.Tag = "100";
			this.radioButton11.Text = "Medium";
			this.radioButton11.UseVisualStyleBackColor = true;
			// 
			// radioButton12
			// 
			this.radioButton12.AutoSize = true;
			this.radioButton12.Location = new System.Drawing.Point(6, 19);
			this.radioButton12.Name = "radioButton12";
			this.radioButton12.Size = new System.Drawing.Size(48, 17);
			this.radioButton12.TabIndex = 0;
			this.radioButton12.Tag = "300";
			this.radioButton12.Text = "Slow";
			this.radioButton12.UseVisualStyleBackColor = true;
			// 
			// samplesGroupBox
			// 
			this.samplesGroupBox.Controls.Add(this.radioButton8);
			this.samplesGroupBox.Controls.Add(this.radioButton13);
			this.samplesGroupBox.Controls.Add(this.radioButton14);
			this.samplesGroupBox.Controls.Add(this.radioButton15);
			this.samplesGroupBox.Controls.Add(this.radioButton16);
			this.samplesGroupBox.Location = new System.Drawing.Point(15, 33);
			this.samplesGroupBox.Name = "samplesGroupBox";
			this.samplesGroupBox.Size = new System.Drawing.Size(252, 44);
			this.samplesGroupBox.TabIndex = 4;
			this.samplesGroupBox.TabStop = false;
			this.samplesGroupBox.Text = "Samples";
			// 
			// radioButton8
			// 
			this.radioButton8.AutoSize = true;
			this.radioButton8.Location = new System.Drawing.Point(166, 19);
			this.radioButton8.Name = "radioButton8";
			this.radioButton8.Size = new System.Drawing.Size(43, 17);
			this.radioButton8.TabIndex = 4;
			this.radioButton8.Text = "100";
			this.radioButton8.UseVisualStyleBackColor = true;
			// 
			// radioButton13
			// 
			this.radioButton13.AutoSize = true;
			this.radioButton13.Location = new System.Drawing.Point(123, 19);
			this.radioButton13.Name = "radioButton13";
			this.radioButton13.Size = new System.Drawing.Size(37, 17);
			this.radioButton13.TabIndex = 3;
			this.radioButton13.Text = "30";
			this.radioButton13.UseVisualStyleBackColor = true;
			// 
			// radioButton14
			// 
			this.radioButton14.AutoSize = true;
			this.radioButton14.Checked = true;
			this.radioButton14.Location = new System.Drawing.Point(80, 19);
			this.radioButton14.Name = "radioButton14";
			this.radioButton14.Size = new System.Drawing.Size(37, 17);
			this.radioButton14.TabIndex = 2;
			this.radioButton14.TabStop = true;
			this.radioButton14.Text = "10";
			this.radioButton14.UseVisualStyleBackColor = true;
			// 
			// radioButton15
			// 
			this.radioButton15.AutoSize = true;
			this.radioButton15.Location = new System.Drawing.Point(43, 19);
			this.radioButton15.Name = "radioButton15";
			this.radioButton15.Size = new System.Drawing.Size(31, 17);
			this.radioButton15.TabIndex = 1;
			this.radioButton15.Text = "3";
			this.radioButton15.UseVisualStyleBackColor = true;
			// 
			// radioButton16
			// 
			this.radioButton16.AutoSize = true;
			this.radioButton16.Location = new System.Drawing.Point(6, 19);
			this.radioButton16.Name = "radioButton16";
			this.radioButton16.Size = new System.Drawing.Size(31, 17);
			this.radioButton16.TabIndex = 0;
			this.radioButton16.Text = "1";
			this.radioButton16.UseVisualStyleBackColor = true;
			// 
			// GenerateImagesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(283, 444);
			this.Controls.Add(this.samplesGroupBox);
			this.Controls.Add(this.speedGroupBox);
			this.Controls.Add(this.bufferSizeGroupBox);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.stopButton);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.layerUpDown);
			this.Controls.Add(this.label1);
			this.Name = "GenerateImagesForm";
			this.Text = "Generate images";
			((System.ComponentModel.ISupportInitialize)(this.layerUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.bufferSizeGroupBox.ResumeLayout(false);
			this.bufferSizeGroupBox.PerformLayout();
			this.speedGroupBox.ResumeLayout(false);
			this.speedGroupBox.PerformLayout();
			this.samplesGroupBox.ResumeLayout(false);
			this.samplesGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown layerUpDown;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.GroupBox bufferSizeGroupBox;
		private System.Windows.Forms.RadioButton radioButton5;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton6;
		private System.Windows.Forms.GroupBox speedGroupBox;
		private System.Windows.Forms.RadioButton radioButton9;
		private System.Windows.Forms.RadioButton radioButton10;
		private System.Windows.Forms.RadioButton radioButton11;
		private System.Windows.Forms.RadioButton radioButton12;
		private System.Windows.Forms.GroupBox samplesGroupBox;
		private System.Windows.Forms.RadioButton radioButton8;
		private System.Windows.Forms.RadioButton radioButton13;
		private System.Windows.Forms.RadioButton radioButton14;
		private System.Windows.Forms.RadioButton radioButton15;
		private System.Windows.Forms.RadioButton radioButton16;
	}
}