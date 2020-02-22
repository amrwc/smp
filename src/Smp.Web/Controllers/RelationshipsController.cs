using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smp.Web.Models;
using Smp.Web.Repositories;
using Smp.Web.Services;

namespace Smp.Web.Controllers
{
    [Route("api/[controller]")]
    public class RelationshipsController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IRelationshipsRepository _relationshipsRepository;

        public RelationshipsController(IAuthService authService, IRelationshipsRepository relationshipsRepository)
        {
            _authService = authService;
            _relationshipsRepository = relationshipsRepository;
        }

        [HttpGet("[action]/{userOneId:Guid}/{userTwoId:Guid}/{relationshipTypeId:int}"), Authorize]
        public async Task<IActionResult> GetRelationship(Guid userOneId, Guid userTwoId, byte relationshipTypeId)
        {
            var tkn = Request.Headers["Authorization"];
            if (!(_authService.AuthorizeSelf(tkn, userOneId) || _authService.AuthorizeSelf(tkn, userTwoId)))
                return Unauthorized();

            var relationship = await _relationshipsRepository.GetRelationshipByIdsAndType(userOneId, userTwoId,
                (RelationshipType) relationshipTypeId);

            return relationship == null ? (IActionResult) NotFound() : Ok(relationship);
        }

        [HttpGet("[action]/{userId:Guid}/{relationshipTypeId:int}"), Authorize]
        public async Task<IActionResult> GetRelationships(Guid userId, byte relationshipTypeId)
        {
            string tkn = Request.Headers["Authorization"];
            if (!_authService.AuthorizeSelf(tkn, userId))
            {
                return Unauthorized();
            }
            IList<Relationship> relationships = await _relationshipsRepository.GetRelationshipsByIdAndType(
                userId, (RelationshipType) relationshipTypeId);
            return relationships.Count > 0 ? Ok(relationships) : (IActionResult) NoContent();
        }
    }
}
