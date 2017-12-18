﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.EmbedBuilder;
using KugorganeHammerHandler;
using KugorganeHammerHandler.Data_Types;

namespace DiscordBot.Modules
{
    [Group("Smash")]
    public class SmashModule : ModuleBase
    {
        [Group("WiiU")]
        public class WiiU : ModuleBase
        {
            [Command("character")]
            public async Task GetCharacter([Remainder] string name)
            {
                var character = RequestHandler.GetCharacterName(name);
                var moves = RequestHandler.GetMoves(name);
                Discord.EmbedBuilder builder = Builders.BaseBuilder("", "", Color.DarkTeal,
                    new EmbedAuthorBuilder().WithName("KuroganeHammer Result:").WithUrl("http://kuroganehammer.com"),
                    character.ThumbnailURL);
                //builder.WithImageUrl(character.MainImageURL);
                builder.WithUrl(character.FullURL);
                string info = "";
                info += "**Name: **" + character.Name;
                if (!string.IsNullOrEmpty(character.Description))
                {
                    info += "**Description: **" + character.Description;
                }
                if (!string.IsNullOrEmpty(character.Style)) info += "**Style: **" + character.Style;
                builder.AddField(new EmbedFieldBuilder().WithName("Information").WithValue(info));
                var movement = RequestHandler.GetMovement(name);
                var half = movement.Attributes.Count / 2;
                var info1 = "";
                var info2 = "";
                for (int i = 0; i < movement.Attributes.Count / 2; i++)
                {
                    info1 += $"**{movement.Attributes[i].Name}**: {movement.Attributes[i].Value}\n";
                    info2 += $"**{movement.Attributes[i+half].Name}**: {movement.Attributes[i+half].Value}\n";
                }
                builder.AddInlineField("Movement", info1);
                builder.AddInlineField("Information", info2);
                string movesinfo = "";
                string specials = "";
                string aerials = "";
                moves.ForEach(x =>
                {
                    if (x.MoveType == "ground") movesinfo += x.Name + "\n";
                    if (x.MoveType == "special") specials += x.Name + "\n";
                    if (x.MoveType == "aerial") aerials += x.Name + "\n";
                });
                builder.AddInlineField("Ground Moves", movesinfo);
                builder.AddInlineField("Specials", specials);
                builder.AddInlineField("Aerials", aerials);
                await ReplyAsync("", embed: builder.Build());
            }

            [Command("Move")]
            public async Task GetMove(string charactername, [Remainder] string moveName)
            {
                Discord.EmbedBuilder builder = null;
                var moves = RequestHandler.GetMoves(charactername);
                Move move = null;
                move = moves.FirstOrDefault(x=> x.Name.ToLower().Equals(moveName.ToLower()));
                if (move == null) move = moves.FirstOrDefault(x => x.Name.ToLower().Contains(moveName.ToLower()));
                if (move != null)
                {
                    builder = Builders.BaseBuilder("", "", Color.DarkBlue, new EmbedAuthorBuilder().WithName(move.Name), "");
                    string statistics = "";
                    if (!string.IsNullOrEmpty(move.MoveType)) statistics += "**Move Type:** " + move.MoveType + "\n";
                    if (!string.IsNullOrEmpty(move.BaseDamage)) statistics += "**Base Knockback:** " + move.BaseDamage + "\n";
                    if (!string.IsNullOrEmpty(move.BaseKockbackSetKnockback)) statistics += "**Base-, Set-Knockback: **" + move.BaseKockbackSetKnockback + "\n";
                    if (!string.IsNullOrEmpty(move.LandingLag)) statistics += "**Landinglag: **" + move.LandingLag + "\n";
                    builder.AddInlineField("Statistics",
                    statistics);
                }
                await ReplyAsync("", embed: builder.Build());
            }
        }

        [Group("Melee")]
        public class Melee : ModuleBase
        {
            [Command("")]
            public async Task GetCharacter([Remainder] string name)
            {
                var characters = new List<string>()
                {
                    "Bowser",
                    "Captain Falcon",
                    "Donkey Kong",
                    "Dr. Mario",
                    "Falco",
                    "Fox",
                    "Ganondorf",
                    "Ice Climbers",
                    "Jugglypuff",
                    "Kirby",
                    "Link",
                    "Luigi",
                    "Mario",
                    "Marth",
                    "Mewtwo",
                    "Mr. Game And Watch",
                    "Ness",
                    "Peach",
                    "Pichu",
                    "Pikachu",
                    "Roy",
                    "Samus",
                    "Sheik",
                    "Yoshi",
                    "Young Link",
                    "Zelda"
                };
                var id = characters.IndexOf(name) + 1;
                var character = MeleeHandler.RequestHandler.GetCharacter(id);
                var builder = Builders.BaseBuilder("", "", Color.Teal,
                    new EmbedAuthorBuilder().WithName(character.name).WithIconUrl($"http://smashlounge.com/img/pixel/{character.name.Replace(" ","")}HeadSSBM.png"), "");
                builder.AddField("Description", character.guide);
                builder.AddField("Stats", $"**Tier: **{character.tierdata}\n" +
                                          $"**Weight: **{character.weight}\n" +
                                          $"**Fallspeed: **{character.fallspeed}\n" +
                                          $"**Can Walljump: **{Convert.ToBoolean(Int32.Parse(character.walljump))}");
                await ReplyAsync("", embed: builder.Build());
            }
        }
    }
}
