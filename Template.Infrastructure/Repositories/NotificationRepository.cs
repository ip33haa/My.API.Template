using Microsoft.EntityFrameworkCore;
using Template.Application.Interfaces;
using Template.Domain.Entities;
using Template.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing notifications in the database.
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        private readonly TemplateDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
        /// </summary>
        /// <param name="context">Database context for accessing notifications.</param>
        public NotificationRepository(TemplateDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new notification to the database.
        /// </summary>
        /// <param name="notification">The notification entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a notification by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the notification.</param>
        /// <returns>The notification if found, otherwise null.</returns>
        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        /// <summary>
        /// Retrieves all notifications from the database.
        /// </summary>
        /// <returns>A list of all notifications.</returns>
        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }
    }
}
