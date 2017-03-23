namespace garesTP10
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class garesDataModel : DbContext
    {
        public garesDataModel()
            : base("name=garesDataModel")
        {
        }

        public virtual DbSet<cp> cps { get; set; }
        public virtual DbSet<gare> gares { get; set; }
        public virtual DbSet<ligne> lignes { get; set; }
        public virtual DbSet<nature> natures { get; set; }
        public virtual DbSet<ville> villes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<cp>()
                .HasMany(e => e.villes)
                .WithMany(e => e.cps)
                .Map(m => m.ToTable("inclus").MapLeftKey("code_postal").MapRightKey("numero_ville"));

            modelBuilder.Entity<gare>()
                .Property(e => e.nom_gare)
                .IsUnicode(false);

            modelBuilder.Entity<gare>()
                .HasMany(e => e.lignes)
                .WithMany(e => e.gares)
                .Map(m => m.ToTable("peut_contenir").MapLeftKey("id_gare").MapRightKey("numero_ligne"));

            modelBuilder.Entity<gare>()
                .HasMany(e => e.natures)
                .WithMany(e => e.gares)
                .Map(m => m.ToTable("possede").MapLeftKey("id_gare").MapRightKey("numero_nature"));

            modelBuilder.Entity<nature>()
                .Property(e => e.nom_nature)
                .IsUnicode(false);

            modelBuilder.Entity<ville>()
                .Property(e => e.nom_ville)
                .IsUnicode(false);

            modelBuilder.Entity<ville>()
                .HasMany(e => e.gares)
                .WithRequired(e => e.ville)
                .WillCascadeOnDelete(false);
        }
    }
}
