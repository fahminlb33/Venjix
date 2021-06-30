using System.ComponentModel.DataAnnotations;
using Venjix.Controllers;
using Venjix.Infrastructure.DataAnnotations;

namespace Venjix.Models.ViewModels
{
    public class SensorEditModel
    {
        public int SensorId { get; set; }

        [NotEqual(DataController.ApiKeyField)]
        [StringLength(100, MinimumLength = 3)]
        public string ApiField { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string DisplayName { get; set; }

        public bool IsEdit { get; set; }
    }
}