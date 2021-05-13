using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
namespace Sistema
{
    public partial class rptventasVendedor : Form
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
        double credito = 0;
        double contado = 0;
        double sub = 0;
        double isv = 0;
        double tot = 0;


        double sumaP = 0;
        double creditoP = 0;
        double contadoP = 0;
        double subP = 0;
        double isvP = 0;
        double totP = 0;



        public rptventasVendedor()
        {
            InitializeComponent();

            try
            {
                conexion.Open();
                obtenerFacturas();
                groupBox2.ForeColor = Color.Black;
                //obtenerArticulos();
                //obtenerProveedor();
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
            groupBox4.Enabled = false;
            groupBox6.Enabled = false;
            groupBox8.Enabled = false;

        }

        public void obtenerFacturas()
        {

            OleDbCommand comando = new OleDbCommand("SELECT  iniciales FROM usuarios ", conexion);
            OleDbDataReader reader2 = comando.ExecuteReader();

            while (reader2.Read())
            {

                    facturas.Add(reader2.GetString(0));

            }
            textBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox5.AutoCompleteCustomSource = facturas;

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

                if (checkBox3.Checked == true)
                {
                    reporte = "Por Vendedor";
                }

                int contador = 0;
                decimal total = Math.Ceiling(Convert.ToDecimal(dataGridView1.Rows.Count) / 43);
                ///// Se establece el tipo de Fuente        
                Font Fuente = new Font("Verdana", 8);
                Font FuenteEncabezados = new Font("Verdana", 10);
                Font FuenteF = new Font("Verdana", 8);

                //titulo
                Font fuente_titulo = new Font("Verdana", 16, FontStyle.Bold);

                ///// Se establece el Color de Fuente
                Brush Brocha = Brushes.Black;
                ///// Se establece las cordenadas
                int Y = 35;

                //se genera el cuadro de la fecha y #cotizacion
                e.Graphics.DrawRectangle(Pens.Black, 650, 45, 174, 40);



                e.Graphics.DrawString(Program.GGG.empresa, fuente_titulo, Brocha, 190, 20);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());
                //muestra el logo
                Bitmap logo = Sistema.Properties.Resources.logo;
                e.Graphics.DrawImage(logo, 20, 20, 150, 100);

                e.Graphics.DrawString("Reporte De Ventas", fuente_titulo, Brocha, (e.MarginBounds.Width / 2), 115);

                e.Graphics.DrawString("Tel: " + Program.GGG.telefono_empresa, Fuente, Brocha, 190, 45);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("RTN: " + Program.GGG.RTN, Fuente, Brocha, 190, 65);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Direccion: " + Program.GGG.direccion, Fuente, Brocha, 190, 85);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Fecha", FuenteEncabezados, Brocha, 20, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Factura", FuenteEncabezados, Brocha, 130, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                //e.Graphics.DrawString("Cliente", FuenteEncabezados, Brocha, 200, 146);
                //Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Vendedor", FuenteEncabezados, Brocha, 550, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Tipo", FuenteEncabezados, Brocha, 650, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Valor Lps", FuenteEncabezados, Brocha, 750, 146);
                Y = Convert.ToInt32(Y + Fuente.GetHeight());

                e.Graphics.DrawString("Reporte: " + reporte, Fuente, Brocha, 650, 45);
                e.Graphics.DrawString("Fecha:" + DateTime.Now.ToString(), FuenteF, Brocha, 650, 65);

                //se genera el cuadro de los articulos


                //se genera el cuadro de los articulos
                e.Graphics.DrawLine(Pens.Black, 20, 145, 825, 145);
                e.Graphics.DrawLine(Pens.Black, 20, 165, 825, 165);




                while (item < dataGridView1.Rows.Count)
                {
                    if (dataGridView1.Rows[fila].Cells[1].Value != DBNull.Value || dataGridView1.Rows[fila].Cells[2].Value != DBNull.Value || dataGridView1.Rows[fila].Cells[3].Value != DBNull.Value || dataGridView1.Rows[fila].Cells[4].Value != DBNull.Value || dataGridView1.Rows[fila].Cells[5].Value != DBNull.Value)
                    {
                        e.Graphics.DrawString(Convert.ToDateTime(dataGridView1.Rows[fila].Cells["Fecha"].Value).ToShortDateString().Trim(), Fuente, Brocha, 23, Y);
                        e.Graphics.DrawString(dataGridView1.Rows[fila].Cells["Factura"].Value.ToString(), Fuente, Brocha, 130, Y);
                        //e.Graphics.DrawString(dataGridView1.Rows[fila].Cells["Cliente"].Value.ToString(), Fuente, Brocha, 200, Y);
                        e.Graphics.DrawString(dataGridView1.Rows[fila].Cells["Vendedor"].Value.ToString(), Fuente, Brocha, 550, Y);
                        e.Graphics.DrawString(dataGridView1.Rows[fila].Cells["Tipo"].Value.ToString(), Fuente, Brocha, 650, Y);
                        e.Graphics.DrawString(Convert.ToDecimal(dataGridView1.Rows[fila].Cells["Total"].Value.ToString()).ToString("N2"), Fuente, Brocha, 750, Y);

                    }
                    item++;
                    fila++;
                    if (Y > e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        e.Graphics.DrawLine(Pens.Black, 20, e.MarginBounds.Bottom + 30, 825, e.MarginBounds.Bottom + 30);
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
                    e.Graphics.DrawLine(Pens.Black, 825, 145, 825, Y + 25);
                    contador++;
                    e.Graphics.DrawString("Pagina: " + page + "/" + total, Fuente, Brocha, 750, e.MarginBounds.Bottom + 50);



                }
                //se genera el cuadro de los totales
                e.Graphics.DrawRectangle(Pens.Black, 20, Y, 805, 80);

                e.Graphics.DrawString("Contado ", Fuente, Brocha, 120, Y + 2);
                e.Graphics.DrawString("Credito ", Fuente, Brocha, 250, Y + 2);
                e.Graphics.DrawString("Subtotal ", Fuente, Brocha, 400, Y + 2);
                e.Graphics.DrawString("ISV ", Fuente, Brocha, 550, Y + 2);
                e.Graphics.DrawString("Total ", Fuente, Brocha, 700, Y + 2);

                e.Graphics.DrawString("Facturas: ", Fuente, Brocha, 40, Y + 20);
                e.Graphics.DrawString("Prestamos: ", Fuente, Brocha, 40, Y + 40);
                e.Graphics.DrawString("Totales: ", Fuente, Brocha, 40, Y + 60);


                e.Graphics.DrawString("" + textBox10.Text, Fuente, Brocha, 120, Y + 20);
                e.Graphics.DrawString("" + textBox15.Text, Fuente, Brocha, 120, Y + 40);
                e.Graphics.DrawString("" + textBox19.Text, Fuente, Brocha, 120, Y + 60);

                e.Graphics.DrawString("" + textBox13.Text, Fuente, Brocha, 250, Y + 20);
                e.Graphics.DrawString("" + textBox14.Text, Fuente, Brocha, 250, Y + 40);
                e.Graphics.DrawString("" + textBox16.Text, Fuente, Brocha, 250, Y + 60);

                e.Graphics.DrawString("" + textBox3.Text, Fuente, Brocha, 400, Y + 20);
                e.Graphics.DrawString("" + textBox11.Text, Fuente, Brocha, 400, Y + 40);
                e.Graphics.DrawString("" + textBox1.Text, Fuente, Brocha, 400, Y + 60);


                e.Graphics.DrawString("" + textBox8.Text, Fuente, Brocha, 550, Y + 20);
                e.Graphics.DrawString("" + textBox7.Text, Fuente, Brocha, 550, Y + 40);
                e.Graphics.DrawString("" + textBox4.Text, Fuente, Brocha, 550, Y + 60);

                e.Graphics.DrawString("" + textBox12.Text, Fuente, Brocha, 700, Y + 20);
                e.Graphics.DrawString("" + textBox6.Text, Fuente, Brocha, 700, Y + 40);
                e.Graphics.DrawString("" + textBox9.Text, Fuente, Brocha, 700, Y + 60);

                Y = Convert.ToInt32(Y + Fuente.GetHeight());


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
                    groupBox8.Enabled = true;
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
            try
            {
                suma = 0;
                 credito = 0;
                 contado = 0;
                 sub = 0;
                 isv = 0;
                tot = 0;

                OleDbDataAdapter data = new OleDbDataAdapter(consulta, conexion);
                data.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    if(dr["TipoVenta"].ToString().ToUpper() == "CONTADO")
                    {
                        contado += Convert.ToDouble(dr["Total"]);
                    }
                    else
                    {
                        credito += Convert.ToDouble(dr["Total"]);
                    }

                    if (dr["ISV"].ToString() != "0")
                    {
                        isv += Convert.ToDouble(dr["Total"].ToString()) * Convert.ToDouble(dr["ISV"].ToString());
                    }
                }
                sub = contado + credito;
                tot = isv + sub;
                textBox10.Text = "";
                textBox13.Text = "";
                textBox3.Text = "";
                textBox8.Text = "";
                textBox12.Text = "";

                textBox10.Text = contado.ToString("N2");
                textBox13.Text = credito.ToString("N2");
                textBox3.Text = sub.ToString("N2");
                textBox8.Text = isv.ToString("N2");
                textBox12.Text = tot.ToString("N2");

                //DataRow row = dt.NewRow();
                //row["Cliente"] = "";
                //dt.Rows.Add(row);

                if (!(dt.Columns.Contains("TIPO")))
                {
                    dt.Columns.Add("TIPO", typeof(System.String));
  
                }
                foreach (DataRow r in dt.Rows)
                {
                    r["TIPO"] = "FACTURA";
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void funcion2(string consulta)
        {
            try
            {
                suma = 0;
                creditoP = 0;
                contadoP = 0;
                subP = 0;
                isvP = 0;
                totP = 0;

                OleDbDataAdapter data = new OleDbDataAdapter(consulta, conexion);
                data.Fill(dt);
                
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["TIPO"].ToString() != "FACTURA")
                    {
                        dr["TIPO"] = "PRESTAMO";
                    }

                    if (dr["Tipo"].ToString().ToUpper() == "PRESTAMO")
                    {
                        sumaP += Convert.ToDouble(dr["Total"]);

                        if (dr["TipoVenta"].ToString().ToUpper() == "CONTADO")
                        {
                            contadoP += Convert.ToDouble(dr["Total"]);
                        }
                        else
                        {
                            creditoP += Convert.ToDouble(dr["Total"]);
                        }
                    }      

                    if (string.IsNullOrEmpty(dr["isv"].ToString())){
                        dr["ISV"] = 0;
                        dr["TotalISV"] = 0;

                    }


                }
                subP = contadoP + creditoP;
                totP = subP;
                textBox6.Text = "";
                textBox7.Text = "";
                textBox11.Text = "";
                textBox14.Text = "";
                textBox15.Text = "";

                textBox15.Text = contadoP.ToString("N2");
                textBox14.Text = creditoP.ToString("N2");
                textBox11.Text = subP.ToString("N2");
                textBox7.Text = "0.00";
                textBox6.Text = totP.ToString("N2");

                textBox1.Text = "";
                textBox4.Text = "";
                textBox9.Text = "";
                textBox16.Text = "";
                textBox19.Text = "";

                textBox19.Text = (contadoP + contado).ToString("N2");
                textBox16.Text = (credito + creditoP).ToString("N2");
                textBox1.Text = (sub + totP).ToString("n2");
                textBox4.Text = isv.ToString("N2");
                textBox9.Text = (tot + totP).ToString("N2");

                dataGridView1.DataSource = dt;
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns[7].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["ISV"].Visible = false;
                //dataGridView1.Columns["TIPO"].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void mostrar()
        {
            string c = @"SELECT fecha AS Fecha, numero_factura AS Factura, vendedor AS Vendedor, 
                          tipo AS TipoVenta,  sum(cantidad*precio) AS Total, isv as ISV, ((cantidad*precio)*isv) as TotalIsv 
                       FROM ventas";
            string order = " order by fecha asc";
            string group = " group by fecha, numero_factura, cliente, vendedor, tipo, isv, ((cantidad*precio)*isv)";
            string group2 = " group by fecha, numero_prestamo, cliente, vendedor, tipo";


            string c2 = c + " WHERE vendedor='" + textBox5.Text + "'" + group + order;
            string c7 = c + group + order;


            string ccc = c + " WHERE vendedor='" + textBox5.Text + "' AND fecha between #" + dateTimePicker1.Value.ToShortDateString().Trim() + "# AND #" + dateTimePicker2.Value.ToShortDateString().Trim() + "#" + group + order;
            string cc = c + " WHERE fecha between Format(#" + dateTimePicker1.Value.ToShortDateString().Trim() + "#, 'm/d/yyyy')" + " AND Format(#" + dateTimePicker2.Value.ToShortDateString().Trim() + "#, 'm/d/yyyy')" + group + order;



            string v = @"SELECT fecha AS Fecha, numero_prestamo AS Factura, vendedor AS Vendedor, tipo AS TipoVenta,  sum(cantidad*precio) AS Total FROM prestamo";
            string v2 = v + " WHERE vendedor='" + textBox5.Text + "'" + group2 + order;
            string vvv = v + " WHERE vendedor='" + textBox5.Text + "' AND fecha between Format(#" + dateTimePicker1.Value.ToShortDateString().Trim() + "#, 'm/d/yyyy')" + " AND Format(#" + dateTimePicker2.Value.ToShortDateString().Trim() + "#, 'm/d/yyyy')" + group2 + order;
            string vvvv = v + " WHERE fecha between Format(#" + dateTimePicker1.Value.ToShortDateString().Trim() + "#, 'm/d/yyyy')" + " AND Format(#" + dateTimePicker2.Value.ToShortDateString().Trim() + "#, 'm/d/yyyy')" + group2 + order;
            string vv = v + group2 + order;

            try
            {
                conexion.Open();

                if (checkBox3.Checked == true && checkBox4.Checked == false)
                {
                    if (textBox5.Text == "" || string.IsNullOrEmpty(textBox5.Text))
                    {
                        dt.Clear();
                        dataGridView1.DataSource = "";

                        funcion(c7);
                        funcion2(vv);
                    }
                    else
                    {
                        dt.Clear();
                        dataGridView1.DataSource = "";
                        funcion(c2);
                        funcion2(v2);
                    }
                }

                if (checkBox3.Checked == true && checkBox4.Checked == true)
                {
                    if (textBox5.Text == "" || string.IsNullOrEmpty(textBox5.Text))
                    {
                        dt.Clear();
                        dataGridView1.DataSource = "";
                        funcion(cc);
                        funcion2(vvvv);
                    }
                    else
                    {
                        dt.Clear();
                        dataGridView1.DataSource = "";
                        funcion(ccc);
                        funcion2(vvv);
                    }
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

        private void button3_Click(object sender, EventArgs e)
        {

            mostrar();
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

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
