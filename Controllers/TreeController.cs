using Microsoft.AspNetCore.Mvc;
using TreeV2.NodeDto;
using TreeV2.Interfaces;

namespace TreeV2.Controllers
{
    public class TreeController : Controller
    {
        private readonly ITreeService _treeService;

        public TreeController(ITreeService treeService)
        {
            _treeService = treeService;
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _treeService.DeleteNodeById(id);

            if (!result.Success)
            {
                return RedirectToAction("Index", new { message = result.Message });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNodeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", new { message = "Wrong Data" });
            }

            var result = await _treeService.CreateNode(dto);

            if (!result.Success)
            {
                return RedirectToAction("Index", new { message = result.Message });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditNodeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", new { message = "Wrong Data" });
            }

            var result = await _treeService.EditNode(dto);

            if (!result.Success)
            {
                return RedirectToAction("Index", new { message = result.Message });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(string? message)
        {
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["sort"]))
            {
                HttpContext.Session.SetString("sort", HttpContext.Request.Query["sort"]);
            }

            ViewData["Message"] = message;
            ViewData["Sort"] = HttpContext.Session.GetString("sort");

            var nodes = await _treeService.GetAllNodesOrderedById();

            return View(nodes);
        }
    }
}