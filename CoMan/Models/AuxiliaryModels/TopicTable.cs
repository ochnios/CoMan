using System.ComponentModel;

namespace CoMan.Models.AuxiliaryModels
{
    public class TopicTable
    {
        public int Id { get; set; }

        public DateTime AddedDate { get; set; }

        public TopicStatus Status { get; set; }

        public string Title { get; set; } = null!;

        public int StudentLimit { get; set; }

        public string AuthorId { get; set; } = null!;

        public string AuthorName { get; set; } = null!;
    }
}
