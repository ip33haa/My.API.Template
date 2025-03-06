using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Template.Application.Interfaces;

namespace Template.Infrastructure.Repositories
{
    /// <summary>
    /// Generic repository providing CRUD operations using Dapper.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbConnection _dbConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const int PH_TIME_OFFSET_HOURS = 8; // UTC offset for PH time.

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="httpContextAccessor">Accessor for HTTP context.</param>
        public Repository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("TemplateDatabase"));
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves all records that are not deleted.
        /// </summary>
        /// <returns>A read-only list of all active records.</returns>
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            string tableName = GetTableName();
            var query = $"SELECT * FROM {tableName} WHERE IsDeleted = 0";
            return (await _dbConnection.QueryAsync<T>(query)).AsList();
        }

        /// <summary>
        /// Retrieves a record by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public async Task<T?> GetByIdAsync(Guid id)
        {
            string tableName = GetTableName();
            var query = $"SELECT * FROM {tableName} WHERE Id = @Id AND IsDeleted = 0";

            return await _dbConnection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
        }

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public async Task<T> AddAsync(T entity)
        {
            SetAuditFields(entity, "DateCreated", "CreatedBy");

            var properties = GetProperties(entity);
            string tableName = GetTableName();

            var query = $"INSERT INTO {tableName} ({string.Join(",", properties.Keys)}) " +
                        $"VALUES ({string.Join(",", properties.Keys.Select(p => "@" + p))})";

            await _dbConnection.ExecuteAsync(query, properties);
            return entity;
        }

        /// <summary>
        /// Marks an entity as deleted in the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public async Task DeleteAsync(T entity)
        {
            SetAuditFields(entity, "DateDeleted", "DeletedBy", isDeleted: true);

            var properties = GetProperties(entity);
            string tableName = GetTableName();

            var query = $"UPDATE {tableName} SET {string.Join(",", properties.Keys.Select(p => p + " = @" + p))} " +
                        $"WHERE Id = @Id";

            await _dbConnection.ExecuteAsync(query, properties);
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public async Task UpdateAsync(T entity)
        {
            SetAuditFields(entity, "DateUpdated", "UpdatedBy");

            var properties = GetProperties(entity);
            string tableName = GetTableName();

            var query = $"UPDATE {tableName} SET {string.Join(",", properties.Keys.Select(p => p + " = @" + p))} " +
                        $"WHERE Id = @Id";

            await _dbConnection.ExecuteAsync(query, properties);
        }

        /// <summary>
        /// Sets auditing fields (CreatedBy, UpdatedBy, DeletedBy, DateCreated, etc.).
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="dateProperty">The date field to set.</param>
        /// <param name="userProperty">The user field to set.</param>
        /// <param name="isDeleted">Optional: whether to mark as deleted.</param>
        private void SetAuditFields(T entity, string dateProperty, string userProperty, bool isDeleted = false)
        {
            SetProperty(entity, dateProperty, GetCurrentDateTime());
            SetProperty(entity, userProperty, GetCurrentUser());
            if (isDeleted)
            {
                SetProperty(entity, "IsDeleted", true);
            }
        }

        /// <summary>
        /// Retrieves a property value from an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The property value.</returns>
        private object? GetProperty(T entity, string propertyName)
        {
            return entity.GetType().GetProperty(propertyName)?.GetValue(entity);
        }

        /// <summary>
        /// Retrieves all properties of an entity that are not null.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A dictionary of property names and values.</returns>
        private Dictionary<string, object> GetProperties(T entity)
        {
            return entity.GetType().GetProperties()
                .Where(p => p.CanRead && p.GetValue(entity) != null)
                .ToDictionary(p => p.Name, p => p.GetValue(entity)!);
        }

        /// <summary>
        /// Sets a property value on an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The value to set.</param>
        private void SetProperty(T entity, string propertyName, object? value)
        {
            var property = entity.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(entity, value);
            }
        }

        /// <summary>
        /// Gets the current user's GUID from the HTTP context.
        /// </summary>
        /// <returns>The user ID if available; otherwise, null.</returns>
        private Guid? GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userIdString = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userIdString, out var userId) ? userId : null;
        }

        /// <summary>
        /// Gets the current date and time adjusted to PH time.
        /// </summary>
        /// <returns>The current PH time.</returns>
        private DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow.AddHours(PH_TIME_OFFSET_HOURS);
        }

        /// <summary>
        /// Gets the database table name for the entity type.
        /// </summary>
        /// <returns>The table name.</returns>
        private string GetTableName()
        {
            return typeof(T).Name + "s"; // Ensure this matches DB table naming conventions
        }
    }
}
