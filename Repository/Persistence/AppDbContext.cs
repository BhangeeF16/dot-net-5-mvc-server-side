#region Imports

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Common.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.IServices;
using Domain.Entities.Common;
using Common.ViewModels.General;
using Domain.Entities.General;
using Domain.Entities.IdentityModule;
using Domain.Entities.CourseModule;


#endregion

namespace Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<UserInfo, Role, string>
    {
        private readonly ICurrentUserService _currentUserService;
        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService) :
            base(options)
        {
            _currentUserService = currentUserService;
        }

        #region Modules

        #region General

        public DbSet<AppSetting> AppSettings { get; set; }

        #endregion

        #region Courses Module

        public DbSet<Course> Courses { get; set; }
        public DbSet<CoursesStudent> CoursesStudents { get; set; }
        public DbSet<TraineesCourse> TraineesCourses { get; set; }

        #endregion
        
        #endregion
     
        #region Overrides

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseForUser>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = getZeroOrCurrentUserID();
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = getZeroOrCurrentUserID();
                        entry.Entity.ModifiedDate = DateTime.Now;
                        break;
                }
            
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = getZeroOrCurrentUserID();
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = getZeroOrCurrentUserID();
                        entry.Entity.ModifiedDate = DateTime.Now;
                        break;
                }

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = getZeroOrCurrentUserID();
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = getZeroOrCurrentUserID();
                        entry.Entity.ModifiedDate = DateTime.Now;
                        break;
                }

            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = getZeroOrCurrentUserID();
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = getZeroOrCurrentUserID();
                        entry.Entity.ModifiedDate = DateTime.Now;
                        break;
                }
            
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = getZeroOrCurrentUserID();
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = getZeroOrCurrentUserID();
                        entry.Entity.ModifiedDate = DateTime.Now;
                        break;
                }

            return base.SaveChanges();
        }
        private string getZeroOrCurrentUserID()
        {
            var id = "0";
            try
            {
                if (_currentUserService?.UserId != "0") {
                    id = _currentUserService?.UserId;
                 }
            }
            catch (Exception)
            {
            }

            return id;
        }

        #endregion
    }
}