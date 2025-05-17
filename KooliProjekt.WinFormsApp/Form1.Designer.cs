namespace KooliProjekt.WinFormsApp
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
            BuildingsGrid = new DataGridView();
            IdField = new TextBox();
            LocationField = new TextBox();
            DateField = new DateTimePicker();
            IdLabel = new Label();
            LocationLabel = new Label();
            DateLabel = new Label();
            NewButton = new Button();
            SaveButton = new Button();
            DeleteButton = new Button();
            ((System.ComponentModel.ISupportInitialize)BuildingsGrid).BeginInit();
            SuspendLayout();
            // 
            // BuildingsGrid
            // 
            BuildingsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            BuildingsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            BuildingsGrid.Location = new Point(12, 12);
            BuildingsGrid.MultiSelect = false;
            BuildingsGrid.Name = "BuildingsGrid";
            BuildingsGrid.ReadOnly = true;
            BuildingsGrid.RowTemplate.Height = 25;
            BuildingsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            BuildingsGrid.Size = new Size(569, 288);
            BuildingsGrid.TabIndex = 0;
            // 
            // IdField
            // 
            IdField.Location = new Point(104, 323);
            IdField.Name = "IdField";
            IdField.ReadOnly = true;
            IdField.Size = new Size(100, 23);
            IdField.TabIndex = 1;
            IdField.Text = "0";
            // 
            // LocationField
            // 
            LocationField.Location = new Point(104, 352);
            LocationField.Name = "LocationField";
            LocationField.Size = new Size(201, 23);
            LocationField.TabIndex = 2;
            // 
            // DateField
            // 
            DateField.Location = new Point(104, 381);
            DateField.Name = "DateField";
            DateField.Size = new Size(201, 23);
            DateField.TabIndex = 3;
            // 
            // IdLabel
            // 
            IdLabel.AutoSize = true;
            IdLabel.Location = new Point(12, 326);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new Size(20, 15);
            IdLabel.TabIndex = 4;
            IdLabel.Text = "Id:";
            // 
            // LocationLabel
            // 
            LocationLabel.AutoSize = true;
            LocationLabel.Location = new Point(12, 355);
            LocationLabel.Name = "LocationLabel";
            LocationLabel.Size = new Size(56, 15);
            LocationLabel.TabIndex = 5;
            LocationLabel.Text = "Location:";
            // 
            // DateLabel
            // 
            DateLabel.AutoSize = true;
            DateLabel.Location = new Point(12, 387);
            DateLabel.Name = "DateLabel";
            DateLabel.Size = new Size(34, 15);
            DateLabel.TabIndex = 6;
            DateLabel.Text = "Date:";
            // 
            // NewButton
            // 
            NewButton.Location = new Point(323, 352);
            NewButton.Name = "NewButton";
            NewButton.Size = new Size(75, 23);
            NewButton.TabIndex = 7;
            NewButton.Text = "New";
            NewButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(415, 352);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(75, 23);
            SaveButton.TabIndex = 8;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            // 
            // DeleteButton
            // 
            DeleteButton.Location = new Point(506, 352);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(75, 23);
            DeleteButton.TabIndex = 9;
            DeleteButton.Text = "Delete";
            DeleteButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(593, 416);
            Controls.Add(DeleteButton);
            Controls.Add(SaveButton);
            Controls.Add(NewButton);
            Controls.Add(DateLabel);
            Controls.Add(LocationLabel);
            Controls.Add(IdLabel);
            Controls.Add(DateField);
            Controls.Add(LocationField);
            Controls.Add(IdField);
            Controls.Add(BuildingsGrid);
            Name = "Form1";
            Text = "Buildings Manager";
            ((System.ComponentModel.ISupportInitialize)BuildingsGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView BuildingsGrid;
        private TextBox IdField;
        private TextBox LocationField;
        private DateTimePicker DateField;
        private Label IdLabel;
        private Label LocationLabel;
        private Label DateLabel;
        private Button NewButton;
        private Button SaveButton;
        private Button DeleteButton;
    }
}
