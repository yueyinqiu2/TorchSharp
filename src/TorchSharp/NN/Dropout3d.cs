// Copyright (c) .NET Foundation and Contributors.  All Rights Reserved.  See LICENSE in the project root for license information.
using System;
using static TorchSharp.torch;
using static TorchSharp.PInvoke.LibTorchSharp;

namespace TorchSharp
{
    using Modules;

    namespace Modules
    {
        /// <summary>
        /// This class is used to represent a Dropout3d module.
        /// </summary>
        public sealed class Dropout3d : nn.Module<Tensor, Tensor>
        {
            internal Dropout3d(double p = 0.5, bool inplace = false) : base(nameof(Dropout3d))
            {
                this.p = p;
                this.inplace = inplace;
            }

            public override Tensor forward(Tensor input)
            {
                var res = THSNN_dropout3d(input.Handle, p, this.training, inplace);
                if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                return new Tensor(res);
            }

            // Rather than spending cycles only to discover that this module has neither
            // parameters nor buffers, just shortcut the move completely.
            protected internal override nn.Module _to(Device device, ScalarType dtype) => this;
            protected internal override nn.Module _to(DeviceType deviceType, int deviceIndex = -1) => this;
            protected internal override nn.Module _to(ScalarType dtype) => this;

            private bool inplace;
            private double p;
        }
    }

    public static partial class torch
    {
        public static partial class nn
        {
            /// <summary>
            /// Randomly zero out entire channels (a channel is a 3D feature map, e.g., the jj -th channel of the ii -th sample in the batched input is a 3D tensor \text{input}[i, j]input[i,j] ).
            /// Each channel will be zeroed out independently on every forward call with probability p using samples from a Bernoulli distribution.
            /// </summary>
            /// <param name="p">Probability of an element to be zeroed. Default: 0.5</param>
            /// <param name="inplace">If set to true, will do this operation in-place. Default: false</param>
            /// <returns></returns>
            public static Dropout3d Dropout3d(double p = 0.5, bool inplace = false)
            {
                return new Dropout3d(p, inplace);
            }

            public static partial class functional
            {
                /// <summary>
                /// Randomly zero out entire channels (a channel is a 3D feature map, e.g., the jj -th channel of the ii -th sample in the batched input is a 3D tensor \text{input}[i, j]input[i,j] ).
                /// Each channel will be zeroed out independently on every forward call with probability p using samples from a Bernoulli distribution.
                /// </summary>
                public static Tensor dropout3d(Tensor input, double p = 0.5, bool training = true, bool inplace = false)
                {
                    var res = THSNN_dropout3d(input.Handle, p, training, inplace);
                    if (res == IntPtr.Zero) { torch.CheckForErrors(); }
                    return new Tensor(res);
                }
            }
        }
    }
}
