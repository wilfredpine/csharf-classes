using System;
using System.Windows.Forms;

namespace red_framework
{
    /// <summary>Small helpers for common Windows Forms input controls.</summary>
    public static class FormControls
    {
        /// <summary>Clears supported inputs inside a form or container. Also accepts a single input control.</summary>
        public static void ClearInputs(Control root)
        {
            if (root == null) return;

            ClearOne(root);
            foreach (Control child in root.Controls)
                ClearInputs(child);
        }

        /// <summary>Clears the Text value of a vendor control that exposes input through Control.Text.</summary>
        public static void ClearText(Control control)
        {
            if (control != null && !IsDisplayOnly(control)) control.Text = string.Empty;
        }

        /// <summary>Returns whether a supported input has a value.</summary>
        public static bool Required(Control control, string fieldName)
        {
            if (control == null) return false;
            if (control is CheckBox checkBox) return checkBox.Checked;
            if (control is RadioButton radioButton) return radioButton.Checked;
            if (control is ComboBox comboBox)
                return comboBox.DropDownStyle == ComboBoxStyle.DropDownList
                    ? comboBox.SelectedIndex >= 0
                    : !string.IsNullOrWhiteSpace(comboBox.Text);
            if (control is NumericUpDown) return true;
            if (control is DateTimePicker) return true;
            if (IsDisplayOnly(control)) return true;

            return !string.IsNullOrWhiteSpace(control.Text);
        }

        /// <summary>Returns the first empty required input, or null when all inputs are valid.</summary>
        public static Control FirstRequired(params Control[] controls)
        {
            if (controls == null) return null;
            foreach (Control control in controls)
            {
                if (!Required(control, control == null ? string.Empty : control.Name))
                    return control;
            }
            return null;
        }

        private static void ClearOne(Control control)
        {
            if (control is TextBoxBase textBox)
            {
                textBox.Clear();
                return;
            }
            if (control is ComboBox comboBox)
            {
                comboBox.SelectedIndex = -1;
                comboBox.Text = string.Empty;
                return;
            }
            if (control is DateTimePicker dateTimePicker)
            {
                var today = DateTime.Today;
                dateTimePicker.Value = today < dateTimePicker.MinDate ? dateTimePicker.MinDate
                    : today > dateTimePicker.MaxDate ? dateTimePicker.MaxDate : today;
                return;
            }
            if (control is CheckBox checkBox) { checkBox.Checked = false; return; }
            if (control is RadioButton radioButton) { radioButton.Checked = false; return; }
            if (control is NumericUpDown numericUpDown) { numericUpDown.Value = numericUpDown.Minimum; return; }
            if (IsDisplayOnly(control)) return;
        }

        private static bool IsDisplayOnly(Control control)
        {
            return control is Label || control is LinkLabel || control is ButtonBase ||
                control is GroupBox || control is TabPage || control is Form;
        }
    }
}
