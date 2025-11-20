using System;
using System.Data;
using System.Windows.Forms;
using red_framework; // This is Important 

namespace OOP_Framework
{
    public partial class Form1 : Form
    {

        // selected id - use in Update & Delete - this value will change once you click/select on DataGridView/ListView Table UI
        private int _selectedId = 0; 

        public Form1()
        {
            InitializeComponent();
        }

        void clearInputs()
        {
            // Clear all Inputs
            txtIDNumber.Clear();
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            txtContactNumber.Clear();
            dtpBirthday.Value = DateTime.Now;
            cbProgramName.SelectedIndex = 0;
            _selectedId = 0; // reset selected id
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            var db = AppDb.Instance; // This is important, to be able to use the Databse Class and Methods

            if (_selectedId == 0) // if no selected records, then SAVE
            {
                var ok = db.Save("Students", 
                    new { 
                        Id_Number = txtIDNumber.Text, 
                        First_Name = txtFirstName.Text, 
                        Middle_name = txtMiddleName.Text, 
                        Last_Name = txtLastName.Text, 
                        Contact_Number = txtContactNumber.Text, 
                        Birthday = dtpBirthday.Value,
                        Program_Name = cbProgramName.SelectedItem
                    });
                MessageBox.Show(ok ? "Successfully Saved." : "Insert failed.");

                //or 
                /* 
                var res = db.CUD(
                    "INSERT INTO Students (First_Name, Last_Name) VALUES (@First, @Last)",
                    new { First_Name = txtFirstName.Text, Last_Name = txtLastName.Text }
                );
                MessageBox.Show(res > 0 ? "Successfully Saved." : "Insert failed.");
                */
            }
            else // if have selected records, then UPDATE
            {
                var updateok = db.Update("Students", 
                    new { 
                        Id = _selectedId,
                        Id_Number = txtIDNumber.Text,
                        First_Name = txtFirstName.Text,
                        Middle_name = txtMiddleName.Text,
                        Last_Name = txtLastName.Text,
                        Contact_Number = txtContactNumber.Text,
                        Birthday = dtpBirthday.Value,
                        Program_Name = cbProgramName.SelectedItem
                    }
                    //, "primaryKeyName" // if your primary key's name is not "Id"
                );

                MessageBox.Show(updateok ? "Successfully Updated." : "Update failed.");

                // or
                /*
                var res = db.CUD(
                    "UPDATE Students SET First_Name=@First WHERE Id=@Id",
                    new { First_Name = txtFirstName.Text, Id = _selectedId }
                );
                MessageBox.Show(res > 0 ? "Successfully Saved." : "Insert failed.");
                */
            }

            loadStudentRecords(); // METHOD to load all records to a DataGridView
            clearInputs(); // METHOD to clear all fields

        }

        void loadStudentRecords()
        {
            var db = AppDb.Instance;

            // load all records to a DataGridView
            db.Table(
                "SELECT Id, Id_Number, First_Name, Middle_name, Last_Name, Contact_Number, Birthday, Program_Name FROM Students ORDER BY Id DESC",
                dgvStudent, // your DataGridView name
                header: new[] {"Id", "ID Number", "First Name", "Middle Name", "Last Name", "Contact", "Birthday", "Program" } // This is the header displayed on DataGridView UI
            );

            dgvStudent.Columns["Id"].Visible = false; // hide primary key (the Id is belong to the Records but Hidden on UI)

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadStudentRecords(); // load all records once the form is apeared/loaded

            // Load Program in a ComboBox
            var db = AppDb.Instance;
            db.list(
                    "SELECT Program_Name FROM Program", 
                    cbProgramName // comboBox name
                );
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return; //some validations

            var drv = dgvStudent.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (drv == null) return;

            // assign Id (primary key) on a private property/variable - use in UPDATE and DELETE
            // Note: the Id (primary key) is belongs to the records (DataGridView), but hidden on UI
            _selectedId = (int)drv[0]; //index 0 is hidden (Id)

            txtIDNumber.Text = drv[1]?.ToString() ?? "";            // first column show on DataGridView (ID Number) - index 1
            txtFirstName.Text = drv[2]?.ToString() ?? "";           // First Name
            txtMiddleName.Text = drv[3]?.ToString() ?? "";          // Middle name
            txtLastName.Text = drv[4]?.ToString() ?? "";            // Last Name
            txtContactNumber.Text = drv[5]?.ToString() ?? "";       // Contact Number
            dtpBirthday.Value = Convert.ToDateTime(drv[6]);         // Birthday
            cbProgramName.SelectedItem = drv[7]?.ToString() ?? "";  //Program Name

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //if accedentally clicked the delete button even no selected records (other approach: disable button if no selected)
            if(_selectedId == 0) { MessageBox.Show("No record selected."); return; }

            // Confirmation dialog
            var confirmResult = MessageBox.Show(
                "Are you sure you want to delete this record?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult == DialogResult.Yes)
            {
                var db = AppDb.Instance;
                var ok = db.Delete("Students", new { Id = _selectedId });
                MessageBox.Show(ok ? "Successfully Deleted." : "Delete failed.");

                // or
                /*
                var res = db.CUD(
                    "DELETE FROM Students WHERE Id=@Id",
                    new { Id = _selectedId }
                );
                MessageBox.Show(res > 0 ? "Successfully Saved." : "Insert failed.");
                */

                loadStudentRecords();
                clearInputs();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var db = AppDb.Instance;

            // Search LIKE
            var results = db.Search(
                    "Students", // table
                    new[] { "First_Name", "Last_Name", "Id_Number" }, // columns where you search the keys/records
                    txtSearchInput.Text // search input box (text box)
                );

            // Fill the results to your DataGridView
            db.Table(
                results, 
                dgvStudent, 
                header: new[] { "Id", "ID Number", "First Name", "Middle Name", "Last Name", "Contact", "Birthday", "Program" }
            );

            dgvStudent.Columns["Id"].Visible = false; // hide primary key

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearInputs(); // clear Inputs and Reset
        }


    }
}

/**
 * ======DATABASE===========
 * NOTE: 
 * Every table has a primary key (e.g. Id). Set it to "Auto Increment". (Properties: IsIdentity = Yes)
 * The column Id (primary key) is not belong/use inside the Insert Into statement becuase it will automatically create a value
 * The column Id (primary key) will use in Update and Delete statement
 * The column Id (primary key) is Hidden inside DataGridView/ListView/Table UI
 * Please see/study the flow and the source code of this sample project
 * 
 * ==Table Students==
 * 

CREATE TABLE [dbo].[Students] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [Id_Number]      VARCHAR (50) NOT NULL,
    [First_Name]     VARCHAR (50) NOT NULL,
    [Middle_Name]    VARCHAR (50) NULL,
    [Last_Name]      VARCHAR (50) NOT NULL,
    [Contact_Number] BIGINT       NOT NULL,
    [BirthDay]       DATE         NOT NULL,
    [Program_Name]   VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


 * 
 * ==Table Program==
 * 

CREATE TABLE [dbo].[Program] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [Program_Name]        VARCHAR (50)  NULL,
    [Program_Description] VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

 * 
 * 
 */
