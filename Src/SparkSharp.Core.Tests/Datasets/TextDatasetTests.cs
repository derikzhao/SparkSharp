﻿namespace SparkSharp.Core.Tests.Datasets
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SparkSharp.Core.Datasets;

    [TestClass]
    public class TextDatasetTests
    {
        [TestMethod]
        public void GetElements()
        {
            TextDataset ds = new TextDataset("foo\nbar");
            var enumerator = ds.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual("foo", enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual("bar", enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(2, ds.Count());
        }

        [TestMethod]
        public void SplitToChars()
        {
            TextDataset ds = new TextDataset("foo\nbar");
            var enumerator = ds.Split<char>(s => s.ToCharArray()).GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('f', enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('o', enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('o', enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('b', enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('a', enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('r', enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void MapConcat()
        {
            TextDataset ds = new TextDataset("foo\nbar");
            IDataset<string> mapds = ds.Map(i => i + "a");
            var enumerator = mapds.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual("fooa", enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual("bara", enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(2, mapds.Count());
        }

        [TestMethod]
        public void ReduceConcat()
        {
            TextDataset ds = new TextDataset("foo\nbar");
            var result = ds.Reduce<string>((x, y) => x + y);

            Assert.IsNotNull(result);
            Assert.AreEqual("foobar", result);
        }

        [TestMethod]
        public void ReduceConcatWithTake()
        {
            TextDataset ds = new TextDataset("foo\nbar\nzoo");
            var result = ds.Take(2).Reduce<string>((x, y) => x + y);

            Assert.IsNotNull(result);
            Assert.AreEqual("foobar", result);
        }

        [TestMethod]
        public void ReduceSumWithSkip()
        {
            TextDataset ds = new TextDataset("zoo\nwar\nfoo\nbar");
            var result = ds.Skip(2).Reduce<string>((x, y) => x + y);

            Assert.IsNotNull(result);
            Assert.AreEqual("foobar", result);
        }

        [TestMethod]
        public void ReduceCountStrings()
        {
            TextDataset ds = new TextDataset("zoo\nwar\nfoo\nbar");
            var result = ds.Map(s => 1).Reduce<int>((x, y) => x + y);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);
        }
    }
}
