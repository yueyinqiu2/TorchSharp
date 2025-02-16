// Copyright (c) .NET Foundation and Contributors.  All Rights Reserved.  See LICENSE in the project root for license information.
#include "THSNN.h"

#include <torch/nn/init.h>

Tensor THSNN_batch_norm(const Tensor input, Tensor running_mean, const Tensor running_var, const Tensor weight, const Tensor bias, const bool training, const double momentum, const double eps)
{
    c10::optional<at::Tensor> w, b, rm, rv;
    if (weight != nullptr) w.emplace(*weight);
    if (bias != nullptr) b.emplace(*bias);
    if (running_mean != nullptr) rm.emplace(*running_mean);
    if (running_var != nullptr) rv.emplace(*running_var);

    CATCH_TENSOR(torch::batch_norm(*input, w, b, rm, rv, training, momentum, eps, false));
}

Tensor THSNN_group_norm(const Tensor input, const int64_t num_groups, const Tensor weight, const Tensor bias, const double eps)
{
    auto opts = torch::nn::functional::GroupNormFuncOptions(num_groups)
        .eps(eps);
    if (weight != nullptr) opts.weight(*weight);
    if (bias != nullptr) opts.bias(*bias);
    CATCH_TENSOR(torch::nn::functional::group_norm(*input, opts));
}

Tensor THSNN_instance_norm(const Tensor input, const Tensor running_mean, const Tensor running_var, const Tensor weight, const Tensor bias, const bool use_input_stats, const double momentum, const double eps)
{
    auto opts = torch::nn::functional::InstanceNormFuncOptions()
        .use_input_stats(use_input_stats)
        .momentum(momentum)
        .eps(eps);
    if (running_mean != nullptr) opts.running_mean(*running_mean);
    if (running_var != nullptr) opts.running_var(*running_var);
    if (weight != nullptr) opts.weight(*weight);
    if (bias != nullptr) opts.bias(*bias);
    CATCH_TENSOR(torch::nn::functional::instance_norm(*input, opts));
}

Tensor THSNN_layer_norm(const Tensor input, const int64_t* normalized_shape, const int64_t normalized_shape_len, const Tensor weight, const Tensor bias, const double eps)
{
    auto opts = torch::nn::functional::LayerNormFuncOptions(
        std::vector<int64_t>(normalized_shape, normalized_shape + normalized_shape_len))
        .eps(eps);
    if (weight != nullptr) opts.weight(*weight);
    if (bias != nullptr) opts.bias(*bias);
    CATCH_TENSOR(torch::nn::functional::layer_norm(*input, opts));
}

Tensor THSNN_local_response_norm(const Tensor input, const int64_t size, const double alpha, const double beta, const double k)
{
    auto opts = torch::nn::functional::LocalResponseNormFuncOptions(size)
        .alpha(alpha)
        .beta(beta)
        .k(k);
    CATCH_TENSOR(torch::nn::functional::local_response_norm(*input, opts));
}
