using Microsoft.AspNetCore.Mvc;
using HotelCRM.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelCRM.Controllers
{
  public class RoomsController : Controller
  {
    private readonly HotelCRMContext _db;

    public RoomsController(HotelCRMContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Rooms.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.GuestId = new SelectList(_db.Guests, "GuestId", "Name");
      ViewBag.PropertyId = new SelectList(_db.Properties, "PropertyId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Room room, int GuestId, int PropertyId)
    {
      _db.Rooms.Add(room);
      if (GuestId != 0)
      {
        _db.GuestRoom.Add(new GuestRoom() { GuestId = GuestId, RoomId = room.RoomId });
      }
      if (PropertyId != 0)
      {
        _db.RoomProperty.Add(new RoomProperty() { PropertyId = PropertyId, RoomId = room.RoomId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisRoom = _db.Rooms
        .Include(Room => Room.Guests)
        .ThenInclude(join => join.Guest)
        .Include(Room => Room.Properties)
        .ThenInclude(join => join.Property)
        .FirstOrDefault(room => room.RoomId == id);
      return View(thisRoom);
    }

    public ActionResult Edit (int id)
    {
      var thisRoom = _db.Rooms.FirstOrDefault(rooms => rooms.RoomId == id);
      ViewBag.PropertyId = new SelectList(_db.Properties, "PropertyId", "Name");
      ViewBag.GuestId = new SelectList(_db.Guests, "GuestId", "Name");
      return View(thisRoom);
    }

    [HttpPost]
    public ActionResult Edit(Room room, int GuestId, int PropertyId)
    {
      if (GuestId != 0)
      {
        _db.GuestRoom.Add(new GuestRoom() { GuestId = GuestId, RoomId = room.RoomId });
      }
      if (PropertyId != 0)
      {
        _db.RoomProperty.Add(new RoomProperty() { PropertyId = PropertyId, RoomId = room.RoomId });
      }
      _db.Entry(room).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddProperty(int id)
    {
      var thisRoom = _db.Rooms.FirstOrDefault(RoomsController => RoomsController.RoomId == id);
      ViewBag.PropertyId = new SelectList(_db.Properties, "PropertyId", "Name");
      return View(thisRoom);
    }

    [HttpPost]
    public ActionResult AddProperty(Room room, int PropertyId)
    {
      if (PropertyId != 0)
      {
        _db.RoomProperty.Add(new RoomProperty() { PropertyId = PropertyId, RoomId = room.RoomId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisRoom = _db.Rooms.FirstOrDefault(rooms => rooms.RoomId == id);
      return View(thisRoom);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisRoom = _db.Rooms.FirstOrDefault(rooms => rooms.RoomId == id);
      _db.Rooms.Remove(thisRoom);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}

      // var room = _db.Rooms.id;
      // ViewBag.Property = new SelectList(_db.Properties.propertyId.Name, "PropertyId", "Name");

      // var thisRoom = _db.Rooms.FirstOrDefault(room => room.RoomId == id);
      // var thisProperty = _db.Properties.FirstOrDefault(property => property.PropertyId == thisRoom.PropertyId);
      // var propertyName = thisProperty.Name;