namespace OrganisationRegistry.SqlServer.Body
{
    using BodyClassification;
    using BodyClassificationType;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Data.Common;
    using System.Linq;
    using OrganisationRegistry.Body.Events;
    using OrganisationRegistry.BodyClassification.Events;
    using OrganisationRegistry.BodyClassificationType.Events;
    using OrganisationRegistry.Infrastructure.Events;

    public class BodyBodyClassificationListItem
    {
        public Guid BodyBodyClassificationId { get; set; }
        public Guid BodyId { get; set; }

        public Guid BodyClassificationTypeId { get; set; }
        public string BodyClassificationTypeName { get; set; }

        public Guid BodyClassificationId { get; set; }
        public string BodyClassificationName { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }

    public class BodyBodyClassificationListConfiguration : EntityMappingConfiguration<BodyBodyClassificationListItem>
    {
        public override void Map(EntityTypeBuilder<BodyBodyClassificationListItem> b)
        {
            b.ToTable(nameof(BodyBodyClassificationListView.ProjectionTables.BodyBodyClassificationList), "OrganisationRegistry")
                .HasKey(p => p.BodyBodyClassificationId)
                .ForSqlServerIsClustered(false);

            b.Property(p => p.BodyId).IsRequired();

            b.Property(p => p.BodyClassificationTypeId).IsRequired();
            b.Property(p => p.BodyClassificationTypeName).HasMaxLength(BodyClassificationTypeListConfiguration.NameLength).IsRequired();

            b.Property(p => p.BodyClassificationId).IsRequired();
            b.Property(p => p.BodyClassificationName).HasMaxLength(BodyClassificationListConfiguration.NameLength).IsRequired();

            b.Property(p => p.ValidFrom);
            b.Property(p => p.ValidTo);

            b.HasIndex(x => x.BodyClassificationTypeName).ForSqlServerIsClustered();
            b.HasIndex(x => x.BodyClassificationName);
            b.HasIndex(x => x.ValidFrom);
            b.HasIndex(x => x.ValidTo);
        }
    }

    public class BodyBodyClassificationListView :
        Projection<BodyBodyClassificationListView>,
        IEventHandler<BodyBodyClassificationAdded>,
        IEventHandler<BodyBodyClassificationUpdated>,
        IEventHandler<BodyClassificationTypeUpdated>,
        IEventHandler<BodyClassificationUpdated>
    {
        public override string[] ProjectionTableNames => Enum.GetNames(typeof(ProjectionTables));

        public enum ProjectionTables
        {
            BodyBodyClassificationList
        }

        private readonly IEventStore _eventStore;

        public BodyBodyClassificationListView(
            ILogger<BodyBodyClassificationListView> logger,
            IEventStore eventStore) : base(logger)
        {
            _eventStore = eventStore;
        }

        public void Handle(DbConnection dbConnection, DbTransaction dbTransaction, IEnvelope<BodyClassificationTypeUpdated> message)
        {
            using (var context = new OrganisationRegistryTransactionalContext(dbConnection, dbTransaction))
            {
                var bodyBodyClassifications =
                    context.BodyBodyClassificationList
                        .Where(x => x.BodyClassificationTypeId == message.Body.BodyClassificationTypeId);

                if (!bodyBodyClassifications.Any())
                    return;

                foreach (var bodyBodyClassification in bodyBodyClassifications)
                    bodyBodyClassification.BodyClassificationTypeName = message.Body.Name;

                context.SaveChanges();
            }
        }

        public void Handle(DbConnection dbConnection, DbTransaction dbTransaction, IEnvelope<BodyClassificationUpdated> message)
        {
            using (var context = new OrganisationRegistryTransactionalContext(dbConnection, dbTransaction))
            {
                var bodyBodyClassifications =
                    context.BodyBodyClassificationList.Where(x =>
                        x.BodyClassificationId == message.Body.BodyClassificationId);

                if (!bodyBodyClassifications.Any())
                    return;

                foreach (var bodyBodyClassification in bodyBodyClassifications)
                {
                    bodyBodyClassification.BodyClassificationTypeId = message.Body.BodyClassificationTypeId;
                    bodyBodyClassification.BodyClassificationTypeName = message.Body.BodyClassificationTypeName;
                    bodyBodyClassification.BodyClassificationName = message.Body.Name;
                }

                context.SaveChanges();
            }
        }

        public void Handle(DbConnection dbConnection, DbTransaction dbTransaction, IEnvelope<BodyBodyClassificationAdded> message)
        {
            var bodyBodyClassificationListItem = new BodyBodyClassificationListItem
            {
                BodyBodyClassificationId = message.Body.BodyBodyClassificationId,
                BodyId = message.Body.BodyId,
                BodyClassificationTypeId = message.Body.BodyClassificationTypeId,
                BodyClassificationId = message.Body.BodyClassificationId,
                BodyClassificationTypeName = message.Body.BodyClassificationTypeName,
                BodyClassificationName = message.Body.BodyClassificationName,
                ValidFrom = message.Body.ValidFrom,
                ValidTo = message.Body.ValidTo
            };

            using (var context = new OrganisationRegistryTransactionalContext(dbConnection, dbTransaction))
            {
                context.BodyBodyClassificationList.Add(bodyBodyClassificationListItem);
                context.SaveChanges();
            }
        }

        public void Handle(DbConnection dbConnection, DbTransaction dbTransaction, IEnvelope<BodyBodyClassificationUpdated> message)
        {
            using (var context = new OrganisationRegistryTransactionalContext(dbConnection, dbTransaction))
            {
                var key = context.BodyBodyClassificationList.SingleOrDefault(item => item.BodyBodyClassificationId == message.Body.BodyBodyClassificationId);

                key.BodyBodyClassificationId = message.Body.BodyBodyClassificationId;
                key.BodyId = message.Body.BodyId;
                key.BodyClassificationTypeId = message.Body.BodyClassificationTypeId;
                key.BodyClassificationId = message.Body.BodyClassificationId;
                key.BodyClassificationTypeName = message.Body.BodyClassificationTypeName;
                key.BodyClassificationName = message.Body.BodyClassificationName;
                key.ValidFrom = message.Body.ValidFrom;
                key.ValidTo = message.Body.ValidTo;

                context.SaveChanges();
            }
        }

        public override void Handle(DbConnection dbConnection, DbTransaction dbTransaction, IEnvelope<RebuildProjection> message)
        {
            RebuildProjection(_eventStore, dbConnection, dbTransaction, message);
        }
    }
}
