namespace DesktopUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DirectionPictureBox = new PictureBox();
            StartButton = new Button();
            MaxTactsEdit = new NumericUpDown();
            PortSelector = new ComboBox();
            label1 = new Label();
            ProgressLabel = new Label();
            RefreshButton = new Button();
            ConnectButton = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)DirectionPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MaxTactsEdit).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // DirectionPictureBox
            // 
            DirectionPictureBox.Location = new Point(147, 0);
            DirectionPictureBox.Name = "DirectionPictureBox";
            DirectionPictureBox.Size = new Size(500, 375);
            DirectionPictureBox.TabIndex = 0;
            DirectionPictureBox.TabStop = false;
            // 
            // StartButton
            // 
            StartButton.Location = new Point(314, 399);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(150, 50);
            StartButton.TabIndex = 1;
            StartButton.Text = "Start";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // MaxTactsEdit
            // 
            MaxTactsEdit.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            MaxTactsEdit.Location = new Point(314, 465);
            MaxTactsEdit.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
            MaxTactsEdit.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            MaxTactsEdit.Name = "MaxTactsEdit";
            MaxTactsEdit.Size = new Size(150, 29);
            MaxTactsEdit.TabIndex = 2;
            MaxTactsEdit.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // PortSelector
            // 
            PortSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            PortSelector.FormattingEnabled = true;
            PortSelector.Location = new Point(324, 209);
            PortSelector.Name = "PortSelector";
            PortSelector.Size = new Size(121, 23);
            PortSelector.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(369, 189);
            label1.Name = "label1";
            label1.Size = new Size(34, 15);
            label1.TabIndex = 5;
            label1.Text = "Ports";
            // 
            // ProgressLabel
            // 
            ProgressLabel.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            ProgressLabel.Location = new Point(178, 399);
            ProgressLabel.Name = "ProgressLabel";
            ProgressLabel.Size = new Size(100, 100);
            ProgressLabel.TabIndex = 6;
            ProgressLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RefreshButton
            // 
            RefreshButton.Location = new Point(460, 208);
            RefreshButton.Name = "RefreshButton";
            RefreshButton.Size = new Size(75, 23);
            RefreshButton.TabIndex = 7;
            RefreshButton.Text = "Refresh";
            RefreshButton.UseVisualStyleBackColor = true;
            RefreshButton.Click += RefreshButton_Click;
            // 
            // ConnectButton
            // 
            ConnectButton.Enabled = false;
            ConnectButton.Location = new Point(348, 244);
            ConnectButton.Name = "ConnectButton";
            ConnectButton.Size = new Size(75, 23);
            ConnectButton.TabIndex = 8;
            ConnectButton.Text = "Connect";
            ConnectButton.UseVisualStyleBackColor = true;
            ConnectButton.Click += ConnectButton_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(PortSelector);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(ConnectButton);
            panel1.Controls.Add(RefreshButton);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(784, 561);
            panel1.TabIndex = 9;
            panel1.Visible = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(MaxTactsEdit);
            panel2.Controls.Add(DirectionPictureBox);
            panel2.Controls.Add(StartButton);
            panel2.Controls.Add(panel1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(784, 561);
            panel2.TabIndex = 10;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(ProgressLabel);
            Controls.Add(panel2);
            MaximumSize = new Size(800, 600);
            MinimumSize = new Size(800, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lab3";
            KeyUp += MainForm_KeyPress;
            ((System.ComponentModel.ISupportInitialize)DirectionPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)MaxTactsEdit).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox DirectionPictureBox;
        private Button StartButton;
        private NumericUpDown MaxTactsEdit;
        private ComboBox PortSelector;
        private Label label1;
        private Label ProgressLabel;
        private Button RefreshButton;
        private Button ConnectButton;
        private Panel panel1;
        private Panel panel2;
    }
}