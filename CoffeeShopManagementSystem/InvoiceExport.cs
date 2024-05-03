using CoffeeShopManagementSystem.DTO;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShopManagementSystem
{
    public partial class InvoiceExport : Form
    {
        public InvoiceExport()
        {
            InitializeComponent();
        }

        private void InvoiceExport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        public void InvoiceExportion(int invoiceID, int discount) 
        {
            this.uSP_InvoiceExportTableAdapter.Fill(this.coffeeShopManagementDataSet.USP_InvoiceExport, invoiceID, discount);

            ReportParameterCollection parameters = new ReportParameterCollection();
            parameters.Add(new ReportParameter("InvoiceID", invoiceID.ToString()));
            parameters.Add(new ReportParameter("discount", discount.ToString()));
            this.reportViewer1.LocalReport.SetParameters(parameters);

            this.reportViewer1.RefreshReport();
        }
    }
}
