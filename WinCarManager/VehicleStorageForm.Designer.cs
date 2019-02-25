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
        private Button buttonAdd;
        private Button buttonModify;
        private Button buttonCancel;
        private Button buttonSave;
        private Button buttonDelete;
        private Button buttonFirst;
        private Button buttonLatest;
        private Button buttonPrevious;
        private Button buttonNext;
        private TextBox textSerial;
        private TextBox textNumber;
        private TextBox textColor;
        private TextBox texHorsePower;
        private TextBox texStarted;
        
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
            this.textSerial = new System.Windows.Forms.TextBox();
            this.textNumber = new System.Windows.Forms.TextBox();
            this.labelEnrollment = new System.Windows.Forms.Label();
            this.labelColor = new System.Windows.Forms.Label();
            this.groupEngine = new System.Windows.Forms.GroupBox();
            this.labelStarted = new System.Windows.Forms.Label();
            this.labelHorsePower = new System.Windows.Forms.Label();
            this.listDoors = new System.Windows.Forms.ListBox();
            this.listWheels = new System.Windows.Forms.ListBox();
            this.labelDoors = new System.Windows.Forms.Label();
            this.labelWheels = new System.Windows.Forms.Label();
            this.groupEngine.SuspendLayout();
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
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(515, 388);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 50);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
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
            this.texHorsePower.Location = new System.Drawing.Point(108, 39);
            this.texHorsePower.Name = "texHorsePower";
            this.texHorsePower.Size = new System.Drawing.Size(100, 22);
            this.texHorsePower.TabIndex = 9;
            // 
            // texStarted
            // 
            this.texStarted.Location = new System.Drawing.Point(108, 67);
            this.texStarted.Name = "texStarted";
            this.texStarted.Size = new System.Drawing.Size(100, 22);
            this.texStarted.TabIndex = 10;
            // 
            // textColor
            // 
            this.textColor.Location = new System.Drawing.Point(122, 104);
            this.textColor.Name = "textColor";
            this.textColor.Size = new System.Drawing.Size(100, 22);
            this.textColor.TabIndex = 11;
            // 
            // textSerial
            // 
            this.textSerial.Location = new System.Drawing.Point(122, 66);
            this.textSerial.Name = "textSerial";
            this.textSerial.Size = new System.Drawing.Size(65, 22);
            this.textSerial.TabIndex = 12;
            // 
            // textNumber
            // 
            this.textNumber.Location = new System.Drawing.Point(187, 66);
            this.textNumber.Name = "textNumber";
            this.textNumber.Size = new System.Drawing.Size(80, 22);
            this.textNumber.TabIndex = 13;
            // 
            // labelEnrollment
            // 
            this.labelEnrollment.AutoSize = true;
            this.labelEnrollment.Location = new System.Drawing.Point(41, 66);
            this.labelEnrollment.Name = "labelEnrollment";
            this.labelEnrollment.Size = new System.Drawing.Size(75, 17);
            this.labelEnrollment.TabIndex = 14;
            this.labelEnrollment.Text = "Enrollment";
            // 
            // labelColor
            // 
            this.labelColor.AutoSize = true;
            this.labelColor.Location = new System.Drawing.Point(41, 104);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(41, 17);
            this.labelColor.TabIndex = 15;
            this.labelColor.Text = "Color";
            // 
            // groupEngine
            // 
            this.groupEngine.Controls.Add(this.labelStarted);
            this.groupEngine.Controls.Add(this.labelHorsePower);
            this.groupEngine.Controls.Add(this.texStarted);
            this.groupEngine.Controls.Add(this.texHorsePower);
            this.groupEngine.Location = new System.Drawing.Point(44, 187);
            this.groupEngine.Name = "groupEngine";
            this.groupEngine.Size = new System.Drawing.Size(228, 121);
            this.groupEngine.TabIndex = 16;
            this.groupEngine.TabStop = false;
            this.groupEngine.Text = "Engine";
            // 
            // labelStarted
            // 
            this.labelStarted.AutoSize = true;
            this.labelStarted.Location = new System.Drawing.Point(10, 67);
            this.labelStarted.Name = "labelStarted";
            this.labelStarted.Size = new System.Drawing.Size(68, 17);
            this.labelStarted.TabIndex = 12;
            this.labelStarted.Text = "Is Started";
            // 
            // labelHorsePower
            // 
            this.labelHorsePower.AutoSize = true;
            this.labelHorsePower.Location = new System.Drawing.Point(7, 39);
            this.labelHorsePower.Name = "labelHorsePower";
            this.labelHorsePower.Size = new System.Drawing.Size(89, 17);
            this.labelHorsePower.TabIndex = 11;
            this.labelHorsePower.Text = "Horse Power";
            // 
            // listDoors
            // 
            this.listDoors.FormattingEnabled = true;
            this.listDoors.ItemHeight = 16;
            this.listDoors.Location = new System.Drawing.Point(410, 86);
            this.listDoors.Name = "listDoors";
            this.listDoors.Size = new System.Drawing.Size(180, 84);
            this.listDoors.TabIndex = 17;
            // 
            // listWheels
            // 
            this.listWheels.FormattingEnabled = true;
            this.listWheels.ItemHeight = 16;
            this.listWheels.Location = new System.Drawing.Point(410, 246);
            this.listWheels.Name = "listWheels";
            this.listWheels.Size = new System.Drawing.Size(131, 84);
            this.listWheels.TabIndex = 18;
            // 
            // labelDoors
            // 
            this.labelDoors.AutoSize = true;
            this.labelDoors.Location = new System.Drawing.Point(410, 66);
            this.labelDoors.Name = "labelDoors";
            this.labelDoors.Size = new System.Drawing.Size(46, 17);
            this.labelDoors.TabIndex = 19;
            this.labelDoors.Text = "Doors";
            // 
            // labelWheels
            // 
            this.labelWheels.AutoSize = true;
            this.labelWheels.Location = new System.Drawing.Point(413, 226);
            this.labelWheels.Name = "labelWheels";
            this.labelWheels.Size = new System.Drawing.Size(55, 17);
            this.labelWheels.TabIndex = 20;
            this.labelWheels.Text = "Wheels";
            // 
            // VehicleStorageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 450);
            this.Controls.Add(this.labelWheels);
            this.Controls.Add(this.labelDoors);
            this.Controls.Add(this.listWheels);
            this.Controls.Add(this.listDoors);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.labelEnrollment);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonFirst);
            this.Controls.Add(this.buttonLatest);
            this.Controls.Add(this.buttonModify);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textColor);
            this.Controls.Add(this.textSerial);
            this.Controls.Add(this.textNumber);
            this.Controls.Add(this.groupEngine);
            this.Name = "VehicleStorageForm";
            this.Text = "VehicleStorageForm";
            this.groupEngine.ResumeLayout(false);
            this.groupEngine.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label labelEnrollment;
        private Label labelColor;
        private GroupBox groupEngine;
        private ListBox listDoors;
        private ListBox listWheels;
        private Label labelDoors;
        private Label labelWheels;
        private Label labelStarted;
        private Label labelHorsePower;

        #endregion
                
    }
}