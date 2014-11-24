using Db4objects.Db4o.Messaging;

namespace ComputationBlock
{
	public class ComputationHandler : IMessageRecipient
	{
		public void ProcessMessage(IMessageContext context, object message)
		{
			var request = message as ComputationRequest;
			if (null == request) return;

			var result = request.Computation.Compute(context.Container);

			// store
			context.Container.Set(new ComputationResult
			{
				RequestId = request.Id,
				Result = result
			});

			context.Sender.Send(new ComputationResponse { RequestId = request.Id });
		}
	}
}