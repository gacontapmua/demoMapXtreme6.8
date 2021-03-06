using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;




using MapInfo.Data;
using MapInfo.Mapping;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Styles;
using MapInfo.Tools;
using MapInfo.Windows.Controls;
using MapInfo.Windows.Dialogs;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public delegate void PassData(string value1, string value2, string value3,int value4);

        public PassData passData;

        public void GetFirstValue(string value1, string value2, string value3, string value4, string value5)
        {
            txtX.Text = value1;
            txtY.Text = value2;
            txtIdPoint.Text = value3;
            txtNearPoint.Text = value4;
            txtListBus.Text = value5;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            WindowsFormsApp2.Form1.passData = new WindowsFormsApp2.Form1.PassData(GetFirstValue);
        }

       
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (passData != null)
            {
                passData(txtIdPoint.Text, txtNearPoint.Text, txtListBus.Text, 1);
            }
            this.Hide();

        }

       
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (passData != null)
            {
                passData(txtIdPoint.Text, txtNearPoint.Text, txtListBus.Text,2);
            }
            this.Hide();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
        //    if (passData != null)
        //    {
        //        passData(txtIdPoint.Text, txtNearPoint.Text, txtListBus.Text, 3);
        //    }
        //    this.Hide();
            this.Close();
       }


    }
}