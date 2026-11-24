using System;

namespace Notatka_App
{
    public class Note
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}