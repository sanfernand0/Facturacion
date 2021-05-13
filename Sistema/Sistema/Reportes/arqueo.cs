using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DGVPrinterHelper;


namespace Sistema.Reportes
{
    public partial class arqueo : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt = new DataTable("Todo");
        public arqueo()
        {
            InitializeComponent();
            obtenerArticulos();
        }
        public void obtenerArticulos()
        {

            conexion.Open();
            OleDbDataAdapter data = new OleDbDataAdapter("SELECT fecha, numero_factura, tipo, cliente, vendedor, cantidad, precio FROM ventas", conexion);
            OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT fecha, numero_prestamo, tipo, cliente, vendedor,  cantidad, precio from prestamo", conexion);
            data1.Fill(dt);
            data.Fill(dt);
            dataGridView1.DataSource = dt;
            conexion.Close();

        }
    }
}
