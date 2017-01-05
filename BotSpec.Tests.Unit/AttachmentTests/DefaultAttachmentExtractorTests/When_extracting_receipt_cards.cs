﻿using System.Collections.Generic;
using System.Linq;
using BotSpec.Attachments;
using BotSpec.Models.Cards;
using FluentAssertions;
using Microsoft.Bot.Connector.DirectLine.Models;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace BotSpec.Tests.Unit.AttachmentTests.DefaultAttachmentExtractorTests
{
    [TestFixture]
    public class When_extracting_receipt_cards
    {
        private IAttachmentRetriever _retriever;

        [SetUp]
        public void SetUp()
        {
            _retriever = Substitute.For<IAttachmentRetriever>();
            AttachmentRetrieverSettings.AttachmentRetrieverType = AttachmentRetrieverType.Custom;
            AttachmentRetrieverSettings.CustomAttachmentRetriever = _retriever;
        }

        [Test]
        public void Only_attachments_with_valid_content_type_should_be_extracted()
        {
            const string validContentType = ReceiptCard.ContentType;
            var validReceiptCard = new ReceiptCard(title: "some text");
            var validJson = JsonConvert.SerializeObject(validReceiptCard);
            var validAttachment = new Attachment("validUrl1", validContentType);

            const string invalidContentType = "invalidContentType";
            var invalidAttachment = new Attachment("validUrl2", invalidContentType);

            var attachments = new List<Attachment> { validAttachment, invalidAttachment };
            var message = new Message(attachments: attachments);

            _retriever.GetAttachmentsFromUrls(Arg.Is<string[]>(arr => arr.Length == 1)).Returns(new[] {validJson});
            _retriever.GetAttachmentsFromUrls(Arg.Is<string[]>(arr => arr.Length == 2)).Returns(new[] {validJson, validJson});

            var sut = new DefaultAttachmentExtractor();

            var returnedCards = sut.ExtractCards<ReceiptCard>(message).ToList();

            returnedCards.Count.Should().Be(1);
        }

        [Test]
        public void Only_attachments_with_valid_json_should_be_extracted()
        {
            var validJson = JsonConvert.SerializeObject(new ReceiptCard(title: "valid"));
            var inValidJson = validJson + "some extra text";
            var attachment = new Attachment(contentType: ReceiptCard.ContentType);
            var message = new Message(attachments: new[] {attachment, attachment});

            _retriever.GetAttachmentsFromUrls(Arg.Any<string[]>()).Returns(new[] {validJson, inValidJson});

            var sut = new DefaultAttachmentExtractor();

            var returnedCards = sut.ExtractCards<ReceiptCard>(message).ToList();

            returnedCards.Count.Should().Be(1);
        }

        [Test]
        public void All_attachments_with_correct_content_type_will_be_extracted_regardless_of_type()
        {
            var receiptCard = new ReceiptCard(title: "some text");
            var receiptJson = JsonConvert.SerializeObject(receiptCard);
            var receiptAttachment = new Attachment("validUrl1", ReceiptCard.ContentType);

            var someOtherType = new {SomeField = "some text"};
            var someOtherTypeJson = JsonConvert.SerializeObject(someOtherType);
            var someOtherTypeAttachment = new Attachment("validUrl2", ReceiptCard.ContentType);

            var attachments = new List<Attachment> { receiptAttachment, someOtherTypeAttachment };
            var message = new Message(attachments: attachments);

            _retriever.GetAttachmentsFromUrls(Arg.Any<string[]>()).Returns(new[] {receiptJson, someOtherTypeJson});

            var sut = new DefaultAttachmentExtractor();

            var returnedCards = sut.ExtractCards<ReceiptCard>(message).ToList();

            returnedCards.Count.Should().Be(2);
            returnedCards.Should().Contain(card => card.Title == null);
        }
    }
}