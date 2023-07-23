﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EGService.Core.Models.ViewModels
{
	public class NameValueNumericViewModel : Base.BaseViewModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
	}
	public class NameValueViewModel : Base.BaseViewModel
	{
		public long Value { get; set; }
		public string Name { get; set; }
	}
}