﻿using Microsoft.AspNetCore.Mvc;
using netscii.Constants;
using netscii.Models.Dto;
using netscii.Models.Entities;
using netscii.Repositories;
using netscii.Utils.ImageConverters.Models;

namespace netscii.Services
{
    public class ConversionLoggingService
    {
        private readonly ConversionLoggingRepository _repository;

        public ConversionLoggingService(ConversionLoggingRepository repository)
        {
            _repository = repository;
        }

        public async Task LogAsync(string format, ConverterResult converterResult, ConverterOptions converterOptions)
        {
            var activity = new ConversionActivity
            {
                Format = format,
                Timestamp = DateTime.UtcNow,
                Width = converterResult.Width,
                Height = converterResult.Height,
                ProcessingTimeMs = converterResult.ProcessingTimeMs,
                OutputLengthBytes = converterResult.OutputLengthBytes
            };

            var parameters = new ConversionParameters
            {
                Characters = converterOptions.Characters,
                Font = converterOptions.Font,
                Background = converterOptions.Background,
                Scale = converterOptions.Scale,
                Invert = converterOptions.Invert,
                Platform = converterOptions.Platform
            };

            await _repository.LogConversionAsync(activity, parameters);
        }

        public async Task<List<LogDto>> GetLogsAsync(string period)
        {
            return await _repository.GetLogsAsync(ConversionConstants.Periods.GetValueOrDefault(period, TimeSpan.FromHours(24)));
        }

        public async Task<List<LogDto>> GetLogsAsync(int period)
        {
            return await _repository.GetLogsAsync(TimeSpan.FromHours(period));
        }
    }
}
