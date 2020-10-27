using System.ComponentModel.DataAnnotations;

namespace Venjix.Infrastructure.DAL
{
    public class Setting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
