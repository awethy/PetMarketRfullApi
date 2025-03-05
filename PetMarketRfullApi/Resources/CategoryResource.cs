﻿using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Resources
{
    public class CategoryResource
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
