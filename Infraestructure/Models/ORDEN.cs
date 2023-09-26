//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infraestructure.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(OrdenMetaData))]
    public partial class ORDEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ORDEN()
        {
            this.DETALLE_ORDEN = new HashSet<DETALLE_ORDEN>();
            this.FACTURA = new HashSet<FACTURA>();
        }
    
        public int ID_ORDEN { get; set; }
        public string ID_USUARIO { get; set; }
        public int ESTADO_ORDEN { get; set; }
        public System.DateTime FECHA { get; set; }
        public int ID_MESA { get; set; }
        public Nullable<int> ID_FACTURA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETALLE_ORDEN> DETALLE_ORDEN { get; set; }
        public virtual ESTADO_ORDEN ESTADO_ORDEN1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FACTURA> FACTURA { get; set; }
        public virtual MESA MESA { get; set; }
        public virtual USUARIO USUARIO { get; set; }
    }
}
