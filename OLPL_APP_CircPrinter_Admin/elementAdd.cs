using DLL_CircPrintServer.Models;
using OLPL_APP_CircPrinter_Admin.Functions;
using OLPL_APP_CircPrinter_Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLPL_APP_CircPrinter_Admin
{
    public partial class elementAdd : Form
    {
        internal Form1 fc;
        public modelElement l1;
        public bool elementNew;
        int index;
        public elementAdd()
        {
            InitializeComponent();
        }

        private void elementAdd_Load(object sender, EventArgs e)
        {
            if (fc.tempateType == "Circulation")
            {
                comboBox1.Items.Add("Logo");
                comboBox1.Items.Add("Textline");
                comboBox1.Items.Add("Checkout Block");
            }
            else if (fc.tempateType == "Holds")
            {
                comboBox1.Items.Add("Logo");
                comboBox1.Items.Add("Textline");
                comboBox1.Items.Add("Textline - FirstName");
                comboBox1.Items.Add("Textline - HoldExpire");
                comboBox1.Items.Add("Textline - ItemID");
                comboBox1.Items.Add("Textline - UserPhoneNumber");
                comboBox1.Items.Add("Textline - VerticalLastName");
                

            }
            else if (fc.tempateType == "Financial")
            {
                comboBox1.Items.Add("Logo");
                comboBox1.Items.Add("Textline");
                comboBox1.Items.Add("Payment Block");
            }
            else if (fc.tempateType == "SerialRoute")
            {
                comboBox1.Items.Add("Logo");
                comboBox1.Items.Add("Textline");
                comboBox1.Items.Add("Textline - SerialTitle");
                comboBox1.Items.Add("Textline - SerialEnumeration");
                comboBox1.Items.Add("Textline - SerialChronology");
                comboBox1.Items.Add("Textline - SerialRouteTO");
            }
            else if (fc.tempateType == "UserRecord")
            {
                comboBox1.Items.Add("Logo");
                comboBox1.Items.Add("Textline");
                comboBox1.Items.Add("Textline - UserRecordName");
                comboBox1.Items.Add("Textline - UserRecordID");
                comboBox1.Items.Add("Block - UserRecordDemographic");
                comboBox1.Items.Add("Block - UserRecordCirculation");
                comboBox1.Items.Add("Block - UserRecordAddress");
                comboBox1.Items.Add("Block - UserRecordExtended");
                comboBox1.Items.Add("Block - UserRecordCheckouts");
                comboBox1.Items.Add("Block - UserRecordBills");
                comboBox1.Items.Add("Block - UserRecordHolds");
            }
            else if (fc.tempateType == "InTransit")
            {
                comboBox1.Items.Add("Logo");
                comboBox1.Items.Add("Barcode - ToLibrary");
                comboBox1.Items.Add("Barcode - ItemID");
                comboBox1.Items.Add("Textline");
                comboBox1.Items.Add("Textline - FromLibraryName");
                comboBox1.Items.Add("Textline - ItemTitle");
                comboBox1.Items.Add("Textline - ToLibraryName");
                comboBox1.Items.Add("Textline - ToLibraryCity");
                comboBox1.Items.Add("Textline - ILLLibsCeckout");
            }
            if (elementNew)
            {
                
                l1 = new modelElement();
                l1.id = fc.el1.Count + 1;
                
            }
            else
            {
                index = fc.el1.IndexOf(l1);
                comboBox1.SelectedItem = l1.name;
                if (l1.fontName != null)
                {
                    fontDialog1.Font=l1.fontName;
                    addListBox("Font Name: " + l1.fontName.Name);
                    addListBox("Font Size: " + l1.fontName.Size.ToString());
                    addListBox("Font Style: " + l1.fontName.Style.ToString());
                }
                if (l1.data != null)
                {
                    tbxData.Text = l1.data;
                    addListBox(lblData.Text + ": " + tbxData.Text);
                    if (tbxData.Text.Contains("<<<") && tbxData.Text.Contains(">>>"))
                    {
                        //lblVar.Visible = true;

                    }
                    else if (tbxData.Text.Contains("<<") && tbxData.Text.Contains(">>"))
                    {
                       lblVars2.Text = "Please make sure there is a variable named " + tbxData.Text.Split('>')[0].Remove(0, 2) + " in the program.settings file";
                        
                    }

                }
                if (l1.spaceTop >-99)
                {
                    tbxSpaceTop.Text = l1.spaceTop.ToString();
                    addListBox(lblSpaceTop.Text + ": " + tbxSpaceTop.Text);
                }
                if (l1.align != null)
                {
                    cbAlignment.SelectedItem = l1.align;
                    addListBox(lblAlignment.Text + ": " + cbAlignment.SelectedItem);
                }
                if (l1.height > 0)
                {
                    tbxHeight.Text = l1.height.ToString();
                    addListBox(lblHeight.Text + ": " + tbxHeight.Text);
                }
                if (l1.width > 0)
                {
                    tbxWidth.Text = l1.width.ToString();
                    addListBox(lblWidth.Text + ": " + tbxWidth.Text);
                }
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            l1.name = (string)comboBox1.SelectedItem;
           
            if (comboBox1.SelectedItem.ToString() == "Logo")
            {
                
                lblData.Visible = true;
                lblData.Text = "Logo Data";
                tbxData.Visible = true;
                btnFont.Enabled = false;
              
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblHeight.Visible = true;
                lblWidth.Visible = true;
                tbxHeight.Visible = true;
                tbxWidth.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Checkout Block")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Space between items";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Payment Block")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Space between items";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Barcode - ToLibrary")
            {
                btnFont.Visible = true;
                tbxData.Visible = false;
                addListBox("Data: None");
                lblData.Visible = false;
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - ToLibraryName")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before library name";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - ILLLibsCeckout")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before due date";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - FromLibraryName")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before library name";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - ToLibraryCity")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before library city";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - ItemTitle")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before item Title";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Barcode - ItemID")
            {
                btnFont.Visible = true;
                tbxData.Visible = false;
                addListBox("Data: None");
                lblData.Visible = false;
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - ItemID")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before item ID";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - SerialTitle")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before serial title";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - SerialEnumeration")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before Serial Enumeration";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - SerialRouteTO")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before route to Name";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - SerialChronology")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before Serial Chronology";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - UserPhoneNumber")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before user phone number";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - VerticalLastName")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Limit number of letters";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - FirstName")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Limit number of letters";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - HoldExpire")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before hold expire date";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - UserRecordName")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before Name";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Textline - UserRecordID")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Text before ID";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Block - UserRecordDemographic")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Space between Items";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Block - UserRecordCirculation")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Space between Items";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Block - UserRecordAddress")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Space between Items";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Block - UserRecordExtended")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Space between Items";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Block - UserRecordCheckouts")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Space between Items";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Block - UserRecordBills")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Space between Items";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            else if (comboBox1.SelectedItem.ToString() == "Block - UserRecordHolds")
            {
                btnFont.Visible = true;
                lblData.Visible = true;
                tbxData.Visible = true;
                lblData.Text = "Data: Space between Items";
                lblAlignment.Visible = true;
                cbAlignment.Visible = true;
                lblSpaceTop.Visible = true;
                tbxSpaceTop.Visible = true;
            }
            comboBox1.Enabled = false;
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            l1.fontName = fontDialog1.Font;
            addListBox("Font Name: " + l1.fontName.Name);
            addListBox("Font Size: " + l1.fontName.Size.ToString());
            addListBox("Font Style: " + l1.fontName.Style.ToString());
        }
        private void addListBox(string st1)
        {
            string[] str11 = st1.Split(':');
            if (listBox1.FindString(str11[0]) != -1)
            {
                listBox1.Items.RemoveAt(listBox1.FindString(str11[0]));
                listBox1.Items.Add(st1);
            }
            else { listBox1.Items.Add(st1); }
        }

        private void tbxData_Leave(object sender, EventArgs e)
        {
            if (tbxData.TextLength > 0)
            {
                if (tbxData.Text.Contains("<<<") && tbxData.Text.Contains(">>>"))
                {
                    //lblVar.Visible = true;

                }
                else if (tbxData.Text.Contains("<<") && tbxData.Text.Contains(">>"))
                {
                    
                    lblVars2.Text = "Please make sure threr is a variable named " + tbxData.Text.Split('>')[0].Remove(0, 2) + " in the program.settings file";
                    
                }

                addListBox(lblData.Text + ": " + tbxData.Text);
                l1.data = tbxData.Text;

            }
        }

        private void cbAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            addListBox(lblAlignment.Text + ": " + cbAlignment.SelectedItem);
            l1.align = (string)cbAlignment.SelectedItem;
        }

        private void tbxWidth_TextChanged(object sender, EventArgs e)
        {
            if (tbxWidth.TextLength > 0)
            {
                addListBox(lblWidth.Text + ": " + tbxWidth.Text);
                l1.width = int.Parse((string)tbxWidth.Text);
            }
            
        }

        private void tbxHeight_TextChanged(object sender, EventArgs e)
        {
            if (tbxHeight.TextLength > 0)
            {
                addListBox(lblHeight.Text + ": " + tbxHeight.Text);
                l1.height = int.Parse((string)tbxHeight.Text);
            }
        }
        private void tbxSpaceTop_Leave(object sender, EventArgs e)
        {
            if (tbxSpaceTop.TextLength > 0)
            {
                addListBox(lblSpaceTop.Text + ": " + tbxSpaceTop.Text);
                l1.spaceTop = int.Parse((string)tbxSpaceTop.Text);
            }
           
        }
        private void btxAdd_Click(object sender, EventArgs e)
        {
            if (validateElement())
            {
                if (elementNew)
                {
                    fc.el1.Add(l1);
                }
                else
                {
                    fc.el1.Where(u => u.id == l1.id).Select(u => u = l1).ToList();
                }
                
                populateLists.populateList(fc);
                PrintControl.CreatePrintPreviewControl(fc);
                this.Close();
            }
            else { MessageBox.Show("Item Missing Please fillout all components"); }

        }
        public bool validateElement()
        {
            string data = "";
            foreach(string str in listBox1.Items)
            {
                data += str;
            }
            if (!data.Contains("Data:")  || !data.Contains("Allignment") || !data.Contains("Space"))
            {
                return false;
            }
            if (l1.name.Contains("Textline"))
            {
                if (!data.Contains("Font"))
                {
                    return false;
                }
            }
            if (l1.name.Contains("Logo"))
            {
                if (!data.Contains("Height")|| !data.Contains("Width"))
                {
                    return false;
                }
            }
            return true;
        }

        private void tbxVar1_Leave(object sender, EventArgs e)
        {
            
        }

        private void tbxVar2_Leave(object sender, EventArgs e)
        {
           
        }

        private void tbxVar3_Leave(object sender, EventArgs e)
        {
            
        }
    }
}
