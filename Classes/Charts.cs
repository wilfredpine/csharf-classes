using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;

namespace red_framework
{
    /// <summary>Creates a chart series from a DataTable.</summary>
    public static class Charts
    {
        /// <summary>Replaces chart series with one series using the selected category and numeric columns.</summary>
        public static Series Bind(
            Chart chart,
            DataTable data,
            string categoryColumn,
            string valueColumn,
            string seriesName,
            SeriesChartType chartType = SeriesChartType.Column)
        {
            if (chart == null) throw new ArgumentNullException(nameof(chart));

            chart.Series.Clear();
            if (data == null || data.Rows.Count == 0) return null;
            if (!data.Columns.Contains(categoryColumn))
                throw new ArgumentException("The category column was not found.", nameof(categoryColumn));
            if (!data.Columns.Contains(valueColumn))
                throw new ArgumentException("The value column was not found.", nameof(valueColumn));

            if (chart.ChartAreas.Count == 0)
                chart.ChartAreas.Add(new ChartArea("MainArea"));

            var series = new Series(string.IsNullOrWhiteSpace(seriesName) ? "Series1" : seriesName)
            {
                ChartArea = chart.ChartAreas[0].Name,
                ChartType = chartType,
                IsXValueIndexed = true,
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Double
            };

            foreach (DataRow row in data.Rows)
            {
                if (row.IsNull(categoryColumn) || row.IsNull(valueColumn)) continue;
                var category = Convert.ToString(row[categoryColumn], CultureInfo.CurrentCulture);
                var value = Convert.ToDouble(row[valueColumn], CultureInfo.CurrentCulture);
                series.Points.AddXY(category, value);
            }

            if (series.Points.Count == 0) return null;
            chart.Series.Add(series);
            return series;
        }
    }
}
