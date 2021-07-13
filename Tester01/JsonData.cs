using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester01
{
    public class JsonEventLog
    {

        [JsonProperty("uid")]
        public string uid { get; set; }

        [JsonProperty("dateEpoch")]
        public int dateEpoch { get; set; }

        [JsonProperty("logValue")]
        public string logValue { get; set; }

        [JsonProperty("logId")]
        public int logId { get; set; }

        [JsonProperty("logParam")]
        public int logParam { get; set; }
    }

    public class JsonDataLog
    {
        public string gsmNetwork { get; set; }
        public double gsmNetworkQuality { get; set; }
        public bool wifiNetworkState { get; set; }
        public double wifiSignalQuality { get; set; }
        public bool ethernetState { get; set; }
        public bool onPower { get; set; }
        public double batteryVoltage { get; set; }
        public int batteryPercent { get; set; }
        public int batteryLife { get; set; }
        public double fridgeTemprature { get; set; }
        public double internalModuleTemprature { get; set; }
        public double externalModuleTemprature { get; set; }
        public bool doorOpen { get; set; }
        public double lightLevel { get; set; }
        public string alarmCode { get; set; }
        public string alarmDescription { get; set; }
        public string faultCode { get; set; }
        public string faultDescription { get; set; }
        public int workingCounter { get; set; }
        public int restartCounter { get; set; }
        public string reportDateEpoch { get; set; }
        public string deviceUid { get; set; }
    }
}
