namespace WindowsFormsApplication1
{
    partial class Principal1
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
            this.cargarArchivo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cargarArchivo
            // 
            this.cargarArchivo.Location = new System.Drawing.Point(261, 267);
            this.cargarArchivo.Name = "cargarArchivo";
            this.cargarArchivo.Size = new System.Drawing.Size(161, 52);
            this.cargarArchivo.TabIndex = 0;
            this.cargarArchivo.Text = "Cargar";
            this.cargarArchivo.UseVisualStyleBackColor = true;
            this.cargarArchivo.Click += new System.EventHandler(this.cargarArchivo_Click);
            // 
            // Principal1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 582);
            this.Controls.Add(this.cargarArchivo);
            this.Name = "Principal1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cargarArchivo;
    }
}

