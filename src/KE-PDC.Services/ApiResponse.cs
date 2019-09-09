using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace KE_PDC.Services
{
    public class ApiResponse
    {
        public bool Success = false;
        public List<object> Errors = new List<object>();
        public List<object> Messages = new List<object>();
        public object Result = null;
        public object ResultInfo = null;
        public string Environment = string.Empty;

        public object Render()
        {
            object value = null;

            if (this.ResultInfo == null)
            {
                value = new
                {
                    success = this.Success,
                    errors = this.Errors,
                    messages = this.Messages,
                    result = this.Result,
                };
            }
            else
            {
                value = new
                {
                    success = this.Success,
                    errors = this.Errors,
                    messages = this.Messages,
                    result = this.Result,
                    resultInfo = this.ResultInfo,
                };
            }

            this.Success = false;
            this.Errors = new List<object>();
            this.Messages = new List<object>();
            this.Result = null;
            this.ResultInfo = null;

            return value;
        }

        public object RenderError(ModelStateDictionary modelState)
        {
            Success = false;
            modelState.Where(ms => ms.Value.Errors.Count > 0).ToList().ForEach(ms => {
                Errors.Add(new
                {
                    Key = ms.Key,
                    Message = ms.Value.Errors.First().ErrorMessage
                });
            });

            return Render();
        }
    }
}
