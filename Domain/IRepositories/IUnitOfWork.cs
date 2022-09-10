#region Imports

using Common.ViewModels.UserAccount;
using Domain.Entities.CourseModule;
using Domain.Entities.General;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using System;

#endregion

namespace Domain.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();
        IGenericRepository<UserInfo> UserRepository { get; }
        IGenericRepository<AppSetting> AppSettingRepository { get; }

        IGenericRepository<Course> CourseRepository { get; }
        IGenericRepository<CoursesStudent> CoursesStudentRepository { get; }
        IGenericRepository<TraineesCourse> TraineesCourseRepository { get; }
    }
}