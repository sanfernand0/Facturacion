namespace Sistema
{
    partial class reporte_traslado
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.BaseDataSet = new Sistema.BaseDataSet();
            this.trasladoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.trasladoTableAdapter = new Sistema.BaseDataSetTableAdapters.trasladoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.BaseDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trasladoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.trasladoBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Sistema.Report2.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(939, 486);
            this.reportViewer1.TabIndex = 0;
            // 
            // BaseDataSet
            // 
            this.BaseDataSet.DataSetName = "BaseDataSet";
            this.BaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // trasladoBindingSource
            // 
            this.trasladoBindingSource.DataMember = "traslado";
            this.trasladoBindingSource.DataSource = this.BaseDataSet;
            // 
            // trasladoTableAdapter
            // 
            this.trasladoTableAdapter.ClearBeforeFill = true;
            // 
            // reporte_traslado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 486);
            this.Controls.Add(this.reportViewer1);
            this.Name = "reporte_traslado";
            this.Text = "reporte_traslado";
            this.Load += new System.EventHandler(this.reporte_traslado_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BaseDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trasladoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource trasladoBindingSource;
        private BaseDataSet BaseDataSet;
        private BaseDataSetTableAdapters.trasladoTableAdapter trasladoTableAdapter;
    }
}