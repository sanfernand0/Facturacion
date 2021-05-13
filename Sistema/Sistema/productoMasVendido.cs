using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sistema
{
    public partial class productoMasVendido : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable factura = new DataTable("Inventario");
        DataTable prestamo = new DataTable("Inventario");
        DataTable cotizacion = new DataTable("Inventario");

        public productoMasVendido()
        {
            InitializeComponent();
            facturas();
            cotizaciones();
            prestamos();
        }
        public void facturas()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT codigo, marca, grupo, count(codigo) as TotalVentas FROM ventas where codigo <> '000' group by codigo, marca, grupo order by count(codigo) desc", conexion);
                data.Fill(factura);
                dataGridView1.DataSource = factura;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conexion.Close();
            }
        }
        public void prestamos()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT codigo, marca, grupo, count(codigo) as TotalVentas FROM prestamo where codigo <> '000' group by codigo, marca, grupo order by count(codigo) desc", conexion);
                data.Fill(prestamo);
                dataGridView2.DataSource = prestamo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conexion.Close();
            }
        }
        public void cotizaciones()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT codigo, marca, grupo, count(codigo) as TotalVentas FROM cotizacion  where codigo <> '000' group by codigo, marca, grupo order by count(codigo) desc", conexion);
                data.Fill(cotizacion);
                dataGridView3.DataSource = cotizacion;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conexion.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
