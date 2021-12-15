using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTasks.RazorLibrary
{
    public class TagsEditorComponentModel : ComponentBase
    {
        [Parameter]
        public List<string>? Tags { get; set; }

        [Inject] 
        public IJSRuntime  JsRuntime { get; set; }
        
        //public int TotalTags => Tags?.Count ?? 0;

        protected async Task DeleteTag(string tag)
        {
            if (!Validate(tag))
            {
                return;
            }
            if (!Tags!.Contains(tag))
            {
                return;
            }
            Tags!.Remove(tag);
            // update tags count
            await new RazorInterop(JsRuntime).SetTagsTotal(Tags.Count);
        }

        private bool Validate(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return false;
            }

            if (Tags is null)
            {
                return false;
            }
            return true;
        }
    }
}
