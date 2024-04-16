using System;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Services
{
	public class CompensationService : ICompensationService
	{
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
		{
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        // Pass the call to Create a Compensation to the Repository Layer
        // then save the changes in the DB and return the compensation
        public Compensation CreateCompensation(Compensation compensation)
        {
            if (compensation != null)
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        // Pass the call to Get a Compensation to the Repository Layer
        // and return the compensation
        public Compensation GetCompensationByEmployeeId(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                Compensation compensation = _compensationRepository.GetCompensationByEmployeeId(id).Result;
                // Added to make sure the reponse is not null / the compensation exists
                if (compensation != null)
                {
                    return compensation;
                }
            }

            return null;
        }
    }
}

