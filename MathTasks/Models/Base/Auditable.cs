using System;

namespace MathTasks.Models.Base
{
    public abstract class Auditable
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdateddBy { get; set; }
    }
}
