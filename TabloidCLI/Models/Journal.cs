//Created by Kelley Crittenden
//This file holds a template for making a Journal Entry

using System;
using System.Collections.Generic;
using System.Text;

namespace TabloidCLI.Models
{
    public class Journal
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateDateTime { get; set; }

    }
}