using Leonardo.Infrastructure.Printers;
using TesteImpresao.Entities;

namespace Leonardo.Services
{
    public class LabelServices
    {
        public void PrintLabel(LabelZebra label)
        {
            var printerName = JCortica_RPRO.Properties.Settings.Default.NomeImpressora;
            var escuridao = JCortica_RPRO.Properties.Settings.Default.EscuridaoEtiqueta;
            PrinterType printerType = PrinterType.Local;

            if(JCortica_RPRO.Properties.Settings.Default.TipoImpressora.Equals("Rede"))
                printerType = PrinterType.Network;

            var PrinterSettings = new PrinterSettings(printerName, printerType);

            var Printer = new PrinterZebra(PrinterSettings);

            label.SetEscuridao(escuridao);
            label.CreateLabel();

            Printer.PrintLabel(label);
        }
    }
}
