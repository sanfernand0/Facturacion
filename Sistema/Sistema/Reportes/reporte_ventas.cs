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
using DGVPrinterHelper;
namespace Sistema
{
    public partial class reporte_ventas : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt= new DataTable("Inventario");
        DataTable dt2 = new DataTable("Inventario");
        DataTable dt3 = new DataTable("Inventario");
        AutoCompleteStringCollection clientes = new AutoCompleteStringCollection();
        AutoCompleteStringCollection clientes2 = new AutoCompleteStringCollection();
        AutoCompleteStringCollection codigos = new AutoCompleteStringCollection();
        AutoCompleteStringCollection proveedor = new AutoCompleteStringCollection();
        AutoCompleteStringCollection vendedor = new AutoCompleteStringCollection();

        public reporte_ventas()
        {
            InitializeComponent();
            groupBox3.Visible = false;
            groupBox6.Visible = false;
            groupBox8.Visible = false;
            obtenerClientes();
            obtenerVendedor();
        }


        public void porVendedor()
        {
            DataTable user = new DataTable("user");
            user.Clear();
            conexion.Open();
            OleDbDataAdapter data3 = new OleDbDataAdapter("SELECT iniciales from usuarios", conexion);
            data3.Fill(user);
            dataGridView2.DataSource = user;
            conexion.Close();

            if (comboBox1.Text == "Por Vendedor" && comboBox2.Text == "" && textBox3.Text == "")
            {
                dt3.Clear();
                dataGridView3.DataSource = "";
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    conexion.Open();
                    OleDbDataAdapter data = new OleDbDataAdapter("SELECT vendedor, SUM(cantidad * precio)  as Factura , (SELECT SUM(cantidad * precio) From prestamo where vendedor = '" + row.Cells[0].Value + "') as Prestamos, SUM(cantidad * precio * '"+Program.ISV+"')  as ISV From ventas where vendedor = '" + row.Cells[0].Value + "' GROUP BY vendedor ", conexion);

                    //OleDbDataAdapter data = new OleDbDataAdapter("SELECT vendedor,  SUM(cantidad * precio) as Total_Vendido, SUM(cantidad*precio*'" + Program.ISV + "') as Total_ISV from ventas WHERE vendedor = '" + row.Cells[0].Value + "' GROUP BY vendedor", conexion);
                    data.Fill(dt3);
                    dataGridView3.DataSource = dt3;
                    conexion.Close();
                }
                dataGridView3.Columns.Add("SubTotal", "SubTotal");
                dataGridView3.Columns.Add("Total", "Total");
                dataGridView3.Columns["Subtotal"].DisplayIndex = 3;

                dataGridView3.Columns[1].DefaultCellStyle.Format = "N2";
                dataGridView3.Columns[2].DefaultCellStyle.Format = "N2";

                dataGridView3.Columns[3].DefaultCellStyle.Format = "N2";
                dataGridView3.Columns[4].DefaultCellStyle.Format = "N2";

                dataGridView3.Columns[5].DefaultCellStyle.Format = "N2";

                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    if (row.Cells[2].Value == DBNull.Value)
                    {
                        row.Cells[2].Value = 0.0;
                    }

                }
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    row.Cells[4].Value =Convert.ToDouble(row.Cells[1].Value) + Convert.ToDouble(row.Cells[2].Value);
                    row.Cells[5].Value = Convert.ToDouble(row.Cells[3].Value) + Convert.ToDouble(row.Cells[4].Value);
                }



            }

        }


        public void porArticuloClientefecha()
        {
            if (comboBox1.Text == "Por Articulo" && textBox1.Text == "" && checkBox1.Checked == true)
            {
                dt2.Clear();
                dataGridView4.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT fecha as FECHA, numero_factura &'   F' &' - '&codigo  as FACTURA_CODIGO, descripcion as DESCRIPCION, cantidad as Cant, (cantidad*precio) as TOTAL from ventas WHERE fecha between #" + dateTimePicker6.Value.ToShortDateString() + "# AND #" + dateTimePicker5.Value.ToShortDateString() + "#", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT fecha as FECHA, numero_prestamo &'   P' &' - '&codigo as FACTURA_CODIGO, descripcion as DESCRIPCION, cantidad as Cant, (cantidad*precio) as TOTAL from prestamo WHERE fecha between #" + dateTimePicker6.Value.ToShortDateString() + "# AND #" + dateTimePicker5.Value.ToShortDateString() + "#", conexion);
                data.Fill(dt2);
                data1.Fill(dt2);
                dataGridView4.DataSource = dt2;
                conexion.Close();
                dataGridView4.Columns[4].DefaultCellStyle.Format = "N2";
                dataGridView4.Sort(dataGridView4.Columns[1], ListSortDirection.Ascending);
            }


            if (comboBox1.Text == "Por Articulo" && textBox1.Text != "" && checkBox1.Checked == true)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT fecha as FECHA, numero_factura &'   F' &' - '&codigo  as FACTURA_CODIGO, descripcion as DESCRIPCION, cantidad as Cant, (cantidad*precio) as TOTAL from ventas WHERE fecha between #" + dateTimePicker6.Value.ToShortDateString() + "# AND #" + dateTimePicker5.Value.ToShortDateString() + "# AND cliente = '"+textBox1.Text+"'", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT fecha as FECHA, numero_prestamo &'   P' &' - '&codigo as FACTURA_CODIGO, descripcion as DESCRIPCION, cantidad as Cant, (cantidad*precio) as TOTAL from prestamo WHERE fecha between #" + dateTimePicker6.Value.ToShortDateString() + "# AND #" + dateTimePicker5.Value.ToShortDateString() + "# AND cliente = '" + textBox1.Text + "'", conexion);
                data.Fill(dt);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[4].DefaultCellStyle.Format = "N2";
                dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
            }
        }

        public void porArticuloCliente()
        {
            if (comboBox1.Text == "Por Articulo" && textBox1.Text != "")
            {
                dt2.Clear();
                dataGridView4.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT fecha as FECHA, numero_factura &'   F' &' - '&codigo  as FACTURA_CODIGO, descripcion as DESCRIPCION, cantidad as Cant, (cantidad*precio) as TOTAL from ventas WHERE cliente= '"+textBox1.Text+"'", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT fecha as FECHA, numero_prestamo &'   P' &' - '&codigo as FACTURA_CODIGO, descripcion as DESCRIPCION, cantidad as Cant, (cantidad*precio) as TOTAL from prestamo WHERE cliente= '" + textBox1.Text + "'", conexion);
                data.Fill(dt2);
                data1.Fill(dt2);
                dataGridView4.DataSource = dt2;
                conexion.Close();
                dataGridView4.Columns[4].DefaultCellStyle.Format = "N2";
                dataGridView4.Sort(dataGridView4.Columns[1], ListSortDirection.Ascending);
            }
        }

        public void porArticulo()
        {
            if (comboBox1.Text == "Por Articulo" && textBox1.Text == "")
            {
                dt2.Clear();
                dataGridView4.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT fecha as FECHA, numero_factura &'   F' &' - '&codigo  as FACTURA_CODIGO, descripcion as DESCRIPCION, cantidad as Cant, (cantidad*precio) as TOTAL from ventas order by descripcion asc", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT fecha as FECHA, numero_prestamo &'   P' &' - '&codigo as FACTURA_CODIGO, descripcion as DESCRIPCION, cantidad as Cant, (cantidad*precio) as TOTAL from prestamo order by descripcion asc", conexion);
                data.Fill(dt2);
                data1.Fill(dt2);
                dataGridView4.DataSource = dt2;
                conexion.Close();
                dataGridView4.Columns[4].DefaultCellStyle.Format = "N2";
                dataGridView4.Sort(dataGridView4.Columns[1], ListSortDirection.Ascending);
            }
        }
        public void general()
        {
            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "" && comboBox4.Text == "" && textBox5.Text == "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas GROUP BY NUMERO_FACTURA, fecha, tipo, cliente, vendedor", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo GROUP BY NUMERO_PRESTAMO, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
                dataGridView1.Sort(dataGridView1.Columns[2], ListSortDirection.Ascending);
                dataGridView1.Update();
            }
        }

        public void facturasclientecredito()
        {
            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Credito" && comboBox4.Text == "Facturas" && textBox5.Text != "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas where cliente = '" + textBox5.Text + "' AND tipo = '" + comboBox3.Text + "' GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Credito" && comboBox4.Text == "Prestamos" && textBox5.Text != "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo where cliente= '" + textBox5.Text + "' AND tipo = '" + comboBox3.Text + "' GROUP BY numero_prestamo, fecha, tipo, cliente, vendedor", conexion);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Credito" && comboBox4.Text == "Integrado" && textBox5.Text != "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas where cliente = '" + textBox5.Text + "' AND tipo = '" + comboBox3.Text + "' GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo where cliente= '" + textBox5.Text + "' AND tipo = '" + comboBox3.Text + "' GROUP BY numero_prestamo, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Credito" && comboBox4.Text == "Facturas" && textBox5.Text == "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas where tipo = '" + comboBox3.Text + "' GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Credito" && comboBox4.Text == "Prestamos" && textBox5.Text == "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo where tipo = '" + comboBox3.Text + "' GROUP BY numero_prestamo, fecha, tipo, cliente, vendedor", conexion);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Credito" && comboBox4.Text == "Integrado" && textBox5.Text == "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas where tipo = '" + comboBox3.Text + "' GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo where tipo = '" + comboBox3.Text + "' GROUP BY numero_prestamo, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }
        }

        public void facturasClientesContado()
        {
            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Contado" && comboBox4.Text == "Facturas" && textBox5.Text != "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas where cliente = '" + textBox5.Text + "' AND tipo = '"+comboBox3.Text+"' GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Contado" && comboBox4.Text == "Prestamos" && textBox5.Text != "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo where cliente= '" + textBox5.Text + "' AND tipo = '" + comboBox3.Text + "' GROUP BY numero_prestamo, fecha, tipo, cliente, vendedor", conexion);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Contado" && comboBox4.Text == "Integrado" && textBox5.Text != "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas where cliente = '" + textBox5.Text + "' AND tipo = '" + comboBox3.Text + "' GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo where cliente= '" + textBox5.Text + "' AND tipo = '" + comboBox3.Text + "' GROUP BY numero_prestamo, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Contado" && comboBox4.Text == "Facturas" && textBox5.Text == "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas where tipo = '" + comboBox3.Text + "' GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Contado" && comboBox4.Text == "Prestamos" && textBox5.Text == "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo where tipo = '" + comboBox3.Text + "' GROUP BY numero_prestamo, fecha, tipo, cliente, vendedor", conexion);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "Contado" && comboBox4.Text == "Integrado" && textBox5.Text == "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas where tipo = '" + comboBox3.Text + "' GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo where tipo = '" + comboBox3.Text + "' GROUP BY numero_prestamo, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }
        }

        public void facturasClientes()
        {
            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "" && comboBox4.Text == "Facturas" && textBox5.Text != "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas where cliente = '" + textBox5.Text + "' GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "" && comboBox4.Text == "Prestamos" && textBox5.Text != "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo where cliente= '" + textBox5.Text + "' GROUP BY numero_prestamo, fecha, tipo, cliente, vendedor", conexion);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "" && comboBox4.Text == "Integrado" && textBox5.Text != "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas where cliente = '" + textBox5.Text + "' GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo where cliente= '" + textBox5.Text + "' GROUP BY numero_prestamo, fecha, tipo, cliente, vendedor", conexion);
                data.Fill(dt);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }
        }

        public void facturas()
        {
            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "" && comboBox4.Text == "Facturas" && textBox5.Text == "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura As NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas GROUP BY NUMERO_FACTURA, FECHA, TIPO, CLIENTE, VENDEDOR", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "" && comboBox4.Text == "Prestamos" && textBox5.Text == "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo GROUP BY numero_prestamo, FECHA, TIPO, CLIENTE, VENDEDOR", conexion);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "" && comboBox4.Text == "Integrado" && textBox5.Text == "" && checkBox1.Checked == false)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas GROUP BY NUMERO_FACTURA, FECHA, TIPO, CLIENTE, VENDEDOR", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo GROUP BY numero_prestamo, FECHA, TIPO, CLIENTE, VENDEDOR", conexion);
                data.Fill(dt);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }


            try
            {
                if (comboBox1.Text == "Por Factura" && comboBox3.Text == "" && comboBox4.Text == "Facturas" && textBox5.Text == "" && checkBox1.Checked == true)
                {
                    dt.Clear();
                    dataGridView1.DataSource = "";
                    conexion.Open();
                    OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas WHERE fecha between #" + dateTimePicker6.Value.ToShortDateString() + "# AND #" + dateTimePicker5.Value.ToShortDateString() + "# GROUP BY numero_factura, fecha, tipo, cliente, vendedor", conexion);
                    data.Fill(dt);
                    dataGridView1.DataSource = dt;
                    conexion.Close();
                    dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "" && comboBox4.Text == "Prestamos" && textBox5.Text == "" && checkBox1.Checked == true)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo WHERE fecha between #" + dateTimePicker6.Value.ToShortDateString() + "# AND #" + dateTimePicker5.Value.ToShortDateString() + "# GROUP BY numero_prestamo, FECHA, TIPO, CLIENTE, VENDEDOR", conexion);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }

            if (comboBox1.Text == "Por Factura" && comboBox3.Text == "" && comboBox4.Text == "Integrado" && textBox5.Text == "" && checkBox1.Checked == true)
            {
                dt.Clear();
                dataGridView1.DataSource = "";
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT numero_factura as NUMERO_FACTURA, fecha AS FECHA, tipo &'   F' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from ventas WHERE fecha between #" + dateTimePicker6.Value.ToShortDateString() + "# AND #" + dateTimePicker5.Value.ToShortDateString() + "# GROUP BY NUMERO_FACTURA, FECHA, TIPO, CLIENTE, VENDEDOR", conexion);
                OleDbDataAdapter data1 = new OleDbDataAdapter("SELECT numero_prestamo As NUMERO_FACTURA, fecha as FECHA, tipo &'   P' as TIPO, cliente as CLIENTE, vendedor as VENDEDOR, SUM(cantidad*precio) as Total from prestamo  WHERE fecha between #" + dateTimePicker6.Value.ToShortDateString() + "# AND #" + dateTimePicker5.Value.ToShortDateString() + "#GROUP BY numero_prestamo, FECHA, TIPO, CLIENTE, VENDEDOR", conexion);
                data.Fill(dt);
                data1.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N2";
            }
        }
        public void obtenerClientes()
        {
            try
            {



                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT  cliente FROM ventas", conexion);
                OleDbDataReader reader2 = comando.ExecuteReader();

                while (reader2.Read())
                {
                    clientes2.Add(reader2.GetString(0));
                }
                textBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox5.AutoCompleteCustomSource = clientes2;


                textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox1.AutoCompleteCustomSource = clientes2;
                conexion.Close();



                conexion.Open();
                OleDbCommand comando1 = new OleDbCommand("SELECT  cliente FROM prestamo", conexion);
                OleDbDataReader reader21 = comando1.ExecuteReader();

                while (reader21.Read())
                {
                    clientes.Add(reader21.GetString(0));
                }
                textBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox5.AutoCompleteCustomSource = clientes;


                textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox1.AutoCompleteCustomSource = clientes;
                conexion.Close();
            }
            catch
            {

            }
            finally
            {
                conexion.Close();
            }


        }

        public void obtenerVendedor()
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand("SELECT iniciales FROM usuarios", conexion);
                OleDbDataReader reader2 = comando.ExecuteReader();

                while (reader2.Read())
                {
                    vendedor.Add(reader2.GetString(0));
                }
                textBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox3.AutoCompleteCustomSource = vendedor;
                conexion.Close();
            }
            catch
            {
            }
            finally
            {
                conexion.Close();
            }
        }

        private void inventarioBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
        }

   
        private void inventario_Load(object sender, EventArgs e)
        {
        }



        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            dt.Clear();
            dataGridView1.DataSource = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 || dataGridView3.Rows.Count > 0 || dataGridView4.Rows.Count > 0)
            {


                try
                {
                    if (comboBox1.Text == "Por Factura")
                    {
                        DGVPrinter printer = new DGVPrinter();
                        printer.Title = "Reporte de Ventas";
                        printer.SubTitle = Program.GGG.empresa + "                   " + DateTime.Now.ToString();
                        printer.SubTitleFormatFlags = StringFormatFlags.LineLimit |
                        StringFormatFlags.NoClip;
                        printer.PageNumbers = true;
                        printer.PageNumberInHeader = false;
                        printer.PorportionalColumns = true;
                        printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
                        printer.TableAlignment = DGVPrinter.Alignment.Center;
                        printer.CellAlignment = StringAlignment.Center;
                        printer.HeaderCellAlignment = StringAlignment.Center;
                        printer.PrintMargins.Left = 1;
                        printer.PrintMargins.Right = 1;
                        printer.Footer = Program.GGG.empresa;
                        printer.FooterSpacing = 15;
                        printer.PrintDataGridView(dataGridView1);
                    }
                    if (comboBox1.Text == "Por Articulo")
                    {
                        DGVPrinter printer = new DGVPrinter();
                        printer.Title = "Reporte de Ventas";
                        printer.SubTitle = Program.GGG.empresa + "                   " + DateTime.Now.ToString();
                        printer.SubTitleFormatFlags = StringFormatFlags.LineLimit |
                        StringFormatFlags.NoClip;
                        printer.PageNumbers = true;
                        printer.PageNumberInHeader = false;
                        printer.PorportionalColumns = true;
                        printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
                        printer.TableAlignment = DGVPrinter.Alignment.Center;
                        printer.CellAlignment = StringAlignment.Center;
                        printer.HeaderCellAlignment = StringAlignment.Center;
                        printer.PrintMargins.Left = 1;
                        printer.PrintMargins.Right = 1;
                        printer.Footer = Program.GGG.empresa;
                        printer.FooterSpacing = 15;
                        printer.PrintDataGridView(dataGridView4);
                    }

                    if (comboBox1.Text == "Por Vendedor")
                    {
                        DGVPrinter printer = new DGVPrinter();
                        printer.Title = "Reporte de Ventas";
                        printer.SubTitle = Program.GGG.empresa + "                   " + DateTime.Now.ToString();
                        printer.SubTitleFormatFlags = StringFormatFlags.LineLimit |
                        StringFormatFlags.NoClip;
                        printer.PageNumbers = true;
                        printer.PageNumberInHeader = false;
                        printer.PorportionalColumns = true;
                        printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
                        printer.TableAlignment = DGVPrinter.Alignment.Center;
                        printer.CellAlignment = StringAlignment.Center;
                        printer.HeaderCellAlignment = StringAlignment.Center;
                        printer.PrintMargins.Left = 1;
                        printer.PrintMargins.Right = 1;
                        printer.Footer = Program.GGG.empresa;
                        printer.FooterSpacing = 15;
                        printer.PrintDataGridView(dataGridView3);
                    }

                }
                catch
                {

                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
              
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            double facturasCredito = 0.00;
            double facturasContado = 0.00;
            double prestamosContado = 0.00;
            double PrestamosCredito = 0.00;
            double total = 0.0;

            try
            {
                if(comboBox1.Text == "Por Factura")
                {
                    general();
                    facturas();
                    facturasClientes();
                    facturasClientesContado();
                    facturasclientecredito();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if(row.Cells[2].Value.ToString() == "Contado   F")
                        {
                            facturasContado +=Convert.ToDouble(row.Cells[5].Value);
                        }

                        if (row.Cells[2].Value.ToString() == "Contado   P")
                        {
                            prestamosContado += Convert.ToDouble(row.Cells[5].Value);
                        }

                        if (row.Cells[2].Value.ToString() == "Credito   F")
                        {
                            facturasCredito += Convert.ToDouble(row.Cells[5].Value);
                        }

                        if (row.Cells[2].Value.ToString() == "Credito   P")
                        {
                            PrestamosCredito += Convert.ToDouble(row.Cells[5].Value);
                        }

                        total += Convert.ToDouble(row.Cells[5].Value);
                    }
                    double isvFacturas = Program.ISV * (facturasContado);

                    textBox6.Text = facturasContado.ToString("N2");
                    textBox4.Text = facturasCredito.ToString("N2");
                    textBox8.Text = (isvFacturas).ToString("N2");
                    textBox7.Text = (isvFacturas + facturasContado + prestamosContado).ToString("N2");

                    textBox9.Text = (facturasCredito * Program.ISV).ToString("N2");
                    textBox10.Text = prestamosContado.ToString("N2");
                    textBox12.Text = PrestamosCredito.ToString("N2");


                    textBox11.Text = (PrestamosCredito + facturasCredito + ((facturasCredito) * Program.ISV)).ToString("N2");
                    textBox15.Text = (facturasCredito + facturasContado).ToString("N2");
                    textBox19.Text = (prestamosContado + PrestamosCredito).ToString("N2");
                    textBox13.Text = (prestamosContado + facturasContado).ToString("N2");
                    textBox14.Text = (PrestamosCredito + facturasCredito).ToString("N2");
                    textBox16.Text = (PrestamosCredito + facturasCredito + prestamosContado + facturasContado).ToString("N2");
                    textBox17.Text = (isvFacturas + (facturasCredito * Program.ISV)).ToString("N2");
                    decimal t = Convert.ToDecimal(textBox7.Text);
                    decimal r = Convert.ToDecimal(textBox11.Text);
                    textBox18.Text = (t + r).ToString("N2");


                }

                if (comboBox1.Text == "Por Articulo")
                {
                    porArticulo();
                    porArticuloCliente();
                    porArticuloClientefecha();
                }
         

                if (comboBox1.Text == "Por Vendedor")
                {
                    porVendedor();
                }




            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
           
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text == "Por Factura")
            {
                groupBox3.Visible = true;
                dataGridView1.Visible = true;
                dataGridView3.Visible = false;
                dataGridView4.Visible = false;
                limpiar();
                
            }
            else
            {
                groupBox3.Visible = false;
            }


            if (comboBox1.Text == "Por Articulo")
            {
                groupBox6.Visible = true;
                obtenerClientes();
                dataGridView1.Visible = false;
                dataGridView3.Visible = false;
                dataGridView4.Visible = true;
                limpiar();
            }
            else
            {
                groupBox6.Visible = false;
            }


            if (comboBox1.Text == "Por Vendedor")
            {
                groupBox8.Visible = true;
                dataGridView1.Visible = false;
                dataGridView4.Visible = false;
                dataGridView3.Visible = true;
                limpiar();
            }
            else
            {
                groupBox8.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        public void limpiar()
        {
            textBox4.Clear();
            textBox6.Clear();
            textBox15.Clear();
            textBox10.Clear();
            textBox12.Clear();
            textBox19.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox16.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox19.Clear();
            textBox7.Clear();
            textBox11.Clear();
            textBox18.Clear();
            textBox17.Clear();
        }
    }
}
