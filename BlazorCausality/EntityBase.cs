using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorCausality
{
    public class EntityBase
    {
        [Key]
        /// <summary>
        /// Id
        /// </summary>
        /// <remarks>
        /// This is a base class value
        /// </remarks>
        public int Id { get; set; }


        /// <summary>
        /// UpdatedDate
        /// </summary>
        /// <remarks>
        /// This is a base class value
        /// </remarks>
        public DateTime UpdatedDate { get; set; }
    }
}
