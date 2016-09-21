namespace APP_CircPrintServer.Forms
{
    partial class changePrinter
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
            this.lblCurrentPrinter = new System.Windows.Forms.Label();
            this.lblListofPrinters = new System.Windows.Forms.Label();
            this.cbListofPrinters = new System.Windows.Forms.ComboBox();
            this.btnSavePrinter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCurrentPrinter
            // 
            this.lblCurrentPrinter.AutoSize = true;
            this.lblCurrentPrinter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblCurrentPrinter.Location = new System.Drawing.Point(82, 32);
            this.lblCurrentPrinter.Name = "lblCurrentPrinter";
            this.lblCurrentPrinter.Size = new System.Drawing.Size(189, 24);
            this.lblCurrentPrinter.TabIndex = 0;
            this.lblCurrentPrinter.Text = "Current Printer Set for";
            // 
            // lblListofPrinters
            // 
            this.lblListofPrinters.AutoSize = true;
            this.lblListofPrinters.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblListofPrinters.Location = new System.Drawing.Point(82, 98);
            this.lblListofPrinters.Name = "lblListofPrinters";
            this.lblListofPrinters.Size = new System.Drawing.Size(500, 24);
            this.lblListofPrinters.TabIndex = 1;
            this.lblListofPrinters.Text = "Please select one of the printers avaliable on this computer";
            // 
            // cbListofPrinters
            // 
            this.cbListofPrinters.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.cbListofPrinters.FormattingEnabled = true;
            this.cbListofPrinters.Location = new System.Drawing.Point(111, 165);
            this.cbListofPrinters.Name = "cbListofPrinters";
            this.cbListofPrinters.Size = new System.Drawing.Size(565, 32);
            this.cbListofPrinters.TabIndex = 2;
            // 
            // btnSavePrinter
            // 
            this.btnSavePrinter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.btnSavePrinter.Location = new System.Drawing.Point(111, 233);
            this.btnSavePrinter.Name = "btnSavePrinter";
            this.btnSavePrinter.Size = new System.Drawing.Size(565, 57);
            this.btnSavePrinter.TabIndex = 3;
            this.btnSavePrinter.Text = "Save ";
            this.btnSavePrinter.UseVisualStyleBackColor = true;
            this.btnSavePrinter.Click += new System.EventHandler(this.btnSavePrinter_Click);
            // 
            // changePrinter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 336);
            this.ControlBox = false;
            this.Controls.Add(this.btnSavePrinter);
            this.Controls.Add(this.cbListofPrinters);
            this.Controls.Add(this.lblListofPrinters);
            this.Controls.Add(this.lblCurrentPrinter);
            this.Name = "changePrinter";
            this.Text = "changePrinter";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.changePrinter_Load);
            this.Leave += new System.EventHandler(this.changePrinter_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblCurrentPrinter;
        public System.Windows.Forms.Label lblListofPrinters;
        private System.Windows.Forms.ComboBox cbListofPrinters;
        private System.Windows.Forms.Button btnSavePrinter;
    }
}