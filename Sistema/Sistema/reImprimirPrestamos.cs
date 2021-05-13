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
    public partial class reImprimirPrestamos : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt1 = new DataTable("p1");
        string prefijo;
        string numero_factura;
        string numero_factura2;
        double subtotal;
        double isv;
        double total;
        string direccion;
        public reImprimirPrestamos()
        {
        }

        public reImprimirPrestamos(string id)
        {
            InitializeComponent();
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT * FROM prestamo where numero_prestamo =  '" + id + "'", conexion);
                data.Fill(dt1);
                dataGridView1.DataSource = dt1;


                numFactura(id);
                //dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";

                OleDbCommand comando200 = new OleDbCommand("SELECT sum(cantidad * precio) as Total_Venta FROM prestamo where numero_prestamo =  '" + id + "' group by numero_prestamo", conexion);
                OleDbDataReader r = comando200.ExecuteReader();
                if (r.Read())
                {
                    subtotal = Convert.ToDouble(r.GetDouble(0));
                }

                OleDbCommand comando2000 = new OleDbCommand("SELECT direccion FROM clientes where Cliente =  '" + dataGridView1.CurrentRow.Cells[10].Value.ToString() + "'", conexion);
                OleDbDataReader rr = comando2000.ExecuteReader();
                if (rr.Read())
                {
                    direccion = Convert.ToString(rr.GetString(0));
                }
            }
            catch
            {

            }
            finally
            {
                conexion.Close();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                int contador = 0;

                ///// Se establece el tipo de Fuente        
                Font Fuente = new Font("Verdana", 9);
                Font Fuente_Factura = new Font("Verdana", 9);

                //titulo
                Font fuente_titulo = new Font("Verdana", 16, FontStyle.Bold);

                ///// Se establece el Color de Fuente
                Brush Brocha = Brushes.Black;
                ///// Se establece las cordenadas
                int Y = 40;
                //se genera el cuadro de los clientes
                e.Graphics.DrawRectangle(Pens.Black, 20, 130, 790, 20);

                //se genera el cuadro de la fecha y #prestamo
                e.Graphics.DrawRectangle(Pens.Black, 600, 45, 209, 40);


                //se genera el cuadro de los articulos
                e.Graphics.DrawRectangle(Pens.Black, 20, 155, 790, 285);

                //se genera el cuadro de los totales
                e.Graphics.DrawRectangle(Pens.Black, 20, 445, 790, 20);

                e.Graphics.DrawString(Program.GGG.empresa, fuente_titulo, Brocha, 190, 20);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                //muestra el logo
                Bitmap logo = Sistema.Properties.Resources.logo;
                e.Graphics.DrawImage(logo, 20, 20, 150, 100);

                e.Graphics.DrawString("Tel: " + Program.GGG.telefono_empresa, Fuente, Brocha, 190, 45);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("RTN: " + Program.GGG.RTN, Fuente, Brocha, 190, 65);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Direccion: " + Program.GGG.direccion, Fuente, Brocha, 190, 85);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Codigo", Fuente, Brocha, 20, 155);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Descripcion", Fuente, Brocha, 160, 155);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Cantidad", Fuente, Brocha, 580, 155);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Precio", Fuente, Brocha, 650, 155);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Total", Fuente, Brocha, 740, 155);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawLine(Pens.Black, 20, 175, 808, 175);

                e.Graphics.DrawString("Cliente:     " + dataGridView1.CurrentRow.Cells["cliente"].Value.ToString(), Fuente, Brocha, 20, 130);
                e.Graphics.DrawString("Vendedor: " + Program.usuario, Fuente, Brocha, 400, 130);

                e.Graphics.DrawString("Prestamo:  " + numero_factura, Fuente_Factura, Brocha, 600, 45);
                e.Graphics.DrawString("Fecha:     " + dataGridView1.CurrentRow.Cells[1].Value.ToString(), Fuente_Factura, Brocha, 600, 65);
                //se genera el cuadro de los articulos
                foreach (DataGridViewRow row in dataGridView1.Rows)

                {
                    if (contador <= 12)
                    {
                        string codigo = row.Cells["codigo"].Value.ToString();
                        string descripcion = row.Cells["descripcion"].Value.ToString();
                        int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);
                        double precio = Convert.ToDouble(row.Cells["precio"].Value);
                        e.Graphics.DrawString(codigo, Fuente, Brocha, 23, Y);
                        e.Graphics.DrawString(descripcion, Fuente, Brocha, 160, Y);
                        e.Graphics.DrawString(cantidad.ToString(), Fuente, Brocha, 590, Y);
                        e.Graphics.DrawString(precio.ToString("N2"), Fuente, Brocha, 650, Y);
                        e.Graphics.DrawString(subtotal.ToString("N2"), Fuente, Brocha, 740, Y);
                        Y = Y + 20;
                    }
                }

                e.Graphics.DrawString("Descuento: " + "0.00", Fuente, Brocha, 23, 447);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("SubTotal: " + subtotal.ToString("N2"), Fuente, Brocha, 250, 447);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("ISV: " + "0.00", Fuente, Brocha, 450, 447);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Total: " + subtotal.ToString("N2"), Fuente, Brocha, 640, 447);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Firma: ", Fuente, Brocha, 20, 470);
                e.Graphics.DrawLine(Pens.Black, 75, 495, 500, 495);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void numFactura(string id)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    OleDbCommand comando200 = new OleDbCommand("SELECT MAX(prefijo) FROM datos", conexion);
                    prefijo = Convert.ToString(comando200.ExecuteScalar());

                    numero_factura = id;



                    if (Convert.ToInt32(numero_factura) >= 1 && Convert.ToInt32(numero_factura) <= 9)
                    {
                        string n = "0000000";
                        numero_factura2 = prefijo + n + numero_factura;
                    }

                    if (Convert.ToInt32(numero_factura) >= 10 && Convert.ToInt32(numero_factura) <= 99)
                    {
                        string n = "000000";
                        numero_factura2 = prefijo + n + numero_factura;
                    }

                    if (Convert.ToInt32(numero_factura) >= 100 && Convert.ToInt32(numero_factura) <= 999)
                    {
                        string n = "00000";
                        numero_factura2 = prefijo + n + numero_factura;
                    }
                    if (Convert.ToInt32(numero_factura) >= 1000 && Convert.ToInt32(numero_factura) <= 9999)
                    {
                        string n = "0000";
                        numero_factura2 = prefijo + n + numero_factura;
                    }
                    if (Convert.ToInt32(numero_factura) >= 10000 && Convert.ToInt32(numero_factura) <= 99999)
                    {
                        string n = "000";
                        numero_factura2 = prefijo + n + numero_factura;
                    }
                    if (Convert.ToInt32(numero_factura) >= 100000 && Convert.ToInt32(numero_factura) <= 999999)
                    {
                        string n = "00";
                        numero_factura2 = prefijo + n + numero_factura;
                    }
                    if (Convert.ToInt32(numero_factura) >= 1000000 && Convert.ToInt32(numero_factura) <= 999999)
                    {
                        string n = "0";
                        numero_factura2 = prefijo + n + numero_factura;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
