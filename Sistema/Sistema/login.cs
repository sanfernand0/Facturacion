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
using System.Threading;

namespace Sistema
{
    public partial class login : Form
    {

        string contra;
        string id;
        string iniciales;
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);

        public login()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        public void conectar()
        {
            conexion.Open();

            OleDbCommand comando = new OleDbCommand("SELECT  *FROM usuarios", conexion);
            OleDbDataReader reader = comando.ExecuteReader();
            Usuario u = new Usuario(txtContrasenia.Text);
            if (string.IsNullOrEmpty(txtContrasenia.Text))
            {
                label1.Text = "Contraseña Vacia";
                conexion.Close();
            }
            else
            {
                while (reader.Read())
                {

                    if (reader.GetString(2) == txtContrasenia.Text)
                    {
                        contra = reader.GetString(2);
                        id = reader.GetString(3);
                        iniciales = reader.GetString(4);
                        Program.usuario = reader.GetString(4);

                    }


                }

                if (u.contrasenia == contra && id == "Administrador")
                {
                    txtContrasenia.Clear();
                    menu1 m1 = new menu1(iniciales);
                    this.Hide();
                    m1.ShowDialog();
                    this.Show();
                    txtContrasenia.Focus();
                    label1.Text = "";
                    login.ActiveForm.Close();

                }
                else

             if (u.contrasenia == contra && id == "Gerente")
                {
                    txtContrasenia.Clear();
                    menu2 m2 = new menu2(iniciales);
                    this.Hide();
                    m2.ShowDialog();
                    this.Show();
                    txtContrasenia.Focus();
                    label1.Text = "";
                    login.ActiveForm.Close();

                }
                else


            if (u.contrasenia == contra && id == "Vendedor")
                {
                    txtContrasenia.Clear();
                    menu3 m3 = new menu3(iniciales);
                    this.Hide();
                    m3.ShowDialog();
                    this.Show();
                    txtContrasenia.Focus();
                    label1.Text = "";
                    login.ActiveForm.Close();

                }
                else


            if (u.contrasenia != contra)
                {
                    label1.Text = "Contraseña invalida, intentelo de nuevo";
                    txtContrasenia.Clear();
                    txtContrasenia.Focus();
                }


                conexion.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                if (comboBox1.Text == "Geminis")
                {
                    //Program.GGG.conectar = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @"\\DESKTOP-51HB388" + @"\conexion\Base.accdb";
                    Program.GGG.conectar = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Base.accdb";
                    conexion = new OleDbConnection(Program.GGG.conectar);
                    Program.GGG.empresa = "AUTOREPUESTOS \"HONDUPARTES\" ";
                    Program.GGG.correo = "honduauto@yahoo.com";
                    Program.GGG.direccion = "Barrio Concepción, 12 calle entre 4ta y 5ta Ave. Comayaguela M.D.C";
                    Program.GGG.RTN = "06111982011810";
                    Program.GGG.telefono_empresa = "2220-0904 Cel:3303-4660";
                    conectar();
                }
                else
                {
                    Program.GGG.conectar = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=SanLuis.accdb";
                    conexion = new OleDbConnection(Program.GGG.conectar);
                    Program.GGG.empresa = "AUTO REPUESTOS SAN LUIS";
                    Program.GGG.correo = "gm061224@gmail.com";
                    Program.GGG.direccion = "12 Calle, 4a y 5a Ave. Comayaguela, M.D.C";
                    Program.GGG.RTN = "0613-1951-000520";
                    Program.GGG.telefono_empresa = "2237-6514";
                    conectar();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            login.ActiveForm.Close();
        }



        private void login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

 
    }

}
