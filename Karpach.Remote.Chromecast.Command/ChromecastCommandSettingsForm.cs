using System;
using System.Windows.Forms;
using SampleCommand;

namespace Karpach.Remote.Chromecast.Command
{
    public class ChromecastCommandSettingsForm : Form
    {
        public readonly ChromecastCommandSettings Settings;

        private Button _btnOk;
        private Button _btnCancel;
        private Label _lbCommandName;
        private TextBox _txtCommandName;
        private Label _lbDelay;
        private TextBox _txtDelay;

        public ChromecastCommandSettingsForm(ChromecastCommandSettings settings)
        {
            InitializeComponent();
            Settings = settings;
            _txtCommandName.Text = Settings.CommandName;
            _txtDelay.Text = Settings.ExecutionDelay?.ToString() ?? "0";
        }                

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChromecastCommandSettingsForm));
            this._btnOk = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._lbCommandName = new System.Windows.Forms.Label();
            this._txtCommandName = new System.Windows.Forms.TextBox();
            this._lbDelay = new System.Windows.Forms.Label();
            this._txtDelay = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _btnOk
            // 
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Location = new System.Drawing.Point(130, 100);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 2;
            this._btnOk.Text = "Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(211, 100);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 3;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // _lbCommandName
            // 
            this._lbCommandName.AutoSize = true;
            this._lbCommandName.Location = new System.Drawing.Point(22, 25);
            this._lbCommandName.Name = "_lbCommandName";
            this._lbCommandName.Size = new System.Drawing.Size(88, 13);
            this._lbCommandName.TabIndex = 0;
            this._lbCommandName.Text = "Command Name:";
            // 
            // _txtCommandName
            // 
            this._txtCommandName.Location = new System.Drawing.Point(116, 22);
            this._txtCommandName.Name = "_txtCommandName";
            this._txtCommandName.Size = new System.Drawing.Size(269, 20);
            this._txtCommandName.TabIndex = 0;
            // 
            // _lbDelay
            // 
            this._lbDelay.AutoSize = true;
            this._lbDelay.Location = new System.Drawing.Point(9, 60);
            this._lbDelay.Name = "_lbDelay";
            this._lbDelay.Size = new System.Drawing.Size(101, 13);
            this._lbDelay.TabIndex = 0;
            this._lbDelay.Text = "Execution delay ms:";
            // 
            // _txtDelay
            // 
            this._txtDelay.Location = new System.Drawing.Point(116, 57);
            this._txtDelay.Name = "_txtDelay";
            this._txtDelay.Size = new System.Drawing.Size(271, 20);
            this._txtDelay.TabIndex = 1;
            // 
            // ChromecastCommandSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 140);
            this.Controls.Add(this._txtDelay);
            this.Controls.Add(this._txtCommandName);
            this.Controls.Add(this._lbDelay);
            this.Controls.Add(this._lbCommandName);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ChromecastCommandSettingsForm";
            this.Text = "ChromeCast Command Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void _btnOk_Click(object sender, EventArgs e)
        {
            Settings.CommandName = _txtCommandName.Text;
            int n;
            Settings.ExecutionDelay = int.TryParse(_txtDelay.Text, out n) ? n : 0;
            Close();
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
