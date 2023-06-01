using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Account
    {
        [Key]
        public long Id { get; init; }

        public string Name { get; init; }

        public DateTime CreateDate { get; init; }
    }
}