using CarManagement.Core.Services;
using System.Windows.Forms;

namespace WinCarManager
{
    partial class VehicleStorageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private readonly IVehicleStorage vehicleStorage;
        private Button buttonAdd;
        private Button buttonModify;
        private Button buttonCancel;
        private Button buttonSave;
        private Button buttonDelete;
        private Button buttonFirst;
        private Button buttonLatest;
        private Button buttonPrevious;
        private Button buttonNext;
        private TextBox textEnrollment;
        private TextBox textColor;
        private TextBox texHorsePower;
        private TextBox texStarted;
        //private TextBox textDoors;
        //private TextBox textWheels;

        public VehicleStorageForm(IVehicleStorage vehicleStorage)
        {
            this.vehicleStorage = vehicleStorage;
        }

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
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonFirst = new System.Windows.Forms.Button();
            this.buttonLatest = new System.Windows.Forms.Button();
            this.buttonModify = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.texHorsePower = new System.Windows.Forms.TextBox();
            this.texStarted = new System.Windows.Forms.TextBox();
            this.textColor = new System.Windows.Forms.TextBox();
            this.textEnrollment = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(23, 388);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 50);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(367, 388);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 50);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(515, 388);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 50);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete";
            // 
            // buttonFirst
            // 
            this.buttonFirst.Location = new System.Drawing.Point(171, 388);
            this.buttonFirst.Name = "buttonFirst";
            this.buttonFirst.Size = new System.Drawing.Size(50, 50);
            this.buttonFirst.TabIndex = 3;
            this.buttonFirst.Text = "|<-";
            // 
            // buttonLatest
            // 
            this.buttonLatest.Location = new System.Drawing.Point(318, 388);
            this.buttonLatest.Name = "buttonLatest";
            this.buttonLatest.Size = new System.Drawing.Size(50, 50);
            this.buttonLatest.TabIndex = 4;
            this.buttonLatest.Text = "->|";
            // 
            // buttonModify
            // 
            this.buttonModify.Location = new System.Drawing.Point(97, 388);
            this.buttonModify.Name = "buttonModify";
            this.buttonModify.Size = new System.Drawing.Size(75, 50);
            this.buttonModify.TabIndex = 5;
            this.buttonModify.Text = "Modify";
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(269, 388);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(50, 50);
            this.buttonNext.TabIndex = 6;
            this.buttonNext.Text = "->";
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Location = new System.Drawing.Point(220, 388);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(50, 50);
            this.buttonPrevious.TabIndex = 7;
            this.buttonPrevious.Text = "<-";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(441, 388);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 50);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Save";
            // 
            // texHorsePower
            // 
            this.texHorsePower.Location = new System.Drawing.Point(570, 182);
            this.texHorsePower.Name = "texHorsePower";
            this.texHorsePower.Size = new System.Drawing.Size(100, 22);
            this.texHorsePower.TabIndex = 9;
            // 
            // texStarted
            // 
            this.texStarted.Location = new System.Drawing.Point(318, 182);
            this.texStarted.Name = "texStarted";
            this.texStarted.Size = new System.Drawing.Size(100, 22);
            this.texStarted.TabIndex = 10;
            // 
            // textColor
            // 
            this.textColor.Location = new System.Drawing.Point(34, 182);
            this.textColor.Name = "textColor";
            this.textColor.Size = new System.Drawing.Size(100, 22);
            this.textColor.TabIndex = 11;
            // 
            // textEnrollment
            // 
            this.textEnrollment.Location = new System.Drawing.Point(196, 99);
            this.textEnrollment.Name = "textEnrollment";
            this.textEnrollment.Size = new System.Drawing.Size(100, 22);
            this.textEnrollment.TabIndex = 12;
            // 
            // VehicleStorageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonFirst);
            this.Controls.Add(this.buttonLatest);
            this.Controls.Add(this.buttonModify);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.texHorsePower);
            this.Controls.Add(this.texStarted);
            this.Controls.Add(this.textColor);
            this.Controls.Add(this.textEnrollment);
            this.Name = "VehicleStorageForm";
            this.Text = "VehicleStorageForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /*private void InitializeComponent()
        {
            
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vehiclesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1067, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // vehiclesToolStripMenuItem
            // 
            this.vehiclesToolStripMenuItem.Name = "vehiclesToolStripMenuItem";
            this.vehiclesToolStripMenuItem.Size = new System.Drawing.Size(83, 24);
            this.vehiclesToolStripMenuItem.Text = "Vehicles...";
            this.vehiclesToolStripMenuItem.Click += new System.EventHandler(this.vehiclesToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }*/
    }
}