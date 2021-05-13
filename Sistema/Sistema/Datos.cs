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
    public partial class Datos : Form
    {
        OleDbConnection conexion = new OleDbConnection(Program.GGG.conectar);
        DataTable dt = new DataTable("Datos");
        public Datos()
        {
            InitializeComponent();
            obtenerdatos();
        }

        private void Datos_Load(object sender, EventArgs e)
        {

        }


        public void obtenerdatos()
        {
            try
            {
                conexion.Open();
                OleDbDataAdapter data = new OleDbDataAdapter("SELECT * FROM datos", conexion);
                data.Fill(dt);
                dataGridView1.DataSource = dt;
                conexion.Close();
         
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int id;
                    string prefijo, factura, cai, rango, numero_recibo, caiNotaCredito, prefijoNotaCredito, numeroNota, 
                        rangoNota, prefijoNotaDebito, numeroNotaDebito, rangoNotaDebito, caiNotaDebito;
                    DateTime fecha, fechaLimiteEmisionNotaCredito, fechaLimiteEmisionNotaDebito;
                    int numero_traslado, numero_cotizacion, numero_prestamo, descuento;

                    id = Convert.ToInt32(row.Cells[0].Value);
                    prefijo = Convert.ToString(row.Cells[1].Value);
                    factura = Convert.ToString(row.Cells[2].Value);
                    cai = Convert.ToString(row.Cells[3].Value);
                    rango = Convert.ToString(row.Cells[4].Value);
                    fecha = Convert.ToDateTime(row.Cells[5].Value);
                    numero_traslado = Convert.ToInt32(row.Cells[6].Value);
                    numero_recibo = Convert.ToString(row.Cells[7].Value);
                    numero_cotizacion = Convert.ToInt32(row.Cells[8].Value);
                    numero_prestamo = Convert.ToInt32(row.Cells[9].Value);
                    descuento = Convert.ToInt32(row.Cells[10].Value);
                    caiNotaCredito = Convert.ToString(row.Cells["caiNotaCredito"].Value);
                    fechaLimiteEmisionNotaCredito = Convert.ToDateTime(row.Cells["fechaLimiteEmisionNotaCredito"].Value);
                    prefijoNotaCredito = Convert.ToString(row.Cells["prefijoNotaCredito"].Value);
                    numeroNota = Convert.ToString(row.Cells["numeroNota"].Value);
                    rangoNota = Convert.ToString(row.Cells["rangoNota"].Value);
                    prefijoNotaDebito = Convert.ToString(row.Cells["prefijoNotaDebito"].Value);
                    numeroNotaDebito = Convert.ToString(row.Cells["numeroNotaDebito"].Value);
                    rangoNotaDebito = Convert.ToString(row.Cells["rangoNotaDebito"].Value);
                    caiNotaDebito = Convert.ToString(row.Cells["caiNotaDebito"].Value);
                    fechaLimiteEmisionNotaDebito = Convert.ToDateTime(row.Cells["fechaLimiteEmisionNotaDebito"].Value);
                    //string update = "UPDATE DAtos set prefijo = '" + prefijo + "', factura = '" + factura + "' ,cai = '" + cai + "' ,rango = '" + rango + "' ,fecha = '" + fecha + "'WHERE id=" + id + "";

                    string update = "UPDATE Datos set prefijo = '" + prefijo + "',factura = '" + factura + "' ,cai = '" + cai + "',rango = '" + rango + "',fecha_limite = '" + fecha + "',numero_traslado = '" + numero_traslado 
                        + "' ,numero_recibo = '" + numero_recibo + "',numero_cotizacion = '" + numero_cotizacion + "',numero_prestamo = '" + numero_prestamo + "' ,descuento = '" + descuento + "' ,caiNotaCredito = '" + caiNotaCredito + "' ,fechaLimiteEmisionNotaCredito = '" 
                        + fechaLimiteEmisionNotaCredito + "' ,prefijoNotaCredito = '" + prefijoNotaCredito + "' ,numeroNota = '" + numeroNota + "' ,rangoNota = '" + rangoNota
                        + "' ,prefijoNotaDebito = '" + prefijoNotaDebito + "' ,numeroNotaDebito = '" + numeroNotaDebito + "' ,rangoNotaDebito = '" + rangoNotaDebito 
                        + "' ,caiNotaDebito = '" + caiNotaDebito + "' ,fechaLimiteEmisionNotaDebito = '" + fechaLimiteEmisionNotaDebito + "' WHERE id=" + id + "";
                    OleDbCommand comando55 = new OleDbCommand(update, conexion);
                    comando55.ExecuteNonQuery();
                    this.Close();
                }
                conexion.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
