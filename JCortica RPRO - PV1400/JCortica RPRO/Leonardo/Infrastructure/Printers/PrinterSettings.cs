using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leonardo.Infrastructure.Printers
{
    public class PrinterSettings
    {
        public PrinterType Type { get; private set; }
        public string Name { get; private set; }
        private string ComputerName { get; set; } = String.Empty;

        public PrinterSettings()
        {
            Type = PrinterType.Unknown;
        }

        public PrinterSettings(string name, PrinterType type)
        {
            Name = name;
            Type = type;
        }

        public PrinterSettings SetType(PrinterType type)
        {
            Type = type;
            return this;
        }

        public PrinterSettings SetName(string name)
        {
            Name = name;
            return this;
        }

        public PrinterSettings SetComputerName(string computerName)
        {
            ComputerName = computerName;
            return this;
        }

        public string GetComputerName()
        {
            if (ComputerName == String.Empty)
            {
                return Environment.MachineName;
            }
            return ComputerName;
        }
    }
}
