using System;
using System.Collections.Generic;
using System.Text;

namespace MicroService.Shareds.Messages
{
    public class CourseNameChangedEvent
    {
        public string ProductId { get; set; }
        public string UpdateName { get; set; }
    }
}
