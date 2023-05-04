using System.ComponentModel;

namespace CoMan.Models.AuxiliaryModels
{
    public class TopicTable
    {
        public int Id { get; set; }

        public string AddedDate { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string Title { get; set; } = null!;

        public int StudentLimit { get; set; }

        public string AuthorId { get; set; } = null!;

        public string AuthorName { get; set; } = null!;
    }
}
