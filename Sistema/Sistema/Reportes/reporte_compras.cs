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
using Microsoft.Reporting.WinForms;
using DGVPrinterHelper;
namespace Sistema
{
    public partial class reporte_compras: Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt = new DataTable("Inventario");
        AutoCompleteStringCollection facturas = new AutoCompleteStringCollection();
        AutoCompleteStringCollection codigos = new AutoCompleteStringCollection();
        AutoCompleteStringCollection proveedor = new AutoCompleteStringCollection();

        int fila = 0; int columna = 0;
        int c = 0;
        int item = 0;
        int page = 1;

        int totalPages = 1;
        //empresa
        string empresa = "";
        string telefono = "";
        string RTN = "";
        string direccion = "";

        double suma = 0;

        public reporte_compras()
        {
            InitializeComponent();

            try
            {
                conexion.Open();
                obtenerFacturas();
                obtenerArticulos();
                obtenerProveedor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conexion.Close();
            }
            

            groupBox2.Enabled = true;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;
            groupBox6.Enabled = false;
            groupBox8.Enabled = false;

        }

        public void obtenerFacturas()
        {

            OleDbCommand comando = new OleDbCommand("SELECT  numero_factura FROM compras ", conexion);
            OleDbDataReader reader2 = comando.ExecuteReader();

            while (reader2.Read())
            {

                    facturas.Add(reader2.GetString(0));

            }
            textBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox5.AutoCompleteCustomSource = facturas;

        }


        public void obtenerArticulos()
        {

            OleDbCommand comando = new OleDbCommand("SELECT codigo FROM compras", conexion);
            OleDbDataReader reader2 = comando.ExecuteReader();

            while (reader2.Read())
            {
                try
                {
                    codigos.Add(reader2.GetString(0));
                }catch
                {

                }
            }
            textBox4.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox4.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox4.AutoCompleteCustomSource = codigos;
        }

        public void obtenerProveedor()
        {

            OleDbCommand comando = new OleDbCommand("SELECT proveedor FROM compras", conexion);
            OleDbDataReader reader2 = comando.ExecuteReader();


            while (reader2.Read())
            {
                try
                {
                    proveedor.Add(reader2.GetString(0));
                }
                catch
                {

                }
            }
            textBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox3.AutoCompleteCustomSource = proveedor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.Rows.Count > 0)
                {
                    totalPages = 0;
                    item = 0;
                    fila = 0;
                    printDocument1.Print();
                    //DGVPrinter printer = new DGVPrinter();
                    //printer.Title = "Reporte de Compras";
                    //printer.SubTitle = Program.GGG.empresa;
                    //printer.SubTitleFormatFlags = StringFormatFlags.LineLimit |
                    //StringFormatFlags.NoClip;
                    //printer.PageNumbers = true;
                    //printer.PageNumberInHeader = false;
                    //printer.RowHeight = DGVPrinter.RowHeightSetting.DataHeight;
                    //printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
                    //printer.HeaderCellAlignment = StringAlignment.Near;
                    //printer.Footer = "Impreso por :" + Program.usuario;
                    //printer.FooterSpacing = 15;
                    //printer.PrintDataGridView(dataGridView1);

                    
                }
                else
                {
                    MessageBox.Show("No hay nada que imprimir", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {

            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {             
                string reporte = "";
                if (checkBox1.Checked == true)
                {
                    reporte = "Por Codigo";
                }
                if (checkBox3.Checked == true)
                {
                    reporte = "Por Factura";
                }
                if (checkBox2.Checked == true)
                {
                    reporte = "Por Proveedor";
                }
                int contador = 0;
                decimal total = Math.Ceiling(Convert.ToDecimal(dataGridView1.Rows.Count)/ 43);
                ///// Se establece el tipo de Fuente        
                Font Fuente = new Font("Verdana", 8);
                Font FuenteEncabezados = new Font("Verdana", 10);
                Font FuenteF = new Font("Verdana", 7);

                //titulo
                Font fuente_titulo = new Font("Verdana", 16, FontStyle.Bold);

                ///// Se establece el Color de Fuente
                Brush Brocha = Brushes.Black;
                ///// Se establece las cordenadas
                int Y = 35;

                //se genera el cuadro de la fecha y #cotizacion
                e.Graphics.DrawRectangle(Pens.Black, 650, 45, 155, 40);



                e.Graphics.DrawString(Program.GGG.empresa, fuente_titulo, Brocha, 190, 20);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                //muestra el logo
                Bitmap logo = Sistema.Properties.Resources.logo;
                e.Graphics.DrawImage(logo, 20, 20, 150, 100);

                e.Graphics.DrawString("Reporte de Compras", fuente_titulo, Brocha, (e.MarginBounds.Width /2), 115);

                e.Graphics.DrawString("Tel: " + Program.GGG.telefono_empresa, Fuente, Brocha, 190, 45);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("RTN: " + Program.GGG.RTN, Fuente, Brocha, 190, 65);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Direccion: " + Program.GGG.direccion, Fuente, Brocha, 190, 85);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Fecha" , FuenteEncabezados, Brocha, 20, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Codigo", FuenteEncabezados, Brocha, 130, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Descripcion", FuenteEncabezados, Brocha, 200, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Cantidad", FuenteEncabezados, Brocha, 550, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Precio", FuenteEncabezados, Brocha, 650, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Total", FuenteEncabezados, Brocha, 730, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Reporte:     " + reporte, Fuente, Brocha, 650, 45);
                e.Graphics.DrawString("Fecha:" + DateTime.Now.ToString(), FuenteF, Brocha, 650, 65);

                //se genera el cuadro de los articulos


                //se genera el cuadro de los articulos
                e.Graphics.DrawLine(Pens.Black, 20, 145, 805, 145);
                e.Graphics.DrawLine(Pens.Black, 20, 165, 805, 165);


                double total1;
                double precio1;

                while (item < dataGridView1.Rows.Count)
                {
                    if (dataGridView1.Rows[fila].Cells[1].Value != DBNull.Value || dataGridView1.Rows[fila].Cells[2].Value != DBNull.Value || dataGridView1.Rows[fila].Cells[3].Value != DBNull.Value || dataGridView1.Rows[fila].Cells[4].Value != DBNull.Value || dataGridView1.Rows[fila].Cells[5].Value != DBNull.Value)
                    {
                        precio1 = Convert.ToDouble(dataGridView1.Rows[fila].Cells[8].Value);
                        total1 = Convert.ToDouble(dataGridView1.Rows[fila].Cells[9].Value);
                        e.Graphics.DrawString(Convert.ToDateTime(dataGridView1.Rows[fila].Cells[0].Value).ToShortDateString(), Fuente, Brocha, 23, Y);
                        e.Graphics.DrawString(dataGridView1.Rows[fila].Cells[1].Value.ToString(), Fuente, Brocha, 130, Y);
                        e.Graphics.DrawString(dataGridView1.Rows[fila].Cells[2].Value.ToString(), Fuente, Brocha, 200, Y);
                        e.Graphics.DrawString(dataGridView1.Rows[fila].Cells[7].Value.ToString(), Fuente, Brocha, 550, Y);
                        e.Graphics.DrawString(precio1.ToString("N2"), Fuente, Brocha, 650, Y);
                        e.Graphics.DrawString(total1.ToString("N2"), Fuente, Brocha, 730, Y);

                    }
                    item++;
                    fila++;
                    if (Y > e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        e.Graphics.DrawLine(Pens.Black, 20, e.MarginBounds.Bottom + 30, 805, e.MarginBounds.Bottom + 30);
                        page++;
                        totalPages++;
                        Y = 30;
                        return;

                    }
                    else
                    {
                        e.HasMorePages = false;
                        Y = Y + 20;

                    }

                    e.Graphics.DrawLine(Pens.Black, 20, 145, 20, Y + 25);
                    e.Graphics.DrawLine(Pens.Black, 805, 145, 805, Y + 25);
                    contador++;
                    e.Graphics.DrawString("Pagina: " + page + "/" + total, Fuente, Brocha, 750, e.MarginBounds.Bottom + 50);



                }
                //se genera el cuadro de los totales
                e.Graphics.DrawRectangle(Pens.Black, 20, Y, 785, 25);

                e.Graphics.DrawString("Total: " + suma.ToString("N2"), Fuente, Brocha, 640, Y + 2);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
              
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked == true)
                {
                    //obtenerArticulos();
                    groupBox3.Enabled = true;
                    groupBox8.Enabled = false;
                    groupBox5.Enabled = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    textBox4.Focus();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox2.Checked == true)
                {
                    //obtenerProveedor();
                    groupBox3.Enabled = false;
                    groupBox8.Enabled = false;
                    groupBox5.Enabled = true;
                    checkBox1.Checked = false;
                    checkBox3.Checked = false;
                    textBox3.Focus();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox3.Checked == true)
                {
                    //obtenerFacturas();
                    groupBox3.Enabled = false;
                    groupBox5.Enabled = false;
                    groupBox8.Enabled = true;
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    textBox5.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void funcion(string consulta)
        {
            suma = 0;
            dt.Clear();
            dataGridView1.DataSource = "";
            OleDbDataAdapter data = new OleDbDataAdapter(consulta, conexion);
            data.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                suma += Convert.ToDouble(dr[9]);
            }

            DataRow row = dt.NewRow();
            row["Total"] = suma;
            dt.Rows.Add(row);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[8].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns[9].DefaultCellStyle.Format = "N2";

        }

        private void button3_Click(object sender, EventArgs e)
        {

            string c = "SELECT fecha AS Fecha, numero_factura AS Numero_Factura, codigo AS Codigo, descripcion AS Descripcion, grupo AS Grupo, marca AS Marca, proveedor AS Proveedor, cantidad AS Cantidad, costo1 as Costo, (cantidad * costo1) as Total FROM compras";
            string order = " order by fecha";
            string c1 = c + " WHERE codigo='" + textBox4.Text + "'" + order;
            string c2 = c + " WHERE numero_factura='" + textBox5.Text + "'" + order;
            string c3 = c + " WHERE proveedor='" + textBox3.Text + "'" + order;
            string c4 = c + " WHERE codigo='" + textBox4.Text + "' AND fecha between Format(#" + dateTimePicker1.Value.ToShortDateString() + "#, 'm/d/yyyy')" + " AND Format(#" + dateTimePicker2.Value.ToShortDateString() + "#, 'm/d/yyyy')" + order;
            string c5 = c + " WHERE numero_factura='" + textBox5.Text + "' AND fecha between Format(#" + dateTimePicker1.Value.ToShortDateString() + "#, 'm/d/yyyy')" + " AND Format(#" + dateTimePicker2.Value.ToShortDateString() + "#, 'm/d/yyyy')" + order;
            string c6 = c + " WHERE proveedor='" + textBox3.Text + "' AND fecha between Format(#" + dateTimePicker1.Value.ToShortDateString() + "#, 'm/d/yyyy')" + " AND Format(#" + dateTimePicker2.Value.ToShortDateString() + "#, 'm/d/yyyy')" + order;
            
            try
            {
                conexion.Open();

                if (checkBox1.Checked == true)
                {
                    funcion(c1);         
                }
                if (checkBox3.Checked == true)
                {
                    funcion(c2);
                }

                if (checkBox2.Checked == true)
                {
                    funcion(c3);
                }

                if (checkBox1.Checked == true && checkBox4.Checked == true)
                {
                    funcion(c4);
                }

                if (checkBox3.Checked == true && checkBox4.Checked == true)
                {
                    funcion(c5);
                }

                if (checkBox2.Checked == true && checkBox4.Checked == true)
                {
                    funcion(c6);
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

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox4.Checked == true)
                {
                    groupBox6.Enabled = true;
                }
                else
                {
                    groupBox6.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
            }
        }

    }
}
