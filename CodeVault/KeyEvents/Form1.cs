using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyEvents {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void cmdClear_Click(object sender, EventArgs e) {
            dgvResults.Rows.Clear();
            txtInput.Clear();
            txtInput.Focus();
        }

        private void OnCheckedChanged(object sender, EventArgs e) {
            txtInput.Focus();
        }

        private void chkKeyPreview_CheckedChanged(object sender, EventArgs e) {
            KeyPreview = chkKeyPreview.Checked;
        }

        private void EnsureVisible() {
            DataGridViewCell c = dgvResults.Rows[dgvResults.Rows.Count - 1].Cells[0];
            dgvResults.CurrentCell = c;
        }


        private void txtInput_KeyPress(object sender, KeyPressEventArgs e) {
            if (chkKeyPress.Checked) {
                dgvResults.Rows.Add("txtInput_KeyPress", e.KeyChar, (int)e.KeyChar, "", "", "", "");
                EnsureVisible();
            }
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e) {
            if (chkKeyDown.Checked) {
                AddKEA("txtInput_KeyDown", e);
            }
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e) {
            if (chkKeyUp.Checked) {
                AddKEA("txtInput_KeyUp", e);
            }
        }

        private void AddKEA(string eventName, KeyEventArgs e) {
            dgvResults.Rows.Add(eventName, "", "", e.Modifiers, e.KeyCode, e.KeyData, e.KeyValue);
            EnsureVisible();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (chkKeyDown.Checked) {
                AddKEA("Form1_KeyDown", e);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) {
            if (chkKeyUp.Checked) {
                AddKEA("Form1_KeyUp", e);
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e) {
            if (chkKeyPress.Checked) {
                dgvResults.Rows.Add("Form1_KeyPress", e.KeyChar, (int)e.KeyChar, "", "", "", "");
                EnsureVisible();
            }
        }
    }
}