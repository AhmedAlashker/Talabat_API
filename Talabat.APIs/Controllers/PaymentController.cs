using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        private const string whSecret = "whsec_bc8660cdbece4c4b78f2ed86e1a0dc3f6640fde1263d5f43b9967ac0e8647af6";


        public PaymentController(IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketid}")]  // GET: /api/payments/{basketid}
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400, "An Error with your Basket"));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], whSecret/*, 300, false*/);

            Order? order;
             
            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            // Handle the event
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);

                    _logger.LogInformation("Order Is Succeeded Ahmed {0}", order?.PaymentIntentId);
                    _logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);

                    break;
                case Events.PaymentIntentPaymentFailed:
                    order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);

                    _logger.LogInformation("Order Is Failed Ahmed {0}", order?.PaymentIntentId);
                    _logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
                    break;
                default:
                    break;
            }

            return Ok();
        }

    }
}
