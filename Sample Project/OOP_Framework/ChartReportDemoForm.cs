using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using red_framework;

namespace OOP_Framework
{
    /// <summary>Demonstrates the built-in chart and report helpers without a database dependency.</summary>
    public sealed class ChartReportDemoForm : Form
    {
        private readonly DataTable _reportData;
        private readonly Chart _chart;
        private readonly ComboBox _chartType;

        public ChartReportDemoForm()
        {
            Text = "Charts and Reports";
            StartPosition = FormStartPosition.CenterParent;
            Width = 900;
            Height = 600;

            _reportData = CreateSampleData();
            _chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.White };
            _chartType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
            _chartType.Items.AddRange(new object[] { "Column", "Line", "Pie" });
            _chartType.SelectedIndex = 0;
            _chartType.SelectedIndexChanged += (sender, args) => BindChart();

            var toolbar = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 48,
                Padding = new Padding(8),
                WrapContents = false
            };
            var preview = new Button { Text = "Preview report", AutoSize = true };
            var print = new Button { Text = "Print report", AutoSize = true };
            preview.Click += (sender, args) => ReportPrinter.Preview(_reportData, "Monthly student totals", "Sample report generated with the built-in WinForms printer.");
            print.Click += (sender, args) => ReportPrinter.Print(_reportData, "Monthly student totals", "Sample report generated with the built-in WinForms printer.");
            toolbar.Controls.Add(_chartType);
            toolbar.Controls.Add(preview);
            toolbar.Controls.Add(print);
            Controls.Add(_chart);
            Controls.Add(toolbar);
            BindChart();
            FormClosed += (sender, args) => _reportData.Dispose();
        }

        private void BindChart()
        {
            var type = SeriesChartType.Column;
            if (_chartType.SelectedIndex == 1) type = SeriesChartType.Line;
            if (_chartType.SelectedIndex == 2) type = SeriesChartType.Pie;
            Charts.Bind(_chart, _reportData, "Month", "Students", "Students", type);
        }

        private static DataTable CreateSampleData()
        {
            var data = new DataTable();
            data.Columns.Add("Month", typeof(string));
            data.Columns.Add("Students", typeof(int));
            data.Rows.Add("January", 24);
            data.Rows.Add("February", 31);
            data.Rows.Add("March", 28);
            data.Rows.Add("April", 37);
            data.Rows.Add("May", 42);
            return data;
        }
    }
}
