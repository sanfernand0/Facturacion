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
    public partial class fmrAgregarUsurios: Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
       public fmrAgregarUsurios()
        {
            InitializeComponent();
            txtTipo.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int codigo = 1;
            try
            {
                if (txtTipo.Text != "")
                {
                    if (txtInciales.Text != "")
                    {
                        conexion.Open();
                        OleDbCommand comando22 = new OleDbCommand("SELECT MAX(codigo) FROM usuarios", conexion);
                        if (comando22.ExecuteScalar() != DBNull.Value)
                        {
                            codigo = Convert.ToInt32(comando22.ExecuteScalar());
                            codigo = codigo + 1;
                        }

                        string insert = "INSERT INTO usuarios VALUES (@codigo, @nombre, @clave, @tipo_usuario, @iniciales, @identidad, @direccion, @telefono, @celular, @correo)";
                        OleDbCommand comando21 = new OleDbCommand(insert, conexion);
                        comando21.Parameters.AddWithValue("@codigo", codigo);
                        comando21.Parameters.AddWithValue("@nombre", txtNombre.Text);
                        comando21.Parameters.AddWithValue("@clave", txtClave.Text);
                        comando21.Parameters.AddWithValue("@tipo_usuario", txtTipo.Text);
                        comando21.Parameters.AddWithValue("@iniciales", txtInciales.Text);
                        comando21.Parameters.AddWithValue("@identidad", txtIdentidad.Text);
                        comando21.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                        comando21.Parameters.AddWithValue("@telefono", txtTelefono.Text);
                        comando21.Parameters.AddWithValue("@celular", txtCelular.Text);
                        comando21.Parameters.AddWithValue("@correo", txtCorreo.Text);
                        comando21.ExecuteNonQuery();
                        MessageBox.Show("Registro Ingresado correctamente", "Bien", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show("Debe selecionar las iniciales del usuario", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
                else
                {
                    MessageBox.Show("Debe selecionar un tipo de usuario", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
       

            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Ya existe un usuario con esa clave, elija otra clave", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
