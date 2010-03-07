namespace Lyapunov
{
    partial class SingleTileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleTileForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_zoom = new System.Windows.Forms.TextBox();
            this.txt_iterations = new System.Windows.Forms.TextBox();
            this.txt_folder = new System.Windows.Forms.TextBox();
            this.btn_browse = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_start = new System.Windows.Forms.Button();
            this.prog_pri = new System.Windows.Forms.ProgressBar();
            this.btn_save = new System.Windows.Forms.Button();
            this.lbl_count = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_X = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_Y = new System.Windows.Forms.TextBox();
            this._output_pb = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rad_bump = new System.Windows.Forms.RadioButton();
            this.rad_norm = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this._output_pb)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zoom";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Iterations";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Folder";
            // 
            // txt_zoom
            // 
            this.txt_zoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_zoom.Location = new System.Drawing.Point(68, 12);
            this.txt_zoom.Name = "txt_zoom";
            this.txt_zoom.Size = new System.Drawing.Size(70, 20);
            this.txt_zoom.TabIndex = 0;
            this.txt_zoom.Text = "0";
            this.txt_zoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txt_iterations
            // 
            this.txt_iterations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_iterations.Location = new System.Drawing.Point(68, 38);
            this.txt_iterations.Name = "txt_iterations";
            this.txt_iterations.Size = new System.Drawing.Size(287, 20);
            this.txt_iterations.TabIndex = 3;
            this.txt_iterations.Text = "1000";
            this.txt_iterations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txt_folder
            // 
            this.txt_folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_folder.Location = new System.Drawing.Point(68, 66);
            this.txt_folder.Name = "txt_folder";
            this.txt_folder.Size = new System.Drawing.Size(206, 20);
            this.txt_folder.TabIndex = 4;
            // 
            // btn_browse
            // 
            this.btn_browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_browse.Location = new System.Drawing.Point(280, 64);
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.Size = new System.Drawing.Size(75, 23);
            this.btn_browse.TabIndex = 6;
            this.btn_browse.Text = "Browse";
            this.btn_browse.UseVisualStyleBackColor = true;
            this.btn_browse.Click += new System.EventHandler(this.btn_browse_Click);
            // 
            // btn_start
            // 
            this.btn_start.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_start.Location = new System.Drawing.Point(197, 384);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(158, 23);
            this.btn_start.TabIndex = 5;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // prog_pri
            // 
            this.prog_pri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.prog_pri.Location = new System.Drawing.Point(12, 355);
            this.prog_pri.Name = "prog_pri";
            this.prog_pri.Size = new System.Drawing.Size(343, 23);
            this.prog_pri.TabIndex = 8;
            // 
            // btn_save
            // 
            this.btn_save.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_save.Location = new System.Drawing.Point(280, 93);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 16;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // lbl_count
            // 
            this.lbl_count.AutoSize = true;
            this.lbl_count.Location = new System.Drawing.Point(12, 389);
            this.lbl_count.Name = "lbl_count";
            this.lbl_count.Size = new System.Drawing.Size(0, 13);
            this.lbl_count.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "X";
            // 
            // txt_X
            // 
            this.txt_X.Location = new System.Drawing.Point(164, 12);
            this.txt_X.Name = "txt_X";
            this.txt_X.Size = new System.Drawing.Size(70, 20);
            this.txt_X.TabIndex = 1;
            this.txt_X.Text = "0";
            this.txt_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(240, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Y";
            // 
            // txt_Y
            // 
            this.txt_Y.Location = new System.Drawing.Point(260, 12);
            this.txt_Y.Name = "txt_Y";
            this.txt_Y.Size = new System.Drawing.Size(70, 20);
            this.txt_Y.TabIndex = 2;
            this.txt_Y.Text = "0";
            this.txt_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _output_pb
            // 
            this._output_pb.Location = new System.Drawing.Point(12, 93);
            this._output_pb.Name = "_output_pb";
            this._output_pb.Size = new System.Drawing.Size(256, 256);
            this._output_pb.TabIndex = 19;
            this._output_pb.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rad_bump);
            this.panel1.Controls.Add(this.rad_norm);
            this.panel1.Location = new System.Drawing.Point(280, 123);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(75, 226);
            this.panel1.TabIndex = 23;
            // 
            // rad_bump
            // 
            this.rad_bump.AutoSize = true;
            this.rad_bump.Location = new System.Drawing.Point(4, 28);
            this.rad_bump.Name = "rad_bump";
            this.rad_bump.Size = new System.Drawing.Size(52, 17);
            this.rad_bump.TabIndex = 1;
            this.rad_bump.Text = "Bump";
            this.rad_bump.UseVisualStyleBackColor = true;
            // 
            // rad_norm
            // 
            this.rad_norm.AutoSize = true;
            this.rad_norm.Checked = true;
            this.rad_norm.Location = new System.Drawing.Point(4, 4);
            this.rad_norm.Name = "rad_norm";
            this.rad_norm.Size = new System.Drawing.Size(58, 17);
            this.rad_norm.TabIndex = 0;
            this.rad_norm.TabStop = true;
            this.rad_norm.Text = "Normal";
            this.rad_norm.UseVisualStyleBackColor = true;
            // 
            // SingleTileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 417);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txt_Y);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_X);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._output_pb);
            this.Controls.Add(this.lbl_count);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.prog_pri);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.btn_browse);
            this.Controls.Add(this.txt_folder);
            this.Controls.Add(this.txt_iterations);
            this.Controls.Add(this.txt_zoom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SingleTileForm";
            this.Text = "Tiles";
            ((System.ComponentModel.ISupportInitialize)(this._output_pb)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_zoom;
        private System.Windows.Forms.TextBox txt_iterations;
        private System.Windows.Forms.TextBox txt_folder;
        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.ProgressBar prog_pri;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label lbl_count;
        private System.Windows.Forms.PictureBox _output_pb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_X;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_Y;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rad_bump;
        private System.Windows.Forms.RadioButton rad_norm;
    }
}