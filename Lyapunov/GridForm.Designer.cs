namespace Lyapunov
{
    partial class GridForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridForm));
            this.search_btn = new System.Windows.Forms.Button();
            this.clientlist_lb = new System.Windows.Forms.ListBox();
            this.net_group = new System.Windows.Forms.GroupBox();
            this.net_radio = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.server_group = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.connect_btn = new System.Windows.Forms.Button();
            this.server_radio = new System.Windows.Forms.RadioButton();
            this.net_group.SuspendLayout();
            this.panel1.SuspendLayout();
            this.server_group.SuspendLayout();
            this.SuspendLayout();
            // 
            // search_btn
            // 
            this.search_btn.Location = new System.Drawing.Point(6, 19);
            this.search_btn.Name = "search_btn";
            this.search_btn.Size = new System.Drawing.Size(75, 23);
            this.search_btn.TabIndex = 0;
            this.search_btn.Text = "Search";
            this.search_btn.UseVisualStyleBackColor = true;
            this.search_btn.Click += new System.EventHandler(this.search_btn_Click);
            // 
            // clientlist_lb
            // 
            this.clientlist_lb.FormattingEnabled = true;
            this.clientlist_lb.Location = new System.Drawing.Point(6, 48);
            this.clientlist_lb.Name = "clientlist_lb";
            this.clientlist_lb.Size = new System.Drawing.Size(222, 95);
            this.clientlist_lb.TabIndex = 1;
            // 
            // net_group
            // 
            this.net_group.Controls.Add(this.clientlist_lb);
            this.net_group.Controls.Add(this.search_btn);
            this.net_group.Location = new System.Drawing.Point(12, 35);
            this.net_group.Name = "net_group";
            this.net_group.Size = new System.Drawing.Size(407, 157);
            this.net_group.TabIndex = 2;
            this.net_group.TabStop = false;
            this.net_group.Text = "Network";
            // 
            // net_radio
            // 
            this.net_radio.AutoSize = true;
            this.net_radio.Checked = true;
            this.net_radio.Location = new System.Drawing.Point(12, 12);
            this.net_radio.Name = "net_radio";
            this.net_radio.Size = new System.Drawing.Size(137, 17);
            this.net_radio.TabIndex = 3;
            this.net_radio.TabStop = true;
            this.net_radio.Text = "Listen to whole network";
            this.net_radio.UseVisualStyleBackColor = true;
            this.net_radio.CheckedChanged += new System.EventHandler(this.net_radio_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.server_group);
            this.panel1.Controls.Add(this.server_radio);
            this.panel1.Controls.Add(this.net_radio);
            this.panel1.Controls.Add(this.net_group);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(432, 391);
            this.panel1.TabIndex = 4;
            // 
            // server_group
            // 
            this.server_group.Controls.Add(this.label1);
            this.server_group.Controls.Add(this.textBox1);
            this.server_group.Controls.Add(this.connect_btn);
            this.server_group.Enabled = false;
            this.server_group.Location = new System.Drawing.Point(12, 221);
            this.server_group.Name = "server_group";
            this.server_group.Size = new System.Drawing.Size(407, 157);
            this.server_group.TabIndex = 5;
            this.server_group.TabStop = false;
            this.server_group.Text = "Server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP address";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(70, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            // 
            // connect_btn
            // 
            this.connect_btn.Location = new System.Drawing.Point(6, 43);
            this.connect_btn.Name = "connect_btn";
            this.connect_btn.Size = new System.Drawing.Size(75, 23);
            this.connect_btn.TabIndex = 1;
            this.connect_btn.Text = "Connect";
            this.connect_btn.UseVisualStyleBackColor = true;
            this.connect_btn.Click += new System.EventHandler(this.connect_btn_Click);
            // 
            // server_radio
            // 
            this.server_radio.AutoSize = true;
            this.server_radio.Location = new System.Drawing.Point(12, 198);
            this.server_radio.Name = "server_radio";
            this.server_radio.Size = new System.Drawing.Size(141, 17);
            this.server_radio.TabIndex = 4;
            this.server_radio.Text = "Connect to single Server";
            this.server_radio.UseVisualStyleBackColor = true;
            // 
            // GridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 390);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GridForm";
            this.Text = "GridForm";
            this.net_group.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.server_group.ResumeLayout(false);
            this.server_group.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button search_btn;
        private System.Windows.Forms.ListBox clientlist_lb;
        private System.Windows.Forms.GroupBox net_group;
        private System.Windows.Forms.RadioButton net_radio;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox server_group;
        private System.Windows.Forms.RadioButton server_radio;
        private System.Windows.Forms.Button connect_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
    }
}