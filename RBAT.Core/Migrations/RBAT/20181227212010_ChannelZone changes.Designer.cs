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
    [Migration("20181227212010_ChannelZone changes")]
    partial class ChannelZonechanges
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

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<int?>("DownstreamNodeID");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int?>("NumberOfRoutingPhases");

                    b.Property<double?>("PercentReturnFlow");

                    b.Property<double?>("RoutingCoefficientA");

                    b.Property<double?>("RoutingCoefficientN");

                    b.Property<bool>("RoutingOptionUse");

                    b.Property<int?>("UpstreamNodeID");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ChannelID");

                    b.ToTable("ChannelOutflowCapacity");
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelPolicyGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ChannelZoneID");

                    b.Property<DateTime>("Created");

                    b.Property<double>("IdealZone");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<double>("ZoneAboveIdeal1");

                    b.Property<double>("ZoneAboveIdeal2");

                    b.Property<double>("ZoneAboveIdeal3");

                    b.Property<double>("ZoneAboveIdeal4");

                    b.Property<double>("ZoneAboveIdeal5");

                    b.Property<double>("ZoneAboveIdeal6");

                    b.Property<double>("ZoneBelowIdeal1");

                    b.Property<double>("ZoneBelowIdeal10");

                    b.Property<double>("ZoneBelowIdeal2");

                    b.Property<double>("ZoneBelowIdeal3");

                    b.Property<double>("ZoneBelowIdeal4");

                    b.Property<double>("ZoneBelowIdeal5");

                    b.Property<double>("ZoneBelowIdeal6");

                    b.Property<double>("ZoneBelowIdeal7");

                    b.Property<double>("ZoneBelowIdeal8");

                    b.Property<double>("ZoneBelowIdeal9");

                    b.HasKey("Id");

                    b.HasIndex("ChannelZoneID");

                    b.ToTable("ChannelPolicyGroup");
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelPolicyGroupChannel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelID");

                    b.Property<long>("ChannelPolicyGroupID");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("Priority");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ChannelID");

                    b.HasIndex("ChannelPolicyGroupID");

                    b.ToTable("ChannelPolicyGroupChannel");
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelRecordedFlowStation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelID");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("RecordedFlowStationID");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("ChannelType");
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelZone", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NumberOfZonesAboveIdealLevel");

                    b.Property<int>("NumberOfZonesBelowIdealLevel");

                    b.Property<long>("ScenarioID");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ScenarioID");

                    b.ToTable("ChannelZone");
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelZoneLevel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelID");

                    b.Property<long>("ChannelZoneID");

                    b.Property<DateTime>("Created");

                    b.Property<double>("IdealZone");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("TimeComponentType");

                    b.Property<int>("TimeComponentValue");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int>("Year");

                    b.Property<double>("ZoneAboveIdeal1");

                    b.Property<double>("ZoneAboveIdeal2");

                    b.Property<double>("ZoneAboveIdeal3");

                    b.Property<double>("ZoneAboveIdeal4");

                    b.Property<double>("ZoneAboveIdeal5");

                    b.Property<double>("ZoneAboveIdeal6");

                    b.Property<double>("ZoneBelowIdeal1");

                    b.Property<double>("ZoneBelowIdeal10");

                    b.Property<double>("ZoneBelowIdeal2");

                    b.Property<double>("ZoneBelowIdeal3");

                    b.Property<double>("ZoneBelowIdeal4");

                    b.Property<double>("ZoneBelowIdeal5");

                    b.Property<double>("ZoneBelowIdeal6");

                    b.Property<double>("ZoneBelowIdeal7");

                    b.Property<double>("ZoneBelowIdeal8");

                    b.Property<double>("ZoneBelowIdeal9");

                    b.HasKey("Id");

                    b.HasIndex("ChannelID");

                    b.HasIndex("ChannelZoneID");

                    b.ToTable("ChannelZoneLevel");
                });

            modelBuilder.Entity("RBAT.Core.Models.ClimateStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<bool>("EpType");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("NodeTypeId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NodeType");
                });

            modelBuilder.Entity("RBAT.Core.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CalculationBegins");

                    b.Property<DateTime>("CalculationEnds");

                    b.Property<DateTime>("Created");

                    b.Property<int?>("DataStepTypeID");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int?>("RoutingOptionTypeID");

                    b.Property<int?>("TimeStepTypeID");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("DataStepTypeID");

                    b.HasIndex("RoutingOptionTypeID");

                    b.HasIndex("TimeStepTypeID");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("RBAT.Core.Models.ProjectChannel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelId");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("ProjectId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectChannel");
                });

            modelBuilder.Entity("RBAT.Core.Models.ProjectNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("NodeId");

                    b.Property<int>("ProjectId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("RecordedFlowStation");
                });

            modelBuilder.Entity("RBAT.Core.Models.RoutingOptionType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("RoutingOptionType");
                });

            modelBuilder.Entity("RBAT.Core.Models.Scenario", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("ProjectID");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ProjectID");

                    b.ToTable("Scenario");
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

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<double>("Value")
                        .HasColumnName("RecordedFlow");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("RecordedFlowStationID");

                    b.ToTable("TimeRecordedFlow");
                });

            modelBuilder.Entity("RBAT.Core.Models.TimeStepType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("CalculationFlag");

                    b.Property<bool>("DataFlag");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("TimeStepType");
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

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

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

            modelBuilder.Entity("RBAT.Core.Models.ChannelPolicyGroup", b =>
                {
                    b.HasOne("RBAT.Core.Models.ChannelZone", "ChannelZone")
                        .WithMany()
                        .HasForeignKey("ChannelZoneID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelPolicyGroupChannel", b =>
                {
                    b.HasOne("RBAT.Core.Models.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RBAT.Core.Models.ChannelPolicyGroup", "ChannelPolicyGroup")
                        .WithMany()
                        .HasForeignKey("ChannelPolicyGroupID")
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

            modelBuilder.Entity("RBAT.Core.Models.ChannelZone", b =>
                {
                    b.HasOne("RBAT.Core.Models.Scenario", "Scenario")
                        .WithMany("ChannelZones")
                        .HasForeignKey("ScenarioID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBAT.Core.Models.ChannelZoneLevel", b =>
                {
                    b.HasOne("RBAT.Core.Models.Channel", "Channel")
                        .WithMany("ChannelZoneLevels")
                        .HasForeignKey("ChannelID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RBAT.Core.Models.ChannelZone", "ChannelZone")
                        .WithMany("ChannelZoneLevels")
                        .HasForeignKey("ChannelZoneID")
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

            modelBuilder.Entity("RBAT.Core.Models.Project", b =>
                {
                    b.HasOne("RBAT.Core.Models.TimeStepType", "DataStepType")
                        .WithMany()
                        .HasForeignKey("DataStepTypeID");

                    b.HasOne("RBAT.Core.Models.RoutingOptionType", "RoutingOptionType")
                        .WithMany()
                        .HasForeignKey("RoutingOptionTypeID");

                    b.HasOne("RBAT.Core.Models.TimeStepType", "TimeStepType")
                        .WithMany()
                        .HasForeignKey("TimeStepTypeID");
                });

            modelBuilder.Entity("RBAT.Core.Models.ProjectChannel", b =>
                {
                    b.HasOne("RBAT.Core.Models.Channel", "Channel")
                        .WithMany()
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RBAT.Core.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
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

            modelBuilder.Entity("RBAT.Core.Models.Scenario", b =>
                {
                    b.HasOne("RBAT.Core.Models.Project", "Project")
                        .WithMany("Scenarios")
                        .HasForeignKey("ProjectID")
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
