using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MathTasks.ViewModels
{
    public class IdentityUserEditViewModel : IEnumerable<UserClaim>
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;
        
        [Display(Name ="Is Administrator")]
        public UserClaim? IsAdmin { get; set; }

        [Display(Name ="Content Editor")]
        public IList<UserClaim>? MathTaskContentEditorClaims { get; set; }

        public IEnumerator<UserClaim> GetEnumerator()
        {
            var query1 = typeof(IdentityUserEditViewModel).GetProperties()
                .Where(_ => _.PropertyType == typeof(UserClaim))
                .Select(_ => _.GetValue(this))
                .Where(_ => _ is not null)
                .Cast<UserClaim>();

            var query2 = typeof(IdentityUserEditViewModel).GetProperties()
                .Where(_ => _.PropertyType == typeof(IList<UserClaim>))
                .SelectMany(_ => _.GetValue(this) as IList<UserClaim> ?? Enumerable.Empty<UserClaim>())
                .Where(_ => _ is not null);

            var query3 = query1.Concat(query2);
            return query3.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}