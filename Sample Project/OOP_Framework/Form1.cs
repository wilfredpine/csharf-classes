using red_framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _selectedId = 0; // reset selected id
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            var db = AppDb.Instance;

            if (_selectedId == 0)
            {
                var ok = db.Save("Students", new { Id_Number = txtIDNumber.Text, First_Name = txtFirstName.Text, Middle_name = txtMiddleName.Text, Last_Name = txtLastName.Text, Contact_Number = txtContactNumber.Text, Birthday = dtpBirthday.Value });
                MessageBox.Show(ok ? "Successfully Saved." : "Insert failed.");
            }
            else
            {
                var updateok = db.Update("Students", new { Id = _selectedId, First_Name = txtFirstName.Text });
                MessageBox.Show(updateok ? "Successfully Updated." : "Update failed.");
            }

            loadStudentRecords();
            clearInputs();

        }

        void loadStudentRecords()
        {
            var db = AppDb.Instance;

            db.Table(
                "SELECT Id, Id_Number, First_Name, Middle_name, Last_Name, Contact_Number, Birthday FROM Students ORDER BY Id DESC",
                dgvStudent,
                header: new[] {"Id", "ID Number", "First Name", "Middle Name", "Last Name", "Contact", "Birthday" }
            );

            dgvStudent.Columns["Id"].Visible = false; // hide primary key

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadStudentRecords();
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
                var ok = AppDb.Instance.Delete("Students", new { Id = _selectedId });

                MessageBox.Show(ok ? "Successfully Deleted." : "Delete failed.");

                loadStudentRecords();
                clearInputs();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var db = AppDb.Instance;

            var results = db.Search("Students", new[] { "First_Name", "Last_Name", "Id_Number" }, txtSearchInput.Text);

            db.Table(
                results, 
                dgvStudent, 
                header: new[] { "Id", "ID Number", "First Name", "Middle Name", "Last Name", "Contact", "Birthday" }
            );

            dgvStudent.Columns["Id"].Visible = false; // hide primary key

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearInputs();
        }
    }
}
