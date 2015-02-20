using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Katchulo.PowerDapper.Infra.DapperRepository;
using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Katchulo.PowerDapper.Tests
{
    [TestClass]
    public class DapperTableCRUDTests
    {

        [Dapper.Contrib.Extensions.Table("UserToken")]
        protected class FakeUserToken
        {
            [Key]
            public int Id { get; set; }

            [Column("AppKey")]
            public int TestColumn { get; set; }

            public int UserId { get; set; }

            public string Token { get; set; }

            public DateTime DateCreated { get; set; }

            public DateTime DateExpiration { get; set; }

            public bool Active { get; set; }

            public FakeUserToken()
            {

            }
        }

        private DapperTable<FakeUserToken> _sut;

        protected DapperTable<FakeUserToken> SUT
        {
            get
            {
                if (_sut == null)
                {
                    _sut = new DapperTable<FakeUserToken>("UserToken");
                }
                return _sut;
            }
        }

        protected FakeUserToken Fake { get; set; }

        [TestMethod]
        public void CRUDFakeUserTokenTest()
        {
            this.InsertFakeUserTokenTest();
            this.UpdateFakeUserTokenTest();
            this.DeleteFakeUserTokenTest();
        }
        public void InsertFakeUserTokenTest()
        {
            //arrage
            Fake = new FakeUserToken();
            Fake.Active = false;
            Fake.DateCreated = DateTime.Now;
            Fake.DateExpiration = DateTime.Now.AddMinutes(30);
            Fake.TestColumn = 1;
            Fake.Token = "ASDFGHJKL";
            Fake.UserId = 8689;
            
            //assert
            Fake.Id = (int)SUT.Create(Fake);
            //act
            Assert.IsTrue(Fake.Id > 0);
        }

        public void UpdateFakeUserTokenTest()
        {
            //arrage
            Fake.Token = "ZZZZZZZZZZZZZZZ";
            //assert
            SUT.Update(Fake);
            FakeUserToken newFake = SUT.GetById(Fake.Id);
            //act
            Assert.AreEqual(Fake.Token, newFake.Token);
        }

        public void DeleteFakeUserTokenTest()
        {
            //arrage
            //assert
            SUT.Delete(Fake);
            FakeUserToken newFake = SUT.GetById(Fake.Id);
            //act
            Assert.IsNull(newFake);

        }


    }
}
