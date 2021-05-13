using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing.Imaging;

namespace Sistema
{
    public partial class factura : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        AutoCompleteStringCollection clientes = new AutoCompleteStringCollection();
        DataTable dt = new DataTable("Inventario");
        double subtotal, totaltotal, descuentobd, ISV, subtotal2;
        double descuentototal;
        int cantidad = 0;
        double precio = 0;
        double precio1;
        string prefijo;
        string numero_factura;
        string numero_factura2;
        int siguiente;
        bool exento;
        string grupo, marca, codigo, descripcion;
        int cantidadfactura;
        double preciofactura;
        string imp = "*";
        string fac2;
        int sig2;


        public factura()
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
            checkBox4.Checked = true;
            dataGridView2.Columns["id"].Visible = false;
            dataGridView2.Columns["exento"].Visible = false;

        }

        public void actualizar()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT id, grupo, marca, codigo, descripcion, aplicacion, referencia, bodega1 as Inventario, tienda1, precio1, precio2, exento as Exento FROM inventario ORDER BY id ASC", conexion);
                data.Fill(dt);
                dataGridView2.DataSource = dt;
                dataGridView2.Columns[9].DefaultCellStyle.Format = "N2";
                dataGridView2.Columns[10].DefaultCellStyle.Format = "N2";
                dataGridView2.Columns["tienda1"].Visible = false;
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
            catch (Exception ex)
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
                OleDbCommand comando = new OleDbCommand("SELECT  *FROM clientes ORDER BY Cliente ASC", conexion);
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
                r.FormClosed += new FormClosedEventHandler(factura_FormClosed);
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
                if (checkBox1.Checked)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Format("codigo like '%{0}%'", textBox9.Text);
                    dataGridView2.DataSource = dv.ToTable();
                }
                else
                if (checkBox2.Checked)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Format("grupo like'%{0}%'", textBox9.Text);
                    dataGridView2.DataSource = dv.ToTable();
                }
                else
                if (checkBox3.Checked)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Format("marca like'%{0}%' ", textBox9.Text);
                    dataGridView2.DataSource = dv.ToTable();
                }
                else
                if (checkBox4.Checked)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Format("grupo like'%{0}%' OR marca like'%{0}%' OR descripcion like'%{0}%' OR codigo like '%{0}%' OR referencia like '%{0}%' OR aplicacion like '%{0}%'", textBox9.Text);
                    dataGridView2.DataSource = dv.ToTable();
                }
                else
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Format("grupo like'%{0}%' OR marca like'%{0}%' OR descripcion like'%{0}%' OR codigo like '%{0}%' OR referencia like '%{0}%' OR aplicacion like '%{0}%'", textBox9.Text);
                    dataGridView2.DataSource = dv.ToTable();
                }


            }
            catch (Exception ex)
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
            }
            catch (Exception ex)
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
            imprimir(sender, e);
        }

        public void numFactura()
        {
            string maximo;
            int codigo;
            int f = 0;
            bool result;

            try
            {
                conexion.Open();
                if (dataGridView1.Rows.Count > 0)
                {

                    OleDbCommand comando200 = new OleDbCommand("SELECT MAX(prefijo) FROM datos", conexion);
                    prefijo = Convert.ToString(comando200.ExecuteScalar());

                    OleDbCommand comando2001 = new OleDbCommand("SELECT codigo FROM clientes where Cliente = '" + textBox1.Text + "'", conexion);
                    result = int.TryParse(Convert.ToString(comando2001.ExecuteScalar()), out f);


                    OleDbCommand comando2000 = new OleDbCommand("SELECT MAX(maximo) FROM datos", conexion);
                    maximo = Convert.ToString(comando2000.ExecuteScalar());

                    if (imp == "*")
                    {
                        OleDbCommand comando9 = new OleDbCommand("SELECT MAX(factura) FROM datos", conexion);
                        numero_factura = Convert.ToString(comando9.ExecuteScalar());
                    }
                    else
                    {
                        OleDbCommand comando9 = new OleDbCommand("SELECT MAX(factura2) FROM datos", conexion);
                        numero_factura = Convert.ToString(comando9.ExecuteScalar());
                    }

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

                    if (decimal.Parse(textBox8.Text) > decimal.Parse(maximo) && result == false)
                    {
                        imp = "X";
                        string update2 = "UPDATE datos set factura2 = '" + siguiente + "' WHERE Id=" + 1 + "";
                        OleDbCommand comando2 = new OleDbCommand(update2, conexion);
                        comando2.ExecuteNonQuery();
                    }
                    else
                    {

                        if (imp == "*")
                        {
                            string update2 = "UPDATE datos set factura = '" + siguiente + "' WHERE Id=" + 1 + "";
                            OleDbCommand comando2 = new OleDbCommand(update2, conexion);
                            comando2.ExecuteNonQuery();
                        }
                        else
                        {
                            string update2 = "UPDATE datos set factura2 = '" + siguiente + "' WHERE Id=" + 1 + "";
                            OleDbCommand comando2 = new OleDbCommand(update2, conexion);
                            comando2.ExecuteNonQuery();
                        }
                    }
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
        public void insertVentas()
        {
            try
            {
                conexion.Open();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    try
                    {
                        grupo = Convert.ToString(row.Cells[8].Value);
                    }
                    catch
                    {
                        grupo = "";
                    }

                    try
                    {
                        marca = Convert.ToString(row.Cells[9].Value);
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
                    OleDbCommand comando20 = new OleDbCommand("SELECT MAX(id) FROM ventas", conexion);
                    if (comando20.ExecuteScalar() != DBNull.Value)
                    {
                        codigo5 = Convert.ToInt32(comando20.ExecuteScalar());
                        codigo5 = codigo5 + 1;
                    }
                    if (Convert.ToBoolean(row.Cells[6].Value))
                    {
                        string insert3 = "INSERT INTO ventas VALUES (@id, @fecha, @numero_factura, @grupo, @marca, @codigo, @descripcion, @cantidad, @precio, @cliente, @rtn, @vendedor, @cai, @rango, @fecha_limite, @tipo, @fecha_anulacion, @isv)";
                        OleDbCommand comando7 = new OleDbCommand(insert3, conexion);
                        comando7.Parameters.AddWithValue("@id", codigo5);
                        comando7.Parameters.AddWithValue("@fecha", textBox4.Text);
                        comando7.Parameters.AddWithValue("@numero_factura", numero_factura);
                        comando7.Parameters.AddWithValue("@grupo", grupo);
                        comando7.Parameters.AddWithValue("@marca", marca);
                        comando7.Parameters.AddWithValue("@codigo", codigo);
                        comando7.Parameters.AddWithValue("@descripcion", descripcion);
                        comando7.Parameters.AddWithValue("@cantidad", cantidadfactura);
                        comando7.Parameters.AddWithValue("@precio", preciofactura);
                        comando7.Parameters.AddWithValue("@cliente", textBox1.Text);
                        comando7.Parameters.AddWithValue("@rtn", textBox2.Text);
                        comando7.Parameters.AddWithValue("@vendedor", Program.usuario);
                        comando7.Parameters.AddWithValue("@cai", Program.cai);
                        comando7.Parameters.AddWithValue("@rango", Program.rango);
                        comando7.Parameters.AddWithValue("@fecha_limite", Program.fecha_limite);
                        comando7.Parameters.AddWithValue("@tipo", comboBox2.Text);
                        comando7.Parameters.AddWithValue("@fecha_anulacion", "00");
                        comando7.Parameters.AddWithValue("@isv", 0);
                        comando7.ExecuteNonQuery();
                    }
                    else
                    {
                        string insert3 = "INSERT INTO ventas VALUES (@id, @fecha, @numero_factura, @grupo, @marca, @codigo, @descripcion, @cantidad, @precio, @cliente, @rtn, @vendedor, @cai, @rango, @fecha_limite, @tipo, @fecha_anulacion, @isv)";
                        OleDbCommand comando7 = new OleDbCommand(insert3, conexion);
                        comando7.Parameters.AddWithValue("@id", codigo5);
                        comando7.Parameters.AddWithValue("@fecha", textBox4.Text);
                        comando7.Parameters.AddWithValue("@numero_factura", numero_factura);
                        comando7.Parameters.AddWithValue("@grupo", grupo);
                        comando7.Parameters.AddWithValue("@marca", marca);
                        comando7.Parameters.AddWithValue("@codigo", codigo);
                        comando7.Parameters.AddWithValue("@descripcion", descripcion);
                        comando7.Parameters.AddWithValue("@cantidad", cantidadfactura);
                        comando7.Parameters.AddWithValue("@precio", preciofactura);
                        comando7.Parameters.AddWithValue("@cliente", textBox1.Text);
                        comando7.Parameters.AddWithValue("@rtn", textBox2.Text);
                        comando7.Parameters.AddWithValue("@vendedor", Program.usuario);
                        comando7.Parameters.AddWithValue("@cai", Program.cai);
                        comando7.Parameters.AddWithValue("@rango", Program.rango);
                        comando7.Parameters.AddWithValue("@fecha_limite", Program.fecha_limite);
                        comando7.Parameters.AddWithValue("@tipo", comboBox2.Text);
                        comando7.Parameters.AddWithValue("@fecha_anulacion", "00");
                        comando7.Parameters.AddWithValue("@isv", Program.ISV);
                        comando7.ExecuteNonQuery();
                    }
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

        public void rebajar()
        {
            try
            {
                conexion.Open();
                foreach (DataGridViewRow factura in dataGridView1.Rows)
                {

                    int cantidad;
                    int cantidadinventario;
                    int idinventario;


                    cantidad = Convert.ToInt32(factura.Cells[3].Value);
                    cantidadinventario = Convert.ToInt32(factura.Cells[11].Value);
                    idinventario = Convert.ToInt32(factura.Cells[10].Value);

                    int resta = cantidadinventario - cantidad;

                    string update = "UPDATE inventario set bodega1 = '" + resta + "' WHERE id= " + idinventario + "";
                    OleDbCommand comando55 = new OleDbCommand(update, conexion);
                    comando55.ExecuteNonQuery();


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
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && textBox8.Text != "")
            {
                if (comboBox2.Text == "Contado")
                {

                    try
                    {
                        numFactura();
                        insertVentas();
                        rebajar();
                        //para insertar en ventas
                        //para rebajar las cantidades de inventario
                        printDocument1.Print();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

                if (comboBox2.Text == "Credito")
                {
                    numFactura();
                    rebajar();
                    //para insertar en ventas
                    insertVentas();
                    //para rebajar las cantidades de inventario
                    printDocument1.Print();

                    try
                    {
                        conexion.Open();
                        int codigo = 1;
                        OleDbCommand comando22 = new OleDbCommand("SELECT MAX(Id) FROM ingresos", conexion);
                        if (comando22.ExecuteScalar() != DBNull.Value)
                        {
                            codigo = Convert.ToInt32(comando22.ExecuteScalar());
                            codigo = codigo + 1;
                        }

                        string insert = "INSERT INTO ingresos VALUES (@Id, @fecha, @numero_factura, @cliente, @monto, @tipo, @recibo, @fechaRecibo, @montoRecibo, @notaCredito, @fechaNotaCredito, @montoNotaCredito)";
                        OleDbCommand comando21 = new OleDbCommand(insert, conexion);
                        comando21.Parameters.AddWithValue("@id", codigo);
                        comando21.Parameters.AddWithValue("@fecha", textBox4.Text);
                        comando21.Parameters.AddWithValue("@numero_factura", numero_factura);
                        comando21.Parameters.AddWithValue("@cliente", textBox1.Text);
                        comando21.Parameters.AddWithValue("@monto", textBox8.Text);
                        comando21.Parameters.AddWithValue("@tipo", comboBox2.Text);
                        comando21.Parameters.AddWithValue("@recibo", "");
                        comando21.Parameters.AddWithValue("@fechaRecibo", textBox4.Text);
                        comando21.Parameters.AddWithValue("@montoRecibo", "");
                        comando21.Parameters.AddWithValue("@notaCredito", "");
                        comando21.Parameters.AddWithValue("@fechaNotaCredito", textBox4.Text);
                        comando21.Parameters.AddWithValue("@montoNotaCredito", "");
                        comando21.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        conexion.Close();
                    }
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Nada que imprimir", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                conexion.Open();
                if (e.KeyValue == (char)Keys.Enter)
                {
                    e.Handled = true;
                    if (textBox1.Text == "Consumidor Final")
                    {
                        textBox9.Focus();
                    }
                    else
                    {
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
                                    textBox2.Text = "";
                                }
                                try
                                {
                                    textBox3.Text = reader.GetString(2);
                                }
                                catch
                                {
                                    textBox3.Text = "";
                                }
                                if (reader.GetBoolean(9) == true)
                                {
                                    comboBox2.Items.Add("Credito");
                                }
                                else
                                {
                                    comboBox2.Items.Remove("Credito");
                                }
                                imp = reader.GetString(12);
                            }

                        }
                        textBox2.Focus();
                    }

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
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox9.Focus();
            }
        }

        public void cargarClick(object sender, EventArgs e)
        {
            cargar();
        }

        public void cargar()
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
                try
                {
                    exento = Convert.ToBoolean(dataGridView2.CurrentRow.Cells[11].Value);
                }
                catch
                {

                }


                int rowEscribir = dataGridView1.Rows.Count;
                if (dataGridView1.Rows.Count < 17)
                {
                    dataGridView1.Rows.Add(1);
                    dataGridView1.Focus();
                    dataGridView1.Rows[rowEscribir].Cells[0].Value = Program.codigo;
                    dataGridView1.Rows[rowEscribir].Cells[1].Value = 0.00;
                    dataGridView1.Rows[rowEscribir].Cells[2].Value = Program.descripcion;
                    dataGridView1.Rows[rowEscribir].Cells[4].Value = Program.precio1;
                    dataGridView1.Rows[rowEscribir].Cells[8].Value = Program.grupo;
                    dataGridView1.Rows[rowEscribir].Cells[9].Value = Program.marca;
                    dataGridView1.Rows[rowEscribir].Cells[10].Value = Program.id;
                    dataGridView1.Rows[rowEscribir].Cells[11].Value = Program.tienda1;
                    dataGridView1.Rows[rowEscribir].Cells[12].Value = Program.precio1;
                    dataGridView1.Rows[rowEscribir].Cells[6].Value = exento;
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
        public void cargarPress(object sender, KeyPressEventArgs e)
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

                        if (dataGridView2.Rows[dataGridView2.CurrentRow.Index].Selected == true)
                        {
                            cargar();
                            dataGridView2.ClearSelection();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            cargarPress(sender, e);
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    e.Handled = true;
                    if (dataGridView2.Rows[dataGridView2.CurrentRow.Index].Selected == true)
                    {
                        cargar();
                        dataGridView2.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox9.Focus();
            checkBox4.Checked = false;
            checkBox3.Checked = false;
            checkBox2.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox9.Focus();
            checkBox4.Checked = false;
            checkBox3.Checked = false;
            checkBox1.Checked = false;


        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            textBox9.Focus();
            checkBox4.Checked = false;
            checkBox2.Checked = false;
            checkBox1.Checked = false;


        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            textBox9.Focus();
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;

        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                c5.ReadOnly = false;
            }
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();

                if (textBox1.Text == "Consumidor Final" || textBox1.Text == "CONSUMIDOR FINAL")
                {
                    textBox9.Focus();
                }
                else
                {
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
                                textBox2.Text = "";
                            }
                            try
                            {
                                textBox3.Text = reader.GetString(2);
                            }
                            catch
                            {
                                textBox3.Text = "";
                            }
                            if (reader.GetBoolean(9) == true)
                            {
                                comboBox2.Items.Add("Credito");
                            }
                            else
                            {
                                comboBox2.Items.Remove("Credito");
                            }
                            imp = reader.GetString(12);
                        }

                    }
                    textBox2.Focus();
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

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("codigo='" + textBox10.Text.ToString().Trim() + "'");
                dataGridView2.DataSource = dv.ToTable();
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Empty;
                dataGridView2.DataSource = dv.ToTable();
                textBox10.Clear();
                textBox9.Focus();
            }

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{

            //}
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox10.Text))
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }
            else
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("codigo='{0}'", textBox10.Text);
            }
        }

        private void factura_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void factura_FormClosed(object sender, FormClosedEventArgs e)
        {
            cargarclientes();
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Down)
            {
                dataGridView2.Focus();
            }
        }


        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            cargarClick(sender, e);
        }

        public void totales(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                subtotal = 0;
                ISV = 0;
                subtotal2 = 0;
                descuentototal = 0;
                totaltotal = 0;
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox5.Clear();

                cantidad = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["c4"].Value.ToString());
                precio = double.Parse(dataGridView1.Rows[e.RowIndex].Cells["c5"].Value.ToString());

                dataGridView1.Rows[e.RowIndex].Cells["c6"].Value = cantidad * precio;
                textBox9.Clear();
                textBox9.Focus();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["c7"].Value) == true)
                    {
                        subtotal += Convert.ToDouble(row.Cells["c6"].Value);
                        descuentototal += Convert.ToDouble(row.Cells["c8"].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["c4"].Value);
                    }
                    else if (Convert.ToBoolean(row.Cells["c7"].Value) == false)
                    {
                        subtotal2 += Convert.ToDouble(row.Cells["c6"].Value);
                        descuentototal += Convert.ToDouble(row.Cells["c8"].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["c4"].Value);
                        ISV = (Program.ISV * subtotal2);
                    }
                }

                totaltotal = subtotal + subtotal2 + ISV;
                textBox7.Text = ISV.ToString("N2");
                textBox8.Text = totaltotal.ToString("N2");
                textBox6.Text = (subtotal + subtotal2).ToString("N2");
                textBox5.Text = descuentototal.ToString("N2");
                dataGridView1.ClearSelection();
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
                conexion.Open();

                //editar cantidad
                if (dataGridView1.Columns[e.ColumnIndex].Name == "c4")
                {
                    totales(sender, e);
                }

                if (dataGridView1.Columns[e.ColumnIndex].Name == "c5")
                {
                    try
                    {
                        double precio11 = double.Parse(dataGridView1.Rows[e.RowIndex].Cells["c5"].Value.ToString());
                        int idInventario = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["c11"].Value.ToString());
                        OleDbCommand comando200 = new OleDbCommand("SELECT precio2 FROM inventario where id = " + idInventario + "", conexion);
                        OleDbDataReader reader = comando200.ExecuteReader();
                        double precio33 = 0.00;
                        if (reader.Read())
                        {
                            precio33 = reader.GetDouble(0);
                        }
                        if (precio11 < precio33)
                        {
                            MessageBox.Show("No se puede dar a ese precio, excede el precio minimo", "Precio demasiado bajo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dataGridView1.Rows[e.RowIndex].Cells["c5"].Value = precio;

                        }
                        if (Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["c2"].Value) > 0)
                        {
                            totales(sender, e);
                        }
                        else
                        {
                            totales(sender, e);
                            c5.ReadOnly = true;
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



                if (dataGridView1.Columns[e.ColumnIndex].Name == "c2")
                {
                    int descuentoprueb = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["c2"].Value.ToString());

                    if (descuentoprueb <= descuentobd)
                    {
                        descuentototal = 0;
                        precio1 = 0;
                        subtotal = 0;
                        ISV = 0;
                        subtotal2 = 0;
                        descuentototal = 0;
                        totaltotal = 0;
                        textBox6.Clear();
                        textBox7.Clear();
                        textBox8.Clear();
                        textBox5.Clear();

                        precio1 = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["c13"].Value);
                        double adescuento = (precio1 * Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["c2"].Value)) / 100;
                        double total = precio1 - adescuento;

                        dataGridView1.Rows[e.RowIndex].Cells["c5"].Value = total;
                        dataGridView1.Rows[e.RowIndex].Cells["c6"].Value = total * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["c4"].Value);
                        dataGridView1.Rows[e.RowIndex].Cells["c8"].Value = adescuento;

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells["c7"].Value) == true)
                            {
                                subtotal += Convert.ToDouble(row.Cells["c6"].Value);
                                descuentototal += Convert.ToDouble(row.Cells["c8"].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["c4"].Value);
                            }
                            else if (Convert.ToBoolean(row.Cells["c7"].Value) == false)
                            {
                                subtotal2 += Convert.ToDouble(row.Cells["c6"].Value);
                                descuentototal += Convert.ToDouble(row.Cells["c8"].Value) * Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["c4"].Value);
                                ISV = (Program.ISV * subtotal2);
                            }
                        }

                        totaltotal = subtotal + subtotal2 + ISV;
                        textBox7.Text = ISV.ToString("N2");
                        textBox8.Text = totaltotal.ToString("N2");
                        textBox6.Text = (subtotal + subtotal2).ToString("N2");
                        textBox5.Text = descuentototal.ToString("N2");
                        dataGridView1.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("Descuento Denegado");
                        dataGridView1.Rows[e.RowIndex].Cells["c2"].Value = 0;
                        totales(sender, e);
                    }

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
        public void prueba(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                MessageBox.Show("" + e.MarginBounds.Right);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void imprimir(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                int contador = 0;

                ///// Se establece el tipo de Fuente        
                Font Fuente = new Font("Verdana", 9);
                Font Fuente_Factura = new Font("Verdana", 9);

                //titulo
                Font fuente_titulo = new Font("Verdana", 16, FontStyle.Bold);
                Font fuente_titulo2 = new Font("Verdana", 10, FontStyle.Bold);

                ///// Se establece el Color de Fuente
                Brush Brocha = Brushes.Black;
                ///// Se establece las cordenadas
                int Y = 65;
                //se genera el cuadro de los clientes
                e.Graphics.DrawRectangle(Pens.Black, 20, 110, 805, 60);
                e.Graphics.DrawRectangle(Pens.Black, 399, 110, 426, 60);

                //se genera el cuadro de la fecha y #cotizacion
                e.Graphics.DrawRectangle(Pens.Black, 580, 65, 244, 40);
                //se genera el cuadro de los articulos
                e.Graphics.DrawRectangle(Pens.Black, 20, 175, 140, 285);
                e.Graphics.DrawRectangle(Pens.Black, 20, 175, 560, 285);
                e.Graphics.DrawRectangle(Pens.Black, 20, 175, 630, 285);
                e.Graphics.DrawRectangle(Pens.Black, 20, 175, 720, 285);


                e.Graphics.DrawRectangle(Pens.Black, 20, 175, 805, 285);

                //se genera el cuadro de los totales
                e.Graphics.DrawRectangle(Pens.Black, 20, 465, 805, 35);

                e.Graphics.DrawLine(Pens.Black, 20, 482, 825, 482);
                e.Graphics.DrawLine(Pens.Black, 250, 465, 250, 482);
                e.Graphics.DrawLine(Pens.Black, 450, 465, 450, 482);
                e.Graphics.DrawLine(Pens.Black, 640, 465, 640, 482);
                e.Graphics.DrawLine(Pens.Black, 450, 482, 450, 500);

                e.Graphics.DrawString(Program.GGG.empresa, fuente_titulo, Brocha, 190, 20);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                //muestra el logo
                Bitmap logo = Sistema.Properties.Resources.logo;
                e.Graphics.DrawImage(logo, 20, 20, 150, 80);

                e.Graphics.DrawString("JC Repuestos", fuente_titulo2, Brocha, 190, 45);

                e.Graphics.DrawString("Dirección: " + Program.GGG.direccion, Fuente, Brocha, 190, 60);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("RTN: " + Program.GGG.RTN + "    Correo: " + Program.GGG.correo, Fuente, Brocha, 190, 75);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Tel: " + Program.GGG.telefono_empresa, Fuente, Brocha, 190, 90);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Codigo", Fuente, Brocha, 20, 176);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Descripcion", Fuente, Brocha, 160, 176);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Cantidad", Fuente, Brocha, 580, 176);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Precio", Fuente, Brocha, 650, 176);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Total", Fuente, Brocha, 740, 176);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                e.Graphics.DrawLine(Pens.Black, 20, 196, 823, 196);


                e.Graphics.DrawString("Cliente:   " + textBox1.Text, Fuente, Brocha, 20, 110);
                e.Graphics.DrawString("RTN:       " + textBox2.Text, Fuente, Brocha, 20, 130);
                e.Graphics.DrawString("Dirección:   " + textBox3.Text, Fuente, Brocha, 20, 150);

                e.Graphics.DrawString("Fecha Limite De Emision:   " + Program.fecha_limite, Fuente, Brocha, 400, 130);
                e.Graphics.DrawString("CAI:           " + Program.cai, Fuente, Brocha, 400, 150);
                e.Graphics.DrawString("Rango:   " + Program.rango, Fuente, Brocha, 400, 110);



                e.Graphics.DrawString("Factura: " + numero_factura2, Fuente_Factura, Brocha, 580, 65);
                e.Graphics.DrawString("Fecha:   " + textBox4.Text, Fuente_Factura, Brocha, 580, 85);

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
                e.Graphics.DrawString("Descuento: " + textBox5.Text, Fuente, Brocha, 23, 466);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("SubTotal: " + textBox6.Text, Fuente, Brocha, 250, 466);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("ISV: " + textBox7.Text, Fuente, Brocha, 450, 466);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Total: " + textBox8.Text, Fuente, Brocha, 640, 466);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                Conv r = new Conv();
                ;
                e.Graphics.DrawString("Total: " + r.enletras(textBox8.Text), Fuente, Brocha, 23, 485);
                e.Graphics.DrawString("Vendedor: " + Program.usuario, Fuente, Brocha, 450, 485);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }      
    }
}
