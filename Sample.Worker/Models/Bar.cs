using System;

namespace Sample.Worker.Models
{
    public class Bar
    {
        public BarNames Name  { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}