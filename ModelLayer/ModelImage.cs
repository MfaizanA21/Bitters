using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class ModelImage
    {
        public byte[] imageData {  get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public int ProductID { get; set; }
    }
}
