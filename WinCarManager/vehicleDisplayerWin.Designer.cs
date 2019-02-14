namespace WinCarManager
{
    partial class vehicleDisplayerWin
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.almacénToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vehículoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.doorListView = new System.Windows.Forms.ListView();
            this.wheelListView = new System.Windows.Forms.ListView();
            this.carListView = new System.Windows.Forms.ListView();
            this.storageTabButt = new System.Windows.Forms.Button();
            this.goToFirstButt = new System.Windows.Forms.Button();
            this.goToPreviousButt = new System.Windows.Forms.Button();
            this.goToNextButt = new System.Windows.Forms.Button();
            this.goToLastButt = new System.Windows.Forms.Button();
            this.horsePowerEngineView = new System.Windows.Forms.ListView();
            this.startedEngineView = new System.Windows.Forms.ListView();
            this.carColorView = new System.Windows.Forms.ListView();
            this.enrollmentView = new System.Windows.Forms.ListView();
            this.pictureBoxCar = new System.Windows.Forms.PictureBox();
            this.searchCarButt = new System.Windows.Forms.Button();
            this.updateCarButt = new System.Windows.Forms.Button();
            this.addCarButt = new System.Windows.Forms.Button();
            this.exitSearchButt = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.doorPropGroup = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.doorOpenedView = new System.Windows.Forms.ListView();
            this.wheelPropGroup = new System.Windows.Forms.GroupBox();
            this.wheelPressure = new System.Windows.Forms.Label();
            this.wheelPressureView = new System.Windows.Forms.ListView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCar)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.doorPropGroup.SuspendLayout();
            this.wheelPropGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.almacénToolStripMenuItem,
            this.vehículoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1054, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // almacénToolStripMenuItem
            // 
            this.almacénToolStripMenuItem.Name = "almacénToolStripMenuItem";
            this.almacénToolStripMenuItem.Size = new System.Drawing.Size(79, 24);
            this.almacénToolStripMenuItem.Text = "Almacén";
            // 
            // vehículoToolStripMenuItem
            // 
            this.vehículoToolStripMenuItem.Name = "vehículoToolStripMenuItem";
            this.vehículoToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.vehículoToolStripMenuItem.Text = "Vehicle";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(171, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enrollment";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Color";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Started";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "HorsePower";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(171, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Doors";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(463, 167);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 17);
            this.label7.TabIndex = 10;
            this.label7.Text = "Wheels";
            // 
            // doorListView
            // 
            this.doorListView.Location = new System.Drawing.Point(171, 187);
            this.doorListView.Name = "doorListView";
            this.doorListView.Size = new System.Drawing.Size(112, 135);
            this.doorListView.TabIndex = 12;
            this.doorListView.UseCompatibleStateImageBehavior = false;
            this.doorListView.SelectedIndexChanged += new System.EventHandler(this.doorListView_SelectedIndexChanged);
            // 
            // wheelListView
            // 
            this.wheelListView.Location = new System.Drawing.Point(466, 187);
            this.wheelListView.Name = "wheelListView";
            this.wheelListView.Size = new System.Drawing.Size(112, 135);
            this.wheelListView.TabIndex = 13;
            this.wheelListView.UseCompatibleStateImageBehavior = false;
            this.wheelListView.SelectedIndexChanged += new System.EventHandler(this.wheelListView_SelectedIndexChanged);
            // 
            // carListView
            // 
            this.carListView.Location = new System.Drawing.Point(0, 31);
            this.carListView.Name = "carListView";
            this.carListView.Size = new System.Drawing.Size(165, 525);
            this.carListView.TabIndex = 16;
            this.carListView.UseCompatibleStateImageBehavior = false;
            this.carListView.SelectedIndexChanged += new System.EventHandler(this.carListView_SelectedIndexChanged);
            // 
            // storageTabButt
            // 
            this.storageTabButt.Location = new System.Drawing.Point(0, 0);
            this.storageTabButt.Name = "storageTabButt";
            this.storageTabButt.Size = new System.Drawing.Size(75, 23);
            this.storageTabButt.TabIndex = 17;
            this.storageTabButt.Text = "Storage";
            this.storageTabButt.UseVisualStyleBackColor = true;
            this.storageTabButt.Click += new System.EventHandler(this.storageTabButt_Click);
            // 
            // goToFirstButt
            // 
            this.goToFirstButt.Location = new System.Drawing.Point(171, 507);
            this.goToFirstButt.Name = "goToFirstButt";
            this.goToFirstButt.Size = new System.Drawing.Size(200, 35);
            this.goToFirstButt.TabIndex = 20;
            this.goToFirstButt.Text = "|| <=";
            this.goToFirstButt.UseVisualStyleBackColor = true;
            this.goToFirstButt.Click += new System.EventHandler(this.goToFirstButt_Click);
            // 
            // goToPreviousButt
            // 
            this.goToPreviousButt.Location = new System.Drawing.Point(377, 507);
            this.goToPreviousButt.Name = "goToPreviousButt";
            this.goToPreviousButt.Size = new System.Drawing.Size(200, 35);
            this.goToPreviousButt.TabIndex = 20;
            this.goToPreviousButt.Text = "<=";
            this.goToPreviousButt.UseVisualStyleBackColor = true;
            this.goToPreviousButt.Click += new System.EventHandler(this.goToPreviousButt_Click);
            // 
            // goToNextButt
            // 
            this.goToNextButt.Location = new System.Drawing.Point(636, 507);
            this.goToNextButt.Name = "goToNextButt";
            this.goToNextButt.Size = new System.Drawing.Size(200, 35);
            this.goToNextButt.TabIndex = 20;
            this.goToNextButt.Text = "=>";
            this.goToNextButt.UseVisualStyleBackColor = true;
            this.goToNextButt.Click += new System.EventHandler(this.goToNextButt_Click);
            // 
            // goToLastButt
            // 
            this.goToLastButt.Location = new System.Drawing.Point(842, 507);
            this.goToLastButt.Name = "goToLastButt";
            this.goToLastButt.Size = new System.Drawing.Size(200, 35);
            this.goToLastButt.TabIndex = 20;
            this.goToLastButt.Text = "=> ||";
            this.goToLastButt.UseVisualStyleBackColor = true;
            this.goToLastButt.Click += new System.EventHandler(this.goToLastButt_Click);
            // 
            // horsePowerEngineView
            // 
            this.horsePowerEngineView.Location = new System.Drawing.Point(115, 51);
            this.horsePowerEngineView.Name = "horsePowerEngineView";
            this.horsePowerEngineView.Size = new System.Drawing.Size(165, 20);
            this.horsePowerEngineView.TabIndex = 23;
            this.horsePowerEngineView.UseCompatibleStateImageBehavior = false;
            // 
            // startedEngineView
            // 
            this.startedEngineView.Location = new System.Drawing.Point(115, 21);
            this.startedEngineView.Name = "startedEngineView";
            this.startedEngineView.Size = new System.Drawing.Size(165, 20);
            this.startedEngineView.TabIndex = 24;
            this.startedEngineView.UseCompatibleStateImageBehavior = false;
            // 
            // carColorView
            // 
            this.carColorView.Location = new System.Drawing.Point(289, 54);
            this.carColorView.Name = "carColorView";
            this.carColorView.Size = new System.Drawing.Size(165, 20);
            this.carColorView.TabIndex = 25;
            this.carColorView.UseCompatibleStateImageBehavior = false;
            // 
            // enrollmentView
            // 
            this.enrollmentView.Location = new System.Drawing.Point(289, 28);
            this.enrollmentView.Name = "enrollmentView";
            this.enrollmentView.Size = new System.Drawing.Size(165, 20);
            this.enrollmentView.TabIndex = 26;
            this.enrollmentView.UseCompatibleStateImageBehavior = false;
            // 
            // pictureBoxCar
            // 
            this.pictureBoxCar.Location = new System.Drawing.Point(468, 35);
            this.pictureBoxCar.Name = "pictureBoxCar";
            this.pictureBoxCar.Size = new System.Drawing.Size(368, 120);
            this.pictureBoxCar.TabIndex = 27;
            this.pictureBoxCar.TabStop = false;
            // 
            // searchCarButt
            // 
            this.searchCarButt.Location = new System.Drawing.Point(842, 158);
            this.searchCarButt.Name = "searchCarButt";
            this.searchCarButt.Size = new System.Drawing.Size(200, 35);
            this.searchCarButt.TabIndex = 28;
            this.searchCarButt.Text = "Search";
            this.searchCarButt.UseVisualStyleBackColor = true;
            this.searchCarButt.Click += new System.EventHandler(this.searchCarButt_Click);
            // 
            // updateCarButt
            // 
            this.updateCarButt.Location = new System.Drawing.Point(842, 278);
            this.updateCarButt.Name = "updateCarButt";
            this.updateCarButt.Size = new System.Drawing.Size(200, 35);
            this.updateCarButt.TabIndex = 29;
            this.updateCarButt.Text = "Update";
            this.updateCarButt.UseVisualStyleBackColor = true;
            this.updateCarButt.Click += new System.EventHandler(this.updateCarButt_Click);
            // 
            // addCarButt
            // 
            this.addCarButt.Location = new System.Drawing.Point(842, 319);
            this.addCarButt.Name = "addCarButt";
            this.addCarButt.Size = new System.Drawing.Size(200, 35);
            this.addCarButt.TabIndex = 30;
            this.addCarButt.Text = "Add";
            this.addCarButt.UseVisualStyleBackColor = true;
            this.addCarButt.Click += new System.EventHandler(this.addCarButt_Click);
            // 
            // exitSearchButt
            // 
            this.exitSearchButt.Location = new System.Drawing.Point(842, 199);
            this.exitSearchButt.Name = "exitSearchButt";
            this.exitSearchButt.Size = new System.Drawing.Size(200, 35);
            this.exitSearchButt.TabIndex = 31;
            this.exitSearchButt.Text = "Quit";
            this.exitSearchButt.UseVisualStyleBackColor = true;
            this.exitSearchButt.Click += new System.EventHandler(this.exitSearchButt_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.startedEngineView);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.horsePowerEngineView);
            this.groupBox1.Location = new System.Drawing.Point(174, 78);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 77);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Engine";
            // 
            // doorPropGroup
            // 
            this.doorPropGroup.Controls.Add(this.label3);
            this.doorPropGroup.Controls.Add(this.doorOpenedView);
            this.doorPropGroup.Location = new System.Drawing.Point(289, 187);
            this.doorPropGroup.Name = "doorPropGroup";
            this.doorPropGroup.Size = new System.Drawing.Size(171, 135);
            this.doorPropGroup.TabIndex = 33;
            this.doorPropGroup.TabStop = false;
            this.doorPropGroup.Text = "Door";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Opened";
            // 
            // doorOpenedView
            // 
            this.doorOpenedView.Location = new System.Drawing.Point(6, 40);
            this.doorOpenedView.Name = "doorOpenedView";
            this.doorOpenedView.Size = new System.Drawing.Size(79, 20);
            this.doorOpenedView.TabIndex = 24;
            this.doorOpenedView.UseCompatibleStateImageBehavior = false;
            // 
            // wheelPropGroup
            // 
            this.wheelPropGroup.Controls.Add(this.wheelPressure);
            this.wheelPropGroup.Controls.Add(this.wheelPressureView);
            this.wheelPropGroup.Location = new System.Drawing.Point(584, 187);
            this.wheelPropGroup.Name = "wheelPropGroup";
            this.wheelPropGroup.Size = new System.Drawing.Size(171, 135);
            this.wheelPropGroup.TabIndex = 34;
            this.wheelPropGroup.TabStop = false;
            this.wheelPropGroup.Text = "Wheel";
            // 
            // wheelPressure
            // 
            this.wheelPressure.AutoSize = true;
            this.wheelPressure.Location = new System.Drawing.Point(3, 20);
            this.wheelPressure.Name = "wheelPressure";
            this.wheelPressure.Size = new System.Drawing.Size(65, 17);
            this.wheelPressure.TabIndex = 7;
            this.wheelPressure.Text = "Pressure";
            // 
            // wheelPressureView
            // 
            this.wheelPressureView.Location = new System.Drawing.Point(6, 40);
            this.wheelPressureView.Name = "wheelPressureView";
            this.wheelPressureView.Size = new System.Drawing.Size(79, 20);
            this.wheelPressureView.TabIndex = 24;
            this.wheelPressureView.UseCompatibleStateImageBehavior = false;
            // 
            // vehicleDisplayerWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 554);
            this.Controls.Add(this.wheelPropGroup);
            this.Controls.Add(this.doorPropGroup);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.exitSearchButt);
            this.Controls.Add(this.addCarButt);
            this.Controls.Add(this.updateCarButt);
            this.Controls.Add(this.searchCarButt);
            this.Controls.Add(this.pictureBoxCar);
            this.Controls.Add(this.enrollmentView);
            this.Controls.Add(this.carColorView);
            this.Controls.Add(this.goToLastButt);
            this.Controls.Add(this.goToNextButt);
            this.Controls.Add(this.goToPreviousButt);
            this.Controls.Add(this.goToFirstButt);
            this.Controls.Add(this.storageTabButt);
            this.Controls.Add(this.carListView);
            this.Controls.Add(this.wheelListView);
            this.Controls.Add(this.doorListView);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "vehicleDisplayerWin";
            this.Text = "vehicleDisplayerWin";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.doorPropGroup.ResumeLayout(false);
            this.doorPropGroup.PerformLayout();
            this.wheelPropGroup.ResumeLayout(false);
            this.wheelPropGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem almacénToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vehículoToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListView doorListView;
        private System.Windows.Forms.ListView wheelListView;
        private System.Windows.Forms.ListView carListView;
        private System.Windows.Forms.Button storageTabButt;
        private System.Windows.Forms.Button goToFirstButt;
        private System.Windows.Forms.Button goToPreviousButt;
        private System.Windows.Forms.Button goToNextButt;
        private System.Windows.Forms.Button goToLastButt;
        private System.Windows.Forms.ListView horsePowerEngineView;
        private System.Windows.Forms.ListView startedEngineView;
        private System.Windows.Forms.ListView carColorView;
        private System.Windows.Forms.ListView enrollmentView;
        private System.Windows.Forms.PictureBox pictureBoxCar;
        private System.Windows.Forms.Button searchCarButt;
        private System.Windows.Forms.Button updateCarButt;
        private System.Windows.Forms.Button addCarButt;
        private System.Windows.Forms.Button exitSearchButt;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox doorPropGroup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView doorOpenedView;
        private System.Windows.Forms.GroupBox wheelPropGroup;
        private System.Windows.Forms.Label wheelPressure;
        private System.Windows.Forms.ListView wheelPressureView;
    }
}