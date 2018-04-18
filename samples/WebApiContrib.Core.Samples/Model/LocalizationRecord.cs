using System.ComponentModel.DataAnnotations;

namespace WebApiContrib.Core.Samples.Model
{
    public class LocalizationRecord
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string LocalizationCulture { get; set; }
        public string ResourceKey { get; set; }

        [Display(Name = "Value")]
        public string ResourceValue { get; set; }
    }
}
