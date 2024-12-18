﻿using ItalyGeo.API.Data;
using ItalyGeo.API.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace ItalyGeo.API.Repositories
{
    public class SqlProvinceRepository : IProvinceRepository
    {
        private readonly ItalyGeoDbContext _dbContext;

        public SqlProvinceRepository(ItalyGeoDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Province>> GetAllAsync(string? filterOn = null, string? filterQuery = null, bool orderByDescending = false)
        {
            var provinces = _dbContext.Provinces.Include(x => x.Region).AsQueryable();

            // Filtering by province name or region name
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("provincename", StringComparison.OrdinalIgnoreCase))
                {
                    provinces = provinces.Where(x => x.Name.Contains(filterQuery));
                }

                if (filterOn.Equals("regionname", StringComparison.OrdinalIgnoreCase))
                {
                    provinces = provinces.Where(x => x.Region.Name.Contains(filterQuery));
                }
            }

            // Sorting
            if (orderByDescending == true)
            {
                provinces = provinces.OrderByDescending(x => x.Name);
            }
            else
            {
                provinces = provinces.OrderBy(x => x.Name);
            }

            return await provinces.ToListAsync();
        }

        public async Task<Province?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Provinces.Include(x => x.Region).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Province?> GetByWikiPagePathAsync(string WikiPagePath)
        {
            return await _dbContext.Provinces.Include(x => x.Region).FirstOrDefaultAsync(x => x.WikipediaPagePath == WikiPagePath);
        }

        public async Task<Province> CreateAsync(Province province)
        {
            await _dbContext.Provinces.AddAsync(province);
            await _dbContext.SaveChangesAsync();

            // Load Region (taken from https://stackoverflow.com/questions/26610337/get-entity-navigation-properties-after-insert) 
            await _dbContext.Entry(province).Reference(x => x.Region).LoadAsync();

            return province;
        }

        public async Task<Province?> UpdateAsync(Guid id, Province province)
        {
            var existingProvince = await _dbContext.Provinces.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProvince == null) return null;

            existingProvince.Name = province.Name;
            existingProvince.RegionId = province.RegionId;
            existingProvince.Latitude = province.Latitude;
            existingProvince.Longitude = province.Longitude;
            existingProvince.WikipediaPagePath = province.WikipediaPagePath;
            existingProvince.Acronym = province.Acronym;
            existingProvince.Areakm2 = province.Areakm2;
            existingProvince.YearCreated = province.YearCreated;
            existingProvince.ZipCode = province.ZipCode;
            existingProvince.GDPPerCapitaEuro = province.GDPPerCapitaEuro;
            existingProvince.GDPNominalMlnEuro = province.GDPNominalMlnEuro;
            existingProvince.ComuneCount = province.ComuneCount;
            existingProvince.InhabitantsPerKm2 = province.InhabitantsPerKm2;
            existingProvince.Population = province.Population;
            existingProvince.Timezone = province.Timezone;
            existingProvince.CapaluogoComuneId = province.CapaluogoComuneId;

            await _dbContext.SaveChangesAsync();

            // Load Region (taken from https://stackoverflow.com/questions/26610337/get-entity-navigation-properties-after-insert) 
            await _dbContext.Entry(existingProvince).Reference(x => x.Region).LoadAsync();

            return existingProvince;
        }

        public async Task<Province?> DeleteAsync(Guid id)
        {
            var existingProvince = await _dbContext.Provinces.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProvince == null) return null;
            _dbContext.Provinces.Remove(existingProvince);
            await _dbContext.SaveChangesAsync();

            // Load Region (taken from https://stackoverflow.com/questions/26610337/get-entity-navigation-properties-after-insert) 
            await _dbContext.Entry(existingProvince).Reference(x => x.Region).LoadAsync();

            return existingProvince;
        }
    }
}
