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
    public partial class fmrEditarUsuarios : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);

        public fmrEditarUsuarios()
        {
            InitializeComponent();
            txtNombre.Focus();
        }

        public fmrEditarUsuarios(int id, string nombre, string tipo, string iniciales, 
            string identidad, string direccion, string telefono, string celular, string correo, string clave)
        {
            InitializeComponent();
            txtId.Text = id.ToString();
            txtNombre.Text = nombre;
            txtTipo.Text = tipo;
            txtInciales.Text = iniciales;
            txtIdentidad.Text = identidad;
            txtDireccion.Text = direccion;
            txtTelefono.Text = telefono;
            txtCelular.Text = celular;
            txtCorreo.Text = correo.ToString();
            txtClave.Text = clave.ToString();
     
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                string update = "UPDATE usuarios set nombre = ?, clave = ?, tipo_usuario = ?, iniciales = ?, identidad = ?, direccion = ?, telefono = ?, celular = ?, correo = ? WHERE codigo = ?";
                OleDbCommand comando7 = new OleDbCommand(update, conexion);
                comando7.Parameters.AddWithValue("@nombre", txtNombre.Text);
                comando7.Parameters.AddWithValue("@clave", txtClave.Text);
                comando7.Parameters.AddWithValue("@tipo_usuario", txtTipo.Text);
                comando7.Parameters.AddWithValue("@iniciales", txtInciales.Text);
                comando7.Parameters.AddWithValue("@identidad", txtIdentidad.Text);
                comando7.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                comando7.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                comando7.Parameters.AddWithValue("@celular", txtCelular.Text);
                comando7.Parameters.AddWithValue("@correo", txtCorreo.Text);
                comando7.Parameters.AddWithValue("@codigo", txtId.Text);
                comando7.ExecuteNonQuery();
                if (comando7.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("Usuario actualizado con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information); this.Close();
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Ya existe un usuario con esa clave, elija otra clave", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            this.Close();
        }

        private void editarInventario_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void txtInciales_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(txtInciales, "Las Iniciales serán las que aparecerán en la facturacion");
            toolTip1.IsBalloon = true;

        }

    }
}
