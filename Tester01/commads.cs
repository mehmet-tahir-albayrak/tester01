namespace Ecg_CML_SDK
{
    public class Commands
    {

        public const byte UART_TX_PREFIX_A = 0xC2;
        public const byte UART_TX_PREFIX_B = 0xB1;
        public const byte UART_RX_PREFIX_A = 0x81;
        public const byte UART_RX_PREFIX_B = 0x12;

        public const byte EEPROM_CMD = 0xA1;
        public const byte DEBUG_CMD = 0xA2;
        public const byte BOOT_CMD = 0xA3;
        public const byte GSM_CMD = 0xA4;
        public const byte USB_CMD = 0xA6;


        public const byte DB_START_STREAM = 0x01;
        public const byte DB_STREAM = 0x02;
        public const byte DB_STATUS = 0x03;
        public const byte DB_STOP = 0x04;
        public const byte DB_GET_LOG = 0x05;
        

        public const byte BL_GET_VER = 0xD0;
        public const byte BL_GET_CID = 0xD1;
        public const byte BL_JUMP_TO_APP = 0xD2;
        public const byte BL_FLASH_ERASE = 0xD3;
        public const byte BL_MEM_WRITE = 0xD4;
        public const byte BL_BOOT_MODE = 0xD5;
        public const byte BL_GET_MEM_WRITE_STATUS = 0xD6;

        public const byte EP_UPDATE_INIT_PARAMS = 0xB0;
        public const byte EP_UPDATE_BOARD_PARAMS = 0xB1;
        public const byte EP_UPDATE_API_PARAMS = 0xB3;
        public const byte EP_GET_INIT_PARAMS = 0xB4;
        public const byte EP_GET_BOARD_PARAMS = 0xB5;
        public const byte EP_GET_API_PARAMS = 0xB7;
        public const byte EP_DELETE_PARAMS = 0xB8;
        public const byte EP_UPDATE_SENSOR = 0xB9;
        public const byte EP_GET_SENSOR = 0xBA;
        public const byte EP_SET_SENSOR_PARAMS = 0xBB;
        public const byte EP_GET_SENSOR_PARAMS = 0xBC;
        public const byte EP_GET_CURR_SENSOR = 0xBD;
        public const byte EP_GET_SAVE_SENSOR = 0xBE;
        public const byte EP_SET_SENSOR_THRS = 0xBF;
        public const byte EP_GET_SENSOR_THRS = 0xC0;
        public const byte EP_GET_UID = 0xC1;
        public const byte EP_SET_UID = 0xC2;
        public const byte EP_GET_WIFI_SSID = 0xC3;
        public const byte EP_SET_WIFI_SSID = 0xC4;
        public const byte EP_GET_WIFI_PASS = 0xC5;
        public const byte EP_SET_WIFI_PASS = 0xC6;
        public const byte EP_CLEAR_LOGS = 0xC7;

        public const byte GSM_STATUS_SEND = 0x01;
        public const byte GSM_STATUS_START = 0x02;
        public const byte GSM_STATUS_STOP = 0x03;
        public const byte GSM_T_MODE_ON = 0x04;
        public const byte GSM_T_MODE_OFF = 0x05;
        public const byte GSM_SEND_AT_CMD = 0x06;
        public const byte GSM_GET_AT_RESP = 0x07;
        public const byte GSM_SEND_CACERT = 0x08;
        public const byte GSM_SEND_C_CERT = 0x09;
        public const byte GSM_SEND_C_KEY = 0x0A;

        public const byte USB_GET_DATA_LOG = 0x01;
        public const byte USB_GET_THRESHOLD = 0x02;
        public const byte USB_SET_THRESHOLD = 0x03;
        public const byte USB_SET_UID = 0x04;
        public const byte USB_GET_UID = 0x05;
        public const byte USB_SET_SSID = 0x06;
        public const byte USB_GET_SSID = 0x07;
        public const byte USB_SET_PASSWORD = 0x08;
        public const byte USB_GET_PASSWORD = 0x09;
        public const byte USB_CALIBRATE = 0x0A;
        public const byte USB_GET_BOARD_PARAMS = 0x0B;
        public const byte USB_SET_BOARD_PARAMS = 0x0C;
        public const byte USB_UPDATE_SENSOR = 0x0D;
        public const byte USB_GET_SENSOR = 0x0E;
        public const byte USB_GET_CURR_SENSOR = 0x0F;
        public const byte USB_SAVE_SENSOR = 0x10;
        public const byte USB_GET_STATUS = 0x11;
        public const byte USB_GET_LOG_TABLE = 0x12;
        public const byte USB_CLEAR_LOG_TABLE = 0x13;
        public const byte USB_FLASH_INFO = 0x14;
        public const byte USB_MEM_WRITE = 0x15;
        public const byte USB_START_STREAM = 0x16;
        public const byte USB_STOP_STREAM = 0x17;
        public const byte USB_STREAM = 0x18;
        public const byte USB_DEBUG_DATA = 0x19;
        public const byte USB_GSM_T_ON = 0x20;
        public const byte USB_GSM_T_OFF = 0x21;
        public const byte USB_GSM_AT_CMD = 0x22;

        public const byte USB_GSM_TEST = 0x30;
        public const byte USB_SYNC_EPOCH = 0x25;

    }
}
