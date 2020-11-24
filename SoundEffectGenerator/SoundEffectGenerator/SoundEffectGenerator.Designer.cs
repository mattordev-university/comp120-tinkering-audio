namespace SoundEffectGenerator
{
    partial class EffectGenerator
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
			this.label1 = new System.Windows.Forms.Label();
			this.SaveSound = new System.Windows.Forms.Button();
			this.PhaseInverterEffect = new System.Windows.Forms.CheckBox();
			this.NormaliseSampleEffect = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.AmplitudeScaleEffect = new System.Windows.Forms.CheckBox();
			this.Generate = new System.Windows.Forms.Button();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.ReverseSampleEffect = new System.Windows.Forms.CheckBox();
			this.GenerateNotes = new System.Windows.Forms.Button();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.LoadSound = new System.Windows.Forms.Button();
			this.FileDirectoryTextBox = new System.Windows.Forms.TextBox();
			this.FileNameTextBox = new System.Windows.Forms.TextBox();
			this.playSound = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(372, 39);
			this.label1.TabIndex = 0;
			this.label1.Text = "Sound Effect Generator";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// SaveSound
			// 
			this.SaveSound.Location = new System.Drawing.Point(19, 386);
			this.SaveSound.Name = "SaveSound";
			this.SaveSound.Size = new System.Drawing.Size(75, 52);
			this.SaveSound.TabIndex = 2;
			this.SaveSound.Text = "Save Sound";
			this.SaveSound.UseVisualStyleBackColor = true;
			this.SaveSound.Click += new System.EventHandler(this.SaveSound_Click);
			// 
			// PhaseInverterEffect
			// 
			this.PhaseInverterEffect.AutoSize = true;
			this.PhaseInverterEffect.Location = new System.Drawing.Point(626, 61);
			this.PhaseInverterEffect.Name = "PhaseInverterEffect";
			this.PhaseInverterEffect.Size = new System.Drawing.Size(92, 17);
			this.PhaseInverterEffect.TabIndex = 4;
			this.PhaseInverterEffect.Text = "PhaseInverter";
			this.PhaseInverterEffect.UseVisualStyleBackColor = true;
			// 
			// NormaliseSampleEffect
			// 
			this.NormaliseSampleEffect.AutoSize = true;
			this.NormaliseSampleEffect.Location = new System.Drawing.Point(626, 84);
			this.NormaliseSampleEffect.Name = "NormaliseSampleEffect";
			this.NormaliseSampleEffect.Size = new System.Drawing.Size(107, 17);
			this.NormaliseSampleEffect.TabIndex = 5;
			this.NormaliseSampleEffect.Text = "NormaliseSample";
			this.NormaliseSampleEffect.UseVisualStyleBackColor = true;
			// 
			// checkBox5
			// 
			this.checkBox5.AutoSize = true;
			this.checkBox5.Location = new System.Drawing.Point(626, 153);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(63, 17);
			this.checkBox5.TabIndex = 8;
			this.checkBox5.Text = "Effect 5";
			this.checkBox5.UseVisualStyleBackColor = true;
			// 
			// AmplitudeScaleEffect
			// 
			this.AmplitudeScaleEffect.AutoSize = true;
			this.AmplitudeScaleEffect.Location = new System.Drawing.Point(626, 130);
			this.AmplitudeScaleEffect.Name = "AmplitudeScaleEffect";
			this.AmplitudeScaleEffect.Size = new System.Drawing.Size(99, 17);
			this.AmplitudeScaleEffect.TabIndex = 7;
			this.AmplitudeScaleEffect.Text = "AmplitudeScale";
			this.AmplitudeScaleEffect.UseVisualStyleBackColor = true;
			// 
			// Generate
			// 
			this.Generate.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
			this.Generate.Location = new System.Drawing.Point(181, 386);
			this.Generate.Name = "Generate";
			this.Generate.Size = new System.Drawing.Size(362, 52);
			this.Generate.TabIndex = 10;
			this.Generate.Text = "Generate Effects";
			this.Generate.UseVisualStyleBackColor = true;
			this.Generate.Click += new System.EventHandler(this.Generate_Click);
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.Location = new System.Drawing.Point(626, 176);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(63, 17);
			this.checkBox4.TabIndex = 9;
			this.checkBox4.Text = "Effect 6";
			this.checkBox4.UseVisualStyleBackColor = true;
			// 
			// ReverseSampleEffect
			// 
			this.ReverseSampleEffect.AutoSize = true;
			this.ReverseSampleEffect.Location = new System.Drawing.Point(626, 107);
			this.ReverseSampleEffect.Name = "ReverseSampleEffect";
			this.ReverseSampleEffect.Size = new System.Drawing.Size(101, 17);
			this.ReverseSampleEffect.TabIndex = 6;
			this.ReverseSampleEffect.Text = "ReverseSample";
			this.ReverseSampleEffect.UseVisualStyleBackColor = true;
			// 
			// GenerateNotes
			// 
			this.GenerateNotes.Location = new System.Drawing.Point(549, 386);
			this.GenerateNotes.Name = "GenerateNotes";
			this.GenerateNotes.Size = new System.Drawing.Size(140, 52);
			this.GenerateNotes.TabIndex = 11;
			this.GenerateNotes.Text = "Generate Notes";
			this.GenerateNotes.UseVisualStyleBackColor = true;
			this.GenerateNotes.Click += new System.EventHandler(this.GenerateNotes_Click);
			// 
			// chart1
			// 
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			this.chart1.Location = new System.Drawing.Point(19, 61);
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			this.chart1.Series.Add(series1);
			this.chart1.Size = new System.Drawing.Size(580, 300);
			this.chart1.TabIndex = 12;
			this.chart1.Text = "chart1";
			// 
			// LoadSound
			// 
			this.LoadSound.Location = new System.Drawing.Point(695, 386);
			this.LoadSound.Name = "LoadSound";
			this.LoadSound.Size = new System.Drawing.Size(93, 52);
			this.LoadSound.TabIndex = 13;
			this.LoadSound.Text = "Load Sound";
			this.LoadSound.UseVisualStyleBackColor = true;
			this.LoadSound.Click += new System.EventHandler(this.LoadSound_Click);
			// 
			// FileDirectoryTextBox
			// 
			this.FileDirectoryTextBox.Location = new System.Drawing.Point(626, 318);
			this.FileDirectoryTextBox.Name = "FileDirectoryTextBox";
			this.FileDirectoryTextBox.Size = new System.Drawing.Size(162, 20);
			this.FileDirectoryTextBox.TabIndex = 14;
			this.FileDirectoryTextBox.Text = "Load and save location here";
			// 
			// FileNameTextBox
			// 
			this.FileNameTextBox.Location = new System.Drawing.Point(626, 344);
			this.FileNameTextBox.Name = "FileNameTextBox";
			this.FileNameTextBox.Size = new System.Drawing.Size(162, 20);
			this.FileNameTextBox.TabIndex = 15;
			this.FileNameTextBox.Text = ".wav file name here";
			// 
			// playSound
			// 
			this.playSound.Location = new System.Drawing.Point(100, 386);
			this.playSound.Name = "playSound";
			this.playSound.Size = new System.Drawing.Size(75, 52);
			this.playSound.TabIndex = 16;
			this.playSound.Text = "PlaySound";
			this.playSound.UseVisualStyleBackColor = true;
			this.playSound.Click += new System.EventHandler(this.playSound_Click);
			// 
			// EffectGenerator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.playSound);
			this.Controls.Add(this.FileNameTextBox);
			this.Controls.Add(this.FileDirectoryTextBox);
			this.Controls.Add(this.LoadSound);
			this.Controls.Add(this.chart1);
			this.Controls.Add(this.GenerateNotes);
			this.Controls.Add(this.Generate);
			this.Controls.Add(this.checkBox4);
			this.Controls.Add(this.checkBox5);
			this.Controls.Add(this.AmplitudeScaleEffect);
			this.Controls.Add(this.ReverseSampleEffect);
			this.Controls.Add(this.NormaliseSampleEffect);
			this.Controls.Add(this.PhaseInverterEffect);
			this.Controls.Add(this.SaveSound);
			this.Controls.Add(this.label1);
			this.Name = "EffectGenerator";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.SoundEffectGenerator);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SaveSound;
        private System.Windows.Forms.CheckBox PhaseInverterEffect;
        private System.Windows.Forms.CheckBox NormaliseSampleEffect;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox AmplitudeScaleEffect;
        private System.Windows.Forms.Button Generate;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox ReverseSampleEffect;
        private System.Windows.Forms.Button GenerateNotes;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
		private System.Windows.Forms.Button LoadSound;
		private System.Windows.Forms.TextBox FileDirectoryTextBox;
		private System.Windows.Forms.TextBox FileNameTextBox;
		private System.Windows.Forms.Button playSound;
	}
}

