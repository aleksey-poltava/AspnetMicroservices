using System;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer
{
	public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
	{
        private readonly IMapper _mapper;
        private readonly IMediator _mediatR;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IMapper mapper, IMediator mediatR, ILogger<BasketCheckoutConsumer> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediatR = mediatR ?? throw new ArgumentNullException(nameof(mediatR));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);

            var result = await _mediatR.Send(command);

            _logger.LogInformation("BasketCheckoutEvent consumed succesfully. Created Order Id : {newOrderId}", result);
        }
    }
}

