﻿namespace StationShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; }


        public float Price { get; set; }
    }
}