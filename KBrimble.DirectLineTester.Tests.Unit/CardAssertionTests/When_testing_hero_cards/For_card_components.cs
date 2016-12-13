﻿using FluentAssertions;
using KBrimble.DirectLineTester.Assertions.Cards;
using KBrimble.DirectLineTester.Assertions.Cards.CardComponents;
using KBrimble.DirectLineTester.Models.Cards;
using KBrimble.DirectLineTester.Tests.Unit.TestData;
using NUnit.Framework;

namespace KBrimble.DirectLineTester.Tests.Unit.CardAssertionTests.When_testing_hero_cards
{
    [TestFixture]
    public class For_card_components
    {
        [Test]
        public void WithButtonsThat_should_return_CardActionSetAssertions()
        {
            var buttons = CardActionTestData.CreateRandomCardActions();
            var heroCard = new HeroCard(buttons: buttons);

            var sut = new HeroCardAssertions(heroCard);

            sut.WithButtonsThat().Should().BeAssignableTo<CardActionSetAssertions>().And.NotBeNull();
        }

        [Test]
        public void WithCardImageThat_should_return_CardImageAssertions()
        {
            var cardImages = CardImageTestData.CreateRandomCardImages();
            var heroCard = new HeroCard(images: cardImages);

            var sut = new HeroCardAssertions(heroCard);

            sut.WithCardImageThat().Should().BeAssignableTo<CardImageSetAssertions>().And.NotBeNull();
        }

        [Test]
        public void WithTapActionThat_should_return_CardActionAssertions()
        {
            var tap = new CardAction();
            var heroCard = new HeroCard(tap: tap);

            var sut = new HeroCardAssertions(heroCard);

            sut.WithTapActionThat().Should().BeAssignableTo<CardActionAssertions>().And.NotBeNull();
        }
    }
}
