using JobMagnet.Models;
using JobMagnet.Service.Interface;

namespace JobMagnet.Service;

public class SummaryService : ISummaryService
{
    public SummaryService()
    {
        
    }
    public Task<SummaryModel> GetById(int id)
    {
        throw new NotImplementedException();
    }
}