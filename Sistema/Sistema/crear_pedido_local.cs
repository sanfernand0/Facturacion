using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Globalization;



namespace Sistema
{
    public partial class crear_pedido_local : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        AutoCompleteStringCollection clientes = new AutoCompleteStringCollection();
        AutoCompleteStringCollection codigos = new AutoCompleteStringCollection();
        int codigo, codigo_proveedor;
        DataTable dt = new DataTable("Inventario");

        double tasa, precio1_tasa, precio2_tasa;


        string existe_inventario;
        string existe_compras = "No Existe";


        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/


        public crear_pedido_local()
        {
            InitializeComponent();
            cargarProveedor();
            obtenertasa();
            //obtenerCodigos();
            actualizar();
            //dataGridView1.Visible = false;
            limpiar();
            Tasas();
            checkBox4.Checked = true;
            comboBox2.SelectedIndex = 0;
        }


        public void Tasas()
        {
            try
            {
                textBox13.Clear();
                textBox15.Clear();
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT  *FROM tasa", conexion);
                OleDbDataReader reader2 = comando.ExecuteReader();

                while (reader2.Read())
                {
                    textBox13.Text = (reader2.GetDouble(2)).ToString();
                    textBox15.Text = (reader2.GetDouble(3)).ToString();

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
        public void actualizar()
        {
            try
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                textBox1.Focus();
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT id as Id, grupo as Grupo, marca as Marca, codigo as Codigo, descripcion as Descripcion, aplicacion as Aplicacion, referencia as Referencia, medida as Medida, bodega1, tienda1, costo1 as Costo, precio1 as PrecioVenta, precio2 as PrecioMinimo, exento as Exento FROM inventario order by id ASC", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[4].FillWeight = 300;
                dataGridView1.Columns["PrecioVenta"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["PrecioMinimo"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["Costo"].DefaultCellStyle.Format = "N2";

                dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular);
                UpdateFont();

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
        private void UpdateFont()
        {
            //Change cell font
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular);
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
        public void obtenerCodigos()
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT  *FROM inventario ORDER BY codigo ASC", conexion);
                OleDbDataReader reader2 = comando.ExecuteReader();

                while (reader2.Read())
                {
                    if (reader2.GetString(3) == DBNull.Value.ToString())
                    {

                    }
                    else
                    {
                        codigos.Add(reader2.GetString(3));
                        textBox9.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        textBox9.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        textBox9.AutoCompleteCustomSource = codigos;
                    }

                }
                conexion.Close();
            }
            catch
            {

            }

        }

        public void cargarProveedor()
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT  *FROM proveedores", conexion);
                OleDbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    this.comboBox1.Items.Add(reader.GetString(1));
                    clientes.Add(reader.GetString(1));
                    if (comboBox1.Text == reader.GetString(1))
                    {
                        codigo_proveedor = reader.GetInt32(0);
                    }
                }

                comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                comboBox1.AutoCompleteCustomSource = clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al Cargar Proveedor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                conexion.Close();
            }

        }


        public void obtenertasa()
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT  *FROM tasa", conexion);
                OleDbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.GetInt32(0) == 1)
                    {
                        tasa = reader.GetDouble(1);
                        precio1_tasa = reader.GetDouble(2);
                        precio2_tasa = reader.GetDouble(3);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al Obtener Tasa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            finally
            {
                conexion.Close();
            }

        }

        /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

        private void button1_Click(object sender, EventArgs e)
        {
            //crear proveedor
            try
            {
                fmrAgregarProveedor r = new fmrAgregarProveedor();
                r.FormClosed += new FormClosedEventHandler(crear_pedido_local_FormClosed);
                r.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Al Cargar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //cancelar
            label19.Text = "";
            textBox9.Clear();
            textBox9.Focus();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //salir
            this.Dispose();
            this.Close();
        }


        private void textBox12_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                dateTimePicker1.Focus();
            }
        }

        private void textBox9_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].Selected == true)
                        {
                            existe_inventario = "Existe";
                            try
                            {
                                Program.grupo = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value);

                            }
                            catch
                            {
                                Program.grupo = "";
                            }
                            try
                            {
                                Program.marca = Convert.ToString(dataGridView1.CurrentRow.Cells[2].Value);

                            }
                            catch
                            {
                                Program.marca = "";
                            }
                            try
                            {
                                Program.codigo = Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value.ToString());

                            }
                            catch
                            {
                                Program.codigo = "";
                            }
                            try
                            {
                                Program.descripcion = Convert.ToString(dataGridView1.CurrentRow.Cells[4].Value.ToString());

                            }
                            catch
                            {
                                Program.descripcion = "";
                            }
                            try
                            {
                                Program.aplicacion = Convert.ToString(dataGridView1.CurrentRow.Cells[5].Value.ToString());

                            }
                            catch
                            {
                                Program.aplicacion = "";
                            }
                            try
                            {
                                Program.referencia = Convert.ToString(dataGridView1.CurrentRow.Cells[6].Value.ToString());

                            }
                            catch
                            {
                                Program.referencia = "";
                            }
                            try
                            {
                                Program.medida = Convert.ToString(dataGridView1.CurrentRow.Cells[7].Value.ToString());

                            }
                            catch
                            {
                                Program.medida = "";
                            }

                            try
                            {
                                Program.tienda1 = Convert.ToInt32(dataGridView1.CurrentRow.Cells[9].Value);

                            }
                            catch
                            {
                                Program.tienda1 = 0;
                            }

                            try
                            {
                                Program.bodega1 = Convert.ToInt32(dataGridView1.CurrentRow.Cells[8].Value);

                            }
                            catch
                            {
                                Program.bodega1 = 0;
                            }
                            try
                            {
                                Program.costo1 = Convert.ToDouble(dataGridView1.CurrentRow.Cells[9].Value);
                            }
                            catch
                            {
                                Program.costo1 = 0.00;
                            }

                            try
                            {
                                Program.costo2 = Convert.ToDouble(dataGridView1.CurrentRow.Cells[11].Value);
                            }
                            catch
                            {
                                Program.costo2 = 0.00;
                            }
                            try
                            {
                                Program.precio1 = Convert.ToDouble(dataGridView1.CurrentRow.Cells[10].Value);
                            }
                            catch
                            {
                                Program.precio1 = 0.00;
                            }

                            try
                            {
                                Program.precio2 = Convert.ToDouble(dataGridView1.CurrentRow.Cells[11].Value);

                            }
                            catch
                            {
                                Program.precio2 = 0.00;
                            }





                            textBox1.Text = Program.grupo;
                            textBox2.Text = Program.marca;
                            textBox4.Text = Program.descripcion;
                            textBox5.Text = Program.aplicacion;
                            textBox6.Text = Program.referencia;
                            textBox7.Text = Program.medida;
                            textBox9.Text = Program.codigo;

                            textBox14.Text = Program.bodega1.ToString();
                            //textBox15.Text = Program.tienda1.ToString();

                            textBox16.Text = Program.costo1.ToString("N2");
                            textBox10.Text = Program.precio1.ToString("N2");
                            textBox11.Text = Program.precio2.ToString("N2");
                            textBox3.Focus();
                        }

                    }
                    else
                    {
                        textBox1.Focus();
                        label19.Text = "No Existe, Se Creara Nuevo Codigo";
                        existe_inventario = "No Existe";

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void dateTimePicker1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox9.Focus();
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
                    dataGridView1.DataSource = dv.ToTable();
                }
                else
                if (checkBox2.Checked)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Format("grupo like'%{0}%'", textBox9.Text);
                    dataGridView1.DataSource = dv.ToTable();
                }
                else
                if (checkBox3.Checked)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Format("marca like'%{0}%' ", textBox9.Text);
                    dataGridView1.DataSource = dv.ToTable();
                }
                else
                if (checkBox4.Checked)
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Format("grupo like'%{0}%' OR marca like'%{0}%' OR descripcion like'%{0}%' OR codigo like '%{0}%' OR referencia like '%{0}%' OR aplicacion like '%{0}%'", textBox9.Text);
                    dataGridView1.DataSource = dv.ToTable();
                }
                else
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Format("grupo like'%{0}%' OR marca like'%{0}%' OR descripcion like'%{0}%' OR codigo like '%{0}%' OR referencia like '%{0}%' OR aplicacion like '%{0}%'", textBox9.Text);
                    dataGridView1.DataSource = dv.ToTable();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.CharacterCasing = CharacterCasing.Upper;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.CharacterCasing = CharacterCasing.Upper;
        }


        private void crear_pedido_local_Activated(object sender, EventArgs e)
        {
            if (Program.codigo == "")
            {

            }
            else
            {
                textBox9.Text = Program.codigo;
                textBox1.Text = Program.grupo;
                textBox2.Text = Program.marca;

            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            existe_inventario = "Existe";
            try
            {
                Program.grupo = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value);

            }
            catch
            {
                Program.grupo = "";
            }
            try
            {
                Program.marca = Convert.ToString(dataGridView1.CurrentRow.Cells[2].Value);

            }
            catch
            {
                Program.marca = "";
            }
            try
            {
                Program.codigo = Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value.ToString());

            }
            catch
            {
                Program.codigo = "";
            }
            try
            {
                Program.descripcion = Convert.ToString(dataGridView1.CurrentRow.Cells[4].Value.ToString());

            }
            catch
            {
                Program.descripcion = "";
            }
            try
            {
                Program.aplicacion = Convert.ToString(dataGridView1.CurrentRow.Cells[5].Value.ToString());

            }
            catch
            {
                Program.aplicacion = "";
            }
            try
            {
                Program.referencia = Convert.ToString(dataGridView1.CurrentRow.Cells[6].Value.ToString());

            }
            catch
            {
                Program.referencia = "";
            }
            try
            {
                Program.medida = Convert.ToString(dataGridView1.CurrentRow.Cells[7].Value.ToString());

            }
            catch
            {
                Program.medida = "";
            }

            try
            {
                Program.tienda1 = Convert.ToInt32(dataGridView1.CurrentRow.Cells[9].Value);

            }
            catch
            {
                Program.tienda1 = 0;
            }

            try
            {
                Program.bodega1 = Convert.ToInt32(dataGridView1.CurrentRow.Cells[8].Value);

            }
            catch
            {
                Program.bodega1 = 0;
            }
            try
            {
                Program.costo1 = Convert.ToDouble(dataGridView1.CurrentRow.Cells[10].Value);
            }
            catch
            {
                Program.costo1 = 0.00;
            }

            try
            {
                Program.costo2 = Convert.ToDouble(dataGridView1.CurrentRow.Cells[11].Value);
            }
            catch
            {
                Program.costo2 = 0.00;
            }
            try
            {
                Program.precio1 = Convert.ToDouble(dataGridView1.CurrentRow.Cells[11].Value);
            }
            catch
            {
                Program.precio1 = 0.00;
            }

            try
            {
                Program.precio2 = Convert.ToDouble(dataGridView1.CurrentRow.Cells[12].Value);

            }
            catch
            {
                Program.precio2 = 0.00;
            }





            textBox1.Text = Program.grupo;
            textBox2.Text = Program.marca;
            textBox4.Text = Program.descripcion;
            textBox5.Text = Program.aplicacion;
            textBox6.Text = Program.referencia;
            textBox7.Text = Program.medida;
            textBox9.Text = Program.codigo;
            if (comboBox2.Text == "Bodega1")
            {
                textBox14.Text = Program.bodega1.ToString();
            }
            else
            {
                textBox14.Text = Program.tienda1.ToString();

            }
            textBox16.Text = Program.costo1.ToString("N2");
            textBox10.Text = Program.precio1.ToString("N2");
            textBox11.Text = Program.precio2.ToString("N2");
            textBox3.Focus();



        }

        private void crear_pedido_local_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.grupo = "";
            Program.marca = "";
            Program.codigo = "";
            Program.descripcion = "";
            Program.aplicacion = "";
            Program.referencia = "";
            Program.medida = "";
            Program.precio1 = 0.00;
            Program.precio2 = 0.00;
            Program.costo1 = 0.00;
            Program.costo2 = 0.00;
            Program.tienda1 = 0;
            Program.bodega1 = 0;
        }


        private void comboBox1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.IsBalloon = true;
            toolTip1.SetToolTip(comboBox1, "Si El Proveedor No Existe, Cree uno Nuevo");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Tasas();
            if (comboBox2.Text == "Tienda1")
            {
                try
                {
                    conexion.Open();
                    OleDbCommand comando = new OleDbCommand("SELECT  *FROM inventario  WHERE codigo ='" + textBox9.Text + "'", conexion);
                    OleDbDataReader reader = comando.ExecuteReader();
                    OleDbCommand comando2 = new OleDbCommand("SELECT  *FROM compras WHERE codigo ='" + textBox9.Text + "'", conexion);
                    OleDbDataReader reader2 = comando2.ExecuteReader();

                    if (String.IsNullOrEmpty(comboBox1.Text) ||
                        string.IsNullOrEmpty(textBox3.Text) ||
                        string.IsNullOrEmpty(textBox8.Text) ||
                        string.IsNullOrEmpty(textBox9.Text) ||
                        string.IsNullOrEmpty(textBox12.Text))
                    {
                        MessageBox.Show("Debe llenar unos campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        while (reader.Read())
                        {
                            if (existe_inventario == "Existe")
                            {

                                if (reader.GetString(3) == textBox9.Text && reader.GetString(1) == textBox1.Text && reader.GetString(2) == textBox2.Text)
                                {
                                    int cantidad1 = Convert.ToInt32(textBox3.Text);
                                    int suma1 = reader.GetInt32(9) + cantidad1;
                                    int codi1 = reader.GetInt32(0);
                                    string update = "UPDATE inventario set tienda1 = '" + suma1 + "' ,descripcion = '" + textBox4.Text + "' ,aplicacion = '" + textBox5.Text + "',referencia = '" + textBox6.Text + "',medida = '" + textBox7.Text + "',costo1 = '" + textBox8.Text + "' ,precio1 = '" + textBox10.Text + "',precio2 = '" + textBox11.Text + "' ,exento = " + checkBox5.Checked + " WHERE id=" + codi1 + "";
                                    OleDbCommand comando55 = new OleDbCommand(update, conexion);
                                    comando55.ExecuteNonQuery();
                                }
                            }
                        }
                        if (existe_compras == "No Existe" && existe_inventario == "Existe")
                        {
                            try
                            {
                                int codigo5 = 1;
                                OleDbCommand comando20 = new OleDbCommand("SELECT MAX(id) FROM compras", conexion);
                                if (comando20.ExecuteScalar() != DBNull.Value)
                                {
                                    codigo5 = Convert.ToInt32(comando20.ExecuteScalar());
                                    codigo5 = codigo5 + 1;
                                }

                                string insert3 = "INSERT INTO compras VALUES (@id, @fecha, @numero_factura, @grupo, @marca, @codigo, @descripcion, @cantidad, @costo1, @costo2, @precio, @precio2, @cod_proveedor, @proveedor, @tasa, @ingresado)";
                                OleDbCommand comando7 = new OleDbCommand(insert3, conexion);
                                comando7.Parameters.AddWithValue("@id", codigo5);
                                comando7.Parameters.AddWithValue("@fecha", dateTimePicker1.Value.Date);
                                comando7.Parameters.AddWithValue("@numero_factura", textBox12.Text);
                                comando7.Parameters.AddWithValue("@grupo", textBox1.Text);
                                comando7.Parameters.AddWithValue("@marca", textBox2.Text);
                                comando7.Parameters.AddWithValue("@codigo", textBox9.Text);
                                comando7.Parameters.AddWithValue("@descripcion", textBox4.Text);
                                comando7.Parameters.AddWithValue("@cantidad", textBox3.Text);
                                comando7.Parameters.AddWithValue("@costo1", textBox8.Text);
                                comando7.Parameters.AddWithValue("@costo2", 0.00);
                                comando7.Parameters.AddWithValue("@precio1", textBox10.Text);
                                comando7.Parameters.AddWithValue("@precio2", textBox11.Text);
                                comando7.Parameters.AddWithValue("@cod_proveedor", codigo_proveedor);
                                comando7.Parameters.AddWithValue("@proveedor", comboBox1.Text);
                                comando7.Parameters.AddWithValue("@tasa", tasa);
                                comando7.Parameters.AddWithValue("@ingresado", comboBox2.Text);
                                comando7.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                        }

                        if (existe_compras == "No Existe" && existe_inventario == "No Existe")
                        {
                            try
                            {
                                OleDbCommand comando20 = new OleDbCommand("SELECT MAX(id) FROM compras", conexion);
                                int codigo5 = 1;
                                if (comando20.ExecuteScalar() != DBNull.Value)
                                {
                                    codigo5 = Convert.ToInt32(comando20.ExecuteScalar());
                                    codigo5 = codigo5 + 1;
                                }


                                string insert3 = "INSERT INTO compras VALUES (@id, @fecha, @numero_factura, @grupo, @marca, @codigo, @descripcion, @cantidad, @costo1, @costo2, @precio, @precio2, @cod_proveedor, @proveedor, @tasa, @ingresado)";
                                OleDbCommand comando7 = new OleDbCommand(insert3, conexion);
                                comando7.Parameters.AddWithValue("@id", codigo5);
                                comando7.Parameters.AddWithValue("@fecha", dateTimePicker1.Value.Date);
                                comando7.Parameters.AddWithValue("@numero_factura", textBox12.Text);
                                comando7.Parameters.AddWithValue("@grupo", textBox1.Text);
                                comando7.Parameters.AddWithValue("@marca", textBox2.Text);
                                comando7.Parameters.AddWithValue("@codigo", textBox9.Text);
                                comando7.Parameters.AddWithValue("@descripcion", textBox4.Text);
                                comando7.Parameters.AddWithValue("@cantidad", textBox3.Text);
                                comando7.Parameters.AddWithValue("@costo1", textBox8.Text);
                                comando7.Parameters.AddWithValue("@costo2", 0.00);
                                comando7.Parameters.AddWithValue("@precio1", textBox10.Text);
                                comando7.Parameters.AddWithValue("@precio2", textBox11.Text);
                                comando7.Parameters.AddWithValue("@cod_proveedor", codigo_proveedor);
                                comando7.Parameters.AddWithValue("@proveedor", comboBox1.Text);
                                comando7.Parameters.AddWithValue("@tasa", tasa);
                                comando7.Parameters.AddWithValue("@ingresado", comboBox2.Text);
                                comando7.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                        }

                        if (existe_inventario == "No Existe")
                        {
                            try
                            {
                                int codigo = 1;
                                OleDbCommand comando22 = new OleDbCommand("SELECT MAX(id) FROM inventario", conexion);
                                if (comando22.ExecuteScalar() != DBNull.Value)
                                {
                                    codigo = Convert.ToInt32(comando22.ExecuteScalar());
                                    codigo = codigo + 1;
                                }

                                string insert = "INSERT INTO inventario VALUES (@id, @grupo, @marca, @codigo, @descripcion, @aplicacion, @referencia, @medida, @bodega1, @tienda1, @costo1, @costo2, @precio1, @precio2, @tasa, @exento)";
                                OleDbCommand comando21 = new OleDbCommand(insert, conexion);
                                comando21.Parameters.AddWithValue("@id", codigo);
                                comando21.Parameters.AddWithValue("@grupo", textBox1.Text);
                                comando21.Parameters.AddWithValue("@marca", textBox2.Text);
                                comando21.Parameters.AddWithValue("@codigo", textBox9.Text);
                                comando21.Parameters.AddWithValue("@descripcion", textBox4.Text);
                                comando21.Parameters.AddWithValue("@aplicacion", textBox5.Text);
                                comando21.Parameters.AddWithValue("@referencia", textBox6.Text);
                                comando21.Parameters.AddWithValue("@medida", textBox7.Text);
                                comando21.Parameters.AddWithValue("@bodega1", 0);
                                comando21.Parameters.AddWithValue("@tienda1", textBox3.Text);
                                comando21.Parameters.AddWithValue("@costo1", textBox8.Text);
                                comando21.Parameters.AddWithValue("@costo2", 0.00);
                                comando21.Parameters.AddWithValue("@precio1", textBox10.Text);
                                comando21.Parameters.AddWithValue("@precio2", textBox11.Text);
                                comando21.Parameters.AddWithValue("@tasa", tasa);
                                comando21.Parameters.AddWithValue("@exento", checkBox5.Checked);
                                comando21.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                        }
                        textBox3.Clear();
                        textBox8.Clear();
                        textBox10.Clear();
                        textBox11.Clear();
                        textBox9.Clear();
                        textBox11.Clear();
                        textBox3.Clear();
                        textBox8.Clear();
                        textBox10.Clear();
                        textBox9.Focus();
                        textBox16.Clear();
                        textBox14.Clear();
                        label19.Text = "";
                    }
                }


                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    conexion.Close();
                    actualizar();
                    textBox9.Focus();
                }
            }

            if (comboBox2.Text == "Bodega1")
            {
                try
                {
                    conexion.Open();
                    OleDbCommand comando = new OleDbCommand("SELECT  *FROM inventario  WHERE codigo ='" + textBox9.Text + "'", conexion);
                    OleDbDataReader reader = comando.ExecuteReader();
                    OleDbCommand comando2 = new OleDbCommand("SELECT  *FROM compras WHERE codigo ='" + textBox9.Text + "'", conexion);
                    OleDbDataReader reader2 = comando2.ExecuteReader();

                    if (String.IsNullOrEmpty(comboBox1.Text) ||
                        string.IsNullOrEmpty(textBox3.Text) ||
                        string.IsNullOrEmpty(textBox8.Text) ||
                        string.IsNullOrEmpty(textBox9.Text) ||
                        string.IsNullOrEmpty(textBox12.Text))
                    {
                        MessageBox.Show("Debe llenar unos campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        while (reader.Read())
                        {
                            if (existe_inventario == "Existe")
                            {

                                if (reader.GetString(3) == textBox9.Text && reader.GetString(1) == textBox1.Text && reader.GetString(2) == textBox2.Text)
                                {
                                    int cantidad1 = Convert.ToInt32(textBox3.Text);
                                    int suma1 = reader.GetInt32(8) + cantidad1;
                                    int codi1 = reader.GetInt32(0);
                                    string update = "UPDATE inventario set bodega1 = '" + suma1 + "' ,descripcion = '" + textBox4.Text + "' ,aplicacion = '" + textBox5.Text + "',referencia = '" + textBox6.Text + "',medida = '" + textBox7.Text + "',costo1 = '" + textBox8.Text + "' ,precio1 = '" + textBox10.Text + "',precio2 = '" + textBox11.Text + "' ,exento = " + checkBox5.Checked + " WHERE id=" + codi1 + "";
                                    OleDbCommand comando55 = new OleDbCommand(update, conexion);
                                    comando55.ExecuteNonQuery();
                                }
                            }
                        }
                        if (existe_compras == "No Existe" && existe_inventario == "Existe")
                        {
                            try
                            {
                                int codigo5 = 1;
                                OleDbCommand comando20 = new OleDbCommand("SELECT MAX(id) FROM compras", conexion);
                                if (comando20.ExecuteScalar() != DBNull.Value)
                                {
                                    codigo5 = Convert.ToInt32(comando20.ExecuteScalar());
                                    codigo5 = codigo5 + 1;
                                }

                                string insert3 = "INSERT INTO compras VALUES (@id, @fecha, @numero_factura, @grupo, @marca, @codigo, @descripcion, @cantidad, @costo1, @costo2, @precio, @precio2, @cod_proveedor, @proveedor, @tasa, @ingresado)";
                                OleDbCommand comando7 = new OleDbCommand(insert3, conexion);
                                comando7.Parameters.AddWithValue("@id", codigo5);
                                comando7.Parameters.AddWithValue("@fecha", dateTimePicker1.Value.Date);
                                comando7.Parameters.AddWithValue("@numero_factura", textBox12.Text);
                                comando7.Parameters.AddWithValue("@grupo", textBox1.Text);
                                comando7.Parameters.AddWithValue("@marca", textBox2.Text);
                                comando7.Parameters.AddWithValue("@codigo", textBox9.Text);
                                comando7.Parameters.AddWithValue("@descripcion", textBox4.Text);
                                comando7.Parameters.AddWithValue("@cantidad", textBox3.Text);
                                comando7.Parameters.AddWithValue("@costo1", textBox8.Text);
                                comando7.Parameters.AddWithValue("@costo2", 0.00);
                                comando7.Parameters.AddWithValue("@precio1", textBox10.Text);
                                comando7.Parameters.AddWithValue("@precio2", textBox11.Text);
                                comando7.Parameters.AddWithValue("@cod_proveedor", codigo_proveedor);
                                comando7.Parameters.AddWithValue("@proveedor", comboBox1.Text);
                                comando7.Parameters.AddWithValue("@tasa", tasa);
                                comando7.Parameters.AddWithValue("@ingresado", comboBox2.Text);
                                comando7.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                        }

                        if (existe_compras == "No Existe" && existe_inventario == "No Existe")
                        {
                            try
                            {
                                OleDbCommand comando20 = new OleDbCommand("SELECT MAX(id) FROM compras", conexion);
                                int codigo5 = 1;
                                if (comando20.ExecuteScalar() != DBNull.Value)
                                {
                                    codigo5 = Convert.ToInt32(comando20.ExecuteScalar());
                                    codigo5 = codigo5 + 1;
                                }


                                string insert3 = "INSERT INTO compras VALUES (@id, @fecha, @numero_factura, @grupo, @marca, @codigo, @descripcion, @cantidad, @costo1, @costo2, @precio, @precio2, @cod_proveedor, @proveedor, @tasa, @ingresado)";
                                OleDbCommand comando7 = new OleDbCommand(insert3, conexion);
                                comando7.Parameters.AddWithValue("@id", codigo5);
                                comando7.Parameters.AddWithValue("@fecha", dateTimePicker1.Value.Date);
                                comando7.Parameters.AddWithValue("@numero_factura", textBox12.Text);
                                comando7.Parameters.AddWithValue("@grupo", textBox1.Text);
                                comando7.Parameters.AddWithValue("@marca", textBox2.Text);
                                comando7.Parameters.AddWithValue("@codigo", textBox9.Text);
                                comando7.Parameters.AddWithValue("@descripcion", textBox4.Text);
                                comando7.Parameters.AddWithValue("@cantidad", textBox3.Text);
                                comando7.Parameters.AddWithValue("@costo1", textBox8.Text);
                                comando7.Parameters.AddWithValue("@costo2", 0.00);
                                comando7.Parameters.AddWithValue("@precio1", textBox10.Text);
                                comando7.Parameters.AddWithValue("@precio2", textBox11.Text);
                                comando7.Parameters.AddWithValue("@cod_proveedor", codigo_proveedor);
                                comando7.Parameters.AddWithValue("@proveedor", comboBox1.Text);
                                comando7.Parameters.AddWithValue("@tasa", tasa);
                                comando7.Parameters.AddWithValue("@ingresado", comboBox2.Text);
                                comando7.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                        }

                        if (existe_inventario == "No Existe")
                        {
                            try
                            {
                                int codigo = 1;
                                OleDbCommand comando22 = new OleDbCommand("SELECT MAX(id) FROM inventario", conexion);
                                if (comando22.ExecuteScalar() != DBNull.Value)
                                {
                                    codigo = Convert.ToInt32(comando22.ExecuteScalar());
                                    codigo = codigo + 1;
                                }

                                string insert = "INSERT INTO inventario VALUES (@id, @grupo, @marca, @codigo, @descripcion, @aplicacion, @referencia, @medida, @bodega1, @tienda1, @costo1, @costo2, @precio1, @precio2, @tasa, @exento)";
                                OleDbCommand comando21 = new OleDbCommand(insert, conexion);
                                comando21.Parameters.AddWithValue("@id", codigo);
                                comando21.Parameters.AddWithValue("@grupo", textBox1.Text);
                                comando21.Parameters.AddWithValue("@marca", textBox2.Text);
                                comando21.Parameters.AddWithValue("@codigo", textBox9.Text);
                                comando21.Parameters.AddWithValue("@descripcion", textBox4.Text);
                                comando21.Parameters.AddWithValue("@aplicacion", textBox5.Text);
                                comando21.Parameters.AddWithValue("@referencia", textBox6.Text);
                                comando21.Parameters.AddWithValue("@medida", textBox7.Text);
                                comando21.Parameters.AddWithValue("@bodega1", textBox3.Text);
                                comando21.Parameters.AddWithValue("@tienda1", 0);
                                comando21.Parameters.AddWithValue("@costo1", textBox8.Text);
                                comando21.Parameters.AddWithValue("@costo2", 0.00);
                                comando21.Parameters.AddWithValue("@precio1", textBox10.Text);
                                comando21.Parameters.AddWithValue("@precio2", textBox11.Text);
                                comando21.Parameters.AddWithValue("@tasa", tasa);
                                comando21.Parameters.AddWithValue("@exento", checkBox5.Checked);
                                comando21.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                        }
                        textBox3.Clear();
                        textBox8.Clear();
                        textBox10.Clear();
                        textBox11.Clear();
                        textBox9.Clear();
                        textBox11.Clear();
                        textBox3.Clear();
                        textBox8.Clear();
                        textBox10.Clear();
                        textBox9.Focus();
                        textBox16.Clear();
                        textBox14.Clear();
                        label19.Text = "";
                    }
                }


                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    conexion.Close();
                    actualizar();
                    textBox9.Focus();
                }
            }
        }
        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT count(*) FROM inventario  WHERE codigo ='" + textBox9.Text + "' AND grupo ='" + textBox1.Text + "' AND marca ='" + textBox2.Text + "'", conexion);
                int existe = Convert.ToInt32(comando.ExecuteScalar());
                conexion.Close();

                if (existe == 0)
                {
                    label19.Text = "No Existe, Se Creara Nuevo Codigo";
                    existe_inventario = "No Existe";
                    textBox14.Clear();
                    textBox16.Clear();
                    textBox10.Clear();
                    textBox11.Clear();
                }
                else
                {
                    existe_inventario = "Existe";
                    label19.Text = "";

                }
                textBox2.Focus();


            }
        }
        private void textBox2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT count(*) FROM inventario  WHERE codigo ='" + textBox9.Text + "' AND grupo ='" + textBox1.Text + "' AND marca ='" + textBox2.Text + "'", conexion);
                int existe = Convert.ToInt32(comando.ExecuteScalar());
                conexion.Close();

                if (existe == 0)
                {
                    label19.Text = "No Existe, Se Creara Nuevo Codigo";
                    existe_inventario = "No Existe";
                    textBox14.Clear();
                    textBox16.Clear();
                    textBox10.Clear();
                    textBox11.Clear();
                }
                else
                {
                    existe_inventario = "Existe";
                    label19.Text = "";

                }
                textBox4.Focus();
            }
        }
        private void textBox4_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox5.Focus();
            }
        }
        private void textBox5_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox6.Focus();
            }
        }
        private void textBox6_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox7.Focus();
            }
        }
        private void textBox7_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox3.Focus();
            }
        }
        private void textBox3_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox8.Focus();
            }
        }
        private void textBox8_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                CultureInfo cc = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.' || e.KeyChar == ',')
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }

                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (textBox8.Text == "")
                    {
                    }
                    else
                    {
                        e.Handled = true;
                        double valor = (Convert.ToDouble(textBox8.Text) * (Convert.ToDouble(textBox13.Text) / 100)) + Convert.ToDouble(textBox8.Text);
                        double valor2 = (Convert.ToDouble(textBox8.Text) * (Convert.ToDouble(textBox15.Text) / 100)) + Convert.ToDouble(textBox8.Text);

                        textBox10.Text = valor.ToString("N2");
                        textBox11.Text = valor2.ToString("N2");
                        double i = Convert.ToDouble(textBox8.Text);
                        textBox8.Text = i.ToString("N2");
                        textBox10.Focus();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void textBox10_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                CultureInfo cc = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (char.IsNumber(e.KeyChar) ||
                e.KeyChar.ToString() == cc.NumberFormat.NumberDecimalSeparator || e.KeyChar == (char)Keys.Back)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    textBox11.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox11_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                CultureInfo cc = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (char.IsNumber(e.KeyChar) ||
                e.KeyChar.ToString() == cc.NumberFormat.NumberDecimalSeparator || e.KeyChar == (char)Keys.Back)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    button2.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void crear_pedido_local_FormClosed(object sender, FormClosedEventArgs e)
        {
            comboBox1.Items.Clear();
            cargarProveedor();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.SelectionStart = 0;
                textBox1.SelectionLength = textBox1.Text.Length;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.SelectionStart = 0;
                textBox2.SelectionLength = textBox2.Text.Length;
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox4.Text))
            {
                textBox4.SelectionStart = 0;
                textBox4.SelectionLength = textBox4.Text.Length;
            }
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox5.Text))
            {
                textBox5.SelectionStart = 0;
                textBox5.SelectionLength = textBox5.Text.Length;
            }
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox6.Text))
            {
                textBox6.SelectionStart = 0;
                textBox6.SelectionLength = textBox6.Text.Length;
            }
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox7.Text))
            {
                textBox7.SelectionStart = 0;
                textBox7.SelectionLength = textBox7.Text.Length;
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox3.Text))
            {
                textBox3.SelectionStart = 0;
                textBox3.SelectionLength = textBox3.Text.Length;
            }
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox10.Text))
            {
                textBox10.SelectionStart = 0;
                textBox10.SelectionLength = textBox10.Text.Length;
            }
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox11.Text))
            {
                textBox11.SelectionStart = 0;
                textBox11.SelectionLength = textBox11.Text.Length;
            }
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox9.Text))
            {
                textBox9.SelectionStart = 0;
                textBox9.SelectionLength = textBox9.Text.Length;
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                CultureInfo cc = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {

            try
            {
                CultureInfo cc = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox9.Focus();
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    textBox12.Focus();
                }

                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT  *FROM proveedores", conexion);
                OleDbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    if (comboBox1.Text == reader.GetString(1))
                    {
                        codigo_proveedor = reader.GetInt32(0);
                    }
                }
                label15.Text = "COD: " + codigo_proveedor;
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT  *FROM proveedores", conexion);
                OleDbDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    if (comboBox1.Text == reader.GetString(1))
                    {
                        codigo_proveedor = reader.GetInt32(0);
                    }
                }
                label15.Text = "COD: " + codigo_proveedor;
                conexion.Close();
                textBox12.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
