namespace DarkUI.Demo.Forms
{
    partial class TestForm
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
            this.BTNDarkTheme = new DarkUI.Controls.DarkButton();
            this.BTNLightTheme = new DarkUI.Controls.DarkButton();
            this.BTNOpenMainForm = new DarkUI.Controls.DarkButton();
            this.darkTabControl1 = new DarkUI.Controls.DarkTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.darkTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BTNDarkTheme
            // 
            this.BTNDarkTheme.Location = new System.Drawing.Point(118, 12);
            this.BTNDarkTheme.Name = "BTNDarkTheme";
            this.BTNDarkTheme.Padding = new System.Windows.Forms.Padding(5);
            this.BTNDarkTheme.Size = new System.Drawing.Size(100, 40);
            this.BTNDarkTheme.TabIndex = 1;
            this.BTNDarkTheme.Text = "Dark Theme";
            this.BTNDarkTheme.Click += new System.EventHandler(this.BTNDarkTheme_Click);
            // 
            // BTNLightTheme
            // 
            this.BTNLightTheme.Location = new System.Drawing.Point(224, 12);
            this.BTNLightTheme.Name = "BTNLightTheme";
            this.BTNLightTheme.Padding = new System.Windows.Forms.Padding(5);
            this.BTNLightTheme.Size = new System.Drawing.Size(100, 40);
            this.BTNLightTheme.TabIndex = 2;
            this.BTNLightTheme.Text = "Light Theme";
            this.BTNLightTheme.Click += new System.EventHandler(this.BTNLightTheme_Click);
            // 
            // BTNOpenMainForm
            // 
            this.BTNOpenMainForm.Location = new System.Drawing.Point(12, 12);
            this.BTNOpenMainForm.Name = "BTNOpenMainForm";
            this.BTNOpenMainForm.Padding = new System.Windows.Forms.Padding(5);
            this.BTNOpenMainForm.Size = new System.Drawing.Size(100, 40);
            this.BTNOpenMainForm.TabIndex = 0;
            this.BTNOpenMainForm.Text = "Open MainForm";
            this.BTNOpenMainForm.Click += new System.EventHandler(this.BTNOpenMainForm_Click);
            // 
            // darkTabControl1
            // 
            this.darkTabControl1.Controls.Add(this.tabPage1);
            this.darkTabControl1.Controls.Add(this.tabPage2);
            this.darkTabControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkTabControl1.Location = new System.Drawing.Point(118, 102);
            this.darkTabControl1.Name = "darkTabControl1";
            this.darkTabControl1.SelectedIndex = 0;
            this.darkTabControl1.Size = new System.Drawing.Size(456, 256);
            this.darkTabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tabPage1.Location = new System.Drawing.Point(1, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(454, 232);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tabPage2.Location = new System.Drawing.Point(1, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(454, 232);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.darkTabControl1);
            this.Controls.Add(this.BTNOpenMainForm);
            this.Controls.Add(this.BTNLightTheme);
            this.Controls.Add(this.BTNDarkTheme);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Name = "TestForm";
            this.ShowIcon = false;
            this.Text = "TestForm";
            this.darkTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DarkButton BTNDarkTheme;
        private Controls.DarkButton BTNLightTheme;
        private Controls.DarkButton BTNOpenMainForm;
        private Controls.DarkTabControl darkTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}