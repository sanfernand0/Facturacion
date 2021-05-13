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
    public partial class prestamo : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        AutoCompleteStringCollection clientes = new AutoCompleteStringCollection();
        DataTable dt = new DataTable("Inventario");
        double subtotal, totaltotal, ISV;
        double descuentototal;
        int cantidad = 0;
        double precio = 0;
        double descuentobd;
        double precio1;

        string prefijo;
        string numero_factura;
        string numero_factura2;
        int siguiente;

        string grupo, marca, codigo, descripcion;
        int cantidadfactura;
        double preciofactura;

        public prestamo()
        {
            InitializeComponent();
            actualizar();
            textBox4.Text = DateTime.Now.ToShortDateString();
            textBox1.Text = "Consumidor Final";
            comboBox2.Text = "Contado";
            textBox1.Focus();
            textBox9.Clear();
            limpiar();
            cargarclientes();
            label12.Text = "Usuario: " + Program.usuario;
            obtenerDescuento();
            obtenerDatos();
            dataGridView2.Columns[4].FillWeight = 300;
            dataGridView2.Columns[3].FillWeight = 150;
            dataGridView2.DefaultCellStyle.ForeColor = Color.Black;

        }

        public void actualizar()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT id, grupo, marca, codigo, descripcion, aplicacion, referencia, bodega1, tienda1, precio1, precio2 FROM inventario ORDER BY id ASC", conexion);
                data.Fill(dt);
                dataGridView2.DataSource = dt;
                dataGridView2.Columns[9].DefaultCellStyle.Format = "N2";
                dataGridView2.Columns[10].DefaultCellStyle.Format = "N2";
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

        public void obtenerDescuento()
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT descuento from datos", conexion);
                OleDbDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    descuentobd = Convert.ToDouble(reader.GetInt32(0));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conexion.Close();
            }
        }


        public void obtenerDatos()
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT cai, rango, fecha_limite from datos", conexion);
                OleDbDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    Program.cai = Convert.ToString(reader.GetString(0));
                    Program.rango = Convert.ToString(reader.GetString(1));
                    Program.fecha_limite = Convert.ToDateTime(reader.GetDateTime(2));
                }
            }
            catch(Exception ex)
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
                OleDbCommand comando = new OleDbCommand("SELECT  *FROM clientes ORDER BY Cliente ASC", conexion);
                OleDbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(reader.GetString(1));


                }
                textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox1.AutoCompleteCustomSource = clientes;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conexion.Close();
            }
        }

        public void limpiar()
        {
            Program.codigo = "";
            Program.descripcion = "";
            Program.precio1 = 0;
            Program.precio2 = 0;
            Program.tienda1 = 0;
            Program.bodega1 = 0;
            Program.grupo = "";
            Program.marca = "";
        }


        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                fmrAgregarCliente r = new fmrAgregarCliente();
                r.FormClosed += new FormClosedEventHandler(prestamo_FormClosed);
                r.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {

                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("codigo like '%{0}%' OR grupo like'%{0}%' OR marca like'%{0}%' OR descripcion like'%{0}%' OR aplicacion like'%{0}%' OR referencia like'%{0}%'", textBox9.Text);
                dataGridView2.DataSource = dv.ToTable();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {

                subtotal = 0;
                ISV = 0;
                totaltotal = 0;
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox9.Clear();
                textBox9.Focus();



                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    subtotal += Convert.ToDouble(row.Cells[4].Value);

                }
                ISV = Program.ISV * subtotal;
                totaltotal = subtotal + ISV;
                textBox7.Text = ISV.ToString("N2");
                textBox8.Text = totaltotal.ToString("N2");
                textBox6.Text = subtotal.ToString("N2");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].Selected == true)
                    {

                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                        subtotal = 0;
                        ISV = 0;
                        totaltotal = 0;
                        textBox6.Clear();
                        textBox7.Clear();
                        textBox8.Clear();
                        textBox9.Clear();
                        textBox9.Focus();



                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            subtotal += Convert.ToDouble(row.Cells[4].Value);

                        }
                        ISV = Program.ISV * subtotal;
                        totaltotal = subtotal + ISV;
                        textBox7.Text = ISV.ToString("N2");
                        textBox8.Text = totaltotal.ToString("N2");
                        textBox6.Text = subtotal.ToString("N2");

                    }
                    else
                    {
                        dataGridView1.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

                e.Graphics.DrawString("Cliente:     " + textBox1.Text, Fuente, Brocha, 20, 130);
                e.Graphics.DrawString("Vendedor: " + Program.usuario, Fuente, Brocha, 400, 130);

                e.Graphics.DrawString("Prestamo:  " + numero_factura, Fuente_Factura, Brocha, 600, 45);
                e.Graphics.DrawString("Fecha:     " + textBox4.Text, Fuente_Factura, Brocha, 600, 65);

                //se genera el cuadro de los articulos
                foreach (DataGridViewRow row in dataGridView1.Rows)

                {
                    if (contador <= 12)
                    {
                        string codigo = row.Cells[0].Value.ToString();
                        string descripcion = row.Cells[2].Value.ToString();
                        int cantidad = Convert.ToInt32(row.Cells[3].Value);
                        double precio = Convert.ToDouble(row.Cells[4].Value);
                        double total2 = Convert.ToDouble(row.Cells[5].Value);
                        e.Graphics.DrawString(codigo, Fuente, Brocha, 23, Y);
                        e.Graphics.DrawString(descripcion, Fuente, Brocha, 160, Y);
                        e.Graphics.DrawString(cantidad.ToString(), Fuente, Brocha, 590, Y);
                        e.Graphics.DrawString(precio.ToString("N2"), Fuente, Brocha, 650, Y);
                        e.Graphics.DrawString(total2.ToString("N2"), Fuente, Brocha, 740, Y);
                        Y = Y + 20;
                    }
                }

                e.Graphics.DrawString("Descuento: " + textBox5.Text, Fuente, Brocha, 23, 447);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("SubTotal: " + textBox6.Text, Fuente, Brocha, 250, 447);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("ISV: " + textBox7.Text, Fuente, Brocha, 450, 447);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Total: " + textBox8.Text, Fuente, Brocha, 640, 447);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Firma: ", Fuente, Brocha, 20, 470);
                e.Graphics.DrawLine(Pens.Black, 75, 495, 500, 495);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    conexion.Open();
                    OleDbCommand comando200 = new OleDbCommand("SELECT MAX(prefijo) FROM datos", conexion);
                    prefijo = Convert.ToString(comando200.ExecuteScalar());

                    OleDbCommand comando9 = new OleDbCommand("SELECT MAX(numero_prestamo) FROM datos", conexion);
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
                    string update2 = "UPDATE datos set numero_prestamo = '" + siguiente + "' WHERE Id=" + 1 + "";
                    OleDbCommand comando2 = new OleDbCommand(update2, conexion);
                    comando2.ExecuteNonQuery();
                    conexion.Close();

                }

                //para insertar en prestamo
                try
                {
                    conexion.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        try
                        {
                            grupo = Convert.ToString(row.Cells[7].Value);
                        }
                        catch
                        {
                            grupo = "";
                        }

                        try
                        {
                            marca = Convert.ToString(row.Cells[8].Value);
                        }
                        catch
                        {
                            marca = "";
                        }

                        try
                        {
                            codigo = Convert.ToString(row.Cells[0].Value);
                        }
                        catch
                        {
                            codigo = "";
                        }


                        try
                        {
                            descripcion = Convert.ToString(row.Cells[2].Value);

                        }
                        catch
                        {
                            descripcion = "";
                        }

                        try
                        {
                            cantidadfactura = Convert.ToInt32(row.Cells[3].Value);

                        }
                        catch
                        {
                            cantidadfactura = 0;
                        }

                        try
                        {
                            preciofactura = Convert.ToDouble(row.Cells[4].Value);

                        }
                        catch
                        {
                            preciofactura = 0;
                        }
                        int codigo5 = 1;

                        OleDbCommand comando20 = new OleDbCommand("SELECT MAX(id) FROM prestamo", conexion);
                        if (comando20.ExecuteScalar() != DBNull.Value)
                        {
                            codigo5 = Convert.ToInt32(comando20.ExecuteScalar());
                            codigo5 = codigo5 + 1;
                        }
                        string insert3 = "INSERT INTO prestamo VALUES (@id, @fecha, @numero_prestamo, @grupo, @marca, @codigo, @descripcion, @cantidad, @precio, @cliente, @rtn, @vendedor, @tipo, @fecha_anulacion)";
                        OleDbCommand comando7 = new OleDbCommand(insert3, conexion);
                        comando7.Parameters.AddWithValue("@id", codigo5);
                        comando7.Parameters.AddWithValue("@fecha", textBox4.Text);
                        comando7.Parameters.AddWithValue("@numero_prestamo", numero_factura);
                        comando7.Parameters.AddWithValue("@grupo", grupo);
                        comando7.Parameters.AddWithValue("@marca", marca);
                        comando7.Parameters.AddWithValue("@codigo", codigo);
                        comando7.Parameters.AddWithValue("@descripcion", descripcion);
                        comando7.Parameters.AddWithValue("@cantidad", cantidadfactura);
                        comando7.Parameters.AddWithValue("@precio", preciofactura);
                        comando7.Parameters.AddWithValue("@cliente", textBox1.Text);
                        comando7.Parameters.AddWithValue("@rtn", textBox2.Text);
                        comando7.Parameters.AddWithValue("@vendedor", Program.usuario);
                        comando7.Parameters.AddWithValue("@tipo", comboBox2.Text);
                        comando7.Parameters.AddWithValue("@fecha_anulacion", "00");
                        comando7.ExecuteNonQuery();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    conexion.Close();
                }


                //para rebajar las cantidades de inventario

                try
                {
                    foreach (DataGridViewRow factura in dataGridView1.Rows)
                    {

                        int cantidad;
                        int cantidadinventario;
                        int idinventario;


                        cantidad = Convert.ToInt32(factura.Cells[3].Value);
                        cantidadinventario = Convert.ToInt32(factura.Cells[10].Value);
                        idinventario = Convert.ToInt32(factura.Cells[9].Value);

                        int resta = cantidadinventario - cantidad;

                        conexion.Open();
                        string update = "UPDATE inventario set bodega1 = '" + resta + "' WHERE id= " + idinventario + "";
                        OleDbCommand comando55 = new OleDbCommand(update, conexion);
                        comando55.ExecuteNonQuery();
                        conexion.Close();

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

                printDocument1.Print();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyValue== (char)Keys.Enter)
                {
                    e.Handled = true;
                    if (textBox1.Text == "Consumidor Final")
                    {
                        textBox9.Focus();
                    }
                    else
                    {
                        conexion.Open();
                        OleDbCommand comando = new OleDbCommand("SELECT  *FROM clientes", conexion);
                        OleDbDataReader reader = comando.ExecuteReader();
                        while (reader.Read())
                        {
                            if (textBox1.Text == reader.GetString(1))
                            {
                                try
                                {
                                    textBox2.Text = reader.GetString(8);
                                }
                                catch
                                {
                                    textBox2.Text = "Error";
                                }
                                try
                                {
                                    textBox3.Text = reader.GetString(2);
                                }
                                catch
                                {
                                    textBox3.Text = "Error";
                                }
                                if (reader.GetBoolean(9) == true)
                                {
                                    comboBox2.Items.Add("Credito");
                                }
                                else
                                {
                                    comboBox2.Items.Remove("Credito");
                                }

                            }

                        }
                        textBox2.Focus();
                    }

                    conexion.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conexion.Close();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox3.Focus();
            }
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox3.Focus();
            }
        }


        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox9.Focus();
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter && textBox9.Text == "")
                {
                    e.Handled = true;
                    toolTip1.Show("Escriba algo", textBox9, 3000);
                }
                else
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        e.Handled = true;
                        try
                        {
                            if (dataGridView2.Rows[dataGridView2.CurrentRow.Index].Selected == true)
                            {
                                try
                                {
                                    try
                                    {
                                        Program.id = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);

                                    }
                                    catch
                                    {
                                        Program.id = 0;
                                    }
                                    try
                                    {
                                        Program.tienda1 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[7].Value);

                                    }
                                    catch
                                    {
                                        Program.tienda1 = 0;
                                    }

                                    try
                                    {
                                        Program.grupo = Convert.ToString(dataGridView2.CurrentRow.Cells[1].Value);

                                    }
                                    catch
                                    {
                                        Program.grupo = "";
                                    }
                                    try
                                    {
                                        Program.marca = Convert.ToString(dataGridView2.CurrentRow.Cells[2].Value);

                                    }
                                    catch
                                    {
                                        Program.marca = "";
                                    }
                                    try
                                    {
                                        Program.codigo = Convert.ToString(dataGridView2.CurrentRow.Cells[3].Value.ToString());

                                    }
                                    catch
                                    {
                                        Program.codigo = "";
                                    }
                                    try
                                    {
                                        Program.descripcion = Convert.ToString(dataGridView2.CurrentRow.Cells[4].Value.ToString());

                                    }
                                    catch
                                    {
                                        Program.descripcion = "";
                                    }

                                    try
                                    {
                                        Program.precio1 = Convert.ToDouble(dataGridView2.CurrentRow.Cells[9].Value);
                                    }
                                    catch
                                    {
                                        Program.precio1 = 0.00;
                                    }


                                    int rowEscribir = dataGridView1.Rows.Count;
                                    if (dataGridView1.Rows.Count < 13)
                                    {
                                        dataGridView1.Rows.Add(1);
                                        dataGridView1.Focus();
                                        dataGridView1.Rows[rowEscribir].Cells[0].Value = Program.codigo;
                                        dataGridView1.Rows[rowEscribir].Cells[1].Value = 0.00;
                                        dataGridView1.Rows[rowEscribir].Cells[2].Value = Program.descripcion;
                                        dataGridView1.Rows[rowEscribir].Cells[4].Value = Program.precio1;
                                        dataGridView1.Rows[rowEscribir].Cells[7].Value = Program.grupo;
                                        dataGridView1.Rows[rowEscribir].Cells[8].Value = Program.marca;
                                        dataGridView1.Rows[rowEscribir].Cells[9].Value = Program.id;
                                        dataGridView1.Rows[rowEscribir].Cells[10].Value = Program.tienda1;
                                        dataGridView1.Rows[rowEscribir].Cells[11].Value = Program.precio1;
                                        dataGridView1.CurrentCell = dataGridView1.Rows[rowEscribir].Cells[3];
                                        //limpiar();
                                    }
                                    else
                                    {
                                        label11.Text = "Ya No Puede Agregar Mas Items A La Factura";
                                    }
                                    dataGridView2.ClearSelection();

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                            }
                        }
                        catch
                        {

                        }
                    }

                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    try
                    {
                        e.Handled = true;
                        if (dataGridView2.Rows[dataGridView2.CurrentRow.Index].Selected == true)
                        {
                            try
                            {
                                try
                                {
                                    Program.tienda1 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[7].Value);

                                }
                                catch
                                {
                                    Program.tienda1 = 0;
                                }
                                try
                                {
                                    Program.id = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);

                                }
                                catch
                                {
                                    Program.id = 0;
                                }

                                try
                                {
                                    Program.grupo = Convert.ToString(dataGridView2.CurrentRow.Cells[1].Value);

                                }
                                catch
                                {
                                    Program.grupo = "";
                                }
                                try
                                {
                                    Program.marca = Convert.ToString(dataGridView2.CurrentRow.Cells[2].Value);

                                }
                                catch
                                {
                                    Program.marca = "";
                                }
                                try
                                {
                                    Program.codigo = Convert.ToString(dataGridView2.CurrentRow.Cells[3].Value.ToString());

                                }
                                catch
                                {
                                    Program.codigo = "";
                                }
                                try
                                {
                                    Program.descripcion = Convert.ToString(dataGridView2.CurrentRow.Cells[4].Value.ToString());

                                }
                                catch
                                {
                                    Program.descripcion = "";
                                }

                                try
                                {
                                    Program.precio1 = Convert.ToDouble(dataGridView2.CurrentRow.Cells[9].Value);
                                }
                                catch
                                {
                                    Program.precio1 = 0.00;
                                }


                                int rowEscribir = dataGridView1.Rows.Count;
                                if (dataGridView1.Rows.Count < 13)
                                {

                                    dataGridView1.Rows.Add(1);
                                    dataGridView1.Focus();
                                    dataGridView1.Rows[rowEscribir].Cells[0].Value = Program.codigo;
                                    dataGridView1.Rows[rowEscribir].Cells[1].Value = 0.00;
                                    dataGridView1.Rows[rowEscribir].Cells[2].Value = Program.descripcion;
                                    dataGridView1.Rows[rowEscribir].Cells[4].Value = Program.precio1;
                                    dataGridView1.Rows[rowEscribir].Cells[7].Value = Program.grupo;
                                    dataGridView1.Rows[rowEscribir].Cells[8].Value = Program.marca;
                                    dataGridView1.Rows[rowEscribir].Cells[9].Value = Program.id;
                                    dataGridView1.Rows[rowEscribir].Cells[10].Value = Program.tienda1;
                                    dataGridView1.Rows[rowEscribir].Cells[11].Value = Program.precio1;
                                    dataGridView1.CurrentCell = dataGridView1.Rows[rowEscribir].Cells[3];
                                    //limpiar();
                                }
                                else
                                {
                                    label11.Text = "Ya No Puede Agregar Mas Items A La Factura";
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            dataGridView2.ClearSelection();
                        }
                    }
                    catch
                    {

                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                    Column4.ReadOnly = false;
            }
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == (char)Keys.Down)
            {
                dataGridView2.Focus();
            }
        }

        private void prestamo_FormClosed(object sender, FormClosedEventArgs e)
        {
            cargarclientes();
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    Program.id = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);

                }
                catch
                {
                    Program.id = 0;
                }

                try
                {
                    Program.tienda1 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[7].Value);

                }
                catch
                {
                    Program.tienda1 = 0;
                }
                try
                {
                    Program.grupo = Convert.ToString(dataGridView2.CurrentRow.Cells[1].Value);

                }
                catch
                {
                    Program.grupo = "";
                }
                try
                {
                    Program.marca = Convert.ToString(dataGridView2.CurrentRow.Cells[2].Value);

                }
                catch
                {
                    Program.marca = "";
                }
                try
                {
                    Program.codigo = Convert.ToString(dataGridView2.CurrentRow.Cells[3].Value.ToString());

                }
                catch
                {
                    Program.codigo = "";
                }
                try
                {
                    Program.descripcion = Convert.ToString(dataGridView2.CurrentRow.Cells[4].Value.ToString());

                }
                catch
                {
                    Program.descripcion = "";
                }

                try
                {
                    Program.precio1 = Convert.ToDouble(dataGridView2.CurrentRow.Cells[9].Value);
                }
                catch
                {
                    Program.precio1 = 0.00;
                }

                int rowEscribir = dataGridView1.Rows.Count;
                if (Program.codigo != "")
                {
                    if (dataGridView1.Rows.Count < 13)
                    {

                        dataGridView1.Rows.Add(1);
                        dataGridView1.Focus();
                        dataGridView1.Rows[rowEscribir].Cells[0].Value = Program.codigo;
                        dataGridView1.Rows[rowEscribir].Cells[1].Value = 0.00;
                        dataGridView1.Rows[rowEscribir].Cells[2].Value = Program.descripcion;
                        dataGridView1.Rows[rowEscribir].Cells[4].Value = Program.precio1;
                        dataGridView1.Rows[rowEscribir].Cells[4].Value = Program.precio1;
                        dataGridView1.Rows[rowEscribir].Cells[7].Value = Program.grupo;
                        dataGridView1.Rows[rowEscribir].Cells[8].Value = Program.marca;
                        dataGridView1.Rows[rowEscribir].Cells[9].Value = Program.id;
                        dataGridView1.Rows[rowEscribir].Cells[10].Value = Program.tienda1;
                        dataGridView1.Rows[rowEscribir].Cells[11].Value = Program.precio1;
                        dataGridView1.CurrentCell = dataGridView1.Rows[rowEscribir].Cells[3];
                        //limpiar();
                    }
                    else
                    {
                        label11.Text = "Ya No Puede Agregar Mas Items";
                    }
                }
                dataGridView2.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //editar cantidad
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Column3")
                {
                    subtotal = 0;
                    ISV = 0;
                    descuentototal = 0;
                    totaltotal = 0;
                    textBox6.Clear();
                    textBox7.Clear();
                    textBox8.Clear();
                    textBox5.Clear();

                    cantidad = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                    precio = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());

                    dataGridView1.Rows[e.RowIndex].Cells[5].Value = cantidad * precio;
                    textBox9.Clear();
                    textBox9.Focus();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        subtotal += Convert.ToDouble(row.Cells[5].Value);
                        descuentototal += Convert.ToDouble(row.Cells[6].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);

                    }

                    ISV = Program.ISV * subtotal;
                    totaltotal = subtotal + ISV;
                    textBox7.Text = ISV.ToString("N2");
                    textBox8.Text = totaltotal.ToString("N2");
                    textBox6.Text = subtotal.ToString("N2");
                    textBox5.Text = descuentototal.ToString("N2");
                    dataGridView1.ClearSelection();
                }

                if (dataGridView1.Columns[e.ColumnIndex].Name == "Column4")
                {
                    if (Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value) > 0)
                    {
                        subtotal = 0;
                        ISV = 0;
                        descuentototal = 0;
                        totaltotal = 0;
                        textBox6.Clear();
                        textBox7.Clear();
                        textBox8.Clear();
                        textBox5.Clear();
                        dataGridView1.Rows[e.RowIndex].Cells[1].Value = 0;
                        cantidad = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                        precio = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());

                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = cantidad * precio;


                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            subtotal += Convert.ToDouble(row.Cells[5].Value);
                            descuentototal += Convert.ToDouble(row.Cells[6].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);

                        }
                        ISV = Program.ISV * subtotal;
                        totaltotal = subtotal + ISV;
                        textBox7.Text = ISV.ToString("N2");
                        textBox8.Text = totaltotal.ToString("N2");
                        textBox6.Text = subtotal.ToString("N2");
                        textBox5.Text = descuentototal.ToString("N2");
                        dataGridView1.ClearSelection();
                    }
                    else
                    {
                        subtotal = 0;
                        ISV = 0;
                        descuentototal = 0;
                        totaltotal = 0;
                        textBox6.Clear();
                        textBox7.Clear();
                        textBox8.Clear();
                        textBox5.Clear();

                        cantidad = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                        precio = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());

                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = cantidad * precio;
                        textBox9.Clear();
                        textBox9.Focus();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            subtotal += Convert.ToDouble(row.Cells[5].Value);
                            descuentototal += Convert.ToDouble(row.Cells[6].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);

                        }

                        ISV = Program.ISV * subtotal;
                        totaltotal = subtotal + ISV;
                        textBox7.Text = ISV.ToString("N2");
                        textBox8.Text = totaltotal.ToString("N2");
                        textBox6.Text = subtotal.ToString("N2");
                        textBox5.Text = descuentototal.ToString("N2");
                        dataGridView1.ClearSelection();
                        Column4.ReadOnly = true;
                    }
                }



                    if (dataGridView1.Columns[e.ColumnIndex].Name == "Column6")
                {
                    int descuentoprueb = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());

                    if (descuentoprueb <= descuentobd)
                    {
                        subtotal = 0;
                        descuentototal = 0;
                        precio1 = 0;

                        precio1 = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[11].Value);
                        double adescuento = (precio1 * Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[1].Value)) / 100;
                        double total = precio1 - adescuento;

                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = total;
                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = total * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = adescuento;

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            subtotal += Convert.ToDouble(row.Cells[5].Value);
                            descuentototal += Convert.ToDouble(row.Cells[6].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);

                        }
                        ISV = Program.ISV * subtotal;
                        totaltotal = subtotal + ISV;
                        textBox7.Text = ISV.ToString("N2");
                        textBox8.Text = totaltotal.ToString("N2");
                        textBox6.Text = subtotal.ToString("N2");
                        textBox5.Text = descuentototal.ToString("N2");
                        dataGridView1.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("Descuento Denegado");
                        dataGridView1.Rows[e.RowIndex].Cells[1].Value = 0;
                        subtotal = 0;
                        descuentototal = 0;
                        precio1 = 0;

                        precio1 = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[11].Value);
                        double adescuento = (precio1 * Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[1].Value)) / 100;
                        double total = precio1 - adescuento;

                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = total;
                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = total * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = adescuento;

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            subtotal += Convert.ToDouble(row.Cells[5].Value);
                            descuentototal += Convert.ToDouble(row.Cells[6].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);

                        }
                        ISV = Program.ISV * subtotal;
                        totaltotal = subtotal + ISV;
                        textBox7.Text = ISV.ToString("N2");
                        textBox8.Text = totaltotal.ToString("N2");
                        textBox6.Text = subtotal.ToString("N2");
                        textBox5.Text = descuentototal.ToString("N2");
                        dataGridView1.ClearSelection();

                    }

                    ISV = Program.ISV * subtotal;
                    totaltotal = subtotal + ISV;
                    textBox7.Text = ISV.ToString("N2");
                    textBox8.Text = totaltotal.ToString("N2");
                    textBox6.Text = subtotal.ToString("N2");
                    textBox5.Text = descuentototal.ToString("N2");
                    dataGridView1.ClearSelection();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
