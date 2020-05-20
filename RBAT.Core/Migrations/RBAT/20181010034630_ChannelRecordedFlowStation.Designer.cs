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
    [Migration("20181010034630_ChannelRecordedFlowStation")]
    partial class ChannelRecordedFlowStation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RBAT.Core.Models.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelTypeId");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<int?>("DownstreamNodeID");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name");

                    b.Property<int?>("NumberOfRoutingPhases");

                    b.Property<double?>("PercentReturnFlow");

                    b.Property<double?>("RoutingCoefficientA");

                    b.Property<double?>("RoutingCoefficientN");

                    b.Property<bool>("RoutingOptionUse");

                    b.Property<int?>("UpstreamNodeID");

                    b.HasKey("Id");

                    b.HasIndex("ChannelTypeId");

                    b.HasIndex("DownstreamNodeID");

                    b.HasIndex("UpstreamNodeID");

                    b.ToTable("Channel");
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelOutflowCapacity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelID");

                    b.Property<DateTime>("Created");

                    b.Property<double>("Elevation");

                    b.Property<double>("MaximumOutflow");

                    b.Property<DateTime>("Modified");

                    b.HasKey("Id");

                    b.HasIndex("ChannelID");

                    b.ToTable("ChannelOutflowCapacity");
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelRecordedFlowStation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelID");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("RecordedFlowStationID");

                    b.HasKey("Id");

                    b.HasIndex("ChannelID");

                    b.HasIndex("RecordedFlowStationID");

                    b.ToTable("ChannelRecordedFlowStation");
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelTravelTime", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelID");

                    b.Property<DateTime>("Created");

                    b.Property<double>("Flow");

                    b.Property<DateTime>("Modified");

                    b.Property<double>("TravelTime");

                    b.HasKey("Id");

                    b.HasIndex("ChannelID");

                    b.ToTable("ChannelTravelTime");
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("ChannelType");
                });

            modelBuilder.Entity("RBAT.Core.Models.ClimateStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<bool>("EpType");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("ClimateStation");
                });

            modelBuilder.Entity("RBAT.Core.Models.NetEvaporation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("AdjustmentFactor");

                    b.Property<int>("ClimateStationID");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NodeID");

                    b.HasKey("Id");

                    b.HasIndex("ClimateStationID");

                    b.HasIndex("NodeID");

                    b.ToTable("NetEvaporation");
                });

            modelBuilder.Entity("RBAT.Core.Models.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<int>("NodeTypeId");

                    b.HasKey("Id");

                    b.HasIndex("NodeTypeId");

                    b.ToTable("Node");
                });

            modelBuilder.Entity("RBAT.Core.Models.NodeType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NodeType");
                });

            modelBuilder.Entity("RBAT.Core.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

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

            modelBuilder.Entity("RBAT.Core.Models.RecordedFlowStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("RecordedFlowStation");
                });

            modelBuilder.Entity("RBAT.Core.Models.TimeClimateData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClimateStationID");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("TimeComponentType");

                    b.Property<int>("TimeComponentValue");

                    b.Property<double>("Value")
                        .HasColumnName("ClimateData");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("ClimateStationID");

                    b.ToTable("TimeClimateData");
                });

            modelBuilder.Entity("RBAT.Core.Models.TimeHistoricLevel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NodeID");

                    b.Property<int>("TimeComponentType");

                    b.Property<int>("TimeComponentValue");

                    b.Property<double>("Value")
                        .HasColumnName("Elevation");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("NodeID");

                    b.ToTable("TimeHistoricLevel");
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

            modelBuilder.Entity("RBAT.Core.Models.TimeRecordedFlow", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("RecordedFlowStationID");

                    b.Property<int>("TimeComponentType");

                    b.Property<int>("TimeComponentValue");

                    b.Property<double>("Value")
                        .HasColumnName("RecordedFlow");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("RecordedFlowStationID");

                    b.ToTable("TimeRecordedFlow");
                });

            modelBuilder.Entity("RBAT.Core.Models.TimeStorageCapacity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Area");

                    b.Property<DateTime>("Created");

                    b.Property<double>("Elevation");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NodeID");

                    b.Property<DateTime>("SurveyDate");

                    b.Property<double>("Volume");

                    b.HasKey("Id");

                    b.HasIndex("NodeID");

                    b.ToTable("TimeStorageCapacity");
                });

            modelBuilder.Entity("RBAT.Core.Models.TimeWaterUse", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NodeID");

                    b.Property<int>("TimeComponentType");

                    b.Property<int>("TimeComponentValue");

                    b.Property<double>("Value")
                        .HasColumnName("WaterUse");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("NodeID");

                    b.ToTable("TimeWaterUse");
                });

            modelBuilder.Entity("RBAT.Core.Models.Channel", b =>
                {
                    b.HasOne("RBAT.Core.Models.ChannelType", "ChannelType")
                        .WithMany()
                        .HasForeignKey("ChannelTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RBAT.Core.Models.Node", "DownstreamNode")
                        .WithMany()
                        .HasForeignKey("DownstreamNodeID");

                    b.HasOne("RBAT.Core.Models.Node", "UpstreamNode")
                        .WithMany()
                        .HasForeignKey("UpstreamNodeID");
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelOutflowCapacity", b =>
                {
                    b.HasOne("RBAT.Core.Models.Channel", "Channel")
                        .WithMany("ChannelOutflowCapacities")
                        .HasForeignKey("ChannelID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelRecordedFlowStation", b =>
                {
                    b.HasOne("RBAT.Core.Models.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RBAT.Core.Models.RecordedFlowStation", "recordedFlowStation")
                        .WithMany()
                        .HasForeignKey("RecordedFlowStationID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelTravelTime", b =>
                {
                    b.HasOne("RBAT.Core.Models.Channel", "Channel")
                        .WithMany("TravelTimes")
                        .HasForeignKey("ChannelID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBAT.Core.Models.NetEvaporation", b =>
                {
                    b.HasOne("RBAT.Core.Models.ClimateStation", "ClimateStation")
                        .WithMany("NetEvaporations")
                        .HasForeignKey("ClimateStationID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RBAT.Core.Models.Node", "Node")
                        .WithMany("NetEvaporations")
                        .HasForeignKey("NodeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBAT.Core.Models.Node", b =>
                {
                    b.HasOne("RBAT.Core.Models.NodeType", "NodeType")
                        .WithMany()
                        .HasForeignKey("NodeTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity("RBAT.Core.Models.TimeClimateData", b =>
                {
                    b.HasOne("RBAT.Core.Models.ClimateStation", "ClimateStation")
                        .WithMany()
                        .HasForeignKey("ClimateStationID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBAT.Core.Models.TimeHistoricLevel", b =>
                {
                    b.HasOne("RBAT.Core.Models.Node", "Node")
                        .WithMany("TimeHistoricLevels")
                        .HasForeignKey("NodeID")
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

            modelBuilder.Entity("RBAT.Core.Models.TimeRecordedFlow", b =>
                {
                    b.HasOne("RBAT.Core.Models.RecordedFlowStation", "RecordedFlowStation")
                        .WithMany("TimeRecordedFlows")
                        .HasForeignKey("RecordedFlowStationID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBAT.Core.Models.TimeStorageCapacity", b =>
                {
                    b.HasOne("RBAT.Core.Models.Node", "Node")
                        .WithMany("TimeStorageCapacities")
                        .HasForeignKey("NodeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBAT.Core.Models.TimeWaterUse", b =>
                {
                    b.HasOne("RBAT.Core.Models.Node", "Node")
                        .WithMany("TimeWaterUses")
                        .HasForeignKey("NodeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
