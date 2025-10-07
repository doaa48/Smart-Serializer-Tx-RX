namespace WaterMeter_id
{
    partial class CollectorConfigForm
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
            this.MainTab = new System.Windows.Forms.TabControl();
            this.AutomaticPage = new System.Windows.Forms.TabPage();
            this.label_com = new System.Windows.Forms.Label();
            this.Start = new Guna.UI2.WinForms.Guna2Button();
            this.StopButton = new Guna.UI2.WinForms.Guna2Button();
            this.ClearRichBoxButton = new Guna.UI2.WinForms.Guna2Button();
            this.UartRichBox = new System.Windows.Forms.RichTextBox();
            this.GetwaygroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.COMPortLabel = new System.Windows.Forms.Label();
            this.FrequencyNowLabel = new System.Windows.Forms.Label();
            this.RemainLabel = new System.Windows.Forms.Label();
            this.FrequencyLabel = new System.Windows.Forms.Label();
            this.TimeOutLabel = new System.Windows.Forms.Label();
            this.GatewayLabel = new System.Windows.Forms.Label();
            this.GetwayTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.FrequenyTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.FrequenyNowTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.TimeoutTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.TestBox_RemainMeter = new Guna.UI2.WinForms.Guna2TextBox();
            this.ComPortAutomatocTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.ManualPage = new System.Windows.Forms.TabPage();
            this.StartMan = new Guna.UI2.WinForms.Guna2Button();
            this.StopMan = new Guna.UI2.WinForms.Guna2Button();
            this.UartRichBoxMan = new System.Windows.Forms.RichTextBox();
            this.SendManualButton = new Guna.UI2.WinForms.Guna2Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.SendManualTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.MeterAggregationNumberTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.MeterNumberTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ComPortManualTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.SettingPage = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.RxFileTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.TxFileTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.TxFileBrowseButton = new Guna.UI2.WinForms.Guna2Button();
            this.RxFileBrowseButton = new Guna.UI2.WinForms.Guna2Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RefrechButton = new Guna.UI2.WinForms.Guna2Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.GetwaySelectionComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.StopBitsLabel = new System.Windows.Forms.Label();
            this.DataBitLabel = new System.Windows.Forms.Label();
            this.ParityLabel = new System.Windows.Forms.Label();
            this.BaudrateLabel = new System.Windows.Forms.Label();
            this.BaudRateComboBox = new System.Windows.Forms.ComboBox();
            this.PortComboBox = new System.Windows.Forms.ComboBox();
            this.PortLabel = new System.Windows.Forms.Label();
            this.ParityComboBox = new System.Windows.Forms.ComboBox();
            this.DataBitsComboBox = new System.Windows.Forms.ComboBox();
            this.StopBitsComboBox = new System.Windows.Forms.ComboBox();
            this.TxSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.RxSaveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.MainTab.SuspendLayout();
            this.AutomaticPage.SuspendLayout();
            this.GetwaygroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.ManualPage.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SettingPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTab
            // 
            this.MainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.MainTab.Controls.Add(this.AutomaticPage);
            this.MainTab.Controls.Add(this.ManualPage);
            this.MainTab.Controls.Add(this.SettingPage);
            this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTab.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainTab.Location = new System.Drawing.Point(0, 0);
            this.MainTab.Name = "MainTab";
            this.MainTab.Padding = new System.Drawing.Point(15, 7);
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(800, 547);
            this.MainTab.TabIndex = 0;
            // 
            // AutomaticPage
            // 
            this.AutomaticPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AutomaticPage.Controls.Add(this.label_com);
            this.AutomaticPage.Controls.Add(this.Start);
            this.AutomaticPage.Controls.Add(this.StopButton);
            this.AutomaticPage.Controls.Add(this.ClearRichBoxButton);
            this.AutomaticPage.Controls.Add(this.UartRichBox);
            this.AutomaticPage.Controls.Add(this.GetwaygroupBox);
            this.AutomaticPage.Location = new System.Drawing.Point(4, 39);
            this.AutomaticPage.Name = "AutomaticPage";
            this.AutomaticPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.AutomaticPage.Size = new System.Drawing.Size(792, 504);
            this.AutomaticPage.TabIndex = 0;
            this.AutomaticPage.Text = "Automatic";
            this.AutomaticPage.UseVisualStyleBackColor = true;
            // 
            // label_com
            // 
            this.label_com.AutoSize = true;
            this.label_com.BackColor = System.Drawing.Color.Yellow;
            this.label_com.Location = new System.Drawing.Point(44, 462);
            this.label_com.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_com.Name = "label_com";
            this.label_com.Size = new System.Drawing.Size(174, 19);
            this.label_com.TabIndex = 7;
            this.label_com.Text = "No COM ports available";
            this.label_com.Visible = false;
            // 
            // Start
            // 
            this.Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Start.BackColor = System.Drawing.Color.Transparent;
            this.Start.BorderColor = System.Drawing.Color.White;
            this.Start.BorderRadius = 25;
            this.Start.BorderThickness = 3;
            this.Start.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
            this.Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Start.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.Start.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.Start.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.Start.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.Start.FillColor = System.Drawing.Color.LimeGreen;
            this.Start.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Start.ForeColor = System.Drawing.Color.White;
            this.Start.Location = new System.Drawing.Point(349, 449);
            this.Start.Margin = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.Start.Name = "Start";
            this.Start.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.Start.Size = new System.Drawing.Size(118, 46);
            this.Start.TabIndex = 3;
            this.Start.Text = "Start";
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // StopButton
            // 
            this.StopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StopButton.BackColor = System.Drawing.Color.Transparent;
            this.StopButton.BorderColor = System.Drawing.Color.White;
            this.StopButton.BorderRadius = 25;
            this.StopButton.BorderThickness = 3;
            this.StopButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
            this.StopButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StopButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.StopButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.StopButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.StopButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.StopButton.FillColor = System.Drawing.Color.Red;
            this.StopButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopButton.ForeColor = System.Drawing.Color.White;
            this.StopButton.Location = new System.Drawing.Point(495, 449);
            this.StopButton.Margin = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.StopButton.Name = "StopButton";
            this.StopButton.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.StopButton.Size = new System.Drawing.Size(120, 46);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "Stop";
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // ClearRichBoxButton
            // 
            this.ClearRichBoxButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearRichBoxButton.BorderColor = System.Drawing.Color.White;
            this.ClearRichBoxButton.BorderRadius = 25;
            this.ClearRichBoxButton.BorderThickness = 3;
            this.ClearRichBoxButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
            this.ClearRichBoxButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ClearRichBoxButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.ClearRichBoxButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.ClearRichBoxButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.ClearRichBoxButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.ClearRichBoxButton.FillColor = System.Drawing.Color.Black;
            this.ClearRichBoxButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearRichBoxButton.ForeColor = System.Drawing.Color.White;
            this.ClearRichBoxButton.Location = new System.Drawing.Point(639, 449);
            this.ClearRichBoxButton.Margin = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.ClearRichBoxButton.Name = "ClearRichBoxButton";
            this.ClearRichBoxButton.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.ClearRichBoxButton.Size = new System.Drawing.Size(118, 46);
            this.ClearRichBoxButton.TabIndex = 1;
            this.ClearRichBoxButton.Text = "Clear";
            this.ClearRichBoxButton.Click += new System.EventHandler(this.ClearRichBoxButton_Click);
            // 
            // UartRichBox
            // 
            this.UartRichBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UartRichBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UartRichBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.UartRichBox.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UartRichBox.Location = new System.Drawing.Point(28, 206);
            this.UartRichBox.Name = "UartRichBox";
            this.UartRichBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.UartRichBox.Size = new System.Drawing.Size(722, 227);
            this.UartRichBox.TabIndex = 1;
            this.UartRichBox.Text = "";
            this.UartRichBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UartRichBox_KeyPress);
            // 
            // GetwaygroupBox
            // 
            this.GetwaygroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GetwaygroupBox.Controls.Add(this.tableLayoutPanel1);
            this.GetwaygroupBox.Location = new System.Drawing.Point(6, 9);
            this.GetwaygroupBox.Name = "GetwaygroupBox";
            this.GetwaygroupBox.Size = new System.Drawing.Size(744, 176);
            this.GetwaygroupBox.TabIndex = 0;
            this.GetwaygroupBox.TabStop = false;
            this.GetwaygroupBox.Text = "Getway Configuration";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.COMPortLabel, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.FrequencyNowLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.RemainLabel, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.FrequencyLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.TimeOutLabel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.GatewayLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.GetwayTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.FrequenyTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.FrequenyNowTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.TimeoutTextBox, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.TestBox_RemainMeter, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.ComPortAutomatocTextBox, 3, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(22, 40);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(702, 130);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // COMPortLabel
            // 
            this.COMPortLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.COMPortLabel.AutoSize = true;
            this.COMPortLabel.Location = new System.Drawing.Point(398, 97);
            this.COMPortLabel.Name = "COMPortLabel";
            this.COMPortLabel.Size = new System.Drawing.Size(80, 19);
            this.COMPortLabel.TabIndex = 12;
            this.COMPortLabel.Text = "COM Port";
            this.COMPortLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // FrequencyNowLabel
            // 
            this.FrequencyNowLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FrequencyNowLabel.AutoSize = true;
            this.FrequencyNowLabel.Location = new System.Drawing.Point(32, 97);
            this.FrequencyNowLabel.Name = "FrequencyNowLabel";
            this.FrequencyNowLabel.Size = new System.Drawing.Size(113, 19);
            this.FrequencyNowLabel.TabIndex = 11;
            this.FrequencyNowLabel.Text = "Frequency Now";
            this.FrequencyNowLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // RemainLabel
            // 
            this.RemainLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RemainLabel.AutoSize = true;
            this.RemainLabel.Location = new System.Drawing.Point(408, 54);
            this.RemainLabel.Name = "RemainLabel";
            this.RemainLabel.Size = new System.Drawing.Size(61, 19);
            this.RemainLabel.TabIndex = 10;
            this.RemainLabel.Text = "Remain";
            this.RemainLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // FrequencyLabel
            // 
            this.FrequencyLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FrequencyLabel.AutoSize = true;
            this.FrequencyLabel.Location = new System.Drawing.Point(49, 54);
            this.FrequencyLabel.Name = "FrequencyLabel";
            this.FrequencyLabel.Size = new System.Drawing.Size(79, 19);
            this.FrequencyLabel.TabIndex = 9;
            this.FrequencyLabel.Text = "Frequency";
            this.FrequencyLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // TimeOutLabel
            // 
            this.TimeOutLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TimeOutLabel.AutoSize = true;
            this.TimeOutLabel.Location = new System.Drawing.Point(406, 12);
            this.TimeOutLabel.Name = "TimeOutLabel";
            this.TimeOutLabel.Size = new System.Drawing.Size(64, 19);
            this.TimeOutLabel.TabIndex = 8;
            this.TimeOutLabel.Text = "Timeout";
            this.TimeOutLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // GatewayLabel
            // 
            this.GatewayLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.GatewayLabel.AutoSize = true;
            this.GatewayLabel.Location = new System.Drawing.Point(29, 12);
            this.GatewayLabel.Name = "GatewayLabel";
            this.GatewayLabel.Size = new System.Drawing.Size(118, 19);
            this.GatewayLabel.TabIndex = 0;
            this.GatewayLabel.Text = "Getway Number";
            this.GatewayLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // GetwayTextBox
            // 
            this.GetwayTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GetwayTextBox.BorderRadius = 15;
            this.GetwayTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.GetwayTextBox.DefaultText = "";
            this.GetwayTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.GetwayTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.GetwayTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.GetwayTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.GetwayTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.GetwayTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.GetwayTextBox.ForeColor = System.Drawing.Color.Black;
            this.GetwayTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.GetwayTextBox.Location = new System.Drawing.Point(180, 6);
            this.GetwayTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GetwayTextBox.Name = "GetwayTextBox";
            this.GetwayTextBox.PasswordChar = '\0';
            this.GetwayTextBox.PlaceholderForeColor = System.Drawing.Color.Black;
            this.GetwayTextBox.PlaceholderText = "";
            this.GetwayTextBox.SelectedText = "";
            this.GetwayTextBox.Size = new System.Drawing.Size(167, 31);
            this.GetwayTextBox.TabIndex = 2;
            this.GetwayTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrequenyTextBox
            // 
            this.FrequenyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrequenyTextBox.BorderRadius = 15;
            this.FrequenyTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.FrequenyTextBox.DefaultText = "";
            this.FrequenyTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.FrequenyTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.FrequenyTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.FrequenyTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.FrequenyTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.FrequenyTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FrequenyTextBox.ForeColor = System.Drawing.Color.Black;
            this.FrequenyTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.FrequenyTextBox.Location = new System.Drawing.Point(180, 48);
            this.FrequenyTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FrequenyTextBox.Name = "FrequenyTextBox";
            this.FrequenyTextBox.PasswordChar = '\0';
            this.FrequenyTextBox.PlaceholderForeColor = System.Drawing.Color.Black;
            this.FrequenyTextBox.PlaceholderText = "";
            this.FrequenyTextBox.SelectedText = "";
            this.FrequenyTextBox.Size = new System.Drawing.Size(167, 31);
            this.FrequenyTextBox.TabIndex = 3;
            this.FrequenyTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FrequenyNowTextBox
            // 
            this.FrequenyNowTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrequenyNowTextBox.BorderRadius = 15;
            this.FrequenyNowTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.FrequenyNowTextBox.DefaultText = "";
            this.FrequenyNowTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.FrequenyNowTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.FrequenyNowTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.FrequenyNowTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.FrequenyNowTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.FrequenyNowTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FrequenyNowTextBox.ForeColor = System.Drawing.Color.Black;
            this.FrequenyNowTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.FrequenyNowTextBox.Location = new System.Drawing.Point(180, 90);
            this.FrequenyNowTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FrequenyNowTextBox.Name = "FrequenyNowTextBox";
            this.FrequenyNowTextBox.PasswordChar = '\0';
            this.FrequenyNowTextBox.PlaceholderForeColor = System.Drawing.Color.Black;
            this.FrequenyNowTextBox.PlaceholderText = "";
            this.FrequenyNowTextBox.SelectedText = "";
            this.FrequenyNowTextBox.Size = new System.Drawing.Size(167, 31);
            this.FrequenyNowTextBox.TabIndex = 4;
            this.FrequenyNowTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TimeoutTextBox
            // 
            this.TimeoutTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeoutTextBox.BorderRadius = 15;
            this.TimeoutTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TimeoutTextBox.DefaultText = "";
            this.TimeoutTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.TimeoutTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.TimeoutTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.TimeoutTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.TimeoutTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.TimeoutTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TimeoutTextBox.ForeColor = System.Drawing.Color.Black;
            this.TimeoutTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.TimeoutTextBox.Location = new System.Drawing.Point(530, 6);
            this.TimeoutTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TimeoutTextBox.Name = "TimeoutTextBox";
            this.TimeoutTextBox.PasswordChar = '\0';
            this.TimeoutTextBox.PlaceholderForeColor = System.Drawing.Color.Black;
            this.TimeoutTextBox.PlaceholderText = "";
            this.TimeoutTextBox.SelectedText = "";
            this.TimeoutTextBox.Size = new System.Drawing.Size(167, 31);
            this.TimeoutTextBox.TabIndex = 5;
            this.TimeoutTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TestBox_RemainMeter
            // 
            this.TestBox_RemainMeter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TestBox_RemainMeter.BorderRadius = 15;
            this.TestBox_RemainMeter.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TestBox_RemainMeter.DefaultText = "";
            this.TestBox_RemainMeter.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.TestBox_RemainMeter.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.TestBox_RemainMeter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.TestBox_RemainMeter.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.TestBox_RemainMeter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.TestBox_RemainMeter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TestBox_RemainMeter.ForeColor = System.Drawing.Color.Black;
            this.TestBox_RemainMeter.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.TestBox_RemainMeter.Location = new System.Drawing.Point(530, 48);
            this.TestBox_RemainMeter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TestBox_RemainMeter.Name = "TestBox_RemainMeter";
            this.TestBox_RemainMeter.PasswordChar = '\0';
            this.TestBox_RemainMeter.PlaceholderForeColor = System.Drawing.Color.Black;
            this.TestBox_RemainMeter.PlaceholderText = "";
            this.TestBox_RemainMeter.SelectedText = "";
            this.TestBox_RemainMeter.Size = new System.Drawing.Size(167, 31);
            this.TestBox_RemainMeter.TabIndex = 6;
            this.TestBox_RemainMeter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ComPortAutomatocTextBox
            // 
            this.ComPortAutomatocTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComPortAutomatocTextBox.BorderRadius = 15;
            this.ComPortAutomatocTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ComPortAutomatocTextBox.DefaultText = "";
            this.ComPortAutomatocTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.ComPortAutomatocTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ComPortAutomatocTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.ComPortAutomatocTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.ComPortAutomatocTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ComPortAutomatocTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ComPortAutomatocTextBox.ForeColor = System.Drawing.Color.Black;
            this.ComPortAutomatocTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ComPortAutomatocTextBox.Location = new System.Drawing.Point(530, 90);
            this.ComPortAutomatocTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComPortAutomatocTextBox.Name = "ComPortAutomatocTextBox";
            this.ComPortAutomatocTextBox.PasswordChar = '\0';
            this.ComPortAutomatocTextBox.PlaceholderForeColor = System.Drawing.Color.Black;
            this.ComPortAutomatocTextBox.PlaceholderText = "";
            this.ComPortAutomatocTextBox.SelectedText = "";
            this.ComPortAutomatocTextBox.Size = new System.Drawing.Size(167, 31);
            this.ComPortAutomatocTextBox.TabIndex = 7;
            this.ComPortAutomatocTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ManualPage
            // 
            this.ManualPage.Controls.Add(this.StartMan);
            this.ManualPage.Controls.Add(this.StopMan);
            this.ManualPage.Controls.Add(this.UartRichBoxMan);
            this.ManualPage.Controls.Add(this.SendManualButton);
            this.ManualPage.Controls.Add(this.tableLayoutPanel6);
            this.ManualPage.Controls.Add(this.tableLayoutPanel5);
            this.ManualPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ManualPage.Location = new System.Drawing.Point(4, 39);
            this.ManualPage.Name = "ManualPage";
            this.ManualPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.ManualPage.Size = new System.Drawing.Size(792, 504);
            this.ManualPage.TabIndex = 1;
            this.ManualPage.Text = "Manual";
            this.ManualPage.UseVisualStyleBackColor = true;
            // 
            // StartMan
            // 
            this.StartMan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StartMan.BackColor = System.Drawing.Color.Transparent;
            this.StartMan.BorderColor = System.Drawing.Color.White;
            this.StartMan.BorderRadius = 25;
            this.StartMan.BorderThickness = 3;
            this.StartMan.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
            this.StartMan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartMan.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.StartMan.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.StartMan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.StartMan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.StartMan.FillColor = System.Drawing.Color.LimeGreen;
            this.StartMan.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartMan.ForeColor = System.Drawing.Color.White;
            this.StartMan.Location = new System.Drawing.Point(504, 445);
            this.StartMan.Margin = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.StartMan.Name = "StartMan";
            this.StartMan.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.StartMan.Size = new System.Drawing.Size(118, 46);
            this.StartMan.TabIndex = 6;
            this.StartMan.Text = "Start";
            this.StartMan.Click += new System.EventHandler(this.StartMan_Click);
            // 
            // StopMan
            // 
            this.StopMan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StopMan.BackColor = System.Drawing.Color.Transparent;
            this.StopMan.BorderColor = System.Drawing.Color.White;
            this.StopMan.BorderRadius = 25;
            this.StopMan.BorderThickness = 3;
            this.StopMan.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
            this.StopMan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StopMan.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.StopMan.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.StopMan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.StopMan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.StopMan.FillColor = System.Drawing.Color.Red;
            this.StopMan.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopMan.ForeColor = System.Drawing.Color.White;
            this.StopMan.Location = new System.Drawing.Point(637, 445);
            this.StopMan.Margin = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.StopMan.Name = "StopMan";
            this.StopMan.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.StopMan.Size = new System.Drawing.Size(120, 46);
            this.StopMan.TabIndex = 5;
            this.StopMan.Text = "Stop";
            this.StopMan.Click += new System.EventHandler(this.StopMan_Click);
            // 
            // UartRichBoxMan
            // 
            this.UartRichBoxMan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UartRichBoxMan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UartRichBoxMan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.UartRichBoxMan.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UartRichBoxMan.Location = new System.Drawing.Point(24, 211);
            this.UartRichBoxMan.Name = "UartRichBoxMan";
            this.UartRichBoxMan.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.UartRichBoxMan.Size = new System.Drawing.Size(754, 224);
            this.UartRichBoxMan.TabIndex = 4;
            this.UartRichBoxMan.Text = "";
            // 
            // SendManualButton
            // 
            this.SendManualButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SendManualButton.BorderColor = System.Drawing.Color.White;
            this.SendManualButton.BorderRadius = 25;
            this.SendManualButton.BorderThickness = 3;
            this.SendManualButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
            this.SendManualButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SendManualButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.SendManualButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.SendManualButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.SendManualButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.SendManualButton.FillColor = System.Drawing.Color.Black;
            this.SendManualButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendManualButton.ForeColor = System.Drawing.Color.White;
            this.SendManualButton.Location = new System.Drawing.Point(660, 153);
            this.SendManualButton.Margin = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.SendManualButton.Name = "SendManualButton";
            this.SendManualButton.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.SendManualButton.Size = new System.Drawing.Size(121, 46);
            this.SendManualButton.TabIndex = 3;
            this.SendManualButton.Text = "Send";
            this.SendManualButton.Click += new System.EventHandler(this.SendManualButton_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 698F));
            this.tableLayoutPanel6.Controls.Add(this.SendManualTextBox, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(8, 89);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(773, 47);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // SendManualTextBox
            // 
            this.SendManualTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SendManualTextBox.BorderRadius = 15;
            this.SendManualTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.SendManualTextBox.DefaultText = "";
            this.SendManualTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.SendManualTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.SendManualTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.SendManualTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.SendManualTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.SendManualTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SendManualTextBox.ForeColor = System.Drawing.Color.Black;
            this.SendManualTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.SendManualTextBox.Location = new System.Drawing.Point(79, 4);
            this.SendManualTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SendManualTextBox.Name = "SendManualTextBox";
            this.SendManualTextBox.PasswordChar = '\0';
            this.SendManualTextBox.PlaceholderForeColor = System.Drawing.Color.Black;
            this.SendManualTextBox.PlaceholderText = "";
            this.SendManualTextBox.SelectedText = "";
            this.SendManualTextBox.Size = new System.Drawing.Size(692, 39);
            this.SendManualTextBox.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 19);
            this.label4.TabIndex = 11;
            this.label4.Text = "Send :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel5.ColumnCount = 6;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.29897F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.30928F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.38144F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.46907F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.94845F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.33505F));
            this.tableLayoutPanel5.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.MeterAggregationNumberTextBox, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.MeterNumberTextBox, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.ComPortManualTextBox, 5, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(8, 24);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(773, 47);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(239, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(191, 19);
            this.label7.TabIndex = 8;
            this.label7.Text = "Meter Aggrigation Number";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(16, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 19);
            this.label8.TabIndex = 0;
            this.label8.Text = "Meter Number";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // MeterAggregationNumberTextBox
            // 
            this.MeterAggregationNumberTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MeterAggregationNumberTextBox.BorderRadius = 15;
            this.MeterAggregationNumberTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MeterAggregationNumberTextBox.DefaultText = "";
            this.MeterAggregationNumberTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.MeterAggregationNumberTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.MeterAggregationNumberTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.MeterAggregationNumberTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.MeterAggregationNumberTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.MeterAggregationNumberTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MeterAggregationNumberTextBox.ForeColor = System.Drawing.Color.Black;
            this.MeterAggregationNumberTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.MeterAggregationNumberTextBox.Location = new System.Drawing.Point(451, 6);
            this.MeterAggregationNumberTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MeterAggregationNumberTextBox.Name = "MeterAggregationNumberTextBox";
            this.MeterAggregationNumberTextBox.PasswordChar = '\0';
            this.MeterAggregationNumberTextBox.PlaceholderForeColor = System.Drawing.Color.Black;
            this.MeterAggregationNumberTextBox.PlaceholderText = "";
            this.MeterAggregationNumberTextBox.SelectedText = "";
            this.MeterAggregationNumberTextBox.Size = new System.Drawing.Size(81, 31);
            this.MeterAggregationNumberTextBox.TabIndex = 5;
            this.MeterAggregationNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MeterAggregationNumberTextBox.TextChanged += new System.EventHandler(this.MeterAggregationNumberTextBox_TextChanged);
            // 
            // MeterNumberTextBox
            // 
            this.MeterNumberTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MeterNumberTextBox.BorderRadius = 15;
            this.MeterNumberTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MeterNumberTextBox.DefaultText = "";
            this.MeterNumberTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.MeterNumberTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.MeterNumberTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.MeterNumberTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.MeterNumberTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.MeterNumberTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MeterNumberTextBox.ForeColor = System.Drawing.Color.Black;
            this.MeterNumberTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.MeterNumberTextBox.Location = new System.Drawing.Point(146, 6);
            this.MeterNumberTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MeterNumberTextBox.Name = "MeterNumberTextBox";
            this.MeterNumberTextBox.PasswordChar = '\0';
            this.MeterNumberTextBox.PlaceholderForeColor = System.Drawing.Color.Black;
            this.MeterNumberTextBox.PlaceholderText = "";
            this.MeterNumberTextBox.SelectedText = "";
            this.MeterNumberTextBox.Size = new System.Drawing.Size(72, 31);
            this.MeterNumberTextBox.TabIndex = 2;
            this.MeterNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MeterNumberTextBox.TextChanged += new System.EventHandler(this.MeterNumberTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(553, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "COM Port";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ComPortManualTextBox
            // 
            this.ComPortManualTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComPortManualTextBox.BorderRadius = 15;
            this.ComPortManualTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ComPortManualTextBox.DefaultText = "";
            this.ComPortManualTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.ComPortManualTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ComPortManualTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.ComPortManualTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.ComPortManualTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ComPortManualTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ComPortManualTextBox.ForeColor = System.Drawing.Color.Black;
            this.ComPortManualTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ComPortManualTextBox.Location = new System.Drawing.Point(655, 6);
            this.ComPortManualTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComPortManualTextBox.Name = "ComPortManualTextBox";
            this.ComPortManualTextBox.PasswordChar = '\0';
            this.ComPortManualTextBox.PlaceholderForeColor = System.Drawing.Color.Black;
            this.ComPortManualTextBox.PlaceholderText = "";
            this.ComPortManualTextBox.SelectedText = "";
            this.ComPortManualTextBox.Size = new System.Drawing.Size(113, 31);
            this.ComPortManualTextBox.TabIndex = 10;
            this.ComPortManualTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SettingPage
            // 
            this.SettingPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SettingPage.Controls.Add(this.groupBox1);
            this.SettingPage.Controls.Add(this.groupBox3);
            this.SettingPage.Controls.Add(this.groupBox2);
            this.SettingPage.Font = new System.Drawing.Font("Trebuchet MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SettingPage.Location = new System.Drawing.Point(4, 39);
            this.SettingPage.Name = "SettingPage";
            this.SettingPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.SettingPage.Size = new System.Drawing.Size(792, 504);
            this.SettingPage.TabIndex = 2;
            this.SettingPage.Text = "Setting";
            this.SettingPage.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 353);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 121);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Data Logging";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tx Log File  :";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 90);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rx Log File  :";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.20489F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.79511F));
            this.tableLayoutPanel4.Controls.Add(this.RxFileTextBox, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.TxFileTextBox, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.TxFileBrowseButton, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.RxFileBrowseButton, 1, 1);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(132, 37);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(622, 78);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // RxFileTextBox
            // 
            this.RxFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RxFileTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.RxFileTextBox.DefaultText = "";
            this.RxFileTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.RxFileTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.RxFileTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.RxFileTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.RxFileTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.RxFileTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.RxFileTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.RxFileTextBox.Location = new System.Drawing.Point(6, 45);
            this.RxFileTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RxFileTextBox.Name = "RxFileTextBox";
            this.RxFileTextBox.PasswordChar = '\0';
            this.RxFileTextBox.PlaceholderText = "";
            this.RxFileTextBox.SelectedText = "";
            this.RxFileTextBox.Size = new System.Drawing.Size(467, 26);
            this.RxFileTextBox.TabIndex = 4;
            // 
            // TxFileTextBox
            // 
            this.TxFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxFileTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TxFileTextBox.DefaultText = "";
            this.TxFileTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.TxFileTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.TxFileTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.TxFileTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.TxFileTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.TxFileTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxFileTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.TxFileTextBox.Location = new System.Drawing.Point(6, 7);
            this.TxFileTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxFileTextBox.Name = "TxFileTextBox";
            this.TxFileTextBox.PasswordChar = '\0';
            this.TxFileTextBox.PlaceholderText = "";
            this.TxFileTextBox.SelectedText = "";
            this.TxFileTextBox.Size = new System.Drawing.Size(467, 27);
            this.TxFileTextBox.TabIndex = 1;
            // 
            // TxFileBrowseButton
            // 
            this.TxFileBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxFileBrowseButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.TxFileBrowseButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.TxFileBrowseButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.TxFileBrowseButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.TxFileBrowseButton.FillColor = System.Drawing.Color.DimGray;
            this.TxFileBrowseButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxFileBrowseButton.ForeColor = System.Drawing.Color.White;
            this.TxFileBrowseButton.Location = new System.Drawing.Point(482, 6);
            this.TxFileBrowseButton.Name = "TxFileBrowseButton";
            this.TxFileBrowseButton.Size = new System.Drawing.Size(134, 29);
            this.TxFileBrowseButton.TabIndex = 2;
            this.TxFileBrowseButton.Text = "Browse";
            this.TxFileBrowseButton.Click += new System.EventHandler(this.TxFileBrowseButton_Click);
            // 
            // RxFileBrowseButton
            // 
            this.RxFileBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RxFileBrowseButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.RxFileBrowseButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.RxFileBrowseButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.RxFileBrowseButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.RxFileBrowseButton.FillColor = System.Drawing.Color.DimGray;
            this.RxFileBrowseButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.RxFileBrowseButton.ForeColor = System.Drawing.Color.White;
            this.RxFileBrowseButton.Location = new System.Drawing.Point(482, 44);
            this.RxFileBrowseButton.Name = "RxFileBrowseButton";
            this.RxFileBrowseButton.Size = new System.Drawing.Size(134, 28);
            this.RxFileBrowseButton.TabIndex = 3;
            this.RxFileBrowseButton.Text = "Browse";
            this.RxFileBrowseButton.Click += new System.EventHandler(this.RxFileBrowseButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.RefrechButton);
            this.groupBox3.Controls.Add(this.tableLayoutPanel3);
            this.groupBox3.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(328, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(444, 315);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Getway Selection";
            // 
            // RefrechButton
            // 
            this.RefrechButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefrechButton.BorderRadius = 25;
            this.RefrechButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
            this.RefrechButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.RefrechButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.RefrechButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.RefrechButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.RefrechButton.FillColor = System.Drawing.Color.Gray;
            this.RefrechButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefrechButton.ForeColor = System.Drawing.Color.White;
            this.RefrechButton.Location = new System.Drawing.Point(312, 261);
            this.RefrechButton.Name = "RefrechButton";
            this.RefrechButton.Size = new System.Drawing.Size(100, 42);
            this.RefrechButton.TabIndex = 1;
            this.RefrechButton.Text = "Refrech";
            this.RefrechButton.Click += new System.EventHandler(this.RefrechButton_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.05948F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.94052F));
            this.tableLayoutPanel3.Controls.Add(this.GetwaySelectionComboBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(14, 37);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(332, 36);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // GetwaySelectionComboBox
            // 
            this.GetwaySelectionComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.GetwaySelectionComboBox.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetwaySelectionComboBox.FormattingEnabled = true;
            this.GetwaySelectionComboBox.Location = new System.Drawing.Point(157, 5);
            this.GetwaySelectionComboBox.Name = "GetwaySelectionComboBox";
            this.GetwaySelectionComboBox.Size = new System.Drawing.Size(137, 25);
            this.GetwaySelectionComboBox.TabIndex = 0;
            this.GetwaySelectionComboBox.Text = "Select Getway";
        //    this.GetwaySelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.GetwaySelectionComboBox_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 8);
            this.label9.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 19);
            this.label9.TabIndex = 0;
            this.label9.Text = "Gatway Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 315);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "UART Setting";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.70412F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.29588F));
            this.tableLayoutPanel2.Controls.Add(this.StopBitsLabel, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.DataBitLabel, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.ParityLabel, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.BaudrateLabel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.BaudRateComboBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.PortComboBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.PortLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ParityComboBox, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.DataBitsComboBox, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.StopBitsComboBox, 1, 4);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(14, 37);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(269, 266);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // StopBitsLabel
            // 
            this.StopBitsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.StopBitsLabel.AutoSize = true;
            this.StopBitsLabel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopBitsLabel.Location = new System.Drawing.Point(5, 227);
            this.StopBitsLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.StopBitsLabel.Name = "StopBitsLabel";
            this.StopBitsLabel.Size = new System.Drawing.Size(79, 19);
            this.StopBitsLabel.TabIndex = 9;
            this.StopBitsLabel.Text = "Stop Bits :";
            // 
            // DataBitLabel
            // 
            this.DataBitLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DataBitLabel.AutoSize = true;
            this.DataBitLabel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataBitLabel.Location = new System.Drawing.Point(5, 173);
            this.DataBitLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.DataBitLabel.Name = "DataBitLabel";
            this.DataBitLabel.Size = new System.Drawing.Size(82, 19);
            this.DataBitLabel.TabIndex = 8;
            this.DataBitLabel.Text = "Data Bits :";
            // 
            // ParityLabel
            // 
            this.ParityLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ParityLabel.AutoSize = true;
            this.ParityLabel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ParityLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ParityLabel.Location = new System.Drawing.Point(5, 121);
            this.ParityLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.ParityLabel.Name = "ParityLabel";
            this.ParityLabel.Size = new System.Drawing.Size(58, 19);
            this.ParityLabel.TabIndex = 7;
            this.ParityLabel.Text = "Parity :";
            // 
            // BaudrateLabel
            // 
            this.BaudrateLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BaudrateLabel.AutoSize = true;
            this.BaudrateLabel.CausesValidation = false;
            this.BaudrateLabel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BaudrateLabel.Location = new System.Drawing.Point(5, 69);
            this.BaudrateLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.BaudrateLabel.Name = "BaudrateLabel";
            this.BaudrateLabel.Size = new System.Drawing.Size(90, 19);
            this.BaudrateLabel.TabIndex = 6;
            this.BaudrateLabel.Text = "Baud Rate :";
            // 
            // BaudRateComboBox
            // 
            this.BaudRateComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BaudRateComboBox.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BaudRateComboBox.FormattingEnabled = true;
            this.BaudRateComboBox.Items.AddRange(new object[] {
            "115200",
            "1200",
            "19200",
            "2400",
            "38400",
            "4800",
            "57600",
            "9600"});
            this.BaudRateComboBox.Location = new System.Drawing.Point(119, 66);
            this.BaudRateComboBox.Name = "BaudRateComboBox";
            this.BaudRateComboBox.Size = new System.Drawing.Size(129, 25);
            this.BaudRateComboBox.Sorted = true;
            this.BaudRateComboBox.TabIndex = 2;
            this.BaudRateComboBox.Text = "9600";
            // 
            // PortComboBox
            // 
            this.PortComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PortComboBox.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PortComboBox.FormattingEnabled = true;
            this.PortComboBox.Location = new System.Drawing.Point(119, 14);
            this.PortComboBox.Name = "PortComboBox";
            this.PortComboBox.Size = new System.Drawing.Size(129, 25);
            this.PortComboBox.TabIndex = 0;
            this.PortComboBox.Text = "Select Port";
            this.PortComboBox.MouseHover += new System.EventHandler(this.PortComboBox_MouseHover);
            // 
            // PortLabel
            // 
            this.PortLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PortLabel.AutoSize = true;
            this.PortLabel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PortLabel.Location = new System.Drawing.Point(5, 17);
            this.PortLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(85, 19);
            this.PortLabel.TabIndex = 0;
            this.PortLabel.Text = "COM Port:";
            // 
            // ParityComboBox
            // 
            this.ParityComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ParityComboBox.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ParityComboBox.FormattingEnabled = true;
            this.ParityComboBox.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd"});
            this.ParityComboBox.Location = new System.Drawing.Point(119, 118);
            this.ParityComboBox.Name = "ParityComboBox";
            this.ParityComboBox.Size = new System.Drawing.Size(129, 25);
            this.ParityComboBox.TabIndex = 3;
            this.ParityComboBox.Text = "None";
            // 
            // DataBitsComboBox
            // 
            this.DataBitsComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DataBitsComboBox.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataBitsComboBox.FormattingEnabled = true;
            this.DataBitsComboBox.Items.AddRange(new object[] {
            "8",
            "9"});
            this.DataBitsComboBox.Location = new System.Drawing.Point(119, 170);
            this.DataBitsComboBox.Name = "DataBitsComboBox";
            this.DataBitsComboBox.Size = new System.Drawing.Size(129, 25);
            this.DataBitsComboBox.TabIndex = 4;
            this.DataBitsComboBox.Text = "8";
            // 
            // StopBitsComboBox
            // 
            this.StopBitsComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.StopBitsComboBox.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopBitsComboBox.FormattingEnabled = true;
            this.StopBitsComboBox.Items.AddRange(new object[] {
            "1",
            "2"});
            this.StopBitsComboBox.Location = new System.Drawing.Point(119, 224);
            this.StopBitsComboBox.Name = "StopBitsComboBox";
            this.StopBitsComboBox.Size = new System.Drawing.Size(129, 25);
            this.StopBitsComboBox.TabIndex = 5;
            this.StopBitsComboBox.Text = "1";
            // 
            // CollectorConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(800, 547);
            this.Controls.Add(this.MainTab);
            this.MaximumSize = new System.Drawing.Size(999, 648);
            this.Name = "CollectorConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Collector App";
            this.MainTab.ResumeLayout(false);
            this.AutomaticPage.ResumeLayout(false);
            this.AutomaticPage.PerformLayout();
            this.GetwaygroupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ManualPage.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.SettingPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTab;
        private System.Windows.Forms.TabPage AutomaticPage;
        private System.Windows.Forms.TabPage ManualPage;
        private System.Windows.Forms.TabPage SettingPage;
        private System.Windows.Forms.GroupBox GetwaygroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox PortComboBox;
        private System.Windows.Forms.Label GatewayLabel;
        private Guna.UI2.WinForms.Guna2TextBox GetwayTextBox;
        private System.Windows.Forms.Label COMPortLabel;
        private System.Windows.Forms.Label FrequencyNowLabel;
        private System.Windows.Forms.Label RemainLabel;
        private System.Windows.Forms.Label FrequencyLabel;
        private System.Windows.Forms.Label TimeOutLabel;
        private Guna.UI2.WinForms.Guna2TextBox FrequenyTextBox;
        private Guna.UI2.WinForms.Guna2TextBox FrequenyNowTextBox;
        private Guna.UI2.WinForms.Guna2TextBox TimeoutTextBox;
        private Guna.UI2.WinForms.Guna2TextBox TestBox_RemainMeter;
        private Guna.UI2.WinForms.Guna2TextBox ComPortAutomatocTextBox;
        private System.Windows.Forms.RichTextBox UartRichBox;
        private Guna.UI2.WinForms.Guna2Button ClearRichBoxButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label StopBitsLabel;
        private System.Windows.Forms.Label DataBitLabel;
        private System.Windows.Forms.Label ParityLabel;
        private System.Windows.Forms.Label BaudrateLabel;
        private System.Windows.Forms.ComboBox ParityComboBox;
        private System.Windows.Forms.ComboBox DataBitsComboBox;
        private System.Windows.Forms.ComboBox StopBitsComboBox;
        private Guna.UI2.WinForms.Guna2Button Start;
        private Guna.UI2.WinForms.Guna2Button StopButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox GetwaySelectionComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Button TxFileBrowseButton;
        private Guna.UI2.WinForms.Guna2Button RxFileBrowseButton;
        private Guna.UI2.WinForms.Guna2TextBox RxFileTextBox;
        private Guna.UI2.WinForms.Guna2TextBox TxFileTextBox;
        private System.Windows.Forms.SaveFileDialog TxSaveFileDialog;
        private System.Windows.Forms.SaveFileDialog RxSaveFileDialog2;
        private System.Windows.Forms.ComboBox BaudRateComboBox;
        private Guna.UI2.WinForms.Guna2Button RefrechButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private Guna.UI2.WinForms.Guna2TextBox MeterAggregationNumberTextBox;
        private Guna.UI2.WinForms.Guna2TextBox MeterNumberTextBox;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox ComPortManualTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2TextBox SendManualTextBox;
        private Guna.UI2.WinForms.Guna2Button SendManualButton;
        private System.Windows.Forms.RichTextBox UartRichBoxMan;
        private Guna.UI2.WinForms.Guna2Button StartMan;
        private Guna.UI2.WinForms.Guna2Button StopMan;
        private System.Windows.Forms.Label label_com;
    }
}

