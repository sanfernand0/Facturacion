using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;


namespace Sistema
{
    public partial class verTodo : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt1 = new DataTable("p1");
        DataTable dt2 = new DataTable("p2");
        DataTable dt3 = new DataTable("p3");
        DataTable dt4 = new DataTable("p4");
        DataTable dt5 = new DataTable("p5");

        public verTodo()
        {
            InitializeComponent();
            facturas();
            facturasAnuladas();
            prestamos();
            prestamosAnulados();
            cotizaciones();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = dt1.DefaultView;
                dv.RowFilter = string.Format("Nro_Factura like '%{0}%' OR Cliente like'%{0}%' OR Vendedor like'%{0}%' OR Credito_Contado like'%{0}%'", textBox1.Text);
                dataGridView1.DataSource = dv.ToTable();
                textBox1.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void facturas()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT  numero_factura as Nro_Factura, fecha as Fecha, cliente as Cliente, vendedor as Vendedor, tipo as Credito_Contado, sum(cantidad * precio) as Total_Venta FROM ventas where fecha_anulacion = '00' group by numero_factura, cliente, fecha, vendedor, tipo", conexion);
                data.Fill(dt1);
                dataGridView1.DataSource = dt1;
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";

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

        public void facturasAnuladas()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT  numero_factura as Nro_Factura, fecha as Fecha, cliente as Cliente, vendedor as Vendedor, tipo as Credito_Contado, sum(cantidad * precio) as Total_Venta FROM ventas where fecha_anulacion <> '00' group by numero_factura, cliente, fecha, vendedor, tipo", conexion);
                data.Fill(dt2);
                dataGridView2.DataSource = dt2;
                dataGridView2.Columns[5].DefaultCellStyle.Format = "N2";

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

        public void prestamosAnulados()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT  numero_prestamo, fecha, cliente, vendedor, tipo, sum(cantidad * precio) as Total_Venta FROM prestamo where fecha_anulacion <> '00' group by numero_prestamo, cliente, fecha, vendedor, tipo order by numero_prestamo", conexion);
                data.Fill(dt4);
                dataGridView5.DataSource = dt4;
                dataGridView5.Columns[5].DefaultCellStyle.Format = "N2";

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
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT  numero_prestamo, fecha, cliente, vendedor, tipo, sum(cantidad * precio) as Total_Venta FROM prestamo where fecha_anulacion = '00' group by numero_prestamo, cliente, fecha, vendedor, tipo order by numero_prestamo", conexion);
                data.Fill(dt3);
                dataGridView3.DataSource = dt3;
                dataGridView3.Columns[5].DefaultCellStyle.Format = "N2";

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
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT  numero_prestamo, fecha, cliente, vendedor, sum(cantidad * precio) as Total_Venta FROM cotizacion group by numero_prestamo, cliente, fecha, vendedor order by numero_prestamo", conexion);
                data.Fill(dt5);
                dataGridView4.DataSource = dt5;
                dataGridView4.Columns[4].DefaultCellStyle.Format = "N2";

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
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.SelectedRows.Count > 0 && tabControl1.SelectedTab == tabControl1.TabPages["tabPage1"])
                {
                    string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    reImprimirFacturas r = new reImprimirFacturas(id);
                    r.Show();
                }
                else
                {
                    if (dataGridView1.SelectedRows.Count > 0 && tabControl1.SelectedTab == tabControl1.TabPages["tabPage3"])
                    {
                        string id = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                        reImprimirPrestamos r = new reImprimirPrestamos(id);
                        r.Show();
                    }
                    else
                    {
                        if (dataGridView4.SelectedRows.Count > 0 && tabControl1.SelectedTab == tabControl1.TabPages["tabPage4"])
                        {
                            string id = dataGridView4.CurrentRow.Cells[0].Value.ToString();
                            reImprimirCotizacion r = new reImprimirCotizacion(id);
                            r.Show();
                        }
                        else
                        {
                            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"] || tabControl1.SelectedTab == tabControl1.TabPages["tabPage5"])
                            {
                                MessageBox.Show("No se puede imprimir algo anulado", "Ninguna fila seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = dt2.DefaultView;
                dv.RowFilter = string.Format("Nro_Factura like '%{0}%' OR Cliente like'%{0}%' OR Vendedor like'%{0}%' OR Credito_Contado like'%{0}%'", textBox2.Text);
                dataGridView2.DataSource = dv.ToTable();
                textBox2.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = dt3.DefaultView;
                dv.RowFilter = string.Format("numero_prestamo like '%{0}%' OR cliente like'%{0}%' OR vendedor like'%{0}%'", textBox3.Text);
                dataGridView3.DataSource = dv.ToTable();
                textBox3.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = dt4.DefaultView;
                dv.RowFilter = string.Format("numero_prestamo like '%{0}%' OR cliente like'%{0}%' OR vendedor like'%{0}%'", textBox4.Text);
                dataGridView5.DataSource = dv.ToTable();
                textBox4.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = dt5.DefaultView;
                dv.RowFilter = string.Format("numero_prestamo like '%{0}%' OR cliente like'%{0}%' OR vendedor like'%{0}%'", textBox5.Text);
                dataGridView4.DataSource = dv.ToTable();
                textBox5.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
