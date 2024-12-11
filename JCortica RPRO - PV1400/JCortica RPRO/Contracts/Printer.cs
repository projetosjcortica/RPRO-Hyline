using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteImpresao.Contracts
{
    public interface IPrinter
    {
        void SetPrinterSettings(PrinterSettings settings);
        void PrintString(string value);
        void PrintFile(string filePath);
    }
}
