using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sistema
{
    public partial class actualizador : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt = new DataTable("Inventario");
        DataTable dt2 = new DataTable("Inventario");

        public actualizador()
        {
            InitializeComponent();
            actualizar();
            actualizar2();
        }

        public void actualizar()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT * from cotizacion where precio = 0.00 and codigo<> '000' and grupo<> '000' and marca<> '000' ", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;

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


        public void actualizar2()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT * from inventario where precio1 <> 0.00 and codigo <> '000' and grupo <> '000' and marca <> '000'", conexion);
                data.Fill(dt2);
                dataGridView2.DataSource = dt2;

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

        public void update()
        {

            conexion.Open();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string codigo = row.Cells["codigo"].Value.ToString();
                string marca = row.Cells["grupo"].Value.ToString();
               string  grupo = row.Cells["marca"].Value.ToString();
                string id = row.Cells["id"].Value.ToString();


                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                   string codigo2 = dataGridView2.Rows[i].Cells["codigo"].Value.ToString();
                  string  marca2 = dataGridView2.Rows[i].Cells["grupo"].Value.ToString();
                  string  grupo2 = dataGridView2.Rows[i].Cells["marca"].Value.ToString();

                    if (codigo == codigo2 && grupo == grupo2 && marca == marca2)
                    {


                        //string update2 = "UPDATE ventas set precio = '" + dataGridView2.Rows[i].Cells["precio1"].Value.ToString() + "' WHERE Id=" + id + "";
                        //OleDbCommand comando2 = new OleDbCommand(update2, conexion);
                        //comando2.ExecuteNonQuery();

                        //string update2 = "UPDATE prestamo set precio = '" + dataGridView2.Rows[i].Cells["precio1"].Value.ToString() + "' WHERE Id=" + id + "";
                        //OleDbCommand comando2 = new OleDbCommand(update2, conexion);
                        //comando2.ExecuteNonQuery();

                        string update2 = "UPDATE cotizacion set precio = '" + dataGridView2.Rows[i].Cells["precio1"].Value.ToString() + "' WHERE Id=" + id + "";
                        OleDbCommand comando2 = new OleDbCommand(update2, conexion);
                        comando2.ExecuteNonQuery();
                    }

                }

            }
            conexion.Close();


            //OleDbCommand comando = new OleDbCommand("SELECT precio1 from inventario where codigo = '"+codigo.Trim()+"' and marca = '"+marca.Trim() + "' and grupo = '"+grupo.Trim() + "'", conexion);
            //    OleDbDataReader reader = comando.ExecuteReader();
            //if (reader != null)
            //{
            //    while (reader.Read())
            //    {

            //    }
            //}
            //reader.Close(); // <- too easy to forget
            //reader.Dispose(); // <- too easy to forget
            //conexion.Close();

            //using (OleDbConnection conexion2 = new OleDbConnection(Program.GGG.conectar))
            //{
            //    conexion2.Open();

            //    using (OleDbCommand comando = new OleDbCommand("SELECT precio1 from inventario where codigo = '" + codigo.Trim() + "' and marca = '" + marca.Trim() + "' and grupo = '" + grupo.Trim() + "' and precio1 <> 0.00", conexion2))
            //    {
            //        using (OleDbDataReader reader = comando.ExecuteReader())
            //        {
            //            if (reader != null)
            //            {
            //                while (reader.Read())
            //                {
            //                    MessageBox.Show("codi= " + codigo + "marca= " + marca + "grupo= " + grupo + "precio= " + reader.GetDouble(0));
            //                }
            //            }
            //        } // reader closed and disposed up here

            //    } // command disposed here

            //}
            // }




        }

        private void button1_Click(object sender, EventArgs e)
        {
            update();
        }
    }
}
