using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AppControl
{
	public partial class Form1 : Form
	{
		// Declarar las instancias de los formularios
		private Conexion conexionForm;
		private Configuracion configuracionForm;
		private Graficas graficasForm;

		//Fields
		private int borderSize = 2;


		public Form1()
		{
			InitializeComponent();
			CollapseMenu();
			this.Padding = new Padding(borderSize); //Border Size
			this.BackColor = Color.FromArgb(98, 102, 244);

			// Inicializar los formularios cuando se carga Form1
			conexionForm = new Conexion();
			configuracionForm = new Configuracion(conexionForm);
			graficasForm = new Graficas(conexionForm, configuracionForm);
		}

		//Drag Forms
		[DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
		private extern static void ReleaseCapture();

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

		private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			SendMessage(this.Handle, 0x112, 0xf012, 0);
		}

		//Override Methods
		protected override void WndProc(ref Message m)
		{
			const int WM_NCCALCSIZE = 0x0083;
			if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
			{
				return;
			}
			base.WndProc(ref m);
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			AdjustForm();
		}

		private void AdjustForm()
		{
			switch (this.WindowState)
			{
				case FormWindowState.Maximized:
					this.Padding = new Padding(8, 8, 8, 0);
					break;
				case FormWindowState.Normal:
					if (this.Padding.Top != borderSize)
						this.Padding = new Padding(borderSize);
					break;
			}
			

		}

		private void btnMinimize_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

		private void btnMaximize_Click(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Normal)
			{
				this.WindowState = FormWindowState.Maximized;

			}
			else
			{
				this.WindowState = FormWindowState.Normal;
			}

		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void iconButton1_Click(object sender, EventArgs e)
		{
			CollapseMenu();
		}

		private void CollapseMenu()
		{
			if (this.panelMenu.Width > 200)
			{//Collapse Menu
				panelMenu.Width = 100;
				pictureBox1.Visible = false;
				btnMenu.Dock = DockStyle.Top;
				foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
				{
					menuButton.Text = "";
					menuButton.ImageAlign = ContentAlignment.MiddleCenter;
					menuButton.Padding = new Padding(0);
				}
			}
			else
			{ //Expand Menu
				panelMenu.Width = 230;
				pictureBox1.Visible = true;
				btnMenu.Dock = DockStyle.None;
				foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
				{
					menuButton.Text = "   " + menuButton.Tag.ToString();
					menuButton.ImageAlign = ContentAlignment.MiddleLeft;
					menuButton.Padding = new Padding(10, 0, 0, 0);
				}
			}

		}

		////Función Multiples formularios en Desktop
		//private Form activeForm = null;
		//private void openChildForm(Form childForm)
		//{
		//	if (activeForm != null)
		//		activeForm.Visible=false;
		//	activeForm = childForm;
		//	childForm.TopLevel = false;
		//	childForm.FormBorderStyle = FormBorderStyle.None;
		//	childForm.Dock = DockStyle.Fill;
		//	panelDesktop.Controls.Add(childForm);
		//	panelDesktop.Tag = childForm;
		//	childForm.BringToFront();
		//	childForm.Show();
		//}

		private Form activeForm = null;

		private void openChildForm(Form childForm)
		{
			if (activeForm != null)
			{
				activeForm.Hide(); // Ocultar el formulario activo en lugar de cerrarlo
			}

			activeForm = childForm;
			childForm.TopLevel = false;
			childForm.FormBorderStyle = FormBorderStyle.None;
			childForm.Dock = DockStyle.Fill;

			// Si el formulario no está añadido ya al panel, se añade
			if (!panelDesktop.Controls.Contains(childForm))
			{
				panelDesktop.Controls.Add(childForm);
				panelDesktop.Tag = childForm;
			}

			childForm.BringToFront(); // Traer el nuevo formulario al frente
			childForm.Show();         // Mostrar el formulario hijo
		}


		private void iconButton2_Click(object sender, EventArgs e)
		{
			openChildForm(conexionForm); // Usar la instancia existente
			//Desactivar comunicación
			if (graficasForm.comunicacionActiva)
			{
				// Llamar al evento como si el botón hubiera sido presionado
				graficasForm.buttonLeer_Click(null, EventArgs.Empty);

			}
;
		}

		private void iconButton3_Click(object sender, EventArgs e)
		{
			openChildForm(configuracionForm); // Usar la instancia existente
											  //Desactivar comunicación
			if (graficasForm.comunicacionActiva) {
				// Llamar al evento como si el botón hubiera sido presionado
				graficasForm.buttonLeer_Click(null, EventArgs.Empty);

			}


		}

		private void iconButton4_Click(object sender, EventArgs e)
		{
			openChildForm(graficasForm); // Usar la instancia existente
		}
	}
}
