using Microsoft.Extensions.Configuration;
using ProductRepairDataAccess.Models;

namespace ProductRepairDataAccess.Interfaces
{
    public interface IConfigurationSettings
    {
        string GetConnectionString();
    }
}