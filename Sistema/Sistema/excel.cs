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
    public partial class excel : Form
    {
        OleDbConnection cone;
        OleDbDataAdapter adap;
        DataTable dt;
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        int codigo5 = 1;

        string grupo, marca, codigo, descripcion1, aplicacion1, referencia1, medida1;
        int bodega11, tienda11;

        double costo11, costo21, precio11, precio21;
        double tasa;


        public excel()
        {
            InitializeComponent();
            progressBar1.Visible = false;
            progressBar2.Visible = false;

        }

        public void data(DataGridView dgv, String hoja)
        {
            String ruta = "";
            try
            {

                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Excel Files |*.xlsx";
                open.Title = "Seleccione Un Archivo Excel";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    if (open.FileName.Equals("") == false)
                    {
                        ruta = open.FileName;
                    }
                }
                progressBar1.Visible = true;
                progressBar1.Maximum = 1000000;

                //minimum indica el valor mínimo de la barra
                progressBar1.Minimum = 0;

                //value indica desde donde se va a comenzar a llenar la barra, la nuestra iniciara desde cero
                progressBar1.Value = 0;

                //Por ejemplo podemos hacer que la barra inicie desde la mitad
                //la siguiente instrucción indica que inicie cargando desde la mitad del tamaño de la barra
                //progressBar1.Value = progressBar1.Maximum / 2;

                //step indica el paso de la barra, entre más pequeño sea la barra tardará más en cargar
                progressBar1.Step = 1;

                //el ciclo for cargará la barra
                for (int i = progressBar1.Minimum; i < progressBar1.Maximum; i = i + progressBar1.Step)
                {
                    //esta instrucción avanza la posición actual de la barra
                    progressBar1.PerformStep();
                }

              
                cone = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ruta + "; Extended Properties = 'Excel 12.0 Xml; HDR = Yes'");
                adap = new OleDbDataAdapter("SELECT * FROM[" + hoja + "$]", cone);
                dt = new DataTable();
                adap.Fill(dt);
                dgv.DataSource = dt;
                if (dataGridView1.Rows.Count > 0)
                {
                    progressBar1.Visible = false;
                }

            }
            catch
            {
                MessageBox.Show("Error");
                this.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            data(dataGridView1, "inventario");

        }

        private void button3_Click(object sender, EventArgs e)
        {
           this.Close();
        } 

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                conexion.Open();
                progressBar2.Visible = true;
                progressBar2.Maximum = dataGridView1.Rows.Count;
                label1.Text = "El Programa estara bloqueado hasta que termine de actualizar el inventario";

                //minimum indica el valor mínimo de la barra
                progressBar2.Minimum = 0;

                //value indica desde donde se va a comenzar a llenar la barra, la nuestra iniciara desde cero
                progressBar2.Value = 100;

                //Por ejemplo podemos hacer que la barra inicie desde la mitad
                //la siguiente instrucción indica que inicie cargando desde la mitad del tamaño de la barra
                //progressBar1.Value = progressBar1.Maximum / 2;

                //step indica el paso de la barra, entre más pequeño sea la barra tardará más en cargar
                progressBar2.Step = 2;

                //el ciclo for cargará la barra
                for (int i = progressBar2.Minimum; i < progressBar2.Maximum; i = i + progressBar2.Step)
                {
                    //esta instrucción avanza la posición actual de la barra
                }


                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    progressBar1.Step = progressBar1.Step + 2;
                    progressBar2.PerformStep();

               

                    try
                    {
                        grupo = Convert.ToString(row.Cells[1].Value);
                    }
                    catch
                    {
                        grupo = "";

                    }

                    try
                    {
                        marca = Convert.ToString(row.Cells[2].Value);

                    }
                    catch
                    {
                        marca = "";
                    }

                    try
                    {
                    codigo = Convert.ToString(row.Cells[3].Value);

                    }
                    catch
                    {
                        codigo = "";
                    }

                    try
                    {
                    descripcion1 = Convert.ToString(row.Cells[4].Value);

                    }
                    catch
                    {
                        descripcion1 = "";
                    }

                    try
                    {
                    aplicacion1 = Convert.ToString(row.Cells[5].Value);

                    }
                    catch
                    {
                        aplicacion1 = "";
                    }

                    try
                    {
                        referencia1 = Convert.ToString(row.Cells[6].Value);

                    }
                    catch
                    {
                        referencia1 = "";
                    }

                    try
                    {
                        medida1 = Convert.ToString(row.Cells[7].Value);

                    }
                    catch
                    {
                        medida1 = "";
                    }
                    try
                    {
                        bodega11 = Convert.ToInt32(row.Cells[8].Value);

                    }
                    catch
                    {
                        bodega11 = 0;
                    }
                    try
                    {
                        tienda11 = Convert.ToInt32(row.Cells[9].Value);

                    }
                    catch
                    {
                        tienda11 = 0;
                    }

                    try
                    {
                        costo11 = Convert.ToDouble(row.Cells[10].Value);

                    }
                    catch
                    {
                        costo11 = 0.00;
                    }
                    try
                    {
                        costo21 = Convert.ToDouble(row.Cells[11].Value);

                    }
                    catch
                    {
                        costo21 = 0.00;
                    }
                    try
                    {
                        precio11 = Convert.ToDouble(row.Cells[12].Value);

                    }
                    catch
                    {
                        precio11 = 0.00;
                    }
                    try
                    {
                        precio21 = Convert.ToDouble(row.Cells[13].Value);

                    }
                    catch
                    {
                        precio21 = 0.00;
                    }
                    try
                    {
                        tasa = Convert.ToDouble(row.Cells[14].Value);

                    }
                    catch
                    {
                        tasa = 0.00;
                    }
                    string insert2 = "INSERT INTO inventario VALUES (@id, @grupo, @marca, @codigo, @descripcion, @aplicacion, @referencia, @medida, @bodega1, @tienda1, @costo1, @costo2, @precio1, @precio2, @tasa)";
                    OleDbCommand comando5 = new OleDbCommand(insert2, conexion);
                    comando5.Parameters.AddWithValue("@id", codigo5);
                    comando5.Parameters.AddWithValue("@grupo", grupo);
                    comando5.Parameters.AddWithValue("@marca", marca);
                    comando5.Parameters.AddWithValue("@codigo", codigo);
                    comando5.Parameters.AddWithValue("@descripcion", descripcion1);
                    comando5.Parameters.AddWithValue("@aplicacion", aplicacion1);
                    comando5.Parameters.AddWithValue("@referencia", referencia1);
                    comando5.Parameters.AddWithValue("@medida", medida1);
                    comando5.Parameters.AddWithValue("@bodega1", bodega11);
                    comando5.Parameters.AddWithValue("@tienda1", tienda11);
                    comando5.Parameters.AddWithValue("@costo1", costo11);
                    comando5.Parameters.AddWithValue("@costo2", costo21);
                    comando5.Parameters.AddWithValue("@precio1", precio11);
                    comando5.Parameters.AddWithValue("@precio2", precio21);
                    comando5.Parameters.AddWithValue("@tasa", tasa);
                    comando5.ExecuteNonQuery();
                    codigo5 = codigo5 + 1;
                    
                }
            }
            
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conexion.Close();
            }
            conexion.Close();

        }
    }
}
