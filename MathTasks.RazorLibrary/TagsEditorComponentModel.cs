using MathTasks.Contracts;
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
        
        public List<string>? Found { get; set; }

        public string TagName { get; set; } = string.Empty;

        [Inject] 
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public ISearchTagsService? SearchTagsService { get; set; }

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

        protected void SearchTags(ChangeEventArgs e)
        {
            if (e?.Value is null)
            {
                Found = null;
                return;
            }
            if (string.IsNullOrEmpty(e.Value.ToString()))
            {
                Found = null;
                return;
            }
            Found = SearchTagsService!.SearchTags(e.Value.ToString()!);
        }

        protected async Task AddTag(string value)
        {
            var tag = value?.ToLower().Trim();
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }
            Tags ??= new List<string>();
            if (!Tags.Exists(x=>x.Equals(tag, StringComparison.InvariantCulture)))
            {
                Tags.Add(tag);
                // update TagsTotal value
                await new RazorInterop(JsRuntime).SetTagsTotal(Tags.Count);
            }
            TagName = String.Empty;
            Found = null;
        }
    }
}
