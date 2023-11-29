﻿// <auto-generated />
using System;
using CMS.System.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CMS.System.Migrations
{
    [DbContext(typeof(DispositivoContext))]
    [Migration("20230928220027_Inicial_Dispositivo")]
    partial class Inicial_Dispositivo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CMS.System.Data.Models.Dispositivo", b =>
                {
                    b.Property<string>("ID_DEVICE")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("COD_USUARIO_ALTERACAO")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(100)")
                        .HasDefaultValue("SISTEMA");

                    b.Property<DateTime>("DT_ALTERACAO")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DT_CADASTRO")
                        .HasColumnType("datetime2");

                    b.Property<int>("FREQUENCY")
                        .HasColumnType("int");

                    b.Property<int>("ID_GROUP")
                        .HasColumnType("int");

                    b.Property<int>("IND_ATIVO")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("IND_ATIVO_HMD_AMB")
                        .HasColumnType("int");

                    b.Property<int>("IND_ATIVO_HMD_SOL")
                        .HasColumnType("int");

                    b.Property<int>("IND_ATIVO_TMP_AMB")
                        .HasColumnType("int");

                    b.HasKey("ID_DEVICE");

                    b.ToTable("DISPOSITIVOS_TBL", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
