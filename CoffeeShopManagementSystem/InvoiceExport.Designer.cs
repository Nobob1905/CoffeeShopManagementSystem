namespace CoffeeShopManagementSystem
{
    partial class InvoiceExport
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvoiceExport));
            this.uSPInvoiceExportBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.coffeeShopManagementDataSet = new CoffeeShopManagementSystem.CoffeeShopManagementDataSet();
            this.panel1 = new System.Windows.Forms.Panel();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.uSP_InvoiceExportTableAdapter = new CoffeeShopManagementSystem.CoffeeShopManagementDataSetTableAdapters.USP_InvoiceExportTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.uSPInvoiceExportBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.coffeeShopManagementDataSet)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uSPInvoiceExportBindingSource
            // 
            this.uSPInvoiceExportBindingSource.DataMember = "USP_InvoiceExport";
            this.uSPInvoiceExportBindingSource.DataSource = this.coffeeShopManagementDataSet;
            // 
            // coffeeShopManagementDataSet
            // 
            this.coffeeShopManagementDataSet.DataSetName = "CoffeeShopManagementDataSet";
            this.coffeeShopManagementDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.reportViewer1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 620);
            this.panel1.TabIndex = 0;
            // 
            // reportViewer1
            // 
            this.reportViewer1.AutoSize = true;
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet";
            reportDataSource1.Value = this.uSPInvoiceExportBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CoffeeShopManagementSystem.InvoiceExport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(776, 620);
            this.reportViewer1.TabIndex = 0;
            // 
            // uSP_InvoiceExportTableAdapter
            // 
            this.uSP_InvoiceExportTableAdapter.ClearBeforeFill = true;
            // 
            // InvoiceExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 644);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InvoiceExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invoice";
            this.Load += new System.EventHandler(this.InvoiceExport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uSPInvoiceExportBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.coffeeShopManagementDataSet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource uSPInvoiceExportBindingSource;
        private CoffeeShopManagementDataSet coffeeShopManagementDataSet;
        private CoffeeShopManagementDataSetTableAdapters.USP_InvoiceExportTableAdapter uSP_InvoiceExportTableAdapter;
    }
}