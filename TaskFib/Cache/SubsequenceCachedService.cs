using TaskFib.Service;
using TaskFib.Service.Contract;
using TaskFib.WebApi.Utilities;

namespace TaskFib.WebApi.Cache
{
    public class SubsequenceCachedService<T>(
            [FromKeyedServices(ServiceKeys.ValuesCache)] ISequenceValueServiceAsync<T> valueService
        ) : SubsequenceServiceAsync<T>(valueService) where T : struct
    { }
}
