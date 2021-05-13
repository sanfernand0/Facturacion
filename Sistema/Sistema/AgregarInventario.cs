using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Globalization;


namespace Sistema
{
    public partial class AgregarInventario : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);

        public AgregarInventario()
        {
            InitializeComponent();
            txtGrupo.Focus();
        }

        public int actualizar()
        {
            conexion.Open();
            OleDbCommand comando = new OleDbCommand("SELECT COUNT(id) FROM inventario where grupo = '"+txtGrupo.Text+ "' AND marca = '" + txtMarca.Text + "' AND codigo = '" + txtCodigo.Text + "'", conexion);
            int total = 0;
            total = (int)comando.ExecuteScalar();
            conexion.Close();
            return total;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int codigo = 1;
            try
            {
                if (string.IsNullOrEmpty(txtGrupo.Text))
                {
                    MessageBox.Show("Ingrese el Grupo del producto", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtGrupo.Focus();
                }
                else if (string.IsNullOrEmpty(txtMarca.Text))
                {
                    MessageBox.Show("Ingrese la Marca del producto", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMarca.Focus();
                }
                else if (string.IsNullOrEmpty(txtCodigo.Text))
                {
                    MessageBox.Show("Ingrese el codigo del producto", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCodigo.Focus();

                }
                 else if (string.IsNullOrEmpty(txtCosto1.Text))
                {
                    MessageBox.Show("Ingrese el costo del producto", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCosto1.Focus();

                }
                else if (string.IsNullOrEmpty(txtBodega1.Text))
                {
                    MessageBox.Show("Ingrese la cantidad en Bodega", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBodega1.Focus();
                }


                else if (string.IsNullOrEmpty(txtPrecio1.Text))
                {
                    MessageBox.Show("Ingrese el precio1 del producto", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrecio1.Focus();
                }
                else if (string.IsNullOrEmpty(txtPrecio2.Text))
                {
                    MessageBox.Show("Ingrese el precio2 del producto", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrecio2.Focus();
                }
                else if (string.IsNullOrEmpty(txtTienda.Text))
                {
                    MessageBox.Show("Ingrese la cantidad en Tienda, si no ingresara nada ingrese 0", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrecio2.Focus();
                }
                else
                {

                    if (actualizar() == 0)
                    {
                        conexion.Open();

                        OleDbCommand comando22 = new OleDbCommand("SELECT MAX(id) FROM inventario", conexion);
                        if (comando22.ExecuteScalar() != DBNull.Value)
                        {
                            codigo = Convert.ToInt32(comando22.ExecuteScalar());
                            codigo = codigo + 1;
                        }

                        string insert = "INSERT INTO inventario VALUES (@id, @grupo, @marca, @codigo, @descripcion, @aplicacion, @referencia, @medida, @bodega1, @tienda1, @costo1, @costo2, @precio1, @precio2, @tasa, @exento)";
                        OleDbCommand comando21 = new OleDbCommand(insert, conexion);
                        comando21.Parameters.AddWithValue("@id", codigo);
                        comando21.Parameters.AddWithValue("@grupo", txtGrupo.Text);
                        comando21.Parameters.AddWithValue("@marca", txtMarca.Text);
                        comando21.Parameters.AddWithValue("@codigo", txtCodigo.Text);
                        comando21.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                        comando21.Parameters.AddWithValue("@aplicacion", txtAplicacion.Text);
                        comando21.Parameters.AddWithValue("@referencia", txtRefencia.Text);
                        comando21.Parameters.AddWithValue("@medida", txtMedida.Text);
                        comando21.Parameters.AddWithValue("@bodega1", txtBodega1.Text);
                        comando21.Parameters.AddWithValue("@tienda1", txtTienda.Text);
                        comando21.Parameters.AddWithValue("@costo1", txtCosto1.Text);
                        comando21.Parameters.AddWithValue("@costo2", decimal.Parse("0.00"));
                        comando21.Parameters.AddWithValue("@precio1", txtPrecio1.Text);
                        comando21.Parameters.AddWithValue("@precio2", txtPrecio2.Text);
                        comando21.Parameters.AddWithValue("@tasa", decimal.Parse("1.00"));
                        comando21.Parameters.AddWithValue("@exento", checkBox1.Checked);
                        comando21.ExecuteNonQuery();
                        MessageBox.Show("Registro Ingresado correctamente", "Bien", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conexion.Close();
                        this.Close();
                    }
                }

            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conexion.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public void valor(TextBox t)
        {
            if (string.IsNullOrEmpty(t.Text))
            {
                t.Text = "0";
            }
        }
        public void costos(TextBox t)
        {
            if (string.IsNullOrEmpty(t.Text))
            {
                t.Text = "0.00";
            }
        }

        private void txtBodega1_Leave(object sender, EventArgs e)
        {
            valor(txtBodega1);
        }

 

        private void txtCosto1_Leave(object sender, EventArgs e)
        {
            costos(txtCosto1);
        }

        private void txtPrecio1_Leave(object sender, EventArgs e)
        {
            costos(txtPrecio1);
        }

        private void txtPrecio2_Leave(object sender, EventArgs e)
        {
            costos(txtPrecio2);
        }

        public static void comprobar(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
            try
            {
                if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.')
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

        private void txtBodega1_KeyPress(object sender, KeyPressEventArgs e)
        {
            comprobar(sender, e);
        }

        private void txtCosto1_KeyPress(object sender, KeyPressEventArgs e)
        {
            comprobar(sender, e);
        }

        private void txtPrecio1_KeyPress(object sender, KeyPressEventArgs e)
        {
            comprobar(sender, e);
        }

        private void txtPrecio2_KeyPress(object sender, KeyPressEventArgs e)
        {
            comprobar(sender, e);
        }

        private void txtTienda_KeyPress(object sender, KeyPressEventArgs e)
        {
            comprobar(sender, e);
        }
    }
}
