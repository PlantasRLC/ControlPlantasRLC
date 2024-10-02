using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppControl
{
	public partial class Conexion : Form
	{
		public Conexion()
		{
			InitializeComponent();
		}

		private void Conexion_Load(object sender, EventArgs e)
		{
			try
			{
				//comboboxPuerto
				string[] ports = SerialPort.GetPortNames();
				comboBoxPuerto.DataSource = ports;

				//comboboxBaud
				string[] rates = { "9600", "38400", "57600", "115200" };
				comboBoxBaudRate.DataSource = rates;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void buttonConectar_Click(object sender, EventArgs e)
		{
			try
			{
				if (!serialPort1.IsOpen)
				{
					serialPort1.BaudRate = Convert.ToInt32(comboBoxBaudRate.Text);
					serialPort1.PortName = comboBoxPuerto.Text;
					serialPort1.Open();

					progressBarConexionSerial.Value = 100;
					buttonConectar.Text = "Desconectar";
					buttonRefrescar.Enabled = false;

				}
				else
				{
					progressBarConexionSerial.Value = 0;
					buttonConectar.Text = "Conectar";
					buttonRefrescar.Enabled = true;
					//StopCommunication();
					serialPort1.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void buttonRefrescar_Click(object sender, EventArgs e)
		{
			//Vuelve a verificar los puertos
			string[] ports = SerialPort.GetPortNames();
			comboBoxPuerto.DataSource = ports;

		}




	}
}
