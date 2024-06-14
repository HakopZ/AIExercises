namespace Visualizer
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            AddNode = new Button();
            textBoxLocation = new TextBox();
            pictureBox = new PictureBox();
            AddEdgeButton = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            Loc = new Label();
            Weight = new Label();
            weightTextbox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // AddNode
            // 
            AddNode.Location = new Point(655, 164);
            AddNode.Name = "AddNode";
            AddNode.Size = new Size(75, 23);
            AddNode.TabIndex = 0;
            AddNode.Text = "Add Node";
            AddNode.UseVisualStyleBackColor = true;
            AddNode.Click += AddNode_Click;
            // 
            // textBoxLocation
            // 
            textBoxLocation.Location = new Point(711, 205);
            textBoxLocation.Name = "textBoxLocation";
            textBoxLocation.Size = new Size(66, 23);
            textBoxLocation.TabIndex = 1;
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(649, 454);
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            pictureBox.Click += pictureBox_Click;
            // 
            // AddEdgeButton
            // 
            AddEdgeButton.Location = new Point(655, 105);
            AddEdgeButton.Name = "AddEdgeButton";
            AddEdgeButton.Size = new Size(75, 23);
            AddEdgeButton.TabIndex = 3;
            AddEdgeButton.Text = "Add Edge";
            AddEdgeButton.UseVisualStyleBackColor = true;
            AddEdgeButton.Click += AddEdgeButton_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 17;
            timer1.Tick += timer1_Tick;
            // 
            // Loc
            // 
            Loc.AutoSize = true;
            Loc.Location = new Point(655, 213);
            Loc.Name = "Loc";
            Loc.Size = new Size(50, 15);
            Loc.TabIndex = 4;
            Loc.Text = "Position";
            // 
            // Weight
            // 
            Weight.AutoSize = true;
            Weight.Location = new Point(655, 251);
            Weight.Name = "Weight";
            Weight.Size = new Size(45, 15);
            Weight.TabIndex = 5;
            Weight.Text = "Weight";
            // 
            // weightTextbox
            // 
            weightTextbox.Location = new Point(711, 243);
            weightTextbox.Name = "weightTextbox";
            weightTextbox.Size = new Size(66, 23);
            weightTextbox.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(weightTextbox);
            Controls.Add(Weight);
            Controls.Add(Loc);
            Controls.Add(AddEdgeButton);
            Controls.Add(pictureBox);
            Controls.Add(textBoxLocation);
            Controls.Add(AddNode);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button AddNode;
        private TextBox textBoxLocation;
        private PictureBox pictureBox;
        private Button AddEdgeButton;
        private System.Windows.Forms.Timer timer1;
        private Label Loc;
        private Label Weight;
        private TextBox weightTextbox;
    }
}
