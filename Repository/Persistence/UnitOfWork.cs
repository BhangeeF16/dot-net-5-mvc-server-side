#region Imports

using Domain.Entities.CourseModule;
using Domain.Entities.General;
using Domain.Entities.IdentityModule;
using Domain.IRepositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;

#endregion

namespace Repository.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        #region Private member variables...
        private readonly AppDbContext _context;
        private IGenericRepository<UserInfo> _userRepository;
        private IGenericRepository<AppSetting> _appSettingRepository;
        private IGenericRepository<IdentityRole> _userRoleRepository;
        private IGenericRepository<Course> _courseRepository;
        private IGenericRepository<CoursesStudent> _coursesStudentRepository;
        private IGenericRepository<TraineesCourse> _traineesCourseRepository;
        #endregion

        #region Public Repository Creation properties...
        public IGenericRepository<UserInfo> UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new GenericRepository<UserInfo>(_context);
                return _userRepository;
            }
        }
        public IGenericRepository<IdentityRole> UserRoleRepository
        {
            get
            {
                if (_userRoleRepository == null)
                    _userRoleRepository = new GenericRepository<IdentityRole>(_context);
                return _userRoleRepository;
            }
        }

        public IGenericRepository<Course> CourseRepository
        {
            get
            {
                if (_courseRepository == null)
                    _courseRepository = new GenericRepository<Course>(_context);
                return _courseRepository;
            }
        }
        public IGenericRepository<CoursesStudent> CoursesStudentRepository
        {
            get
            {
                if (_coursesStudentRepository == null)
                    _coursesStudentRepository = new GenericRepository<CoursesStudent>(_context);
                return _coursesStudentRepository;
            }
        }
        public IGenericRepository<TraineesCourse> TraineesCourseRepository
        {
            get
            {
                if (_traineesCourseRepository == null)
                    _traineesCourseRepository = new GenericRepository<TraineesCourse>(_context);
                return _traineesCourseRepository;
            }
        }
        public IGenericRepository<AppSetting> AppSettingRepository
        {
            get
            {
                if (_appSettingRepository == null)
                    _appSettingRepository = new GenericRepository<AppSetting>(_context);
                return _appSettingRepository;
            }
        }
        #endregion

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _context.Dispose();
        }
    }
}