﻿using System.ComponentModel.DataAnnotations;

namespace ConferenceDTO
{
    public class Speaker
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? Bio {  get; set; }

        [StringLength(200)]
        public string? Website { get; set; }
    }
}
