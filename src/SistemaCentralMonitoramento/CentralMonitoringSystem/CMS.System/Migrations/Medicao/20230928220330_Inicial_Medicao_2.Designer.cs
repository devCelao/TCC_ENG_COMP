﻿// <auto-generated />
using System;
using CMS.System.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CMS.System.Migrations.Medicao
{
    [DbContext(typeof(MedicaoContext))]
    [Migration("20230928220330_Inicial_Medicao_2")]
    partial class Inicial_Medicao_2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CMS.System.Data.Models.DispositivoMedicao", b =>
                {
                    b.Property<DateTime>("DT_PUBLICACAO")
                        .HasColumnType("datetime2");

                    b.Property<string>("ID_DEVICE")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<float>("VAL_HMD_AMB")
                        .HasColumnType("real");

                    b.Property<float>("VAL_HMD_SOL")
                        .HasColumnType("real");

                    b.Property<float>("VAL_TMP_AMB")
                        .HasColumnType("real");

                    b.ToTable("MEDICOES_TBL", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
