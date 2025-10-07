using System;
using System.IO.Ports;
using System.Management;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using GarageSystem;
using WindowsFormsApp1;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
namespace Serializer_pro
{
    public partial class Serial_Visualizer : Form
    {
        private Dictionary<string, string> portDescriptions = new Dictionary<string, string>();

        private SerialConnection _TxserialConnection;
   
        private SerialConnection _RxserialConnection;
        private Image CloseConnectionImage;
        private Image OpenConnectionImage;

        public Serial_Visualizer()
        {
            InitializeComponent();
            LoadSequenceNumber();
            Initialize_GUI();
        }
        private void LoadSerialSettings()
        {
            // Load detailed port descriptions
            portDescriptions = GetPortDescriptions();

            // Initially populate both combo boxes
            PopulatePortComboBoxes();

            // Event handlers to filter ports
            TxPortComboBox.SelectedIndexChanged += TxPortComboBox_SelectedIndexChanged;
            RxPortComboBox.SelectedIndexChanged += RxPortComboBox_SelectedIndexChanged;

            // Fill Baud Rate options
            BaudRateComboBox.Items.AddRange(new object[] { 9600, 19200, 38400, 57600, 115200 });
            BaudRateComboBox.SelectedIndex = 0;

            // Fill Data Bits
            DataBitsComboBox.Items.AddRange(new object[] { 5, 6, 7, 8 });
            DataBitsComboBox.SelectedIndex = 0;

            // Fill Parity options
            ParityComboBox.DataSource = Enum.GetValues(typeof(Parity));
            ParityComboBox.SelectedItem = Parity.None;

            // Fill Stop Bits options
            StopBitsComboBox.DataSource = Enum.GetValues(typeof(StopBits));
            StopBitsComboBox.SelectedItem = StopBits.One;
        }

        private Dictionary<string, string> GetPortDescriptions()
        {
            var descriptions = new Dictionary<string, string>();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%(COM%'"))
            {
                foreach (var obj in searcher.Get())
                {
                    string name = obj["Name"]?.ToString();
                    if (!string.IsNullOrEmpty(name))
                    {
                        int start = name.IndexOf("(COM");
                        int end = name.IndexOf(")", start);
                        if (start >= 0 && end > start)
                        {
                            string port = name.Substring(start + 1, end - start - 1); // e.g. "COM27"
                            descriptions[port] = name;
                        }
                    }
                }
            }
            return descriptions;
        }

        private void PopulatePortComboBoxes()
        {
            // Prevent recursive events
            TxPortComboBox.SelectedIndexChanged -= TxPortComboBox_SelectedIndexChanged;
            RxPortComboBox.SelectedIndexChanged -= RxPortComboBox_SelectedIndexChanged;

            string selectedTx = TxPortComboBox.SelectedItem?.ToString();
            string selectedRx = RxPortComboBox.SelectedItem?.ToString();

            TxPortComboBox.Items.Clear();
            RxPortComboBox.Items.Clear();

            foreach (var pair in portDescriptions)
            {
                string fullName = pair.Value;
                string port = pair.Key;

                if (port != GetSelectedPort(RxPortComboBox))
                    TxPortComboBox.Items.Add(fullName);

                if (port != GetSelectedPort(TxPortComboBox))
                    RxPortComboBox.Items.Add(fullName);
            }

            // Restore previous selection if possible
            if (!string.IsNullOrEmpty(selectedTx) && TxPortComboBox.Items.Contains(selectedTx))
                TxPortComboBox.SelectedItem = selectedTx;
            else if (TxPortComboBox.Items.Count > 0)
                TxPortComboBox.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(selectedRx) && RxPortComboBox.Items.Contains(selectedRx))
                RxPortComboBox.SelectedItem = selectedRx;
            else if (RxPortComboBox.Items.Count > 0)
                RxPortComboBox.SelectedIndex = 0;

            // Reattach events
            TxPortComboBox.SelectedIndexChanged += TxPortComboBox_SelectedIndexChanged;
            RxPortComboBox.SelectedIndexChanged += RxPortComboBox_SelectedIndexChanged;
        }

        private string GetSelectedPort(ComboBox comboBox)
        {
            if (comboBox.SelectedItem == null) return null;
            string text = comboBox.SelectedItem.ToString();
            int start = text.IndexOf("(COM");
            int end = text.IndexOf(")", start);
            return (start >= 0 && end > start) ? text.Substring(start + 1, end - start - 1) : null;
        }
        // Load available ports and populate combo boxes
        /*
         * private void LoadSerialSettings()
        {
            // Fill Port ComboBox
            TxPortComboBox.Items.AddRange(SerialPort.GetPortNames());
            if (TxPortComboBox.Items.Count > 0) TxPortComboBox.SelectedIndex = 0;

            // Fill Port ComboBox
            RxPortComboBox.Items.AddRange(SerialPort.GetPortNames());
            if (RxPortComboBox.Items.Count > 0) RxPortComboBox.SelectedIndex = 0;

            // Fill Baud Rate options
            BaudRateComboBox.Items.AddRange(new object[] { 9600, 19200, 38400, 57600, 115200 });
            BaudRateComboBox.SelectedIndex = 0;

            // Fill Data Bits
            DataBitsComboBox.Items.AddRange(new object[] { 5, 6, 7, 8 });
            DataBitsComboBox.SelectedIndex = 0;

            // Fill Parity options
            ParityComboBox.DataSource = Enum.GetValues(typeof(Parity));
            ParityComboBox.SelectedItem = Parity.None;

            // Fill Stop Bits options
            StopBitsComboBox.DataSource = Enum.GetValues(typeof(StopBits));
            StopBitsComboBox.SelectedItem = StopBits.One;
        }
        */

        private void InitializeSerialConnection()
        {
            try
            {
                string txPort = GetSelectedPort(TxPortComboBox);
                string rxPort = GetSelectedPort(RxPortComboBox);

                if (string.IsNullOrEmpty(txPort) || string.IsNullOrEmpty(rxPort))
                {
                    MessageBox.Show("Both TX and RX ports must be selected.");
                    return;
                }

                var Txbll = new BLL_SerialConnection
                {
                    SerialConnection_PortName = txPort,
                    SerialConnection_BaudRate = Convert.ToInt32(BaudRateComboBox.SelectedItem),
                    SerialConnection_DataBits = Convert.ToInt32(DataBitsComboBox.SelectedItem),
                    SerialConnection_Parity = (Parity)ParityComboBox.SelectedItem,
                    SerialConnection_StopBits = (StopBits)StopBitsComboBox.SelectedItem
                };

                var Rxbll = new BLL_SerialConnection
                {
                    SerialConnection_PortName = rxPort,
                    SerialConnection_BaudRate = Convert.ToInt32(BaudRateComboBox.SelectedItem),
                    SerialConnection_DataBits = Convert.ToInt32(DataBitsComboBox.SelectedItem),
                    SerialConnection_Parity = (Parity)ParityComboBox.SelectedItem,
                    SerialConnection_StopBits = (StopBits)StopBitsComboBox.SelectedItem
                };

                _TxserialConnection = new SerialConnection(Txbll);
                //_TxserialConnection.DataReceived += TxSerialConnection_DataReceived;

                _RxserialConnection = new SerialConnection(Rxbll);
                _RxserialConnection.DataReceived += RxSerialConnection_DataReceived;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing serial connection: " + ex.Message);
            }
        }


        /*
        private void InitializeSerialConnection()
        {
            try
            {
                var Txbll = new BLL_SerialConnection
                {
                    SerialConnection_PortName = TxPortComboBox.SelectedItem.ToString(),
                    SerialConnection_BaudRate = Convert.ToInt32(BaudRateComboBox.SelectedItem),
                    SerialConnection_DataBits = Convert.ToInt32(DataBitsComboBox.SelectedItem),
                    SerialConnection_Parity = (Parity)ParityComboBox.SelectedItem,
                    SerialConnection_StopBits = (StopBits)StopBitsComboBox.SelectedItem
                };

                var Rxbll = new BLL_SerialConnection
                {
                    SerialConnection_PortName = RxPortComboBox.SelectedItem.ToString(),
                    SerialConnection_BaudRate = Convert.ToInt32(BaudRateComboBox.SelectedItem),
                    SerialConnection_DataBits = Convert.ToInt32(DataBitsComboBox.SelectedItem),
                    SerialConnection_Parity = (Parity)ParityComboBox.SelectedItem,
                    SerialConnection_StopBits = (StopBits)StopBitsComboBox.SelectedItem
                };

                _TxserialConnection = new SerialConnection(Txbll);
              //  _TxserialConnection.DataReceived += TxSerialConnection_DataReceived;

                _RxserialConnection = new SerialConnection(Rxbll);
                _RxserialConnection.DataReceived += RxSerialConnection_DataReceived;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing serial connection: " + ex.Message);
            }
        }

        */

        // Declare this globally in your class to accumulate incoming data
        private List<byte> receiveBuffer = new List<byte>();
        uint kofreID = 0, deviceID = 0;
        double mainSupplyCurrent = 0, mainSupplyCurrentTemp = 0,  meterCurrent = 0;
        bool dataValid = false;
        private void RxSerialConnection_DataReceived(byte[] data)
        {
        
            Invoke((MethodInvoker)delegate
            {
                // Append to the accumulated buffer
                receiveBuffer.AddRange(data);

                const int expectedMessageLength = 256;

                while (receiveBuffer.Count >= expectedMessageLength)
                {
                    byte[] messageBytes = receiveBuffer.Take(expectedMessageLength).ToArray();
                    receiveBuffer.RemoveRange(0, expectedMessageLength);

                    string hexString = BitConverter.ToString(messageBytes).Replace("-", " ");
                    //Console.WriteLine("Received (HEX): " + hexString);
                    System.Diagnostics.Debug.WriteLine("Received (HEX): " + hexString);
                    //MessageBox.Show("Received (HEX): " + hexString);

                    try
                    {
                        if (messageBytes.Length >= 110) // enough for index 59 + 2 bytes
                        {
                            if (messageBytes[13] == (byte)5 && messageBytes[1]== (byte)2)
                            {
                                if (messageBytes[106] != 0 || messageBytes[107] != 0 || messageBytes[108] != 0 )
                                {
                                    dataValid = true;

                                    // 4-byte unsigned int (big-endian)
                                    kofreID = (uint)((messageBytes[10] << 24) |
                                                     (messageBytes[11] << 16) |
                                                     (messageBytes[12] << 8) |
                                                     messageBytes[13]);

                                    // Integer part
                                    int intPart = (messageBytes[40] * 100) + (messageBytes[41] * 10) + messageBytes[42];

                                    // Fractional part (assuming byte 42 contains fractional in hundredths)
                                    int fracPart = (messageBytes[43] * 10) + messageBytes[44];

                                    // Combine into one number (e.g., 25 + 34 → 25.34)

                                    mainSupplyCurrentTemp = intPart + (fracPart / 100.0);
                                }
                                else
                                {
                                    dataValid = false;
                                }

                                 
                            }
                            else if (messageBytes[13] == (byte)2 && messageBytes[1] == (byte)2)
                            {

                                if (messageBytes[106] != 0 || messageBytes[107] != 0 || messageBytes[108] != 0)
                                {
                                    deviceID = (uint)((messageBytes[10] << 24) |
                                                      (messageBytes[11] << 16) |
                                                      (messageBytes[12] << 8) |
                                                      messageBytes[13]);

                                    /* meterCurrent = (ushort)((messageBytes[59] << 8) |
                                                              messageBytes[60]);
                                     meterCurrent /= 100;*/
                                    ushort rawMeterCurrent = (ushort)((messageBytes[59] << 8) | messageBytes[60]);


                                    if (dataValid)
                                    {

                                        // divide by 100 to scale
                                        meterCurrent = rawMeterCurrent / 100.0;
                                        
                                        // keep only 2 decimal places
                                        meterCurrent = Math.Round(meterCurrent, 2);

                                        mainSupplyCurrent = mainSupplyCurrentTemp;
                            
                                    }
                                   

                                }
                              
                                dataValid = false;
                                


                            }

                            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string theftStatus = "NO";
                            if  (mainSupplyCurrent >= meterCurrent+1 || mainSupplyCurrent < meterCurrent - 1)
                            {
                                 theftStatus = "YES";
                            }
                            
                         

                            KofreIDTextBox.Text = kofreID.ToString();
                            DeviceIDTextBox.Text = deviceID.ToString();
                            MSupplyCurrentTextBox.Text = mainSupplyCurrent.ToString();
                            MeterCurrentTextBox.Text = meterCurrent.ToString();
                            TimestampTextBox.Text = timestamp;
                            TheftStatusTextBox.Text = theftStatus;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error parsing message: " + ex.Message);
                    }
                }
            });
        }

        /*
                private void RxSerialConnection_DataReceived(string data)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        // Convert incoming ASCII string to byte array
                        byte[] rawBytes = Encoding.Default.GetBytes(data);

                        // Append to the accumulated buffer
                        receiveBuffer.AddRange(rawBytes);

                        // Set expected message length (adjust if your message format changes)
                        const int expectedMessageLength = 255;

                        // Process all complete messages
                        while (receiveBuffer.Count >= expectedMessageLength)
                        {
                            byte[] messageBytes = receiveBuffer.Take(expectedMessageLength).ToArray();
                            receiveBuffer.RemoveRange(0, expectedMessageLength);

                            // Convert to hex string for display/debugging
                            string hexString = BitConverter.ToString(messageBytes).Replace("-", " ");
                            MessageBox.Show("Received (HEX): " + hexString);

                            try
                            {
                                // Safe field parsing
                                if (messageBytes.Length >= 51) // Must have at least up to byte 50
                                {
                                    int kofreID = BitConverter.ToInt32(messageBytes, 0);        // bytes 0 to 3
                                    int deviceID = BitConverter.ToInt32(messageBytes, 10);      // bytes 10 to 13
                                    int mainSupplyCurrent = BitConverter.ToInt32(messageBytes, 30); // bytes 30 to 33
                                    int meterCurrent = BitConverter.ToInt16(messageBytes, 49);      // bytes 49 to 50

                                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    string theftStatus = (mainSupplyCurrent != meterCurrent) ? "YES" : "NO";

                                    // Update UI
                                    KofreIDTextBox.Text = kofreID.ToString();
                                    DeviceIDTextBox.Text = deviceID.ToString();
                                    MSupplyCurrentTextBox.Text = mainSupplyCurrent.ToString();
                                    MeterCurrentTextBox.Text = meterCurrent.ToString();
                                    TimestampTextBox.Text = timestamp;
                                    TheftStatusTextBox.Text = theftStatus;
                                }
                                else
                                {
                                    MessageBox.Show("Data too short to parse required fields.");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error parsing message: " + ex.Message);
                            }
                        }
                    });
                }
                */
        /*
        private void RxSerialConnection_DataReceived(string data)
        {
            Invoke((MethodInvoker)delegate
            {
                // Convert ASCII string into raw byte array
                byte[] rawBytes = Encoding.Default.GetBytes(data);  // Or use UTF8 if you're sure

                // Convert to HEX string for display
                string hexString = BitConverter.ToString(rawBytes).Replace("-", " ");
                MessageBox.Show("Received (HEX): " + hexString);

                if (rawBytes.Length < 4)
                {
                    MessageBox.Show("Invalid data length received!");
                    return;
                }

                int kofreID = BitConverter.ToInt32(rawBytes, 0);            // bytes 0 to 3
                int deviceID = BitConverter.ToInt32(rawBytes, 10);          // bytes 10 to 13
                int mainSupplyCurrent = BitConverter.ToInt32(rawBytes, 30); // bytes 30 to 33
                int meterCurrent = BitConverter.ToInt16(rawBytes, 49);      // bytes 49 to 50


                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string theftStatus = (mainSupplyCurrent != meterCurrent) ? "YES" : "NO";

                KofreIDTextBox.Text = kofreID.ToString();
                DeviceIDTextBox.Text = deviceID.ToString();
                MSupplyCurrentTextBox.Text = mainSupplyCurrent.ToString();
                MeterCurrentTextBox.Text = meterCurrent.ToString();
                TimestampTextBox.Text = timestamp;
                TheftStatusTextBox.Text = theftStatus;
            });
        }
        */

        /*private void RxSerialConnection_DataReceived(string data)
        {
            Invoke((MethodInvoker)delegate
            {
                MessageBox.Show(data);
                // Convert received ASCII string to raw bytes
                byte[] rawBytes = Encoding.ASCII.GetBytes(data);

                if (rawBytes.Length < 4)
                {
                    MessageBox.Show("Invalid data length received!");
                    return;
                }

                // Each byte now represents the actual value sent (e.g., 30, 41, 55, 32)
                int kofreID = rawBytes[0];            // byte value of first char (030)
                int deviceID = rawBytes[1];           // (041) = ASCII ')'
                int mainSupplyCurrent = rawBytes[2];  // (055) = ASCII '7'
                int meterCurrent = rawBytes[3];       // (032) = ASCII space

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string theftStatus = (mainSupplyCurrent != meterCurrent) ? "YES" : "NO";

                // Update the UI
                KofreIDTextBox.Text = kofreID.ToString();
                DeviceIDTextBox.Text = deviceID.ToString();
                MSupplyCurrentTextBox.Text = mainSupplyCurrent.ToString();
                MeterCurrentTextBox.Text = meterCurrent.ToString();
                TimestampTextBox.Text = timestamp;
                TheftStatusTextBox.Text = theftStatus;
            });
        }

        */

        private void Serial_Visualizer_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSequenceNumber();
            _TxserialConnection?.Dispose();
            _RxserialConnection?.Dispose();
        }

        private void StartMan_Click(object sender, EventArgs e)
        {
          
            InitializeSerialConnection();
            pictureBox1.Image = OpenConnectionImage;
            _RxserialConnection?.OpenConnection();
            _TxserialConnection?.OpenConnection();

            StopMan.Enabled = true;
            SendButton.Enabled = true;
            AutomaticSendButton.Enabled = true;
            StartMan.Enabled = false;
        }

        private void StopMan_Click(object sender, EventArgs e)
        {
            StopMan.Enabled = false;
            SendButton.Enabled = false;
            AutomaticSendButton.Enabled = false;
            StartMan.Enabled = true;
            pictureBox1.Image = CloseConnectionImage;
            _RxserialConnection?.CloseConnection();
            _TxserialConnection?.CloseConnection();
        }


       private void Initialize_GUI()
        {
           
            try
            {
                string basePath = Application.StartupPath;
                string openImagePath = Path.Combine(basePath, "images_used", "open connection.png");
                string closeImagePath = Path.Combine(basePath, "images_used", "close connection.png");

                OpenConnectionImage = Image.FromFile(openImagePath);
                CloseConnectionImage = Image.FromFile(closeImagePath);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading images: " + ex.Message);
            }
            pictureBox1.Image = CloseConnectionImage; // Set default
            StopMan.Enabled = false;
            LoadSerialSettings();
        }

        private void SendPacket(byte[] packet)
        {
            _TxserialConnection?.SendSerial(packet);
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string textToSend = SendTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(textToSend))
            {
                try
                {
                    // Convert space-separated hex string to byte array
                    string[] hexValues = textToSend.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    byte[] dataBytes = hexValues.Select(hex => Convert.ToByte(hex, 16)).ToArray();

                    _TxserialConnection?.SendSerial(dataBytes); // Assume SendSerial(byte[]) is implemented
                    SendTextBox.Clear();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Invalid HEX format. Please enter space-separated hex bytes like: 00 FF 12 ...");
                }
            }
        }

        /*

        private void SendButton_Click(object sender, EventArgs e)
        {
            string textToSend = SendTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(textToSend))
            {
                _TxserialConnection?.SendSerial(textToSend);
                SendTextBox.Clear();
            }
        }*/

        private void TxPortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePortComboBoxes(); // Refresh both, excluding selected TX
        }

        private void RxPortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePortComboBoxes(); // Refresh both, excluding selected RX
        }

        private void TxPortComboBox_DropDown(object sender, EventArgs e)
        {
            RefreshPortList(excludePort: GetSelectedPort(RxPortComboBox), targetComboBox: TxPortComboBox);
        }

        private void RxPortComboBox_DropDown(object sender, EventArgs e)
        {
            RefreshPortList(excludePort: GetSelectedPort(TxPortComboBox), targetComboBox: RxPortComboBox);
        }
        private void RefreshPortList(string excludePort, ComboBox targetComboBox)
        {
            var currentDescriptions = GetPortDescriptions();

            string previousSelection = targetComboBox.SelectedItem?.ToString();
            string previousPort = GetSelectedPort(targetComboBox);

            targetComboBox.Items.Clear();

            foreach (var pair in currentDescriptions)
            {
                if (pair.Key != excludePort)
                    targetComboBox.Items.Add(pair.Value);
            }

            // Try to preserve selection if still valid
            if (!string.IsNullOrEmpty(previousSelection) && targetComboBox.Items.Contains(previousSelection))
                targetComboBox.SelectedItem = previousSelection;
            else if (targetComboBox.Items.Count > 0)
                targetComboBox.SelectedIndex = 0;

            // Update global portDescriptions for consistency
            portDescriptions = currentDescriptions;
        }
        /*

        private void AutomaticSendButton_Click(object sender, EventArgs e)
        {
            byte[] packet1 = new byte[]
            {
        0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x17, 0x00, 0x04, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x19, 0xBB, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            byte[] packet2 = new byte[]
            {
        0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x19, 0xBC, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            SendPacket(packet1);
            Thread.Sleep(10); // Optional delay between packets
            SendPacket(packet2);
        }

        */

        // Start value: FF 00 19 72 (big-endian)
        private uint sequenceNumber = 0xFF002E0A;
        private readonly string sequenceNumberFilePath = Path.Combine(Application.StartupPath, "sequence_number.txt");

        private void IncrementSequence()
        {
            sequenceNumber += 2;
            SaveSequenceNumber();
        }

        private void ApplySequenceNumber(byte[] packet)
        {
            // Big-endian storage
            packet[26] = (byte)((sequenceNumber >> 24) & 0xFF);
            packet[27] = (byte)((sequenceNumber >> 16) & 0xFF);
            packet[28] = (byte)((sequenceNumber >> 8) & 0xFF);
            packet[29] = (byte)(sequenceNumber & 0xFF);
        }

        private void SaveSequenceNumber()
        {
            try
            {
                File.WriteAllText(sequenceNumberFilePath, sequenceNumber.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to save sequence number: " + ex.Message);
            }
        }

        private void LoadSequenceNumber()
        {
            try
            {
                if (File.Exists(sequenceNumberFilePath))
                {
                    string content = File.ReadAllText(sequenceNumberFilePath);
                    if (uint.TryParse(content, out uint savedNumber))
                    {
                        sequenceNumber = savedNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to load sequence number: " + ex.Message);
            }
        }


        private CancellationTokenSource cts;
        private bool isRunning = false;

        // private void AutomaticSendButton_Click(object sender, EventArgs e)
        // {

        //     byte[] packet1 = new byte[]
        //{
        // 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x19, 0x72, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        //};

        //     byte[] packet2 = new byte[]
        //     {
        // 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x19, 0x72, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        // 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        //     };

        //     // Apply sequence number to packet1
        //     ApplySequenceNumber(packet2);
        //     SendPacket(packet2);

        //     // Increment for packet2
        //     IncrementSequence();
        //     ApplySequenceNumber(packet1);
        //     Thread.Sleep(5000);
        //     SendPacket(packet1);

        //     // Increment ready for next call
        //     IncrementSequence();
        // }

        private void AutomaticSendButton_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                StartBackgroundTask();
                AutomaticSendButton.Text = "Stop Auto Send";   // toggle button text
            }
            else
            {
                StopBackgroundTask();
                AutomaticSendButton.Text = "Start Auto Send";  // toggle button text
            }

            isRunning = !isRunning;
        }

        private void StartBackgroundTask()
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;

            Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        SendMyPackets();   // call your existing logic
                    }
                    catch (Exception ex)
                    {
                        // optional: handle errors
                        this.Invoke((MethodInvoker)(() =>
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }));
                    }

                    Thread.Sleep(20000); // wait 2 seconds before next run
                }
            }, token);
        }

        private void StopBackgroundTask()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
                cts = null;
            }
        }

        // 🔽 Your existing packet logic moved here 🔽
        private void SendMyPackets()
        {
            byte[] packet1 = new byte[]
            {
                 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x19, 0x72, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            byte[] packet2 = new byte[]
            {
                 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x04, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x19, 0x72, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            // Apply sequence number to packet2
            ApplySequenceNumber(packet2);
            SendPacket(packet2);

            // Increment for packet2
            IncrementSequence();
            ApplySequenceNumber(packet1);
            Thread.Sleep(5000); // keep your 5s delay between packets
            SendPacket(packet1);

            // Increment ready for next call
            IncrementSequence();
        }

    }
}
