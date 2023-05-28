using CoMan.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoMan.Models
{
    public class CooperationRequestDatatable
    {
        public int Id { get; set; }

        public string CreationDate { get; set; }

        public string Status { get; set; }

        public string ConsiderationDate { get; set; }


        public string Student { get; set; }

        public string Teacher { get; set; }
    }
}
