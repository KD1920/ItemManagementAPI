using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ItemManagement.Data;

public partial class ItemManagementDbContext : DbContext
{
    public ItemManagementDbContext()
    {
    }

    public ItemManagementDbContext(DbContextOptions<ItemManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ItemModel> ItemModels { get; set; }

    public virtual DbSet<ItemRequest> ItemRequests { get; set; }

    public virtual DbSet<ItemRequestItemModel> ItemRequestItemModels { get; set; }

    public virtual DbSet<ItemReturnRequest> ItemReturnRequests { get; set; }

    public virtual DbSet<ItemReturnRequestItemModel> ItemReturnRequestItemModels { get; set; }

    public virtual DbSet<ItemType> ItemTypes { get; set; }

    public virtual DbSet<PurchaseRequest> PurchaseRequests { get; set; }

    public virtual DbSet<PurchaseRequestItemModel> PurchaseRequestItemModels { get; set; }

    public virtual DbSet<ReturnStatus> ReturnStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserItem> UserItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=online-db-asp-dot-net-demo.h.aivencloud.com;Port=27072;Database=itemManagement;Username=avnadmin;Password=AVNS_D5mjnMB2Rf9paXPKYew;SSL Mode=Require;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("itemmodel_pkey");

            entity.ToTable("item_model");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('itemmodel_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ItemTypeId).HasColumnName("item_type_id");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemModelCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("itemmodel_created_by_fkey");

            entity.HasOne(d => d.ItemType).WithMany(p => p.ItemModels)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("itemmodel_item_type_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ItemModelModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("itemmodel_modified_by_fkey");
        });

        modelBuilder.Entity<ItemRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("itemrequest_pkey");

            entity.ToTable("item_request");

            entity.HasIndex(e => e.RequestNumber, "itemrequest_request_number_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('itemrequest_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("request_date");
            entity.Property(e => e.RequestNumber)
                .HasMaxLength(50)
                .HasColumnName("request_number");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemRequestCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("itemrequest_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ItemRequestModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("itemrequest_modified_by_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.ItemRequests)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("itemrequest_status_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ItemRequestUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("itemrequest_user_id_fkey");
        });

        modelBuilder.Entity<ItemRequestItemModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("itemrequestitem_pkey");

            entity.ToTable("item_request_item_model");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('itemrequestitem_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.ItemModelId).HasColumnName("item_model_id");
            entity.Property(e => e.ItemRequestId).HasColumnName("item_request_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.ItemModel).WithMany(p => p.ItemRequestItemModels)
                .HasForeignKey(d => d.ItemModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("itemrequestitem_item_model_id_fkey");

            entity.HasOne(d => d.ItemRequest).WithMany(p => p.ItemRequestItemModels)
                .HasForeignKey(d => d.ItemRequestId)
                .HasConstraintName("itemrequestitem_item_request_id_fkey");
        });

        modelBuilder.Entity<ItemReturnRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("item_return_request_pkey");

            entity.ToTable("item_return_request");

            entity.HasIndex(e => e.RequestNumber, "item_return_request_request_number_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("request_date");
            entity.Property(e => e.RequestNumber)
                .HasMaxLength(50)
                .HasColumnName("request_number");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemReturnRequestCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("item_return_request_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ItemReturnRequestModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("item_return_request_modified_by_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.ItemReturnRequests)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_return_request_status_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ItemReturnRequestUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_return_request_user_id_fkey");
        });

        modelBuilder.Entity<ItemReturnRequestItemModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("item_return_request_item_model_pkey");

            entity.ToTable("item_return_request_item_model");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItemModelId).HasColumnName("item_model_id");
            entity.Property(e => e.ItemRequestId).HasColumnName("item_request_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.ItemModel).WithMany(p => p.ItemReturnRequestItemModels)
                .HasForeignKey(d => d.ItemModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_return_request_item_model_item_model_id_fkey");

            entity.HasOne(d => d.ItemRequest).WithMany(p => p.ItemReturnRequestItemModels)
                .HasForeignKey(d => d.ItemRequestId)
                .HasConstraintName("item_return_request_item_model_item_request_id_fkey");
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("itemtype_pkey");

            entity.ToTable("item_type");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('itemtype_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ItemTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("itemtype_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ItemTypeModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("itemtype_modified_by_fkey");
        });

        modelBuilder.Entity<PurchaseRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purchase_request_pkey");

            entity.ToTable("purchase_request");

            entity.HasIndex(e => e.InvoiceNumber, "purchase_request_invoice_number_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(50)
                .HasColumnName("invoice_number");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("request_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserName)
                .HasColumnType("character varying")
                .HasColumnName("user_name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchaseRequestCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("purchase_request_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchaseRequestModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("purchase_request_modified_by_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.PurchaseRequestUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("purchase_request_user_id_fkey");
        });

        modelBuilder.Entity<PurchaseRequestItemModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purchase_request_item_models_pkey");

            entity.ToTable("purchase_request_item_models");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.ItemModelId).HasColumnName("item_model_id");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.PurchaseRequestId).HasColumnName("purchase_request_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchaseRequestItemModelCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("purchase_request_item_models_created_by_fkey");

            entity.HasOne(d => d.ItemModel).WithMany(p => p.PurchaseRequestItemModels)
                .HasForeignKey(d => d.ItemModelId)
                .HasConstraintName("purchase_request_item_models_item_model_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PurchaseRequestItemModelModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("purchase_request_item_models_modified_by_fkey");

            entity.HasOne(d => d.PurchaseRequest).WithMany(p => p.PurchaseRequestItemModels)
                .HasForeignKey(d => d.PurchaseRequestId)
                .HasConstraintName("purchase_request_item_models_purchase_request_id_fkey");
        });

        modelBuilder.Entity<ReturnStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("return_status_pkey");

            entity.ToTable("return_status");

            entity.HasIndex(e => e.Name, "return_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReturnStatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("return_status_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ReturnStatusModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("return_status_modified_by_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "role_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RoleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("role_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.RoleModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("role_modified_by_fkey");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.ToTable("status");

            entity.HasIndex(e => e.Name, "status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("status_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.StatusModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("status_modified_by_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IsAdmin)
                .HasDefaultValue(false)
                .HasColumnName("is_admin");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("users_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.InverseModifiedByNavigation)
                .HasForeignKey(d => d.ModifiedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("users_modified_by_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_users_role");
        });

        modelBuilder.Entity<UserItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_item_pkey");

            entity.ToTable("user_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_on");
            entity.Property(e => e.ItemModelId).HasColumnName("item_model_id");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_on");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UserItemCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("user_item_created_by_fkey");

            entity.HasOne(d => d.ItemModel).WithMany(p => p.UserItems)
                .HasForeignKey(d => d.ItemModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_item_item_model_id_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.UserItemModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("user_item_modified_by_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserItemUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_item_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
