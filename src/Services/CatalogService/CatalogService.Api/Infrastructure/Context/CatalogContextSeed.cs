using CatalogService.Api.Core.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Api.Infrastructure.Context
{
    public class CatalogContextSeed
    {
        public async Task SeedAsync(CatalogContext context, IWebHostEnvironment env, ILogger<CatalogContextSeed> logger)
        {
            var policy = Policy.Handle<SqlException>()
                .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timespan, retry, ctx) =>
                 {
                     logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message");
                 }
                );

            var setupDirPath = Path.Combine(env.ContentRootPath, "Infrastructue", "Setup", "SeedFiles");
            var picturePath = "Pics";

            await policy.ExecuteAsync(() => ProcessSeeding(context, setupDirPath, picturePath, logger));
        }

        private async Task ProcessSeeding(CatalogContext context, string setupDirPath, string picturePath, ILogger logger)
        {
            if (!context.CatalogBrands.Any())
            {
                await context.CatalogBrands.AddRangeAsync(GetCatalogBrandsFromFile(setupDirPath));

                await context.SaveChangesAsync();
            }

            if (!context.CatalogTypes.Any())
            {
                await context.CatalogTypes.AddRangeAsync(GetCatalogTypesFromFile(setupDirPath));

                await context.SaveChangesAsync();
            }
        }
        private IEnumerable<CatalogBrand> GetCatalogBrandsFromFile(string contentPath)
        {
            string fileName = Path.Combine(contentPath, "CatalogBrands.txt");


            var fileContent = File.ReadAllLines(fileName);

            var list = fileContent.Select(i => new CatalogBrand()
            {
                Brand = i.ToString().Trim('"')
            }).Where(i => i != null);


            return list;
        }
        private IEnumerable<CatalogType> GetCatalogTypesFromFile(string contentPath)
        {
            string fileName = Path.Combine(contentPath, "CatalogTypes.txt");


            var fileContent = File.ReadAllLines(fileName);

            var list = fileContent.Select(i => new CatalogType()
            {
                Type = i.ToString().Trim('"')
            }).Where(i => i != null);
                

            return list;
        }
        private IEnumerable<CatalogItem> GetCatalogItemsFromFile(string contentPath, CatalogContext context)
        {
            string fileName = Path.Combine(contentPath,"CatalogItems.txt");

            var catalogTypeIdLookup = context.CatalogTypes.ToDictionary(ct=>ct.Type,ct=>ct.Id);
            var catalogBrandIdLookup = context.CatalogBrands.ToDictionary(ct=>ct.Brand,ct=>ct.Id);

            var fileContent = File.ReadAllLines(fileName)
                .Skip(1)
                .Select(i => i.Split(","))
                .Select(i => new CatalogItem()
                {
                    CatalogTypeId = catalogTypeIdLookup[i[0].ToString()],
                    CatalogBrandId = catalogBrandIdLookup[i[0].ToString()],
                    Description = i[2].ToString().Trim('"').Trim(),
                    Name = i[3].ToString().Trim('"').Trim(),
                    Price = Decimal.Parse(i[4].ToString().Trim('"').Trim(), System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture),
                    PictureFileName = i[5].ToString().Trim('"').Trim(),
                    AvailableStock = string.IsNullOrEmpty(i[6].ToString()) ? 0 : int.Parse(i[6].ToString()),
                    OnReorder = Convert.ToBoolean(i[7].ToString())
                });

            return fileContent;
        }
        private void GetCatalogItemPicture(string contentPath, string picturePath)
        {
            picturePath ??= "pics";

            if (picturePath != null)
            {
                DirectoryInfo directory = new DirectoryInfo(picturePath);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }

                string zipFileCatalogItemPictures = Path.Combine(contentPath, "CatalogItems.zip");
                ZipFile.ExtractToDirectory(zipFileCatalogItemPictures, picturePath);
            }
        }
    }
}
