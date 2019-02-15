namespace WinCarManager
{
    partial class VehicleForm
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
            this.components = new System.ComponentModel.Container();
            this.EnrollmentSerial = new System.Windows.Forms.TextBox();
            this.EnrollmentNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EngineIsStarted = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.EngineHorsePower = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonFirst = new System.Windows.Forms.Button();
            this.ButtonPrev = new System.Windows.Forms.Button();
            this.ButtonNext = new System.Windows.Forms.Button();
            this.ButtonLast = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label5 = new System.Windows.Forms.Label();
            this.LabelPosition = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ButtonAdd = new System.Windows.Forms.Button();
            this.ButtonModify = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.DoorGridView = new System.Windows.Forms.DataGridView();
            this.IsOpen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WheelGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Color = new System.Windows.Forms.ComboBox();
            this.EnrollmentsGridView = new System.Windows.Forms.DataGridView();
            this.EnrollmentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SerialColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DoorGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WheelGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnrollmentsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // EnrollmentSerial
            // 
            this.EnrollmentSerial.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.EnrollmentSerial.Location = new System.Drawing.Point(334, 12);
            this.EnrollmentSerial.Name = "EnrollmentSerial";
            this.EnrollmentSerial.ReadOnly = true;
            this.EnrollmentSerial.Size = new System.Drawing.Size(48, 22);
            this.EnrollmentSerial.TabIndex = 0;
            // 
            // EnrollmentNumber
            // 
            this.EnrollmentNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.EnrollmentNumber.Location = new System.Drawing.Point(388, 12);
            this.EnrollmentNumber.Name = "EnrollmentNumber";
            this.EnrollmentNumber.ReadOnly = true;
            this.EnrollmentNumber.Size = new System.Drawing.Size(67, 22);
            this.EnrollmentNumber.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enrollment:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.EngineIsStarted);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.EngineHorsePower);
            this.groupBox1.Location = new System.Drawing.Point(252, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 78);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Engine";
            // 
            // EngineIsStarted
            // 
            this.EngineIsStarted.AutoSize = true;
            this.EngineIsStarted.Enabled = false;
            this.EngineIsStarted.Location = new System.Drawing.Point(105, 47);
            this.EngineIsStarted.Name = "EngineIsStarted";
            this.EngineIsStarted.Size = new System.Drawing.Size(18, 17);
            this.EngineIsStarted.TabIndex = 23;
            this.EngineIsStarted.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Is Started:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Horse Power:";
            // 
            // EngineHorsePower
            // 
            this.EngineHorsePower.Location = new System.Drawing.Point(105, 15);
            this.EngineHorsePower.Name = "EngineHorsePower";
            this.EngineHorsePower.ReadOnly = true;
            this.EngineHorsePower.Size = new System.Drawing.Size(100, 22);
            this.EngineHorsePower.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Color:";
            // 
            // ButtonFirst
            // 
            this.ButtonFirst.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ButtonFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonFirst.Location = new System.Drawing.Point(293, 361);
            this.ButtonFirst.Name = "ButtonFirst";
            this.ButtonFirst.Size = new System.Drawing.Size(50, 50);
            this.ButtonFirst.TabIndex = 6;
            this.ButtonFirst.UseVisualStyleBackColor = true;
            this.ButtonFirst.Click += new System.EventHandler(this.ButtonFirst_Click);
            // 
            // ButtonPrev
            // 
            this.ButtonPrev.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ButtonPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonPrev.Location = new System.Drawing.Point(349, 361);
            this.ButtonPrev.Name = "ButtonPrev";
            this.ButtonPrev.Size = new System.Drawing.Size(50, 50);
            this.ButtonPrev.TabIndex = 7;
            this.ButtonPrev.UseVisualStyleBackColor = true;
            this.ButtonPrev.Click += new System.EventHandler(this.ButtonPrev_Click);
            // 
            // ButtonNext
            // 
            this.ButtonNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ButtonNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonNext.Location = new System.Drawing.Point(405, 361);
            this.ButtonNext.Name = "ButtonNext";
            this.ButtonNext.Size = new System.Drawing.Size(50, 50);
            this.ButtonNext.TabIndex = 8;
            this.ButtonNext.UseVisualStyleBackColor = true;
            this.ButtonNext.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // ButtonLast
            // 
            this.ButtonLast.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ButtonLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonLast.Location = new System.Drawing.Point(461, 361);
            this.ButtonLast.Name = "ButtonLast";
            this.ButtonLast.Size = new System.Drawing.Size(50, 50);
            this.ButtonLast.TabIndex = 9;
            this.ButtonLast.UseVisualStyleBackColor = true;
            this.ButtonLast.Click += new System.EventHandler(this.ButtonLast_Click);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(325, 341);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Current Position:";
            // 
            // LabelPosition
            // 
            this.LabelPosition.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LabelPosition.AutoSize = true;
            this.LabelPosition.Location = new System.Drawing.Point(444, 341);
            this.LabelPosition.Name = "LabelPosition";
            this.LabelPosition.Size = new System.Drawing.Size(0, 17);
            this.LabelPosition.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(535, 182);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "Doors";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(535, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Wheels";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonAdd.Location = new System.Drawing.Point(12, 361);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(75, 50);
            this.ButtonAdd.TabIndex = 16;
            this.ButtonAdd.Text = "Add";
            this.ButtonAdd.UseVisualStyleBackColor = true;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // ButtonModify
            // 
            this.ButtonModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonModify.Location = new System.Drawing.Point(93, 361);
            this.ButtonModify.Name = "ButtonModify";
            this.ButtonModify.Size = new System.Drawing.Size(75, 50);
            this.ButtonModify.TabIndex = 17;
            this.ButtonModify.Text = "Modify";
            this.ButtonModify.UseVisualStyleBackColor = true;
            this.ButtonModify.Click += new System.EventHandler(this.ButtonModify_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.Location = new System.Drawing.Point(632, 361);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 50);
            this.ButtonCancel.TabIndex = 19;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.Location = new System.Drawing.Point(713, 361);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 50);
            this.ButtonOK.TabIndex = 18;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // DoorGridView
            // 
            this.DoorGridView.AllowUserToAddRows = false;
            this.DoorGridView.AllowUserToResizeColumns = false;
            this.DoorGridView.AllowUserToResizeRows = false;
            this.DoorGridView.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DoorGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DoorGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.DoorGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DoorGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsOpen});
            this.DoorGridView.Location = new System.Drawing.Point(538, 202);
            this.DoorGridView.MultiSelect = false;
            this.DoorGridView.Name = "DoorGridView";
            this.DoorGridView.ReadOnly = true;
            this.DoorGridView.RowTemplate.Height = 24;
            this.DoorGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DoorGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DoorGridView.Size = new System.Drawing.Size(250, 150);
            this.DoorGridView.TabIndex = 20;
            // 
            // IsOpen
            // 
            this.IsOpen.HeaderText = "IsOpen";
            this.IsOpen.Name = "IsOpen";
            this.IsOpen.ReadOnly = true;
            // 
            // WheelGridView
            // 
            this.WheelGridView.AllowUserToAddRows = false;
            this.WheelGridView.AllowUserToResizeColumns = false;
            this.WheelGridView.AllowUserToResizeRows = false;
            this.WheelGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WheelGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.WheelGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.WheelGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WheelGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.WheelGridView.Location = new System.Drawing.Point(538, 29);
            this.WheelGridView.MultiSelect = false;
            this.WheelGridView.Name = "WheelGridView";
            this.WheelGridView.ReadOnly = true;
            this.WheelGridView.RowTemplate.Height = 24;
            this.WheelGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.WheelGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.WheelGridView.Size = new System.Drawing.Size(250, 150);
            this.WheelGridView.TabIndex = 21;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Pressure";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // Color
            // 
            this.Color.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Color.Enabled = false;
            this.Color.FormattingEnabled = true;
            this.Color.Location = new System.Drawing.Point(334, 40);
            this.Color.Name = "Color";
            this.Color.Size = new System.Drawing.Size(121, 24);
            this.Color.TabIndex = 22;
            // 
            // EnrollmentsGridView
            // 
            this.EnrollmentsGridView.AllowUserToAddRows = false;
            this.EnrollmentsGridView.AllowUserToDeleteRows = false;
            this.EnrollmentsGridView.AllowUserToResizeColumns = false;
            this.EnrollmentsGridView.AllowUserToResizeRows = false;
            this.EnrollmentsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.EnrollmentsGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.EnrollmentsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EnrollmentsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EnrollmentColumn,
            this.SerialColumn,
            this.NumberColumn});
            this.EnrollmentsGridView.Location = new System.Drawing.Point(12, 12);
            this.EnrollmentsGridView.MultiSelect = false;
            this.EnrollmentsGridView.Name = "EnrollmentsGridView";
            this.EnrollmentsGridView.RowTemplate.Height = 24;
            this.EnrollmentsGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.EnrollmentsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.EnrollmentsGridView.Size = new System.Drawing.Size(156, 340);
            this.EnrollmentsGridView.TabIndex = 23;
            this.EnrollmentsGridView.SelectionChanged += new System.EventHandler(this.EnrollmentsGridView_SelectionChanged);
            // 
            // EnrollmentColumn
            // 
            this.EnrollmentColumn.HeaderText = "Enrollment";
            this.EnrollmentColumn.Name = "EnrollmentColumn";
            this.EnrollmentColumn.ReadOnly = true;
            // 
            // SerialColumn
            // 
            this.SerialColumn.HeaderText = "Serial";
            this.SerialColumn.Name = "SerialColumn";
            this.SerialColumn.ReadOnly = true;
            this.SerialColumn.Visible = false;
            // 
            // NumberColumn
            // 
            this.NumberColumn.HeaderText = "Number";
            this.NumberColumn.Name = "NumberColumn";
            this.NumberColumn.ReadOnly = true;
            this.NumberColumn.Visible = false;
            // 
            // VehicleForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 417);
            this.Controls.Add(this.EnrollmentsGridView);
            this.Controls.Add(this.Color);
            this.Controls.Add(this.WheelGridView);
            this.Controls.Add(this.DoorGridView);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.ButtonModify);
            this.Controls.Add(this.ButtonAdd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LabelPosition);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ButtonLast);
            this.Controls.Add(this.ButtonNext);
            this.Controls.Add(this.ButtonPrev);
            this.Controls.Add(this.ButtonFirst);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EnrollmentNumber);
            this.Controls.Add(this.EnrollmentSerial);
            this.Name = "VehicleForm";
            this.Text = "Vehicles";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.VehicleForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DoorGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WheelGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnrollmentsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox EnrollmentSerial;
        private System.Windows.Forms.TextBox EnrollmentNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox EngineHorsePower;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonFirst;
        private System.Windows.Forms.Button ButtonPrev;
        private System.Windows.Forms.Button ButtonNext;
        private System.Windows.Forms.Button ButtonLast;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LabelPosition;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button ButtonAdd;
        private System.Windows.Forms.Button ButtonModify;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.DataGridView DoorGridView;
        private System.Windows.Forms.DataGridView WheelGridView;
        private System.Windows.Forms.ComboBox Color;
        private System.Windows.Forms.CheckBox EngineIsStarted;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsOpen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridView EnrollmentsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn EnrollmentColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberColumn;
    }
}