using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorCausality
{
    public class PropertyBase : EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
