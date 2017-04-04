using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IRSI.Services.Website.Models.AVT;
using IRSI.Services.Website.Models.Common;
using Microsoft.AspNetCore.Authorization;
using IRSI.Services.Website.ApiClients;
using IRSI.Services.Website.Configuration;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRSI.Services.Website.Controllers
{
    [Authorize(Policy = "UseAVTService")]
    public class AVTServiceController : Controller
    {
        private readonly AVTApiClient _avtClient;
        private readonly StoresApiClient _storeClient;
        private decimal _avtTarget;

        public AVTServiceController(AVTApiClient avtClient, StoresApiClient storesClient, IOptions<AVTOptions> options)
        {
            _avtClient = avtClient;
            _storeClient = storesClient;
            _avtTarget = options.Value.AVTTarget;
        }

        public async Task<IActionResult> Index(int page = 1, int page_size = 25)
        {
            var avts = await _avtClient.GetAVTAsync(page, page_size);
            if (avts != null)
            {
                var stores = await _storeClient.GetStoresAsync();
                var storeDictionary = new Dictionary<Guid, Store>();

                foreach (var store in stores.OrderBy(s => s.Number).Where(s => s.Number != "1"))
                {
                    storeDictionary.Add(store.Id, store);
                }

                ViewBag.AVTPage = avts;
                ViewBag.Stores = storeDictionary;
                ViewBag.Page = page;
                ViewBag.PageSize = page_size;
                ViewBag.AVTTarget = _avtTarget;

                if (!string.IsNullOrEmpty(avts._Prev))
                {
                    ViewBag.PreviousPage = Url.Action("Index", "AVTService", new { page = page - 1, page_size = page_size });
                }

                if (!string.IsNullOrEmpty(avts._Next))
                {
                    ViewBag.NextPage = Url.Action("Index", "AVTService", new { page = page + 1, page_size = page_size });
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var stores = await _storeClient.GetStoresAsync();
            var storeDictionary = new Dictionary<Guid, Store>();

            foreach (var store in stores.OrderBy(s => s.Number).Where(s => s.Number != "1"))
            {
                storeDictionary.Add(store.Id, store);
            }
            ViewBag.Stores = storeDictionary;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AVTItem item)
        {
            if (ModelState.IsValid)
            {
                await _avtClient.PostAVTAsync(item);
                return RedirectToAction("Index", "AVTService");
            }

            var stores = await _storeClient.GetStoresAsync();
            var storeDictionary = new Dictionary<Guid, Store>();

            foreach (var store in stores.OrderBy(s => s.Number).Where(s => s.Number != "1"))
            {
                storeDictionary.Add(store.Id, store);
            }
            ViewBag.Stores = storeDictionary;
            ViewBag.AVTTarget = _avtTarget;


            return View(item);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var stores = await _storeClient.GetStoresAsync();
            var storeDictionary = new Dictionary<Guid, Store>();

            foreach (var store in stores.OrderBy(s => s.Number).Where(s => s.Number != "1"))
            {
                storeDictionary.Add(store.Id, store);
            }
            ViewBag.Stores = storeDictionary;
            ViewBag.AVTTarget = _avtTarget;

            var avt = await _avtClient.GetAVTItemAsync(id);
            return View(avt);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AVTItem value)
        {
            if (ModelState.IsValid)
            {
                await _avtClient.UpdateAVTAsync(id, value);
                return RedirectToAction("Index", "AVTService");
            }

            var stores = await _storeClient.GetStoresAsync();
            var storeDictionary = new Dictionary<Guid, Store>();

            foreach (var store in stores.OrderBy(s => s.Number).Where(s => s.Number != "1"))
            {
                storeDictionary.Add(store.Id, store);
            }
            ViewBag.Stores = storeDictionary;
            ViewBag.AVTTarget = _avtTarget;

            return View(value);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _avtClient.DeleteAVTAsync(id);
            return RedirectToAction("Index", "AVTService");
        }
    }
}
