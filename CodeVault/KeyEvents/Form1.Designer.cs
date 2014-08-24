namespace KeyEvents {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.cmdClear = new System.Windows.Forms.Button();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblAdvice = new System.Windows.Forms.Label();
            this.chkKeyDown = new System.Windows.Forms.CheckBox();
            this.chkKeyUp = new System.Windows.Forms.CheckBox();
            this.chkKeyPress = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblAdviceKeyPreview = new System.Windows.Forms.Label();
            this.chkKeyPreview = new System.Windows.Forms.CheckBox();
            this.lblAdvicePreviewKeyDown = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Place caret in textbox and press keys, including Shift, Ctrl etc.";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(16, 29);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(213, 20);
            this.txtInput.TabIndex = 1;
            this.txtInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyUp);
            this.txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInput_KeyPress);
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
            // 
            // cmdClear
            // 
            this.cmdClear.Location = new System.Drawing.Point(235, 29);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(75, 23);
            this.cmdClear.TabIndex = 3;
            this.cmdClear.Text = "Clear";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column7,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dgvResults.Location = new System.Drawing.Point(16, 103);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.Size = new System.Drawing.Size(745, 390);
            this.dgvResults.TabIndex = 4;
            // 
            // Column1
            // 
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle15;
            this.Column1.HeaderText = "Event";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Column2.DefaultCellStyle = dataGridViewCellStyle16;
            this.Column2.HeaderText = "KeyChar";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column7
            // 
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Column7.DefaultCellStyle = dataGridViewCellStyle17;
            this.Column7.HeaderText = "(int)KeyChar";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column3
            // 
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Column3.DefaultCellStyle = dataGridViewCellStyle18;
            this.Column3.HeaderText = "Modifiers";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Column4.DefaultCellStyle = dataGridViewCellStyle19;
            this.Column4.HeaderText = "KeyCode";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Column5.DefaultCellStyle = dataGridViewCellStyle20;
            this.Column5.HeaderText = "KeyData";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Column6.DefaultCellStyle = dataGridViewCellStyle21;
            this.Column6.HeaderText = "KeyValue";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // lblAdvice
            // 
            this.lblAdvice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAdvice.Location = new System.Drawing.Point(12, 496);
            this.lblAdvice.Name = "lblAdvice";
            this.lblAdvice.Size = new System.Drawing.Size(693, 43);
            this.lblAdvice.TabIndex = 5;
            this.lblAdvice.Text = resources.GetString("lblAdvice.Text");
            // 
            // chkKeyDown
            // 
            this.chkKeyDown.AutoSize = true;
            this.chkKeyDown.Checked = true;
            this.chkKeyDown.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeyDown.Location = new System.Drawing.Point(446, 8);
            this.chkKeyDown.Name = "chkKeyDown";
            this.chkKeyDown.Size = new System.Drawing.Size(72, 17);
            this.chkKeyDown.TabIndex = 6;
            this.chkKeyDown.Text = "KeyDown";
            this.chkKeyDown.UseVisualStyleBackColor = true;
            this.chkKeyDown.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // chkKeyUp
            // 
            this.chkKeyUp.AutoSize = true;
            this.chkKeyUp.Checked = true;
            this.chkKeyUp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeyUp.Location = new System.Drawing.Point(446, 29);
            this.chkKeyUp.Name = "chkKeyUp";
            this.chkKeyUp.Size = new System.Drawing.Size(58, 17);
            this.chkKeyUp.TabIndex = 7;
            this.chkKeyUp.Text = "KeyUp";
            this.chkKeyUp.UseVisualStyleBackColor = true;
            this.chkKeyUp.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // chkKeyPress
            // 
            this.chkKeyPress.AutoSize = true;
            this.chkKeyPress.Checked = true;
            this.chkKeyPress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeyPress.Location = new System.Drawing.Point(446, 53);
            this.chkKeyPress.Name = "chkKeyPress";
            this.chkKeyPress.Size = new System.Drawing.Size(70, 17);
            this.chkKeyPress.TabIndex = 8;
            this.chkKeyPress.Text = "KeyPress";
            this.chkKeyPress.UseVisualStyleBackColor = true;
            this.chkKeyPress.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox1.Location = new System.Drawing.Point(118, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 100);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "KeyPressEventArgs";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.groupBox2.Location = new System.Drawing.Point(321, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(397, 100);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "KeyEventArgs";
            // 
            // lblAdviceKeyPreview
            // 
            this.lblAdviceKeyPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAdviceKeyPreview.Location = new System.Drawing.Point(13, 539);
            this.lblAdviceKeyPreview.Name = "lblAdviceKeyPreview";
            this.lblAdviceKeyPreview.Size = new System.Drawing.Size(693, 29);
            this.lblAdviceKeyPreview.TabIndex = 13;
            this.lblAdviceKeyPreview.Text = resources.GetString("lblAdviceKeyPreview.Text");
            // 
            // chkKeyPreview
            // 
            this.chkKeyPreview.AutoSize = true;
            this.chkKeyPreview.Location = new System.Drawing.Point(541, 8);
            this.chkKeyPreview.Name = "chkKeyPreview";
            this.chkKeyPreview.Size = new System.Drawing.Size(108, 17);
            this.chkKeyPreview.TabIndex = 14;
            this.chkKeyPreview.Text = "Form KeyPreview";
            this.chkKeyPreview.UseVisualStyleBackColor = true;
            this.chkKeyPreview.CheckedChanged += new System.EventHandler(this.chkKeyPreview_CheckedChanged);
            // 
            // lblAdvicePreviewKeyDown
            // 
            this.lblAdvicePreviewKeyDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAdvicePreviewKeyDown.AutoSize = true;
            this.lblAdvicePreviewKeyDown.Location = new System.Drawing.Point(13, 578);
            this.lblAdvicePreviewKeyDown.Name = "lblAdvicePreviewKeyDown";
            this.lblAdvicePreviewKeyDown.Size = new System.Drawing.Size(689, 13);
            this.lblAdvicePreviewKeyDown.TabIndex = 15;
            this.lblAdvicePreviewKeyDown.Text = "Lastly, the PreviewKeyDown event (which is not shown on this form because it is v" +
                "ery similar to KeyDown and KeyUp) occurs *before* KeyDown.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 600);
            this.Controls.Add(this.lblAdvicePreviewKeyDown);
            this.Controls.Add(this.chkKeyPreview);
            this.Controls.Add(this.lblAdviceKeyPreview);
            this.Controls.Add(this.chkKeyPress);
            this.Controls.Add(this.chkKeyUp);
            this.Controls.Add(this.chkKeyDown);
            this.Controls.Add(this.lblAdvice);
            this.Controls.Add(this.dgvResults);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "Key events decoder";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button cmdClear;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.Label lblAdvice;
        private System.Windows.Forms.CheckBox chkKeyDown;
        private System.Windows.Forms.CheckBox chkKeyUp;
        private System.Windows.Forms.CheckBox chkKeyPress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.Label lblAdviceKeyPreview;
        private System.Windows.Forms.CheckBox chkKeyPreview;
        private System.Windows.Forms.Label lblAdvicePreviewKeyDown;
    }
}

