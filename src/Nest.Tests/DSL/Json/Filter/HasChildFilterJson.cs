﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Nest.DSL;
using Nest.TestData.Domain;

namespace Nest.Tests.Dsl.Json.Filter
{
	[TestFixture]
	public class HasChildFilterJson
	{
		[Test]
		public void HasChildFilter()
		{
			var s = new SearchDescriptor<ElasticSearchProject>().From(0).Size(10)
				.Filter(ff=>ff
					.HasChild<Person>(d=>d
						.Scope("my_scope")
						.Query(q=>q.Term(p=>p.FirstName, "value"))
					)
				);
				
			var json = ElasticClient.Serialize(s);
			var expected = @"{ from: 0, size: 10, 
				filter : {
					""has_child"": {
					  ""type"": ""people"",
					  ""scope"": ""my_scope"",
					  ""query"": {
						""term"": {
						  ""firstName"": {
							""value"": ""value""
						  }
						}
					  }
					}
				}
			}";
			Assert.True(json.JsonEquals(expected), json);		
		}
	}
}
