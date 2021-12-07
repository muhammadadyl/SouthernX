using NUnit.Framework;
using SouthernCross.Core.Configs;
using SouthernCross.Core.Entities;
using SouthernCross.Core.Services;
using SouthernCross.Core.Services.HelperServices;
using System;
using System.Threading.Tasks;

namespace SouthernCross.IntegrationTest
{
    public class Tests
    {
        private readonly IMemberService _memberService;
        private readonly ISampleData<Member> _sampleData;
        private Member _testMemberObject;
        public Tests()
        {
            var memberService = new MemberService(
                new SouthernXDatabaseOptions
                {
                    ConnectionString = "mongodb://SouthernX:S0m3p%40ssw0rd@localhost:27017",
                    DatabaseName = "SouthernXDbTest"
                },
                new SimpleMemoryCache(),
                new SampleDataOptions { Path = "Mock/" });

            _memberService = memberService;
            _sampleData = memberService;
        }

        [SetUp]
        public async Task Setup()
        {
            await _sampleData.LoadDataAsync();

            while (true)
            {
                _testMemberObject = await _memberService.GetAsync(new Random().Next(1, 1000));

                // check just to insure that we always get random object
                if (_testMemberObject != null) break;
            }
        }

        [Test]
        public async Task Member_Should_Load()
        {
            var members = await _memberService.GetAsync();
            Assert.IsTrue(members.Count == 1000);
        }

        [Test]
        public async Task Member_Should_GetById()
        {
            var member = await _memberService.GetAsync(_testMemberObject.Id);
            Assert.IsTrue(member?.Id == _testMemberObject.Id);
        }

        [Test]
        public async Task Member_Should_GetByPolicyNumberAndMemberCardNumber()
        {
            var members = await _memberService.GetAsync(_testMemberObject.PolicyNumber, _testMemberObject.MemberCardNumber);
            Assert.IsTrue(members.Count == 1);
            Assert.IsTrue(members[0].Id == _testMemberObject.Id);
        }

        [Test]
        public async Task Member_Should_GetByDateOfBirth()
        {
            var members = await _memberService.GetAsync(_testMemberObject.DateOfBirth);
            Assert.IsTrue(members.Count == 1);
            Assert.IsTrue(members[0].Id == _testMemberObject.Id);
        }
    }
}