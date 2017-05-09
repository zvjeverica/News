using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsService.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NewsService.ViewModels
{
    public class PersonViewModel
    {
        public Person Person { get; set; }
        public IEnumerable<SelectListItem> AllTopics { get; set; }

        private List<int> _selectedTopics;
        [Required]
        [Display(Name = "Subscriptions")]
        public List<int> SelectedTopics
        {
            get
            {
                if (_selectedTopics == null)
                {
                    if (Person == null)
                        _selectedTopics = new List<int>();
                    else
                        _selectedTopics = Person.Subscriptions.Select(m => m.Topic.Id).ToList();
                }
                return _selectedTopics;
            }
            set { _selectedTopics = value; }
        }
    }
}
