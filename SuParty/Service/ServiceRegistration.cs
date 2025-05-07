namespace SuParty.Service
{
    /// <summary>
    /// 動態註冊
    /// </summary>
    public static class ServiceRegistration
    {
        public static void AddDynamicServices(this IServiceCollection services, IConfiguration config)
        {
            var mappings = config.GetSection("Services").Get<List<ServiceMapping>>() ?? [];

            foreach (var mapping in mappings)
            {
                var interfaceType = Type.GetType(mapping.Interface);
                var implType = Type.GetType(mapping.Implementation);

                if (interfaceType != null && implType != null)
                {
                    switch (mapping.Lifetime.ToLower())
                    {
                        case "singleton":
                            services.AddSingleton(interfaceType, implType);
                            break;
                        case "transient":
                            services.AddTransient(interfaceType, implType);
                            break;
                        default:
                            services.AddScoped(interfaceType, implType);
                            break;
                    }
                }
            }
        }
    }
}
