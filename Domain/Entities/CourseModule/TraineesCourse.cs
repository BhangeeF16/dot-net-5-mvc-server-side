using Domain.Entities.IdentityModule;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CourseModule
{
    [Table("tbl_Trainees_Course")]
    public class TraineesCourse
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Course")]
        public int fk_CourseId { get; set; } = 0;
        [ForeignKey("Trainee")]
        public int fk_TraineeId { get; set; } = 0;

        public virtual Course Course { get; set; }
        public virtual UserInfo Trainee { get; set; }
    }
}
