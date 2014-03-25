namespace Eft.BioNN.UI.WinForms
{
	partial class ClassifierForm
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
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.prevButton = new System.Windows.Forms.Button();
			this.nextButton = new System.Windows.Forms.Button();
			this.guessLabel = new System.Windows.Forms.Label();
			this.correctLabel = new System.Windows.Forms.Label();
			this.wrongLabel = new System.Windows.Forms.Label();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(80, 12);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(28, 28);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			// 
			// prevButton
			// 
			this.prevButton.Enabled = false;
			this.prevButton.Location = new System.Drawing.Point(12, 12);
			this.prevButton.Name = "prevButton";
			this.prevButton.Size = new System.Drawing.Size(62, 28);
			this.prevButton.TabIndex = 1;
			this.prevButton.Text = "Previous";
			this.prevButton.UseVisualStyleBackColor = true;
			this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
			// 
			// nextButton
			// 
			this.nextButton.Location = new System.Drawing.Point(114, 12);
			this.nextButton.Name = "nextButton";
			this.nextButton.Size = new System.Drawing.Size(62, 28);
			this.nextButton.TabIndex = 2;
			this.nextButton.Text = "Next";
			this.nextButton.UseVisualStyleBackColor = true;
			this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
			// 
			// guessLabel
			// 
			this.guessLabel.AutoSize = true;
			this.guessLabel.Location = new System.Drawing.Point(12, 43);
			this.guessLabel.Name = "guessLabel";
			this.guessLabel.Size = new System.Drawing.Size(40, 13);
			this.guessLabel.TabIndex = 3;
			this.guessLabel.Text = "Guess:";
			// 
			// correctLabel
			// 
			this.correctLabel.AutoSize = true;
			this.correctLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.correctLabel.ForeColor = System.Drawing.Color.Green;
			this.correctLabel.Location = new System.Drawing.Point(182, 12);
			this.correctLabel.Name = "correctLabel";
			this.correctLabel.Size = new System.Drawing.Size(84, 24);
			this.correctLabel.TabIndex = 4;
			this.correctLabel.Text = "Correct!";
			// 
			// wrongLabel
			// 
			this.wrongLabel.AutoSize = true;
			this.wrongLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.wrongLabel.ForeColor = System.Drawing.Color.Red;
			this.wrongLabel.Location = new System.Drawing.Point(182, 12);
			this.wrongLabel.Name = "wrongLabel";
			this.wrongLabel.Size = new System.Drawing.Size(78, 24);
			this.wrongLabel.TabIndex = 5;
			this.wrongLabel.Text = "Wrong!";
			// 
			// dataGridView
			// 
			this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Location = new System.Drawing.Point(12, 59);
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.Size = new System.Drawing.Size(798, 150);
			this.dataGridView.TabIndex = 6;
			// 
			// ClassifierForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(822, 220);
			this.Controls.Add(this.dataGridView);
			this.Controls.Add(this.wrongLabel);
			this.Controls.Add(this.correctLabel);
			this.Controls.Add(this.guessLabel);
			this.Controls.Add(this.nextButton);
			this.Controls.Add(this.prevButton);
			this.Controls.Add(this.pictureBox);
			this.Name = "ClassifierForm";
			this.Text = "ClassifierForm";
			this.Shown += new System.EventHandler(this.ClassifierForm_Shown);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Button prevButton;
		private System.Windows.Forms.Button nextButton;
		private System.Windows.Forms.Label guessLabel;
		private System.Windows.Forms.Label correctLabel;
		private System.Windows.Forms.Label wrongLabel;
		private System.Windows.Forms.DataGridView dataGridView;
	}
}