using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RBAT.Core.Clasess;
using RBAT.Core.Models;
using Remotion.Linq.Parsing.ExpressionVisitors;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RBAT.Core
{

    public class RBATContext : DbContext
    {
        private readonly string _username;
        private readonly bool _isAdmin;

        public RBATContext(IHttpContextAccessor contextAccessor)
        {
            this._username = contextAccessor?.HttpContext?.User?.Identity.Name;
            this._isAdmin = contextAccessor?.HttpContext?.User?.IsInRole("Admin") ?? false;
        }

        public RBATContext()
        {

        }

        public RBATContext(IHttpContextAccessor contextAccessor, string username, bool isAdmin)
        {
            this.Database.SetCommandTimeout(3600);//set Timeout to 60 min
            this._username = username;
            this._isAdmin = isAdmin;
        }

        public DbSet<CustomTimeStep> CustomTimeSteps { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeType> NodeTypes { get; set; }
        public DbSet<ProjectNode> ProjectNodes { get; set; }
        public DbSet<TimeNaturalFlow> TimeNaturalFlows { get; set; }
        public DbSet<TimeWaterUse> TimeWaterUses { get; set; }
        public DbSet<TimeHistoricLevel> TimeHistoricLevels { get; set; }
        public DbSet<ClimateStation> ClimateStations { get; set; }
        public DbSet<TimeClimateData> TimeClimateDatas { get; set; }
        public DbSet<NetEvaporation> NetEvaporations { get; set; }
        public DbSet<TimeStorageCapacity> TimeStorageCapacities { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<ChannelType> ChannelTypes { get; set; }
        public DbSet<ChannelTravelTime> ChannelTravelTimes { get; set; }
        public DbSet<ChannelOutflowCapacity> ChannelOutflowCapacities { get; set; }
        public DbSet<RecordedFlowStation> RecordedFlowStations { get; set; }
        public DbSet<TimeRecordedFlow> TimeRecordedFlows { get; set; }
        public DbSet<TimeStepType> TimeStepTypes { get; set; }
        public DbSet<RoutingOptionType> RoutingOptionTypes { get; set; }
        public DbSet<ProjectChannel> ProjectChannels { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<ChannelPolicyGroup> ChannelPolicyGroups { get; set; }
        public DbSet<ChannelPolicyGroupChannel> ChannelPolicyGroupChannels { get; set; }
        public DbSet<ChannelZoneLevel> ChannelZoneLevels { get; set; }
        public DbSet<ChannelZoneWeight> ChannelZoneWeights { get; set; }
        public DbSet<ScenarioType> ScenarioTypes { get; set; }
        public DbSet<NodePolicyGroup> NodePolicyGroups { get; set; }
        public DbSet<NodePolicyGroupNode> NodePolicyGroupNodes { get; set; }
        public DbSet<NodeZoneLevel> NodeZoneLevels { get; set; }
        public DbSet<NodeZoneWeight> NodeZoneWeights { get; set; }
        public DbSet<StartingReservoirLevel> StartingReservoirLevels { get; set; }
        public DbSet<MeasuringUnit> MeasuringUnits { get; set; }
        public DbSet<ChannelOptimalSolutions> ChannelOptimalSolutions { get; set; }
        public DbSet<ChannelOptimalSolutionsData> ChannelOptimalSolutionsData { get; set; }
        public DbSet<NodeOptimalSolutions> NodeOptimalSolutions { get; set; }
        public DbSet<NodeOptimalSolutionsData> NodeOptimalSolutionsData { get; set; }
        public DbSet<ChannelZoneLevelRecordedFlowStation> ChannelZoneLevelRecordedFlowStations { get; set; }
        public DbSet<NodeZoneLevelHistoricReservoirLevel> NodeZoneLevelHistoricReservoirLevels { get; set; }
        public static string ConnectionString { get; set; }

        private void Audit()
        {
            var entries = ChangeTracker.Entries().Where(x =>
               x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added) ((BaseEntity)entry.Entity).Created = DateTime.UtcNow;
                ((BaseEntity)entry.Entity).Modified = DateTime.UtcNow;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }            

            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpression<BaseEntity>(e => _isAdmin || e.UserName == _username, entityType.ClrType));
            }

            modelBuilder.Entity<Channel>()
            .HasOne(e => e.UpstreamChannelWithControlStructure)
            .WithMany();

            modelBuilder.Entity<Channel>()
            .HasOne(e => e.UpstreamChannelHeadWaterElevation)
            .WithMany();

            modelBuilder.Entity<Channel>()
            .HasOne(e => e.DownstreamChannelTailWaterElevation)
            .WithMany();
        }

        public override int SaveChanges()
        {
            Audit();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async Task<int> SaveChangesAsync()
        {
            Audit();
            return await base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity entity)
                {                                       
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entity.Modified = now;
                            break;

                        case EntityState.Added:
                            entity.Created = now;
                            entity.Modified = now;
                            if (entity.UserName == null) entity.UserName = this._username;
                            break;
                    }
                }
            }
        }

        private static LambdaExpression ConvertFilterExpression<TInterface>(Expression<Func<TInterface, bool>> filterExpression, Type entityType)
        {
            var newParam = Expression.Parameter(entityType);
            var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);

            return Expression.Lambda(newBody, newParam);
        }
    }

}
