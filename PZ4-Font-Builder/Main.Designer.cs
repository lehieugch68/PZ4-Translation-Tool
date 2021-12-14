﻿
namespace PZ4_Font_Builder
{
    partial class Main
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
            this.btnBuild = new System.Windows.Forms.Button();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.textBoxImage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxKerning = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxTranslation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxGDLG = new System.Windows.Forms.TextBox();
            this.checkBoxNumber = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(12, 244);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(560, 23);
            this.btnBuild.TabIndex = 0;
            this.btnBuild.Text = "Build";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // textBoxFile
            // 
            this.textBoxFile.AllowDrop = true;
            this.textBoxFile.Location = new System.Drawing.Point(12, 124);
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.Size = new System.Drawing.Size(560, 20);
            this.textBoxFile.TabIndex = 1;
            // 
            // textBoxImage
            // 
            this.textBoxImage.AllowDrop = true;
            this.textBoxImage.Location = new System.Drawing.Point(12, 170);
            this.textBoxImage.Name = "textBoxImage";
            this.textBoxImage.Size = new System.Drawing.Size(560, 20);
            this.textBoxImage.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "STRIMAG2 File";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Image Directory";
            // 
            // textBoxKerning
            // 
            this.textBoxKerning.Location = new System.Drawing.Point(196, 203);
            this.textBoxKerning.Name = "textBoxKerning";
            this.textBoxKerning.Size = new System.Drawing.Size(376, 20);
            this.textBoxKerning.TabIndex = 5;
            this.textBoxKerning.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxKerning_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Kerning Mod";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Translation File";
            // 
            // textBoxTranslation
            // 
            this.textBoxTranslation.AllowDrop = true;
            this.textBoxTranslation.Location = new System.Drawing.Point(12, 75);
            this.textBoxTranslation.Name = "textBoxTranslation";
            this.textBoxTranslation.Size = new System.Drawing.Size(560, 20);
            this.textBoxTranslation.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "GDLG File";
            // 
            // textBoxGDLG
            // 
            this.textBoxGDLG.AllowDrop = true;
            this.textBoxGDLG.Location = new System.Drawing.Point(12, 30);
            this.textBoxGDLG.Name = "textBoxGDLG";
            this.textBoxGDLG.Size = new System.Drawing.Size(560, 20);
            this.textBoxGDLG.TabIndex = 9;
            // 
            // checkBoxNumber
            // 
            this.checkBoxNumber.AutoSize = true;
            this.checkBoxNumber.Location = new System.Drawing.Point(12, 206);
            this.checkBoxNumber.Name = "checkBoxNumber";
            this.checkBoxNumber.Size = new System.Drawing.Size(91, 17);
            this.checkBoxNumber.TabIndex = 12;
            this.checkBoxNumber.Text = "Number Code";
            this.checkBoxNumber.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 291);
            this.Controls.Add(this.checkBoxNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxGDLG);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxTranslation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxKerning);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxImage);
            this.Controls.Add(this.textBoxFile);
            this.Controls.Add(this.btnBuild);
            this.Name = "Main";
            this.Text = "Project Zero 4 Translation Tool";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.TextBox textBoxImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxKerning;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxTranslation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxGDLG;
        private System.Windows.Forms.CheckBox checkBoxNumber;
    }
}

