using System;
using System.Data;
using System.Windows.Forms;
using red_framework;

namespace OOP_Framework
{
    public partial class Form1 : Form
    {
        private int _selectedId;

        public Form1()
        {
            InitializeComponent();
            AddDemoButton();
        }

        private void AddDemoButton()
        {
            var button = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new System.Drawing.Point(780, 10),
                Name = "btnChartReportDemo",
                Size = new System.Drawing.Size(171, 30),
                Text = "Charts and Reports"
            };
            button.Click += (sender, args) => { using (var demo = new ChartReportDemoForm()) demo.ShowDialog(this); };
            Controls.Add(button);
            button.BringToFront();
        }

        private object NewStudentData()
        {
            return new
            {
                Id_Number = txtIDNumber.Text,
                First_Name = txtFirstName.Text,
                Middle_name = txtMiddleName.Text,
                Last_Name = txtLastName.Text,
                Contact_Number = txtContactNumber.Text,
                Birthday = dtpBirthday.Value,
                Program_Name = cbProgramName.SelectedItem
            };
        }

        private object UpdatedStudentData()
        {
            return new
            {
                Id = _selectedId,
                Id_Number = txtIDNumber.Text,
                First_Name = txtFirstName.Text,
                Middle_name = txtMiddleName.Text,
                Last_Name = txtLastName.Text,
                Contact_Number = txtContactNumber.Text,
                Birthday = dtpBirthday.Value,
                Program_Name = cbProgramName.SelectedItem
            };
        }

        private void ClearInputs()
        {
            FormControls.ClearInputs(groupBox1);
            _selectedId = 0;
        }

        private void LoadStudentRecords()
        {
            AppDb.Instance.Table(
                "SELECT Id, Id_Number, First_Name, Middle_name, Last_Name, Contact_Number, Birthday, Program_Name FROM Students ORDER BY Id DESC",
                dgvStudent,
                header: new[] { "Id", "ID Number", "First Name", "Middle Name", "Last Name", "Contact", "Birthday", "Program" });
            HideIdColumn();
        }

        private void HideIdColumn()
        {
            if (dgvStudent.Columns.Contains("Id")) dgvStudent.Columns["Id"].Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudentRecords();
            AppDb.Instance.list("SELECT Program_Name FROM Program", cbProgramName);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var missing = FormControls.FirstRequired(txtIDNumber, txtFirstName, txtLastName);
            if (missing != null)
            {
                missing.Focus();
                MessageBox.Show(missing.Name + " is required.");
                return;
            }

            var ok = _selectedId == 0
                ? AppDb.Instance.Save("Students", NewStudentData())
                : AppDb.Instance.Update("Students", UpdatedStudentData());
            MessageBox.Show(ok ? "Student saved." : "The record could not be saved.");
            LoadStudentRecords();
            ClearInputs();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedId == 0) { MessageBox.Show("Select a record first."); return; }
            if (MessageBox.Show("Delete this student?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            var deleted = AppDb.Instance.Delete("Students", new { Id = _selectedId });
            MessageBox.Show(deleted ? "Student deleted." : "The record could not be deleted.");
            LoadStudentRecords();
            ClearInputs();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var rows = AppDb.Instance.Search("Students", new[] { "First_Name", "Last_Name", "Id_Number" }, txtSearchInput.Text);
            AppDb.Instance.Table(rows, dgvStudent,
                header: new[] { "Id", "ID Number", "First Name", "Middle Name", "Last Name", "Contact", "Birthday", "Program" });
            HideIdColumn();
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvStudent.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (row == null) return;

            _selectedId = Convert.ToInt32(row[0]);
            txtIDNumber.Text = row[1]?.ToString() ?? string.Empty;
            txtFirstName.Text = row[2]?.ToString() ?? string.Empty;
            txtMiddleName.Text = row[3]?.ToString() ?? string.Empty;
            txtLastName.Text = row[4]?.ToString() ?? string.Empty;
            txtContactNumber.Text = row[5]?.ToString() ?? string.Empty;
            dtpBirthday.Value = Convert.ToDateTime(row[6]);
            cbProgramName.SelectedItem = row[7]?.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}
