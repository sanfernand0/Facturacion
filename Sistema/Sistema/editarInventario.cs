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
    public partial class editarInventario : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);

        public editarInventario()
        {
            InitializeComponent();
            txtGrupo.Focus();
        }
        public editarInventario(int id, string grupo, string marca, string codigo, 
            string descripcion, string aplicacion, string referencia, string medida, int bodega1, int tienda1,
            decimal costo1,  decimal precio1, decimal precio2, bool exento)
        {
            InitializeComponent();
            txtId.Text = id.ToString();
            txtGrupo.Text = grupo;
            txtMarca.Text = marca;
            txtCodigo.Text = codigo;
            txtDescripcion.Text = descripcion;
            txtAplicacion.Text = aplicacion;
            txtRefencia.Text = referencia;
            txtMedida.Text = medida;
            txtBodega1.Text = bodega1.ToString();
            txtTienda.Text = tienda1.ToString();
            txtCosto1.Text = costo1.ToString("N2");
            txtPrecio1.Text = precio1.ToString("N2");
            txtPrecio2.Text = precio2.ToString("N2");
            checkBox1.Checked = exento;
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
                    MessageBox.Show("Ingrese el Codigo del producto", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCodigo.Focus();

                }
                else if (string.IsNullOrEmpty(txtCosto1.Text))
                {
                    MessageBox.Show("Ingrese el Costo del producto", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCosto1.Focus();

                }
                else if (string.IsNullOrEmpty(txtBodega1.Text))
                {
                    MessageBox.Show("Ingrese la Cantidad en Bodega", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBodega1.Focus();
                }


                else if (string.IsNullOrEmpty(txtPrecio1.Text))
                {
                    MessageBox.Show("Ingrese el Precio De Venta del producto", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrecio1.Focus();
                }
                else if (string.IsNullOrEmpty(txtPrecio2.Text))
                {
                    MessageBox.Show("Ingrese el Precio Minimo del producto", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrecio2.Focus();
                }
                else if (string.IsNullOrEmpty(txtTienda.Text))
                {
                    MessageBox.Show("Ingrese la cantidad en Tienda, si no ingresara nada ingrese 0", "Campos Vacios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPrecio2.Focus();
                }
                else
                {
                    conexion.Open();
                    //string update = "UPDATE inventario set grupo = '" + txtGrupo.Text + "', marca = '" + txtMarca.Text + "', codigo = '" + txtCodigo.Text + "', descripcion = '" + txtDescripcion.Text + "', aplicacion = '" +
                    //    txtAplicacion.Text + "', referencia = '" + txtRefencia.Text + "', medida = '" + txtMedida.Text + "', bodega1 = " + txtBodega1.Text + ", costo1 = " + txtCosto1.Text +
                    //    " ,precio1 = " + txtPrecio1.Text + ", precio2 = " + txtPrecio2.Text + " , exento = '" + checkBox1.Checked + "' WHERE id=" + txtId.Text + "";

                    string update = "UPDATE inventario SET grupo=@grupo, marca=@marca, codigo=@codigo, descripcion=@descripcion, aplicacion = @aplicacion, referencia= @referencia, medida=@medida, bodega1=@bodega1, tienda1=@tienda1, costo1=@costo1, costo2=@costo2, precio1=@precio1, precio2=@precio2, tasa=@tasa, exento=@exento where id = @id";
                    OleDbCommand comando21 = new OleDbCommand(update, conexion);

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
                    comando21.Parameters.AddWithValue("@id", txtId.Text);
                    comando21.ExecuteNonQuery();

                    comando21.ExecuteNonQuery();
                    MessageBox.Show("Articulo actualizado con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conexion.Close();
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conexion.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editarInventario_FormClosed(object sender, FormClosedEventArgs e)
        {

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

        private void txtBodega1_KeyPress(object sender, KeyPressEventArgs e)
        {
            AgregarInventario.comprobar(sender, e);
        }

        private void txtCosto1_KeyPress(object sender, KeyPressEventArgs e)
        {
            AgregarInventario.comprobar(sender, e);
        }

        private void txtPrecio1_KeyPress(object sender, KeyPressEventArgs e)
        {
            AgregarInventario.comprobar(sender, e);
        }

        private void txtPrecio2_KeyPress(object sender, KeyPressEventArgs e)
        {
            AgregarInventario.comprobar(sender, e);
        }

        private void txtTienda_KeyPress(object sender, KeyPressEventArgs e)
        {
            AgregarInventario.comprobar(sender, e);
        }
    }
}
