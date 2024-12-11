using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leonardo.Contracts;

namespace Leonardo.Infrastructure.Printers
{
    public class PrinterZebra
    {
        private PrinterSettings Settings { get; set; } = new PrinterSettings();

        public PrinterZebra (PrinterSettings settings)
        {
            Settings = settings;
        }
        public void SetPrinterSettings(PrinterSettings settings)
        {
            Settings = settings;
        }


        public void PrintFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void PrintString(string value)
        {
            string PrinterPath = GetPrinterPath();

            RawPrinterHelper.SendStringToPrinter(PrinterPath, value);
        }

        private string GetPrinterPath()
        {
            switch (Settings.Type)
            {
                case PrinterType.Network:
                    return $"\\{Settings.Name}";

                case PrinterType.Local:
                    return $@"\\{Environment.MachineName}\{Settings.Name}";

                case PrinterType.Unknown:
                    throw new Exception("Impressora não definida!");
                default:
                   throw new Exception("Impressora não definida!");
            }
        }

        public void PrintLabel(ILabel label)
        {
            PrintString(label.GetLabel());
        }


    }
}
