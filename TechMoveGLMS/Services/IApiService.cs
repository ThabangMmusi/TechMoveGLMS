using TechMoveGLMS.Models;

namespace TechMoveGLMS.Services
{
    public interface IApiService
    {
        // Contracts
        Task<IEnumerable<Contract>> GetContractsAsync(DateTime? startDate = null, DateTime? endDate = null, ContractStatus? status = null);
        Task<Contract?> GetContractByIdAsync(int id);
        Task<Contract> CreateContractAsync(Contract contract);
        Task UpdateContractStatusAsync(int id, ContractStatus status);

        // Clients
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client> CreateClientAsync(Client client);

        // Service Requests
        Task<IEnumerable<ServiceRequest>> GetServiceRequestsAsync();
        Task<ServiceRequest?> GetServiceRequestByIdAsync(int id);
        Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request);
        Task UpdateServiceRequestStatusAsync(int id, RequestStatus status);

        // Invoices
        Task<IEnumerable<Invoice>> GetInvoicesAsync();
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceStatusAsync(int id, InvoiceStatus status);

        // Drivers & Schedules
        Task<IEnumerable<Driver>> GetDriversAsync();
        Task<IEnumerable<DriverSchedule>> GetDriverSchedulesAsync();
        Task<DriverSchedule?> GetDriverScheduleByIdAsync(int id);
        Task<DriverSchedule> CreateDriverScheduleAsync(DriverSchedule schedule);
        Task UpdateDriverScheduleStatusAsync(int id, TripStatus status);
        Task DeleteDriverScheduleAsync(int id);
    }
}
