namespace garesTP10
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ligne")]
    public partial class ligne
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ligne()
        {
            gares = new HashSet<gare>();
        }

        [Key]
        public int numero_ligne { get; set; }
        
        public int code_ligne { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<gare> gares { get; set; }
    }
}
