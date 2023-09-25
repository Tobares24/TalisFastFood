using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Models
{
    internal partial class MesaMetaData
    {
        public int ID_MESA { get; set; }
        [Display(Name = "Código")]
        public string COD_MESA { get; set; }
        [Display(Name = "Capacidad")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CAPACIDAD { get; set; }
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int ID_ESTADO_MESA { get; set; }
        [Display(Name = "Restaurante")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int ID_RESTAURANTE { get; set; }
        [Display(Name = "Restaurante")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public virtual RESTAURANTE RESTAURANTE { get; set; }
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public virtual ESTADO_MESA ESTADO_MESA { get; set; }
    }

    internal partial class FacturaMetaData
    {
        [Display(Name = "Número de Factura")]
        public int ID_FACTURA { get; set; }
        [Display(Name = "Subtotal")]
        public double SUBTOTAL { get; set; }
        [Display(Name = "Impuesto")]
        public double IMPUESTO { get; set; }
        [Display(Name = "Total")]
        public double TOTAL { get; set; }
        [Display(Name = "Fecha")]
        public DateTime FECHA { get; set; }
        [Display(Name = "Número de Orden")]
        public int ID_ORDEN { get; set; }
        [Display(Name = "Número de Mesa")]
        public int ID_MESA { get; set; }
        public virtual ORDEN ORDEN { get; set; }
        public virtual MESA MESA { get; set; }
    }

    internal partial class EstadoMesaMetadata
    {
        [Display(Name = "Código")]
        public int ID_ESTADO_MESA { get; set; }

        [Display(Name = "Descripción")]
        public string ESTADO { get; set; }

        public virtual ICollection<MESA> MESA { get; set; }
    }

    internal partial class RestauranteMetadata
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "El campo {0} restaurante es requerido")]
        public int ID_RESTAURANTE { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NOMBRE { get; set; }

        [Display(Name = "Ubicación")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string UBICACION { get; set; }

        [Display(Name = "Mesas")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public virtual ICollection<MESA> MESA { get; set; }
    }

    internal partial class ProductoMetaData
    {
        [Display(Name = "Código")]
        public int ID_PRODUCTO { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NOMBRE { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string DESCRIPCION { get; set; }

        [Display(Name = "Ingredientes")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string INGREDIENTES { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public double PRECIO { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public bool ESTADO { get; set; }

        [Display(Name = "Url fotografía")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string FOTOGRAFIA { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int ID_CATEGORIA { get; set; }
    }

    internal partial class OrdenMetaData
    {

        [Display(Name = "Número")]
        public int ID_ORDEN { get; set; }

        [Display(Name = "Usuario")]
        public string ID_USUARIO { get; set; }

        [Display(Name = "Estado")]
        public int ESTADO_ORDEN { get; set; }

        public DateTime FECHA { get; set; }

        [Display(Name = "Mesa")]
        public int ID_MESA { get; set; }
    }

    internal partial class EstadoOrdenMetaData
    {

        [Display(Name = "Estado")]
        public int ID_ESTADO_ORDEN { get; set; }
        public string ESTADO { get; set; }
    }

    internal partial class UsuarioMetaData
    {
        [Display(Name = "Cédula")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string ID_USUARIO { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string NOMBRE { get; set; }

        [Display(Name = "Primer Apellido")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string PRIMER_APELLIDO { get; set; }

        [Display(Name = "Segundo Apellido")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string SEGUNDO_APELLIDO { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string TELEFONO { get; set; }

        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "{0} no tiene formato válido")]
        public string EMAIL { get; set; }

        [Display(Name = "Clave")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string CLAVE { get; set; }

        [Display(Name = "Rol")]
        public int ID_ROL { get; set; }

        [Display(Name = "Restaurante")]
        public int ID_RESTAURANTE { get; set; }
    }
}