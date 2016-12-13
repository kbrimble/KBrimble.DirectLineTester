﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using KBrimble.DirectLineTester.Assertions.Cards;
using KBrimble.DirectLineTester.Exceptions;
using KBrimble.DirectLineTester.Models.Cards;
using NUnit.Framework;

namespace KBrimble.DirectLineTester.Tests.Unit.CardAssertionTests.When_testing_hero_cards
{
    [TestFixture]
    public class For_text_matching
    {
        [TestCase("some text")]
        [TestCase("")]
        [TestCase("symbols ([*])?")]
        public void HasTextMatching_should_pass_if_regex_exactly_matches_message_Text(string cardTextAndRegex)
        {
            var heroCard = new HeroCard(text: cardTextAndRegex);

            var sut = new HeroCardAssertions(heroCard);

            Action act = () => sut.HasTextMatching(cardTextAndRegex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "SOME TEXT")]
        [TestCase(@"SYMBOLS ([*])?", @"symbols ([*])?")]
        public void HasTextMatching_should_pass_regardless_of_case(string cardText, string regex)
        {
            var heroCard = new HeroCard(text: cardText);

            var sut = new HeroCardAssertions(heroCard);

            Action act = () => sut.HasTextMatching(regex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "so.*xt")]
        [TestCase("some text", "[a-z ]*")]
        [TestCase("some text", "s(ome tex)t")]
        public void HasTextMatching_should_pass_when_using_standard_regex_features(string cardText, string regex)
        {
            var heroCard = new HeroCard(text: cardText);

            var sut = new HeroCardAssertions(heroCard);

            Action act = () => sut.HasTextMatching(regex);

            act.ShouldNotThrow<Exception>();
        }

        [TestCase("some text", "some text!")]
        [TestCase("some text", "^[j-z ]*$")]
        [TestCase("some text", "s{12}")]
        public void HasTextMatching_should_throw_HeroCardAssertionFailedException_for_non_matching_regexes(string cardText, string regex)
        {
            var heroCard = new HeroCard(text: cardText);

            var sut = new HeroCardAssertions(heroCard);

            Action act = () => sut.HasTextMatching(regex);

            act.ShouldThrow<HeroCardAssertionFailedException>();
        }

        [Test]
        public void HasTextMatching_should_not_output_matches_when_regex_does_not_match_text()
        {
            IList<string> matches = null;

            var heroCard = new HeroCard(text: "some text");

            var sut = new HeroCardAssertions(heroCard);

            Action act = () => sut.HasTextMatching("non matching regex", "(some text)", out matches);

            act.ShouldThrow<HeroCardAssertionFailedException>();
            matches.Should().BeNull();
        }

        [Test]
        public void HasTextMatching_should_not_output_matches_when_groupMatchingRegex_does_not_match_text()
        {
            IList<string> matches;

            var heroCard = new HeroCard(text: "some text");

            var sut = new HeroCardAssertions(heroCard);

            sut.HasTextMatching("some text", "(non matching)", out matches);

            matches.Should().BeNull();
        }

        [Test]
        public void HasTextMatching_should_output_matches_when_groupMatchingRegex_matches_text()
        {
            IList<string> matches;

            const string someText = "some text";
            var heroCard = new HeroCard(text: someText);

            var sut = new HeroCardAssertions(heroCard);

            sut.HasTextMatching(someText, $"({someText})", out matches);

            matches.First().Should().Be(someText);
        }

        [Test]
        public void HasTextMatching_should_output_multiple_matches_when_groupMatchingRegex_matches_text_several_times()
        {
            IList<string> matches;

            const string someText = "some text";
            var heroCard = new HeroCard(text: someText);

            var sut = new HeroCardAssertions(heroCard);

            const string match1 = "some";
            const string match2 = "text";
            sut.HasTextMatching(someText, $"({match1}) ({match2})", out matches);

            matches.Should().Contain(match1, match2);
        }

        [Test]
        public void HasTextMatching_should_throw_HeroCardAssertionFailedException_when_text_is_null()
        {
            var card = new HeroCard();

            var sut = new HeroCardAssertions(card);

            Action act = () => sut.HasTextMatching("anything");

            act.ShouldThrow<HeroCardAssertionFailedException>();
        }

        [Test]
        public void HasTextMatching_should_throw_HeroCardAssertionFailedException_when_trying_to_capture_groups_but_text_is_null()
        {
            IList<string> matches;
            var card = new HeroCard();

            var sut = new HeroCardAssertions(card);

            Action act = () => sut.HasTextMatching("anything", "(.*)", out matches);

            act.ShouldThrow<HeroCardAssertionFailedException>();
        }

        [Test]
        public void HasTextMatching_should_throw_ArgumentNullException_if_regex_is_null()
        {
            var card = new HeroCard();

            var sut = new HeroCardAssertions(card);

            Action act = () => sut.HasTextMatching(null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void HasTextMatching_should_throw_ArgumentNullException_if_when_capturing_groups_regex_is_null()
        {
            IList<string> matches;

            var card = new HeroCard();

            var sut = new HeroCardAssertions(card);

            Action act = () => sut.HasTextMatching(null, "(.*)", out matches);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void HasTextMatching_should_throw_ArgumentNullException_if_groupMatchRegex_is_null()
        {
            IList<string> matches;

            var card = new HeroCard();

            var sut = new HeroCardAssertions(card);

            Action act = () => sut.HasTextMatching("(.*)", null, out matches);

            act.ShouldThrow<ArgumentNullException>();
        }
    }
}
