<div>

# C# WinForms Classes

A quick, practical guide to database CRUD, reusable form controls,
charts, and printable reports. Works with .NET Framework 4.7.2 and
standard or third-party WinForms controls derived from Control.

</div>

<div role="main">

**In this guide**

- [1. Installation/config](#setup)
- [2. CRUD and DataGridView](#crud)
- [3. All methods](#methods)
- [4. Other helpers](#helpers)
- [5. Complete implementation](#implementation)
- [Additional short examples](#more-examples)
- [Troubleshooting](#troubleshooting)

<div id="setup" class="section card">

## Part 1 — Step 1: Installation and configuration

1.  In Visual Studio, create a **Windows Forms App (.NET Framework)**
    and choose **.NET Framework 4.7.2**.
2.  Add a folder named `Classes`. Copy the files from the archive's root
    `Classes` folder into it: `AppDb.cs`, `IDatabase.cs`,
    `SqlDatabase.cs`, `FormControls.cs`, `Charts.cs`, and
    `ReportPrinter.cs`.
3.  Add framework references if Visual Studio has not added them:
    `System.Configuration`, `System.Data`, `System.Drawing`,
    `System.Windows.Forms`, and
    `System.Windows.Forms.DataVisualization`.
4.  Create a SQL Server LocalDB database named `OOPFrameworkDemo`. Run
    [docs/schema.sql](docs/schema.sql) to create the sample tables.
5.  Set the `AppDB` connection string in `App.config`. The name must be
    exactly `AppDB`:

<!-- -->

    <connectionStrings>
      <add name="AppDB"
           connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OOPFrameworkDemo;Integrated Security=True;Connect Timeout=30" />
    </connectionStrings>

The sample uses SQL Server LocalDB. For SQL Server Express or a remote
SQL Server, replace the server and database names in the connection
string. Keep the connection string name `AppDB`.

### Installation checklist

1.  Open [docs/schema.sql](docs/schema.sql) in SQL Server Management
    Studio or Visual Studio SQL Server Object Explorer and execute it.
2.  Confirm the `Program` and `Students` tables exist.
3.  Open `Sample Project/OOP_Framework.sln` to see the working CRUD and
    chart/report examples.
4.  Register cleanup once at application startup or form startup.

At application exit, close the shared database connection once:

    Application.ApplicationExit += (_, __) => AppDb.Dispose();

</div>

<div id="crud" class="section card">

## Part 2 — Step 2: Save, update, delete, and show records

Use `AppDb.Instance`. Pass values as an object; database values are
parameterized by the helper.

    // Save
    bool saved = AppDb.Instance.Save("Students", new { Id_Number = txtIDNumber.Text, First_Name = txtFirstName.Text, Middle_name = txtMiddleName.Text, Last_Name = txtLastName.Text, Contact_Number = txtContactNumber.Text, Birthday = dtpBirthday.Value, Program_Name = cbProgramName.SelectedItem });

    // Update by Id
    bool updated = AppDb.Instance.Update("Students", new { Id = selectedId, First_Name = txtFirstName.Text });

    // Delete by Id
    bool deleted = AppDb.Instance.Delete("Students", new { Id = selectedId });

    // Bind a table to a grid
    AppDb.Instance.Table("SELECT Id, First_Name, Last_Name FROM Students ORDER BY Id DESC", dgvStudents);

### Parameterized SQL for a custom query

    var rows = AppDb.Instance.TableData(
        "SELECT Id, First_Name, Last_Name FROM Students WHERE Id_Number = @IdNumber",
        new { IdNumber = txtIDNumber.Text });
    dgvStudents.DataSource = rows;

Use `Save`, `Update`, or `Delete` for common CRUD. Use `CUD` and
`TableData` when a custom query is needed.

### Example usage for different controls

    // DataGridView
    AppDb.Instance.Table("SELECT * FROM Students", dataGridView1);

    // ListView
    AppDb.Instance.Table("SELECT * FROM Students", listView1);

    // ComboBox: values from the first query column are added to the list
    AppDb.Instance.list("SELECT Program_Name FROM Program ORDER BY Program_Name", comboBox1);

    // TextBox, Label, or another control: read the DataTable and assign a value
    var first = AppDb.Instance.TableData("SELECT TOP 1 First_Name FROM Students ORDER BY Id");
    if (first.Rows.Count > 0) textBox1.Text = Convert.ToString(first.Rows[0]["First_Name"]);

### Separate search example using TableData

    var data = AppDb.Instance.TableData(
        "SELECT * FROM Students WHERE First_Name LIKE @keyword OR Last_Name LIKE @keyword",
        new { keyword = "%" + txtSearch.Text.Trim() + "%" });

    AppDb.Instance.Table(data, dataGridView1);

### Parameterized grid search with custom column headers

    AppDb.Instance.Table(
        "SELECT Id, First_Name, Last_Name, Program_Name FROM Students WHERE Program_Name = @program",
        new { program = comboBox1.Text },
        dataGridView1,
        new[] { "ID", "First Name", "Last Name", "Program" });

</div>

<div id="controls" class="section card">

## Part 2 — Step 3: Control examples

Control helpers accept `System.Windows.Forms.Control`. Standard controls
and vendor controls that expose their input through `Text` can use the
same methods.

    // Clear inputs in a panel, group, or form; labels and buttons stay unchanged
    FormControls.ClearInputs(groupBox1);

    // Validate required fields and focus the first empty one
    var missing = FormControls.FirstRequired(txtIDNumber, txtFirstName, txtLastName);
    if (missing != null) { missing.Focus(); MessageBox.Show(missing.Name + " is required."); return; }

    // Check one control
    if (!FormControls.Required(gunaTextBoxName, "Name")) return;

`ClearInputs` recursively handles text boxes, combo boxes, dates, check
boxes, radio buttons, and numeric inputs. For a vendor control that
inherits directly from `Control`, call
`FormControls.ClearText(vendorInput)` when its input value is exposed by
`Text`. Unsupported controls stay unchanged.

</div>

<div id="charts" class="section card">

## Part 2 — Step 4: Chart example

1.  Drag a WinForms `Chart` control from the toolbox onto your form.
2.  Load a `DataTable` with a category column and a numeric value
    column.
3.  Bind a column, line, or pie series:

<!-- -->

    var data = AppDb.Instance.TableData("SELECT Program_Name, COUNT(*) AS StudentCount FROM Students GROUP BY Program_Name");
    Charts.Bind(chart1, data, "Program_Name", "StudentCount", "Students", SeriesChartType.Column);

    // Change chart type
    Charts.Bind(chart1, data, "Program_Name", "StudentCount", "Trend", SeriesChartType.Line);

The helper replaces the previous series each time. It returns `null`
when the table is null or empty. Add
`using System.Windows.Forms.DataVisualization.Charting;` for
`SeriesChartType`.

</div>

<div id="reports" class="section card">

## Part 2 — Step 5: Report example

Pass a `DataTable` to the built-in report printer. Preview opens the
Windows print preview; Print opens the printer selection dialog.

    var report = AppDb.Instance.TableData("SELECT Id, Id_Number, First_Name, Last_Name FROM Students ORDER BY Id");
    ReportPrinter.Preview(report, "Student List", "First Semester 2026–2027");

    // Send to a selected printer
    ReportPrinter.Print(report, "Student List", "First Semester 2026–2027");

Reports include a title, optional subtitle, shaded column headings,
alternating rows, repeated headings on each page, margins, and page
numbers. Tables with six or more columns print in landscape orientation.
Wide columns shrink to fit the printable page.

</div>

<div id="methods" class="section card">

## Part 3 — Method definitions and examples

The library is synchronous and targets SQL Server. Parameters named
`parameters` accept an anonymous object or
`IDictionary<string, object>`; values become SQL parameters
automatically. `CommandTimeoutSeconds` controls the default timeout, and
the optional `Logger` action receives command text for diagnostics.

### AppDb

| Member            | Parameters | Definition and sample                                                                          |
|-------------------|------------|------------------------------------------------------------------------------------------------|
| `AppDb.Instance`  | None       | Returns the shared `IDatabase`. Use it from every form: `AppDb.Instance.Table(query, dgv);`    |
| `AppDb.Dispose()` | None       | Closes the shared connection. Use `Application.ApplicationExit += (_, __) => AppDb.Dispose();` |

### Core SQL and CRUD methods

| Method                                  | Definition                                                              | Sample                                                                                               |
|-----------------------------------------|-------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------|
| `SqlDatabase(connectionString)`         | Creates and opens a SQL Server connection.                              | Normally created automatically by `AppDb.Instance`.                                                  |
| `Close()` / `Dispose()`                 | Closes and releases the current connection.                             | Prefer `AppDb.Dispose()` at application exit.                                                        |
| `CUD(query, parameters)`                | Executes INSERT, UPDATE, or DELETE and returns affected rows.           | `AppDb.Instance.CUD("UPDATE Students SET Last_Name=@name WHERE Id=@id", new { name="Pine", id=1 });` |
| `Scalar(query, parameters)`             | Returns the first value from the first result row.                      | `var total = AppDb.Instance.Scalar("SELECT COUNT(*) FROM Students");`                                |
| `TableData(query, parameters)`          | Returns a `DataTable`.                                                  | `var rows = AppDb.Instance.TableData("SELECT * FROM Students WHERE Id=@id", new { id=1 });`          |
| `select(query)`                         | Returns a `SqlDataReader`; use `TableData` for most forms.              | `using (var reader = AppDb.Instance.select("SELECT Id FROM Students")) { }`                          |
| `Save(table, data)`                     | Inserts object properties as columns and returns `bool`.                | `AppDb.Instance.Save("Students", new { First_Name="Ana", Last_Name="Cruz" });`                       |
| `Update(table, data, keyColumn)`        | Updates object properties except the key; key defaults to `Id`.         | `AppDb.Instance.Update("Students", new { Id=1, Last_Name="Cruz" });`                                 |
| `Delete(table, data, keyColumn)`        | Deletes by the key property; key defaults to `Id`.                      | `AppDb.Instance.Delete("Students", new { Id=1 });`                                                   |
| `Max/Min(table, column)`                | Returns the maximum or minimum value.                                   | `var highest = AppDb.Instance.Max("Students", "Id");`                                                |
| `Count(table, condition)`               | Returns row count; condition is an application-controlled SQL fragment. | `var count = AppDb.Instance.Count("Students");`                                                      |
| `Exists(table, condition)`              | Returns true when at least one row matches.                             | `if (AppDb.Instance.Exists("Students", "Id_Number = '1001'")) { }`                                   |
| `GetById(table, id, keyColumn)`         | Returns one `DataRow` or `null`.                                        | `var row = AppDb.Instance.GetById("Students", 1);`                                                   |
| `Search(table, columns, keyword)`       | Searches listed columns using a parameterized `LIKE`.                   | `var rows = AppDb.Instance.Search("Students", new[] { "First_Name", "Last_Name" }, "Ana");`          |
| `Sum/Average(table, column, condition)` | Returns decimal aggregate; empty results return 0.                      | `var average = AppDb.Instance.Average("Scores", "Points");`                                          |
| `GetTop(table, top, orderBy)`           | Returns the first rows using a controlled order expression.             | `var latest = AppDb.Instance.GetTop("Students", 10, "Id DESC");`                                     |

### Paging, schema, stored procedures, and transactions

| Method                                                       | Parameters                                | Definition and sample                                                                                                            |
|--------------------------------------------------------------|-------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------|
| `Paginate(table, pageNumber, pageSize, orderBy)`             | 1-based page, page size, controlled order | `var page = AppDb.Instance.Paginate("Students", 2, 20);`                                                                         |
| `PaginateWithCount(...)`                                     | Page, size, optional where/order          | `var result = AppDb.Instance.PaginateWithCount("Students", 1, 20);`                                                              |
| `GetDistinct(table, column)`                                 | Table and column                          | `var programs = AppDb.Instance.GetDistinct("Students", "Program_Name");`                                                         |
| `Join(table1, table2, onCondition, columns, where)`          | Tables and controlled SQL fragments       | `var rows = AppDb.Instance.Join("Students", "Program", "Students.Program_Name = Program.Program_Name");`                         |
| `ExecStoredProcedure(procName, parameters, timeoutSeconds)`  | Name, values, optional timeout            | `AppDb.Instance.ExecStoredProcedure("dbo.SaveStudent", new { Id=1 });`                                                           |
| `QueryStoredProcedure(procName, parameters, timeoutSeconds)` | Name, values, optional timeout            | `var rows = AppDb.Instance.QueryStoredProcedure("dbo.GetStudents");`                                                             |
| `ExecuteTransaction(commands)`                               | Query/parameter tuples                    | `AppDb.Instance.ExecuteTransaction(("DELETE FROM A WHERE Id=@id", new { id=1 }), ("DELETE FROM B WHERE Id=@id", new { id=1 }));` |
| `ExecuteTransaction(work)`                                   | `Func<SqlTransaction,bool>`               | Runs custom work and commits only when it returns true.                                                                          |
| `ExecuteWithRetry(operation, maxRetries, delayMs)`           | Function, retry count, initial delay      | `var value = AppDb.Instance.ExecuteWithRetry(() => AppDb.Instance.Ping());`                                                      |
| `GetSchemaTables/Columns`                                    | Optional table for columns                | Returns SQL Server metadata tables.                                                                                              |
| `ColumnExists(table, column)`                                | Table and column                          | `if (AppDb.Instance.ColumnExists("Students", "Birthday")) { }`                                                                   |
| `Ping()`                                                     | None                                      | `if (!AppDb.Instance.Ping()) MessageBox.Show("Database unavailable");`                                                           |
| `Backup(filePath)`                                           | SQL Server-accessible path                | Attempts a database backup and returns `bool`.                                                                                   |
| `BulkInsert(table, data)`                                    | Table and matching `DataTable`            | Uses `SqlBulkCopy` and returns `bool`.                                                                                           |
| `ClearTable(table)`                                          | Controlled table name                     | Deletes every row and returns `bool`.                                                                                            |

### Upsert, audit, POCO, and WinForms methods

| Method                                                    | Definition and parameters                                      | Sample                                                                                                       |
|-----------------------------------------------------------|----------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------|
| `Upsert(table, keyColumn, data)`                          | Updates by key or inserts when missing.                        | `AppDb.Instance.Upsert("Students", "Id", new { Id=1, Last_Name="Cruz" });`                                   |
| `SoftDelete/Restore(table, id, keyColumn, deletedColumn)` | Sets the controlled flag column to 1 or 0.                     | `AppDb.Instance.SoftDelete("Students", 1, "Id", "IsDeleted");`                                               |
| `Touch(table, id, keyColumn, column)`                     | Sets a timestamp column to `SYSDATETIME()`.                    | `AppDb.Instance.Touch("Students", 1);`                                                                       |
| `QuerySingle<T> / QueryList<T>`                           | Maps result columns to matching POCO properties.               | `var student = AppDb.Instance.QuerySingle<StudentRow>("SELECT * FROM Students WHERE Id=@id", new { id=1 });` |
| `Table(DataTable, DataGridView, header)`                  | Binds existing data and applies grid settings.                 | `AppDb.Instance.Table(data, dgvStudents);`                                                                   |
| `Table(query, DataGridView, header)`                      | Queries and binds a grid.                                      | `AppDb.Instance.Table("SELECT * FROM Students", dgvStudents);`                                               |
| `Table(query, parameters, DataGridView, header)`          | Parameterized query and grid binding.                          | `AppDb.Instance.Table("SELECT * FROM Students WHERE Id=@id", new { id=1 }, dgvStudents);`                    |
| `Table(query, ListView, headers)`                         | Loads query rows into a details ListView.                      | `AppDb.Instance.Table("SELECT * FROM Students", lvStudents);`                                                |
| `list(query, ComboBox)`                                   | Clears and fills a DropDownList using the first result column. | `AppDb.Instance.list("SELECT Program_Name FROM Program", cbProgramName);`                                    |

</div>

<div id="helpers" class="section card">

## Part 4 — Other helpers and features

### Reusable control helpers

    FormControls.ClearInputs(groupBox1);
    FormControls.ClearText(gunaTextBoxName); // explicit vendor Control.Text input
    var missing = FormControls.FirstRequired(txtIDNumber, txtFirstName, txtLastName);
    if (missing != null) { missing.Focus(); return; }
    if (!FormControls.Required(txtFirstName, "First name")) return;

`ClearInputs` recursively handles TextBox, ComboBox, DateTimePicker,
CheckBox, RadioButton, and NumericUpDown. Labels, buttons, containers,
and unsupported custom controls remain unchanged. Use `ClearText` when a
vendor control inherits directly from `Control` and stores its input in
`Text`.

### Charts

    using System.Windows.Forms.DataVisualization.Charting;
    Charts.Bind(chart1, data, "Program_Name", "StudentCount", "Students", SeriesChartType.Column);
    Charts.Bind(chart1, data, "Program_Name", "StudentCount", "Trend", SeriesChartType.Line);
    Charts.Bind(chart1, data, "Program_Name", "StudentCount", "Share", SeriesChartType.Pie);

The chart helper clears previous series, creates a chart area when
needed, skips null rows, and returns `null` when the table has no usable
data.

### Reports and printing

    var report = AppDb.Instance.TableData("SELECT Id, Id_Number, First_Name, Last_Name FROM Students ORDER BY Id");
    ReportPrinter.Preview(report, "Student List", "First Semester 2026–2027");
    ReportPrinter.Print(report, "Student List", "First Semester 2026–2027");

The report uses Windows `PrintDocument` and includes a title, subtitle,
colored headings, alternating rows, repeated headings, margins, page
numbers, and landscape mode for six or more columns. No external
reporting application is required.

### POCO mapping

    public class StudentRow { public int Id { get; set; } public string First_Name { get; set; } public string Last_Name { get; set; } }
    var student = AppDb.Instance.QuerySingle<StudentRow>(
        "SELECT Id, First_Name, Last_Name FROM Students WHERE Id=@id", new { id = 1 });

<div class="best">

**Best practices**  
Always use parameterized queries to prevent SQL injection. Dispose
database connections on application exit. Use `AppDb.Instance` for
centralized DB access.

</div>

</div>

<div id="implementation" class="section card">

## Part 5 — Complete sample code implementation

This sample keeps event handlers short and uses the helpers for the
repeated work.

### Load grid and ComboBox

    private void Form1_Load(object sender, EventArgs e)
    {
        LoadStudents();
        AppDb.Instance.list("SELECT Program_Name FROM Program ORDER BY Program_Name", cbProgramName);
    }

    private void LoadStudents()
    {
        AppDb.Instance.Table(
            "SELECT Id, Id_Number, First_Name, Last_Name, Program_Name FROM Students ORDER BY Id DESC",
            dgvStudents,
            new[] { "ID", "ID Number", "First Name", "Last Name", "Program" });
        if (dgvStudents.Columns.Contains("Id")) dgvStudents.Columns["Id"].Visible = false;
    }

### Save or update

    private void btnSave_Click(object sender, EventArgs e)
    {
        var missing = FormControls.FirstRequired(txtIDNumber, txtFirstName, txtLastName);
        if (missing != null) { missing.Focus(); return; }
        var data = new { Id = _selectedId, Id_Number = txtIDNumber.Text, First_Name = txtFirstName.Text, Last_Name = txtLastName.Text, Contact_Number = txtContactNumber.Text, Birthday = dtpBirthday.Value, Program_Name = cbProgramName.SelectedItem };
        var ok = _selectedId == 0 ? AppDb.Instance.Save("Students", data) : AppDb.Instance.Update("Students", data);
        if (ok) { LoadStudents(); FormControls.ClearInputs(groupBox1); _selectedId = 0; }
    }

### Search, select, and delete

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        AppDb.Instance.Table("SELECT * FROM Students WHERE First_Name LIKE @keyword OR Last_Name LIKE @keyword", new { keyword = "%" + txtSearch.Text.Trim() + "%" }, dgvStudents);
    }

    private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dgvStudents.CurrentRow == null) return;
        _selectedId = Convert.ToInt32(dgvStudents.CurrentRow.Cells["Id"].Value);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (_selectedId != 0 && AppDb.Instance.Delete("Students", new { Id = _selectedId })) LoadStudents();
    }

### Chart and report button

    private void btnReports_Click(object sender, EventArgs e)
    {
        var data = AppDb.Instance.TableData("SELECT Program_Name, COUNT(*) AS StudentCount FROM Students GROUP BY Program_Name");
        Charts.Bind(chart1, data, "Program_Name", "StudentCount", "Students");
        ReportPrinter.Preview(AppDb.Instance.TableData("SELECT * FROM Students ORDER BY Id"), "Student List", "First Semester 2026–2027");
    }

</div>

<div id="more-examples" class="section card">

## Additional short implementations

### Count, exists, and get one row

    int total = AppDb.Instance.Count("Students");
    bool exists = AppDb.Instance.Exists("Students", "Id_Number = '1001'");
    DataRow row = AppDb.Instance.GetById("Students", selectedId);
    if (row != null) txtFirstName.Text = Convert.ToString(row["First_Name"]);

### Top rows and paging

    var latest = AppDb.Instance.GetTop("Students", 10, "Id DESC");
    var page = AppDb.Instance.Paginate("Students", pageNumber: 1, pageSize: 20, orderBy: "Id DESC");
    AppDb.Instance.Table(page, dataGridView1);

### Scalar value and custom update

    var totalStudents = AppDb.Instance.Scalar("SELECT COUNT(*) FROM Students");
    AppDb.Instance.CUD(
        "UPDATE Students SET Contact_Number = @contact WHERE Id = @id",
        new { contact = txtContact.Text, id = selectedId });

### Upsert and soft delete

    AppDb.Instance.Upsert("Students", "Id", new { Id = selectedId, First_Name = txtFirstName.Text, Last_Name = txtLastName.Text });
    AppDb.Instance.SoftDelete("Students", selectedId, "Id", "IsDeleted");
    AppDb.Instance.Restore("Students", selectedId, "Id", "IsDeleted");

### Stored procedure and POCO list

    var rows = AppDb.Instance.QueryStoredProcedure("dbo.GetStudents", new { Program_Name = "BSIT" });
    var students = AppDb.Instance.QueryList<StudentRow>("SELECT * FROM Students WHERE Program_Name = @program", new { program = "BSIT" });

</div>

<div id="troubleshooting" class="section card">

## Step 6 — Troubleshooting

- **Configuration error:** confirm the connection string name is exactly
  `AppDB` and the database exists.
- **Table or column not found:** run `docs/schema.sql` and match the
  names used in your query.
- **Chart is blank:** verify the category and value column names; the
  value column must contain numeric data.
- **No printer preview:** install or select a Windows printer. Preview
  and printing are Windows desktop features.
- **Third-party input is not cleared:** its value should map to
  `Control.Text`; otherwise clear it directly with that control's own
  API.

Open `Sample Project/OOP_Framework.sln` to see the CRUD form and the
**Charts and Reports** demo.

</div>

</div>

C# WinForms Classes • .NET Framework 4.7.2 • No external chart or report
package required
