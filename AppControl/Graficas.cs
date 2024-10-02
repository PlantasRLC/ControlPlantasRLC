using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppControl
{
	public partial class Graficas : Form
	{
		// Variable para mantener la referencia a configuración
		private Configuracion _configuracionForm;
		// Variable para mantener la referencia a conexion
		private Conexion _conexionForm;

		public Graficas(Conexion conexion, Configuracion configuracionForm)
		{
			InitializeComponent();
			_conexionForm = conexion;
			_configuracionForm = configuracionForm;
		}



		public bool comunicacionActiva = false;
		public async void buttonLeer_Click(object sender, EventArgs e)
		{
			try
			{
				
				// Verificar si la comunicación está activa
				if (!comunicacionActiva)
				{

					buttonLeer.Text = "Detener Comunicación";
					buttonLeer.BackColor = Color.LightPink;
					StartCommunication();

					// Ahora creamos una tarea asíncrona para leer los datos
					await Task.Run(() =>
					{
						int tramaCounter = 0;

						while (comunicacionActiva && _conexionForm.serialPort1.IsOpen)
						{
							// Verificar si hay datos disponibles en el puerto serial
							if (_conexionForm.serialPort1.BytesToRead > 0)
							{
								// Incrementamos el contador de tramas recibidas
								tramaCounter++;

								// Ignorar las primeras 20 tramas
								if (tramaCounter <= 20)
								{
									continue;  // No procesamos estas tramas, las descartamos
								}

								string serialData = _conexionForm.serialPort1.ReadLine();  // Leer línea completa de datos
								serialData = serialData.Trim().Trim('<', '>');  // Limpiar delimitadores

								string[] parts = serialData.Split(':');
								if (parts.Length == 3)
								{
									string id = parts[0].Trim();
									string value = parts[1].Trim();
									string type = parts[2].Trim();

									if (type == "F")
									{
										if (float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float floatValue))
										{
											// Usar un switch para graficar dependiendo del ID
											switch (id)
											{
												case "Vestado0":
													chart1.Invoke((MethodInvoker)(() => chart1.Series["Analog0"].Points.AddY(floatValue)));
													chart1.Invoke((MethodInvoker)(() => chart1.Series["Ref"].Points.AddY(_configuracionForm.referencia)));
													if (chart1.Series["Analog0"].Points.Count > 1000)
													{
														chart1.Invoke((MethodInvoker)(() => chart1.Series["Analog0"].Points.Clear()));
														chart1.Invoke((MethodInvoker)(() => chart1.Series["Ref"].Points.Clear()));
													}
													break;

												case "Vestado1":
													chart2.Invoke((MethodInvoker)(() => chart2.Series["Analog1"].Points.AddY(floatValue)));
													if (chart2.Series["Analog1"].Points.Count > 1000)
													{
														chart2.Invoke((MethodInvoker)(() => chart2.Series["Analog1"].Points.Clear()));
													}
													break;

												case "Vestado2":
													chart3.Invoke((MethodInvoker)(() => chart3.Series["Analog2"].Points.AddY(floatValue)));
													if (chart3.Series["Analog2"].Points.Count > 1000)
													{
														chart3.Invoke((MethodInvoker)(() => chart3.Series["Analog2"].Points.Clear()));
													}
													break;

												case "Vestado3":
													chart4.Invoke((MethodInvoker)(() => chart4.Series["Analog3"].Points.AddY(floatValue)));
													if (chart4.Series["Analog3"].Points.Count > 1000)
													{
														chart4.Invoke((MethodInvoker)(() => chart4.Series["Analog3"].Points.Clear()));
													}
													break;

												default:
													// Si el ID no coincide con ninguno, manejar el error
													chart1.Invoke((MethodInvoker)(() => MessageBox.Show("ID no reconocido: " + id)));
													break;
											}
											//Control();
										}
										else
										{
											// Si no pudo parsear, muestra un mensaje de error
											chart1.Invoke((MethodInvoker)(() => MessageBox.Show("Error al convertir el valor a flotante: " + value)));
										}
									}
									else
									{
										// Si no es un tipo de dato reconocido
										chart1.Invoke((MethodInvoker)(() => MessageBox.Show("Tipo de dato no reconocido.")));
									}
								}
								else
								{
									// Manejo de error si la trama no tiene el formato correcto
									chart1.Invoke((MethodInvoker)(() => MessageBox.Show("Trama inválida.")));
								}
							}

							// Pequeño retardo para no bloquear el hilo
							Thread.Sleep(10);
						}
					});
				}
				else
				{
					buttonLeer.Text = "Iniciar Comunicación";
					buttonLeer.BackColor = Color.Orchid;
					StopCommunication();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void StartCommunication()
		{
			try
			{
				_conexionForm.serialPort1.Write("%S\n");  // Enviar señal al Arduino para comenzar a enviar datos
				chart1.Invoke((MethodInvoker)(() => chart1.Series["Analog0"].Points.Clear()));
				chart1.Invoke((MethodInvoker)(() => chart2.Series["Analog1"].Points.Clear()));
				chart1.Invoke((MethodInvoker)(() => chart3.Series["Analog2"].Points.Clear()));
				chart1.Invoke((MethodInvoker)(() => chart4.Series["Analog3"].Points.Clear()));
				chart1.Invoke((MethodInvoker)(() => chart1.Series["Ref"].Points.Clear()));
				comunicacionActiva = true;
				
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error al iniciar la comunicación: " + ex.Message);
			}
		}

		private void StopCommunication()
		{
			try
			{
				if (_conexionForm.serialPort1.IsOpen)
				{
					_conexionForm.serialPort1.Write("%C\n");  // Enviar señal al Arduino para detener envío de datos

					// Limpia el buffer de entrada de datos
					_conexionForm.serialPort1.DiscardInBuffer();
				}
				comunicacionActiva = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error al detener la comunicación: " + ex.Message);
			}
		}



	}
}
