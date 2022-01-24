using System;

namespace MathTasks.Models {
    public class ErrorViewModel {
        public string RequestId { get; set; } = null!;

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
