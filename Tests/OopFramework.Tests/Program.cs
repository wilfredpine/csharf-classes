using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Printing;
using System.Windows.Forms;
using red_framework;

namespace OopFramework.Tests
{
    internal static class Program
    {
        [STAThread]
        private static int Main()
        {
            try
            {
                NestedInputsClearAndLabelsRemain();
                DerivedTextBoxCanBePassedAsControl();
                RequiredRejectsWhitespace();
                FirstRequiredReturnsFirstEmptyControl();
                ChartBindingCreatesAndReplacesSeries();
                ChartBindingSkipsNullAndEmptyData();
                ReportRendererPaginatesAndResets();
                ReportDocumentSetsPrintLayout();
                Console.WriteLine("All helper tests passed.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }
        }

        private static void NestedInputsClearAndLabelsRemain()
        {
            using (var root = new Panel())
            using (var nested = new GroupBox())
            using (var text = new TextBox { Text = "name" })
            using (var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList })
            using (var label = new Label { Text = "Keep this" })
            using (var button = new Button { Text = "Keep button" })
            using (var caption = new CustomCaption { Text = "Keep custom caption" })
            {
                combo.Items.Add("A"); combo.SelectedIndex = 0;
                nested.Controls.Add(text); nested.Controls.Add(combo); nested.Controls.Add(label); nested.Controls.Add(button); nested.Controls.Add(caption);
                root.Controls.Add(nested);
                FormControls.ClearInputs(root);
                Assert(text.Text == "", "TextBox should clear");
                Assert(combo.SelectedIndex == -1, "ComboBox selection should clear");
                Assert(label.Text == "Keep this", "Label should remain unchanged");
                Assert(button.Text == "Keep button", "Button should remain unchanged");
                Assert(caption.Text == "Keep custom caption", "unsupported custom controls should remain unchanged");
            }
        }

        private static void DerivedTextBoxCanBePassedAsControl()
        {
            using (var input = new CustomTextBox { Text = "third-party style" })
            {
                Control control = input;
                Assert(FormControls.Required(control, "Name"), "non-empty derived input should be valid");
                FormControls.ClearInputs(control);
                Assert(input.Text == "", "derived TextBox should clear");
            }
        }

        private static void RequiredRejectsWhitespace()
        {
            using (var input = new TextBox { Text = "   " })
                Assert(!FormControls.Required(input, "Name"), "whitespace-only input should be invalid");
        }

        private static void FirstRequiredReturnsFirstEmptyControl()
        {
            using (var first = new TextBox { Text = "ok" })
            using (var second = new TextBox { Text = "" })
            using (var third = new TextBox { Text = "also ok" })
                Assert(ReferenceEquals(FormControls.FirstRequired(first, second, third), second), "first invalid input should be returned");
        }

        private static void ChartBindingCreatesAndReplacesSeries()
        {
            using (var chart = new Chart())
            using (var data = new DataTable())
            {
                data.Columns.Add("Month", typeof(string));
                data.Columns.Add("Total", typeof(double));
                data.Rows.Add("Jan", 10d); data.Rows.Add("Feb", 15d);
                var column = Charts.Bind(chart, data, "Month", "Total", "Students");
                Assert(column != null && column.ChartType == SeriesChartType.Column, "column chart should be created");
                Assert(column.Points.Count == 2 && column.Points[1].YValues[0] == 15d, "numeric values should map to points");
                var line = Charts.Bind(chart, data, "Month", "Total", "Trend", SeriesChartType.Line);
                Assert(line.ChartType == SeriesChartType.Line && chart.Series.Count == 1, "new series should replace old series");
            }
        }

        private static void ChartBindingSkipsNullAndEmptyData()
        {
            using (var chart = new Chart())
            using (var empty = new DataTable())
            {
                Assert(Charts.Bind(chart, null, "Month", "Total", "None") == null, "null data should return no series");
                Assert(Charts.Bind(chart, empty, "Month", "Total", "None") == null, "empty data should return no series");
                Assert(chart.Series.Count == 0, "empty data should not leave a misleading series");
            }
        }

        private static void ReportRendererPaginatesAndResets()
        {
            using (var data = new DataTable())
            using (var bitmap = new Bitmap(320, 180))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                data.Columns.Add("Name", typeof(string));
                data.Columns.Add("Count", typeof(int));
                for (var i = 0; i < 40; i++) data.Rows.Add("Row " + i, i);
                var row = 0; var page = 1; var pages = 0;
                while (row < data.Rows.Count)
                {
                    using (var pageBitmap = new Bitmap(320, 180))
                    using (var pageGraphics = Graphics.FromImage(pageBitmap))
                    {
                        pageGraphics.PageUnit = GraphicsUnit.Pixel;
                        pageGraphics.Clear(Color.White);
                        var result = PrintPageRenderer.Render(pageGraphics, new Rectangle(0, 0, 260, 140), data, "Test report", "Subtitle", row, page);
                        Assert(pageBitmap.GetPixel(2, 70).ToArgb() == Color.FromArgb(37, 78, 112).ToArgb(), "every page should repeat the colored column header");
                        Assert(HasInk(pageBitmap, 190, 120, 260, 140), "every page should draw its page-number footer");
                        Assert(result.NextRow > row, "each page should advance rows");
                        row = result.NextRow; pages++; page++;
                        Assert(pages < 50, "pagination should terminate");
                    }
                }
                Assert(pages > 1 && row == data.Rows.Count, "report should paginate all rows");
                var restart = PrintPageRenderer.Render(graphics, new Rectangle(0, 0, 260, 140), data, "Test report", null, 0, 1);
                Assert(restart.NextRow > 0, "new preview should start at the first row");

                var tooSmallFailed = false;
                try { PrintPageRenderer.Render(graphics, new Rectangle(0, 0, 260, 90), data, "Test report", "Subtitle", 0, 1); }
                catch (InvalidOperationException) { tooSmallFailed = true; }
                Assert(tooSmallFailed, "a page too small for a row should fail clearly instead of dropping rows");
            }
        }

        private static bool HasInk(Bitmap bitmap, int left, int top, int right, int bottom)
        {
            for (var y = top; y < bottom; y++)
                for (var x = left; x < right; x++)
                    if (bitmap.GetPixel(x, y).ToArgb() != Color.White.ToArgb()) return true;
            return false;
        }

        private static void ReportDocumentSetsPrintLayout()
        {
            using (var data = new DataTable())
            using (var document = ReportPrinter.CreateDocument(data, "Monthly Report", "October"))
            {
                Assert(document.DocumentName == "Monthly Report", "print job should use report title");
                Assert(document.DefaultPageSettings.Margins.Left >= 40, "report should have readable page margins");
            }
        }

        private static void Assert(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }

        private sealed class CustomTextBox : TextBox { }
        private sealed class CustomCaption : Control { }
    }
}
