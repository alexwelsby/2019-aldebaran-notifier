namespace ADBNotifier
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.LabelLogin = new System.Windows.Forms.Label();
            this.EmailInput = new System.Windows.Forms.TextBox();
            this.passwordInput = new System.Windows.Forms.TextBox();
            this.WhyLabel = new System.Windows.Forms.Label();
            this.LoginButton = new System.Windows.Forms.Button();
            this.loadingLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.howToButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelLogin
            // 
            this.LabelLogin.AutoSize = true;
            this.LabelLogin.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.LabelLogin.Location = new System.Drawing.Point(13, 62);
            this.LabelLogin.MaximumSize = new System.Drawing.Size(378, 0);
            this.LabelLogin.Name = "LabelLogin";
            this.LabelLogin.Size = new System.Drawing.Size(348, 54);
            this.LabelLogin.TabIndex = 0;
            this.LabelLogin.Text = "ENTER YOUR DETAILS BELOW TO LOG IN.";
            this.LabelLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EmailInput
            // 
            this.EmailInput.ForeColor = System.Drawing.Color.Silver;
            this.EmailInput.Location = new System.Drawing.Point(121, 118);
            this.EmailInput.Name = "EmailInput";
            this.EmailInput.Size = new System.Drawing.Size(142, 20);
            this.EmailInput.TabIndex = 1;
            this.EmailInput.Text = "email";
            this.EmailInput.Click += new System.EventHandler(this.EmailInput_Click);
            this.EmailInput.TextChanged += new System.EventHandler(this.EmailInput_TextChanged);
            // 
            // passwordInput
            // 
            this.passwordInput.ForeColor = System.Drawing.Color.Silver;
            this.passwordInput.Location = new System.Drawing.Point(121, 144);
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.PasswordChar = '*';
            this.passwordInput.Size = new System.Drawing.Size(142, 20);
            this.passwordInput.TabIndex = 2;
            this.passwordInput.Text = "password";
            this.passwordInput.Click += new System.EventHandler(this.PasswordInput_Click);
            this.passwordInput.TextChanged += new System.EventHandler(this.PasswordInput_TextChanged);
            this.passwordInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordInput_KeyDown);
            // 
            // WhyLabel
            // 
            this.WhyLabel.AutoSize = true;
            this.WhyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WhyLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.WhyLabel.Location = new System.Drawing.Point(20, 80);
            this.WhyLabel.MaximumSize = new System.Drawing.Size(374, 0);
            this.WhyLabel.Name = "WhyLabel";
            this.WhyLabel.Size = new System.Drawing.Size(0, 18);
            this.WhyLabel.TabIndex = 4;
            // 
            // LoginButton
            // 
            this.LoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.LoginButton.Location = new System.Drawing.Point(151, 170);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(80, 24);
            this.LoginButton.TabIndex = 5;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // loadingLabel
            // 
            this.loadingLabel.AutoSize = true;
            this.loadingLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadingLabel.ForeColor = System.Drawing.Color.DarkGray;
            this.loadingLabel.Location = new System.Drawing.Point(22, 206);
            this.loadingLabel.MaximumSize = new System.Drawing.Size(374, 0);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new System.Drawing.Size(372, 85);
            this.loadingLabel.TabIndex = 6;
            this.loadingLabel.Text = resources.GetString("loadingLabel.Text");
            this.loadingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.loadingLabel.Click += new System.EventHandler(this.LoadingLabel_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(-1, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(404, 40);
            this.label6.TabIndex = 14;
            this.label6.Text = "ADBNOTIFIER";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // howToButton
            // 
            this.howToButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.howToButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.howToButton.Location = new System.Drawing.Point(121, 286);
            this.howToButton.Name = "howToButton";
            this.howToButton.Size = new System.Drawing.Size(143, 30);
            this.howToButton.TabIndex = 7;
            this.howToButton.Text = "Take me to a how-to!";
            this.howToButton.UseVisualStyleBackColor = true;
            this.howToButton.Visible = false;
            this.howToButton.Click += new System.EventHandler(this.HowToButton_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(400, 328);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.howToButton);
            this.Controls.Add(this.loadingLabel);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.WhyLabel);
            this.Controls.Add(this.passwordInput);
            this.Controls.Add(this.EmailInput);
            this.Controls.Add(this.LabelLogin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.Text = "ADBNOTIFIER LOGIN";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelLogin;
        private System.Windows.Forms.TextBox EmailInput;
        private System.Windows.Forms.TextBox passwordInput;
        private System.Windows.Forms.Label WhyLabel;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Label loadingLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button howToButton;
    }
}

