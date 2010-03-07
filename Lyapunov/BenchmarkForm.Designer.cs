namespace Lyapunov
{
    partial class BenchmarkForm
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
            this.type_btn = new System.Windows.Forms.RadioButton();
            this.numruns_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.run_btn = new System.Windows.Forms.Button();
            this.iter_txt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.clear_btn = new System.Windows.Forms.Button();
            this.result_lst = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // type_btn
            // 
            this.type_btn.AutoSize = true;
            this.type_btn.Checked = true;
            this.type_btn.Location = new System.Drawing.Point(12, 12);
            this.type_btn.Name = "type_btn";
            this.type_btn.Size = new System.Drawing.Size(118, 17);
            this.type_btn.TabIndex = 0;
            this.type_btn.TabStop = true;
            this.type_btn.Text = "Standard 256 x 256";
            this.type_btn.UseVisualStyleBackColor = true;
            // 
            // numruns_txt
            // 
            this.numruns_txt.Location = new System.Drawing.Point(12, 35);
            this.numruns_txt.Name = "numruns_txt";
            this.numruns_txt.Size = new System.Drawing.Size(50, 20);
            this.numruns_txt.TabIndex = 1;
            this.numruns_txt.Text = "10";
            this.numruns_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Num Runs";
            // 
            // run_btn
            // 
            this.run_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.run_btn.Location = new System.Drawing.Point(209, 178);
            this.run_btn.Name = "run_btn";
            this.run_btn.Size = new System.Drawing.Size(75, 23);
            this.run_btn.TabIndex = 4;
            this.run_btn.Text = "Start";
            this.run_btn.UseVisualStyleBackColor = true;
            this.run_btn.Click += new System.EventHandler(this.run_btn_Click);
            // 
            // iter_txt
            // 
            this.iter_txt.Location = new System.Drawing.Point(131, 35);
            this.iter_txt.Name = "iter_txt";
            this.iter_txt.Size = new System.Drawing.Size(65, 20);
            this.iter_txt.TabIndex = 5;
            this.iter_txt.Text = "1000";
            this.iter_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Iterations";
            // 
            // clear_btn
            // 
            this.clear_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clear_btn.Location = new System.Drawing.Point(12, 178);
            this.clear_btn.Name = "clear_btn";
            this.clear_btn.Size = new System.Drawing.Size(75, 23);
            this.clear_btn.TabIndex = 8;
            this.clear_btn.Text = "Clear";
            this.clear_btn.UseVisualStyleBackColor = true;
            this.clear_btn.Click += new System.EventHandler(this.clear_btn_Click);
            // 
            // result_lst
            // 
            this.result_lst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.result_lst.Location = new System.Drawing.Point(12, 61);
            this.result_lst.Name = "result_lst";
            this.result_lst.Size = new System.Drawing.Size(272, 111);
            this.result_lst.TabIndex = 9;
            this.result_lst.TileSize = new System.Drawing.Size(45, 16);
            this.result_lst.UseCompatibleStateImageBehavior = false;
            this.result_lst.View = System.Windows.Forms.View.Tile;
            // 
            // BenchmarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 213);
            this.Controls.Add(this.result_lst);
            this.Controls.Add(this.clear_btn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.iter_txt);
            this.Controls.Add(this.run_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numruns_txt);
            this.Controls.Add(this.type_btn);
            this.Name = "BenchmarkForm";
            this.Text = "Benchmark";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton type_btn;
        private System.Windows.Forms.TextBox numruns_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button run_btn;
        private System.Windows.Forms.TextBox iter_txt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button clear_btn;
        private System.Windows.Forms.ListView result_lst;
    }
}