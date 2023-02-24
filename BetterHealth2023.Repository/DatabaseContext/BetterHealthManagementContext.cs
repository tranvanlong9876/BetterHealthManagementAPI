using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseModels;

#nullable disable

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext
{
    public partial class BetterHealthManagementContext : DbContext
    {
        public BetterHealthManagementContext()
        {
        }

        public BetterHealthManagementContext(DbContextOptions<BetterHealthManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<CategoryMain> CategoryMains { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CommentInformation> CommentInformations { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public virtual DbSet<CustomerPoint> CustomerPoints { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<DynamicAddress> DynamicAddresses { get; set; }
        public virtual DbSet<EventProductDiscount> EventProductDiscounts { get; set; }
        public virtual DbSet<InternalUser> InternalUsers { get; set; }
        public virtual DbSet<InternalUserWorkingSite> InternalUserWorkingSites { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<OrderBatch> OrderBatches { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderExecution> OrderExecutions { get; set; }
        public virtual DbSet<OrderHeader> OrderHeaders { get; set; }
        public virtual DbSet<OrderShipment> OrderShipments { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<OrderVoucher> OrderVouchers { get; set; }
        public virtual DbSet<ProductDescription> ProductDescriptions { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProductImportBatch> ProductImportBatches { get; set; }
        public virtual DbSet<ProductImportDetail> ProductImportDetails { get; set; }
        public virtual DbSet<ProductImportReceipt> ProductImportReceipts { get; set; }
        public virtual DbSet<ProductIngredient> ProductIngredients { get; set; }
        public virtual DbSet<ProductIngredientDescription> ProductIngredientDescriptions { get; set; }
        public virtual DbSet<ProductParent> ProductParents { get; set; }
        public virtual DbSet<PurchaseVoucherHistory> PurchaseVoucherHistories { get; set; }
        public virtual DbSet<RoleInternal> RoleInternals { get; set; }
        public virtual DbSet<SiteInformation> SiteInformations { get; set; }
        public virtual DbSet<SiteInventory> SiteInventories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<VoucherCustomerRestriction> VoucherCustomerRestrictions { get; set; }
        public virtual DbSet<VoucherDetail> VoucherDetails { get; set; }
        public virtual DbSet<VoucherShop> VoucherShops { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:BetterHealthDatabase");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Blog_Employee");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.CommentInformation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CommentInformationId)
                    .HasConstraintName("FK_Comment_Comment_Information");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Product_Details");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Comment_Customer");
            });

            modelBuilder.Entity<CommentInformation>(entity =>
            {
                entity.Property(e => e.LastName).IsFixedLength(true);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Iso).IsUnicode(false);

                entity.Property(e => e.Iso3).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasOne(d => d.Address)
                    .WithMany(p => p.CustomerAddresses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Address_Dynamic_Address");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerAddresses)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Address_Customer");
            });

            modelBuilder.Entity<CustomerPoint>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerPoints)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Card_Customer");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasOne(d => d.City)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_District_City");
            });

            modelBuilder.Entity<DynamicAddress>(entity =>
            {
                entity.HasOne(d => d.City)
                    .WithMany(p => p.DynamicAddresses)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Dynamic_Address_City");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.DynamicAddresses)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_Dynamic_Address_District");

                entity.HasOne(d => d.Ward)
                    .WithMany(p => p.DynamicAddresses)
                    .HasForeignKey(d => d.WardId)
                    .HasConstraintName("FK_Dynamic_Address_Ward");
            });

            modelBuilder.Entity<EventProductDiscount>(entity =>
            {
                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.EventProductDiscounts)
                    .HasForeignKey(d => d.DiscountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_ProductDiscount_Product_Discount");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.EventProductDiscounts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_ProductDiscount_Product_Details");
            });

            modelBuilder.Entity<InternalUser>(entity =>
            {
                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PasswordSalt).IsUnicode(false);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.InternalUsers)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Employee_Dynamic_Address");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.InternalUsers)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Internal_User_Role_Internal");
            });

            modelBuilder.Entity<InternalUserWorkingSite>(entity =>
            {
                entity.HasOne(d => d.Site)
                    .WithMany(p => p.InternalUserWorkingSites)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InternalUser_WorkingSite_Site_Information");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InternalUserWorkingSites)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InternalUser_WorkingSite_Internal_User");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Manufacturers)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Manufacturer_Country");
            });

            modelBuilder.Entity<OrderBatch>(entity =>
            {
                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.OrderBatches)
                    .HasForeignKey(d => d.BatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Batches_ProductImport_Batches");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderBatches)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Batches_OrderHeader");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_OrderHeader");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Product_Details");
            });

            modelBuilder.Entity<OrderExecution>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderExecutions)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Execution_OrderHeader");

                entity.HasOne(d => d.StatusChangeFromNavigation)
                    .WithMany(p => p.OrderExecutionStatusChangeFromNavigations)
                    .HasForeignKey(d => d.StatusChangeFrom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Execution_OrderStatus");

                entity.HasOne(d => d.StatusChangeToNavigation)
                    .WithMany(p => p.OrderExecutionStatusChangeToNavigations)
                    .HasForeignKey(d => d.StatusChangeTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Execution_OrderStatus1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OrderExecutions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Execution_Employee");
            });

            modelBuilder.Entity<OrderHeader>(entity =>
            {
                entity.Property(e => e.DiscountPrice).IsFixedLength(true);

                entity.Property(e => e.SubTotalPrice).IsFixedLength(true);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_OrderHeader_Customer");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderHeader_Employee");

                entity.HasOne(d => d.OrderStatusNavigation)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.OrderStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderHeader_OrderStatus");

                entity.HasOne(d => d.OrderType)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.OrderTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderHeader_Order_Type");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderHeader_Site_Information");
            });

            modelBuilder.Entity<OrderShipment>(entity =>
            {
                entity.HasOne(d => d.DestinationAddress)
                    .WithMany(p => p.OrderShipmentDestinationAddresses)
                    .HasForeignKey(d => d.DestinationAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Shipment_Dynamic_Address");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderShipments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Shipment_OrderHeader");

                entity.HasOne(d => d.StartAddress)
                    .WithMany(p => p.OrderShipmentStartAddresses)
                    .HasForeignKey(d => d.StartAddressId)
                    .HasConstraintName("FK_Order_Shipment_Dynamic_Address1");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasOne(d => d.ApplyForTypeNavigation)
                    .WithMany(p => p.OrderStatuses)
                    .HasForeignKey(d => d.ApplyForType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderStatus_Order_Type");
            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<OrderVoucher>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderVouchers)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Voucher_OrderHeader");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.OrderVouchers)
                    .HasForeignKey(d => d.VoucherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Voucher_Voucher");
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.Property(e => e.BarCode).IsUnicode(false);

                entity.HasOne(d => d.ProductIdParentNavigation)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.ProductIdParent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Details_Product");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Details_Unit");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Image_Product_Details");
            });

            modelBuilder.Entity<ProductImportBatch>(entity =>
            {
                entity.HasOne(d => d.ImportDetail)
                    .WithMany(p => p.ProductImportBatches)
                    .HasForeignKey(d => d.ImportDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductImport_Batches_ProductImport_Details");
            });

            modelBuilder.Entity<ProductImportDetail>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImportDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WarehouseEntry_Receipt_ProductDetails_Product_Details");

                entity.HasOne(d => d.Receipt)
                    .WithMany(p => p.ProductImportDetails)
                    .HasForeignKey(d => d.ReceiptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WarehouseEntry_Receipt_ProductDetails_WarehouseEntry_Receipt");
            });

            modelBuilder.Entity<ProductImportReceipt>(entity =>
            {
                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.ProductImportReceipts)
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductImport_Receipt_Internal_User");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.ProductImportReceipts)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductImport_Receipt_Site_Information");
            });

            modelBuilder.Entity<ProductIngredientDescription>(entity =>
            {
                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.ProductIngredientDescriptions)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Ingredient_Description_Product_Ingredient");

                entity.HasOne(d => d.ProductDescription)
                    .WithMany(p => p.ProductIngredientDescriptions)
                    .HasForeignKey(d => d.ProductDescriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Ingredient_Description_Product_Description");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.ProductIngredientDescriptions)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_Product_Ingredient_Description_Unit");
            });

            modelBuilder.Entity<ProductParent>(entity =>
            {
                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.ProductParents)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK_Drug_Manufacturer");

                entity.HasOne(d => d.ProductDescription)
                    .WithMany(p => p.ProductParents)
                    .HasForeignKey(d => d.ProductDescriptionId)
                    .HasConstraintName("FK_Drug_Drug_Description");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.ProductParents)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drug_Sub_Category");
            });

            modelBuilder.Entity<PurchaseVoucherHistory>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.PurchaseVoucherHistories)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseVoucherHistory_Customer");

                entity.HasOne(d => d.VoucherShop)
                    .WithMany(p => p.PurchaseVoucherHistories)
                    .HasForeignKey(d => d.VoucherShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseVoucherHistory_VoucherShop");
            });

            modelBuilder.Entity<SiteInformation>(entity =>
            {
                entity.HasOne(d => d.Address)
                    .WithMany(p => p.SiteInformations)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Site_Information_Dynamic_Address");
            });

            modelBuilder.Entity<SiteInventory>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SiteInventories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Site_Inventory_Product_Details");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.SiteInventories)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Site_Inventory_Site_Information");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasOne(d => d.MainCategory)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.MainCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sub_Category_Category_Main");
            });

            modelBuilder.Entity<VoucherCustomerRestriction>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.VoucherCustomerRestrictions)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VoucherCustomerRestriction_Customer");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.VoucherCustomerRestrictions)
                    .HasForeignKey(d => d.VoucherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VoucherCustomerRestriction_Voucher");
            });

            modelBuilder.Entity<VoucherDetail>(entity =>
            {
                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.VoucherDetails)
                    .HasForeignKey(d => d.VoucherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Voucher_Detail_Voucher");
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Warp_District");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
