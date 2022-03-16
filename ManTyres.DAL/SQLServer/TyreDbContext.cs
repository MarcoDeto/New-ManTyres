using ManTyres.DAL.SQLServer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ManTyres.DAL.SQLServer
{
	public partial class TyreDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{

		public TyreDbContext(DbContextOptions<TyreDbContext> options) : base(options) { }

		public virtual DbSet<Clienti> Clienti { get; set; }
		public virtual DbSet<Depositi> Depositi { get; set; }
		public virtual DbSet<Inventario> Inventario { get; set; }
		public virtual DbSet<Pneumatici> Pneumatici { get; set; }
		public virtual DbSet<Sedi> Sedi { get; set; }
		public virtual DbSet<Stagioni> Stagioni { get; set; }
		public virtual DbSet<Veicoli> Veicoli { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Inventario>()
				 .HasKey(p => new { p.PneumaticiId, p.InizioDeposito });

			base.OnModelCreating(modelBuilder);

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}

