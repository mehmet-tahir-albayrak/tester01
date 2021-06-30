using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;





namespace Tester01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ScanofPorts.Enabled = true;
        }

        enum Commands
        {
            HostPrefixA     = 0xc2,
            HostPrefixB     = 0xb1,
            DevicePrefixA   = 0x81,
            DevicePrefixB   = 0x12,

            UsbCmd = 0xA6,
            UsbGetStatus = 0x11
        }

        enum Cmd
        {
            GetVersion = 0,
            GetStatus
        }

        UInt32 Crc32(UInt32 Crc, UInt32 Data)
        {
            int i;
            Crc = Crc ^ Data;
            for (i = 0; i < 32; i++)
                if ((Crc & 0x80000000) == 0x80000000)
                    Crc = (Crc << 1) ^ 0x04C11DB7; // Polynomial used in STM32
                else
                    Crc = (Crc << 1);
            return (Crc);
        }

        byte[] CalcCRC(byte[] data, int length)
        {
            byte[] crc_data = new byte[length - 2];
            int crc_length = length - 2;

            Array.Copy(data, 2, crc_data, 0, crc_length);

            UInt32 Crc = 0xFFFFFFFF;
            UInt32 i_data;
            for (int i = 0; i < crc_length / 4; ++i)
            {
                i_data = BitConverter.ToUInt32(crc_data, i * 4);
                Crc = Crc32(Crc, i_data);
            }
            return (BitConverter.GetBytes(Crc));
        }

        bool GetCommand(SerialPort sp, Commands cmd, Commands subCmd, out int length,out byte[] data)
        {
            data = new byte[1]; length = 0;
            try
            {
               data = new byte[1];

               length = sp.BytesToRead;

                byte[] rsp = new byte[length];
                if (sp.Read(rsp, 0, length) < 9) return false;

                for (int i = 0; i < length - 2; i++)
                {
                    if ((rsp[i] == 0x81) & (rsp[i + 1] == 0x12))
                    {
                        if (rsp[i + 2] == (byte)cmd)
                        {
                            if (rsp[i + 3] == (byte)subCmd)
                            {
                                length = rsp[i+4]; data = new byte[length];
                                Array.Copy(rsp, i + 5, data, 0, length);
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        bool SendCommand(SerialPort sp,Commands cmd,Commands subCmd,byte length,byte[] parameters)
        {
            try
            {
                byte[] sendBuffer = new byte[length + 9];
                sendBuffer[0] = (byte)Commands.HostPrefixA;
                sendBuffer[1] = (byte)Commands.HostPrefixB;
                sendBuffer[2] = (byte)cmd;
                sendBuffer[3] = (byte)subCmd;
                sendBuffer[4] = length;
                if(parameters.Length >= length)
                    Array.Copy(parameters, 0, sendBuffer,5,length);

                byte[] crc = CalcCRC(sendBuffer, 5+length);

                Array.Copy(crc, 0, sendBuffer, 5+length, 4);

                sp.Write(sendBuffer, 0, sendBuffer.Length);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string GetVersion(SerialPort sp)
        {
            try
            {
                SendCommand(sp, Commands.UsbCmd, Commands.UsbGetStatus, 0, new byte[0]);
                
                System.Threading.Thread.Sleep(500);

                int length = 0; byte[] rsp;
                if (GetCommand(sp, Commands.UsbCmd, Commands.UsbGetStatus, out length, out rsp))
                {
                    string Version = $"Version : {rsp[2].ToString()}.{rsp[1].ToString()}";
                    return Version;
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        string vaccinerefport = "";
        private void SearchVaccineRef(object sender, EventArgs e)
        {

            string[] ports = SerialPort.GetPortNames();
            if (ports.Length>0)
            {
                foreach (string port in ports)
                {
                    if (port == vaccinerefport)
                    {
                        pbMain.Value = 100;
                        return;
                    }
                    pbMain.Value = 0;
                    VaccPort = new SerialPort(port,115200,Parity.None,8,StopBits.One);
                    VaccPort.Open();
                    if (VaccPort.IsOpen)
                    {
                        tsLblMain.Text = GetVersion(VaccPort);
                        if (tsLblMain.Text != "")
                        {
                            pbMain.Value = 100;
                            vaccinerefport = port;
                        }
                    }
                    VaccPort.Close();
                }
            }
            else
            {
                tsLblMain.Text = "Trying to connect to refrigereator...";
                pbMain.Value = 0;
                vaccinerefport = "";
            }
        }
    }
}
