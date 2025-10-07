using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using WaterMeter_id;
using WaterMeter_id.SEL;
using Org.BouncyCastle.Asn1.X509;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace WaterMeter_id
{

    public partial class CollectorConfigForm : Form
    {
        SEL_CollectorConfig SEL_CollectorConfigObj=new SEL_CollectorConfig();
        SEL_Getway SEL_GetwayObj = new SEL_Getway();
        SEL_SmartConfiguration SEL_SmartConfigurationObj = new SEL_SmartConfiguration();
        BLL_JsonSettings loadedSettings = new BLL_JsonSettings();

        public delegate void ManualPrintRichBoxCallbackTransmittedData(string TxData);
        public delegate void ManualPrintRichBoxCallbackRecievedData(string RxData);

        Thread thread ;
        bool PortIsOpenOrNot;
        bool PortExistOrNot;

        private ManualPrintRichBoxCallbackTransmittedData PrintTxData;
        private void MainTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selectedTab = MainTab.SelectedTab;
            if (selectedTab.Name == "SettingPage")
            {
                //setup page of start to 
                //Get Getways Numbers from database and put it in Combobox
                PopulateComboBox(GetwaySelectionComboBox);



                string[] Ports = SEL_CollectorConfigObj.CheckSerialPorts(ref PortExistOrNot);


                if (PortExistOrNot)
                {
                    UartRichBox.Enabled = true;

                    foreach (string portName in Ports)
                    {
                        if (!PortComboBox.Items.Contains(portName))
                        {
                            PortComboBox.Items.Add(portName);
                        }

                    }
                    if (PortComboBox.Items.Count > 0)
                    {
                        PortComboBox.SelectedIndex = 0;
                        //Load Data from Json File To Text and Combo Boxes
                         loadedSettings = SEL_CollectorConfigObj.GetJsonData();

                        PortComboBox.Text = loadedSettings.ComPort;
                        BaudRateComboBox.Text = loadedSettings.UART_BaudRate.ToString();
                        DataBitsComboBox.Text = loadedSettings.UART_DataBits.ToString();
                        StopBitsComboBox.Text = loadedSettings.UART_StopBits.ToString();
                        GetwaySelectionComboBox.Text = loadedSettings.GatewayNumber.ToString();
                        ParityComboBox.Text = loadedSettings.UART_Parity;
                        TxFileTextBox.Text = loadedSettings.TxLog;
                        RxFileTextBox.Text = loadedSettings.RxLog;

                        //Set UART Configuration From Text and ComboBoxes to Serial Port
                        SEL_CollectorConfigObj.SetSerialConfig(PortComboBox.Text, BaudRateComboBox.Text, StopBitsComboBox.Text, DataBitsComboBox.Text, ParityComboBox.Text);
                        SEL_CollectorConfigObj.BLL_JsonSettings_Data = loadedSettings;
                        RefrechButtun();

                        // COM port is connected, hide the label
                        //label_com.Visible = false;
                    }
                }
                else
                {
                    UartRichBox.Enabled = false;
                    PortComboBox.Text = " Select Port ";

                     loadedSettings = SEL_CollectorConfigObj.GetJsonData();

                    BaudRateComboBox.Text = loadedSettings.UART_BaudRate.ToString();
                    DataBitsComboBox.Text = loadedSettings.UART_DataBits.ToString();
                    StopBitsComboBox.Text = loadedSettings.UART_StopBits.ToString();
                    GetwaySelectionComboBox.Text = loadedSettings.GatewayNumber.ToString();
                    ParityComboBox.Text = loadedSettings.UART_Parity;
                    TxFileTextBox.Text = loadedSettings.TxLog;
                    RxFileTextBox.Text = loadedSettings.RxLog;

                    //Set UART Configuration From Text and ComboBoxes to Serial Port
                    SEL_CollectorConfigObj.SetSerialConfig(PortComboBox.Text, BaudRateComboBox.Text, StopBitsComboBox.Text, DataBitsComboBox.Text, ParityComboBox.Text);
                    SEL_CollectorConfigObj.BLL_JsonSettings_Data = loadedSettings;
                    RefrechButtun();
                    // Handle case where no COM ports are available
                    MessageBox.Show("No COM ports available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // COM port is not connected, show the label
                    //label_com.Visible = true;
                }

            }
            else if (selectedTab.Name == "ManualPage")
            {

            }
            else if (selectedTab.Name == "AutomaticPage")
            {
            
            }
        }
        public CollectorConfigForm()
        {
           
            
            InitializeComponent();
            MainTab.SelectedIndexChanged += new EventHandler(MainTab_SelectedIndexChanged);

            PrintTxData = (string text) =>
            {
                AppendColoredText(UartRichBox, text, Color.Red);
            };


          
            // Set the callback function
            SEL_CollectorConfigObj.CallBack_ReveiveData += ReceiveDataEvent;
            SEL_CollectorConfigObj.CallBack_TransimitData += TransmitDataEvent;
            SEL_CollectorConfigObj.CallBack_UpdateData+=UpdateDataEvent;
            SEL_CollectorConfigObj.CallBack_label += LabelEvent;

            //check if servr working refil data 
            loadedSettings = SEL_CollectorConfigObj.GetJsonData();
            // Start the thread
            if (SEL_CollectorConfigObj.BLL_SmartConfiguration_Data != null)
             {
                //save data of frequency and timeout
                loadedSettings.Timeout = SEL_CollectorConfigObj.BLL_SmartConfiguration_Data.SmartConfigurations_TimeOut;
                loadedSettings.Frequency = SEL_CollectorConfigObj.BLL_SmartConfiguration_Data.SmartConfigurations_Frequency;

                SEL_CollectorConfigObj.SaveJsonData(loadedSettings);
             }   
                    
        
            //Get Data from BLL to Put it in the Text Box
            TimeoutTextBox.Text= loadedSettings.Timeout.ToString();
            FrequenyTextBox.Text = loadedSettings.Frequency.ToString();
            GetwayTextBox.Text = loadedSettings.GatewayNumber.ToString();
            ComPortAutomatocTextBox.Text = loadedSettings.ComPort;

            //Set UART Configuration From Text and ComboBoxes to Serial Port
            SEL_CollectorConfigObj.SetSerialConfig(loadedSettings.ComPort, loadedSettings.UART_BaudRate.ToString(), loadedSettings.UART_StopBits.ToString(), loadedSettings.UART_DataBits.ToString(), loadedSettings.UART_Parity);
            SEL_CollectorConfigObj.rxfilePath = loadedSettings.RxLog;
            SEL_CollectorConfigObj.txfilePath = loadedSettings.TxLog;

            SEL_CollectorConfigObj.BLL_JsonSettings_Data = loadedSettings;
            Start.PerformClick();

            //Get Data from BLL to Put it in the Text Box

        }

        void LabelEvent(int conn)
        {
            try
            {
                if (label_com.InvokeRequired)
                {
                    label_com.Invoke(new Action<int>(LabelEvent), conn);
                }
                else
                {
                    if (conn == 0)
                    {
                        label_com.Visible = false;
                    }
                    else
                    {
                        label_com.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                // Console.WriteLine("An exception occurred: " + ex.Message);
            }
        }


        void ReceiveDataEvent(string Data)
        {
            
            if (ManualPage.Enabled == true)
            {
                if (UartRichBoxMan.InvokeRequired)
                {
                    UartRichBoxMan.Invoke((MethodInvoker)(() => ReceiveDataEvent(Data)));
                }
                else
                {
                    // Update the UartRichBox control with received data
                    AppendColoredText(UartRichBoxMan, Data, Color.Green);
                }
            }
            else
            {
                if (UartRichBox.InvokeRequired)
                {
                    UartRichBox.Invoke((MethodInvoker)(() => ReceiveDataEvent(Data)));
                }
                else
                {
                    // Update the UartRichBox control with received data
                    AppendColoredText(UartRichBox, Data, Color.Green);
                }
            }
        }

        void TransmitDataEvent(string Data)
        {
     
            if (ManualPage.Enabled == true)
            {
                if (UartRichBoxMan.InvokeRequired)
                {
                    UartRichBoxMan.Invoke((MethodInvoker)(() => TransmitDataEvent(Data)));
                }
                else
                {
                    // Update the UartRichBox control with transmitted data
                    AppendColoredText(UartRichBoxMan, Data, Color.DarkRed);
                }
            }
            else
            {
                if (UartRichBox.InvokeRequired)
                {
                    UartRichBox.Invoke((MethodInvoker)(() => TransmitDataEvent(Data)));
                }
                else
                {
                    // Update the UartRichBox control with transmitted data
                    AppendColoredText(UartRichBox, Data, Color.DarkRed);
                }

            }
               
        }

        void UpdateDataEvent(int frequencyNum, int remainNum,int timeout,int frequency)
        {
          
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateDataEvent(frequencyNum, remainNum, timeout, frequency)));
            }
            else
            {
                frequencyNum += 1; //because it start with index 0
                // Update the UI controls with the provided data
                TestBox_RemainMeter.Text = remainNum.ToString();
                FrequenyNowTextBox.Text = frequencyNum.ToString();
                TimeoutTextBox.Text = timeout.ToString();
                FrequenyTextBox.Text = frequency.ToString();
            }
        }

        private void DisplayText(object sender, EventArgs e)
        {
            string DislayedText = "";

            SEL_CollectorConfigObj.ReceiveSerialData(ref DislayedText);

            AppendColoredText(UartRichBox, DislayedText, Color.Green);

            SEL_CollectorConfigObj.WriteOnRxLogFile(DislayedText);
        }

        private void UartRichBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            string DislayedText=null;

            if (string.IsNullOrEmpty(SEL_CollectorConfigObj.txfilePath) && string.IsNullOrEmpty(SEL_CollectorConfigObj.rxfilePath))
            {
                MessageBox.Show("Please select a file path.");
                return;
            }
            else
            {
                AppendColoredText(UartRichBox, " ", Color.Orange);
               

                if (e.KeyChar == (char)Keys.Enter)
                {
                    DislayedText = Environment.NewLine + " at " + DateTime.Now.ToString() + " :: ";

                    // Write newline character to the serial port
                    AppendColoredText(UartRichBox, DislayedText, Color.Orange);

                    SEL_CollectorConfigObj.WriteOnTxLogFile(DislayedText);

                   
                }
                else
                {
                    // Write the pressed character to the serial port
                    char[] ch = new char[1];
                    ch[0] = e.KeyChar;


                    SEL_CollectorConfigObj.SendSerialData(e.KeyChar.ToString(), ref PortExistOrNot, ref PortIsOpenOrNot, ref DislayedText);

                    SEL_CollectorConfigObj.WriteOnTxLogFile(DislayedText);
                   // SEL_CollectorConfigObj.UartActionsObj.MySerialPort.Write(ch, 0, 1);
                    //MySerialPort.Write(e.KeyChar.ToString());
                }

                SEL_CollectorConfigObj.SetVarReceivedOrNot(0);
            }
        }

        private void PortComboBox_MouseHover(object sender, EventArgs e)
        {

            string[] Ports = SEL_CollectorConfigObj.CheckSerialPorts(ref PortExistOrNot);

            if (PortExistOrNot)
            {
                UartRichBox.Enabled = true;

                foreach (string portName in Ports)
                {
                    if (!PortComboBox.Items.Contains(portName))
                    {
                        PortComboBox.Items.Add(portName);
                    }
                }

                if (PortComboBox.Items.Count > 0)
                {
                    PortComboBox.SelectedIndex = 0;
                    SEL_CollectorConfigObj.SetSerialConfig(PortComboBox.Text, BaudRateComboBox.Text, StopBitsComboBox.Text, DataBitsComboBox.Text, ParityComboBox.Text);

                    //Load Selected Port to Display it in TextBoxs
                    ComPortManualTextBox.Text = PortComboBox.Text;
                    ComPortAutomatocTextBox.Text = PortComboBox.Text;
                }
                else
                {
                    UartRichBox.Enabled = false;
                    PortComboBox.Text = " Select Port ";
                    // Handle case where no distinct COM ports are available
                    MessageBox.Show("No distinct COM ports available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                UartRichBox.Enabled = false;
                PortComboBox.Text = " Select Port ";
                // Handle case where no COM ports are available
                MessageBox.Show("No COM ports available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            string DislayedText;
            if (string.IsNullOrEmpty(SEL_CollectorConfigObj.txfilePath) && string.IsNullOrEmpty(SEL_CollectorConfigObj.rxfilePath))
            {
                MessageBox.Show("Please select a file path.");
                return;
            }
            else
            {
                //string[] Ports = SEL_CollectorConfigObj.CheckSerialPorts(ref PortExistOrNot);

               // if (PortExistOrNot)
               if(true) {
                    SEL_CollectorConfigObj.SetSerialConfig(loadedSettings.ComPort, loadedSettings.UART_BaudRate.ToString(), loadedSettings.UART_StopBits.ToString(), loadedSettings.UART_DataBits.ToString(), loadedSettings.UART_Parity);

                    UartRichBox.Enabled = true;
                    Start.Enabled = false;
                    RefrechButton.Enabled = false;
                    StopButton.Enabled = true;
                    ManualPage.Enabled = false;

                    if (SEL_CollectorConfigObj.OpenSerialConnection())
                    {
                        
                        DislayedText = Environment.NewLine + " at " + DateTime.Now.ToString() + " :: " + "COM Port Openned Succesfully";
                        DislayedText += Environment.NewLine + " at " + DateTime.Now.ToString() + " :: ";
                        
                        //to color the added Text
                        AppendColoredText(UartRichBox, DislayedText, Color.Green);
                        label_com.Visible = false;
                       
                       
                    }
                    else
                    {
                        DislayedText = " at " + DateTime.Now.ToString() + " :: UART Port Busy for other Running Task";
                        DislayedText += Environment.NewLine + " at " + DateTime.Now.ToString() + " :: ";
                        AppendColoredText(UartRichBox, DislayedText, Color.Red);
                        label_com.Visible = true;
                    }
                    // Create a new thread and pass the method you want to execute
                    thread = new Thread(SEL_CollectorConfigObj.Mainthread);
                    thread.Start();
                    SEL_CollectorConfigObj.Mainthread_Start();
                    SEL_CollectorConfigObj.WriteOnTxLogFile(DislayedText + Environment.NewLine);

                    SEL_CollectorConfigObj.WriteOnRxLogFile(DislayedText + Environment.NewLine);

                }
                else
                {
                    UartRichBox.Enabled = false;
                    PortComboBox.Text = " Select Port ";
                    // Handle case where no COM ports are available
                    MessageBox.Show("No COM ports available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }
        
        private void StopButton_Click(object sender, EventArgs e)
        {
            string DislayedText;
            if (string.IsNullOrEmpty(SEL_CollectorConfigObj.txfilePath) && string.IsNullOrEmpty(SEL_CollectorConfigObj.rxfilePath))
            {
                MessageBox.Show("Please select a file path.");
                return;
            }
            else
            {
                if (thread != null)
                {
                    thread.Abort();
                }

             
                    UartRichBox.Enabled = true;
                    SEL_CollectorConfigObj.SetVarReceivedOrNot(0);
                    Start.Enabled = true;
                    StopButton.Enabled = false;
                    RefrechButton.Enabled = true;
                    ManualPage.Enabled = true;


                    SEL_CollectorConfigObj.CloseSerialConnection();
                    
                        //to color the added Text
                        DislayedText = Environment.NewLine + " at " + DateTime.Now.ToString() + " :: " + "COM Port is Closing....";
                        DislayedText += Environment.NewLine + " at " + DateTime.Now.ToString() + " :: ";

                        AppendColoredText(UartRichBox, DislayedText, Color.Red);
                        SEL_CollectorConfigObj.Mainthread_Stop();
                        SEL_CollectorConfigObj.WriteOnTxLogFile(DislayedText + Environment.NewLine);

                        SEL_CollectorConfigObj.WriteOnRxLogFile(DislayedText + Environment.NewLine);
                    
                   
                   
                
               


            }




        }

        private void TxFileBrowseButton_Click(object sender, EventArgs e)
        {
            if (SEL_CollectorConfigObj.TxFileCreation())
            {
                TxFileTextBox.Text = SEL_CollectorConfigObj.txfilePath;
            }
            else
            {
                TxFileTextBox.Text = null;
            }
        }

        private void RxFileBrowseButton_Click(object sender, EventArgs e)
        {
            if (SEL_CollectorConfigObj.RxFileCreation())
            {
                RxFileTextBox.Text = SEL_CollectorConfigObj.rxfilePath;
            }
            else
            {
                RxFileTextBox.Text = null;
            }

        }

        private void ClearRichBoxButton_Click(object sender, EventArgs e)
        {
            UartRichBox.Clear();
        }

        private void RefrechButton_Click(object sender, EventArgs e)
        {
            RefrechButtun();
        }

        public void RefrechButtun()
        {

            if (string.IsNullOrEmpty(TxFileTextBox.Text) && string.IsNullOrEmpty(RxFileTextBox.Text))
            {
                MessageBox.Show("Please select a file path.");
                return;
            }
            else
            { 
            SEL_CollectorConfigObj.txfilePath = TxFileTextBox.Text;
            SEL_CollectorConfigObj.rxfilePath = RxFileTextBox.Text;

            //Load Data from ComboBox to TextBox
            GetwayTextBox.Text = GetwaySelectionComboBox.Text;

             //Load Selected Port to Display it in TextBoxs
             ComPortManualTextBox.Text = PortComboBox.Text;

             ComPortAutomatocTextBox.Text = PortComboBox.Text;

            //Load Data to Uart Serial Port
            SEL_CollectorConfigObj.SetSerialConfig(PortComboBox.Text, BaudRateComboBox.Text, StopBitsComboBox.Text, DataBitsComboBox.Text, ParityComboBox.Text);

            //save in Json File
            SEL_CollectorConfigObj.LoadFromGUIToJson(PortComboBox.Text, BaudRateComboBox.Text, DataBitsComboBox.Text, StopBitsComboBox.Text, GetwaySelectionComboBox.Text, ParityComboBox.Text, TxFileTextBox.Text, RxFileTextBox.Text);

            }
        }

        public void PopulateComboBox(ComboBox comboBox)
        {
            // Call your Select function to get the DataTable
            DataTable dt = SEL_GetwayObj.GetwayNum();

            // Check if the DataTable has data and contains the "Getway_Number" column
            if (dt != null && dt.Columns.Contains("Getway_Number"))
            {
                // Clear existing items in the ComboBox
                comboBox.Items.Clear();

                // Loop through the rows and add each "Getway_Number" to the ComboBox
                foreach (DataRow row in dt.Rows)
                {
                    comboBox.Items.Add(row["Getway_Number"].ToString());
                }
            }
            else
            {
               // MessageBox.Show("Error: Unable to retrieve data from the database.");
            }
        }

        private static void AppendColoredText(RichTextBox box, string text, Color color)
        {
            int start = box.TextLength;
            box.AppendText(text);
            int end = box.TextLength;

            // Select the newly added text
            box.Select(start, end - start);
            {
                // Apply the color
                box.SelectionColor = color;

            }
            box.ScrollToCaret(); // Optionally scroll to the bottom after appending text
        }



        #region Manual Part
        private void MeterNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            // Set the MeterNumberVar property of SEL_CollectorConfigObj to the text in MeterNumberTextBox
            SEL_CollectorConfigObj.MeterNumberVar = MeterNumberTextBox.Text;

            // Check if both MeterNumberVar and MeterAggregationNumberVar are not null or empty before processing
            if (!string.IsNullOrEmpty(SEL_CollectorConfigObj.MeterNumberVar) &&
                !string.IsNullOrEmpty(SEL_CollectorConfigObj.MeterAggregationNumberVar))
            {
                // Declare a string variable to hold the concatenated data
                string SendDataDisplay = "";

                // Call the ManualConcatinatedData method to concatenate data and update SendDataDisplay
                SEL_CollectorConfigObj.ManualConcatinatedData(SEL_CollectorConfigObj.MeterNumberVar, SEL_CollectorConfigObj.MeterAggregationNumberVar, ref SendDataDisplay);

                // Update the text of SendManualTextBox with SendDataDisplay
                SendManualTextBox.Text = SendDataDisplay;
            }
            else
            {
                // If either MeterNumberVar or MeterAggregationNumberVar is null or empty, clear SendManualTextBox
                SendManualTextBox.Text = string.Empty;
            }
        }

        private void MeterAggregationNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            // Set the MeterAggregationNumberVar property of SEL_CollectorConfigObj to the text in MeterAggregationNumberTextBox
            SEL_CollectorConfigObj.MeterAggregationNumberVar = MeterAggregationNumberTextBox.Text;

            // Check if both MeterNumberVar and MeterAggregationNumberVar are not null or empty before processing
            if (!string.IsNullOrEmpty(SEL_CollectorConfigObj.MeterNumberVar) &&
                !string.IsNullOrEmpty(SEL_CollectorConfigObj.MeterAggregationNumberVar))
            {
                // Declare a string variable to hold the concatenated data
                string SendDataDisplay = "";

                // Call the ManualConcatinatedData method to concatenate data and update SendDataDisplay
                SEL_CollectorConfigObj.ManualConcatinatedData(SEL_CollectorConfigObj.MeterNumberVar, SEL_CollectorConfigObj.MeterAggregationNumberVar, ref SendDataDisplay);

                // Update the text of SendManualTextBox with SendDataDisplay
                SendManualTextBox.Text = SendDataDisplay;
            }
            else
            {
                // If either MeterNumberVar or MeterAggregationNumberVar is null or empty, clear SendManualTextBox
                SendManualTextBox.Text = string.Empty;
            }
        }

        private void SendManualButton_Click(object sender, EventArgs e)
        {
            // Retrieve meter number, aggregation number, and send data from SEL_CollectorConfigObj
            string meterNumber = SEL_CollectorConfigObj.MeterNumberVar;
            string aggregationNumber = SEL_CollectorConfigObj.MeterAggregationNumberVar;
            string sendData = SendManualTextBox.Text;

            // Check if SendManualTextBox is empty
            if (string.IsNullOrEmpty(sendData))
            {
                // If SendManualTextBox is empty, display a warning message
                MessageBox.Show("Warning: Meter number or Aggregation number, or both may be null.");
                return; // Exit the method
            }

            // Call the ManualSendRecieveData method with the retrieved data
            SEL_CollectorConfigObj.ManualSendRecieveData(meterNumber, aggregationNumber, sendData);
        }

        private void StartMan_Click(object sender, EventArgs e)
        {
            string DislayedText;
            if (string.IsNullOrEmpty(SEL_CollectorConfigObj.txfilePath) && string.IsNullOrEmpty(SEL_CollectorConfigObj.rxfilePath))
            {
                MessageBox.Show("Please select a file path.");
                return;
            }
            else
            {
                string[] Ports = SEL_CollectorConfigObj.CheckSerialPorts(ref PortExistOrNot);

                if (PortExistOrNot)
                {
                    SEL_CollectorConfigObj.SetSerialConfig(PortComboBox.Text, BaudRateComboBox.Text, StopBitsComboBox.Text, DataBitsComboBox.Text, ParityComboBox.Text);

                    UartRichBoxMan.Enabled = true;
                    StartMan.Enabled = false;
                    RefrechButton.Enabled = false;
                    StopMan.Enabled = true;
                    AutomaticPage.Enabled = false;

                    if (SEL_CollectorConfigObj.OpenSerialConnection())
                    {

                        DislayedText = Environment.NewLine + " at " + DateTime.Now.ToString() + " :: " + "COM Port Openned Succesfully";
                        DislayedText += Environment.NewLine + " at " + DateTime.Now.ToString() + " :: ";

                        //to color the added Text
                        AppendColoredText(UartRichBoxMan, DislayedText, Color.Green);

                        label_com.Visible = false;

                        //  SEL_CollectorConfigObj.Mainthread_Start();

                    }
                    else
                    {
                        DislayedText = " at " + DateTime.Now.ToString() + " :: UART Port Busy for other Running Task";
                        DislayedText += Environment.NewLine + " at " + DateTime.Now.ToString() + " :: ";
                        AppendColoredText(UartRichBoxMan, DislayedText, Color.Red);
                        label_com.Visible = true;
                    }
                    SEL_CollectorConfigObj.WriteOnTxLogFile(DislayedText + Environment.NewLine);

                    SEL_CollectorConfigObj.WriteOnRxLogFile(DislayedText + Environment.NewLine);

                }
                else
                {
                    UartRichBoxMan.Enabled = false;
                    PortComboBox.Text = " Select Port ";
                    // Handle case where no COM ports are available
                    MessageBox.Show("No COM ports available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void StopMan_Click(object sender, EventArgs e)
        {
            string DislayedText;

            if (string.IsNullOrEmpty(SEL_CollectorConfigObj.txfilePath) && string.IsNullOrEmpty(SEL_CollectorConfigObj.rxfilePath))
            {
                MessageBox.Show("Please select a file path.");
                return;
            }
            else
            {
                //thread.Abort();

                string[] Ports = SEL_CollectorConfigObj.CheckSerialPorts(ref PortExistOrNot);

                if (PortExistOrNot)
                {
                    UartRichBoxMan.Enabled = true;
                    SEL_CollectorConfigObj.SetVarReceivedOrNot(0);
                    StartMan.Enabled = true;
                    StopMan.Enabled = false;
                    RefrechButton.Enabled = true;
                    AutomaticPage.Enabled = true;


                    if (SEL_CollectorConfigObj.CloseSerialConnection())
                    {
                        //to color the added Text
                        DislayedText = Environment.NewLine + " at " + DateTime.Now.ToString() + " :: " + "COM Port is Closing....";
                        DislayedText += Environment.NewLine + " at " + DateTime.Now.ToString() + " :: ";

                        AppendColoredText(UartRichBoxMan, DislayedText, Color.Red);
                        // SEL_CollectorConfigObj.Mainthread_Stop();
                        SEL_CollectorConfigObj.WriteOnTxLogFile(DislayedText + Environment.NewLine);

                        SEL_CollectorConfigObj.WriteOnRxLogFile(DislayedText + Environment.NewLine);
                    }


                }
                else
                {
                    UartRichBoxMan.Enabled = false;
                    PortComboBox.Text = " Select Port ";
                    // Handle case where no COM ports are available
                    MessageBox.Show("No COM ports available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }

        }
        #endregion


    }
}

