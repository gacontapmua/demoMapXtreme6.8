using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        public delegate void PassData(string value1, string value2, string value3,int value4);

        public PassData passDatal;
         public void GetFirstValuel(string value1, string value2, string value3,string value4,string value5)
        {
            txtIdLine.Text = value1;
            txtNameLine.Text= value2;
            txtPoints.Text = value3;

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            WindowsFormsApp2.Form1.passDatal = new WindowsFormsApp2.Form1.PassData(GetFirstValuel);
        
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
          //  txtIdLine.Text = Convert.ToString(iAllLines + 1);
            if (txtNameLine.Text!="")
            {

                passDatal(txtIdLine.Text, txtNameLine.Text, txtPoints.Text, 0); 
            }
            if (passDatal != null)
            {
                passDatal(txtIdLine.Text, txtNameLine.Text, txtPoints.Text,1);
            }
            this.Hide();
        
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
             if (passDatal != null)
            {
                passDatal(txtIdLine.Text, txtNameLine.Text, txtPoints.Text, 2);
            }
            this.Hide();
        
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}



  

       

       
      