using Serilog;
using Serilog.Events;

namespace SouthernCross.Core.Extensions
{
    /// <summary>
    /// Extensions for setting up serilog using defaults across the Career project.
    /// </summary>
    public static class SerilogExtensions
    {
        /// <summary>
        /// Setup a default logger.
        /// </summary>
        public static LoggerConfiguration WithDefaults(this LoggerConfiguration config)
        {
            config
                .WithMinimumLevel()
                .WithMinimumLevelOverrides()
                .Enrich.FromLogContext()
                .WriteToConsole();

            return config;
        }

        /// <summary>
        /// Setup minimum levels for debug and release builds.
        /// </summary>
        public static LoggerConfiguration WithMinimumLevel(this LoggerConfiguration config)
        {
#if DEBUG
            config.MinimumLevel.Debug();
#else
            config.MinimumLevel.Information();
#endif
            return config;
        }

        /// <summary>
        /// Setup minium level overrides.
        /// </summary>
        public static LoggerConfiguration WithMinimumLevelOverrides(this LoggerConfiguration config)
        {
            config
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information);
            return config;
        }

        /// <summary>
        /// Write to console using defaults.
        /// </summary>
        public static LoggerConfiguration WriteToConsole(this LoggerConfiguration config, string outputTemplate = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        {
            config.WriteTo.Console(outputTemplate: outputTemplate);
            return config;
        }
    }
}
