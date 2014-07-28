using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using RegawMOD.Android;


namespace MediaLibUtil
{
    public class Devices
    {

        AndroidController android;
        Device device;

        public event EventHandler TableDataChanged;

        private List<Device> tableData;
        public List<Device> TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                if (TableDataChanged != null)
                {
                    TableDataChanged(this, EventArgs.Empty);
                }
            }
        }
        public Devices()
        {

        }
    }
    public class GetDevices
    {
        public static AndroidController GetDeviceData()
        {
            AndroidController android;
            Device device;
            android = AndroidController.Instance;
            if (android.HasConnectedDevices)
            {
                int num = android.ConnectedDevices.Count;
                for (int i = 0; i < android.ConnectedDevices.Count; i++)
                {
                    string serial = android.ConnectedDevices[i];
                    device = android.GetConnectedDevice(serial);
                    string devserial = device.SerialNumber;
                    Phone phone = device.Phone;
                    DeviceState state = device.State;
                    string test = "\"";
                    string prodcmd = @"shell cat /system/build.prop | grep ""product""";
                    Debug.WriteLine("Single quote: " + @"""");
                    Debug.WriteLine(@"Single quote: "" .");
                    AdbCommand adbCmdCard = Adb.FormAdbCommand(device, "shell df /storage/*Card*");
                    AdbCommand adbCmdcard = Adb.FormAdbCommand(device, "shell df /storage/*card*");
                    AdbCommand adbCmddisc = Adb.FormAdbCommand(device, prodcmd);
                    string exp = @"\s+";
                    //AdbCommand cmd;
                    //cmd = Adb.FormAdbCommand(device, "shell df /storage/*card*", null);
                    string Card = Adb.ExecuteAdbCommand(adbCmdCard);
                    string card = Adb.ExecuteAdbCommand(adbCmdcard);
                    string descript = Adb.ExecuteAdbCommand(adbCmddisc);
                    string[] group1 = Regex.Split(Card, exp);
                    string[] group2 = Regex.Split(card, exp);

                }
            }
            return android;
        }
    }
}
