using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Admissions.Commands.ValidateAdmission
{
    public class ValidateAdmissionCommand: IRequest
    {
        public int CommunityId { get; set; }
        public string UserId { get; set; }
    }

    public class ValidateAdmissionHandler : IRequestHandler<ValidateAdmissionCommand>
    {
        private readonly IApplicationDbContext _context;

        public ValidateAdmissionHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ValidateAdmissionCommand request, CancellationToken cancellationToken)
        {
            var admission = _context.Admissions.FirstOrDefault(a => a.User.Id.Equals(request.UserId) && a.Community.Id == request.CommunityId);
            if (admission == null)
                throw new Exception($"no pending admission for user: {request.UserId} and community: {request.CommunityId}");

            admission.Pending = false;
            admission.JoiningDate = DateTime.Now;

            _context.Admissions.Update(admission);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
