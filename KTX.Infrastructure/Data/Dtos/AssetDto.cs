using Microsoft.AspNetCore.Http;
using QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Infrastructure.Data.Dtos
{
    public class AssetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public AssetStatus Status { get; set; }
    }
    public class AddAssetDto
    {
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public Guid CategoryId { get; set; }
        public AssetStatus Status { get; set; }
    }
}
