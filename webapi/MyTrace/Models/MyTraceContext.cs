using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyTrace.Models;

public partial class MyTraceContext : DbContext
{
    public MyTraceContext()
    {
    }

    public MyTraceContext(DbContextOptions<MyTraceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientsAddress> ClientsAddresses { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<ComponentsType> ComponentsTypes { get; set; }

    public virtual DbSet<Lot> Lots { get; set; }

    public virtual DbSet<LotsStage> LotsStages { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<ModelsComponent> ModelsComponents { get; set; }

    public virtual DbSet<ModelsSizesColor> ModelsSizesColors { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<OrganizationsAddress> OrganizationsAddresses { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Stage> Stages { get; set; }

    public virtual DbSet<StagesModel> StagesModels { get; set; }

    public virtual DbSet<StagesModelStage> StagesModelStages { get; set; }

    public virtual DbSet<StagesType> StagesTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersType> UsersTypes { get; set; }

    public virtual DbSet<UsersTypeRoute> UsersTypeRoutes { get; set; }

    private static String getConnection()
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
        var dbEncrypt = Environment.GetEnvironmentVariable("DB_Encrypt");

        return $"Server={dbHost},{dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};Encrypt={dbEncrypt};";
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(getConnection());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId });

            entity.HasIndex(e => new { e.OrganizationId, e.Email }, "EmailOrgId_Clients").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Organization).WithMany(p => p.Clients)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clients_Organizations");
        });

        modelBuilder.Entity<ClientsAddress>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ClientId, e.OrganizationId });

            entity.ToTable("Clients_Addresses");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("clientId");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientsAddresses)
                .HasForeignKey(d => new { d.ClientId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clients_Addresses_Clients");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId });

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.Color1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("color");

            entity.HasOne(d => d.Organization).WithMany(p => p.Colors)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Colors_Organizations");
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId });

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.ComponentsTypeId).HasColumnName("componentsTypeId");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.ProviderId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("providerId");

            entity.HasOne(d => d.ComponentsType).WithMany(p => p.Components)
                .HasForeignKey(d => new { d.ComponentsTypeId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Components_ComponentsType");

            entity.HasOne(d => d.Provider).WithMany(p => p.Components)
                .HasForeignKey(d => new { d.ProviderId, d.OrganizationId })
                .HasConstraintName("FK_Components_Providers");
        });

        modelBuilder.Entity<ComponentsType>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId });

            entity.ToTable("ComponentsType");

            entity.HasIndex(e => e.ComponentType, "Unique_ComponentsType").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.ComponentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("componentType");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");

            entity.HasOne(d => d.Organization).WithMany(p => p.ComponentsTypes)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComponentsType_Organizations");
        });

        modelBuilder.Entity<Lot>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId });

            entity.HasIndex(e => e.Hash, "UK_Hash_Lots").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.CanceledAt)
                .HasColumnType("datetime")
                .HasColumnName("canceledAt");
            entity.Property(e => e.ClientAddressId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("clientAddressId");
            entity.Property(e => e.ClientId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("clientId");
            entity.Property(e => e.DeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("deliveryDate");
            entity.Property(e => e.Hash)
                .HasMaxLength(256)
                .IsFixedLength()
                .HasColumnName("hash");
            entity.Property(e => e.LotSize).HasColumnName("lotSize");
            entity.Property(e => e.ModelColorId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("modelColorId");
            entity.Property(e => e.ModelId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("modelId");
            entity.Property(e => e.ModelSizeId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("modelSizeId");
            entity.Property(e => e.OrganizationAddressId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("organizationAddressId");
            entity.Property(e => e.StagesModelId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("stagesModelId");

            entity.HasOne(d => d.Model).WithMany(p => p.Lots)
                .HasForeignKey(d => new { d.ModelId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lots_Models");

            entity.HasOne(d => d.Organization).WithMany(p => p.Lots)
                .HasForeignKey(d => new { d.OrganizationAddressId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lots_Organizations_Addresses");

            entity.HasOne(d => d.StagesModel).WithMany(p => p.Lots)
                .HasForeignKey(d => new { d.StagesModelId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lots_StagesModel");

            entity.HasOne(d => d.ClientsAddress).WithMany(p => p.Lots)
                .HasForeignKey(d => new { d.ClientAddressId, d.ClientId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lots_Clients_Addresses1");

            entity.HasOne(d => d.ModelsSizesColor).WithMany(p => p.Lots)
                .HasForeignKey(d => new { d.ModelId, d.ModelColorId, d.ModelSizeId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lots_Models_Colors");
        });

        modelBuilder.Entity<LotsStage>(entity =>
        {
            entity.HasKey(e => new { e.LotId, e.StageId, e.OrganizationId });

            entity.ToTable("Lots_Stages");

            entity.HasIndex(e => e.Hash, "UK_Hash_Lots_Stages").IsUnique();

            entity.Property(e => e.LotId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("lotId");
            entity.Property(e => e.StageId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("stageId");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.Hash)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("hash");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");

            entity.HasOne(d => d.Lot).WithMany(p => p.LotsStages)
                .HasForeignKey(d => new { d.LotId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lots_Stages_Lots1");

            entity.HasOne(d => d.Stage).WithMany(p => p.LotsStages)
                .HasForeignKey(d => new { d.StageId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lots_Stages_Stages");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId });

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.ModelPhoto)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("modelPhoto");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("name");
            entity.Property(e => e.StagesModelId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("stagesModelId");

            entity.HasOne(d => d.StagesModel).WithMany(p => p.Models)
                .HasForeignKey(d => new { d.StagesModelId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Models_StagesModel");
        });

        modelBuilder.Entity<ModelsComponent>(entity =>
        {
            entity.HasKey(e => new { e.ModelId, e.ComponentsId, e.OrganizationId });

            entity.ToTable("Models_Components");

            entity.Property(e => e.ModelId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("modelId");
            entity.Property(e => e.ComponentsId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("componentsId");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.Component).WithMany(p => p.ModelsComponents)
                .HasForeignKey(d => new { d.ComponentsId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Models_Components_Components");

            entity.HasOne(d => d.Model).WithMany(p => p.ModelsComponents)
                .HasForeignKey(d => new { d.ModelId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Models_Components_Models");
        });

        modelBuilder.Entity<ModelsSizesColor>(entity =>
        {
            entity.HasKey(e => new { e.ModelId, e.ColorId, e.SizeId, e.OrganizationId }).HasName("PK_Models_Colors");

            entity.ToTable("Models_Sizes_Colors");

            entity.Property(e => e.ModelId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("modelId");
            entity.Property(e => e.ColorId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("colorId");
            entity.Property(e => e.SizeId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("sizeId");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");

            entity.HasOne(d => d.Color).WithMany(p => p.ModelsSizesColors)
                .HasForeignKey(d => new { d.ColorId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Models_Colors_Colors");

            entity.HasOne(d => d.Model).WithMany(p => p.ModelsSizesColors)
                .HasForeignKey(d => new { d.ModelId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Models_Colors_Models");

            entity.HasOne(d => d.Size).WithMany(p => p.ModelsSizesColors)
                .HasForeignKey(d => new { d.SizeId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Models_Colors_Sizes");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Organization");

            entity.HasIndex(e => e.Email, "UK_Email_Organizations").IsUnique();

            entity.HasIndex(e => e.WalletAddress, "UK_WalletAddress").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Logo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Photo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("photo");
            entity.Property(e => e.RegexComponentsType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexComponentsType");
            entity.Property(e => e.RegexIdClients)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdClients");
            entity.Property(e => e.RegexIdColors)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdColors");
            entity.Property(e => e.RegexIdCoponents)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdCoponents");
            entity.Property(e => e.RegexIdLots)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdLots");
            entity.Property(e => e.RegexIdModels)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdModels");
            entity.Property(e => e.RegexIdOrganizationsAddresses)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdOrganizations_Addresses");
            entity.Property(e => e.RegexIdProviders)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdProviders");
            entity.Property(e => e.RegexIdSizes)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdSizes");
            entity.Property(e => e.RegexIdStates)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdStates");
            entity.Property(e => e.RegexIdStatesModel)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdStatesModel");
            entity.Property(e => e.RegexIdStatesType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("regexIdStatesType");
            entity.Property(e => e.WalletAddress)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("walletAddress");
        });

        modelBuilder.Entity<OrganizationsAddress>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId });

            entity.ToTable("Organizations_Addresses");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("zipcode");

            entity.HasOne(d => d.Organization).WithMany(p => p.OrganizationsAddresses)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Organizations_Addresses_Organizations");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId });

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Organization).WithMany(p => p.Providers)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Providers_Organizations");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.route);

            entity.Property(e => e.route)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("route");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId }).HasName("PK_Size");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.Size1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("size");

            entity.HasOne(d => d.Organization).WithMany(p => p.Sizes)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sizes_Organizations");
        });

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId });

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.StageDescription)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("stageDescription");
            entity.Property(e => e.StageName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stageName");
            entity.Property(e => e.StagesTypeId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("stagesTypeId");

            entity.HasOne(d => d.Organization).WithMany(p => p.Stages)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stages_Organizations");

            entity.HasOne(d => d.StagesType).WithMany(p => p.Stages)
                .HasForeignKey(d => d.StagesTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stages_StagesType");
        });

        modelBuilder.Entity<StagesModel>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OrganizationId });

            entity.ToTable("StagesModel");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.StagesModelName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stagesModelName");

            entity.HasOne(d => d.Organization).WithMany(p => p.StagesModels)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StagesModel_Organizations");
        });

        modelBuilder.Entity<StagesModelStage>(entity =>
        {
            entity.HasKey(e => new { e.StagesModelId, e.StagesId, e.OrganizationId });

            entity.ToTable("StagesModel_Stages");

            entity.Property(e => e.StagesModelId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("stagesModelId");
            entity.Property(e => e.StagesId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("stagesId");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.Position).HasColumnName("position");

            entity.HasOne(d => d.Stage).WithMany(p => p.StagesModelStages)
                .HasForeignKey(d => new { d.StagesId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StagesModel_Stages_Stages");

            entity.HasOne(d => d.StagesModel).WithMany(p => p.StagesModelStages)
                .HasForeignKey(d => new { d.StagesModelId, d.OrganizationId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StagesModel_Stages_StagesModel");
        });

        modelBuilder.Entity<StagesType>(entity =>
        {
            entity.ToTable("StagesType");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.StageType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stageType");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.WalletAddress);

            entity.HasIndex(e => e.Email, "UK_Email_Users").IsUnique();

            entity.Property(e => e.WalletAddress)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("walletAddress");
            entity.Property(e => e.Avatar)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("avatar");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birthDate");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Nonce).HasColumnName("nonce");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.UserTypeId).HasColumnName("userTypeId");

            entity.HasOne(d => d.Organization).WithMany(p => p.Users)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK_Users_Organizations");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_UsersType");
        });

        modelBuilder.Entity<UsersType>(entity =>
        {
            entity.ToTable("UsersType");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.UserType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userType");
        });

        modelBuilder.Entity<UsersTypeRoute>(entity =>
        {
            entity.HasKey(e => new { e.UserTypeId, e.Route }).HasName("PK_UsersType/Routes");

            entity.ToTable("UsersType_Routes");

            entity.Property(e => e.UserTypeId).HasColumnName("userTypeId");
            entity.Property(e => e.Route)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("route");
            entity.Property(e => e.Permissions)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("permissions");

            entity.HasOne(d => d.RoutesNavigation).WithMany(p => p.UsersTypeRoutes)
                .HasForeignKey(d => d.Route)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersType_Routes_Routes");

            entity.HasOne(d => d.UserType).WithMany(p => p.UsersTypeRoutes)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersType_Routes_UsersType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
