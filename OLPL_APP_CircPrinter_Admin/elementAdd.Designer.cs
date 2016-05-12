namespace OLPL_APP_CircPrinter_Admin
{
    partial class elementAdd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnFont = new System.Windows.Forms.Button();
            this.lblData = new System.Windows.Forms.Label();
            this.tbxData = new System.Windows.Forms.TextBox();
            this.lblAlignment = new System.Windows.Forms.Label();
            this.cbAlignment = new System.Windows.Forms.ComboBox();
            this.tbxWidth = new System.Windows.Forms.TextBox();
            this.lblWidth = new System.Windows.Forms.Label();
            this.tbxHeight = new System.Windows.Forms.TextBox();
            this.lblHeight = new System.Windows.Forms.Label();
            this.tbxSpaceTop = new System.Windows.Forms.TextBox();
            this.lblSpaceTop = new System.Windows.Forms.Label();
            this.btxAdd = new System.Windows.Forms.Button();
            this.lblSettings = new System.Windows.Forms.Label();
            this.lblVars2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(280, 81);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(394, 33);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 87);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Type of Element";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Location = new System.Drawing.Point(1160, 23);
            this.listBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(348, 329);
            this.listBox1.TabIndex = 2;
            // 
            // btnFont
            // 
            this.btnFont.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFont.Location = new System.Drawing.Point(74, 169);
            this.btnFont.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(162, 100);
            this.btnFont.TabIndex = 3;
            this.btnFont.Text = "Font";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Visible = false;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(354, 144);
            this.lblData.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(141, 25);
            this.lblData.TabIndex = 4;
            this.lblData.Text = "Element Data";
            this.lblData.Visible = false;
            // 
            // tbxData
            // 
            this.tbxData.Location = new System.Drawing.Point(452, 175);
            this.tbxData.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tbxData.Name = "tbxData";
            this.tbxData.Size = new System.Drawing.Size(618, 31);
            this.tbxData.TabIndex = 5;
            this.tbxData.Visible = false;
            this.tbxData.Leave += new System.EventHandler(this.tbxData_Leave);
            // 
            // lblAlignment
            // 
            this.lblAlignment.AutoSize = true;
            this.lblAlignment.Location = new System.Drawing.Point(702, 235);
            this.lblAlignment.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblAlignment.Name = "lblAlignment";
            this.lblAlignment.Size = new System.Drawing.Size(112, 25);
            this.lblAlignment.TabIndex = 8;
            this.lblAlignment.Text = "Allignment";
            this.lblAlignment.Visible = false;
            // 
            // cbAlignment
            // 
            this.cbAlignment.FormattingEnabled = true;
            this.cbAlignment.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Center"});
            this.cbAlignment.Location = new System.Drawing.Point(820, 229);
            this.cbAlignment.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cbAlignment.Name = "cbAlignment";
            this.cbAlignment.Size = new System.Drawing.Size(250, 33);
            this.cbAlignment.TabIndex = 9;
            this.cbAlignment.Visible = false;
            this.cbAlignment.SelectedIndexChanged += new System.EventHandler(this.cbAlignment_SelectedIndexChanged);
            // 
            // tbxWidth
            // 
            this.tbxWidth.Location = new System.Drawing.Point(450, 283);
            this.tbxWidth.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tbxWidth.Name = "tbxWidth";
            this.tbxWidth.Size = new System.Drawing.Size(204, 31);
            this.tbxWidth.TabIndex = 11;
            this.tbxWidth.Visible = false;
            this.tbxWidth.TextChanged += new System.EventHandler(this.tbxWidth_TextChanged);
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(368, 288);
            this.lblWidth.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(67, 25);
            this.lblWidth.TabIndex = 10;
            this.lblWidth.Text = "Width";
            this.lblWidth.Visible = false;
            // 
            // tbxHeight
            // 
            this.tbxHeight.Location = new System.Drawing.Point(778, 283);
            this.tbxHeight.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tbxHeight.Name = "tbxHeight";
            this.tbxHeight.Size = new System.Drawing.Size(204, 31);
            this.tbxHeight.TabIndex = 13;
            this.tbxHeight.Visible = false;
            this.tbxHeight.TextChanged += new System.EventHandler(this.tbxHeight_TextChanged);
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(696, 288);
            this.lblHeight.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(74, 25);
            this.lblHeight.TabIndex = 12;
            this.lblHeight.Text = "Height";
            this.lblHeight.Visible = false;
            // 
            // tbxSpaceTop
            // 
            this.tbxSpaceTop.Location = new System.Drawing.Point(486, 225);
            this.tbxSpaceTop.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tbxSpaceTop.Name = "tbxSpaceTop";
            this.tbxSpaceTop.Size = new System.Drawing.Size(154, 31);
            this.tbxSpaceTop.TabIndex = 15;
            this.tbxSpaceTop.Visible = false;
            this.tbxSpaceTop.Leave += new System.EventHandler(this.tbxSpaceTop_Leave);
            // 
            // lblSpaceTop
            // 
            this.lblSpaceTop.AutoSize = true;
            this.lblSpaceTop.Location = new System.Drawing.Point(354, 233);
            this.lblSpaceTop.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSpaceTop.Name = "lblSpaceTop";
            this.lblSpaceTop.Size = new System.Drawing.Size(116, 25);
            this.lblSpaceTop.TabIndex = 14;
            this.lblSpaceTop.Text = "Space Top";
            this.lblSpaceTop.Visible = false;
            // 
            // btxAdd
            // 
            this.btxAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btxAdd.Location = new System.Drawing.Point(85, 430);
            this.btxAdd.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btxAdd.Name = "btxAdd";
            this.btxAdd.Size = new System.Drawing.Size(996, 94);
            this.btxAdd.TabIndex = 18;
            this.btxAdd.Text = "Save";
            this.btxAdd.UseVisualStyleBackColor = true;
            this.btxAdd.Click += new System.EventHandler(this.btxAdd_Click);
            // 
            // lblSettings
            // 
            this.lblSettings.AutoSize = true;
            this.lblSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettings.ForeColor = System.Drawing.Color.Red;
            this.lblSettings.Location = new System.Drawing.Point(142, 348);
            this.lblSettings.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSettings.Name = "lblSettings";
            this.lblSettings.Size = new System.Drawing.Size(0, 37);
            this.lblSettings.TabIndex = 26;
            this.lblSettings.Visible = false;
            // 
            // lblVars2
            // 
            this.lblVars2.AutoSize = true;
            this.lblVars2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVars2.ForeColor = System.Drawing.Color.Red;
            this.lblVars2.Location = new System.Drawing.Point(78, 366);
            this.lblVars2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblVars2.Name = "lblVars2";
            this.lblVars2.Size = new System.Drawing.Size(58, 37);
            this.lblVars2.TabIndex = 27;
            this.lblVars2.Text = "Fill";
            this.lblVars2.Visible = false;
            // 
            // elementAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1538, 554);
            this.Controls.Add(this.lblVars2);
            this.Controls.Add(this.lblSettings);
            this.Controls.Add(this.btxAdd);
            this.Controls.Add(this.tbxSpaceTop);
            this.Controls.Add(this.lblSpaceTop);
            this.Controls.Add(this.tbxHeight);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.tbxWidth);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.cbAlignment);
            this.Controls.Add(this.lblAlignment);
            this.Controls.Add(this.tbxData);
            this.Controls.Add(this.lblData);
            this.Controls.Add(this.btnFont);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "elementAdd";
            this.Text = "elementAdd";
            this.Load += new System.EventHandler(this.elementAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.TextBox tbxData;
        private System.Windows.Forms.Label lblAlignment;
        private System.Windows.Forms.ComboBox cbAlignment;
        private System.Windows.Forms.TextBox tbxWidth;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.TextBox tbxHeight;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.TextBox tbxSpaceTop;
        private System.Windows.Forms.Label lblSpaceTop;
        private System.Windows.Forms.Button btxAdd;
        private System.Windows.Forms.Label lblSettings;
        private System.Windows.Forms.Label lblVars2;
    }
}