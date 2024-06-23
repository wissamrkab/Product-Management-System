using AutoMapper;
using MediatR;
using PMS.Application.Interfaces.Repositories;

namespace PMS.Application.Features;

public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected IUnitOfWork UnitOfWork;
    protected IMediator Mediator;
    protected IMapper Mapper;

    protected BaseRequestHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        Mediator = mediator;
        Mapper = mapper;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}