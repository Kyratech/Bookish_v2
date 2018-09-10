﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookish_V2.DataAccessFmwk;

namespace Bookish_V2.Web.Models
{
	public class LibraryItemViewModel
	{
		public Book Book { get; set; }
		public int TotalCopies { get; set; }
		public int AvailableCopies { get; set; }
	}
}