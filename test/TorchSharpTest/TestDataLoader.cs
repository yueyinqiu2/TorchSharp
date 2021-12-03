// Copyright (c) .NET Foundation and Contributors.  All Rights Reserved.  See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Xunit;


namespace TorchSharp
{
    public class TestDataLoader
    {
        private class TestDataset : torch.utils.data.Dataset
        {
            public override long Count { get; } = 10;
            public override Dictionary<string, torch.Tensor> GetTensor(long index)
            {
                return new() {{"data", torch.tensor(1)}, {"label", torch.tensor(13)}, {"index", torch.tensor(index)}};
            }
        }

        [Fact]
        public void DatasetTest()
        {
            using var dataset = new TestDataset();
            var d = dataset.GetTensor(0);
            Assert.True(d.ContainsKey("data"));
            Assert.True(d.ContainsKey("index"));
            Assert.True(d.ContainsKey("label"));

            Assert.Equal(d["data"], torch.tensor(1));
            Assert.Equal(d["label"], torch.tensor(13));
            Assert.Equal(d["index"], torch.tensor(0L));
        }

        [Fact]
        public void DataLoaderTest()
        {
            using var dataset = new TestDataset();
            using var dataloader = new torch.utils.data.DataLoader(dataset, 2, false, torch.CPU);
            var iterator = dataloader.GetEnumerator();
            iterator.MoveNext();
            Assert.Equal(iterator.Current["data"], torch.tensor(rawArray: new[]{1L, 1L}, dimensions: new[]{2L}, dtype: torch.ScalarType.Int32));
            Assert.Equal(iterator.Current["label"], torch.tensor(rawArray: new[]{13L, 13L}, dimensions: new[]{2L}, dtype: torch.ScalarType.Int32));
            Assert.Equal(iterator.Current["index"], torch.tensor(rawArray: new[]{0L, 1L}, dimensions: new[]{2L}, dtype: torch.ScalarType.Int64));
            iterator.Dispose();
        }
    }
}