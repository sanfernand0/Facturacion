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
    public partial class fmrNotaCredito : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        AutoCompleteStringCollection clientes = new AutoCompleteStringCollection();
        DataTable dt = new DataTable("Inventario");
        string cai;
        DateTime fechaL;
        string prefijo, rango;
        string numero_factura;
        string numero_factura2;
        int siguiente;
        string direccion;
        public fmrNotaCredito()
        {
            InitializeComponent();
            cargarclientes();
            obtenerDatos();
            agregarFilas();
        }

        public void obtenerDatos()
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT caiNotaCredito, fechaLimiteEmisionNotaCredito, rangoNota from datos", conexion);
                OleDbDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    cai = Convert.ToString(reader.GetString(0));
                    fechaL = Convert.ToDateTime(reader.GetDateTime(1));
                    rango = Convert.ToString(reader.GetString(2));

                }
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
        public void cargarclientes()
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT  * FROM clientes ORDER BY Cliente ASC", conexion);
                OleDbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(reader.GetString(1));
                }
                textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox1.AutoCompleteCustomSource = clientes;

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
        public void numFactura()
        {
            try
            {
                conexion.Open();
                if (dataGridView1.Rows.Count > 0)
                {
                    OleDbCommand comando200 = new OleDbCommand("SELECT MAX(prefijoNotaCredito) FROM datos", conexion);
                    prefijo = Convert.ToString(comando200.ExecuteScalar());

                    OleDbCommand comando9 = new OleDbCommand("SELECT MAX(numeroNota) FROM datos", conexion);
                    numero_factura = Convert.ToString(comando9.ExecuteScalar());



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

                    siguiente = Convert.ToInt32(numero_factura) + 1;
                    string update2 = "UPDATE datos set numeroNota = '" + siguiente + "' WHERE Id=" + 1 + "";
                    OleDbCommand comando2 = new OleDbCommand(update2, conexion);
                    comando2.ExecuteNonQuery();
                }
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
            if (textBox1.Text == "")
            {
                MessageBox.Show("Ingrese nombre del cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (checkBox1.Checked == false && checkBox2.Checked == false && checkBox3.Checked == false)
            {
                MessageBox.Show("Seleccione el motivo de Emisión", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
               
                numFactura();
                insertVentas();
                printDocument1.Print();
                this.Close();
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
                int Y = 65;


                //se genera el cuadro de la fecha y #cotizacion
                e.Graphics.DrawRectangle(Pens.Black, 580, 15, 215, 80);
                e.Graphics.DrawString("Nota de Credito: " + numero_factura2, new Font("Verdana", 7), Brocha, 580, 15);
                e.Graphics.DrawString("Fecha emisión del comprobante: " + dateTimePicker1.Value.ToShortDateString(), new Font("Verdana", 7), Brocha, 580, 35);
                //e.Graphics.DrawLine(Pens.Black, 450, 385, 450, 400);
                e.Graphics.DrawString("Vendedor: " + Program.usuario, new Font("Verdana", 7), Brocha, 580, 55);




                //cuadritos de contado credito checkbox
                e.Graphics.DrawString("Anulación", Fuente, Brocha, 580, 75);
                e.Graphics.DrawString("Devolución", Fuente, Brocha, 650, 75);
                e.Graphics.DrawString("Descuento", Fuente, Brocha, 723, 75);

                e.Graphics.DrawRectangle(Pens.Black, 630, 75, 15, 15);
                e.Graphics.DrawRectangle(Pens.Black, 705, 75, 15, 15);
                e.Graphics.DrawRectangle(Pens.Black, 778, 75, 15, 15);

                if (checkBox1.Checked)
                {
                    e.Graphics.DrawString("\u221A", Fuente, Brocha, 630, 75);
                }
                else if (checkBox2.Checked)
                {
                    e.Graphics.DrawString("\u221A", Fuente, Brocha, 705, 75);
                }
                else
                {
                    e.Graphics.DrawString("\u221A", Fuente, Brocha, 778, 75);
                }

                //fin del cuadro fecha cotizacion

                //se genera el cuadro de los articulos
                //e.Graphics.DrawRectangle(Pens.Black, 20, 120, 780, 12);
                e.Graphics.DrawLine(Pens.Black, 20, 171, 800, 171);
                e.Graphics.DrawRectangle(Pens.Black, 20, 120, 780, 275);

                //lineas verticales
                e.Graphics.DrawLine(Pens.Black, 150, 160, 150, 395);
                e.Graphics.DrawLine(Pens.Black, 550, 160, 550, 395);
                //e.Graphics.DrawLine(Pens.Black, 630, 160, 630, 395);
                e.Graphics.DrawLine(Pens.Black, 700, 160, 700, 395);



                //firma del cliente
                e.Graphics.DrawLine(Pens.Black, 35, 435, 350, 435);

                e.Graphics.DrawString("Nombre Y Firma del Cliente", Fuente, Brocha, 80, 435);


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

                e.Graphics.DrawString("CAI del comprobante: " + textBox3.Text, Fuente, Brocha, 190, 105);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("N° Correlativo Comprobante: " + textBox4.Text, Fuente, Brocha, 580, 105);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("N°", Fuente, Brocha, 20, 159);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Descripcion", Fuente, Brocha, 160, 159);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Monto Total", Fuente, Brocha, 550, 159);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                //                e.Graphics.DrawString("Precio", Fuente, Brocha, 630, 159);
                //Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Impuesto Asociado", Fuente, Brocha, 700, 159);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                //e.Graphics.DrawLine(Pens.Black, 20, 196, 823, 196);



                //se genera el cuadro de los clientes
                e.Graphics.DrawRectangle(Pens.Black, 20, 120, 780, 40);


                //linea vertical que divide el cuadro de clientes         
                e.Graphics.DrawLine(Pens.Black, 400, 120, 400, 170);


                // e.Graphics.DrawRectangle(Pens.Black, 399, 110, 426, 40);
                e.Graphics.DrawString("Cliente: " + textBox1.Text, Fuente, Brocha, 20, 120);
                e.Graphics.DrawString("Rango: " + rango, Fuente, Brocha, 400, 120);
                e.Graphics.DrawString("RTN: " + textBox2.Text, Fuente, Brocha, 20, 132);
                e.Graphics.DrawString("Fecha Limite De Emision:   " + fechaL.ToShortDateString(), Fuente, Brocha, 400, 132);
                e.Graphics.DrawString("Dirección: " + direccion, Fuente, Brocha, 20, 144);//Direccion
                e.Graphics.DrawString("CAI: " + cai, Fuente, Brocha, 400, 144);



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

                        string codigo = row.Cells[0].Value.ToString();
                        string descripcion = row.Cells[1].Value.ToString();
                        double cantidad = Convert.ToDouble(row.Cells[2].Value.ToString());
                        string precio = row.Cells[3].Value.ToString();

                        e.Graphics.DrawString(codigo, Fuente, Brocha, 23, Y);
                        e.Graphics.DrawString(descripcion, Fuente, Brocha, 160, Y);
                        e.Graphics.DrawString(cantidad.ToString(), Fuente, Brocha, 590, Y);
                        e.Graphics.DrawString(precio.ToString(), Fuente, Brocha, 780, Y, stringFormat);
                        Y = Y + 10;
                    
                }


            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox2.Checked = false;
                checkBox1.Checked = false;
            }
        }

        public void agregarFilas()
        {
            for (int i = 0; i < 20; i++)
            {
                dataGridView1.Rows.Add();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string consulta;
                conexion.Open();
                if (e.KeyValue == (char)Keys.Enter)
                {
                    e.Handled = true;
                    consulta = "SELECT RTN, direccion FROM clientes where Cliente =  '" + textBox1.Text.Trim() + "'";
                    OleDbCommand comando = new OleDbCommand(consulta, conexion);
                    OleDbDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        textBox2.Text = reader.GetString(0);
                        direccion = reader.GetString(1);
                    }
                }
            }
            catch (Exception ex)
            {
                textBox2.Text = "";
            }
            finally
            {
                conexion.Close();
            }
        }

        public void insertVentas()
        {
            try
            {
                conexion.Open();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["Column2"].Value.ToString() != "" && row.Cells["Column1"].Value.ToString() != "" && row.Cells["Column3"].Value.ToString() != "" && row.Cells["Column4"].Value.ToString() != "")
                    {
                        int codigo5 = 1;
                        OleDbCommand comando20 = new OleDbCommand("SELECT MAX(id) FROM notaCredito", conexion);
                        if (comando20.ExecuteScalar() != DBNull.Value)
                        {
                            codigo5 = Convert.ToInt32(comando20.ExecuteScalar());
                            codigo5 = codigo5 + 1;
                        }

                        string insert3 = "INSERT INTO notaCredito VALUES (@id, @fecha, @numeroNota, @caiNotaCredito, @motivoEmision, @descripcion, @impuestoAsociado, @montoTotal, @cliente, @rtn, @vendedor, @caiComprobante, @rango, @fechaLimiteEmisionNotaCredito)";
                        OleDbCommand comando7 = new OleDbCommand(insert3, conexion);
                        comando7.Parameters.AddWithValue("@id", codigo5);
                        comando7.Parameters.AddWithValue("@fecha", dateTimePicker1.Value.ToShortDateString());
                        comando7.Parameters.AddWithValue("@numeroNota", numero_factura);
                        comando7.Parameters.AddWithValue("@caiNotaCredito", cai);
                        if (checkBox1.Checked)
                        {
                            comando7.Parameters.AddWithValue("@motivoEmision", "Anulacion");
                        }
                        else if (checkBox2.Checked)
                        {
                            comando7.Parameters.AddWithValue("@motivoEmision", "Devolucion");
                        }
                        else
                        {
                            comando7.Parameters.AddWithValue("@motivoEmision", "Descuento");
                        }
                        comando7.Parameters.AddWithValue("@descripcion", row.Cells["Column2"].Value.ToString());
                        comando7.Parameters.AddWithValue("@impuestoAsociado", row.Cells["Column4"].Value.ToString());
                        comando7.Parameters.AddWithValue("@montoTotal", row.Cells["Column3"].Value.ToString());
                        comando7.Parameters.AddWithValue("@cliente", textBox1.Text);
                        comando7.Parameters.AddWithValue("@rtn", textBox2.Text);
                        comando7.Parameters.AddWithValue("@vendedor", Program.usuario);
                        comando7.Parameters.AddWithValue("@caiComprobante", textBox3.Text);
                        comando7.Parameters.AddWithValue("@rango", rango);
                        comando7.Parameters.AddWithValue("@fechaLimiteEmisionNotaCredito", fechaL);
                        comando7.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
            finally
            {
                conexion.Close();
            }
        }
    }
}
