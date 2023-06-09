using System.ComponentModel;

namespace CoMan.Models.AuxiliaryModels
{
    public class CommentDatatable
    {
        public int Id { get; set; }

        public string? AddedDate { get; set; }

        public string? Comment { get; set; }

        public string? Author { get; set; }
    }
}
