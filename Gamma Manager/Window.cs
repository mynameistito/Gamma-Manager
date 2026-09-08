using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gamma_Manager
{
    public partial class Window : Form
    {
        private const string StartupRegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string StartupValueName = "Gamma Manager";
        private const int DwmwaUseImmersiveDarkMode = 20;
        private const int DwmwaUseImmersiveDarkModeLegacy = 19;
        private const int DwmwaBorderColor = 34;
        private const int DwmwaCaptionColor = 35;
        private const int DwmwaTextColor = 36;

        [DllImport("dwmapi.dll", ExactSpelling = true)]
        private static extern int DwmSetWindowAttribute(IntPtr windowHandle, int attribute, ref int value, int valueSize);

        System.Globalization.CultureInfo customCulture;
        IniFile iniFile;

        List<Display.DisplayInfo> displays = new List<Display.DisplayInfo>();
        int numDisplay = 0;
        Display.DisplayInfo currDisplay;

        List<ToolStripComboBox> toolMonitors = new List<ToolStripComboBox>();
        ToolStripComboBox toolMonitor;

        bool disableChangeFunc = false;

        bool allColors = true;
        bool redColor = false;
        bool greenColor = false;
        bool blueColor = false;
        bool allMonitors = false;

        private void clearColors()
        {
            buttonAllColors.Font = new Font(buttonAllColors.Font.Name, buttonAllColors.Font.Size, FontStyle.Regular);
            buttonRed.Font = new Font(buttonRed.Font.Name, buttonRed.Font.Size, FontStyle.Regular);
            buttonGreen.Font = new Font(buttonGreen.Font.Name, buttonGreen.Font.Size, FontStyle.Regular);
            buttonBlue.Font = new Font(buttonBlue.Font.Name, buttonBlue.Font.Size, FontStyle.Regular);

            allColors = false;
            redColor = false;
            greenColor = false;
            blueColor = false;
        }

        private void initPresets()
        {
            comboBoxPresets.Items.Clear();
            comboBoxPresets.Text = string.Empty;
            string[] presets = iniFile.GetSections();
            if (presets != null)
            {
                for (int i = 0; i < presets.Length; i++)
                {
                    if (iniFile.Read("monitor", presets[i]).Equals(currDisplay.displayName))
                    {
                        comboBoxPresets.Items.Add(presets[i]);
                    }
                }
            }
        }

        private void initTrayMenu()
        {
            contextMenu.Items.Clear();
            toolMonitors.Clear();

            ToolStripMenuItem toolSetting = new ToolStripMenuItem("Settings", null, toolSettings_Click);
            contextMenu.Items.Add(toolSetting);

            ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
            contextMenu.Items.Add(toolStripSeparator1);

            for (int i = 0; i < displays.Count; i++)
            {
                toolMonitor = new ToolStripComboBox(displays[i].displayName);
                toolMonitor.DropDownStyle = ComboBoxStyle.DropDownList;

                toolMonitor.Items.Add(displays[i].displayName + ":");
                toolMonitor.Text = displays[i].displayName + ":";

                toolMonitor.SelectedIndexChanged += new EventHandler(comboBoxToolMonitor_IndexChanged);

                string[] presets = iniFile.GetSections();
                if (presets != null)
                {
                    for (int j = 0; j < presets.Length; j++)
                    {
                        if (iniFile.Read("monitor", presets[j]).Equals(displays[i].displayName))
                        {
                            //preset.name = presets[j].Substring(presets[j].IndexOf(")") + 1);
                            toolMonitor.Items.Add(presets[j]);
                        }
                    }
                }
                toolMonitors.Add(toolMonitor);
                contextMenu.Items.Add(toolMonitor);
            }
            ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
            contextMenu.Items.Add(toolStripSeparator2);
            ToolStripMenuItem toolExit = new ToolStripMenuItem("Exit", null, toolExit_Click);
            contextMenu.Items.Add(toolExit);
            ApplySystemTheme();
        }

        private void fillInfo(Display.DisplayInfo currDisplay)
        {
            disableChangeFunc = true;

            textBoxGamma.Text = ((currDisplay.rGamma + currDisplay.gGamma + currDisplay.bGamma) / 3f).ToString("0.00");
            textBoxContrast.Text = ((currDisplay.rContrast + currDisplay.gContrast + currDisplay.bContrast) / 3f).ToString("0.00");
            textBoxBrightness.Text = ((currDisplay.rBright + currDisplay.gBright + currDisplay.bBright) / 3f).ToString("0.00");

            trackBarGamma.Value = (int)(((currDisplay.rGamma + currDisplay.gGamma + currDisplay.bGamma) / 3f) * 100f);
            trackBarContrast.Value = (int)(((currDisplay.rContrast + currDisplay.gContrast + currDisplay.bContrast) / 3f) * 100f);
            trackBarBrightness.Value = (int)(((currDisplay.rBright + currDisplay.gBright + currDisplay.bBright) / 3f) * 100f);

            if (currDisplay.isExternal)
            {
                labelMonitorContrastUp.Visible = true;
                labelMonitorContrastDown.Visible = true;
                trackBarMonitorContrast.Visible = true;
                textBoxMonitorContrast.Visible = true;

                currDisplay.monitorBrightness = ExternalMonitor.GetBrightness(currDisplay.PhysicalHandle);
                textBoxMonitorBrightness.Text = currDisplay.monitorBrightness.ToString();
                trackBarMonitorBrightness.Value = currDisplay.monitorBrightness;

                currDisplay.monitorContrast = ExternalMonitor.GetContrast(currDisplay.PhysicalHandle);
                textBoxMonitorContrast.Text = currDisplay.monitorContrast.ToString();
                trackBarMonitorContrast.Value = currDisplay.monitorContrast;
            }
            else
            {
                labelMonitorContrastUp.Visible = false;
                labelMonitorContrastDown.Visible = false;
                trackBarMonitorContrast.Visible = false;
                textBoxMonitorContrast.Visible = false;

                currDisplay.monitorBrightness = InternalMonitor.GetBrightness();
                textBoxMonitorBrightness.Text = currDisplay.monitorBrightness.ToString();
                trackBarMonitorBrightness.Value = currDisplay.monitorBrightness;
            }
            disableChangeFunc = false;
        }

        private void fillAllMonitorInfo()
        {
            disableChangeFunc = true;

            textBoxGamma.Text = GetSelectedColorValue(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma).ToString("0.00");
            textBoxContrast.Text = GetSelectedColorValue(currDisplay.rContrast, currDisplay.gContrast, currDisplay.bContrast).ToString("0.00");
            textBoxBrightness.Text = GetSelectedColorValue(currDisplay.rBright, currDisplay.gBright, currDisplay.bBright).ToString("0.00");

            trackBarGamma.Value = ClampTrackBarValue((int)(GetSelectedColorValue(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma) * 100f), trackBarGamma);
            trackBarContrast.Value = ClampTrackBarValue((int)(GetSelectedColorValue(currDisplay.rContrast, currDisplay.gContrast, currDisplay.bContrast) * 100f), trackBarContrast);
            trackBarBrightness.Value = ClampTrackBarValue((int)(GetSelectedColorValue(currDisplay.rBright, currDisplay.gBright, currDisplay.bBright) * 100f), trackBarBrightness);

            bool hasExternalMonitor = false;
            foreach (Display.DisplayInfo display in displays)
            {
                if (display.isExternal)
                {
                    hasExternalMonitor = true;
                    break;
                }
            }

            labelMonitorContrastUp.Visible = hasExternalMonitor;
            labelMonitorContrastDown.Visible = hasExternalMonitor;
            trackBarMonitorContrast.Visible = hasExternalMonitor;
            textBoxMonitorContrast.Visible = hasExternalMonitor;

            textBoxMonitorBrightness.Text = currDisplay.monitorBrightness.ToString();
            trackBarMonitorBrightness.Value = ClampTrackBarValue(currDisplay.monitorBrightness, trackBarMonitorBrightness);

            Display.DisplayInfo contrastDisplay = currDisplay;
            if (!contrastDisplay.isExternal)
            {
                foreach (Display.DisplayInfo display in displays)
                {
                    if (display.isExternal)
                    {
                        contrastDisplay = display;
                        break;
                    }
                }
            }

            if (hasExternalMonitor)
            {
                textBoxMonitorContrast.Text = contrastDisplay.monitorContrast.ToString();
                trackBarMonitorContrast.Value = ClampTrackBarValue(contrastDisplay.monitorContrast, trackBarMonitorContrast);
            }

            disableChangeFunc = false;
        }

        private static int ClampTrackBarValue(int value, TrackBar trackBar)
        {
            return Math.Max(trackBar.Minimum, Math.Min(trackBar.Maximum, value));
        }

        private float GetSelectedColorValue(float redValue, float greenValue, float blueValue)
        {
            if (allColors)
            {
                return (redValue + greenValue + blueValue) / 3f;
            }

            if (greenColor)
            {
                return greenValue;
            }

            if (blueColor)
            {
                return blueValue;
            }

            return redValue;
        }

        private void SetAllMonitorsMode(bool enabled)
        {
            allMonitors = enabled;
            buttonAllMonitors.Text = allMonitors ? "Single Monitor" : "All Monitors";
            comboBoxMonitors.Enabled = !allMonitors;
            buttonForward.Enabled = !allMonitors;
            comboBoxPresets.Enabled = !allMonitors;
        }

        private static void ApplyGammaRamp(Display.DisplayInfo display)
        {
            Gamma.SetGammaRamp(display.displayLink,
                Gamma.CreateGammaRamp(display.rGamma, display.gGamma, display.bGamma, display.rContrast,
                display.gContrast, display.bContrast, display.rBright, display.gBright, display.bBright));
        }

        private static bool IsSystemDarkModeEnabled()
        {
            using (RegistryKey personalizeKey = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", false))
            {
                if (personalizeKey == null)
                {
                    return false;
                }

                object lightThemeValue = personalizeKey.GetValue("AppsUseLightTheme");
                return lightThemeValue != null && Convert.ToInt32(lightThemeValue) == 0;
            }
        }

        private static void ApplyControlTheme(Control control, bool darkMode)
        {
            Color backgroundColor = darkMode ? Color.FromArgb(30, 35, 42) : SystemColors.Control;
            Color controlColor = darkMode ? Color.FromArgb(36, 41, 47) : SystemColors.Window;
            Color foregroundColor = darkMode ? Color.FromArgb(230, 237, 243) : SystemColors.ControlText;
            Color borderColor = darkMode ? Color.FromArgb(82, 90, 101) : SystemColors.ControlDark;

            control.ForeColor = foregroundColor;
            if (control is Form || control is TrackBar || control is PictureBox)
            {
                control.BackColor = backgroundColor;
            }
            else if (control is TextBoxBase || control is ComboBox)
            {
                control.BackColor = controlColor;
                if (control is TextBoxBase textBox)
                {
                    textBox.BorderStyle = darkMode ? BorderStyle.FixedSingle : BorderStyle.Fixed3D;
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.FlatStyle = darkMode ? FlatStyle.Flat : FlatStyle.Standard;
                }
            }
            else if (control is Button button)
            {
                button.BackColor = darkMode ? Color.FromArgb(43, 49, 58) : SystemColors.Control;
                button.UseVisualStyleBackColor = !darkMode;
                button.FlatStyle = darkMode ? FlatStyle.Flat : FlatStyle.Standard;
                button.FlatAppearance.BorderColor = borderColor;
                button.FlatAppearance.BorderSize = darkMode ? 1 : 0;
                button.FlatAppearance.MouseOverBackColor = darkMode ? Color.FromArgb(56, 65, 76) : SystemColors.Control;
                button.FlatAppearance.MouseDownBackColor = darkMode ? Color.FromArgb(65, 74, 86) : SystemColors.Control;
            }
            else if (control is CheckBox checkBox)
            {
                checkBox.BackColor = backgroundColor;
            }
            else if (control is Label label)
            {
                label.BackColor = Color.Transparent;
            }

            foreach (Control childControl in control.Controls)
            {
                ApplyControlTheme(childControl, darkMode);
            }
        }

        private void ApplySystemTheme()
        {
            bool darkMode = IsSystemDarkModeEnabled();
            ApplyControlTheme(this, darkMode);

            Color menuBackgroundColor = darkMode ? Color.FromArgb(36, 41, 47) : SystemColors.Menu;
            Color menuForegroundColor = darkMode ? Color.FromArgb(230, 237, 243) : SystemColors.MenuText;
            contextMenu.BackColor = menuBackgroundColor;
            contextMenu.ForeColor = menuForegroundColor;
            foreach (ToolStripItem item in contextMenu.Items)
            {
                item.BackColor = menuBackgroundColor;
                item.ForeColor = menuForegroundColor;

                if (item is ToolStripComboBox toolStripComboBox)
                {
                    toolStripComboBox.ComboBox.BackColor = menuBackgroundColor;
                    toolStripComboBox.ComboBox.ForeColor = menuForegroundColor;
                }
            }

            ApplyWindowFrameTheme(darkMode);
        }

        private void ApplyWindowFrameTheme(bool darkMode)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            int darkModeValue = darkMode ? 1 : 0;
            int result = DwmSetWindowAttribute(Handle, DwmwaUseImmersiveDarkMode, ref darkModeValue, sizeof(int));
            if (result != 0)
            {
                DwmSetWindowAttribute(Handle, DwmwaUseImmersiveDarkModeLegacy, ref darkModeValue, sizeof(int));
            }

            Color captionColor = darkMode ? Color.FromArgb(30, 35, 42) : Color.FromArgb(246, 248, 250);
            Color captionTextColor = darkMode ? Color.FromArgb(230, 237, 243) : Color.FromArgb(31, 35, 40);
            Color borderColor = darkMode ? Color.FromArgb(82, 90, 101) : Color.FromArgb(208, 215, 222);
            int captionColorValue = ColorTranslator.ToWin32(captionColor);
            int captionTextColorValue = ColorTranslator.ToWin32(captionTextColor);
            int borderColorValue = ColorTranslator.ToWin32(borderColor);

            DwmSetWindowAttribute(Handle, DwmwaCaptionColor, ref captionColorValue, sizeof(int));
            DwmSetWindowAttribute(Handle, DwmwaTextColor, ref captionTextColorValue, sizeof(int));
            DwmSetWindowAttribute(Handle, DwmwaBorderColor, ref borderColorValue, sizeof(int));
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }

            try
            {
                BeginInvoke(new MethodInvoker(ApplySystemTheme));
            }
            catch (InvalidOperationException)
            {
                // The form can close between the handle check and BeginInvoke.
            }
        }

        private void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
            SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
        }

        private static bool IsStartupEnabled()
        {
            using (RegistryKey runKey = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, false))
            {
                if (runKey == null)
                {
                    return false;
                }

                object startupValue = runKey.GetValue(StartupValueName);
                string startupCommand = startupValue as string;
                if (startupCommand == null)
                {
                    return false;
                }

                string executablePath = Application.ExecutablePath;
                return string.Equals(startupCommand, executablePath, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(startupCommand, "\"" + executablePath + "\"", StringComparison.OrdinalIgnoreCase);
            }
        }

        private static void SetStartupEnabled(bool enabled)
        {
            RegistryKey runKey = Registry.CurrentUser.CreateSubKey(StartupRegistryPath);
            if (runKey == null)
            {
                throw new InvalidOperationException("Unable to access the Windows startup registry key.");
            }

            using (runKey)
            {
                if (enabled)
                {
                    runKey.SetValue(StartupValueName, "\"" + Application.ExecutablePath + "\"");
                }
                else
                {
                    runKey.DeleteValue(StartupValueName, false);
                }
            }
        }

        private void checkBoxStartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetStartupEnabled(checkBoxStartWithWindows.Checked);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "Could not update Windows startup settings.\n\n" + exception.Message,
                    "Gamma Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);

                checkBoxStartWithWindows.CheckedChanged -= checkBoxStartWithWindows_CheckedChanged;
                checkBoxStartWithWindows.Checked = !checkBoxStartWithWindows.Checked;
                checkBoxStartWithWindows.CheckedChanged += checkBoxStartWithWindows_CheckedChanged;
            }
        }

        private void Window_Load(object sender, EventArgs e)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Size.Width;
            int windowWidth = Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Size.Height;
            int windowHeight = Height;
            int tmp = Screen.PrimaryScreen.Bounds.Height;
            int TaskBarHeight = tmp - Screen.PrimaryScreen.WorkingArea.Height;

            //dpi
            /*int PSH = SystemParameters.PrimaryScreenHeight;
            int PSBH = Screen.PrimaryScreen.Bounds.Height;
            double ratio = PSH / PSBH;
            int TaskBarHeight = PSBH - Screen.PrimaryScreen.WorkingArea.Height;
            TaskBarHeight *= ratio;*/

            Location = new Point(screenWidth - windowWidth, screenHeight - (windowHeight + TaskBarHeight));
            ApplySystemTheme();
        }

        public Window()
        {
            InitializeComponent();
            checkBoxStartWithWindows.Checked = IsStartupEnabled();
            customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ",";

            string settingsDirectory = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Gamma Manager");
            System.IO.Directory.CreateDirectory(settingsDirectory);
            string settingsPath = System.IO.Path.Combine(settingsDirectory, "GammaManager.ini");
            string legacySettingsPath = System.IO.Path.Combine(Application.StartupPath, "GammaManager.ini");
            if (!System.IO.File.Exists(settingsPath) && System.IO.File.Exists(legacySettingsPath))
            {
                System.IO.File.Copy(legacySettingsPath, settingsPath);
            }

            iniFile = new IniFile(settingsPath);

            buttonAllColors.Font = new Font(buttonAllColors.Font.Name, buttonAllColors.Font.Size, FontStyle.Bold);


            displays = Display.QueryDisplayDevices();
            displays.Reverse();
            for (int i = 0; i < displays.Count; i++)
            {
                displays[i].numDisplay = i;
                comboBoxMonitors.Items.Add(i+1+") "+ displays[i].displayName);
            }
            currDisplay = displays[numDisplay];
            comboBoxMonitors.SelectedIndex = numDisplay;

            fillInfo(currDisplay);

            initPresets();

            initTrayMenu();
            notifyIcon.ContextMenuStrip = contextMenu;
            ApplySystemTheme();
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            FormClosed += Window_FormClosed;
        }

        private void trackBarGamma_ValueChanged(object sender, EventArgs e)
        {
            comboBoxPresets.Text = string.Empty;

            if (!disableChangeFunc)
            {
                textBoxGamma.Text = ((float)trackBarGamma.Value / 100f).ToString("0.00");

                if (allMonitors)
                {
                    foreach (Display.DisplayInfo display in displays)
                    {
                        if (allColors)
                        {
                            display.rGamma = (float)trackBarGamma.Value / 100f;
                            display.gGamma = (float)trackBarGamma.Value / 100f;
                            display.bGamma = (float)trackBarGamma.Value / 100f;
                        }
                        else if (redColor)
                        {
                            display.rGamma = (float)trackBarGamma.Value / 100f;
                        }
                        else if (greenColor)
                        {
                            display.gGamma = (float)trackBarGamma.Value / 100f;
                        }
                        else if (blueColor)
                        {
                            display.bGamma = (float)trackBarGamma.Value / 100f;
                        }

                        ApplyGammaRamp(display);
                    }

                    return;
                }

                if (allColors)
                {
                    currDisplay.rGamma = (float)trackBarGamma.Value / 100f;
                    currDisplay.gGamma = (float)trackBarGamma.Value / 100f;
                    currDisplay.bGamma = (float)trackBarGamma.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (redColor)
                {
                    currDisplay.rGamma = (float)trackBarGamma.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (greenColor)
                {
                    currDisplay.gGamma = (float)trackBarGamma.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (blueColor)
                {
                    currDisplay.bGamma = (float)trackBarGamma.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                }

            EndColors:
                return;

            }
        }
            

        private void trackBarContrast_ValueChanged(object sender, EventArgs e)
        {
            comboBoxPresets.Text = string.Empty;

            if (!disableChangeFunc)
            {
                textBoxContrast.Text = ((float)trackBarContrast.Value / 100f).ToString("0.00");

                if (allMonitors)
                {
                    foreach (Display.DisplayInfo display in displays)
                    {
                        if (allColors)
                        {
                            display.rContrast = (float)trackBarContrast.Value / 100f;
                            display.gContrast = (float)trackBarContrast.Value / 100f;
                            display.bContrast = (float)trackBarContrast.Value / 100f;
                        }
                        else if (redColor)
                        {
                            display.rContrast = (float)trackBarContrast.Value / 100f;
                        }
                        else if (greenColor)
                        {
                            display.gContrast = (float)trackBarContrast.Value / 100f;
                        }
                        else if (blueColor)
                        {
                            display.bContrast = (float)trackBarContrast.Value / 100f;
                        }

                        ApplyGammaRamp(display);
                    }

                    return;
                }

                if (allColors)
                {
                    currDisplay.rContrast = (float)trackBarContrast.Value / 100f;
                    currDisplay.gContrast = (float)trackBarContrast.Value / 100f;
                    currDisplay.bContrast = (float)trackBarContrast.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (redColor)
                {
                    currDisplay.rContrast = (float)trackBarContrast.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (greenColor)
                {
                    currDisplay.gContrast = (float)trackBarContrast.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (blueColor)
                {
                    currDisplay.bContrast = (float)trackBarContrast.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                }

            EndColors:
                return;
            }
        }

        private void trackBarBrightness_ValueChanged(object sender, EventArgs e)
        {
            comboBoxPresets.Text = string.Empty;

            if (!disableChangeFunc)
            {
                textBoxBrightness.Text = ((float)trackBarBrightness.Value / 100f).ToString("0.00");

                if (allMonitors)
                {
                    foreach (Display.DisplayInfo display in displays)
                    {
                        if (allColors)
                        {
                            display.rBright = (float)trackBarBrightness.Value / 100f;
                            display.gBright = (float)trackBarBrightness.Value / 100f;
                            display.bBright = (float)trackBarBrightness.Value / 100f;
                        }
                        else if (redColor)
                        {
                            display.rBright = (float)trackBarBrightness.Value / 100f;
                        }
                        else if (greenColor)
                        {
                            display.gBright = (float)trackBarBrightness.Value / 100f;
                        }
                        else if (blueColor)
                        {
                            display.bBright = (float)trackBarBrightness.Value / 100f;
                        }

                        ApplyGammaRamp(display);
                    }

                    return;
                }

                if (allColors)
                {
                    currDisplay.rBright = (float)trackBarBrightness.Value / 100f;
                    currDisplay.gBright = (float)trackBarBrightness.Value / 100f;
                    currDisplay.bBright = (float)trackBarBrightness.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (redColor)
                {
                    currDisplay.rBright = (float)trackBarBrightness.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (greenColor)
                {
                    currDisplay.gBright = (float)trackBarBrightness.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                    goto EndColors;
                }

                if (blueColor)
                {
                    currDisplay.bBright = (float)trackBarBrightness.Value / 100f;
                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma, currDisplay.rContrast,
                        currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright, currDisplay.bBright));
                }

            EndColors:
                return;
            }
        }

        private void trackBarMonitorBrightness_ValueChanged(object sender, EventArgs e)
        {
            comboBoxPresets.Text = string.Empty;

            if (!disableChangeFunc)
            {
                textBoxMonitorBrightness.Text = trackBarMonitorBrightness.Value.ToString();

                if (allMonitors)
                {
                    foreach (Display.DisplayInfo display in displays)
                    {
                        display.monitorBrightness = trackBarMonitorBrightness.Value;
                        if (display.isExternal)
                        {
                            ExternalMonitor.SetBrightness(display.PhysicalHandle, (uint)trackBarMonitorBrightness.Value);
                        }
                        else
                        {
                            InternalMonitor.SetBrightness((byte)trackBarMonitorBrightness.Value);
                        }
                    }

                    return;
                }

                currDisplay.monitorBrightness = trackBarMonitorBrightness.Value;

                if (currDisplay.isExternal)
                {
                    ExternalMonitor.SetBrightness(currDisplay.PhysicalHandle, (uint)trackBarMonitorBrightness.Value);
                }
                else
                {
                    InternalMonitor.SetBrightness((byte)trackBarMonitorBrightness.Value);
                }
            }
        }

        private void trackBarMonitorContrast_ValueChanged(object sender, EventArgs e)
        {
            comboBoxPresets.Text = string.Empty;

            if (!disableChangeFunc)
            {
                textBoxMonitorContrast.Text = trackBarMonitorContrast.Value.ToString();

                if (allMonitors)
                {
                    foreach (Display.DisplayInfo display in displays)
                    {
                        if (display.isExternal)
                        {
                            display.monitorContrast = trackBarMonitorContrast.Value;
                            ExternalMonitor.SetContrast(display.PhysicalHandle, (uint)trackBarMonitorContrast.Value);
                        }
                    }

                    return;
                }

                currDisplay.monitorContrast = trackBarMonitorContrast.Value;

                ExternalMonitor.SetContrast(currDisplay.PhysicalHandle, (uint)trackBarMonitorContrast.Value);
            }
        }

        private void sliderTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            TextBox textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }

            ApplySliderTextBoxValue(textBox);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void sliderTextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                ApplySliderTextBoxValue(textBox);
            }
        }

        private void ApplySliderTextBoxValue(TextBox textBox)
        {
            if (textBox == textBoxGamma)
            {
                ApplyDecimalSliderTextBoxValue(textBox, trackBarGamma);
                return;
            }

            if (textBox == textBoxContrast)
            {
                ApplyDecimalSliderTextBoxValue(textBox, trackBarContrast);
                return;
            }

            if (textBox == textBoxBrightness)
            {
                ApplyDecimalSliderTextBoxValue(textBox, trackBarBrightness);
                return;
            }

            if (textBox == textBoxMonitorBrightness)
            {
                ApplyIntegerSliderTextBoxValue(textBox, trackBarMonitorBrightness);
                return;
            }

            if (textBox == textBoxMonitorContrast)
            {
                ApplyIntegerSliderTextBoxValue(textBox, trackBarMonitorContrast);
            }
        }

        private void ApplyDecimalSliderTextBoxValue(TextBox textBox, TrackBar trackBar)
        {
            float parsedValue;
            if (!TryParseFloat(textBox.Text, out parsedValue) || float.IsNaN(parsedValue) || float.IsInfinity(parsedValue))
            {
                textBox.Text = (trackBar.Value / 100f).ToString("0.00");
                return;
            }

            float scaledValue = parsedValue * 100f;
            int sliderValue;
            if (scaledValue <= trackBar.Minimum)
            {
                sliderValue = trackBar.Minimum;
            }
            else if (scaledValue >= trackBar.Maximum)
            {
                sliderValue = trackBar.Maximum;
            }
            else
            {
                sliderValue = (int)Math.Round(scaledValue, MidpointRounding.AwayFromZero);
            }

            trackBar.Value = sliderValue;
            textBox.Text = (trackBar.Value / 100f).ToString("0.00");
        }

        private void ApplyIntegerSliderTextBoxValue(TextBox textBox, TrackBar trackBar)
        {
            int parsedValue;
            if (!int.TryParse(textBox.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsedValue) &&
                !int.TryParse(textBox.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedValue))
            {
                textBox.Text = trackBar.Value.ToString();
                return;
            }

            trackBar.Value = Math.Max(trackBar.Minimum, Math.Min(trackBar.Maximum, parsedValue));
            textBox.Text = trackBar.Value.ToString();
        }

        private static bool TryParseFloat(string text, out float value)
        {
            return float.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value) ||
                float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        private void buttonAllColors_Click(object sender, EventArgs e)
        {
            disableChangeFunc = true;
            clearColors();
            allColors = true;

            textBoxGamma.Text = ((currDisplay.rGamma + currDisplay.gGamma + currDisplay.bGamma) / 3f).ToString("0.00");
            textBoxContrast.Text = ((currDisplay.rContrast + currDisplay.gContrast + currDisplay.bContrast) / 3f).ToString("0.00");
            textBoxBrightness.Text = ((currDisplay.rBright + currDisplay.gBright + currDisplay.bBright) / 3f).ToString("0.00");

            trackBarGamma.Value = (int)(((currDisplay.rGamma + currDisplay.gGamma + currDisplay.bGamma) / 3f) * 100f);
            trackBarContrast.Value = (int)(((currDisplay.rContrast + currDisplay.gContrast + currDisplay.bContrast) / 3f) * 100f);
            trackBarBrightness.Value = (int)(((currDisplay.rBright + currDisplay.gBright + currDisplay.bBright) / 3f) * 100f);

            buttonAllColors.Font = new Font(buttonAllColors.Font.Name, buttonAllColors.Font.Size, FontStyle.Bold);
            disableChangeFunc = false;
        }

        private void buttonAllMonitors_Click(object sender, EventArgs e)
        {
            comboBoxPresets.Text = string.Empty;
            SetAllMonitorsMode(!allMonitors);

            if (allMonitors)
            {
                fillAllMonitorInfo();
            }
            else
            {
                fillInfo(currDisplay);
            }
        }

        private void buttonRed_Click(object sender, EventArgs e)
        {
            disableChangeFunc = true;
            clearColors();
            redColor = true;

            textBoxGamma.Text = currDisplay.rGamma.ToString("0.00");
            textBoxContrast.Text = currDisplay.rContrast.ToString("0.00");
            textBoxBrightness.Text = currDisplay.rBright.ToString("0.00");

            trackBarGamma.Value = (int)(currDisplay.rGamma * 100f);
            trackBarContrast.Value = (int)(currDisplay.rContrast * 100f);
            trackBarBrightness.Value = (int)(currDisplay.rBright * 100f);

            buttonRed.Font = new Font(buttonRed.Font.Name, buttonRed.Font.Size, FontStyle.Bold);
            disableChangeFunc = false;
        }

        private void buttonGreen_Click(object sender, EventArgs e)
        {
            disableChangeFunc = true;
            clearColors();
            greenColor = true;

            textBoxGamma.Text = currDisplay.gGamma.ToString("0.00");
            textBoxContrast.Text = currDisplay.gContrast.ToString("0.00");
            textBoxBrightness.Text = currDisplay.gBright.ToString("0.00");

            trackBarGamma.Value = (int)(currDisplay.gGamma * 100f);
            trackBarContrast.Value = (int)(currDisplay.gContrast * 100f);
            trackBarBrightness.Value = (int)(currDisplay.gBright * 100f);

            buttonGreen.Font = new Font(buttonGreen.Font.Name, buttonGreen.Font.Size, FontStyle.Bold);
            disableChangeFunc = false;
        }

        private void buttonBlue_Click(object sender, EventArgs e)
        {
            disableChangeFunc = true;
            clearColors();
            blueColor = true;

            textBoxGamma.Text = currDisplay.bGamma.ToString("0.00");
            textBoxContrast.Text = currDisplay.bContrast.ToString("0.00");
            textBoxBrightness.Text = currDisplay.bBright.ToString("0.00");

            trackBarGamma.Value = (int)(currDisplay.bGamma * 100f);
            trackBarContrast.Value = (int)(currDisplay.bContrast * 100f);
            trackBarBrightness.Value = (int)(currDisplay.bBright * 100f);

            buttonBlue.Font = new Font(buttonBlue.Font.Name, buttonBlue.Font.Size, FontStyle.Bold);
            disableChangeFunc = false;
        }

        private void checkBoxExContrast_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxExContrast.Checked)
            {
                trackBarContrast.Maximum = 10000;
            } else
            {
                trackBarContrast.Maximum = 300;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string tmp = comboBoxPresets.Text;
            iniFile.Write("monitor", currDisplay.displayName, currDisplay.displayName+": "+ comboBoxPresets.Text);
            iniFile.Write("rGamma", currDisplay.rGamma.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);
            iniFile.Write("gGamma", currDisplay.gGamma.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);
            iniFile.Write("bGamma", currDisplay.bGamma.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);
            iniFile.Write("rContrast", currDisplay.rContrast.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);
            iniFile.Write("gContrast", currDisplay.gContrast.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);
            iniFile.Write("bContrast", currDisplay.bContrast.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);
            iniFile.Write("rBright", currDisplay.rBright.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);
            iniFile.Write("gBright", currDisplay.gBright.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);
            iniFile.Write("bBright", currDisplay.bBright.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);
            iniFile.Write("monitorBrightness", currDisplay.monitorBrightness.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);
            iniFile.Write("monitorContrast", currDisplay.monitorContrast.ToString(customCulture), currDisplay.displayName + ": " + comboBoxPresets.Text);

            initPresets();
            comboBoxPresets.Text = currDisplay.displayName + ": " + tmp;

            initTrayMenu();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            iniFile.DeleteSection(comboBoxPresets.Text);

            initPresets();
            initTrayMenu();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            comboBoxPresets.Text = string.Empty;

            if (allMonitors)
            {
                foreach (Display.DisplayInfo display in displays)
                {
                    display.rGamma = 1;
                    display.gGamma = 1;
                    display.bGamma = 1;
                    display.rContrast = 1;
                    display.gContrast = 1;
                    display.bContrast = 1;
                    display.rBright = 0;
                    display.gBright = 0;
                    display.bBright = 0;
                    display.monitorBrightness = 100;

                    if (display.isExternal)
                    {
                        display.monitorContrast = 50;
                        ExternalMonitor.SetBrightness(display.PhysicalHandle, 100);
                        ExternalMonitor.SetContrast(display.PhysicalHandle, 50);
                    }
                    else
                    {
                        InternalMonitor.SetBrightness(100);
                    }

                    ApplyGammaRamp(display);
                }

                fillAllMonitorInfo();
                initPresets();
                initTrayMenu();
                return;
            }

            buttonAllColors.PerformClick();

            trackBarGamma.Value = 100;
            trackBarContrast.Value = 100;
            trackBarBrightness.Value = 0;

            currDisplay.rGamma = 1;
            currDisplay.gGamma = 1;
            currDisplay.bGamma = 1;
            currDisplay.rContrast = 1;
            currDisplay.gContrast = 1;
            currDisplay.bContrast = 1;
            currDisplay.rBright = 0;
            currDisplay.gBright = 0;
            currDisplay.bBright = 0;

            
            if (currDisplay.isExternal)
            {

                trackBarMonitorBrightness.Value = 100;

                trackBarMonitorContrast.Value = 50;
            } else
            {
                trackBarMonitorBrightness.Value = 100;
            }
            

            Gamma.SetGammaRamp(displays[numDisplay].displayLink, Gamma.CreateGammaRamp(1, 1, 1, 1, 1, 1, 0, 0, 0));

            initPresets();
            initTrayMenu();
        }

        private void buttonHide_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void comboBoxMonitors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (allMonitors)
            {
                return;
            }

            string num = comboBoxMonitors.SelectedItem.ToString();

            num = num.Substring(0, num.IndexOf(")"));
            numDisplay = Int32.Parse(num)-1;

            currDisplay = displays[numDisplay];
            fillInfo(currDisplay);
            
            initPresets();
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            if (allMonitors)
            {
                return;
            }

            if (numDisplay + 1 <= displays.Count-1)
            {
                comboBoxMonitors.SelectedIndex = numDisplay + 1;
            } else
            {
                comboBoxMonitors.SelectedIndex = 0;
            }
        }

        private void comboBoxPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!disableChangeFunc && !allMonitors)
            {
                string test = comboBoxPresets.Text;
                currDisplay.rGamma = float.Parse(iniFile.Read("rGamma", comboBoxPresets.Text), customCulture);
                currDisplay.gGamma = float.Parse(iniFile.Read("gGamma", comboBoxPresets.Text), customCulture);
                currDisplay.bGamma = float.Parse(iniFile.Read("bGamma", comboBoxPresets.Text), customCulture);
                currDisplay.rContrast = float.Parse(iniFile.Read("rContrast", comboBoxPresets.Text), customCulture);
                currDisplay.gContrast = float.Parse(iniFile.Read("gContrast", comboBoxPresets.Text), customCulture);
                currDisplay.bContrast = float.Parse(iniFile.Read("bContrast", comboBoxPresets.Text), customCulture);
                currDisplay.rBright = float.Parse(iniFile.Read("rBright", comboBoxPresets.Text), customCulture);
                currDisplay.gBright = float.Parse(iniFile.Read("gBright", comboBoxPresets.Text), customCulture);
                currDisplay.bBright = float.Parse(iniFile.Read("bBright", comboBoxPresets.Text), customCulture);
                currDisplay.monitorBrightness = int.Parse(iniFile.Read("monitorBrightness", comboBoxPresets.Text));
                currDisplay.monitorContrast = int.Parse(iniFile.Read("monitorContrast", comboBoxPresets.Text));

                fillInfo(currDisplay);
                clearColors();
                buttonAllColors.PerformClick();
                initTrayMenu();

                Gamma.SetGammaRamp(currDisplay.displayLink,
                    Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma,
                    currDisplay.rContrast, currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright,
                    currDisplay.bBright));

                if (currDisplay.isExternal)
                {
                    trackBarMonitorBrightness.Value = currDisplay.monitorBrightness;
                    trackBarMonitorContrast.Value = currDisplay.monitorContrast;
                }
                else
                {
                    trackBarMonitorBrightness.Value = currDisplay.monitorBrightness;
                }
            }
        }

        //tray
        private void Window_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void toolSettings_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void toolExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBoxToolMonitor_IndexChanged(object sender, EventArgs e)
        {
            if (allMonitors)
            {
                SetAllMonitorsMode(false);
            }

            if (!disableChangeFunc)
            {
                string monitor = sender.ToString().Substring(0, sender.ToString().IndexOf(":"));
                /*string comName = toolMonitor.Items[i].ToString().Substring
                        (0, toolMonitor.Items[i].ToString().IndexOf(":"));*/

                int tmp = 0;

                disableChangeFunc = true;

                for (int i = 0; i < displays.Count; i++)
                {
                    if (monitor.Equals(displays[i].displayName))
                    {
                        tmp = i;
                    }
                    else
                    {
                        toolMonitor = toolMonitors[i];
                        toolMonitor.SelectedIndex = 0;
                    }
                }
                disableChangeFunc = false;

                toolMonitor = toolMonitors[tmp];

                if (toolMonitor.SelectedIndex != 0)
                {

                    for (int i = 0; i < displays.Count; i++)
                    {
                        if (displays[i].displayName.Equals(toolMonitor.Items[0].ToString().Substring(0, toolMonitor.Items[0].ToString().IndexOf(":"))))
                        {
                            comboBoxMonitors.Text = i + 1 + ") " + displays[i].displayName;
                            
                            numDisplay = i;
                            currDisplay.numDisplay = numDisplay;
                            currDisplay.displayLink = displays[i].displayLink;
                            currDisplay.isExternal = displays[i].isExternal;
                            break;
                        }
                    }

                    currDisplay.displayName = toolMonitor.Items[0].ToString().Substring(0, toolMonitor.Items[0].ToString().IndexOf(":"));

                    currDisplay.rGamma = float.Parse(iniFile.Read("rGamma", toolMonitor.Text), customCulture);
                    currDisplay.gGamma = float.Parse(iniFile.Read("gGamma", toolMonitor.Text), customCulture);
                    currDisplay.bGamma = float.Parse(iniFile.Read("bGamma", toolMonitor.Text), customCulture);
                    currDisplay.rContrast = float.Parse(iniFile.Read("rContrast", toolMonitor.Text), customCulture);
                    currDisplay.gContrast = float.Parse(iniFile.Read("gContrast", toolMonitor.Text), customCulture);
                    currDisplay.bContrast = float.Parse(iniFile.Read("bContrast", toolMonitor.Text), customCulture);
                    currDisplay.rBright = float.Parse(iniFile.Read("rBright", toolMonitor.Text), customCulture);
                    currDisplay.gBright = float.Parse(iniFile.Read("gBright", toolMonitor.Text), customCulture);
                    currDisplay.bBright = float.Parse(iniFile.Read("bBright", toolMonitor.Text), customCulture);
                    currDisplay.monitorBrightness = int.Parse(iniFile.Read("monitorBrightness", toolMonitor.Text));
                    currDisplay.monitorContrast = int.Parse(iniFile.Read("monitorContrast", toolMonitor.Text));

                    fillInfo(currDisplay);
                    initPresets();
                    buttonAllColors.PerformClick();

                    Gamma.SetGammaRamp(currDisplay.displayLink,
                        Gamma.CreateGammaRamp(currDisplay.rGamma, currDisplay.gGamma, currDisplay.bGamma,
                        currDisplay.rContrast, currDisplay.gContrast, currDisplay.bContrast, currDisplay.rBright, currDisplay.gBright,
                        currDisplay.bBright));

                    if (currDisplay.isExternal)
                    {
                        trackBarMonitorBrightness.Value = currDisplay.monitorBrightness;
                        trackBarMonitorContrast.Value = currDisplay.monitorContrast;
                    }
                    else
                    {
                        trackBarMonitorBrightness.Value = currDisplay.monitorBrightness;
                    }
                }
            }
        }

        //destroy focuses on buttons, trackbars, comboboxes, text, checkbox
    }
}
