﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SuParty.Data;

#nullable disable

namespace SuParty.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250519131601_a48484")]
    partial class a48484
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ProductDataUserData", b =>
                {
                    b.Property<string>("ShoppingCartId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UsersId")
                        .HasColumnType("TEXT");

                    b.HasKey("ShoppingCartId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ProductDataUserData");
                });

            modelBuilder.Entity("SuParty.Data.DataModel.ProductData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Images")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Introduction")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SalesId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ProductDatas");
                });

            modelBuilder.Entity("SuParty.Data.DataModel.RealEstate.HouseData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("City")
                        .HasColumnType("INTEGER");

                    b.Property<float>("CommonArea")
                        .HasColumnType("REAL");

                    b.Property<string>("DevelopmentName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Floor")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HouseType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Images")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Index")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Introduction")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("LivingRoomCount")
                        .HasColumnType("INTEGER");

                    b.Property<float>("LotSize")
                        .HasColumnType("REAL");

                    b.Property<int>("MaintenanceFee")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("ParkingSpace")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParkingSpaceCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParkingSpaceType")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<float>("PricePerPing")
                        .HasColumnType("REAL");

                    b.Property<string>("ProductType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ProfitSharing")
                        .HasColumnType("TEXT");

                    b.Property<float>("RealSpace")
                        .HasColumnType("REAL");

                    b.Property<float>("Rent")
                        .HasColumnType("REAL");

                    b.Property<int>("RestroomCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SalesId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Seller")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("Space")
                        .HasColumnType("REAL");

                    b.Property<int>("TotalFloor")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Year")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("HouseDatas");
                });

            modelBuilder.Entity("SuParty.Data.DataModel.Tracking", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("TrackingList")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Trackings");
                });

            modelBuilder.Entity("SuParty.Data.DataModel.UserData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("Budget")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Bust")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChatRooms")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtraUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Height")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Hips")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HousePhone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IG_Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Income")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Introduction")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Line_Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("MembershipExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("MembershipLevel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Store")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Waist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Weight")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserData");

                    b.HasDiscriminator().HasValue("UserData");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("SuParty.Data.DataModel.UserWallet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("HouseCatTokens")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Points")
                        .HasColumnType("TEXT");

                    b.Property<string>("Referrers")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Wallet")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserWallets");
                });

            modelBuilder.Entity("SuParty.Service.Referrer.ReferrerMember", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BonusLogs")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LeftId")
                        .HasColumnType("TEXT");

                    b.Property<int>("LeftPoints")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RightId")
                        .HasColumnType("TEXT");

                    b.Property<int>("RightPoints")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SponsorId")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalEarnings")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LeftId");

                    b.HasIndex("RightId");

                    b.HasIndex("SponsorId");

                    b.ToTable("ReferrerMembers");
                });

            modelBuilder.Entity("Utility.Service.Product.enums.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Utility.Service.Product.enums.OrderItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("Utility.Service.Product.enums.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<int>("Method")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OrderId1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PaidAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrderId1");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("SuParty.Data.DataModel.RealEstateUserData", b =>
                {
                    b.HasBaseType("SuParty.Data.DataModel.UserData");

                    b.Property<string>("Brokerage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TraceRealEstates")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("RealEstateUserData");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductDataUserData", b =>
                {
                    b.HasOne("SuParty.Data.DataModel.ProductData", null)
                        .WithMany()
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SuParty.Data.DataModel.UserData", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SuParty.Service.Referrer.ReferrerMember", b =>
                {
                    b.HasOne("SuParty.Service.Referrer.ReferrerMember", "Left")
                        .WithMany()
                        .HasForeignKey("LeftId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SuParty.Service.Referrer.ReferrerMember", "Right")
                        .WithMany()
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SuParty.Service.Referrer.ReferrerMember", "Sponsor")
                        .WithMany()
                        .HasForeignKey("SponsorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Left");

                    b.Navigation("Right");

                    b.Navigation("Sponsor");
                });

            modelBuilder.Entity("Utility.Service.Product.enums.Order", b =>
                {
                    b.HasOne("SuParty.Data.DataModel.UserData", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Utility.Service.Product.enums.OrderItem", b =>
                {
                    b.HasOne("Utility.Service.Product.enums.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("Utility.Service.Product.enums.Payment", b =>
                {
                    b.HasOne("Utility.Service.Product.enums.Order", "Order")
                        .WithMany("Payments")
                        .HasForeignKey("OrderId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Utility.Service.Product.enums.Order", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
