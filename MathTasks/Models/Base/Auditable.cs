using System;

namespace MathTasks.Models.Base
{
    public abstract class Auditable : IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }


}
