﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RBAT.Core;
using System;

namespace RBAT.Core.Migrations.RBAT
{
    [DbContext(typeof(RBATContext))]
    [Migration("20180628050501_ProjectNode")]
    partial class ProjectNode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RBAT.Core.Models.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.HasKey("Id");

                    b.ToTable("Node");
                });

            modelBuilder.Entity("RBAT.Core.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("RBAT.Core.Models.ProjectNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NodeId");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectNode");
                });

            modelBuilder.Entity("RBAT.Core.Models.TimeNaturalFlow", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NodeID");

                    b.Property<int>("ProjectID");

                    b.Property<int>("TimeComponentType");

                    b.Property<int>("TimeComponentValue");

                    b.Property<double>("Value")
                        .HasColumnName("NaturalFlow");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("NodeID");

                    b.HasIndex("ProjectID");

                    b.ToTable("TimeNaturalFlow");
                });

            modelBuilder.Entity("RBAT.Core.Models.ProjectNode", b =>
                {
                    b.HasOne("RBAT.Core.Models.Node", "Node")
                        .WithMany("ProjectNodes")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RBAT.Core.Models.Project", "Project")
                        .WithMany("ProjectNodes")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBAT.Core.Models.TimeNaturalFlow", b =>
                {
                    b.HasOne("RBAT.Core.Models.Node", "Node")
                        .WithMany("TimeNaturalFlows")
                        .HasForeignKey("NodeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RBAT.Core.Models.Project", "Project")
                        .WithMany("TimeNaturalFlows")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
