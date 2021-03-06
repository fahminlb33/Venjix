﻿using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Venjix.Infrastructure.Services.Options
{
    public interface IVenjixOptionsService
    {
        VenjixOptions Options { get; set; }

        Task Reload();
        Task Save();
    }

    public class VenjixOptionsService : IVenjixOptionsService
    {
        private static readonly string _jsonPath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "data/settings.json");

        public VenjixOptions Options { get; set; }

        public async Task Reload()
        {
            if (!File.Exists(_jsonPath))
            {
                Options = new VenjixOptions();
                return;
            }

            var json = await File.ReadAllTextAsync(_jsonPath);
            Options = JsonConvert.DeserializeObject<VenjixOptions>(json);
        }

        public async Task Save()
        {
            await File.WriteAllTextAsync(_jsonPath, JsonConvert.SerializeObject(Options));
        }
    }
}