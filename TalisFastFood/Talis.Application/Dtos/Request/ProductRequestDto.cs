using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talis.Application.Dtos
{
    public class ProductRequestDto
    {
        public int CodProducto { get; set; }
        public byte[] Fotografía { get; set; }
        public string Nombre { get; set; }
        public string DscProducto { get; set; }
        public int CantidadMaxima { get; set; }
        public int CantidadExistencia { get; set; }
        public int LogActivo { get; set; }
        public decimal Precio { get; set; }
    }
}
