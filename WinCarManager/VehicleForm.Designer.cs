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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.EngineIsStarted = new System.Windows.Forms.TextBox();
            this.EngineHorsePower = new System.Windows.Forms.TextBox();
            this.Color = new System.Windows.Forms.TextBox();
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Door = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsOpen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // EnrollmentSerial
            // 
            this.EnrollmentSerial.Location = new System.Drawing.Point(97, 6);
            this.EnrollmentSerial.Name = "EnrollmentSerial";
            this.EnrollmentSerial.ReadOnly = true;
            this.EnrollmentSerial.Size = new System.Drawing.Size(48, 22);
            this.EnrollmentSerial.TabIndex = 0;
            // 
            // EnrollmentNumber
            // 
            this.EnrollmentNumber.Location = new System.Drawing.Point(151, 6);
            this.EnrollmentNumber.Name = "EnrollmentNumber";
            this.EnrollmentNumber.ReadOnly = true;
            this.EnrollmentNumber.Size = new System.Drawing.Size(100, 22);
            this.EnrollmentNumber.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enrollment:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.EngineIsStarted);
            this.groupBox1.Controls.Add(this.EngineHorsePower);
            this.groupBox1.Location = new System.Drawing.Point(12, 108);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 78);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Engine";
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
            // EngineIsStarted
            // 
            this.EngineIsStarted.Location = new System.Drawing.Point(105, 43);
            this.EngineIsStarted.Name = "EngineIsStarted";
            this.EngineIsStarted.ReadOnly = true;
            this.EngineIsStarted.Size = new System.Drawing.Size(100, 22);
            this.EngineIsStarted.TabIndex = 6;
            // 
            // EngineHorsePower
            // 
            this.EngineHorsePower.Location = new System.Drawing.Point(105, 15);
            this.EngineHorsePower.Name = "EngineHorsePower";
            this.EngineHorsePower.ReadOnly = true;
            this.EngineHorsePower.Size = new System.Drawing.Size(100, 22);
            this.EngineHorsePower.TabIndex = 5;
            // 
            // Color
            // 
            this.Color.Location = new System.Drawing.Point(97, 34);
            this.Color.Name = "Color";
            this.Color.ReadOnly = true;
            this.Color.Size = new System.Drawing.Size(100, 22);
            this.Color.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Color:";
            // 
            // ButtonFirst
            // 
            this.ButtonFirst.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ButtonFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonFirst.Location = new System.Drawing.Point(293, 266);
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
            this.ButtonPrev.Location = new System.Drawing.Point(349, 266);
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
            this.ButtonNext.Location = new System.Drawing.Point(405, 266);
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
            this.ButtonLast.Location = new System.Drawing.Point(461, 266);
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
            this.label5.Location = new System.Drawing.Point(325, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Current Position:";
            // 
            // LabelPosition
            // 
            this.LabelPosition.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LabelPosition.AutoSize = true;
            this.LabelPosition.Location = new System.Drawing.Point(444, 246);
            this.LabelPosition.Name = "LabelPosition";
            this.LabelPosition.Size = new System.Drawing.Size(0, 17);
            this.LabelPosition.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(279, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "Doors";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(535, 17);
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
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.Location = new System.Drawing.Point(15, 266);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(75, 50);
            this.ButtonAdd.TabIndex = 16;
            this.ButtonAdd.Text = "Add";
            this.ButtonAdd.UseVisualStyleBackColor = true;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // ButtonModify
            // 
            this.ButtonModify.Location = new System.Drawing.Point(96, 266);
            this.ButtonModify.Name = "ButtonModify";
            this.ButtonModify.Size = new System.Drawing.Size(75, 50);
            this.ButtonModify.TabIndex = 17;
            this.ButtonModify.Text = "Modify";
            this.ButtonModify.UseVisualStyleBackColor = true;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(632, 266);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 50);
            this.ButtonCancel.TabIndex = 19;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(713, 266);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 50);
            this.ButtonOK.TabIndex = 18;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Door,
            this.IsOpen});
            this.dataGridView1.Location = new System.Drawing.Point(282, 37);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(250, 200);
            this.dataGridView1.TabIndex = 20;
            // 
            // Door
            // 
            this.Door.HeaderText = "Door #";
            this.Door.Name = "Door";
            this.Door.ReadOnly = true;
            // 
            // IsOpen
            // 
            this.IsOpen.HeaderText = "IsOpen";
            this.IsOpen.Name = "IsOpen";
            this.IsOpen.ReadOnly = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dataGridView2.Location = new System.Drawing.Point(538, 37);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(250, 200);
            this.dataGridView2.TabIndex = 21;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Wheel #";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Pressure";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // VehicleForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 322);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
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
            this.Controls.Add(this.Color);
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
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
        private System.Windows.Forms.TextBox EngineIsStarted;
        private System.Windows.Forms.TextBox EngineHorsePower;
        private System.Windows.Forms.TextBox Color;
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
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Door;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsOpen;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}