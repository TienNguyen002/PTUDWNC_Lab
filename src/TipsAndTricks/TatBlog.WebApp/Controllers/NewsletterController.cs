using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly ISubscriberRepository _subscriberRepository;
        public NewsletterController(ISubscriberRepository subscriberRepository)
        {
            _subscriberRepository = subscriberRepository;
        }
        public async Task<IActionResult> Subscribe(string email)
        {
            var subscribe = await _subscriberRepository.SubscribeAsync(email);
            return View(subscribe);
        }
        public async Task<IActionResult> Unsubscribe(string email)
        {
            return View();
        }
    }
}
