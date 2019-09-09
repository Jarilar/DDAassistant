namespace DDAassistant
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.IMPORTbtn = new System.Windows.Forms.Button();
            this.importShbox = new System.Windows.Forms.TextBox();
            this.Buildbtn = new System.Windows.Forms.Button();
            this.monitorbox = new System.Windows.Forms.TextBox();
            this.Parbtn = new System.Windows.Forms.Button();
            this.Ribtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // IMPORTbtn
            // 
            this.IMPORTbtn.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IMPORTbtn.Location = new System.Drawing.Point(169, 343);
            this.IMPORTbtn.Name = "IMPORTbtn";
            this.IMPORTbtn.Size = new System.Drawing.Size(92, 36);
            this.IMPORTbtn.TabIndex = 0;
            this.IMPORTbtn.Text = "导入";
            this.IMPORTbtn.UseVisualStyleBackColor = true;
            this.IMPORTbtn.Click += new System.EventHandler(this.IMPORTbtn_Click);
            // 
            // importShbox
            // 
            this.importShbox.Location = new System.Drawing.Point(12, 192);
            this.importShbox.Multiline = true;
            this.importShbox.Name = "importShbox";
            this.importShbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.importShbox.Size = new System.Drawing.Size(249, 127);
            this.importShbox.TabIndex = 1;
            // 
            // Buildbtn
            // 
            this.Buildbtn.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Buildbtn.Location = new System.Drawing.Point(280, 343);
            this.Buildbtn.Name = "Buildbtn";
            this.Buildbtn.Size = new System.Drawing.Size(112, 36);
            this.Buildbtn.TabIndex = 2;
            this.Buildbtn.Text = "建立模型";
            this.Buildbtn.UseVisualStyleBackColor = true;
            this.Buildbtn.Click += new System.EventHandler(this.Buildbtn_Click);
            // 
            // monitorbox
            // 
            this.monitorbox.Location = new System.Drawing.Point(280, 39);
            this.monitorbox.Multiline = true;
            this.monitorbox.Name = "monitorbox";
            this.monitorbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.monitorbox.Size = new System.Drawing.Size(392, 280);
            this.monitorbox.TabIndex = 3;
            // 
            // Parbtn
            // 
            this.Parbtn.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Parbtn.Location = new System.Drawing.Point(417, 343);
            this.Parbtn.Name = "Parbtn";
            this.Parbtn.Size = new System.Drawing.Size(119, 36);
            this.Parbtn.TabIndex = 4;
            this.Parbtn.Text = "参数文件";
            this.Parbtn.UseVisualStyleBackColor = true;
            this.Parbtn.Click += new System.EventHandler(this.Parbtn_Click);
            // 
            // Ribtn
            // 
            this.Ribtn.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ribtn.Location = new System.Drawing.Point(567, 343);
            this.Ribtn.Name = "Ribtn";
            this.Ribtn.Size = new System.Drawing.Size(105, 36);
            this.Ribtn.TabIndex = 5;
            this.Ribtn.Text = "折射率";
            this.Ribtn.UseVisualStyleBackColor = true;
            this.Ribtn.Click += new System.EventHandler(this.Ribtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Eras Bold ITC", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 40);
            this.label1.TabIndex = 7;
            this.label1.Text = "DDscat 7.0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(64, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 28);
            this.label2.TabIndex = 8;
            this.label2.Text = "吸湿计算助手";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 443);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Ribtn);
            this.Controls.Add(this.Parbtn);
            this.Controls.Add(this.monitorbox);
            this.Controls.Add(this.Buildbtn);
            this.Controls.Add(this.importShbox);
            this.Controls.Add(this.IMPORTbtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Form1";
            this.Text = "APTX_1869";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button IMPORTbtn;
        private System.Windows.Forms.TextBox importShbox;
        private System.Windows.Forms.Button Buildbtn;
        private System.Windows.Forms.TextBox monitorbox;
        private System.Windows.Forms.Button Parbtn;
        private System.Windows.Forms.Button Ribtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

