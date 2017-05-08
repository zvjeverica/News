using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsService.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewsService.ViewModels
{
    public class PersonViewModel
    {
        public Person Person { get; set; }
        public IEnumerable<SelectListItem> AllTopics { get; set; }

        private List<int> _selectedJobTags;
        public List<int> SelectedJobTags
        {
            get
            {
                if (_selectedJobTags == null)
                {
                    if (Person == null)
                        _selectedJobTags = new List<int>();
                    else
                        _selectedJobTags = Person.Subscriptions.Select(m => m.Topic.Id).ToList();
                }
                return _selectedJobTags;
            }
            set { _selectedJobTags = value; }
        }
    }
}
