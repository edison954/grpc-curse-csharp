﻿using Grpc.Core;
using Sqrt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sqrt.SqrtService;

namespace server
{
    public class SqrtServiceImpl: SqrtServiceBase
    {
        public override async Task<SqrtResponse> sqrt(SqrtRequest request, ServerCallContext context)
        {

            //deadline
            await Task.Delay(500);

            int number = request.Number;
            if (number >= 0)
                return new SqrtResponse() { SquareRoot = Math.Sqrt(number) };
            else
                throw new RpcException(new Status(StatusCode.InvalidArgument, "nomber > 0"));

        }

    }
}
