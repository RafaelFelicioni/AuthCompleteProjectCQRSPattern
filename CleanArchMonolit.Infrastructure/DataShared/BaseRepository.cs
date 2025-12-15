using CleanArchMonolit.Application.Auth.Interfaces.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanArchMonolit.Infrastructure.DataShared
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly TContext _context;
        public TEntity OldEntity { get; set; } = null;

        protected BaseRepository(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
       
        public virtual TContext GetDbContext() => _context;

        public virtual DbSet<TEntity> GetDbSet() => _dbSet;

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await GetDbSet().ToListAsync();
        }

        public virtual IQueryable<TEntity> GetDbSetQuery() => _dbSet?.AsNoTracking();

        public virtual async Task<IQueryable<TEntity>> Where(Expression<Func<TEntity, bool>> expression)
        {
            return GetDbSet().Where(expression);
        }

        public virtual async Task<TEntity> FindAsync(int key)
        {
            return await GetDbSet().FindAsync(key);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await GetDbSet().FirstOrDefaultAsync(expression);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await GetDbSet().AddAsync(entity);
        }

        public virtual async Task AddListAsync(IEnumerable<TEntity> entities)
        {
            await GetDbSet().AddRangeAsync(entities);
        }

        public virtual Task UpdateListAsync(IEnumerable<TEntity> entities)
        {
            GetDbSet().UpdateRange(entities);

            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            GetDbSet().Update(entity);

            return Task.CompletedTask;
        }

        public virtual Task RemoveAsync(TEntity entity)
        {
            GetDbSet().Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            GetDbSet().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public virtual async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        #region CouldBeUsedSomeday
        //public async Task SaveChangesWithAuditAsync()
        //{
        //    var changes = _context.ChangeTracker.Entries()
        //        .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified)
        //                    && Attribute.GetCustomAttribute(e.Entity.GetType(), typeof(AuditLogTableAttribute)) != null);

        //    foreach (var change in changes)
        //    {
        //        var auditLogTableAttribute =
        //            (AuditLogTableAttribute)Attribute.GetCustomAttribute(change.Entity.GetType(),
        //                typeof(AuditLogTableAttribute));

        //        if (auditLogTableAttribute != null)
        //        {
        //            var auditLogType = auditLogTableAttribute.AuditLogTableName;
        //            var auditableDeltaType = auditLogTableAttribute.AuditableDeltaTableName;

        //            var auditLogInstance = Activator.CreateInstance(auditLogTableAttribute.AuditLogTableName);

        //            auditLogType.GetProperty("Action").SetValue(auditLogInstance, change.State == EntityState.Added
        //                ? AuditActionEnum.ActionCreate : AuditActionEnum.ActionUpdate);
        //            auditLogType.GetProperty("ObjectName").SetValue(auditLogInstance, change.Entity.GetType().Name);
        //            auditLogType.GetProperty("CarrierId").SetValue(auditLogInstance, _httpContext?.User?.GetCarrierId());
        //            auditLogType.GetProperty("RegisteredDate").SetValue(auditLogInstance, DateTime.Now);
        //            auditLogType.GetProperty("ModifierId").SetValue(auditLogInstance, _httpContext?.User?.GetUserId());
        //            auditLogType.GetProperty("ModifierName").SetValue(auditLogInstance, _httpContext?.User?.GetUserName());

        //            var hasModifiedProperties = false;
        //            var hasObjectDescription = false;
        //            var properties = change.Entity.GetType().GetProperties();
        //            foreach (var property in properties)
        //            {
        //                var auditLogObjectDescription =
        //                    (AuditLogObjectDescription)Attribute.GetCustomAttribute(property,
        //                        typeof(AuditLogObjectDescription));

        //                if (auditLogObjectDescription != null)
        //                {
        //                    var propertyValue = change.OriginalValues[property.Name]?.ToString();
        //                    var currentValue = auditLogType.GetProperty("ObjectDescription").GetValue(auditLogInstance);
        //                    if (currentValue != null)
        //                    {
        //                        propertyValue = currentValue + " - " + propertyValue;
        //                    }
        //                    auditLogType.GetProperty("ObjectDescription").SetValue(auditLogInstance, propertyValue);
        //                    hasObjectDescription = true;
        //                }

        //                var auditLogPropertyAttribute =
        //                    (AuditLogPropertyAttribute)Attribute.GetCustomAttribute(property,
        //                        typeof(AuditLogPropertyAttribute));

        //                var shouldAddOnCreate =
        //                    (ShouldAddOnCreate)Attribute.GetCustomAttribute(property,
        //                        typeof(ShouldAddOnCreate)) != null && change.State == EntityState.Added;

        //                if (auditLogPropertyAttribute != null || property.Name == "Active")
        //                {
        //                    var oldValue = change.OriginalValues[property.Name]?.ToString();
        //                    var newValue = change.CurrentValues[property.Name]?.ToString();

        //                    if (oldValue != newValue || shouldAddOnCreate)
        //                    {
        //                        var auditableDeltaInstance = Activator.CreateInstance(auditLogTableAttribute.AuditableDeltaTableName);

        //                        auditableDeltaType.GetProperty("FieldName").SetValue(auditableDeltaInstance, property.Name);
        //                        auditableDeltaType.GetProperty("CarrierId").SetValue(auditableDeltaInstance, _httpContext?.User?.GetCarrierId());
        //                        auditableDeltaType.GetProperty("OldValue").SetValue(auditableDeltaInstance, shouldAddOnCreate ? null : oldValue);
        //                        auditableDeltaType.GetProperty("NewValue").SetValue(auditableDeltaInstance, newValue);

        //                        var auditLogDelta = auditLogType.GetProperties()
        //                            .FirstOrDefault(p => typeof(IEnumerable).IsAssignableFrom(p.PropertyType)
        //                                                 && p.PropertyType.GetGenericArguments().Any(t => t == auditableDeltaType));


        //                        var castMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(auditableDeltaType);
        //                        var modifiedProperties = (IList)castMethod.Invoke(null, new object[] { auditLogDelta.GetValue(auditLogInstance) });

        //                        modifiedProperties.Add(auditableDeltaInstance);
        //                        hasModifiedProperties = true;
        //                    }
        //                }
        //            }

        //            if (!hasObjectDescription)
        //            {
        //                auditLogType.GetProperty("ObjectDescription").SetValue(auditLogInstance, change.Entity.GetType().Name);
        //            }

        //            var auditLogEnumerableProperty = change.Entity.GetType().GetProperties()
        //                .FirstOrDefault(p => typeof(IEnumerable).IsAssignableFrom(p.PropertyType)
        //                                     && p.PropertyType.GetGenericArguments().Any(t => t == auditLogType));

        //            if (auditLogEnumerableProperty != null && hasModifiedProperties)
        //            {
        //                var auditLogList = (IList)auditLogEnumerableProperty.GetValue(change.Entity);
        //                auditLogList.Add(auditLogInstance);
        //            }
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //}

        //public virtual async Task AddWithLogAsync(TEntity entity)
        //{
        //    string entityName = (entity.GetType().Name).Replace("Proxy", "");
        //    if (Enum.TryParse(entityName, out ChangeLogTableEnum changeLogTable))
        //    {
        //        var changeLog = new TripDischargeAuditlog(_httpContext?.User?.GetTaxId(), changeLogTable, TypeOfChangeLogTripDischargeEnum.Create, DateTime.UtcNow, -1);

        //        Type type = typeof(TEntity);
        //        PropertyInfo[] propertys = type.GetProperties();
        //        foreach (PropertyInfo property in propertys)
        //        {
        //            object propertyValue = property.GetValue(entity);
        //            changeLog.TripDischargeAuditlogProperty.Add(new TripDischargeAuditlogProperty(property.Name, null, propertyValue == null ? "null" : propertyValue.ToString()));
        //        }

        //        await _context.Set<TripDischargeAuditlog>().AddAsync(changeLog);
        //        await AddAsync(entity);

        //    }
        //    else
        //    {
        //        throw new Exception($"entity {entityName} is not registered in enum ChangeLogTable");
        //    }
        //}

        //public virtual async Task UpdateWithLog(TEntity entity, int id)
        //{
        //    try
        //    {
        //        if (OldEntity == null)
        //            throw new InvalidOperationException("Use the ShallowCopy method to save the entity's previous state in the [OldEntity] property.");

        //        string entityName = entity.GetType().Name.Replace("Proxy", "");

        //        if (Enum.TryParse(entityName, out ChangeLogTableEnum changeLogTable))
        //        {
        //            var changeLog = new TripDischargeAuditlog(
        //                _httpContext?.User?.GetTaxId(),
        //                changeLogTable,
        //                TypeOfChangeLogTripDischargeEnum.Update,
        //                DateTime.UtcNow,
        //                id
        //            );

        //            Type type = typeof(TEntity);
        //            PropertyInfo[] properties = type.GetProperties();

        //            foreach (PropertyInfo property in properties)
        //            {
        //                try
        //                {
        //                    object oldValue = property.GetValue(OldEntity);
        //                    object newValue = property.GetValue(entity);

        //                    string oldStringValue = oldValue?.ToString() ?? "null";
        //                    string newStringValue = newValue?.ToString() ?? "null";

        //                    if (oldStringValue != newStringValue)
        //                    {
        //                        changeLog.TripDischargeAuditlogProperty.Add(
        //                            new TripDischargeAuditlogProperty(property.Name, oldStringValue, newStringValue)
        //                        );
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw;
        //                }
        //            }

        //            await _context.Set<TripDischargeAuditlog>().AddAsync(changeLog);
        //            await UpdateAsync(entity);
        //        }
        //        else
        //        {
        //            throw new ArgumentException($"Entity '{entityName}' is not registered in enum ChangeLogTableEnum.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw; // Rethrow the exception after logging
        //    }
        //}

        //public virtual async Task RemoveWithLog(TEntity entity, int id)
        //{

        //    string entityName = (entity.GetType().Name).Replace("Proxy", "");
        //    if (Enum.TryParse(entityName, out ChangeLogTableEnum changeLogTable))
        //    {
        //        var changeLog = new TripDischargeAuditlog(_httpContext?.User?.GetTaxId(), changeLogTable, TypeOfChangeLogTripDischargeEnum.Delete, DateTime.UtcNow, id);
        //        Type type = typeof(TEntity);
        //        PropertyInfo[] propertys = type.GetProperties();

        //        changeLog.TripDischargeAuditlogProperty.Add(new TripDischargeAuditlogProperty("Delete", null, entityName.ToString()));

        //        await _context.Set<TripDischargeAuditlog>().AddAsync(changeLog);

        //        await RemoveAsync(entity);
        //    }
        //    else
        //    {
        //        throw new Exception($"entity {entityName} is not registered in enum ChangeLogTable");
        //    }
        //}
        #endregion
    }
}
