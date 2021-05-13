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
    public partial class traslado1 : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        AutoCompleteStringCollection clientes = new AutoCompleteStringCollection();
        DataTable dt = new DataTable("Inventario");
        double subtotal, totaltotal;
        int cantidad = 0;
        double precio = 0;
        string numero_traslado;

        string grupo, marca, codigo, descripcion;
        int cantidadfactura;
        double preciofactura;

        public traslado1()
        {
            InitializeComponent();
            actualizar();
            textBox4.Text = DateTime.Now.ToShortDateString();
            limpiar();
            label12.Text = "Usuario: " + Program.usuario;
            obtenerDatos();
            dataGridView2.Columns[4].FillWeight = 250;
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
            this.Close();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("grupo like'%{0}%' OR marca like'%{0}%' OR descripcion like'%{0}%' OR codigo like '%{0}%' OR referencia like '%{0}%'", textBox9.Text);
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
                totaltotal = 0;
                textBox8.Clear();
                textBox9.Clear();
                textBox9.Focus();



                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    subtotal += Convert.ToDouble(row.Cells[4].Value);

                }
                totaltotal = subtotal;
                textBox8.Text = totaltotal.ToString("N2");
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
                        totaltotal = 0;

                        textBox8.Clear();
                        textBox9.Clear();
                        textBox9.Focus();



                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            subtotal += Convert.ToDouble(row.Cells[4].Value);

                        }
                        totaltotal = subtotal;
                        textBox8.Text = totaltotal.ToString("N2");

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
                int Y = 45;
                //se genera el cuadro de los clientes
                e.Graphics.DrawRectangle(Pens.Black, 20, 130, 805, 20);

                //se genera el cuadro de la fecha y #cotizacion
                e.Graphics.DrawRectangle(Pens.Black, 600, 45, 224, 40);


                //se genera el cuadro de los articulos
                e.Graphics.DrawRectangle(Pens.Black, 20, 155, 805, 285);

                //se genera el cuadro de los totales
                e.Graphics.DrawRectangle(Pens.Black, 20, 445, 805, 25);

                e.Graphics.DrawString(Program.GGG.empresa, fuente_titulo, Brocha, 190, 20);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                //muestra el logo
                Bitmap logo = Sistema.Properties.Resources.logo;
                e.Graphics.DrawImage(logo, 20, 20, 150, 100);

                e.Graphics.DrawString("Tel: " + Program.GGG.telefono_empresa, Fuente, Brocha, 190, 45);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("RTN: " + Program.GGG.RTN, Fuente, Brocha, 190, 65);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Dirección: " + Program.GGG.direccion, Fuente, Brocha, 190, 85);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Codigo", Fuente, Brocha, 20, 156);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Descripcion", Fuente, Brocha, 160, 156);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Cantidad", Fuente, Brocha, 580, 156);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Precio", Fuente, Brocha, 650, 156);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Total", Fuente, Brocha, 740, 156);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());


                e.Graphics.DrawString("Vendedor: " + Program.usuario, Fuente, Brocha, 20, 130);

                e.Graphics.DrawString("Traslado: " + numero_traslado, Fuente_Factura, Brocha, 600, 45);
                e.Graphics.DrawString("Fecha:   " + textBox4.Text, Fuente_Factura, Brocha, 600, 65);

                e.Graphics.DrawLine(Pens.Black, 20, 175, 823, 175);
                //se genera el cuadro de los articulos
                foreach (DataGridViewRow row in dataGridView1.Rows)

                {
                    if (contador <= 12)
                    {
                        string codigo = row.Cells[0].Value.ToString();
                        string descripcion = row.Cells[1].Value.ToString();
                        int cantidad = Convert.ToInt32(row.Cells[2].Value);
                        double precio = Convert.ToDouble(row.Cells[3].Value);
                        double total2 = Convert.ToDouble(row.Cells[4].Value);
                        e.Graphics.DrawString(codigo, Fuente, Brocha, 23, Y);
                        e.Graphics.DrawString(descripcion, Fuente, Brocha, 160, Y);
                        e.Graphics.DrawString(cantidad.ToString(), Fuente, Brocha, 590, Y);
                        e.Graphics.DrawString(precio.ToString("N2"), Fuente, Brocha, 650, Y);
                        e.Graphics.DrawString(total2.ToString("N2"), Fuente, Brocha, 740, Y);
                        Y = Y + 20;
                    }
                }

                e.Graphics.DrawString("Total: " + textBox8.Text, Fuente, Brocha, 640, 446);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                conexion.Open();
                //para insertar en traslado
                try
                {

                    OleDbCommand comando9 = new OleDbCommand("SELECT MAX(numero_traslado) FROM datos", conexion);
                    numero_traslado = Convert.ToString(comando9.ExecuteScalar());

                    int siguiente = Convert.ToInt32(numero_traslado) + 1;
                    string update22 = "UPDATE datos set numero_traslado = '" + siguiente + "' WHERE Id=" + 1 + "";
                    OleDbCommand comando22 = new OleDbCommand(update22, conexion);
                    comando22.ExecuteNonQuery();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {

                        if (comboBox1.Text == "Bodega1")
                        {
                            int resta, suma;
                            suma = Convert.ToInt32(row.Cells[5].Value) - Convert.ToInt32(row.Cells[2].Value);
                            resta = Convert.ToInt32(row.Cells[6].Value) + Convert.ToInt32(row.Cells[2].Value);
                            string update = "UPDATE inventario set bodega1 = '" + suma + "' WHERE id=" + Convert.ToInt32(row.Cells[7].Value) + "";
                            string update2 = "UPDATE inventario set tienda1 = '" + resta + "' WHERE id=" + Convert.ToInt32(row.Cells[7].Value) + "";

                            OleDbCommand comando2 = new OleDbCommand(update, conexion);
                            OleDbCommand comando1 = new OleDbCommand(update2, conexion);
                            comando1.ExecuteNonQuery();
                            comando2.ExecuteNonQuery();
                        }
                        if (comboBox1.Text == "Tienda1")
                        {

                            int resta, suma;
                            suma = Convert.ToInt32(row.Cells[5].Value) + Convert.ToInt32(row.Cells[2].Value);
                            resta = Convert.ToInt32(row.Cells[6].Value) - Convert.ToInt32(row.Cells[2].Value);
                            string update = "UPDATE inventario set bodega1 = '" + suma + "' WHERE id=" + Convert.ToInt32(row.Cells[7].Value) + "";
                            string update2 = "UPDATE inventario set tienda1 = '" + resta + "' WHERE id=" + Convert.ToInt32(row.Cells[7].Value) + "";

                            OleDbCommand comando2 = new OleDbCommand(update, conexion);
                            OleDbCommand comando1 = new OleDbCommand(update2, conexion);
                            comando1.ExecuteNonQuery();
                            comando2.ExecuteNonQuery();
                        }
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
                            descripcion = Convert.ToString(row.Cells[1].Value);

                        }
                        catch
                        {
                            descripcion = "";
                        }

                        try
                        {
                            cantidadfactura = Convert.ToInt32(row.Cells[2].Value);

                        }
                        catch
                        {
                            cantidadfactura = 0;
                        }

                        try
                        {
                            preciofactura = Convert.ToInt32(row.Cells[3].Value);

                        }
                        catch
                        {
                            preciofactura = 0;
                        }


                        int codigo5 = 1;
                        OleDbCommand comando20 = new OleDbCommand("SELECT MAX(id) FROM traslado", conexion);
                        if (comando20.ExecuteScalar() != DBNull.Value)
                        {
                            codigo5 = Convert.ToInt32(comando20.ExecuteScalar());
                            codigo5 = codigo5 + 1;
                        }
                        string insert3 = "INSERT INTO traslado VALUES (@Id, @fecha, @numero_traslado, @grupo, @marca, @codigo, @descripcion, @cantidad, @precio, @total, @de, @para, @vendedor, @fecha_anulacion)";
                        OleDbCommand comando7 = new OleDbCommand(insert3, conexion);
                        comando7.Parameters.AddWithValue("@Id", codigo5);
                        comando7.Parameters.AddWithValue("@fecha", textBox4.Text);
                        comando7.Parameters.AddWithValue("@numero_traslado", numero_traslado);
                        comando7.Parameters.AddWithValue("@grupo", grupo);
                        comando7.Parameters.AddWithValue("@marca", marca);
                        comando7.Parameters.AddWithValue("@codigo", codigo);
                        comando7.Parameters.AddWithValue("@descripcion", descripcion);
                        comando7.Parameters.AddWithValue("@cantidad", cantidad);
                        comando7.Parameters.AddWithValue("@precio", preciofactura);
                        comando7.Parameters.AddWithValue("@total", textBox8.Text);
                        comando7.Parameters.AddWithValue("@de", comboBox1.Text);
                        comando7.Parameters.AddWithValue("@para", comboBox2.Text);
                        comando7.Parameters.AddWithValue("@vendedor", Program.usuario);
                        comando7.Parameters.AddWithValue("@fecha_anulacion", "00");

                        comando7.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                { 
                    MessageBox.Show(ex.ToString());
                }
            
                //para rebajar las cantidades de inventario
                if (DialogResult.Yes == MessageBox.Show("Desea imprimir el traslado", "Imprimir Traslado?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                printDocument1.Print();
                    this.Close();
                } else
                {
                    this.Close();
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



        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                comboBox2.Focus();
            }
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
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
                                        Program.tienda1 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[8].Value);

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
                                        Program.cantidad = Convert.ToInt32(dataGridView2.CurrentRow.Cells[7].Value);

                                    }
                                    catch
                                    {
                                        Program.cantidad = 0;
                                    }

                                    try
                                    {
                                        Program.cantidad2 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[8].Value);
                                    }
                                    catch
                                    {
                                        Program.cantidad2 = 0;
                                    }


                                    int rowEscribir = dataGridView1.Rows.Count;
                                    if (dataGridView1.Rows.Count < 13)
                                    {
                                        dataGridView1.Rows.Add(1);
                                        dataGridView1.Focus();
                                        dataGridView1.Rows[rowEscribir].Cells[0].Value = Program.codigo;
                                        dataGridView1.Rows[rowEscribir].Cells[1].Value = Program.descripcion;
                                        dataGridView1.Rows[rowEscribir].Cells[3].Value = Program.precio1;
                                        dataGridView1.Rows[rowEscribir].Cells[5].Value = Program.cantidad;
                                        dataGridView1.Rows[rowEscribir].Cells[6].Value = Program.cantidad2;
                                        dataGridView1.Rows[rowEscribir].Cells[7].Value = Program.id;
                                        dataGridView1.Rows[rowEscribir].Cells[8].Value = Program.grupo;
                                        dataGridView1.Rows[rowEscribir].Cells[9].Value = Program.marca;

                                        dataGridView1.CurrentCell = dataGridView1.Rows[rowEscribir].Cells[2];
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
                                    Program.tienda1 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[8].Value);

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
                                try
                                {
                                    Program.cantidad = Convert.ToInt32(dataGridView2.CurrentRow.Cells[7].Value);

                                }
                                catch
                                {
                                    Program.cantidad = 0;
                                }

                                try
                                {
                                    Program.cantidad2 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[8].Value);
                                }
                                catch
                                {
                                    Program.cantidad2 = 0;
                                }


                                int rowEscribir = dataGridView1.Rows.Count;
                                if (dataGridView1.Rows.Count < 13)
                                {

                                    dataGridView1.Rows.Add(1);
                                    dataGridView1.Focus();
                                    dataGridView1.Rows[rowEscribir].Cells[0].Value = Program.codigo;
                                    dataGridView1.Rows[rowEscribir].Cells[1].Value = Program.descripcion;
                                    dataGridView1.Rows[rowEscribir].Cells[3].Value = Program.precio1;
                                    dataGridView1.Rows[rowEscribir].Cells[5].Value = Program.cantidad;
                                    dataGridView1.Rows[rowEscribir].Cells[6].Value = Program.cantidad2;
                                    dataGridView1.Rows[rowEscribir].Cells[7].Value = Program.id;
                                    dataGridView1.Rows[rowEscribir].Cells[8].Value = Program.grupo;
                                    dataGridView1.Rows[rowEscribir].Cells[9].Value = Program.marca;
                                    dataGridView1.CurrentCell = dataGridView1.Rows[rowEscribir].Cells[2];
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Bodega1")
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("Tienda1");
                comboBox2.SelectedIndex = 0;
            }
            else
            {
                if (comboBox1.Text == "Tienda1")
                {
                    comboBox2.Items.Clear();
                    comboBox2.Items.Add("Bodega1");
                    comboBox2.SelectedIndex = 0;
                }
            }
            textBox9.Clear();
            textBox9.Focus();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                comboBox1.Focus();
                comboBox1.DroppedDown = true;
            }
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == (char)Keys.Down)
            {
                dataGridView2.Focus();
            }
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
                    Program.tienda1 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[8].Value);

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
                    Program.cantidad = Convert.ToInt32(dataGridView2.CurrentRow.Cells[7].Value);

                }
                catch
                {
                    Program.cantidad = 0;
                }

                try
                {
                    Program.cantidad2 = Convert.ToInt32(dataGridView2.CurrentRow.Cells[8].Value);
                }
                catch
                {
                    Program.cantidad2 = 0;
                }

                int rowEscribir = dataGridView1.Rows.Count;
                if (Program.codigo != "")
                {
                    if (dataGridView1.Rows.Count < 13)
                    {

                        dataGridView1.Rows.Add(1);
                        dataGridView1.Focus();
                        dataGridView1.Rows[rowEscribir].Cells[0].Value = Program.codigo;
                        dataGridView1.Rows[rowEscribir].Cells[1].Value = Program.descripcion;
                        dataGridView1.Rows[rowEscribir].Cells[3].Value = Program.precio1;
                        dataGridView1.Rows[rowEscribir].Cells[5].Value = Program.cantidad;
                        dataGridView1.Rows[rowEscribir].Cells[6].Value = Program.cantidad2;
                        dataGridView1.Rows[rowEscribir].Cells[7].Value = Program.id;
                        dataGridView1.Rows[rowEscribir].Cells[8].Value = Program.grupo;
                        dataGridView1.Rows[rowEscribir].Cells[9].Value = Program.marca;
                        dataGridView1.CurrentCell = dataGridView1.Rows[rowEscribir].Cells[2];

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
                    totaltotal = 0;
                    textBox8.Clear();

                    cantidad = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    precio = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());

                    dataGridView1.Rows[e.RowIndex].Cells[4].Value = cantidad * precio;
                    textBox9.Clear();
                    textBox9.Focus();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        subtotal += Convert.ToDouble(row.Cells[4].Value);
                    }

                    totaltotal = subtotal;
                    textBox8.Text = totaltotal.ToString("N2");
                    dataGridView1.ClearSelection();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
