using red_framework;
using System;
using System.Data;
using System.Windows.Forms;


namespace OOP_Framework
{
    public partial class Form1 : Form
    {

        private int _selectedId = 0; // selected id - use in Update & Delete

        public Form1()
        {
            InitializeComponent();
        }

        void clearInputs()
        {
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
            var db = AppDb.Instance;

            if (_selectedId == 0)
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
            else
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
                    });
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

            loadStudentRecords();
            clearInputs();

        }

        void loadStudentRecords()
        {
            var db = AppDb.Instance;

            db.Table(
                "SELECT Id, Id_Number, First_Name, Middle_name, Last_Name, Contact_Number, Birthday, Program_Name FROM Students ORDER BY Id DESC",
                dgvStudent,
                header: new[] {"Id", "ID Number", "First Name", "Middle Name", "Last Name", "Contact", "Birthday", "Program" }
            );

            dgvStudent.Columns["Id"].Visible = false; // hide primary key

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadStudentRecords();

            // Load Program in ComboBox
            var db = AppDb.Instance;
            db.list("SELECT Program_Name FROM Program", cbProgramName);
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return;

            var drv = dgvStudent.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (drv == null) return;

            _selectedId = (int)drv[0]; // assign id (primary key) - use in update and delete

            txtIDNumber.Text = drv[1]?.ToString() ?? "";
            txtFirstName.Text = drv[2]?.ToString() ?? "";
            txtMiddleName.Text = drv[3]?.ToString() ?? "";
            txtLastName.Text = drv[4]?.ToString() ?? "";
            txtContactNumber.Text = drv[5]?.ToString() ?? "";
            dtpBirthday.Value = Convert.ToDateTime(drv[6]);
            cbProgramName.SelectedItem = drv[7]?.ToString() ?? "";

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
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

            var results = db.Search(
                    "Students", 
                    new[] { "First_Name", "Last_Name", "Id_Number" }, 
                    txtSearchInput.Text
                );

            db.Table(
                results, 
                dgvStudent, 
                header: new[] { "Id", "ID Number", "First Name", "Middle Name", "Last Name", "Contact", "Birthday", "Program" }
            );

            dgvStudent.Columns["Id"].Visible = false; // hide primary key

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearInputs();
        }
    }
}
