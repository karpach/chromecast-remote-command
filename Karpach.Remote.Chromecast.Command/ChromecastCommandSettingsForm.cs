using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GoogleCast;
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
        private Label lbIP;
        private ComboBox _cbxChromeCast;
        private TrackBar _tbVolume;
        private CheckBox _chkVolume;
        private TextBox _txtDelay;

        public ChromecastCommandSettingsForm(ChromecastCommandSettings settings)
        {
            InitializeComponent();
            Settings = settings;
            _txtCommandName.Text = Settings.CommandName;
            _txtDelay.Text = Settings.ExecutionDelay?.ToString() ?? "0";
            Task.Factory.StartNew(async () => { 
                string[] receivers = (await new DeviceLocator().FindReceiversAsync()).Select(r => r.FriendlyName).ToArray();
                _cbxChromeCast.DataSource = receivers;
            },CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());        
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
            this.lbIP = new System.Windows.Forms.Label();
            this._cbxChromeCast = new System.Windows.Forms.ComboBox();
            this._tbVolume = new System.Windows.Forms.TrackBar();
            this._chkVolume = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._tbVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnOk
            // 
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Location = new System.Drawing.Point(130, 169);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 5;
            this._btnOk.Text = "Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(211, 169);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 6;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
            // 
            // _lbCommandName
            // 
            this._lbCommandName.AutoSize = true;
            this._lbCommandName.Location = new System.Drawing.Point(36, 25);
            this._lbCommandName.Name = "_lbCommandName";
            this._lbCommandName.Size = new System.Drawing.Size(88, 13);
            this._lbCommandName.TabIndex = 0;
            this._lbCommandName.Text = "Command Name:";
            // 
            // _txtCommandName
            // 
            this._txtCommandName.Location = new System.Drawing.Point(130, 22);
            this._txtCommandName.Name = "_txtCommandName";
            this._txtCommandName.Size = new System.Drawing.Size(255, 20);
            this._txtCommandName.TabIndex = 0;
            // 
            // _lbDelay
            // 
            this._lbDelay.AutoSize = true;
            this._lbDelay.Location = new System.Drawing.Point(23, 60);
            this._lbDelay.Name = "_lbDelay";
            this._lbDelay.Size = new System.Drawing.Size(101, 13);
            this._lbDelay.TabIndex = 0;
            this._lbDelay.Text = "Execution delay ms:";
            // 
            // _txtDelay
            // 
            this._txtDelay.Location = new System.Drawing.Point(130, 57);
            this._txtDelay.Name = "_txtDelay";
            this._txtDelay.Size = new System.Drawing.Size(257, 20);
            this._txtDelay.TabIndex = 1;
            // 
            // lbIP
            // 
            this.lbIP.AutoSize = true;
            this.lbIP.Location = new System.Drawing.Point(57, 94);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(67, 13);
            this.lbIP.TabIndex = 0;
            this.lbIP.Text = "ChromeCast:";
            // 
            // _cbxChromeCast
            // 
            this._cbxChromeCast.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbxChromeCast.FormattingEnabled = true;
            this._cbxChromeCast.Location = new System.Drawing.Point(130, 89);
            this._cbxChromeCast.Name = "_cbxChromeCast";
            this._cbxChromeCast.Size = new System.Drawing.Size(255, 21);
            this._cbxChromeCast.TabIndex = 2;
            // 
            // _tbVolume
            // 
            this._tbVolume.Enabled = false;
            this._tbVolume.Location = new System.Drawing.Point(161, 122);
            this._tbVolume.Name = "_tbVolume";
            this._tbVolume.Size = new System.Drawing.Size(224, 45);
            this._tbVolume.TabIndex = 4;
            // 
            // _chkVolume
            // 
            this._chkVolume.AutoSize = true;
            this._chkVolume.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkVolume.Location = new System.Drawing.Point(75, 124);
            this._chkVolume.Name = "_chkVolume";
            this._chkVolume.Size = new System.Drawing.Size(70, 17);
            this._chkVolume.TabIndex = 3;
            this._chkVolume.Text = "Volume:  ";
            this._chkVolume.ThreeState = true;
            this._chkVolume.UseVisualStyleBackColor = true;
            this._chkVolume.CheckedChanged += new System.EventHandler(this._chkVolume_CheckedChanged);
            // 
            // ChromecastCommandSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 207);
            this.Controls.Add(this._chkVolume);
            this.Controls.Add(this._tbVolume);
            this.Controls.Add(this._cbxChromeCast);
            this.Controls.Add(this._txtDelay);
            this.Controls.Add(this.lbIP);
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
            ((System.ComponentModel.ISupportInitialize)(this._tbVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void _btnOk_Click(object sender, EventArgs e)
        {
            Settings.CommandName = _txtCommandName.Text;
            Settings.ExecutionDelay = int.TryParse(_txtDelay.Text, out var n) ? n : 0;
            Settings.ChromeCastName = _cbxChromeCast.Text;
            if (_chkVolume.Checked)
            {
                Settings.Volume = (float)_tbVolume.Value / 10;
            }
            else
            {
                Settings.Volume = null;
            }
            Close();
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _chkVolume_CheckedChanged(object sender, EventArgs e)
        {
            _tbVolume.Enabled = _chkVolume.Checked;
        }
    }
}
