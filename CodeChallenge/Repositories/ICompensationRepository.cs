using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
	public interface ICompensationRepository
	{
        Task<Compensation> GetCompensationByEmployeeId(String id);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}

