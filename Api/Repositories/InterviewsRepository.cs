﻿using System.Collections.Generic;
using Api.Models;
using Api.Services;
using MongoDB.Driver;

namespace Api.Repositories
{
	public class InterviewsRepository
	{
		private readonly IMongoCollection<Interview> _interviews;
		private readonly IMongoCollection<InterviewAnswer> _interviewAnswers;

		public InterviewsRepository(IMongoDatabase database)
		{
			_interviews = database.GetCollection<Interview>("interviews");
			_interviewAnswers = database.GetCollection<InterviewAnswer>("interviewsAnswers");
		}

		public Interview AddInterview(Interview interview)
		{
			_interviews.InsertOne(interview);

			return GetInterview(interview.Id.ToString());
		}

		public IEnumerable<Interview> GetInterviews(string botId)
		{
			return _interviews.Find(x => x.BotId == botId).ToList();
		}

		public Interview GetInterview(string id)
		{
			return _interviews.Find(x => x.Id == MongoService.TryCreateObjectId(id)).FirstOrDefault();
		}

		public bool RemoveInterview(string id)
		{
			var deleteDependensiesResult = _interviewAnswers.DeleteMany(x => x.InterviewId == id);
			
			var deleteResult = _interviews.DeleteOne(x => x.Id == MongoService.TryCreateObjectId(id));

			return deleteResult.DeletedCount > 0 && deleteDependensiesResult.DeletedCount > 0;
		}

	}
}