using JobMagnet.Models;
using JobMagnet.Service.Interface;

namespace JobMagnet.Service;

public class SummaryService : ISummaryService
{
    public Task<SummaryModel> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<SummaryModel> Create(SummaryCreateRequest summaryCreateRequest)
    {
        throw new NotImplementedException();
    }
}