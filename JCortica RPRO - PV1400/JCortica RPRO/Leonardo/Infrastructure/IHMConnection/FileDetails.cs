using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteImpresao.Infrastructure.IHMConnection
{
    public class FileDetails
    {
        public string Name { get; set; }
        public DateTime ModificationDate { get; set; }
        public FileDetails()
        {
            
        }

        public FileDetails(string name, DateTime date)
        {
            Name = name;
            ModificationDate = date;
        }

       
    }
}
