using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Threading;

namespace SharePointCommands
{
    public class ShPFarm
    {
        public event EventHandler TableDataChanged;

        private List<string> tableData;
        public List<string> TableData
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

        public ShPFarm()
        {

        }
    }
    public class GetFarm
    {
        public static ShPFarm GetFarmData(string url)
        {
            ShPFarm table = new ShPFarm();
            table.TableData = new List<string>();
            PowerShell command = PowerShell.Create();
            command.AddScript(@"$SPOCred = (GCI C:\*\SPOCreds.PS1).fullname
            invoke-expression -Command $SPOCred
            $servers = Get-Content '\\SRVCORDC01\Scripts\SharePoint\Management\ProductionServers.txt'
            foreach($server in $servers)
            {
                Enable-WSManCredSSP Client –DelegateComputer $server -Force
                invoke-command -ComputerName $server{Enable-WSManCredSSP Server -Force}
            }");
            PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
            outputCollection.DataAdded += outputCollection_DataAdded;
            command.Streams.Error.DataAdded += Error_DataAdded;
            IAsyncResult result = command.BeginInvoke<PSObject, PSObject>(null, outputCollection);
            while (result.IsCompleted == false)
            {
                Console.WriteLine("Waiting for pipeline to finish...");
                Thread.Sleep(1000);

                // might want to place a timeout here...
            }
            Console.WriteLine("Execution has stopped. The pipeline state: " + command.InvocationStateInfo.State);

            foreach (PSObject outputItem in outputCollection)
            {
                //TODO: handle/process the output items if required
                Console.WriteLine(outputItem.BaseObject.ToString());
                table.TableData.Add(outputItem.BaseObject.ToString());
            }
            
            return table;
        }

        private static void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void outputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
