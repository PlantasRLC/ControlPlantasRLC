namespace AppControl
{
	partial class Conexion
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.progressBarConexionSerial = new System.Windows.Forms.ProgressBar();
			this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
			this.comboBoxPuerto = new System.Windows.Forms.ComboBox();
			this.labelBaudRate = new System.Windows.Forms.Label();
			this.labelPuerto = new System.Windows.Forms.Label();
			this.buttonConectar = new System.Windows.Forms.Button();
			this.buttonRefrescar = new System.Windows.Forms.Button();
			this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.progressBarConexionSerial);
			this.panel1.Controls.Add(this.comboBoxBaudRate);
			this.panel1.Controls.Add(this.comboBoxPuerto);
			this.panel1.Controls.Add(this.labelBaudRate);
			this.panel1.Controls.Add(this.labelPuerto);
			this.panel1.Controls.Add(this.buttonConectar);
			this.panel1.Controls.Add(this.buttonRefrescar);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(615, 459);
			this.panel1.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Verdana", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(254, 23);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(123, 25);
			this.label3.TabIndex = 7;
			this.label3.Text = "CONEXIÓN";
			// 
			// progressBarConexionSerial
			// 
			this.progressBarConexionSerial.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progressBarConexionSerial.Location = new System.Drawing.Point(0, 415);
			this.progressBarConexionSerial.Margin = new System.Windows.Forms.Padding(10);
			this.progressBarConexionSerial.Name = "progressBarConexionSerial";
			this.progressBarConexionSerial.Size = new System.Drawing.Size(615, 44);
			this.progressBarConexionSerial.TabIndex = 6;
			// 
			// comboBoxBaudRate
			// 
			this.comboBoxBaudRate.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.comboBoxBaudRate.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboBoxBaudRate.FormattingEnabled = true;
			this.comboBoxBaudRate.Location = new System.Drawing.Point(301, 145);
			this.comboBoxBaudRate.Name = "comboBoxBaudRate";
			this.comboBoxBaudRate.Size = new System.Drawing.Size(121, 24);
			this.comboBoxBaudRate.TabIndex = 5;
			// 
			// comboBoxPuerto
			// 
			this.comboBoxPuerto.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.comboBoxPuerto.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboBoxPuerto.FormattingEnabled = true;
			this.comboBoxPuerto.Location = new System.Drawing.Point(174, 145);
			this.comboBoxPuerto.Name = "comboBoxPuerto";
			this.comboBoxPuerto.Size = new System.Drawing.Size(121, 24);
			this.comboBoxPuerto.TabIndex = 4;
			// 
			// labelBaudRate
			// 
			this.labelBaudRate.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelBaudRate.AutoSize = true;
			this.labelBaudRate.Font = new System.Drawing.Font("Verdana", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBaudRate.Location = new System.Drawing.Point(297, 120);
			this.labelBaudRate.Name = "labelBaudRate";
			this.labelBaudRate.Size = new System.Drawing.Size(103, 22);
			this.labelBaudRate.TabIndex = 3;
			this.labelBaudRate.Text = "Baud Rate";
			// 
			// labelPuerto
			// 
			this.labelPuerto.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelPuerto.AutoSize = true;
			this.labelPuerto.Font = new System.Drawing.Font("Verdana", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPuerto.Location = new System.Drawing.Point(170, 120);
			this.labelPuerto.Name = "labelPuerto";
			this.labelPuerto.Size = new System.Drawing.Size(69, 22);
			this.labelPuerto.TabIndex = 2;
			this.labelPuerto.Text = "Puerto";
			// 
			// buttonConectar
			// 
			this.buttonConectar.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.buttonConectar.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonConectar.Location = new System.Drawing.Point(301, 236);
			this.buttonConectar.Name = "buttonConectar";
			this.buttonConectar.Size = new System.Drawing.Size(121, 39);
			this.buttonConectar.TabIndex = 1;
			this.buttonConectar.Text = "Conectar";
			this.buttonConectar.UseVisualStyleBackColor = true;
			this.buttonConectar.Click += new System.EventHandler(this.buttonConectar_Click);
			// 
			// buttonRefrescar
			// 
			this.buttonRefrescar.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.buttonRefrescar.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonRefrescar.Location = new System.Drawing.Point(174, 236);
			this.buttonRefrescar.Name = "buttonRefrescar";
			this.buttonRefrescar.Size = new System.Drawing.Size(121, 39);
			this.buttonRefrescar.TabIndex = 0;
			this.buttonRefrescar.Text = "Refrescar";
			this.buttonRefrescar.UseVisualStyleBackColor = true;
			this.buttonRefrescar.Click += new System.EventHandler(this.buttonRefrescar_Click);
			// 
			// Conexion
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
			this.ClientSize = new System.Drawing.Size(615, 459);
			this.Controls.Add(this.panel1);
			this.Name = "Conexion";
			this.Text = "Conexion";
			this.Load += new System.EventHandler(this.Conexion_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ProgressBar progressBarConexionSerial;
		private System.Windows.Forms.ComboBox comboBoxBaudRate;
		private System.Windows.Forms.ComboBox comboBoxPuerto;
		private System.Windows.Forms.Label labelBaudRate;
		private System.Windows.Forms.Label labelPuerto;
		private System.Windows.Forms.Button buttonConectar;
		private System.Windows.Forms.Button buttonRefrescar;
		private System.Windows.Forms.Label label3;
		public System.IO.Ports.SerialPort serialPort1;
	}
}