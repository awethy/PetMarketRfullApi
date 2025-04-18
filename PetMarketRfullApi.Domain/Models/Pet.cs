﻿using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Domain.Models
{
    public class Pet
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
