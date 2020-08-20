using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheWall.Models;

namespace TheWall.Controllers
{
    public class WallController : Controller
    {
        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("userId");
            }
        }

        private bool isLoggedIn
        {
            get
            {
                return uid != null;
            }
        }

        private TheWallContext db;

        public WallController(TheWallContext context)
        {
            db = context;
        }

        [HttpGet("/wall")]
        public IActionResult All()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Login", "Home");
            }
            List<Message> allMessages = db.Messages
                .Include(mess => mess.Author)
                .Include(mess => mess.MessageComments)
                .ThenInclude(comm => comm.User)
                .OrderByDescending(mess => mess.CreatedAt)
                .ToList();
            ViewBag.AllMessages = allMessages;    
            return View("Wall");
        }

        [HttpPost("/wall/create")]
        public IActionResult Create(Message newMessage)
        {
            if (ModelState.IsValid)
            {
                newMessage.UserId = (int)uid;
                db.Messages.Add(newMessage);
                db.SaveChanges();
                return RedirectToAction("All", "Wall");
            }
            List<Message> allMessages = db.Messages
                .Include(mess => mess.Author)
                .Include(mess => mess.MessageComments)
                .ThenInclude(comm => comm.User)
                .OrderByDescending(mess => mess.CreatedAt)
                .ToList();
            ViewBag.AllMessages = allMessages;
            return View("Wall");
        }

        [HttpPost("/wall/{messageId}/comment/create")]
        public IActionResult CreateComment(Comment newComment, int messageId, string commentBody)
        {
            if (commentBody != null)
            {
                newComment.UserId = (int)uid;
                newComment.MessageId = messageId;
                newComment.CommentBody = commentBody;
                db.Comments.Add(newComment);
                db.SaveChanges();
                return RedirectToAction("All", "Wall");
            }
            List<Message> allMessages = db.Messages
                .Include(mess => mess.Author)
                .Include(mess => mess.MessageComments)
                .ThenInclude(comm => comm.User)
                .OrderByDescending(mess => mess.CreatedAt)
                .ToList();
            ViewBag.AllMessages = allMessages;
            return View("Wall");
        }

        [HttpGet("/wall/{messageId}/delete")]
        public IActionResult Delete(int messageId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Login", "Home");
            }
            Message messageToDelete = db.Messages.FirstOrDefault(mess => mess.MessageId == messageId);
            bool lessThanThirty = (DateTime.Now - messageToDelete.CreatedAt).TotalMinutes < 30;
            if (!lessThanThirty)
            {
                List<Message> allMessages = db.Messages
                    .Include(mess => mess.Author)
                    .Include(mess => mess.MessageComments)
                    .ThenInclude(comm => comm.User)
                    .OrderByDescending(mess => mess.CreatedAt)
                    .ToList();
                ViewBag.AllMessages = allMessages;
            return View("Wall");
            }
            if (messageToDelete != null)
            {
                db.Messages.Remove(messageToDelete);
                db.SaveChanges();
            }
            return RedirectToAction("All", "Wall");
        }
    }
}