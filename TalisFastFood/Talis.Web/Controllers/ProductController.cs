using Microsoft.AspNetCore.Mvc;
using Talis.Application.Dtos;
using Talis.Application.Interfaces;
using Talis.Infrastructure.Commons.Bases.Request;
using Talis.Utilities.Converters;
using Talis.Web.Models;

namespace Talis.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApplication _productApplication;

        public ProductController(IProductApplication productApplication)
        {
            this._productApplication = productApplication;
        }
        public async Task<IActionResult> Index(BaseFiltersRequest filters)
        {
            var products = await _productApplication.GetAllAsyncProduct(filters);
            var numMaxRecords = products.Data.TotalRecords;
            var response = new Pagination<ProductRequestDto>()
            {
                TextFilter = filters.TextFilter,
                StateFilter = filters.StateFilter,
                NumPage = filters.NumPage,
                NumRecordsPage = filters.NumRecordsPage,
                NumMaxRecords = numMaxRecords,
                Elements = products.Data.Items,
                BaseURL = Url.Action()
            };

            if (response is null)
            {
                return RedirectToAction("Error", "Home");
            }

            foreach (var element in response.Elements)
            {
                if (element.Fotografía != null)
                {
                    string imageBase64Url = ImageConverter.ConvertImage(element.Fotografía);
                    ViewBag.ImageUrl = imageBase64Url;
                }
            }

            return View(response);
        }
    }
}
