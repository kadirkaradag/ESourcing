﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ESourcing.UI.ViewModel
{
    public class AuctionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please fill name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please fill Description")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Please fill Product")]

        public string ProductId { get; set; }
        [Required(ErrorMessage = "Please fill Quantity")]

        public int Quantity { get; set; }
        [Required(ErrorMessage = "Please fill Start Date")]

        public DateTime StartedAt { get; set; }
        [Required(ErrorMessage = "Please fill Finish Date")]

        public DateTime FinishedAt { get; set; }
        public DateTime CreatedTime { get; set; }
        public int Status { get; set; }
        public List<string> IncludedSellers { get; set; }
    }
}
