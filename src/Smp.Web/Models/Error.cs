namespace Smp.Web.Models
{
    public class Error
    {
        public Error(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}