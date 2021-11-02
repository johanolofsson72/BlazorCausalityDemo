using System.Collections.Generic;

namespace BlazorCausality
{
    public class EntityResponse<TEntity> where TEntity : class
    {
        public bool Success { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public List<string> InfoMessages { get; set; } = new List<string>();
        public TEntity Data { get; set; }
    }
}
