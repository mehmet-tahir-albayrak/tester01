
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Tester01
{
	public partial class Form1 : Form
	{
		#region Variables
		bool BleDongleConnected = false;
		private string file_path;
		int memory_write_active = 0;
		UInt32 base_mem_address = 0;
		UInt32 ex_mem_write_addr = 0;
		int bytes_so_far_sent = 0;
		int get_new_packet = 0;
		int ex_len_to_read = 0;
		int last_packet_sent = 0;
		int page_counter = 0;
		int packet_counter = 0;
		int file_index = 0;
		int lenght_of_file = 0;
		int len_to_read = 0;
		byte[] file_byte;
		int sended_len = 0;
		int remaining_bytes = 0;
		bool thread_active;
		Queue<string> data_log_queue = new Queue<string>();
		static object Lock = new object();
		private HttpClient client = new HttpClient();
		private string device_uid = "D00130222113";
		private DateTime startTime;
		private int MAX_CHART2_INDEX = 100;

		int gsm_total_retry = 0;
		int gsm_total_success = 0;
		bool error = false;
		int statusDebugIndex = 0;
		bool ConnectBlink;
		bool BlePeripheralConnected = false;
        #endregion
        #region
        #endregion
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
			HostPrefixA = 0xc2,
			HostPrefixB = 0xb1,
			DevicePrefixA = 0x81,
			DevicePrefixB = 0x12,

			UsbCmd = 0xA6,
			UsbGetStatus = 0x11,
			USB_GET_THRESHOLD = 0x02,
			USB_SET_THRESHOLD = 0x03,
			USB_SET_SSID = 0x06,
			USB_GET_SSID = 0x07,

			USB_SET_UID = 0x04,
			USB_GET_UID = 0x05,
			EP_GET_WIFI_PASS = 0xC5,
			EP_SET_WIFI_PASS = 0xC6,
			USB_SET_PASSWORD = 0x08,
			USB_GET_PASSWORD = 0x09,

			USB_GET_BOARD_PARAMS = 0x0B,
			USB_SET_BOARD_PARAMS = 0x0C,
			USB_GET_LOG_TABLE = 0x12,
			USB_GET_DATA_LOG = 0x01,
			USB_CLEAR_LOG_TABLE = 0x13,
			USB_SYNC_EPOCH = 0x25,
			USB_GET_DATA_LOG_TABLE_LEN = 0x24,
			USB_GET_LOG_TABLE_LEN = 0x23,
			USB_GET_DATA_TABLE_LEN = 0x24
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

		bool GetResponse(SerialPort sp, Commands cmd, Commands subCmd, out int length, out byte[] data)
		{
			data = new byte[1]; length = 0; 
			try
			{
				data = new byte[1];
				if (sp.IsOpen == false)
					return false;

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
								length = rsp[i + 4]; data = new byte[length];
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

		bool GetResponse(SerialPort sp, Commands cmd, Commands subCmd, out byte[] data, out int logIndex)
		{
			data = new byte[1]; int loglength = 0; logIndex = 0;
			try
			{
				data = new byte[1];
				if (sp.IsOpen == false)
					return false;

				loglength = sp.BytesToRead;
				// Console.WriteLine(length.ToString()) ;
				byte[] rsp = new byte[loglength]; data = new byte[loglength];
				if (sp.Read(rsp, 0, loglength) < 9) return false;
				int j = 0;
				for (int i = 0; i < rsp.Length - 2; i++)
				{
					if ((rsp[i] == 0x81) & (rsp[i + 1] == 0x12))
					{
						if (rsp[i + 2] == (byte)cmd)
						{
							if (rsp[i + 3] == (byte)subCmd)
							{
								loglength = rsp[i + 4];
								Array.Copy(rsp, i + 5, data, logIndex, loglength);
								logIndex += loglength;
								if (rsp.Length > logIndex + 9)
								{
									i = logIndex + 5;
								}
								else
								{
									return true;
								}

							}
						}
					}
				}
				if (logIndex > 0)
				{
					return true;
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}

		bool GetDataLog(SerialPort sp, Commands cmd, Commands subCmd, out byte[] data, out int logIndex)
		{
			data = new byte[1]; int loglength = 0; logIndex = 0;
			try
			{
				data = new byte[1];
				if (sp.IsOpen == false)
					return false;

				loglength = sp.BytesToRead;
				// Console.WriteLine(length.ToString()) ;
				byte[] rsp = new byte[loglength]; data = new byte[loglength];
				if (sp.Read(rsp, 0, loglength) < 8) return false;
				int j = 0;
				for (int i = 0; i < rsp.Length - 2; i++)
				{
					if ((rsp[i] == 0x81) & (rsp[i + 1] == 0x12))
					{
						if (rsp[i + 2] == (byte)cmd)
						{
							if (rsp[i + 3] == (byte)subCmd)
							{
								loglength = rsp[i + 4];
								Array.Copy(rsp, i + 5, data, logIndex, loglength);
								logIndex += loglength;
								if (rsp.Length > logIndex + 9)
								{
									i = logIndex + 5;
								}
								else
								{
									return true;
								}

							}
						}
					}
				}
				if (logIndex > 0)
				{
					return true;
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}

		bool SendCommand(SerialPort sp, Commands cmd, Commands subCmd, byte length, byte[] parameters)
		{
			try
			{
				byte[] sendBuffer = new byte[length + 9];
				sendBuffer[0] = (byte)Commands.HostPrefixA;
				sendBuffer[1] = (byte)Commands.HostPrefixB;
				sendBuffer[2] = (byte)cmd;
				sendBuffer[3] = (byte)subCmd;
				sendBuffer[4] = length;

				if ((parameters != null))

					if (parameters.Length >= length)
						Array.Copy(parameters, 0, sendBuffer, 5, length);

				byte[] crc = CalcCRC(sendBuffer, 5 + length);

				Array.Copy(crc, 0, sendBuffer, 5 + length, 4);

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
				if (GetResponse(sp, Commands.UsbCmd, Commands.UsbGetStatus, out length, out rsp))
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
			if (ports.Length > 0)
			{
				foreach (string port in ports)
				{
					if (port == vaccinerefport)
					{
						pbMain.Value = 100;
						return;
					}
					pbMain.Value = 0;
					VaccPort = new SerialPort(port, 115200, Parity.None, 8, StopBits.One);
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
					VaccPort.Close(); //bu satır aktifken get set etmiyor
				}
			}
			else
			{
				tsLblMain.Text = "Trying to connect to refrigereator...";
				pbMain.Value = 0;
				vaccinerefport = "";
			}
		}

		private void btnSsıdGet_Click(object sender, EventArgs e)
		{
			VaccPort.Open();

			try
			{

				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_SSID, 0, new byte[0]);

				System.Threading.Thread.Sleep(500);

				int length = 0; byte[] rsp;


				if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_SSID, out length, out rsp))
				{

					String wifi_ssid = Encoding.ASCII.GetString(rsp, 0, 32);// sayısaldan çevirme

					textBox1.Text = (wifi_ssid); // sayısaldan çevirme

				}

			}
			catch (Exception ex)
			{

			}
			VaccPort.Close();

		}

		private void btnSsıdSet_Click(object sender, EventArgs e)
		{
			VaccPort.Open();
			byte[] ssid = Encoding.ASCII.GetBytes(textBox1.Text);
			byte[] data = new byte[32];

			Array.Copy(ssid, data, textBox1.Text.Length);
			for (int i = textBox1.Text.Length; i < 32; ++i)
				data[i] = 0x00;

			SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_SET_SSID, 32, data);


			VaccPort.Close();

		}

		private void textBox4_TextChanged(object sender, EventArgs e)
		{
			VaccPort.Open();
			byte[] ssid = Encoding.ASCII.GetBytes(textBox1.Text);
			byte[] data = new byte[32];

			Array.Copy(ssid, data, textBox1.Text.Length);
			for (int i = textBox1.Text.Length; i < 32; ++i)
				data[i] = 0x00;

			SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_SET_UID, 32, data);


			VaccPort.Close();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			VaccPort.Open();
			byte[] uid = Encoding.ASCII.GetBytes(textBox3.Text);
			byte[] data = new byte[32];

			Array.Copy(uid, data, textBox3.Text.Length);
			for (int i = textBox3.Text.Length; i < 32; ++i)
				data[i] = 0x00;

			SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_SET_UID, 32, data);


			VaccPort.Close();
		}

		private void button9_Click(object sender, EventArgs e)
		{
			VaccPort.Open();

			try
			{

				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_UID, 0, new byte[0]);

				System.Threading.Thread.Sleep(500);

				int length = 0; byte[] rsp;


				if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_UID, out length, out rsp))
				{

					String wifi_uid = Encoding.ASCII.GetString(rsp, 0, 32);// sayısaldan çevirme

					textBox3.Text = (wifi_uid); // sayısaldan çevirme

				}

			}
			catch (Exception ex)
			{

			}
			VaccPort.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			VaccPort.Open();
			byte[] pasword = Encoding.ASCII.GetBytes(textBox2.Text);
			byte[] data = new byte[32];

			Array.Copy(pasword, data, textBox2.Text.Length);
			for (int i = textBox2.Text.Length; i < 32; ++i)
				data[i] = 0x00;

			SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_SET_PASSWORD, 32, data);


			VaccPort.Close();

		}

		private void button2_Click(object sender, EventArgs e)
		{
			VaccPort.Open();

			try
			{

				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_PASSWORD, 0, new byte[0]);

				System.Threading.Thread.Sleep(500);

				int length = 0; byte[] rsp;


				if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_PASSWORD, out length, out rsp))
				{

					String wifi_pasword = Encoding.ASCII.GetString(rsp, 0, 32);// sayısaldan çevirme

					textBox2.Text = (wifi_pasword); // sayısaldan çevirme

				}

			}
			catch (Exception ex)
			{

			}
			VaccPort.Close();

		}

		private void button3_Click(object sender, EventArgs e)
		{

			VaccPort.Open();
			byte[] sens_thrs = new byte[12];

			try
			{
				Int32 resist1 = Convert.ToInt32(textBox5.Text);


				int index = 0;
				sens_thrs[index++] = (byte)((resist1 >> 0) & 0x000000FF);
				sens_thrs[index++] = (byte)((resist1 >> 8) & 0x000000FF);
				sens_thrs[index++] = (byte)((resist1 >> 16) & 0x000000FF);
				sens_thrs[index++] = (byte)((resist1 >> 24) & 0x000000FF);

				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_SET_THRESHOLD, 4, sens_thrs);

			}
			catch (Exception ex)
			{

			}
			VaccPort.Close();
		}
		private void button7_Click(object sender, EventArgs e)
		{
			VaccPort.Open();

			try
			{

				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_THRESHOLD, 0, new byte[0]);


				int length = 0; byte[] rsp;

				System.Threading.Thread.Sleep(500);
				if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_THRESHOLD, out length, out rsp))
				{

					Int32 sens_thrs = BitConverter.ToInt32(rsp, 0); // sayısaldan çevirme

					textBox5.Text = (sens_thrs.ToString()); // sayısaldan çevirme

				}

			}
			catch (Exception ex)
			{

			}
			VaccPort.Close();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			byte[] data = new byte[12];
			VaccPort.Open();
			try
			{
				Int32 dev_id = Convert.ToInt32(textBox4.Text);
				Int32 hw_vers = Convert.ToInt32(textBox6.Text);


				int index = 0;
				data[index++] = (byte)((dev_id >> 0) & 0x000000FF);
				data[index++] = (byte)((dev_id >> 8) & 0x000000FF);
				data[index++] = (byte)((dev_id >> 16) & 0x000000FF);
				data[index++] = (byte)((dev_id >> 24) & 0x000000FF);

				data[index++] = (byte)((hw_vers >> 32) & 0x000000FF);
				data[index++] = (byte)((hw_vers >> 40) & 0x000000FF);
				data[index++] = (byte)((hw_vers >> 48) & 0x000000FF);
				data[index++] = (byte)((hw_vers >> 56) & 0x000000FF);

				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_SET_BOARD_PARAMS, 8, data);

			}
			catch (Exception ex)
			{

			}
			VaccPort.Close();
		}

		private void button6_Click(object sender, EventArgs e)
		{
			VaccPort.Open();

			try
			{

				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_BOARD_PARAMS, 0, new byte[0]);

				System.Threading.Thread.Sleep(500);

				int length = 0; byte[] rsp;


				if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_BOARD_PARAMS, out length, out rsp))
				{

					uint dev_id = BitConverter.ToUInt32(rsp, 0);// sayısaldan çevirme
					textBox4.Text = (dev_id.ToString()); // sayısaldan çevirme

					uint hw_version = BitConverter.ToUInt32(rsp, 4);
					textBox6.Text = (hw_version.ToString()); // sayısaldan çevirme
				}

			}
			catch (Exception ex)
			{

			}
			VaccPort.Close();
		}

		private void tabPage1_Click(object sender, EventArgs e)
		{

		}

		//*********************************************************

		//LOG CASE
		enum LOG_ID
		{
			LOG_TEMPRATURE_HIGH = 0,
			LOG_TEMPRATURE_LOW,
			LOG_INTERNET_NOT_ACTIVE,
			LOG_WARNING_BATTERY,
			LOG_WARNING_SIGNAL_WIFI,
			LOG_WARNING_SIGNAL_GSM,
			LOG_WIFI_CONNECTED,
			LOG_WIFI_DISCONNECTED,
			LOG_WIFI_NETWORK,
			LOG_GSM_SIM,
			LOG_GSM_NETWORK,
			LOG_POWER_STATUS,
			LOG_ADC_STATUS,
			LOG_TIME_SYNCHRONIZATION,
			LOG_GET_TOKEN,
			LOG_FIRMWARE_UPDATE,
			LOG_DATA_SYNC_DONE,
			LOG_WIFI_TOKEN_RESPONSE,
			LOG_WIFI_EPOCH_RESPONSE,
			LOG_WIFI_DATA_RESPONSE,
			LOG_GSM_TOKEN_RESPONSE,
			LOG_GSM_EPOCH_RESPONSE,
			LOG_GSM_DATA_RESPONSE,
			LOG_NEW_SSID,
			LOG_NEW_PASSWORD,
			LOG_THRESHOLD_SET,
			LOG_UID_SET,
			LOG_CALIBRATE,
			LOG_BOARD_PARAM_SET,
			LOG_BOOT_MODE_ACTIVE,
			LOG_SENSOR_TABLE_LOAD,
			LOG_SENSOR_TABLE_SAVE,
			LOG_GET_SSID,
			LOG_GET_PASSWORD,
			LOG_GET_BOARD_PARAMS,
			LOG_GET_SENSOR_TABLE,
			LOG_GET_CURR_SENSOR_TABLE,
			LOG_GET_THRESHOLD,
			LOG_GET_UID,
			LOG_DEBUG_START,
			LOG_DEBUG_STOP,
			LOG_DEBUG_GET_STATUS,
			LOG_GSM_STATU_START,
			LOG_GSM_STATU_STOP,
			LOG_WIFI_SSID_REQUEST,
			LOG_WIFI_SSID_RESPONSE,
			LOG_WIFI_PW_REQUEST,
			LOG_WIFI_PW_RESPONSE,
			LOF_WIFI_CONNECT_REQUEST,
			LOG_WIFI_STATU_REQUEST,
			LOG_WIFI_NETWORK_STATU_REQUEST,
			LOG_WIFI_CHECK_STATU_REQUEST,
			LOG_WIFI_CHECK_STATU_RESPONSE,
			LOG_WIFI_TOKEN_REQUEST,
			LOG_WIFI_EPOCH_REQUEST,
			LOG_WIFI_QUALITY_REQUEST,
			LOG_WIFI_QUALITY_RESPONSE,
			LOG_WIFI_DATA_REQUEST,
			LOG_GSM_POWER_ON,
			LOG_GSM_INIT_AT_REQUEST,
			LOG_GSM_INIT_AT_RESPONSE,
			LOG_GSM_CPIN_REQUEST,
			LOG_GSM_CPIN_RESPONSE,
			LOG_GSM_QSPN_REQUEST,
			LOG_GSM_QSPN_RESPONSE,
			LOG_GSM_ACTIVE_AT_REQUEST,
			LOG_GSM_ACTIVE_AT_RESPONSE,
			LOG_GSM_CSQ_REQUEST,
			LOG_GSM_CSQ_RESPONSE,
			LOG_GSM_HTTP_HEADER_REQUEST,
			LOG_GSM_HTTP_HEADER_RESPONSE,
			LOG_GSM_TOKEN_URL_REQUEST,
			LOG_GSM_TOKEN_URL_RESPONSE,
			LOG_GSM_EPOCH_URL_REQUEST,
			LOG_GSM_EPOCH_URL_RESPONSE,
			LOG_GSM_DATA_URL_REQUEST,
			LOG_GSM_DATA_URL_RESPONSE,
			LOG_GSM_READ_REQUEST,
			LOG_GSM_READ_RESPONSE,
			LOG_GSM_QIDEACT_REQUEST,
			LOG_GSM_QIDEACT_RESPONSE,
			LOG_EEPROM_STATU_ERROR
		};
		enum LOG_PARAMS
		{
			LOG_ANY = 0,
			LOG_OK,
			LOG_FAIL,
			LOG_PARAM_WIFI,
			LOG_PARAM_GSM,
			LOG_PARAM_WIFI_NO_AP,
			LOG_PARAM_WIFI_WRONG_PAS,
			LOG_PARAM_PLUGGED,
			LOG_PARAM_UNPLUGGED
		};
		public void USB_Handle(byte[] rsp, int lenght)

		{

			if (lenght == 0)
			{
				LogDebugMessage("Log table is empty", Color.Black);
			}
			else
			{
				//LogDebugMessage(buffer[4].ToString(), Color.Black);
				int epoch = 0;
				string command = "";
				string parameter = "";
				int cmd;
				int cmd_param;
				Color clr = Color.Black;

				for (int i = 0; i < lenght; i += 6)
				{
					epoch = BitConverter.ToInt32(rsp, i);
					Int16 input = BitConverter.ToInt16(rsp, i + 4);
					cmd = ((((input & 0xF0) >> 2) & 0x0F) << 2) | (input & 0x0F);

					if ((LOG_ID)cmd == LOG_ID.LOG_TEMPRATURE_HIGH || (LOG_ID)cmd == LOG_ID.LOG_TEMPRATURE_LOW)
					{
						int temp_flo = (input >> 6) & 0x0F;

						int temp_dec = ((((input >> 12) & 0x0F) << 2) & 0xF0) | ((input >> 10) & 0x0F);

						cmd_param = (temp_dec - 20) * 10 + temp_flo;
					}
					else
						cmd_param = (input >> 6);
					parameter = "";

					switch ((LOG_ID)cmd)
					{
						case LOG_ID.LOG_WIFI_CONNECTED:
							command = "WIFI CONNECTED";
							clr = Color.Green;
							break;
						case LOG_ID.LOG_WIFI_DISCONNECTED:
							command = "WIFI DISCONNECTED ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_PARAM_WIFI_NO_AP)
								parameter = "No AP Found";
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_PARAM_WIFI_WRONG_PAS)
								parameter = "Wrong Password";

							clr = Color.Green;
							break;
						case LOG_ID.LOG_WIFI_NETWORK:
							command = "WIFI NETWORK ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Green;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Orange;
							}
							break;
						case LOG_ID.LOG_GSM_SIM:
							command = "SIM ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "CONNECTED";
								clr = Color.Green;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Orange;
							}
							break;

						case LOG_ID.LOG_GSM_NETWORK:
							command = "SIM NETWORK ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Green;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Orange;
							}
							break;
						case LOG_ID.LOG_POWER_STATUS:
							command = "POWER ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_PARAM_UNPLUGGED)
							{
								parameter = "UNPLUGGED";
								clr = Color.Orange;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_PARAM_PLUGGED)
							{
								parameter = "PLUGGED";
								clr = Color.Green;
							}
							break;

						case LOG_ID.LOG_ADC_STATUS:
							command = "SENSORS ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_PARAM_UNPLUGGED)
							{
								parameter = "UNPLUGGED";
								clr = Color.Orange;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_PARAM_PLUGGED)
							{
								parameter = "PLUGGED";
								clr = Color.Green;
							}
							break;
						case LOG_ID.LOG_WIFI_TOKEN_RESPONSE:
							command = "WIFI TOKEN RESPONSE ";
							clr = Color.Blue;
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
							}
							break;
						case LOG_ID.LOG_WIFI_EPOCH_RESPONSE:
							command = "WIFI EPOCH RESPONSE ";

							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
							}
							break;

						case LOG_ID.LOG_WIFI_DATA_RESPONSE:
							command = "WIFI DATA RESPONSE ";
							clr = Color.Blue;
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
							}
							break;
						case LOG_ID.LOG_GSM_TOKEN_RESPONSE:
							command = "GSM TOKEN RESPONSE ";
							clr = Color.Blue;
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
							}
							break;
						case LOG_ID.LOG_GSM_EPOCH_RESPONSE:
							command = "GSM EPOCH RESPONSE ";
							clr = Color.Blue;
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
							}
							break;
						case LOG_ID.LOG_GSM_DATA_RESPONSE:
							command = "GSM DATA RESPONSE ";
							clr = Color.Blue;
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
							}
							break;
						case LOG_ID.LOG_INTERNET_NOT_ACTIVE:
							command = "NO VALID INTERNET CONNECTION";
							clr = Color.Red;
							break;
						case LOG_ID.LOG_NEW_SSID:
							command = "WIFI new SSID Set";
							clr = Color.Blue;
							break;
						case LOG_ID.LOG_NEW_PASSWORD:
							command = "WIFI new Password Set";
							clr = Color.Blue;
							break;
						case LOG_ID.LOG_THRESHOLD_SET:
							command = "New Threshold Set";
							clr = Color.Blue;
							break;
						case LOG_ID.LOG_UID_SET:
							command = "New Board UID Set";
							clr = Color.Blue;
							break;
						case LOG_ID.LOG_CALIBRATE:
							command = "Sensors Calibrated";
							clr = Color.Blue;
							break;
						case LOG_ID.LOG_BOARD_PARAM_SET:
							command = "New board parameters set";
							clr = Color.Blue;
							break;
						case LOG_ID.LOG_BOOT_MODE_ACTIVE:
							command = "Boot mode active";
							clr = Color.Blue;
							break;
						case LOG_ID.LOG_SENSOR_TABLE_SAVE:
							command = "New Sensor Table saved";
							clr = Color.Blue;
							break;
						case LOG_ID.LOG_GET_SSID:
							command = "SSID get";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GET_PASSWORD:
							command = "Password get";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GET_BOARD_PARAMS:
							command = "Get board parameters";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GET_SENSOR_TABLE:
							command = "Get Sensor Table";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GET_CURR_SENSOR_TABLE:
							command = "Get Current Sensor Table";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GET_THRESHOLD:
							command = "Get Threshold";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GET_UID:
							command = "Get UID";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_DEBUG_START:
							command = "Debuf Stream Start";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_DEBUG_STOP:
							command = "Debug Stream Stop";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_DEBUG_GET_STATUS:
							command = "Debug Get Status";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_STATU_START:
							command = "GSM Statu Stream Start";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_STATU_STOP:
							command = "GSM Statu Stream Stop";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_WIFI_SSID_REQUEST:
							command = "Wifi SSID Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_WIFI_SSID_RESPONSE:
							command = "Wifi SSID Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_WIFI_PW_REQUEST:
							command = "Wifi Password Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_WIFI_PW_RESPONSE:
							command = "Wifi Password Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOF_WIFI_CONNECT_REQUEST:
							command = "Wifi Connect Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_WIFI_STATU_REQUEST:
							command = "Wifi Statu Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_WIFI_NETWORK_STATU_REQUEST:
							command = "Wifi Network Statu Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_WIFI_CHECK_STATU_REQUEST:
							command = "Wifi Check Statu Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_WIFI_CHECK_STATU_RESPONSE:
							command = "Wifi Check Statu Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_WIFI_TOKEN_REQUEST:
							command = "Wifi Token Request";
							parameter = "";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_WIFI_EPOCH_REQUEST:
							command = "Wifi Epoch Request";
							parameter = "";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_WIFI_QUALITY_REQUEST:
							command = "Wifi Quality Request";
							parameter = "";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_WIFI_QUALITY_RESPONSE:
							command = "Wifi Quality Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_WIFI_DATA_REQUEST:
							command = "Wifi DATA Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_POWER_ON:
							command = "GSM Power ON";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_INIT_AT_REQUEST:
							command = "GSM Init AT Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_INIT_AT_RESPONSE:
							command = "GSM Init AT Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_GSM_CPIN_REQUEST:
							command = "GSM CPIN Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_CPIN_RESPONSE:
							command = "GSM CPIN Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_GSM_QSPN_REQUEST:
							command = "GSM QSPN Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_QSPN_RESPONSE:
							command = "GSM QSPN Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_GSM_ACTIVE_AT_REQUEST:
							command = "GSM Active state AT Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_ACTIVE_AT_RESPONSE:
							command = "GSM Active state AT Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_GSM_CSQ_REQUEST:
							command = "GSM CSQ Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_CSQ_RESPONSE:
							command = "GSM CSQ Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_GSM_HTTP_HEADER_REQUEST:
							command = "GSM HTTP Header Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_HTTP_HEADER_RESPONSE:
							command = "GSM HTTP Header Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_GSM_TOKEN_URL_REQUEST:
							command = "GSM Token URL Request";
							parameter = "";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_TOKEN_URL_RESPONSE:
							command = "GSM Token URL Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_GSM_EPOCH_URL_REQUEST:
							command = "GSM Epoch URL Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_EPOCH_URL_RESPONSE:
							command = "GSM Epoch URL Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_GSM_DATA_URL_REQUEST:
							command = "GSM Data URL Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_DATA_URL_RESPONSE:
							command = "GSM Data URL Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_GSM_READ_REQUEST:
							command = "GSM Read Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_READ_RESPONSE:
							command = "GSM Read Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_GSM_QIDEACT_REQUEST:
							command = "GSM QIDEACT Request";
							clr = Color.Black;
							break;
						case LOG_ID.LOG_GSM_QIDEACT_RESPONSE:
							command = "GSM QIDEACT Response ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_OK)
							{
								parameter = "OK";
								clr = Color.Black;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_FAIL)
							{
								parameter = "ERROR";
								clr = Color.Black;
							}
							break;
						case LOG_ID.LOG_TIME_SYNCHRONIZATION:
							command = "Time Synchronation - ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_PARAM_GSM)
							{
								parameter = "GSM";
								clr = Color.Green;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_PARAM_WIFI)
							{
								parameter = "WIFI";
								clr = Color.Green;
							}
							break;
						case LOG_ID.LOG_GET_TOKEN:
							command = "Get Token - ";
							if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_PARAM_GSM)
							{
								parameter = "GSM";
								clr = Color.Green;
							}
							else if ((LOG_PARAMS)cmd_param == LOG_PARAMS.LOG_PARAM_WIFI)
							{
								parameter = "WIFI";
								clr = Color.Green;
							}
							break;
						case LOG_ID.LOG_FIRMWARE_UPDATE:
							command = "Firmware Update";
							clr = Color.Green;
							break;
						case LOG_ID.LOG_TEMPRATURE_HIGH:
							command = "TEMP HIGH - " + (cmd_param / 10.0).ToString();
							clr = Color.Red;
							break;
						case LOG_ID.LOG_TEMPRATURE_LOW:
							command = "TEMP LOW - " + (cmd_param / 10.0).ToString();
							clr = Color.Red;
							break;
						case LOG_ID.LOG_WARNING_BATTERY:
							command = "BATTERY LOW - " + cmd_param.ToString();
							clr = Color.Orange;
							break;
						case LOG_ID.LOG_WARNING_SIGNAL_WIFI:
							command = "WIFI Signal Quality low - " + cmd_param.ToString(); ;
							clr = Color.Orange;
							break;
						case LOG_ID.LOG_WARNING_SIGNAL_GSM:
							command = "GSM Signal Quality low - " + cmd_param.ToString(); ;
							clr = Color.Orange;
							break;
						case LOG_ID.LOG_DATA_SYNC_DONE:
							command = "Data Syncronation completed";
							clr = Color.Green;
							break;
						case LOG_ID.LOG_EEPROM_STATU_ERROR:
							command = "EEPROM Statu FAIL";
							clr = Color.Blue;
							break;
						default:
							command = "unkown log id: ";
							parameter = cmd.ToString();
							clr = Color.Black;
							break;
					}
					LogDebugMessage("[" + (i / 6).ToString() + "]  " 
										+ new DateTime(1970, 1, 1).AddSeconds((double)epoch).ToString() 
										+ "  " + command + parameter, clr);





				}


			}



		}

		private void LogDebugMessage(string msg, Color clr) /// logtextbox

		{
			LogTextBox.Invoke(new EventHandler(delegate
			{
				LogTextBox.SelectedText = string.Empty;
				LogTextBox.SelectionColor = clr;
				LogTextBox.AppendText(msg);
				LogTextBox.AppendText(Environment.NewLine);
				LogTextBox.ScrollToCaret();
			}));
		}

		private void DataLogDebugMessage(string msg, Color clr) /// datalogtextbox
		{
			dataLogTxtBox.Invoke(new EventHandler(delegate
			{
				dataLogTxtBox.SelectedText = string.Empty;
				dataLogTxtBox.SelectionColor = clr;
				dataLogTxtBox.AppendText(msg);
				dataLogTxtBox.AppendText(Environment.NewLine);
				dataLogTxtBox.ScrollToCaret();
			}));
		}

		private void ClearLogMessage()
		{

			LogTextBox.Invoke(new EventHandler(delegate
			{
				LogTextBox.Clear();
				LogTextBox.ScrollToCaret();
			}));

		}

		//******************************************************** lLOG

		private void DebugMessage(string msg, Color clr)
		{
			int debugIndex = 0;

			richTextBox2.Invoke(new EventHandler(delegate //log
			{
				richTextBox2.SelectedText = string.Empty;
				richTextBox2.SelectionColor = clr;
				richTextBox2.AppendText("[" + debugIndex.ToString() + "] " + msg);
				richTextBox2.AppendText(Environment.NewLine);
				richTextBox2.ScrollToCaret();
			}));
			debugIndex++;
		}
		private void LogTableComingData()

			{
			ClearLogMessage();

			VaccPort.Open();

			try
			{
				int timeout = 0;
				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_LOG_TABLE, 0, null);
				System.Threading.Thread.Sleep(500);
				while (true)
				{
					int length = 0; byte[] rsp;
					System.Threading.Thread.Sleep(500);
					if (VaccPort.BytesToRead > 0)
					{
						if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_LOG_TABLE, out rsp, out length))
						{
							USB_Handle(rsp, length); //veriler burda
							timeout = 0;
						}
					}
					if (timeout++ > 10)
						break;
				}
			}
			catch (Exception ex)
			{

			}
			VaccPort.Close();
		}


		private void button8_Click(object sender, EventArgs e)
		{
				LogTableComingData();
		}

		private void btnGSMSend4_Click(object sender, EventArgs e)
		{
			string url = @"https://www.eniskurtayyilmaz.com/ornek.json";

			HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
			string jsonVerisi = "";
			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				StreamReader reader = new StreamReader(response.GetResponseStream());
				//jsonVerisi adlı değişkene elde ettiği veriyi atıyoruz.
				jsonVerisi = reader.ReadToEnd();

				LogTextBox.Text = (jsonVerisi.ToString());





			}
		}

		private string getToken()
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ats.isina.com.tr/rs/oauth/token");
			request.ContentType = "application/x-www-form-urlencoded";
			request.Headers.Add("Authorization", "Basic dU5vRmRnWmlkWE1DYkNsTmIxT1didGdtL2luQWpFT3BGRklVRkVkc3V6Zz06b1kyL0xRREx5UDM1Y3BBZmdtTmsrUkhHa1dxUXJleHJoQ2I0Vm5YVWtIdz0=");
			request.Method = "POST";
			//var data = Encoding.ASCII.GetBytes(data_log_queue.Dequeue());
			var data = Encoding.ASCII.GetBytes("grant_type=client_credentials");
			request.ContentLength = data.Length;
			using (var stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}
			var response = (HttpWebResponse)request.GetResponse();

			var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

			JObject json = JObject.Parse(responseString);
			return json["access_token"].ToString();
		}
		private int getEpoch(string token)
		{

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ats.isina.com.tr/rs/rest/device-data/time");
			request.Headers.Add("Authorization", "Bearer " + token);
			request.Method = "GET";

			var response = (HttpWebResponse)request.GetResponse();

			var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

			Console.WriteLine(responseString);

			JObject json = JObject.Parse(responseString);

			return json.GetValue("epoch").ToObject<int>();
		}

		private void btnEpochSync_Click(object sender, EventArgs e)
		{

			VaccPort.Open();

			byte[] epoch_data = new byte[4];

			try
			{
				string access_token = getToken();
				Int32 epoch = getEpoch(access_token);

				int index = 0;
				epoch_data[index++] = (byte)((epoch >> 0) & 0x000000FF);
				epoch_data[index++] = (byte)((epoch >> 8) & 0x000000FF);
				epoch_data[index++] = (byte)((epoch >> 16) & 0x000000FF);
				epoch_data[index++] = (byte)((epoch >> 24) & 0x000000FF);

				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_SYNC_EPOCH, 4, epoch_data);

			}
			catch (Exception)
			{

			}
			VaccPort.Close();
		}
		private void sync_datas()
		{
			string access_token = getToken();
			bool flag = false;
			while (thread_active)
			{
				if (data_log_queue.Count != 0)
				{
					flag = true;
					HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ats.isina.com.tr/rs/rest/device-data/");
					request.ContentType = "application/json";
					request.Headers.Add("Authorization", "Bearer " + access_token);
					request.Method = "POST";
					var data = Encoding.ASCII.GetBytes(data_log_queue.Dequeue());
					request.ContentLength = data.Length;
					using (var stream = request.GetRequestStream())
					{
						stream.Write(data, 0, data.Length);
					}

					var response = (HttpWebResponse)request.GetResponse();

					var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
					Thread.Sleep(30);
				}
				else if (flag)
				{
					flag = false;
					DebugMessage("Sync completed.", Color.Black);
				}
			}
		}

		private void button10_Click(object sender, EventArgs e)
		{
			
		}


		private void DataLogTableComingData()
		{
			ClearLogMessage();

			VaccPort.Open();

			try
			{
				int timeout = 0; int length = 0; byte[] rsp;
				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG_TABLE_LEN, 0, null);
				System.Threading.Thread.Sleep(500);
				if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG_TABLE_LEN, out length, out rsp))
				{
					intDataLogLength = BitConverter.ToUInt32(rsp, 0);
					DataLogDebugMessage("Data Log Count : " + intDataLogLength.ToString(), Color.Red);
					if (intDataLogLength == 0)
					{
						VaccPort.Close(); return;
					}
				}
				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG, 0, null);

				while (true)
				{
					System.Threading.Thread.Sleep(500);
					if (VaccPort.BytesToRead > 0)
					{
						if (GetDataLog(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG, out rsp, out length))
						{	
							USB_Handle2(rsp);
							timeout = 0;
						}
					}
					if (timeout++ > 10)
						break;
				}
			}
			catch (Exception ex)
			{

			}
			VaccPort.Close();
		}



		private int GetDataLogTable(out JsonDataLog[] datas)
		{
			try
			{
				datas = new JsonDataLog[1];
				ClearLogMessage();
				VaccPort.Open();
				int timeout = 0; int length = 0; byte[] rsp;
				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG_TABLE_LEN, 0, null);
				System.Threading.Thread.Sleep(500);
				if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG_TABLE_LEN, out length, out rsp))
				{
					intDataLogLength = BitConverter.ToUInt32(rsp, 0);
					DataLogDebugMessage("Data Log Count : " + intDataLogLength.ToString(), Color.Red);
					datas = new JsonDataLog[intDataLogLength];
					if (intDataLogLength == 0)
					{
						VaccPort.Close(); return 0;
					}
				}
                else
                {
					VaccPort.Close(); return 0;
				}
				SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG, 0, null);

				while (true)
				{
					System.Threading.Thread.Sleep(500);
					if (VaccPort.BytesToRead > 0)
					{
						if (GetDataLog(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG, out rsp, out length))
						{
							USB_Handle2(rsp);
							timeout = 0;
						}
					}
					if (timeout++ > 10)
						break;
				}
				VaccPort.Close();
				return (int)intDataLogLength;
			}
			catch (Exception ex)
			{
				datas = null;
				return 0;
			}
		
		}



		UInt32 intDataLogLength = 0;
		private void btnGetDataLogs_Click(object sender, EventArgs e)
		{
				DataLogTableComingData();
			
		}


		private void Data_Log_parser(byte[] rsp, out uint epoch, out int door_open, out int on_power, out double ref_temp, out double in_temp, out double ex_temp)
		{

			/*
			 * response[] : <0     ><     1>     <2>     <3>     <4> 
			 * response[] : <Epoch0><Epoch1><Epoch2><Epoch3><Door Open | On Power | Ref_Temp_Flo> 
			 */

			epoch = BitConverter.ToUInt32(rsp, 0);  // Byte 0-4 : Epoch DateTime

			door_open = rsp[4] << 3;
			door_open = (door_open & 0x0F) >> 3;

			on_power = (rsp[4] << 3) & 0xF0;
			on_power = ((on_power >> 1) & 0x0F) >> 3;

			int ref_temp_flo = (rsp[4] >> 2) & 0x0F;
			int ref_temp_dec = ((((rsp[4] >> 2) & 0xF0) >> 2) & 0x0F);
			ref_temp_dec = (((rsp[5] << 4) & 0xF0) | ref_temp_dec) >> 2;
			ref_temp = (ref_temp_dec * 10 + ref_temp_flo) / 10.0 - 20;

			int in_temp_flo = (rsp[5] >> 4) & 0x0F;
			int in_temp_dec = ((rsp[6] & 0xF0) >> 2) & 0x0F;
			in_temp_dec = (in_temp_dec << 2) | (rsp[6] & 0x0F);
			in_temp = (in_temp_dec * 10 + in_temp_flo) / 10.0 - 20;

			int ex_temp_dec = (((rsp[6] >> 2) & 0xF0) >> 2) & 0x0F;
			ex_temp_dec = (((rsp[7] << 4) & 0xF0) | ex_temp_dec) >> 2;
			int ex_temp_flo = ((rsp[7] >> 4) & 0x0F);
			ex_temp = (ex_temp_dec * 10 + ex_temp_flo) / 10.0 - 20;
		}

		public void USB_Handle2(byte[] rsp)
		{

			//DebugMessage("len : " + len.ToString(),Color.Black);
			const int DATA_LOG_ENTRY_LEN = 8;
			uint epoch = 0;
			int door_open = 0, on_power = 0;
			double ref_temp = 0.0, in_temp = 0.0, ex_temp = 0.0;
			byte[] raw_data_log = new byte[DATA_LOG_ENTRY_LEN];

			if (true)
			{
				int len = (int)intDataLogLength;
				for (int i = 0; i < len; ++i)
				{
					try
					{
						Array.Copy(rsp, 8 * i, raw_data_log, 0, DATA_LOG_ENTRY_LEN);
						Data_Log_parser(raw_data_log, out epoch, out door_open, out on_power, out ref_temp, out in_temp, out ex_temp);

						string log_data = "{\"gsmNetwork\":\"\",\"gsmNetworkQuality\":0.0,\"wifiNetworkState\":false," +
											"\"wifiSignalQuality\":0.0,\"ethernetState\":false,\"onPower\":" + (on_power == 1 ? "true" : "false") + ",\"batteryVoltage\":220.0,\"batteryPercent\":0," +
											"\"batteryLife\":0,\"fridgeTemprature\":" + ref_temp.ToString() + ",\"internalModuleTemprature\":" + in_temp.ToString() + ",\"externalModuleTemprature\":" + ex_temp.ToString() + "," +
											"\"doorOpen\":" + (door_open == 1 ? "true" : "false") + ",\"lightLevel\":100.0,\"alarmCode\":\"A123\",\"alarmDescription\":\"X Alarm\",\"faultCode\":\"F123\"," +
											"\"faultDescription\":\"X Fault\",\"workingCounter\":0,\"restartCounter\":0,\"reportDateEpoch\":\"" + epoch.ToString() + "\"," +
											"\"deviceUid\":\"" + device_uid + "\"}";

						dataLogTxtBox.Invoke(new EventHandler(delegate
						{
							dataLogTxtBox.SelectedText = string.Empty;
							dataLogTxtBox.SelectionColor = Color.Black;
							dataLogTxtBox.AppendText("[" + i.ToString() + "]  " + "DateTime: " + new DateTime(1970, 1, 1).AddSeconds(epoch).ToString() + " Door: " + door_open.ToString() + " Power: " + on_power.ToString() + " Ref: " + ref_temp.ToString() + " In: " + in_temp.ToString() + " Ex: " + ex_temp.ToString());
							dataLogTxtBox.AppendText(Environment.NewLine);
							dataLogTxtBox.ScrollToCaret();
						}));
					}
					catch (Exception ex)
					{
						DebugMessage("Exception :" + ex.Message, Color.Black);
					}

				}
			}
		}




		bool SendCommand2(SerialPort sp, Commands cmd, Commands subCmd, byte length, byte[] parameters)
		{
			try
			{
				byte[] sendBuffer = new byte[length + 9];
				sendBuffer[0] = (byte)Commands.HostPrefixA;
				sendBuffer[1] = (byte)Commands.HostPrefixB;
				sendBuffer[2] = (byte)cmd;
				sendBuffer[3] = (byte)subCmd;
				sendBuffer[4] = length;

				if ((parameters != null))

					if (parameters.Length >= length)
						Array.Copy(parameters, 0, sendBuffer, 5, length);

				byte[] crc = CalcCRC(sendBuffer, 5 + length);

				Array.Copy(crc, 0, sendBuffer, 5 + length, 4);

				sp.Write(sendBuffer, 0, sendBuffer.Length);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private void btnStartDataSend_Click(object sender, EventArgs e)
		{

		}

		private void button12_Click(object sender, EventArgs e)
		{

		}
		/***************************************LOG POST*************************************************************************************************/
		int GetEventsLogs(out JsonEventLog[] logs)
		{
			logs = new JsonEventLog[1];
			try
			{
				VaccPort.Open();
				int timeout = 0; int length = 0; byte[] rsp; int logIndex = 0;
				try
				{
					SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_LOG_TABLE_LEN, 0, null);
					System.Threading.Thread.Sleep(500);
					if (GetResponse(VaccPort,Commands.UsbCmd,Commands.USB_GET_LOG_TABLE_LEN, out logIndex, out rsp))
					{
						logIndex = BitConverter.ToInt32(rsp, 0);
						logs = new JsonEventLog[logIndex];
						logIndex = 0;
					}
					SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_LOG_TABLE, 0, null);
					
					while (true)
					{
						System.Threading.Thread.Sleep(500);
						if (VaccPort.BytesToRead > 0)
						{
							if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_LOG_TABLE, out rsp, out length))
							{
								for (int i = 0; i < length; i += 6)
								{
									int epoch = BitConverter.ToInt32(rsp, i);
									Int16 input = BitConverter.ToInt16(rsp, i + 4);
									int logID = ((((input & 0xF0) >> 2) & 0x0F) << 2) | (input & 0x0F);
									int logParam = 0;
									string logValue = "0";

									if ((LOG_ID)logID == LOG_ID.LOG_TEMPRATURE_HIGH || (LOG_ID)logID == LOG_ID.LOG_TEMPRATURE_LOW)
									{
										int temp_flo = (input >> 6) & 0x0F;

										int temp_dec = ((((input >> 12) & 0x0F) << 2) & 0xF0) | ((input >> 10) & 0x0F);

										logParam = (temp_dec - 20) * 10 + temp_flo;
										logValue = logParam.ToString();
									}
									else
									{
										logParam = input >> 6;
										logValue = logParam.ToString();
									}
									if (logIndex > logs.Length + 1)
									{
										VaccPort.Close();
										return logIndex;
									}
									logs[logIndex++] = new JsonEventLog() { uid=UID, dateEpoch = epoch, logId = logID, logParam = logParam, logValue = logValue };
								}
								timeout = 0;
							}
						}
						if (timeout++ > 10)
							break;
					}
				}
				catch (Exception ex)
				{

				}
				VaccPort.Close();
				return logIndex;
			}
			catch (Exception)
			{
				return 0;
			}
		}
		/* log post*/
		public string UID = "D00130222113";
		
		private void JsonPost_Click(object sender, EventArgs e)
		{

			JsonEventLog[] jsonDatas;

			int iLogLength = GetEventsLogs(out jsonDatas);

			//for (int i = 0; i < jsonDatas.Length; i++)
			//{
			//    jsonDatas[i] = new JsonEventLog() { dateEpoch = (1625569609 + i*60), logId = 1, logParam = 0, logValue = "3", uid = "E00630222113" };
			//}

			try
			{
				string access_token = getToken();
				int balance = jsonDatas.Length % 8;
				int j = ((jsonDatas.Length / 8) + (balance > 0 ? 1 : 0));
				
				for (int i = 0; i < j; i++)
				{
					
					JsonEventLog[] logs = new JsonEventLog[i == j - 1 ? balance : 8];
					Array.Copy(jsonDatas, i * 8, logs, 0, i == j - 1 ? balance : 8);
					


						string strData = JsonConvert.SerializeObject(logs);
						dataLogTxtBox.AppendText(strData);
						byte[] datas = Encoding.ASCII.GetBytes(strData);

						if (strData.Length != 0)
						{


							HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ats.isina.com.tr/rs/rest/events/");
							request.ContentType = "application/json";
							request.Headers.Add("Authorization", "Bearer " + access_token);
							request.Method = "POST";

							request.ContentLength = datas.Length;
							using (var stream = request.GetRequestStream())
							{
								stream.Write(datas, 0, datas.Length);
							}

							var response = (HttpWebResponse)request.GetResponse();

							var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
						
                    }
                }
			}
			catch (Exception ex)
			{

			}
		}
		/************************************************************************************************************************************************************************************************************************************************************************************/

		/****************************************************************DATA LOG POST *****************************************************************************************************************************************/
		int GetDataLogs(out JsonDataLog[] logs)
		{
			const int DATA_LOG_ENTRY_LEN = 8;
			byte[] raw_data_log = new byte[DATA_LOG_ENTRY_LEN];
			logs = new JsonDataLog[1];
					try
					{
						VaccPort.Open();
						int timeout = 0; int length = 0; byte[] rsp; int DatalogIndex = 0;
						SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG_TABLE_LEN, 0, null);
						System.Threading.Thread.Sleep(500);
						if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG_TABLE_LEN, out length, out rsp))
						{
							DatalogIndex = BitConverter.ToInt32(rsp, 0);
							logs = new JsonDataLog[DatalogIndex];
							DatalogIndex = 0;
						}
					else
					{
						return 0;
					}
						SendCommand(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG, 0, null);

						while (true)
						{
							System.Threading.Thread.Sleep(500);
							if (VaccPort.BytesToRead > 0)
							{
								if (GetResponse(VaccPort, Commands.UsbCmd, Commands.USB_GET_DATA_LOG, out rsp, out length))
								{
									{
										for (int i = 0; i < length; i += DATA_LOG_ENTRY_LEN)
										{
											Array.Copy(rsp, 8 * i, raw_data_log, 0, DATA_LOG_ENTRY_LEN);
											Data_Log_parser(raw_data_log, out uint epoch, out int door_open, out int on_power, out double ref_temp, out double in_temp, out double ex_temp );

											logs[DatalogIndex++] = new JsonDataLog()
											{
												deviceUid = UID,
												reportDateEpoch = epoch.ToString(), 
												externalModuleTemprature=ex_temp, 
												internalModuleTemprature=in_temp,
												fridgeTemprature=ref_temp,
												onPower= Convert.ToBoolean(on_power),
												alarmDescription = "X Alarm",
												gsmNetwork = "",
												gsmNetworkQuality = 0.0,
												wifiSignalQuality = 0.0,
												ethernetState = false,
												batteryVoltage = 220.0,
												batteryPercent = 0,
												batteryLife=0 ,
												lightLevel=100.0 , 
												alarmCode="A123"  , 
												faultCode = "F123" ,
												faultDescription="X FAULT",
												workingCounter=0 ,
												restartCounter=0 
											};

											if (DatalogIndex > logs.Length + 1)
											{
												VaccPort.Close();
											
											}
									   
										}
										timeout = 0;
									}
							
								}
						
								if (timeout++ > 10)
								{
									break;
								}
							}
						}
					}

				catch (Exception ex)
				{
					
				}
			VaccPort.Close();
			//logs = null;
			return 0;
		}           


		private void button10_Click_1(object sender, EventArgs e)
		{

			JsonDataLog[] jsonDatas;

			int iLogLength = GetDataLogs(out jsonDatas);

			//for (int i = 0; i < jsonDatas.Length; i++)
			//{
			//    jsonDatas[i] = new JsonEventLog() { dateEpoch = (1625569609 + i*60), logId = 1, logParam = 0, logValue = "3", uid = "E00630222113" };
			//}

			try
			{
				int balance = jsonDatas.Length % 16;
				int j = ((jsonDatas.Length / 16) + (balance > 0 ? 1 : 0));
				string access_token = getToken();
				
				for (int i = 0; i < j; i++)
				{
					JsonDataLog[] logs = new JsonDataLog[i == j -1 ? balance : 16];
					Array.Copy(jsonDatas, i *  16, logs, 0, i == j -1 ? balance : 16);
                    for (int n = 0; n < logs.Length; n++)
                    {

                        string strData = JsonConvert.SerializeObject(logs[n]);
						dataLogTxtBox.AppendText(strData) ;
                        byte[] datas = Encoding.ASCII.GetBytes(strData);

                        if (strData.Length != 0)
                        {


                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ats.isina.com.tr/rs/rest/device-data/");
                            request.ContentType = "application/json";
                            request.Headers.Add("Authorization", "Bearer " + access_token);
                            request.Method = "POST";

                            request.ContentLength = datas.Length;
                            using (var stream = request.GetRequestStream())
                            {
                                stream.Write(datas, 0, datas.Length);
                            }

                            var response = (HttpWebResponse)request.GetResponse();

                            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        } 
                    }
				}
			}
			catch (Exception ex)
			{

			}
		}

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tsLblMain_Click(object sender, EventArgs e)
        {

        }

        /******************************************************************************************************************************************//******************************************************************************************************************************************/

    }
}

	

	
	


