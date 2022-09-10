using Domain.Entities.IdentityModule;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CourseModule
{
    [Table("tbl_Courses_Student")]
    public class CoursesStudent
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Course")]
        public int fk_CourseId { get; set; } = 0;
        [ForeignKey("Student")]
        public int fk_StudentId { get; set; } = 0;

        public virtual Course Course { get; set; }
        public virtual UserInfo Student { get; set; }
    }
}
