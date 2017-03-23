namespace garesTP10
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("gare")]
    public partial class gare
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public gare()
        {
            lignes = new HashSet<ligne>();
            natures = new HashSet<nature>();
        }

        [Key]
        public int id_gare { get; set; }

        [StringLength(50)]
        public string nom_gare { get; set; }

        public int numero_ville { get; set; }

        public virtual ville ville { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ligne> lignes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nature> natures { get; set; }
    }
}
