
namespace OOP_Framework
{
    partial class Form1
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
            this.dgvStudent = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpBirthday = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtContactNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMiddleName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtIDNumber = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSearchInput = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudent)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvStudent
            // 
            this.dgvStudent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudent.Location = new System.Drawing.Point(363, 78);
            this.dgvStudent.Name = "dgvStudent";
            this.dgvStudent.Size = new System.Drawing.Size(588, 381);
            this.dgvStudent.TabIndex = 0;
            this.dgvStudent.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStudent_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpBirthday);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtContactNumber);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtLastName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtMiddleName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Controls.Add(this.txtIDNumber);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Location = new System.Drawing.Point(12, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 436);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Form";
            // 
            // dtpBirthday
            // 
            this.dtpBirthday.CustomFormat = "";
            this.dtpBirthday.Location = new System.Drawing.Point(6, 341);
            this.dtpBirthday.Name = "dtpBirthday";
            this.dtpBirthday.Size = new System.Drawing.Size(322, 24);
            this.dtpBirthday.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 320);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 18);
            this.label6.TabIndex = 13;
            this.label6.Text = "Birthday";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 259);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 18);
            this.label5.TabIndex = 12;
            this.label5.Text = "Contact Number";
            // 
            // txtContactNumber
            // 
            this.txtContactNumber.Location = new System.Drawing.Point(6, 280);
            this.txtContactNumber.Name = "txtContactNumber";
            this.txtContactNumber.Size = new System.Drawing.Size(322, 24);
            this.txtContactNumber.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "Last  Name";
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(6, 220);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(322, 24);
            this.txtLastName.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "Middle  Name";
            // 
            // txtMiddleName
            // 
            this.txtMiddleName.Location = new System.Drawing.Point(6, 163);
            this.txtMiddleName.Name = "txtMiddleName";
            this.txtMiddleName.Size = new System.Drawing.Size(322, 24);
            this.txtMiddleName.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "First  Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "ID Number";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(6, 103);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(322, 24);
            this.txtFirstName.TabIndex = 4;
            // 
            // txtIDNumber
            // 
            this.txtIDNumber.Location = new System.Drawing.Point(6, 46);
            this.txtIDNumber.Name = "txtIDNumber";
            this.txtIDNumber.Size = new System.Drawing.Size(322, 24);
            this.txtIDNumber.TabIndex = 3;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(253, 380);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 35);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(172, 380);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 35);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Cancel";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(91, 380);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 35);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(363, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 18);
            this.label7.TabIndex = 16;
            this.label7.Text = "Search Input";
            // 
            // txtSearchInput
            // 
            this.txtSearchInput.Location = new System.Drawing.Point(363, 48);
            this.txtSearchInput.Name = "txtSearchInput";
            this.txtSearchInput.Size = new System.Drawing.Size(507, 24);
            this.txtSearchInput.TabIndex = 15;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(876, 48);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 24);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 478);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtSearchInput);
            this.Controls.Add(this.dgvStudent);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudent)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvStudent;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpBirthday;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtContactNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMiddleName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtIDNumber;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSearchInput;
        private System.Windows.Forms.Button btnSearch;
    }
}

