namespace Gamma_Manager
{
    partial class Window
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.trackBarGamma = new System.Windows.Forms.TrackBar();
            this.buttonRed = new System.Windows.Forms.Button();
            this.buttonGreen = new System.Windows.Forms.Button();
            this.buttonBlue = new System.Windows.Forms.Button();
            this.buttonAllColors = new System.Windows.Forms.Button();
            this.comboBoxPresets = new System.Windows.Forms.ComboBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.comboBoxMonitors = new System.Windows.Forms.ComboBox();
            this.trackBarContrast = new System.Windows.Forms.TrackBar();
            this.trackBarBrightness = new System.Windows.Forms.TrackBar();
            this.textBoxGamma = new System.Windows.Forms.TextBox();
            this.textBoxContrast = new System.Windows.Forms.TextBox();
            this.textBoxBrightness = new System.Windows.Forms.TextBox();
            this.labelGamma = new System.Windows.Forms.Label();
            this.labelContrast = new System.Windows.Forms.Label();
            this.labelBrightness = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.labelMonitorBrightnessUp = new System.Windows.Forms.Label();
            this.textBoxMonitorBrightness = new System.Windows.Forms.TextBox();
            this.trackBarMonitorBrightness = new System.Windows.Forms.TrackBar();
            this.labelMonitorBrightnessDown = new System.Windows.Forms.Label();
            this.buttonHide = new System.Windows.Forms.Button();
            this.labelMonitorContrastUp = new System.Windows.Forms.Label();
            this.labelMonitorContrastDown = new System.Windows.Forms.Label();
            this.trackBarMonitorContrast = new System.Windows.Forms.TrackBar();
            this.textBoxMonitorContrast = new System.Windows.Forms.TextBox();
            this.buttonForward = new System.Windows.Forms.Button();
            this.buttonAllMonitors = new System.Windows.Forms.Button();
            this.checkBoxStartWithWindows = new System.Windows.Forms.CheckBox();
            this.checkBoxExContrast = new System.Windows.Forms.CheckBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBoxColorAdjustment = new System.Windows.Forms.GroupBox();
            this.groupBoxDisplay = new System.Windows.Forms.GroupBox();
            this.groupBoxHardware = new System.Windows.Forms.GroupBox();
            this.groupBoxPreview = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGamma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMonitorBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMonitorContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBoxColorAdjustment.SuspendLayout();
            this.groupBoxDisplay.SuspendLayout();
            this.groupBoxHardware.SuspendLayout();
            this.groupBoxPreview.SuspendLayout();
            this.SuspendLayout();
            //
            // trackBarGamma
            //
            this.trackBarGamma.LargeChange = 1;
            this.trackBarGamma.Location = new System.Drawing.Point(100, 20);
            this.trackBarGamma.Maximum = 440;
            this.trackBarGamma.Minimum = 30;
            this.trackBarGamma.Name = "trackBarGamma";
            this.trackBarGamma.Size = new System.Drawing.Size(400, 40);
            this.trackBarGamma.SmallChange = 5;
            this.trackBarGamma.TabIndex = 0;
            this.trackBarGamma.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarGamma.Value = 100;
            this.trackBarGamma.ValueChanged += new System.EventHandler(this.trackBarGamma_ValueChanged);
            //
            // buttonRed
            //
            this.buttonRed.Location = new System.Drawing.Point(596, 61);
            this.buttonRed.Name = "buttonRed";
            this.buttonRed.Size = new System.Drawing.Size(80, 34);
            this.buttonRed.TabIndex = 1;
            this.buttonRed.Text = "Red";
            this.buttonRed.UseVisualStyleBackColor = true;
            this.buttonRed.Click += new System.EventHandler(this.buttonRed_Click);
            //
            // buttonGreen
            //
            this.buttonGreen.Location = new System.Drawing.Point(684, 61);
            this.buttonGreen.Name = "buttonGreen";
            this.buttonGreen.Size = new System.Drawing.Size(80, 34);
            this.buttonGreen.TabIndex = 2;
            this.buttonGreen.Text = "Green";
            this.buttonGreen.UseVisualStyleBackColor = true;
            this.buttonGreen.Click += new System.EventHandler(this.buttonGreen_Click);
            //
            // buttonBlue
            //
            this.buttonBlue.Location = new System.Drawing.Point(596, 102);
            this.buttonBlue.Name = "buttonBlue";
            this.buttonBlue.Size = new System.Drawing.Size(80, 34);
            this.buttonBlue.TabIndex = 3;
            this.buttonBlue.Text = "Blue";
            this.buttonBlue.UseVisualStyleBackColor = true;
            this.buttonBlue.Click += new System.EventHandler(this.buttonBlue_Click);
            //
            // buttonAllColors
            //
            this.buttonAllColors.Location = new System.Drawing.Point(596, 20);
            this.buttonAllColors.Name = "buttonAllColors";
            this.buttonAllColors.Size = new System.Drawing.Size(168, 34);
            this.buttonAllColors.TabIndex = 4;
            this.buttonAllColors.Text = "All Colors";
            this.buttonAllColors.UseVisualStyleBackColor = true;
            this.buttonAllColors.Click += new System.EventHandler(this.buttonAllColors_Click);
            //
            // comboBoxPresets
            //
            this.comboBoxPresets.FormattingEnabled = true;
            this.comboBoxPresets.Location = new System.Drawing.Point(270, 27);
            this.comboBoxPresets.Name = "comboBoxPresets";
            this.comboBoxPresets.Size = new System.Drawing.Size(278, 24);
            this.comboBoxPresets.TabIndex = 5;
            this.comboBoxPresets.SelectedIndexChanged += new System.EventHandler(this.comboBoxPresets_SelectedIndexChanged);
            //
            // buttonReset
            //
            this.buttonReset.Location = new System.Drawing.Point(558, 65);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(98, 28);
            this.buttonReset.TabIndex = 6;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            //
            // buttonSave
            //
            this.buttonSave.Location = new System.Drawing.Point(558, 27);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(98, 28);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            //
            // comboBoxMonitors
            //
            this.comboBoxMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMonitors.FormattingEnabled = true;
            this.comboBoxMonitors.Location = new System.Drawing.Point(16, 27);
            this.comboBoxMonitors.Name = "comboBoxMonitors";
            this.comboBoxMonitors.Size = new System.Drawing.Size(166, 24);
            this.comboBoxMonitors.TabIndex = 8;
            this.comboBoxMonitors.SelectedIndexChanged += new System.EventHandler(this.comboBoxMonitors_SelectedIndexChanged);
            //
            // trackBarContrast
            //
            this.trackBarContrast.LargeChange = 1;
            this.trackBarContrast.Location = new System.Drawing.Point(100, 104);
            this.trackBarContrast.Maximum = 300;
            this.trackBarContrast.Minimum = 10;
            this.trackBarContrast.Name = "trackBarContrast";
            this.trackBarContrast.Size = new System.Drawing.Size(400, 40);
            this.trackBarContrast.TabIndex = 9;
            this.trackBarContrast.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarContrast.Value = 10;
            this.trackBarContrast.ValueChanged += new System.EventHandler(this.trackBarContrast_ValueChanged);
            //
            // trackBarBrightness
            //
            this.trackBarBrightness.LargeChange = 1;
            this.trackBarBrightness.Location = new System.Drawing.Point(100, 62);
            this.trackBarBrightness.Maximum = 100;
            this.trackBarBrightness.Minimum = -100;
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Size = new System.Drawing.Size(400, 40);
            this.trackBarBrightness.SmallChange = 5;
            this.trackBarBrightness.TabIndex = 10;
            this.trackBarBrightness.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarBrightness.ValueChanged += new System.EventHandler(this.trackBarBrightness_ValueChanged);
            //
            // textBoxGamma
            //
            this.textBoxGamma.Location = new System.Drawing.Point(514, 27);
            this.textBoxGamma.Name = "textBoxGamma";
            this.textBoxGamma.Size = new System.Drawing.Size(60, 23);
            this.textBoxGamma.TabIndex = 11;
            this.textBoxGamma.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxGamma.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sliderTextBox_KeyDown);
            this.textBoxGamma.Leave += new System.EventHandler(this.sliderTextBox_Leave);
            //
            // textBoxContrast
            //
            this.textBoxContrast.Location = new System.Drawing.Point(514, 111);
            this.textBoxContrast.Name = "textBoxContrast";
            this.textBoxContrast.Size = new System.Drawing.Size(60, 23);
            this.textBoxContrast.TabIndex = 12;
            this.textBoxContrast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxContrast.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sliderTextBox_KeyDown);
            this.textBoxContrast.Leave += new System.EventHandler(this.sliderTextBox_Leave);
            //
            // textBoxBrightness
            //
            this.textBoxBrightness.Location = new System.Drawing.Point(514, 69);
            this.textBoxBrightness.Name = "textBoxBrightness";
            this.textBoxBrightness.Size = new System.Drawing.Size(60, 23);
            this.textBoxBrightness.TabIndex = 13;
            this.textBoxBrightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxBrightness.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sliderTextBox_KeyDown);
            this.textBoxBrightness.Leave += new System.EventHandler(this.sliderTextBox_Leave);
            //
            // labelGamma
            //
            this.labelGamma.AutoSize = true;
            this.labelGamma.Location = new System.Drawing.Point(16, 32);
            this.labelGamma.Name = "labelGamma";
            this.labelGamma.Size = new System.Drawing.Size(53, 15);
            this.labelGamma.TabIndex = 14;
            this.labelGamma.Text = "Gamma";
            //
            // labelContrast
            //
            this.labelContrast.AutoSize = true;
            this.labelContrast.Location = new System.Drawing.Point(16, 116);
            this.labelContrast.Name = "labelContrast";
            this.labelContrast.Size = new System.Drawing.Size(53, 15);
            this.labelContrast.TabIndex = 15;
            this.labelContrast.Text = "Contrast";
            //
            // labelBrightness
            //
            this.labelBrightness.AutoSize = true;
            this.labelBrightness.Location = new System.Drawing.Point(16, 74);
            this.labelBrightness.Name = "labelBrightness";
            this.labelBrightness.Size = new System.Drawing.Size(66, 15);
            this.labelBrightness.TabIndex = 16;
            this.labelBrightness.Text = "Brightness";
            //
            // buttonDelete
            //
            this.buttonDelete.Location = new System.Drawing.Point(664, 27);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 28);
            this.buttonDelete.TabIndex = 17;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            //
            // labelMonitorBrightnessUp
            //
            this.labelMonitorBrightnessUp.AutoSize = true;
            this.labelMonitorBrightnessUp.Location = new System.Drawing.Point(16, 28);
            this.labelMonitorBrightnessUp.Name = "labelMonitorBrightnessUp";
            this.labelMonitorBrightnessUp.Size = new System.Drawing.Size(51, 15);
            this.labelMonitorBrightnessUp.TabIndex = 20;
            this.labelMonitorBrightnessUp.Text = "Monitor";
            //
            // textBoxMonitorBrightness
            //
            this.textBoxMonitorBrightness.Location = new System.Drawing.Point(514, 23);
            this.textBoxMonitorBrightness.Name = "textBoxMonitorBrightness";
            this.textBoxMonitorBrightness.Size = new System.Drawing.Size(60, 23);
            this.textBoxMonitorBrightness.TabIndex = 19;
            this.textBoxMonitorBrightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMonitorBrightness.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sliderTextBox_KeyDown);
            this.textBoxMonitorBrightness.Leave += new System.EventHandler(this.sliderTextBox_Leave);
            //
            // trackBarMonitorBrightness
            //
            this.trackBarMonitorBrightness.LargeChange = 1;
            this.trackBarMonitorBrightness.Location = new System.Drawing.Point(100, 16);
            this.trackBarMonitorBrightness.Maximum = 100;
            this.trackBarMonitorBrightness.Name = "trackBarMonitorBrightness";
            this.trackBarMonitorBrightness.Size = new System.Drawing.Size(400, 40);
            this.trackBarMonitorBrightness.TabIndex = 18;
            this.trackBarMonitorBrightness.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarMonitorBrightness.Value = 100;
            this.trackBarMonitorBrightness.ValueChanged += new System.EventHandler(this.trackBarMonitorBrightness_ValueChanged);
            //
            // labelMonitorBrightnessDown
            //
            this.labelMonitorBrightnessDown.AutoSize = true;
            this.labelMonitorBrightnessDown.Location = new System.Drawing.Point(16, 43);
            this.labelMonitorBrightnessDown.Name = "labelMonitorBrightnessDown";
            this.labelMonitorBrightnessDown.Size = new System.Drawing.Size(64, 15);
            this.labelMonitorBrightnessDown.TabIndex = 21;
            this.labelMonitorBrightnessDown.Text = "Brightness";
            //
            // buttonHide
            //
            this.buttonHide.Location = new System.Drawing.Point(664, 65);
            this.buttonHide.Name = "buttonHide";
            this.buttonHide.Size = new System.Drawing.Size(100, 28);
            this.buttonHide.TabIndex = 22;
            this.buttonHide.Text = "Hide";
            this.buttonHide.UseVisualStyleBackColor = true;
            this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
            //
            // labelMonitorContrastUp
            //
            this.labelMonitorContrastUp.AutoSize = true;
            this.labelMonitorContrastUp.Location = new System.Drawing.Point(16, 76);
            this.labelMonitorContrastUp.Name = "labelMonitorContrastUp";
            this.labelMonitorContrastUp.Size = new System.Drawing.Size(51, 15);
            this.labelMonitorContrastUp.TabIndex = 24;
            this.labelMonitorContrastUp.Text = "Monitor";
            //
            // labelMonitorContrastDown
            //
            this.labelMonitorContrastDown.AutoSize = true;
            this.labelMonitorContrastDown.Location = new System.Drawing.Point(16, 91);
            this.labelMonitorContrastDown.Name = "labelMonitorContrastDown";
            this.labelMonitorContrastDown.Size = new System.Drawing.Size(53, 15);
            this.labelMonitorContrastDown.TabIndex = 25;
            this.labelMonitorContrastDown.Text = "Contrast";
            //
            // trackBarMonitorContrast
            //
            this.trackBarMonitorContrast.LargeChange = 1;
            this.trackBarMonitorContrast.Location = new System.Drawing.Point(100, 64);
            this.trackBarMonitorContrast.Maximum = 100;
            this.trackBarMonitorContrast.Name = "trackBarMonitorContrast";
            this.trackBarMonitorContrast.Size = new System.Drawing.Size(400, 40);
            this.trackBarMonitorContrast.TabIndex = 26;
            this.trackBarMonitorContrast.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarMonitorContrast.Value = 100;
            this.trackBarMonitorContrast.ValueChanged += new System.EventHandler(this.trackBarMonitorContrast_ValueChanged);
            //
            // textBoxMonitorContrast
            //
            this.textBoxMonitorContrast.Location = new System.Drawing.Point(514, 71);
            this.textBoxMonitorContrast.Name = "textBoxMonitorContrast";
            this.textBoxMonitorContrast.Size = new System.Drawing.Size(60, 23);
            this.textBoxMonitorContrast.TabIndex = 27;
            this.textBoxMonitorContrast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxMonitorContrast.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sliderTextBox_KeyDown);
            this.textBoxMonitorContrast.Leave += new System.EventHandler(this.sliderTextBox_Leave);
            //
            // buttonForward
            //
            this.buttonForward.Location = new System.Drawing.Point(190, 27);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(66, 28);
            this.buttonForward.TabIndex = 29;
            this.buttonForward.Text = "Next";
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.Click += new System.EventHandler(this.buttonForward_Click);
            //
            // buttonAllMonitors
            //
            this.buttonAllMonitors.Location = new System.Drawing.Point(16, 65);
            this.buttonAllMonitors.Name = "buttonAllMonitors";
            this.buttonAllMonitors.Size = new System.Drawing.Size(166, 28);
            this.buttonAllMonitors.TabIndex = 31;
            this.buttonAllMonitors.Text = "All Monitors";
            this.buttonAllMonitors.UseVisualStyleBackColor = true;
            this.buttonAllMonitors.Click += new System.EventHandler(this.buttonAllMonitors_Click);
            //
            // checkBoxStartWithWindows
            //
            this.checkBoxStartWithWindows.AutoSize = true;
            this.checkBoxStartWithWindows.Location = new System.Drawing.Point(198, 69);
            this.checkBoxStartWithWindows.Name = "checkBoxStartWithWindows";
            this.checkBoxStartWithWindows.TabIndex = 32;
            this.checkBoxStartWithWindows.Text = "Start with";
            this.checkBoxStartWithWindows.UseVisualStyleBackColor = true;
            this.checkBoxStartWithWindows.CheckedChanged += new System.EventHandler(this.checkBoxStartWithWindows_CheckedChanged);
            //
            // checkBoxExContrast
            //
            this.checkBoxExContrast.AutoSize = true;
            this.checkBoxExContrast.Location = new System.Drawing.Point(596, 145);
            this.checkBoxExContrast.Name = "checkBoxExContrast";
            this.checkBoxExContrast.Size = new System.Drawing.Size(41, 19);
            this.checkBoxExContrast.TabIndex = 30;
            this.checkBoxExContrast.Text = "+++";
            this.checkBoxExContrast.UseVisualStyleBackColor = true;
            this.checkBoxExContrast.CheckedChanged += new System.EventHandler(this.checkBoxExContrast_CheckedChanged);
            //
            // notifyIcon
            //
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Gamma Manager";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            //
            // pictureBox1
            //
            this.pictureBox1.BackgroundImage = global::Gamma_Manager.Properties.Resources.TestMonitor;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(16, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 156);
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
            //
            // contextMenu
            //
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(61, 4);
            //
            // groupBoxColorAdjustment
            //
            this.groupBoxColorAdjustment.Controls.Add(this.labelGamma);
            this.groupBoxColorAdjustment.Controls.Add(this.labelBrightness);
            this.groupBoxColorAdjustment.Controls.Add(this.labelContrast);
            this.groupBoxColorAdjustment.Controls.Add(this.trackBarGamma);
            this.groupBoxColorAdjustment.Controls.Add(this.trackBarBrightness);
            this.groupBoxColorAdjustment.Controls.Add(this.trackBarContrast);
            this.groupBoxColorAdjustment.Controls.Add(this.textBoxGamma);
            this.groupBoxColorAdjustment.Controls.Add(this.textBoxBrightness);
            this.groupBoxColorAdjustment.Controls.Add(this.textBoxContrast);
            this.groupBoxColorAdjustment.Controls.Add(this.buttonAllColors);
            this.groupBoxColorAdjustment.Controls.Add(this.buttonRed);
            this.groupBoxColorAdjustment.Controls.Add(this.buttonGreen);
            this.groupBoxColorAdjustment.Controls.Add(this.buttonBlue);
            this.groupBoxColorAdjustment.Controls.Add(this.checkBoxExContrast);
            this.groupBoxColorAdjustment.Location = new System.Drawing.Point(16, 12);
            this.groupBoxColorAdjustment.Name = "groupBoxColorAdjustment";
            this.groupBoxColorAdjustment.Size = new System.Drawing.Size(780, 180);
            this.groupBoxColorAdjustment.TabIndex = 33;
            this.groupBoxColorAdjustment.TabStop = false;
            this.groupBoxColorAdjustment.Text = "Color adjustment";
            //
            // groupBoxDisplay
            //
            this.groupBoxDisplay.Controls.Add(this.comboBoxMonitors);
            this.groupBoxDisplay.Controls.Add(this.buttonForward);
            this.groupBoxDisplay.Controls.Add(this.comboBoxPresets);
            this.groupBoxDisplay.Controls.Add(this.buttonSave);
            this.groupBoxDisplay.Controls.Add(this.buttonDelete);
            this.groupBoxDisplay.Controls.Add(this.buttonAllMonitors);
            this.groupBoxDisplay.Controls.Add(this.checkBoxStartWithWindows);
            this.groupBoxDisplay.Controls.Add(this.buttonReset);
            this.groupBoxDisplay.Controls.Add(this.buttonHide);
            this.groupBoxDisplay.Location = new System.Drawing.Point(16, 204);
            this.groupBoxDisplay.Name = "groupBoxDisplay";
            this.groupBoxDisplay.Size = new System.Drawing.Size(780, 96);
            this.groupBoxDisplay.TabIndex = 34;
            this.groupBoxDisplay.TabStop = false;
            this.groupBoxDisplay.Text = "Display and presets";
            //
            // groupBoxHardware
            //
            this.groupBoxHardware.Controls.Add(this.labelMonitorBrightnessUp);
            this.groupBoxHardware.Controls.Add(this.labelMonitorBrightnessDown);
            this.groupBoxHardware.Controls.Add(this.trackBarMonitorBrightness);
            this.groupBoxHardware.Controls.Add(this.textBoxMonitorBrightness);
            this.groupBoxHardware.Controls.Add(this.labelMonitorContrastUp);
            this.groupBoxHardware.Controls.Add(this.labelMonitorContrastDown);
            this.groupBoxHardware.Controls.Add(this.trackBarMonitorContrast);
            this.groupBoxHardware.Controls.Add(this.textBoxMonitorContrast);
            this.groupBoxHardware.Location = new System.Drawing.Point(16, 312);
            this.groupBoxHardware.Name = "groupBoxHardware";
            this.groupBoxHardware.Size = new System.Drawing.Size(780, 128);
            this.groupBoxHardware.TabIndex = 35;
            this.groupBoxHardware.TabStop = false;
            this.groupBoxHardware.Text = "Monitor hardware";
            //
            // groupBoxPreview
            //
            this.groupBoxPreview.Controls.Add(this.pictureBox1);
            this.groupBoxPreview.Location = new System.Drawing.Point(812, 12);
            this.groupBoxPreview.Name = "groupBoxPreview";
            this.groupBoxPreview.Size = new System.Drawing.Size(232, 428);
            this.groupBoxPreview.TabIndex = 36;
            this.groupBoxPreview.TabStop = false;
            this.groupBoxPreview.Text = "Preview";
            //
            // Window
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 452);
            this.Controls.Add(this.groupBoxPreview);
            this.Controls.Add(this.groupBoxHardware);
            this.Controls.Add(this.groupBoxDisplay);
            this.Controls.Add(this.groupBoxColorAdjustment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Name = "Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Gamma Manager";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Window_Load);
            this.Resize += new System.EventHandler(this.Window_Resize);
            this.groupBoxColorAdjustment.ResumeLayout(false);
            this.groupBoxColorAdjustment.PerformLayout();
            this.groupBoxDisplay.ResumeLayout(false);
            this.groupBoxDisplay.PerformLayout();
            this.groupBoxHardware.ResumeLayout(false);
            this.groupBoxHardware.PerformLayout();
            this.groupBoxPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGamma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMonitorBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMonitorContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TrackBar trackBarGamma;
        private System.Windows.Forms.Button buttonRed;
        private System.Windows.Forms.Button buttonGreen;
        private System.Windows.Forms.Button buttonBlue;
        private System.Windows.Forms.Button buttonAllColors;
        private System.Windows.Forms.ComboBox comboBoxPresets;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ComboBox comboBoxMonitors;
        private System.Windows.Forms.TrackBar trackBarContrast;
        private System.Windows.Forms.TrackBar trackBarBrightness;
        private System.Windows.Forms.TextBox textBoxGamma;
        private System.Windows.Forms.TextBox textBoxContrast;
        private System.Windows.Forms.TextBox textBoxBrightness;
        private System.Windows.Forms.Label labelGamma;
        private System.Windows.Forms.Label labelContrast;
        private System.Windows.Forms.Label labelBrightness;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label labelMonitorBrightnessUp;
        private System.Windows.Forms.TextBox textBoxMonitorBrightness;
        private System.Windows.Forms.TrackBar trackBarMonitorBrightness;
        private System.Windows.Forms.Label labelMonitorBrightnessDown;
        private System.Windows.Forms.Button buttonHide;
        private System.Windows.Forms.Label labelMonitorContrastUp;
        private System.Windows.Forms.Label labelMonitorContrastDown;
        private System.Windows.Forms.TrackBar trackBarMonitorContrast;
        private System.Windows.Forms.TextBox textBoxMonitorContrast;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonForward;
        private System.Windows.Forms.Button buttonAllMonitors;
        private System.Windows.Forms.CheckBox checkBoxStartWithWindows;
        private System.Windows.Forms.CheckBox checkBoxExContrast;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.GroupBox groupBoxColorAdjustment;
        private System.Windows.Forms.GroupBox groupBoxDisplay;
        private System.Windows.Forms.GroupBox groupBoxHardware;
        private System.Windows.Forms.GroupBox groupBoxPreview;
    }
}
