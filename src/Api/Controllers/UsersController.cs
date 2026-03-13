using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserApication.DTOs;
using UserApication.UseCases.CreateUser;
using UserApication.UseCases.DeleteUser;
using UserApication.UseCases.FindAllUser;
using UserApication.UseCases.FindByIdUser;
using UserApication.UseCases.UpdateUser;

namespace UserApiontrollers;

/// <summary>Manages user resources — CRUD operations with pagination.</summary>
[ApiController]
[Route("api/users")]
[Produces("application/json")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Creates a new user.</summary>
    /// <response code="201">User created successfully.</response>
    /// <response code="400">Business rule violation (e.g., duplicate email).</response>
    /// <response code="422">Validation errors in the request body.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(dto.Name, dto.Email, dto.Password);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Business rule violation.",
                Detail = result.Error,
                Instance = HttpContext.Request.Path
            });

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    /// <summary>Returns a paginated list of active users with optional search.</summary>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Records per page (default: 10, max: 100).</param>
    /// <param name="search">Optional text to search in name or email.</param>
    /// <response code="200">Paginated user list.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<UserResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = new FindAllUserQuery(page, pageSize, search);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    /// <summary>Returns a single user by ID.</summary>
    /// <param name="id">User GUID.</param>
    /// <response code="200">User found.</response>
    /// <response code="404">User does not exist or is inactive.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new FindByIdUserQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found.",
                Detail = result.Error,
                Instance = HttpContext.Request.Path
            });

        return Ok(result.Value);
    }

    /// <summary>Updates an existing user's profile.</summary>
    /// <param name="id">User GUID.</param>
    /// <response code="200">User updated successfully.</response>
    /// <response code="400">Business rule violation.</response>
    /// <response code="404">User not found.</response>
    /// <response code="422">Validation errors in the request body.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(id, dto.Name, dto.Email, dto.Password);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error!.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found.",
                    Detail = result.Error,
                    Instance = HttpContext.Request.Path
                });

            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Business rule violation.",
                Detail = result.Error,
                Instance = HttpContext.Request.Path
            });
        }

        return Ok(result.Value);
    }

    /// <summary>Soft-deletes a user by ID.</summary>
    /// <param name="id">User GUID.</param>
    /// <response code="204">User deleted.</response>
    /// <response code="404">User not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found.",
                Detail = result.Error,
                Instance = HttpContext.Request.Path
            });

        return NoContent();
    }
}
