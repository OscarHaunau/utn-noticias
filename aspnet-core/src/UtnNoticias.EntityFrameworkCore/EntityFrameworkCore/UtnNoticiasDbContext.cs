using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using UtnNoticias.Alerts;
using UtnNoticias.Monitoring;
using UtnNoticias.ReadingLists;
using UtnNoticias.Themes;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace UtnNoticias.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class UtnNoticiasDbContext :
	AbpDbContext<UtnNoticiasDbContext>,
	IIdentityDbContext,
	ITenantManagementDbContext
{
	/* Add DbSet properties for your Aggregate Roots / Entities here. */

	#region Entities from the modules

	/* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

	//Identity
	public DbSet<IdentityUser> Users { get; set; }
	public DbSet<IdentityRole> Roles { get; set; }
	public DbSet<IdentityClaimType> ClaimTypes { get; set; }
	public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
	public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
	public DbSet<IdentityLinkUser> LinkUsers { get; set; }
	public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

	// Tenant Management
	public DbSet<Tenant> Tenants { get; set; }
	public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

	//Domain
	public DbSet<Acceso> Accesos { get; set; }
	public DbSet<Alerta> Alertas { get; set; }
	public DbSet<Busqueda> Busquedas { get; set; }
	public DbSet<ErrorAcceso> ErrorAccesos { get; set; }

	public DbSet<ListaContenedor> ListaContenedores { get; set; }
	public DbSet<Noticia> Noticias { get; set; }
	public DbSet<PanelMonitoreo> PanelMonitoreos { get; set; }
	public DbSet<RegistroUsuario> RegistroUsuarios { get; set; }
	public DbSet<Usuario> Usuarios { get; set; }

	public DbSet<Theme> Themes { get; set; }
	public DbSet<ReadingList> ReadingLists { get; set; }
	public DbSet<ReadingListItem> ReadingListItems { get; set; }
		// Libro: p.55 explica que para guardar entidades hay que agregarlas al DbContext.
	public DbSet<NewsAlert> NewsAlerts { get; set; }
	public DbSet<NewsNotification> NewsNotifications { get; set; }
	public DbSet<ApiAccessLog> ApiAccessLogs { get; set; }

	#endregion

	public UtnNoticiasDbContext(DbContextOptions<UtnNoticiasDbContext> options)
		: base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		/* Include modules to your migration db context */

		builder.ConfigurePermissionManagement();
		builder.ConfigureSettingManagement();
		builder.ConfigureBackgroundJobs();
		builder.ConfigureAuditLogging();
		builder.ConfigureIdentity();
		builder.ConfigureOpenIddict();
		builder.ConfigureFeatureManagement();
		builder.ConfigureTenantManagement();

		/* Configure your own tables/entities inside here */

		//builder.Entity<YourEntity>(b =>
		//{
		//    b.ToTable(UtnNoticiasConsts.DbTablePrefix + "YourEntities", UtnNoticiasConsts.DbSchema);
		//    b.ConfigureByConvention(); //auto configure for the base class props
		//    //...
		//});

		// Entidad Theme
		builder.Entity<Theme>(b =>
		{
			b.ToTable(UtnNoticiasConsts.DbTablePrefix + "Themes", UtnNoticiasConsts.DbSchema);
			b.ConfigureByConvention();
			b.Property(x => x.Name).IsRequired().HasMaxLength(128);
		});

		builder.Entity<ReadingList>(b =>
		{
			b.ToTable(UtnNoticiasConsts.DbTablePrefix + "ReadingLists", UtnNoticiasConsts.DbSchema);
			b.ConfigureByConvention();
			b.Property(x => x.Name).IsRequired().HasMaxLength(ReadingListConsts.MaxNameLength);
			b.HasIndex(x => new { x.OwnerId, x.Name });
			b.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.ReadingListId).OnDelete(DeleteBehavior.Cascade);
		});

		builder.Entity<ReadingListItem>(b =>
		{
			b.ToTable(UtnNoticiasConsts.DbTablePrefix + "ReadingListItems", UtnNoticiasConsts.DbSchema);
			b.ConfigureByConvention();
			b.Property(x => x.Title).IsRequired().HasMaxLength(ReadingListConsts.MaxTitleLength);
			b.Property(x => x.Url).IsRequired().HasMaxLength(ReadingListConsts.MaxUrlLength);
			b.Property(x => x.Author).HasMaxLength(ReadingListConsts.MaxAuthorLength);
			b.Property(x => x.Description).HasMaxLength(ReadingListConsts.MaxDescriptionLength);
			b.Property(x => x.UrlToImage).HasMaxLength(ReadingListConsts.MaxImageUrlLength);
			b.Property(x => x.Content).HasMaxLength(ReadingListConsts.MaxContentLength);
			b.HasIndex(x => new { x.ReadingListId, x.Url }).IsUnique();
		});

				// Libro: p.55: configuracion de tablas para que EF Core pueda crear/migrar la base.
		builder.Entity<NewsAlert>(b =>
		{
			b.ToTable(UtnNoticiasConsts.DbTablePrefix + "NewsAlerts", UtnNoticiasConsts.DbSchema);
			b.ConfigureByConvention();
			b.Property(x => x.SearchText).IsRequired().HasMaxLength(200);
			b.HasIndex(x => new { x.OwnerId, x.SearchText });
			b.HasMany(x => x.Notifications).WithOne().HasForeignKey(x => x.AlertId).OnDelete(DeleteBehavior.Cascade);
		});

		builder.Entity<NewsNotification>(b =>
		{
			b.ToTable(UtnNoticiasConsts.DbTablePrefix + "NewsNotifications", UtnNoticiasConsts.DbSchema);
			b.ConfigureByConvention();
			b.Property(x => x.Title).IsRequired().HasMaxLength(256);
			b.Property(x => x.Url).IsRequired().HasMaxLength(2048);
			b.HasIndex(x => new { x.AlertId, x.Url }).IsUnique();
		});

		builder.Entity<ApiAccessLog>(b =>
		{
			b.ToTable(UtnNoticiasConsts.DbTablePrefix + "ApiAccessLogs", UtnNoticiasConsts.DbSchema);
			b.ConfigureByConvention();
			b.Property(x => x.SearchText).IsRequired().HasMaxLength(200);
			b.Property(x => x.ErrorMessage).HasMaxLength(2000);
		});
	}
}
