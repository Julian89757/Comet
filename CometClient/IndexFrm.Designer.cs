namespace CometClient
{
    partial class IndexFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IndexFrm));
            this.bodyContainer = new System.Windows.Forms.SplitContainer();
            this.contentBody = new System.Windows.Forms.Panel();
            this.cbText = new System.Windows.Forms.ComboBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bodyContainer)).BeginInit();
            this.bodyContainer.Panel2.SuspendLayout();
            this.bodyContainer.SuspendLayout();
            this.contentBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // bodyContainer
            // 
            this.bodyContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyContainer.IsSplitterFixed = true;
            this.bodyContainer.Location = new System.Drawing.Point(0, 0);
            this.bodyContainer.Name = "bodyContainer";
            this.bodyContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // bodyContainer.Panel1
            // 
            this.bodyContainer.Panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bodyContainer.Panel1.BackgroundImage")));
            // 
            // bodyContainer.Panel2
            // 
            this.bodyContainer.Panel2.Controls.Add(this.contentBody);
            this.bodyContainer.Size = new System.Drawing.Size(510, 312);
            this.bodyContainer.SplitterDistance = 170;
            this.bodyContainer.TabIndex = 0;
            // 
            // contentBody
            // 
            this.contentBody.Controls.Add(this.cbText);
            this.contentBody.Controls.Add(this.txtPwd);
            this.contentBody.Controls.Add(this.btnLogin);
            this.contentBody.Location = new System.Drawing.Point(86, 3);
            this.contentBody.Name = "contentBody";
            this.contentBody.Size = new System.Drawing.Size(330, 123);
            this.contentBody.TabIndex = 0;
            // 
            // cbText
            // 
            this.cbText.FormattingEnabled = true;
            this.cbText.Location = new System.Drawing.Point(93, 15);
            this.cbText.Name = "cbText";
            this.cbText.Size = new System.Drawing.Size(174, 21);
            this.cbText.TabIndex = 1;
            this.cbText.Text = "用户名";
            this.cbText.Enter += new System.EventHandler(this.cbText_Enter);
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(93, 56);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(174, 20);
            this.txtPwd.TabIndex = 2;
            this.txtPwd.Text = "密码";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(93, 97);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(116, 23);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "登陆";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // IndexFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 312);
            this.Controls.Add(this.bodyContainer);
            this.Name = "IndexFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IndexFrm";
            this.bodyContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bodyContainer)).EndInit();
            this.bodyContainer.ResumeLayout(false);
            this.contentBody.ResumeLayout(false);
            this.contentBody.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer bodyContainer;
        private System.Windows.Forms.Panel contentBody;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.ComboBox cbText;

    }
}