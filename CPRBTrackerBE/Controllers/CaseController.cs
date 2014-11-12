using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using CPRBTrackerBE.Models;
using Newtonsoft.Json;

namespace CPRBTrackerBE.Controllers
{
    public class CaseController : ApiController
    {
        public string connString = System.Configuration.ConfigurationManager.ConnectionStrings["sqlConnectionString"].ConnectionString;

        [HttpPost]
        [Route("v1/NewCase")]
        public int PostNewCase([FromBody] BasicCase newCase)
        {
            var sqlConnection = new SqlConnection(connString);
            int caseId;

            sqlConnection.Open();

            string sql = @"INSERT INTO [Cases] (CaseNo, Summary, IssuedDate, IsResolved)
                           VALUES (@CaseNo, @Summary, @IssuedDate, @IsResolved);
                           SELECT CAST(SCOPE_IDENTITY() as int);";
            caseId = sqlConnection.Query<int>(sql,
                new {
                    CaseNo = newCase.CaseNo,
                    Summary = newCase.Summary,
                    IssuedDate = newCase.IssuedDate,
                    IsResolved = newCase.IsResolved
                }).Single();

            foreach (int allegationId in newCase.Allegations)
            {
                sql = @"INSERT INTO [CaseAllegations] (CaseId, AllegationId)
                        VALUES (@CaseId, @AllegationId);";
                sqlConnection.Execute(sql, new { CaseId = caseId, AllegationId = allegationId });
            }
            
            foreach (int investigatorId in newCase.Investigators)
            {
                sql = @"INSERT INTO [CaseInvestigators] (CaseId, InvestigatorId)
                        VALUES (@CaseId, @InvestigatorId);";
                sqlConnection.Execute(sql, new { CaseId = caseId, InvestigatorID = investigatorId });
            }

            return caseId;
        }

        [HttpGet]
        [Route("v1/Cases")]
        public IEnumerable<CaseSummary> GetCases()
        {
            var sqlConnection = new SqlConnection(connString);

            List<CaseSummary> result = new List<CaseSummary>();
            CaseSummary toAdd;

            sqlConnection.Open();

            IEnumerable<CasesDb> Cases = sqlConnection.Query<CasesDb>("SELECT * FROM Cases");

            foreach (CasesDb Case in Cases)
            {
                toAdd = new CaseSummary();
                string sqlString = @"SELECT TOP 1 Recommendation.RecommendationText, BoardHearing.DateHeld
                                    FROM [Status]
                                    LEFT JOIN Recommendation ON Recommendation.RecommendationId = [Status].RecommendationId
                                    LEFT JOIN BoardHearing ON BoardHearing.BoardHearingId = [Status].BoardHearingId
                                    WHERE CaseId = @Id
                                    ORDER BY BoardHearing.DateHeld";

                dynamic caseData = sqlConnection.Query(sqlString, new { Id = Case.CaseId }).First();
                toAdd.CaseId = Case.CaseId;
                toAdd.CaseNo = Case.CaseNo;
                toAdd.IssuedDate = Case.IssuedDate;
                toAdd.IsResolved = Case.IsResolved;
                toAdd.LatestDisposition = caseData.RecommendationText;
                toAdd.LatestDispositionDate = caseData.DateHeld;
                result.Add(toAdd);
            }
            return result;
        }

        [HttpGet]
        [Route("v1/Case/{caseNumber}")]
        public BasicCase GetCase(string caseNumber)
        {
            var result = new BasicCase();
            var sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();

            string sqlString = "SELECT * FROM Cases WHERE CaseNo=@CaseNo";
            try
            {
                result = sqlConnection.Query<BasicCase>(sqlString, new { CaseNo = caseNumber }).First();
                int id = result.CaseId;
                result.Allegations = sqlConnection.Query<int>("SELECT AllegationId FROM CaseAllegations WHERE CaseId=@id",
                                     new { id = id });
                result.Investigators = sqlConnection.Query<int>("SELECT InvestigatorId FROM CaseInvestigators WHERE CaseId=@id",
                                       new { id = id });
            }
            catch (InvalidOperationException ex)
            {
                return new BasicCase();
            }

            return result;
        }

        [HttpGet]
        [Route("v1/Allegations")]
        public IEnumerable<AllegationDb> GetAllegations()
        {
            var sqlConnection = new SqlConnection(connString);

            sqlConnection.Open();

            IEnumerable<AllegationDb> result = sqlConnection.Query<AllegationDb>("SELECT * From Allegation");

            return result;
        }

        [HttpPost]
        [Route("v1/Allegations")]
        public int PostAllegations([FromBody] AllegationDb allegation)
        {
            var sqlConnection = new SqlConnection(connString);

            sqlConnection.Open();

            string sql = @"INSERT INTO [Allegation] (AllegationText, AllegationCode)
                           VALUES(@Text, @Code);
                           SELECT CAST(SCOPE_IDENTITY() as int);";
            
            return sqlConnection.Query<int>(sql, new { Text = allegation.AllegationText, Code = allegation.AllegationCode }).Single();            
        }



        [HttpGet]
        [Route("v1/Investigators")]
        public IEnumerable<InvestigatorDb> GetInvestigators()
        {
            var sqlConnection = new SqlConnection(connString);

            sqlConnection.Open();

            IEnumerable<InvestigatorDb> result = sqlConnection.Query<InvestigatorDb>("SELECT * FROM Investigator");

            return result;
        }
        [HttpPost]
        [Route("v1/Investigators")]
        public int PostInvestigators([FromBody] InvestigatorDb investigator)
        {
            var sqlConnection = new SqlConnection(connString);

            sqlConnection.Open();

            string sql = @"INSERT INTO [Investigator] (InvestigatorFirstName, InvestigatorLastName)
                          VALUES(@FirstName, @LastName);
                          SELECT CAST(SCOPE_IDENTITY() as int);";

            return sqlConnection.Query<int>(sql, new { FirstName = investigator.InvestigatorFirstName, LastName = investigator.InvestigatorLastName }).Single();
        }

        [HttpGet]
        [Route("v1/Recommendations")]
        public IEnumerable<RecommendationDb> GetRecommendations()
        {
            var sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();

            IEnumerable<RecommendationDb> result = sqlConnection.Query<RecommendationDb>("SELECT * FROM Recommendation");

            return result;
        }
        [HttpPost]
        [Route("v1/Recommendations")]
        public int PostRecommendations([FromBody] RecommendationDb recommendation)
        {
            var sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();

            string sql = @"INSERT INTO [Recommendation] (RecommendationText, RecommendationComplete)
                           VALUES (@Text, @Complete);
                           SELECT CAST(SCOPE_IDENTITY() as int);";
            return sqlConnection.Query<int>(sql, new { Text = recommendation.RecommendationText, Complete = recommendation.RecommendationComplete }).Single();
        }

        [HttpGet]
        [Route("v1/Rationales")]
        public IEnumerable<RationaleDb> GetRationales()
        {
            var sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();

            IEnumerable<RationaleDb> result = sqlConnection.Query<RationaleDb>("SELECT * FROM Rationale");

            return result;
        }
        [HttpPost]
        [Route("v1/Rationales")]
        public int PostRationales([FromBody] RationaleDb rationale)
        {
            var sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();

            string sql = @"INSERT INTO [Rationale] (RationaleText)
                           VALUES (@Text);
                           SELECT CAST(SCOPE_IDENTITY() as int);";

            return sqlConnection.Query<int>(sql, new { Text = rationale.RationaleText }).Single();
        }
    }
}
