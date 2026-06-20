using System.Net.Http.Json;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        public ApiService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            
            var baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5001/api/";
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<IEnumerable<Contract>> GetContractsAsync(DateTime? startDate = null, DateTime? endDate = null, ContractStatus? status = null)
        {
            var url = "contracts?";
            if (startDate.HasValue) url += $"startDate={startDate.Value:yyyy-MM-dd}&";
            if (endDate.HasValue) url += $"endDate={endDate.Value:yyyy-MM-dd}&";
            if (status.HasValue) url += $"status={(int)status.Value}&";

            return await _httpClient.GetFromJsonAsync<IEnumerable<Contract>>(url) ?? new List<Contract>();
        }

        public async Task<Contract?> GetContractByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Contract>($"contracts/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching contract {id}");
                return null;
            }
        }

        public async Task<Contract> CreateContractAsync(Contract contract)
        {
            var response = await _httpClient.PostAsJsonAsync("contracts", contract);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Contract>() ?? contract;
        }

        public async Task UpdateContractStatusAsync(int id, ContractStatus status)
        {
            var response = await _httpClient.PatchAsJsonAsync($"contracts/{id}/status", status);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Client>>("clients") ?? new List<Client>();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Client>($"clients/{id}");
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            var response = await _httpClient.PostAsJsonAsync("clients", client);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Client>() ?? client;
        }

        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ServiceRequest>>("servicerequests") ?? new List<ServiceRequest>();
        }

        public async Task<ServiceRequest?> GetServiceRequestByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ServiceRequest>($"servicerequests/{id}");
        }

        public async Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("servicerequests", request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
            return await response.Content.ReadFromJsonAsync<ServiceRequest>() ?? request;
        }

        public async Task UpdateServiceRequestStatusAsync(int id, RequestStatus status)
        {
            var response = await _httpClient.PatchAsJsonAsync($"servicerequests/{id}/status", status);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Invoice>>("invoices") ?? new List<Invoice>();
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Invoice>($"invoices/{id}");
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            var response = await _httpClient.PostAsJsonAsync("invoices", invoice);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Invoice>() ?? invoice;
        }

        public async Task UpdateInvoiceStatusAsync(int id, InvoiceStatus status)
        {
            var response = await _httpClient.PatchAsJsonAsync($"invoices/{id}/status", status);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Driver>> GetDriversAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Driver>>("drivers") ?? new List<Driver>();
        }

        public async Task<IEnumerable<DriverSchedule>> GetDriverSchedulesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<DriverSchedule>>("driverschedules") ?? new List<DriverSchedule>();
        }

        public async Task<DriverSchedule?> GetDriverScheduleByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<DriverSchedule>($"driverschedules/{id}");
        }

        public async Task<DriverSchedule> CreateDriverScheduleAsync(DriverSchedule schedule)
        {
            var response = await _httpClient.PostAsJsonAsync("driverschedules", schedule);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DriverSchedule>() ?? schedule;
        }

        public async Task UpdateDriverScheduleStatusAsync(int id, TripStatus status)
        {
            var response = await _httpClient.PatchAsJsonAsync($"driverschedules/{id}/status", status);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteDriverScheduleAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"driverschedules/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
