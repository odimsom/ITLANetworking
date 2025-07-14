using ITLANetworking.Core.Application.Features.Friends.Commands.SendFriendRequest;
using ITLANetworking.Core.Application.Features.Friends.Commands.AcceptFriendRequest;
using ITLANetworking.Core.Application.Features.Friends.Queries.GetFriends;
using ITLANetworking.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;

namespace ITLANetworking.Presentation.WebApp.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IFriendshipService _friendshipService;

        public FriendsController(IMediator mediator, IFriendshipService friendshipService)
        {
            _mediator = mediator;
            _friendshipService = friendshipService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            var friends = await _mediator.Send(new GetFriendsQuery { UserId = userId });
            var availableUsers = await _friendshipService.GetAvailableUsers(userId, search);
            
            ViewBag.Friends = friends;
            ViewBag.AvailableUsers = availableUsers;
            ViewBag.SearchTerm = search;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(string receiverId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            var command = new SendFriendRequestCommand
            {
                RequesterId = userId,
                ReceiverId = receiverId
            };

            var response = await _mediator.Send(command);
            
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(int friendshipId)
        {
            var command = new AcceptFriendRequestCommand { FriendshipId = friendshipId };
            var response = await _mediator.Send(command);
            
            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
            }

            return RedirectToAction("Requests");
        }

        [HttpPost]
        public async Task<IActionResult> RejectFriendRequest(int friendshipId)
        {
            try
            {
                await _friendshipService.RejectFriendRequest(friendshipId);
                TempData["SuccessMessage"] = "Solicitud rechazada";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Requests");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriend(string friendId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            try
            {
                await _friendshipService.RemoveFriend(userId, friendId);
                TempData["SuccessMessage"] = "Amigo eliminado";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Requests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            var pendingRequests = await _friendshipService.GetPendingRequests(userId);
            var sentRequests = await _friendshipService.GetSentRequests(userId);
            
            ViewBag.PendingRequests = pendingRequests;
            ViewBag.SentRequests = sentRequests;
            
            return View();
        }
    }
}
