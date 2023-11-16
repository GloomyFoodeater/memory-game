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
            ((System.ComponentModel.ISupportInitialize)DirectionPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MaxTactsEdit).BeginInit();
            SuspendLayout();
            // 
            // DirectionPictureBox
            // 
            DirectionPictureBox.Location = new Point(150, 0);
            DirectionPictureBox.Name = "DirectionPictureBox";
            DirectionPictureBox.Size = new Size(500, 375);
            DirectionPictureBox.TabIndex = 0;
            DirectionPictureBox.TabStop = false;
            // 
            // StartButton
            // 
            StartButton.Location = new Point(326, 381);
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
            MaxTactsEdit.Location = new Point(326, 437);
            MaxTactsEdit.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
            MaxTactsEdit.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            MaxTactsEdit.Name = "MaxTactsEdit";
            MaxTactsEdit.Size = new Size(150, 29);
            MaxTactsEdit.TabIndex = 2;
            MaxTactsEdit.Value = new decimal(new int[] { 3, 0, 0, 0 });
            MaxTactsEdit.ValueChanged += MaxTactsEdit_ValueChanged;
            // 
            // PortSelector
            // 
            PortSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            PortSelector.FormattingEnabled = true;
            PortSelector.Location = new Point(529, 408);
            PortSelector.Name = "PortSelector";
            PortSelector.Size = new Size(121, 23);
            PortSelector.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(574, 388);
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
            RefreshButton.Location = new Point(656, 408);
            RefreshButton.Name = "RefreshButton";
            RefreshButton.Size = new Size(75, 23);
            RefreshButton.TabIndex = 7;
            RefreshButton.Text = "Refresh";
            RefreshButton.UseVisualStyleBackColor = true;
            RefreshButton.Click += RefreshButton_Click;
            // 
            // ConnectButton
            // 
            ConnectButton.Location = new Point(553, 443);
            ConnectButton.Name = "ConnectButton";
            ConnectButton.Size = new Size(75, 23);
            ConnectButton.TabIndex = 8;
            ConnectButton.Text = "Connect";
            ConnectButton.UseVisualStyleBackColor = true;
            ConnectButton.Click += ConnectButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(ConnectButton);
            Controls.Add(RefreshButton);
            Controls.Add(ProgressLabel);
            Controls.Add(label1);
            Controls.Add(PortSelector);
            Controls.Add(MaxTactsEdit);
            Controls.Add(StartButton);
            Controls.Add(DirectionPictureBox);
            MaximumSize = new Size(800, 600);
            MinimumSize = new Size(800, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lab3";
            ((System.ComponentModel.ISupportInitialize)DirectionPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)MaxTactsEdit).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
    }
}