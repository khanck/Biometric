﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TCC.Payment.Data.Context;

#nullable disable

namespace TCC.Payment.Migrations.SqlServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TCC.Payment.Data.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("accountNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("bankName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("business_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("iban")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("business_ID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.BiometricVerification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("biometricData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("biometricType")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("customer_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("verificationID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("verificationResponse")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("verificationStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("customer_ID");

                    b.ToTable("BiometricVerifications");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.Biometrics", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("abisReferenceID")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("biometricData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("biometricType")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("customer_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("customer_ID");

                    b.ToTable("Biometrics");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.Business", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("businessTypes")
                        .HasColumnType("int");

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool?>("isEmailVerified")
                        .HasColumnType("bit");

                    b.Property<bool?>("isMobileVerified")
                        .HasColumnType("bit");

                    b.Property<string>("mobile")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("password")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Business");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TerminalUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TerminalUserId"));

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool?>("isEmailVerified")
                        .HasColumnType("bit");

                    b.Property<bool?>("isMobileVerified")
                        .HasColumnType("bit");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("mobile")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("password")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("pin")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.PaymentCard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("cardNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("cardType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("customer_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("cvv")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("expiryMonth")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("expiryYear")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool?>("isPrimary")
                        .HasColumnType("bit");

                    b.Property<string>("nameOnCard")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("customer_ID");

                    b.ToTable("PaymentCards");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.Property<Guid>("account_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("amount")
                        .HasColumnType("float");

                    b.Property<string>("billNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("biometricVerification_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("paymentCard_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<string>("transactionNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("account_ID");

                    b.HasIndex("biometricVerification_ID");

                    b.HasIndex("paymentCard_ID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.Trigger", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("account_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("amount")
                        .HasColumnType("float");

                    b.Property<string>("billNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("createdDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("device_ID")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Triggers");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.Account", b =>
                {
                    b.HasOne("TCC.Payment.Data.Entities.Business", "business")
                        .WithMany()
                        .HasForeignKey("business_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("business");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.BiometricVerification", b =>
                {
                    b.HasOne("TCC.Payment.Data.Entities.Customer", "customer")
                        .WithMany()
                        .HasForeignKey("customer_ID");

                    b.Navigation("customer");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.Biometrics", b =>
                {
                    b.HasOne("TCC.Payment.Data.Entities.Customer", "customer")
                        .WithMany()
                        .HasForeignKey("customer_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.PaymentCard", b =>
                {
                    b.HasOne("TCC.Payment.Data.Entities.Customer", "customer")
                        .WithMany()
                        .HasForeignKey("customer_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");
                });

            modelBuilder.Entity("TCC.Payment.Data.Entities.Transaction", b =>
                {
                    b.HasOne("TCC.Payment.Data.Entities.Account", "account")
                        .WithMany()
                        .HasForeignKey("account_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TCC.Payment.Data.Entities.BiometricVerification", "biometricVerification")
                        .WithMany()
                        .HasForeignKey("biometricVerification_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TCC.Payment.Data.Entities.PaymentCard", "paymentCard")
                        .WithMany()
                        .HasForeignKey("paymentCard_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("account");

                    b.Navigation("biometricVerification");

                    b.Navigation("paymentCard");
                });
#pragma warning restore 612, 618
        }
    }
}
