﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.EmbedBuilder;
using KugorganeHammerHandler;

namespace DiscordBot.Modules
{
    [Group("Smash")]
    public class SmashModule : ModuleBase
    {
        [Group("WiiU")]
        public class WiiU : ModuleBase
        {
            [Command("")]
            public async Task GetCharacter([Remainder] string name)
            {
                var character = RequestHandler.GetCharacterName(name);
                Discord.EmbedBuilder builder = Builders.BaseBuilder("", "", Color.DarkTeal,
                    new EmbedAuthorBuilder().WithName("KuroganeHammer Result:").WithUrl("http://kuroganehammer.com"), character.ThumbnailURL);
                builder.WithImageUrl(character.MainImageURL);
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
                string movementinfo = "";
                //foreach (var move in movement.Attributes)
                for(int i = 0; i< movement.Attributes.Count/3; i++)
                {
                    var info1 = "**" + movement.Attributes[i].Name + ": **" +
                                movement.Attributes[i].Value.Replace("frames", " Frames");
                    var info2 = "**" + movement.Attributes[i+1].Name + ": **" +
                                movement.Attributes[i].Value.Replace("frames", " Frames");
                    var info3 = "**" + movement.Attributes[i+2].Name + ": **" +
                                movement.Attributes[i].Value.Replace("frames", " Frames");
                    if (movement.Attributes[i].Name.Contains(" "))
                    {
                       while (info1.Length < 28)
                        {
                            info1 += " ";
                        }
                    }
                    else
                    {
                        while (info1.Length < 30)
                        {
                            info1 = info1 + " ";
                        }
                    }
                    if (info1.Contains(".")) info1 += " ";

                    while (info2.Length <30 )
                    {
                        info2 += " ";
                    }
                    movementinfo += $"{info1}{info2}{info3 + "\n"}";
                    //if ((move.Name == "Crawl" || move.Name == "Tether") && move.Value == "No");
                    //else
                    //{
                    //    move.Value = move.Value.Replace("frames", " Frames");
                    //    movementinfo += "**" + move.Name + ": **" + move.Value + "\n";
                    //}

                }
                builder.AddField(new EmbedFieldBuilder().WithName("Movement").WithValue(movementinfo));
                await ReplyAsync("", embed: builder.Build());
            }
        }
    }
}
