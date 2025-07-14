using AutoMapper;
using ITLANetworking.Core.Application.Features.Comments.Commands.CreateComment;
using ITLANetworking.Core.Application.Features.Posts.Commands.CreatePost;
using ITLANetworking.Core.Application.Features.Posts.Commands.DeletePost;
using ITLANetworking.Core.Application.Features.Posts.Commands.ReactToPost;
using ITLANetworking.Core.Application.Features.Posts.Commands.UpdatePost;
using ITLANetworking.Core.Application.Features.Posts.Queries.GetUserPosts;
using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Application.ViewModels.Post;
using ITLANetworking.Core.Application.ViewModels.User;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITLANetworking.Presentation.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IUserSyncService _userSyncService;
        private readonly IPostReactionRepository _postReactionRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public HomeController(IPostService postService, ICommentService commentService, IUserSyncService userSyncService,IPostReactionRepository postReactionRepository, IMapper mapper, IMediator mediator)
        {
            _postService = postService;
            _commentService = commentService;
            _userSyncService = userSyncService;
            _postReactionRepository = postReactionRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var user = await _userSyncService.GetUserByIdAsync(userId);
                var posts = await _postService.GetAllPostsWithDetailsAsync();

                var userReactions = await _postService.GetByUserIdAsync(userId);

                var orderedPosts = posts.OrderByDescending(p => p.Created).ToList();

                var postViewModels = _mapper.Map<List<PostViewModel>>(orderedPosts);

               


                foreach (var postVm in postViewModels)
                {
                    var userReaction = userReactions.FirstOrDefault(r => r.Id == postVm.Id);
                    postVm.HasUserReacted = userReaction != null;
                    postVm.UserReactionType = userReaction?.ReactionType;
                    postVm.CanEdit = postVm.UserId == userId;
                    postVm.ReactionsCount = await _postReactionRepository.GetReactionCountAsync(postVm.Id, Core.Domain.Enums.ReactionType.Like);
                }

                var viewModel = new PostViewModel
                {
                    Posts = postViewModels,
                    User = _mapper.Map<UserViewModel>(user),
                    NewPost = new SavePostViewModel(),
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar las publicaciones. Por favor, intenta de nuevo.";
                return View(new PostViewModel
                {
                    Posts = new List<PostViewModel>(),
                    User = new UserViewModel(),
                    NewPost = new SavePostViewModel()
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(SavePostViewModel vm)
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(vm.Content))
            {
                TempData["ErrorMessage"] = "El contenido de la publicación es requerido.";
                return RedirectToAction("Index");
            }

            try
            {
                var command = new CreatePostCommand
                {
                    Content = vm.Content.Trim(),
                    ImageUrl = string.IsNullOrWhiteSpace(vm.ImageUrl) ? null : vm.ImageUrl.Trim(),
                    VideoUrl = string.IsNullOrWhiteSpace(vm.VideoUrl) ? null : vm.VideoUrl.Trim(),
                    UserId = userId
                };

                await _mediator.Send(command);
                TempData["SuccessMessage"] = "¡Publicación creada exitosamente!";
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = "Error al crear la publicación. Por favor, intenta de nuevo.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(int postId, string content, int? parentCommentId = null)
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Debes iniciar sesión para comentar.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "El comentario no puede estar vacío.";
                return RedirectToAction("Index");
            }

            try
            {
                var command = new CreateCommentCommand
                {
                    Content = content.Trim(),
                    PostId = postId,
                    UserId = userId,
                    ParentCommentId = parentCommentId
                };

                var result = await _mediator.Send(command);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = "Error al agregar el comentario. Por favor, intenta de nuevo.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReactToPost(int postId, int reactionType)
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Debes iniciar sesión para reaccionar.";
                return RedirectToAction("Index");
            }

            try
            {
                var command = new ReactToPostCommand
                {
                    PostId = postId,
                    UserId = userId,
                    ReactionType = reactionType
                };

                var result = await _mediator.Send(command);
                TempData["SuccessMessage"] = "Reacción actualizada exitosamente.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error al procesar la reacción. Por favor, intenta de nuevo.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPosts(string userId)
        {
            try
            {
                var query = new GetUserPostsQuery { UserId = userId };
                var result = await _mediator.Send(query);
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = "Error al obtener las publicaciones" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int postId, SavePostViewModel vm)
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(vm.Content))
            {
                TempData["ErrorMessage"] = "El contenido de la publicación es requerido.";
                return RedirectToAction("Index");
            }

            try
            {
                var post = await _postService.GetByIdAsync(postId);
                if (post == null || post.UserId != userId)
                {
                    TempData["ErrorMessage"] = "No tienes permisos para editar esta publicación.";
                    return RedirectToAction("Index");
                }

                var command = new UpdatePostCommand
                {
                    Id = postId,
                    Content = vm.Content.Trim(),
                    ImageUrl = string.IsNullOrWhiteSpace(vm.ImageUrl) ? null : vm.ImageUrl.Trim(),
                    VideoUrl = string.IsNullOrWhiteSpace(vm.VideoUrl) ? null : vm.VideoUrl.Trim(),
                    UserId = userId
                };

                await _mediator.Send(command);
                TempData["SuccessMessage"] = "¡Publicación actualizada exitosamente!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar la publicación. Por favor, intenta de nuevo.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                var post = await _postService.GetByIdAsync(postId);
                if (post == null || post.UserId != userId)
                {
                    TempData["ErrorMessage"] = "No tienes permisos para eliminar esta publicación.";
                    return RedirectToAction("Index");
                }

                var command = new DeletePostCommand { Id = postId, UserId = userId };
                await _mediator.Send(command);
                TempData["SuccessMessage"] = "Publicación eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar la publicación. Por favor, intenta de nuevo.";
            }

            return RedirectToAction("Index");
        }


        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        private string GetCurrentUserName()
        {
            return User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        }

        private string GetCurrentUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        }

        private bool IsInRole(string role)
        {
            return User.IsInRole(role);
        }
    }
}