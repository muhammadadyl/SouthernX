using MongoDB.Driver;
using SouthernCross.Core.Configs;
using SouthernCross.Core.Entities;
using SouthernCross.Core.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SouthernCross.Core.Services
{
    public class MemberService : IMemberService, ISampleData<Member>
    {
        private readonly IMongoCollection<Member> _MembersCollection;
        private readonly ISimpleMemoryCache _simpleMemoryCache;
        private readonly SampleDataOptions _sampleDataOptions;

        public MemberService(
            SouthernXDatabaseOptions southernXDatabaseOptions,
            ISimpleMemoryCache simpleMemoryCache,
            SampleDataOptions sampleDataOptions)
        {
            var mongoClient = new MongoClient(southernXDatabaseOptions.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(southernXDatabaseOptions.DatabaseName);

            _MembersCollection = mongoDatabase.GetCollection<Member>(nameof(Member).ToLower());
            _simpleMemoryCache = simpleMemoryCache;
            _sampleDataOptions = sampleDataOptions;
        }

        public async Task<Member> GetAsync(int Id)
        {
            return await _MembersCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<IList<Member>> GetAsync()
        {
            return await _MembersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<Member>> GetAsync(string policyNumber, string memberCardNumber)
        {
            if (!string.IsNullOrEmpty(memberCardNumber) && !string.IsNullOrEmpty(policyNumber))
            {
                return await _simpleMemoryCache.GetOrCreate($"PN{policyNumber}MCN{memberCardNumber}", async () =>
                {
                    return await _MembersCollection.Find(x => x.PolicyNumber == policyNumber && x.MemberCardNumber == memberCardNumber).ToListAsync();
                });
            }

            return await _simpleMemoryCache.GetOrCreate($"PN{policyNumber}", async () =>
            {
                return await _MembersCollection.Find(x => x.PolicyNumber == policyNumber).ToListAsync();
            });
        }

        public async Task<List<Member>> GetAsync(DateTime dateOfBith)
        {
            return await _simpleMemoryCache.GetOrCreate($"DB{dateOfBith.ToString("ddmmyyyy")}", async () =>
            {
                return await _MembersCollection.Find(x => x.DateOfBirth == dateOfBith).ToListAsync();
            });
        }

        public async Task LoadDataAsync()
        {
            if (await _MembersCollection.EstimatedDocumentCountAsync(null) == 0)
            {
                var filePath = Path.Combine(_sampleDataOptions.Path, $"{nameof(Member)}.json");
                if (File.Exists(filePath))
                {
                    using (FileStream stream = File.OpenRead(filePath))
                    {
                        var members = await JsonSerializer.DeserializeAsync<List<Member>>(stream);
                        await _MembersCollection.InsertManyAsync(members);
                    }
                }
            }
        }
    }
}
