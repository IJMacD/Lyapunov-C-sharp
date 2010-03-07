namespace Lyapunov
{
    partial class TilesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TilesForm));
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
            this.prog_tiles = new System.Windows.Forms.ProgressBar();
            this.listView1 = new System.Windows.Forms.ListView();
            this.hdr_Zoom = new System.Windows.Forms.ColumnHeader();
            this.hdr_Iterations = new System.Windows.Forms.ColumnHeader();
            this.hdr_Path = new System.Windows.Forms.ColumnHeader();
            this.btn_add = new System.Windows.Forms.Button();
            this.prog_jobs = new System.Windows.Forms.ProgressBar();
            this.lbl_count = new System.Windows.Forms.Label();
            this.btn_remove = new System.Windows.Forms.Button();
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
            this.txt_zoom.Size = new System.Drawing.Size(287, 20);
            this.txt_zoom.TabIndex = 3;
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
            this.txt_iterations.TabIndex = 4;
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
            this.txt_folder.TabIndex = 5;
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
            this.btn_start.Location = new System.Drawing.Point(197, 314);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(158, 23);
            this.btn_start.TabIndex = 7;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // prog_pri
            // 
            this.prog_pri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.prog_pri.Location = new System.Drawing.Point(12, 256);
            this.prog_pri.Name = "prog_pri";
            this.prog_pri.Size = new System.Drawing.Size(343, 23);
            this.prog_pri.TabIndex = 8;
            // 
            // prog_tiles
            // 
            this.prog_tiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.prog_tiles.Location = new System.Drawing.Point(12, 285);
            this.prog_tiles.Name = "prog_tiles";
            this.prog_tiles.Size = new System.Drawing.Size(343, 23);
            this.prog_tiles.TabIndex = 14;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdr_Zoom,
            this.hdr_Iterations,
            this.hdr_Path});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(12, 151);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(343, 99);
            this.listView1.TabIndex = 15;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // hdr_Zoom
            // 
            this.hdr_Zoom.Tag = "Zoom";
            this.hdr_Zoom.Text = "Zoom";
            this.hdr_Zoom.Width = 40;
            // 
            // hdr_Iterations
            // 
            this.hdr_Iterations.Tag = "Iterations";
            this.hdr_Iterations.Text = "Iterations";
            // 
            // hdr_Path
            // 
            this.hdr_Path.Tag = "Path";
            this.hdr_Path.Text = "Folder";
            this.hdr_Path.Width = 230;
            // 
            // btn_add
            // 
            this.btn_add.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_add.Location = new System.Drawing.Point(280, 93);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 16;
            this.btn_add.Text = "Add";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // prog_jobs
            // 
            this.prog_jobs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.prog_jobs.Location = new System.Drawing.Point(12, 122);
            this.prog_jobs.Name = "prog_jobs";
            this.prog_jobs.Size = new System.Drawing.Size(343, 23);
            this.prog_jobs.TabIndex = 17;
            // 
            // lbl_count
            // 
            this.lbl_count.AutoSize = true;
            this.lbl_count.Location = new System.Drawing.Point(15, 315);
            this.lbl_count.Name = "lbl_count";
            this.lbl_count.Size = new System.Drawing.Size(0, 13);
            this.lbl_count.TabIndex = 18;
            // 
            // btn_remove
            // 
            this.btn_remove.Location = new System.Drawing.Point(12, 93);
            this.btn_remove.Name = "btn_remove";
            this.btn_remove.Size = new System.Drawing.Size(75, 23);
            this.btn_remove.TabIndex = 19;
            this.btn_remove.Text = "Remove";
            this.btn_remove.UseVisualStyleBackColor = true;
            this.btn_remove.Click += new System.EventHandler(this.btn_remove_Click);
            // 
            // TilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 356);
            this.Controls.Add(this.btn_remove);
            this.Controls.Add(this.lbl_count);
            this.Controls.Add(this.prog_jobs);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.prog_tiles);
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
            this.Name = "TilesForm";
            this.Text = "Tiles";
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
        private System.Windows.Forms.ProgressBar prog_tiles;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.ProgressBar prog_jobs;
        private System.Windows.Forms.ColumnHeader hdr_Iterations;
        private System.Windows.Forms.ColumnHeader hdr_Path;
        private System.Windows.Forms.ColumnHeader hdr_Zoom;
        private System.Windows.Forms.Label lbl_count;
        private System.Windows.Forms.Button btn_remove;
    }
}