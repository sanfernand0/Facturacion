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
    public partial class reImprimirFacturas : Form
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
        public reImprimirFacturas()
        {
        }

        public reImprimirFacturas(string id)
        {
            InitializeComponent();
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT * FROM ventas where numero_factura =  '" + id + "'", conexion);
                data.Fill(dt1);
                dataGridView1.DataSource = dt1;


                numFactura(id);
                //dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";

                OleDbCommand comando200 = new OleDbCommand("SELECT sum(cantidad * precio) as Total_Venta FROM ventas where numero_factura =  '" + id + "' group by numero_factura", conexion);
                OleDbDataReader r = comando200.ExecuteReader();
                if (r.Read())
                {
                    subtotal = Convert.ToDouble(r.GetDouble(0));
                }

                OleDbCommand comando2000 = new OleDbCommand("SELECT direccion FROM clientes where Cliente =  '" + dataGridView1.CurrentRow.Cells[10].Value.ToString().Trim() + "'", conexion);
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
                Font Fuente = new Font("Verdana", 7);
                Font Fuente_Factura = new Font("Verdana", 7);
                //titulo
                Font fuente_titulo = new Font("Verdana", 14, FontStyle.Bold);
                Font fuente_titulo2 = new Font("Verdana", 9, FontStyle.Bold);

                ///// Se establece el Color de Fuente
                Brush Brocha = Brushes.Black;
                ///// Se establece las cordenadas
                int Y = 75;


                //se genera el cuadro de la fecha y #cotizacion
                e.Graphics.DrawRectangle(Pens.Black, 580, 15, 215, 80);
                e.Graphics.DrawString("Factura: " + numero_factura2, new Font("Verdana", 8), Brocha, 580, 15);
                e.Graphics.DrawString("Fecha: " + dataGridView1.CurrentRow.Cells["fecha"].Value.ToString().Substring(0, 10), new Font("Verdana", 9), Brocha, 580, 35);
                //e.Graphics.DrawLine(Pens.Black, 450, 385, 450, 400);
                e.Graphics.DrawString("Vendedor: " + dataGridView1.CurrentRow.Cells["vendedor"].Value.ToString(), new Font("Verdana", 9), Brocha, 580, 55);




                //cuadritos de contado credito checkbox
                e.Graphics.DrawString("Contado:   ", Fuente, Brocha, 580, 75);
                e.Graphics.DrawString("Credito:   ", Fuente, Brocha, 680, 75);
                e.Graphics.DrawRectangle(Pens.Black, 645, 75, 15, 15);
                e.Graphics.DrawRectangle(Pens.Black, 745, 75, 15, 15);
                if (dataGridView1.CurrentRow.Cells["tipo"].Value.ToString() == "Contado")
                {
                    e.Graphics.DrawString("\u221A", Fuente, Brocha, 645, 75);
                }
                else
                {
                    e.Graphics.DrawString("\u221A", Fuente, Brocha, 745, 75);
                }

                //fin del cuadro fecha cotizacion

                //se genera el cuadro de los articulos
                //e.Graphics.DrawRectangle(Pens.Black, 20, 120, 780, 12);
                e.Graphics.DrawLine(Pens.Black, 20, 171, 800, 171);
                e.Graphics.DrawRectangle(Pens.Black, 20, 120, 780, 275);

                //lineas verticales
                e.Graphics.DrawLine(Pens.Black, 150, 160, 150, 350);
                e.Graphics.DrawLine(Pens.Black, 550, 160, 550, 350);
                e.Graphics.DrawLine(Pens.Black, 630, 160, 630, 350);
                e.Graphics.DrawLine(Pens.Black, 710, 160, 710, 350);


                //e.Graphics.DrawRectangle(Pens.Black, 20, 175, 560, 285);
                //e.Graphics.DrawRectangle(Pens.Black, 20, 175, 630, 285);
                //e.Graphics.DrawRectangle(Pens.Black, 20, 175, 720, 285);
                //e.Graphics.DrawRectangle(Pens.Black, 20, 175, 780, 285);

                //se genera el cuadro de los totales
                e.Graphics.DrawRectangle(Pens.Black, 20, 350, 780, 15);
                e.Graphics.DrawRectangle(Pens.Black, 20, 365, 780, 15);
                e.Graphics.DrawRectangle(Pens.Black, 20, 380, 780, 60);

                //se genera el cuadro de la nota
                e.Graphics.DrawRectangle(Pens.Black, 20, 443, 780, 45);


                //linea vertical que divide nro de orden
                e.Graphics.DrawLine(Pens.Black, 400, 395, 400, 440);

                //firma del cliente
                e.Graphics.DrawLine(Pens.Black, 35, 425, 350, 425);

                e.Graphics.DrawString("Nombre Y Firma del Cliente", Fuente, Brocha, 80, 425);

                e.Graphics.DrawString("N° De Orden De Compra Exenta: ", Fuente, Brocha, 410, 395);
                e.Graphics.DrawString("N° Constancia De Registro De Exoneración: ", Fuente, Brocha, 410, 410);
                e.Graphics.DrawString("N° Registro De La SAG: ", Fuente, Brocha, 410, 425);

                e.Graphics.DrawString("ORIGINAL - CLIENTE COPIA 1 - OBLIGADO TRIBUTARIO EMISOR COPIA 2", Fuente, Brocha, 20, 443);
                e.Graphics.DrawString("\"La factura es beneficio de todos, exijala\"", Fuente, Brocha, 580, 443);


                e.Graphics.DrawString("**Nota: No se aceptan cambios ni devoluciones en productos electricos**", new Font("Verdana", 5), Brocha, 20, 453);
                e.Graphics.DrawString("**Nota: No se aceptan cambios con facturas al credito**", new Font("Verdana", 5), Brocha, 20, 463);


                //e.Graphics.DrawRectangle(Pens.Black, 20, 465, 780, 35);

                //e.Graphics.DrawLine(Pens.Black, 20, 482, 825, 482);
                //e.Graphics.DrawLine(Pens.Black, 250, 465, 250, 482);
                //e.Graphics.DrawLine(Pens.Black, 450, 465, 450, 482);
                //e.Graphics.DrawLine(Pens.Black, 640, 465, 640, 482);
                //e.Graphics.DrawLine(Pens.Black, 450, 482, 450, 500);

                e.Graphics.DrawString(Program.GGG.empresa, fuente_titulo, Brocha, 188, 25);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                //muestra el logo
                Bitmap logo = Sistema.Properties.Resources.logo;
                e.Graphics.DrawImage(logo, 10, 5, 170, 110);



                e.Graphics.DrawString("" + Program.GGG.direccion, Fuente, Brocha, 190, 60);//Direccion
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("RTN: " + Program.GGG.RTN + "    Correo: " + Program.GGG.correo, Fuente, Brocha, 190, 75);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Tel: " + Program.GGG.telefono_empresa, Fuente, Brocha, 190, 90);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Codigo", Fuente, Brocha, 20, 159);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Descripcion", Fuente, Brocha, 160, 159);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Cantidad", Fuente, Brocha, 550, 159);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Precio", Fuente, Brocha, 630, 159);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Total", Fuente, Brocha, 710, 159);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                //e.Graphics.DrawLine(Pens.Black, 20, 196, 823, 196);



                //se genera el cuadro de los clientes
                e.Graphics.DrawRectangle(Pens.Black, 20, 120, 780, 40);


                //linea vertical que divide el cuadro de clientes         
                e.Graphics.DrawLine(Pens.Black, 400, 120, 400, 170);


                // e.Graphics.DrawRectangle(Pens.Black, 399, 110, 426, 40);
                e.Graphics.DrawString("Cliente: " + dataGridView1.CurrentRow.Cells["cliente"].Value.ToString(), Fuente, Brocha, 20, 120);
                e.Graphics.DrawString("Rango: " + Program.rango, Fuente, Brocha, 400, 120);
                e.Graphics.DrawString("RTN: " + dataGridView1.CurrentRow.Cells["rtn"].Value.ToString(), Fuente, Brocha, 20, 132);
                e.Graphics.DrawString("Fecha Limite De Emision:   " + Program.fecha_limite.ToShortDateString(), Fuente, Brocha, 400, 132);
                e.Graphics.DrawString("Dirección: " + direccion, Fuente, Brocha, 20, 144);//Direccion
                e.Graphics.DrawString("CAI: " + Program.cai, Fuente, Brocha, 400, 144);



                //cuadritos de contado credito checkbox
                //e.Graphics.DrawString("Contado:   ", Fuente, Brocha, 190, 110);
                //e.Graphics.DrawString("Credito:   ", Fuente, Brocha, 290, 110);
                //e.Graphics.DrawRectangle(Pens.Black, 240, 110, 15, 15);
                //e.Graphics.DrawRectangle(Pens.Black, 340, 110, 15, 15);





                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Far;


                //se genera el cuadro de los articulos
                foreach (DataGridViewRow row in dataGridView1.Rows)

                {
                    if (contador <= 16)
                    {
                        string codigo = row.Cells["codigo"].Value.ToString();
                        string descripcion = row.Cells["descripcion"].Value.ToString();
                        int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);
                        double precio = Convert.ToDouble(row.Cells["precio"].Value);
                        double total2 = Convert.ToDouble(precio * cantidad);
                        e.Graphics.DrawString(codigo, Fuente, Brocha, 23, Y);
                        e.Graphics.DrawString(descripcion, Fuente, Brocha, 160, Y);
                        e.Graphics.DrawString(cantidad.ToString(), Fuente, Brocha, 590, Y);
                        e.Graphics.DrawString(precio.ToString("N2"), Fuente, Brocha, 710, Y, stringFormat);
                        e.Graphics.DrawString(total2.ToString("N2"), Fuente, Brocha, 800, Y, stringFormat);
                        Y = Y + 10;
                    }
                }

                //imprime descuento
                e.Graphics.DrawString("Desc: ", Fuente, Brocha, 23, 351);
                e.Graphics.DrawString("" + "0.00", Fuente, Brocha, 23, 366);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());


                e.Graphics.DrawString("Importe Exento L: ", Fuente, Brocha, 110, 351);
                e.Graphics.DrawString("" + "0.00", Fuente, Brocha, 110, 366);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                e.Graphics.DrawLine(Pens.Black, 110, 350, 110, 380);//linea vertical


                e.Graphics.DrawString("Importe Gravado L: ", Fuente, Brocha, 240, 351);
                e.Graphics.DrawString("" + "0.00", Fuente, Brocha, 240, 366);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                e.Graphics.DrawLine(Pens.Black, 240, 350, 240, 380);//linea vertical


                e.Graphics.DrawString("Importe Exonerado L: ", Fuente, Brocha, 380, 351);
                e.Graphics.DrawString("" + "0.00", Fuente, Brocha, 380, 366);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                e.Graphics.DrawLine(Pens.Black, 380, 350, 380, 380);//linea vertical


                e.Graphics.DrawString("SubTotal: ", Fuente, Brocha, 520, 351);
                e.Graphics.DrawString("" + subtotal.ToString("N2"), Fuente, Brocha, 520, 366);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                e.Graphics.DrawLine(Pens.Black, 520, 350, 520, 380);//linea vertical


                e.Graphics.DrawString("ISV: ", Fuente, Brocha, 630, 351);
                e.Graphics.DrawString("" + (subtotal * double.Parse(dataGridView1.CurrentRow.Cells["isv"].Value.ToString())).ToString("N2"), Fuente, Brocha, 630, 366);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                e.Graphics.DrawLine(Pens.Black, 630, 350, 630, 380);//linea vertical


                e.Graphics.DrawString("Total: ", Fuente, Brocha, 710, 351);
                e.Graphics.DrawString("" + ((subtotal * double.Parse(dataGridView1.CurrentRow.Cells["isv"].Value.ToString())) + subtotal).ToString("N2"), Fuente, Brocha, 710, 366);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                e.Graphics.DrawLine(Pens.Black, 710, 350, 710, 380);//linea vertical


                //total en letras
                Conv r = new Conv();
                e.Graphics.DrawString("Total: " + r.enletras(((subtotal * double.Parse(dataGridView1.CurrentRow.Cells["isv"].Value.ToString())) + subtotal).ToString()), Fuente, Brocha, 23, 380);



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
