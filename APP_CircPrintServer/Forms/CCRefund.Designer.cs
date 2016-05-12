namespace APP_CircPrintServer.Forms
{
    partial class CCRefund
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
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbContactInfo = new System.Windows.Forms.TextBox();
            this.tbCCDigits = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dpTransDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.tbRecieptNumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbNotifyPreference = new System.Windows.Forms.ComboBox();
            this.tbNotes = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(53, 291);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(241, 48);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Contact Information";
            // 
            // tbContactInfo
            // 
            this.tbContactInfo.Location = new System.Drawing.Point(118, 147);
            this.tbContactInfo.Multiline = true;
            this.tbContactInfo.Name = "tbContactInfo";
            this.tbContactInfo.Size = new System.Drawing.Size(210, 66);
            this.tbContactInfo.TabIndex = 2;
            // 
            // tbCCDigits
            // 
            this.tbCCDigits.Location = new System.Drawing.Point(118, 12);
            this.tbCCDigits.Name = "tbCCDigits";
            this.tbCCDigits.Size = new System.Drawing.Size(111, 20);
            this.tbCCDigits.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Last 4 Digits of card";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Transaction date";
            // 
            // dpTransDate
            // 
            this.dpTransDate.Location = new System.Drawing.Point(118, 50);
            this.dpTransDate.Name = "dpTransDate";
            this.dpTransDate.Size = new System.Drawing.Size(200, 20);
            this.dpTransDate.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Receipt Number";
            // 
            // tbRecieptNumber
            // 
            this.tbRecieptNumber.Location = new System.Drawing.Point(114, 86);
            this.tbRecieptNumber.Name = "tbRecieptNumber";
            this.tbRecieptNumber.Size = new System.Drawing.Size(111, 20);
            this.tbRecieptNumber.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Notification Preference";
            // 
            // cbNotifyPreference
            // 
            this.cbNotifyPreference.FormattingEnabled = true;
            this.cbNotifyPreference.Items.AddRange(new object[] {
            "Email",
            "Phone",
            "Letter"});
            this.cbNotifyPreference.Location = new System.Drawing.Point(149, 120);
            this.cbNotifyPreference.Name = "cbNotifyPreference";
            this.cbNotifyPreference.Size = new System.Drawing.Size(121, 21);
            this.cbNotifyPreference.TabIndex = 10;
            // 
            // tbNotes
            // 
            this.tbNotes.Location = new System.Drawing.Point(53, 219);
            this.tbNotes.Multiline = true;
            this.tbNotes.Name = "tbNotes";
            this.tbNotes.Size = new System.Drawing.Size(275, 66);
            this.tbNotes.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Notes";
            // 
            // CCRefund
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 388);
            this.Controls.Add(this.tbNotes);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbNotifyPreference);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbRecieptNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dpTransDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbCCDigits);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbContactInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Name = "CCRefund";
            this.Text = "CCRefund";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.CCRefund_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbContactInfo;
        private System.Windows.Forms.TextBox tbCCDigits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dpTransDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbRecieptNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbNotifyPreference;
        private System.Windows.Forms.TextBox tbNotes;
        private System.Windows.Forms.Label label6;
    }
}