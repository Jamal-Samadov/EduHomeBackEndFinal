using EduHome.Areas.admin.Data;
using EduHome.Areas.admin.Models;
using EduHome.DAL;
using EduHome.DAL.Entities;
using EduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace EduHome.Areas.admin.Controllers
{
    public class EventsController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public EventsController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Event> events = await _dbContext.Events
                .Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker)
                .Where(e => !e.IsDeleted)
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            return View(events);
        }

        public async Task<IActionResult> Create()
        {
            var speakers = await _dbContext.Speakers.ToListAsync();

            var eventSpeakersSelectList = new List<SelectListItem>();

            speakers.ForEach(c => eventSpeakersSelectList
            .Add(new SelectListItem(c.Name + " " + c.Surname, c.Id.ToString())));

            var model = new EventCreateModel
            {
                Speakers = eventSpeakersSelectList
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return Ok(errorList);
            }


            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "Must be selected image");
                return View(model);
            }

            if (!model.Image.ImageAllowed(3))
            {
                ModelState.AddModelError("", "Image size can be max 3 mb");
                return View(model);
            }

            var unicalName = await model.Image.GenerateFile(Constans.EventPath);

            var events = new Event
            {
                EventName = model.EventName,
                Date = model.Date,
                ImageUrl = unicalName,
                Time = model.Time,
                Location = model.Location,
                Description = model.Description,
                Reply =model.Reply,
            };

            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

            foreach (int speakerId in model.SpeakerIds)
            {
                if (!await _dbContext.Speakers.AnyAsync(s => s.Id == speakerId))
                {
                    ModelState.AddModelError("", "Incorect Speaker Id");
                    return View();
                }

                eventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = speakerId,
                });

            }

            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();

            var eventSpeakersSelectList = new List<SelectListItem>();
            speakers.ForEach(c => eventSpeakersSelectList
            .Add(new SelectListItem(c.Name + " " + c.Surname, c.Id.ToString())));

            events.EventSpeakers = eventSpeakers;
            model.Speakers = eventSpeakersSelectList;

            await _dbContext.Events.AddAsync(events);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();

            var mainEvent = await _dbContext.Events
                .Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker)
                .Where(e => !e.IsDeleted && e.Id == id)
                .FirstOrDefaultAsync();

            if (mainEvent is null) return NotFound();

            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();

            var eventSpeakersSelectList = new List<SelectListItem>();

            speakers.ForEach(c => eventSpeakersSelectList
            .Add(new SelectListItem(c.Name + " " + c.Surname, c.Id.ToString())));

            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();

            foreach (EventSpeaker eventSpeaker in mainEvent.EventSpeakers)
            {
                if (!await _dbContext.Speakers.AnyAsync(s => s.Id == eventSpeaker.SpeakerId))
                {
                    ModelState.AddModelError("", "Incorect Speaker Id");
                    return View();
                }

                eventSpeakers.Add(new EventSpeaker
                {
                    SpeakerId = eventSpeaker.SpeakerId
                });
            }

            var eventViewModel = new EventUpdateModel
            {
                Id = mainEvent.Id,
                EventName = mainEvent.EventName,
                Date = mainEvent.Date,
                Time = mainEvent.Time,
                Location = mainEvent.Location,
                Description = mainEvent.Description,
                Reply = mainEvent.Reply,
                ImageUrl = mainEvent.ImageUrl,
                Speakers = eventSpeakersSelectList,
                SpeakerIds = eventSpeakers.Select(s => s.SpeakerId).ToList()

            };

            return View(eventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, EventUpdateModel model)
        {

            if (id is null) return BadRequest();

            var mainEvent = await _dbContext.Events
                .Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker)
                .Where(e => !e.IsDeleted && e.Id == id)
                .FirstOrDefaultAsync();

            if (mainEvent is null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var speakers = await _dbContext.Speakers.Where(s => !s.IsDeleted).ToListAsync();

            var eventSpeakersSelectList = new List<SelectListItem>();

            if (model.SpeakerIds.Count > 0)
            {
                foreach (int speakerId in model.SpeakerIds)
                {
                    if (!await _dbContext.Speakers.AnyAsync(c => c.Id == speakerId))
                    {
                        ModelState.AddModelError("", "Has been selected incorrect speaker.");
                        return View(model);
                    }
                }

                _dbContext.EventSpeakers.RemoveRange(mainEvent.EventSpeakers);

                List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();
                foreach (var item in model.SpeakerIds)
                {
                    EventSpeaker eventSpeaker = new EventSpeaker
                    {
                        SpeakerId = item
                    };

                    eventSpeakers.Add(eventSpeaker);
                }
                mainEvent.EventSpeakers = eventSpeakers;
            }
            else
            {
                ModelState.AddModelError("", "Select minimum 1 speaker");
                return View(model);
            }

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(model);
                }

                if (!model.Image.ImageAllowed(3))
                {
                    ModelState.AddModelError("", "Image size can be max 3 mb");
                    return View(model);
                }

                if (mainEvent.ImageUrl is null) return NotFound();

                var eventImagePath = Path.Combine(Constans.RootPath, "assets", "img", "event", mainEvent.ImageUrl);

                if (System.IO.File.Exists(eventImagePath))
                    System.IO.File.Delete(eventImagePath);

                var unicalName = await model.Image.GenerateFile(Constans.EventPath);
                mainEvent.ImageUrl = unicalName;
            }

            mainEvent.EventName = model.EventName;
            mainEvent.Description = model.Description;
            mainEvent.Date = model.Date;
            mainEvent.Time = model.Time;
            mainEvent.Reply = model.Reply;
            mainEvent.Location = model.Location;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existevent = await _dbContext.Events
                  .Include(e => e.EventSpeakers).ThenInclude(es => es.Speaker)
                  .FirstOrDefaultAsync(e => e.Id == id);

            if (existevent is null) return NotFound();

            if (existevent.ImageUrl is null) return NotFound();

            if (existevent.Id != id) return BadRequest();

            var eventImagePath = Path.Combine(Constans.RootPath, "assets", "img", "event", existevent.ImageUrl);


            if (System.IO.File.Exists(eventImagePath))
                System.IO.File.Delete(eventImagePath);

            _dbContext.Events.Remove(existevent);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

    }
}
