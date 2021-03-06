﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NHibernate.Dialect;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest
{
	using System.Threading.Tasks;
	/// <summary>
	/// Tests for mapping a type="Time" for a DateTime Property to a database field.
	/// </summary>
	[TestFixture]
	public class BasicTimeFixtureAsync : TestCase
	{
		protected override string[] Mappings
		{
			get { return new string[] {"NHSpecific.BasicTime.hbm.xml"}; }
		}

		private void IgnoreOnMySQL()
		{
			if (Dialect is MySQLDialect)
			{
				Assert.Ignore("MySQL requires TimeSpan for type='time'");
			}
		}

		[Test]
		public async Task InsertAsync()
		{
			IgnoreOnMySQL();

			BasicTime basic = Create(1);

			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();

			s = OpenSession();

			BasicTime basicLoaded = (BasicTime) await (s.LoadAsync(typeof(BasicTime), 1));

			Assert.IsNotNull(basicLoaded);
			Assert.IsFalse(basic == basicLoaded);

			Assert.AreEqual(basic.TimeValue.Hour, basicLoaded.TimeValue.Hour);
			Assert.AreEqual(basic.TimeValue.Minute, basicLoaded.TimeValue.Minute);
			Assert.AreEqual(basic.TimeValue.Second, basicLoaded.TimeValue.Second);

			await (s.DeleteAsync(basicLoaded));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task TimeArrayAsync()
		{
			IgnoreOnMySQL();

			BasicTime basic = Create(1);

			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();

			s = OpenSession();

			BasicTime basicLoaded = (BasicTime) await (s.LoadAsync(typeof(BasicTime), 1));

			Assert.AreEqual(0, basicLoaded.TimeArray.Length);

			basicLoaded.TimeArray = new DateTime[] {new DateTime(2000, 01, 01, 12, 1, 1), new DateTime(1500, 1, 1)};

			await (s.FlushAsync());
			s.Close();

			s = OpenSession();
			basic = (BasicTime) await (s.LoadAsync(typeof(BasicTime), 1));
			// make sure the 0 index saved with values in Time
			Assert.AreEqual(12, basic.TimeArray[0].Hour);
			Assert.AreEqual(1, basic.TimeArray[0].Minute);
			Assert.AreEqual(1, basic.TimeArray[0].Second);

			// make sure the value below 1753 was not written to db - per msdn docs
			// meaning of DbType.Time.  If not written to the db it will have the value
			// of an uninitialized DateTime - which is the min value.
			Assert.AreEqual(DateTime.MinValue, basic.TimeArray[1], "date before 1753 should not have been written");
			await (s.DeleteAsync(basic));
			await (s.FlushAsync());
			s.Close();
		}

		[Test]
		public async Task UpdateAsync()
		{
			IgnoreOnMySQL();

			BasicTime basic = Create(1);

			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();

			s = OpenSession();
			basic = (BasicTime) await (s.LoadAsync(typeof(BasicTime), 1));

			basic.TimeValue = new DateTime(2000, 12, 1, 13, 1, 2);

			await (s.FlushAsync());
			s.Close();

			s = OpenSession();
			// make sure the update went through
			BasicTime basicLoaded = (BasicTime) await (s.LoadAsync(typeof(BasicTime), 1));

			Assert.AreEqual(13, basicLoaded.TimeValue.Hour);
			Assert.AreEqual(1, basicLoaded.TimeValue.Minute);
			Assert.AreEqual(2, basicLoaded.TimeValue.Second);

			await (s.DeleteAsync(basicLoaded));
			await (s.FlushAsync());
			s.Close();
		}

		private BasicTime Create(int id)
		{
			BasicTime basic = new BasicTime();
			basic.Id = id;

			basic.TimeValue = new DateTime(1753, 01, 01, 12, 00, 00, 00);

			return basic;
		}
	}
}