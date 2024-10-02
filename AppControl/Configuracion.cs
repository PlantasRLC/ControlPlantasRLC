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
	public partial class Configuracion : Form
	{
		// Variable para mantener la referencia a conexion
		private Conexion _conexionForm;

		public Configuracion(Conexion conexion)
		{
			InitializeComponent();
			_conexionForm = conexion;

			Circuitos();
		}
		
		public float referencia =0;
		//public void button1_Click(object sender, EventArgs e)
		//{
		//	try
		//	{
		//		if (_conexionForm.serialPort1.IsOpen)
		//		{

		//			//Ley de Control
		//			float k1 = float.Parse(textBoxk1.Text);
		//			float k2 = float.Parse(textBoxk2.Text);
		//			float k3 = float.Parse(textBoxk3.Text);
		//			float k4 = float.Parse(textBoxk4.Text);
		//			float P = float.Parse(textBoxp.Text);
		//			float I = float.Parse(textBoxI.Text);
		//			referencia = float.Parse(textBoxref.Text);
		//			float dt = float.Parse(textBoxdt.Text);

		//			//Parámetros Circuitales
		//			float R1 = float.Parse(textBoxR1.Text);
		//			float R2 = float.Parse(textBoxR2.Text);
		//			float R3 = float.Parse(textBoxR3.Text);
		//			float R4 = float.Parse(textBoxR4.Text);
		//			float L1 = float.Parse(textBoxL1.Text);
		//			float L2 = float.Parse(textBoxL2.Text);
		//			float C1 = float.Parse(textBoxC1.Text);
		//			float C2 = float.Parse(textBoxC2.Text);

		//			//Envío de la información al Arduino
		//			_conexionForm.serialPort1.Write($"%P{k1}:{k2}:{k3}:{k4}:{P}:{I}:{referencia}:{dt}:{R1}:{R2}:{R3}:{R4}:{L1}:{L2}:{C1}:{C2}:{labelCircuito.Text}:{comboBoxEstadoControlado.Text}\n");  

		//			MessageBox.Show($"Se han enviado los datos hacia la tarjeta satisfactoriamente");
		//		}
		//		else
		//		{
		//			MessageBox.Show("El puerto está cerrado. Abre el puerto primero.");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show(ex.Message);
		//	}
		//}
		public void button1_Click(object sender, EventArgs e)
		{
			try
			{
				if (_conexionForm.serialPort1.IsOpen)
				{
					// Ley de Control
					float k1 = float.Parse(textBoxk1.Text);
					float k2 = float.Parse(textBoxk2.Text);
					float k3 = float.Parse(textBoxk3.Text);
					float k4 = float.Parse(textBoxk4.Text);
					float P = float.Parse(textBoxp.Text);
					float I = float.Parse(textBoxI.Text);
					referencia = float.Parse(textBoxref.Text);
					float dt = float.Parse(textBoxdt.Text);
					//float k1 = float.Parse(textBoxk1.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float k2 = float.Parse(textBoxk2.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float k3 = float.Parse(textBoxk3.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float k4 = float.Parse(textBoxk4.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float P = float.Parse(textBoxp.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float I = float.Parse(textBoxI.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float referencia = float.Parse(textBoxref.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float dt = float.Parse(textBoxdt.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);

					// Parámetros Circuitales
					float R1 = float.Parse(textBoxR1.Text);
					float R2 = float.Parse(textBoxR2.Text);
					float R3 = float.Parse(textBoxR3.Text);
					float R4 = float.Parse(textBoxR4.Text);
					float L1 = float.Parse(textBoxL1.Text);
					float L2 = float.Parse(textBoxL2.Text);
					float C1 = float.Parse(textBoxC1.Text);
					float C2 = float.Parse(textBoxC2.Text);
					//float R1 = float.Parse(textBoxR1.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float R2 = float.Parse(textBoxR2.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float R3 = float.Parse(textBoxR3.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float R4 = float.Parse(textBoxR4.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float L1 = float.Parse(textBoxL1.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float L2 = float.Parse(textBoxL2.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float C1 = float.Parse(textBoxC1.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
					//float C2 = float.Parse(textBoxC2.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);


					// Formatear los floats a 6 decimales
					string formattedData = string.Format(
						"%P{0:F6}:{1:F6}:{2:F6}:{3:F6}:{4:F6}:{5:F6}:{6:F6}:{7:F6}:{8:F6}:{9:F6}:{10:F6}:{11:F6}:{12:F6}:{13:F6}:{14:F6}:{15:F6}:{16}:{17}\n",
						k1, k2, k3, k4, P, I, referencia, dt, R1, R2, R3, R4, L1, L2, C1, C2, labelCircuito.Text, comboBoxEstadoControlado.Text
					);

					// Envío de la información al Arduino
					_conexionForm.serialPort1.Write(formattedData);

					//MessageBox.Show(formattedData);  // Esto muestra la cadena que se enviará

					MessageBox.Show("Se han enviado los datos hacia la tarjeta satisfactoriamente");
				}
				else
				{
					MessageBox.Show("El puerto está cerrado. Abre el puerto primero.");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void Circuitos()
		{
			// Obtener el valor seleccionado del ComboBox
			string seleccion = comboBoxSeleccionCircuito.SelectedItem.ToString();

			// Usar switch para ejecutar el código según la opción seleccionada
			switch (seleccion)
			{
				case "Seleccione":

					// Condiciones default
					textBoxk1.Enabled  = false;
					textBoxk2.Enabled  = false;
					textBoxk3.Enabled  = false;
					textBoxk4.Enabled  = false;
					textBoxp.Enabled   = false;
					textBoxI.Enabled   = false;
					textBoxref.Enabled = false;
					textBoxdt.Enabled  = false;

					textBoxR1.Enabled = false;
					textBoxR2.Enabled = false;
					textBoxR3.Enabled = false;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = false;
					textBoxL2.Enabled = false;
					textBoxC1.Enabled = false;
					textBoxC2.Enabled = false;

					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { });
					break;

				case "CIRCUITO I-V":
					// Condiciones para CIRCUITO I-V
					textBoxk1.Enabled  = true;
					textBoxk2.Enabled  = true;
					textBoxk3.Enabled  = true;
					textBoxk4.Enabled  = true;
					textBoxp.Enabled   = true;
					textBoxI.Enabled   = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled  = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = true;
					textBoxR4.Enabled = true;
					textBoxL1.Enabled = true;
					textBoxL2.Enabled = true;
					textBoxC1.Enabled = false;
					textBoxC2.Enabled = false;

					labelCircuito.Text = "I-V";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] {"iL1","iL2" });
					break;

				case "CIRCUITO I-VI":
					// Condiciones para CIRCUITO I-VI
					textBoxk1.Enabled  = true;
					textBoxk2.Enabled  = true;
					textBoxk3.Enabled  = true;
					textBoxk4.Enabled  = true;
					textBoxp.Enabled   = true;
					textBoxI.Enabled   = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled  = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = true;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = true;
					textBoxL2.Enabled = false;
					textBoxC1.Enabled = true;
					textBoxC2.Enabled = false;

					labelCircuito.Text = "I-VI";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { "iL1", "Vc1" });
					break;

				case "CIRCUITO I-VII":
					// Condiciones para CIRCUITO I-VII
					textBoxk1.Enabled = true;
					textBoxk2.Enabled = true;
					textBoxk3.Enabled = true;
					textBoxk4.Enabled = true;
					textBoxp.Enabled = true;
					textBoxI.Enabled = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = true;
					textBoxR4.Enabled = true;
					textBoxL1.Enabled = true;
					textBoxL2.Enabled = true;
					textBoxC1.Enabled = true;
					textBoxC2.Enabled = false;

					labelCircuito.Text = "I-VII";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { "iL1", "iL2","Vc1"});
					break;

				case "CIRCUITO II-V":
					// Condiciones para CIRCUITO II-V
					textBoxk1.Enabled = true;
					textBoxk2.Enabled = true;
					textBoxk3.Enabled = true;
					textBoxk4.Enabled = true;
					textBoxp.Enabled = true;
					textBoxI.Enabled = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = true;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = true;
					textBoxL2.Enabled = false;
					textBoxC1.Enabled = true;
					textBoxC2.Enabled = false;

					labelCircuito.Text = "II-V";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { "iL1", "Vc1" });
					break;

				case "CIRCUITO II-VI":
					// Condiciones para CIRCUITO II-VI
					textBoxk1.Enabled = true;
					textBoxk2.Enabled = true;
					textBoxk3.Enabled = true;
					textBoxk4.Enabled = true;
					textBoxp.Enabled = true;
					textBoxI.Enabled = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = false;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = false;
					textBoxL2.Enabled = false;
					textBoxC1.Enabled = true;
					textBoxC2.Enabled = true;

					labelCircuito.Text = "II-VI";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { "Vc1", "Vc2" });
					break;

				case "CIRCUITO II-VII":
					// Condiciones para CIRCUITO II-VII
					textBoxk1.Enabled = true;
					textBoxk2.Enabled = true;
					textBoxk3.Enabled = true;
					textBoxk4.Enabled = true;
					textBoxp.Enabled = true;
					textBoxI.Enabled = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = true;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = true;
					textBoxL2.Enabled = false;
					textBoxC1.Enabled = true;
					textBoxC2.Enabled = true;

					labelCircuito.Text = "II-VII";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { "iL1", "Vc1","Vc2" });
					break;

				case "CIRCUITO III-V":
					// Condiciones para CIRCUITO III-V
					textBoxk1.Enabled = true;
					textBoxk2.Enabled = true;
					textBoxk3.Enabled = true;
					textBoxk4.Enabled = true;
					textBoxp.Enabled = true;
					textBoxI.Enabled = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = true;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = true;
					textBoxL2.Enabled = false;
					textBoxC1.Enabled = true;
					textBoxC2.Enabled = false;

					labelCircuito.Text = "III-V";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { "iL1", "Vc1" });
					break;

				case "CIRCUITO III-VI":
					// Condiciones para CIRCUITO III-VI
					textBoxk1.Enabled = false;
					textBoxk2.Enabled = false;
					textBoxk3.Enabled = false;
					textBoxk4.Enabled = false;
					textBoxp.Enabled = false;
					textBoxI.Enabled = false;
					textBoxref.Enabled = false;
					textBoxdt.Enabled = false;

					textBoxR1.Enabled = false;
					textBoxR2.Enabled = false;
					textBoxR3.Enabled = false;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = false;
					textBoxL2.Enabled = false;
					textBoxC1.Enabled = false;
					textBoxC2.Enabled = false;

					labelCircuito.Text = "III-VI";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { });
					break;

				case "CIRCUITO III-VII":
					// Condiciones para CIRCUITO III-VII
					textBoxk1.Enabled = true;
					textBoxk2.Enabled = true;
					textBoxk3.Enabled = true;
					textBoxk4.Enabled = true;
					textBoxp.Enabled = true;
					textBoxI.Enabled = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = true;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = true;
					textBoxL2.Enabled = false;
					textBoxC1.Enabled = true;
					textBoxC2.Enabled = true;

					labelCircuito.Text = "III-VII";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { "iL1", "Vc1","Vc2" });
					break;

				case "CIRCUITO IV-V":
					// Condiciones para CIRCUITO IV-V
					textBoxk1.Enabled = true;
					textBoxk2.Enabled = true;
					textBoxk3.Enabled = true;
					textBoxk4.Enabled = true;
					textBoxp.Enabled = true;
					textBoxI.Enabled = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = true;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = true;
					textBoxL2.Enabled = true;
					textBoxC1.Enabled = true;
					textBoxC2.Enabled = false;

					labelCircuito.Text = "IV-V";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { "iL1", "iL2","Vc1" });
					break;

				case "CIRCUITO IV-VI":
					// Condiciones para CIRCUITO IV-VI
					textBoxk1.Enabled  = true;
					textBoxk2.Enabled  = true;
					textBoxk3.Enabled = true;
					textBoxk4.Enabled  = true;
					textBoxp.Enabled   = true;
					textBoxI.Enabled   = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled  = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = false;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = true;
					textBoxL2.Enabled = false;
					textBoxC1.Enabled = true;
					textBoxC2.Enabled = true;

					labelCircuito.Text = "IV-VI";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { "iL1", "Vceq"});
					break;

				case "CIRCUITO IV-VII":
					// Condiciones para CIRCUITO IV-VII
					textBoxk1.Enabled  = true;
					textBoxk2.Enabled  = true;
					textBoxk3.Enabled = true;
					textBoxk4.Enabled  = true;
					textBoxp.Enabled   = true;
					textBoxI.Enabled   = true;
					textBoxref.Enabled = true;
					textBoxdt.Enabled  = true;

					textBoxR1.Enabled = true;
					textBoxR2.Enabled = true;
					textBoxR3.Enabled = true;
					textBoxR4.Enabled = false;
					textBoxL1.Enabled = true;
					textBoxL2.Enabled = true;
					textBoxC1.Enabled = true;
					textBoxC2.Enabled = true;

					labelCircuito.Text = "IV-VII";
					comboBoxEstadoControlado.Items.Clear();
					comboBoxEstadoControlado.Items.AddRange(new string[] { "iL1", "iL2","Vc1","Vc2" });
					break;

				default:
					// En caso de que se seleccione una opción no válida o "Seleccione"
					MessageBox.Show("Por favor selecciona un circuito válido.");
					break;
			}
		}

		private void comboBoxSeleccionCircuito_TextChanged(object sender, EventArgs e)
		{
			Circuitos();
		}

		private void comboBoxSeleccionCircuito_SelectionChangeCommitted(object sender, EventArgs e)
		{
			comboBoxEstadoControlado.Text = "Seleccione";
		}

		private void comboBoxEstadoControlado_SelectionChangeCommitted(object sender, EventArgs e)
		{
			comboBoxEstadoControlado.Text = comboBoxEstadoControlado.SelectedItem.ToString();
		}
	}
}
