using System;

namespace Akuma.Model
{
    public class Task
    {
        public String Id { get; set; }
        public String ProjectId { get; set; }
        public String Desc { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public String TotalTime { get; set; }
        public String TimeSpan { get; set; }
    }
}